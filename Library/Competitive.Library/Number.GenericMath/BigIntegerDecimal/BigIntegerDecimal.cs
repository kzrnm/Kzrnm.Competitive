// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 10進多倍長整数
    /// </summary>
    public readonly partial struct BigIntegerDecimal
        : ISpanFormattable,
          IComparable<BigIntegerDecimal>,
          IEquatable<BigIntegerDecimal>,
          INumber<BigIntegerDecimal>,
          ISignedNumber<BigIntegerDecimal>
    {
        /*
         * Original is BigInteger
         *
         * Copyright (c) .NET Foundation and Contributors
         * Released under the MIT license
         * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
         */
        internal const string LISENCE = @"
Original is BigInteger

Copyright (c) .NET Foundation and Contributors
Released under the MIT license
https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
";
        /// <summary>
        /// 基数。実際には 10^9 進数ということ
        /// </summary>
        internal const int BASE = 1000000000;
        internal const int LOG_B = 9;
        internal readonly int sign; // Do not rename (binary serialization)
        internal readonly uint[] digits; // Do not rename (binary serialization)
        private static readonly BigIntegerDecimal s_bnOneInt = new BigIntegerDecimal(1);
        private static readonly BigIntegerDecimal s_bnZeroInt = new BigIntegerDecimal(0);
        private static readonly BigIntegerDecimal s_bnMinusOneInt = new BigIntegerDecimal(-1);

        public BigIntegerDecimal(long value)
            : this(new RefValue(stackalloc uint[3], value)) { }
        public BigIntegerDecimal(ulong value)
            : this(new RefValue(stackalloc uint[3], value)) { }
        private BigIntegerDecimal(ReadOnlySpan<uint> value, bool negative)
            : this(new RefValue(value, negative)) { }
        private BigIntegerDecimal(RefValue v)
        {
            sign = v.sign;
            digits = v.digits.IsEmpty ? null : v.digits.ToArray();
            AssertValid();
        }
        private BigIntegerDecimal(int sign, uint[] digits)
        {
            this.sign = sign;
            this.digits = digits;
            AssertValid();
        }


        public static BigIntegerDecimal Zero => s_bnZeroInt;
        public static BigIntegerDecimal One => s_bnOneInt;
        public static BigIntegerDecimal MinusOne => s_bnMinusOneInt;
        static BigIntegerDecimal ISignedNumber<BigIntegerDecimal>.NegativeOne => MinusOne;
        static int MaxLength => Array.MaxLength / sizeof(uint);

        public bool IsZero { get { AssertValid(); return sign == 0; } }
        public bool IsOne { get { AssertValid(); return sign == 1 && digits == null; } }
        public bool IsEven { get { AssertValid(); return digits == null ? (sign & 1) == 0 : (digits[0] & 1) == 0; } }
        public int Sign { get { AssertValid(); return sign; } }

        public override int GetHashCode()
        {
            AssertValid();

            if (digits is null)
                return sign;

            HashCode hash = default;
            hash.AddBytes(MemoryMarshal.AsBytes(digits.AsSpan()));
            hash.Add(sign);
            return hash.ToHashCode();
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            AssertValid();

            if (obj is BigIntegerDecimal other)
                return Equals(other);

            return RefValue.TryConvert(stackalloc uint[3], obj, out var v) && Equals(v);
        }

        [凾(256)]
        public bool Equals(BigIntegerDecimal other) => Equals(new RefValue(other));
        [凾(256)]
        bool Equals(RefValue other)
        {
            AssertValid();
            other.AssertValid();

            if (sign != other.sign)
                return false;
            if (digits == other.digits)
                // sign == other.sign && digits == null && other.digits == null
                return true;

            if (digits == null || other.digits == null)
                return false;
            int cu = digits.Length;
            if (cu != other.digits.Length)
                return false;
            int cuDiff = GetDiffLength(digits, other.digits, cu);
            return cuDiff == 0;
        }

        [凾(256)]
        public int CompareTo(BigIntegerDecimal other) => CompareTo(new RefValue(other));
        [凾(256)]
        int CompareTo(RefValue other)
        {
            AssertValid();
            other.AssertValid();

            if ((sign ^ other.sign) < 0)
            {
                // Different signs, so the comparison is easy.
                return sign < 0 ? -1 : +1;
            }

            // Same signs
            if (digits == null)
            {
                if (other.digits == null)
                    return sign < other.sign ? -1 : sign > other.sign ? +1 : 0;
                return -other.sign;
            }
            int cuThis, cuOther;
            if (other.digits == null || (cuThis = digits.Length) > (cuOther = other.digits.Length))
                return sign;
            if (cuThis < cuOther)
                return -sign;

            int cuDiff = GetDiffLength(digits, other.digits, cuThis);
            if (cuDiff == 0)
                return 0;
            return digits[cuDiff - 1] < other.digits[cuDiff - 1] ? -sign : sign;
        }

        [凾(256)]
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is BigIntegerDecimal other)
                return CompareTo(other);
            if (RefValue.TryConvert(stackalloc uint[3], obj, out var v))
                return CompareTo(v);
            throw new ArgumentException("MustBeBigInt", nameof(obj));
        }

        [凾(256)]
        internal static int GetDiffLength(ReadOnlySpan<uint> rgu1, ReadOnlySpan<uint> rgu2, int cu)
        {
            for (int iv = cu; --iv >= 0;)
            {
                if (rgu1[iv] != rgu2[iv])
                    return iv + 1;
            }
            return 0;
        }


        public static BigIntegerDecimal Parse(string value)
            => Parse(value.AsSpan());
        public static BigIntegerDecimal Parse(string value, IFormatProvider provider)
            => Parse(value.AsSpan());
        public static BigIntegerDecimal Parse(ReadOnlySpan<char> value, IFormatProvider provider)
            => Parse(value);
        public static BigIntegerDecimal Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider)
            => Parse(s);
        public static BigIntegerDecimal Parse(string s, NumberStyles style, IFormatProvider provider)
            => Parse(s);
        public static BigIntegerDecimal Parse(ReadOnlySpan<char> value)
        {
            if (TryParseDecimal(value, out var result)) return result;
            return ThrowFormatException();
            static BigIntegerDecimal ThrowFormatException() => throw new FormatException();
        }


        public static bool TryParse([NotNullWhen(true)] string value, out BigIntegerDecimal result)
            => TryParseDecimal(value, out result);
        public static bool TryParse(ReadOnlySpan<char> value, IFormatProvider provider, out BigIntegerDecimal result)
            => TryParseDecimal(value, out result);
        public static bool TryParse(ReadOnlySpan<char> value, out BigIntegerDecimal result)
            => TryParseDecimal(value, out result);

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out BigIntegerDecimal result)
            => TryParseDecimal(s, out result);
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out BigIntegerDecimal result)
            => TryParseDecimal(s, out result);

        public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out BigIntegerDecimal result)
            => TryParseDecimal(s, out result);

        static bool TryParseDecimal(ReadOnlySpan<char> value, out BigIntegerDecimal result)
        {
            value = value.Trim();

            if (value.Length == 0)
            {
                result = default;
                return false;
            }

            bool negative = false;
            if (value[0] == '-')
            {
                if (value.Length == 1)
                {
                    result = default;
                    return false;
                }
                value = value[1..];
                negative = true;
            }

            var digits = ArrayPool<uint>.Shared.Rent(value.Length / (LOG_B - 1) + 5);
            try
            {
                int bi = 0;
                for (; value.Length >= LOG_B; value = value[..^LOG_B])
                {
                    if (!uint.TryParse(value[^LOG_B..], out uint s) || s >= BASE)
                    {
                        result = default;
                        return false;
                    }
                    digits[bi++] = s;
                }
                if (value.Length > 0)
                {
                    if (!uint.TryParse(value, out uint s) || s >= BASE)
                    {
                        result = default;
                        return false;
                    }
                    digits[bi++] = s;
                }
                result = new BigIntegerDecimal(digits.AsSpan(0, bi), negative);
                return true;
            }
            finally
            {
                ArrayPool<uint>.Shared.Return(digits);
            }
        }

        int MaxCharLength()
        {
            if (digits == null)
                return LOG_B + 1;

            return digits.Length * LOG_B + 1;
        }

        public override string ToString()
        {
            var buffer = ArrayPool<char>.Shared.Rent(MaxCharLength());
            try
            {
                bool result = TryFormat(buffer, out var charsWritten);
                Debug.Assert(result);
                return new string(buffer, 0, charsWritten);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }
        public string ToString(string format, IFormatProvider provider) => ToString();
        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            if (digits == null)
                return sign.TryFormat(destination, out charsWritten, format, provider);

            charsWritten = 0;
            if (sign < 0)
            {
                if (destination.IsEmpty)
                    return false;
                destination[0] = '-';
                destination = destination[1..];
                ++charsWritten;
            }

            {
                if (!digits[^1].TryFormat(destination, out var written))
                    return false;
                charsWritten += written;
                destination = destination[written..];
            }
            for (var remains = digits.AsSpan()[..^1]; remains.Length > 0; remains = remains[..^1])
            {
                if (!remains[^1].TryFormat(destination, out var written, "D9", provider))
                    return false;
                charsWritten += written;
                destination = destination[written..];
            }
            return true;
        }


        public static BigIntegerDecimal Abs(BigIntegerDecimal value)
        {
            return (value >= Zero) ? value : -value;
        }
        public static double Log(BigIntegerDecimal value)
        {
            return Log(value, Math.E);
        }

        public static double Log(BigIntegerDecimal value, double baseValue)
        {
            if (baseValue == 1.0D)
                return double.NaN;
            if (baseValue == double.PositiveInfinity)
                return value.IsOne ? 0.0D : double.NaN;
            if (baseValue == 0.0D && !value.IsOne)
                return double.NaN;
            if (value.digits == null)
                return Math.Log(value.sign, baseValue);

            return Log10(value) / Math.Log(baseValue, 10);
        }

        public static double Log10(BigIntegerDecimal value)
        {
            if (value.sign < 0)
                return double.NaN;
            if (value.digits == null)
                return Math.Log10(value.sign);

            Debug.Assert(value.digits.Length > 1);
            if (value.digits.Length == 2)
                return Math.Log10(value.digits[0] + 1e9 * value.digits[1]);

            return Math.Log10(value.digits[0] + 1e9 * (value.digits[1] + 1e9 * value.digits[2])) + LOG_B * (value.digits.Length - 3);
        }

        public static BigIntegerDecimal Max(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            if (left.CompareTo(right) < 0)
                return right;
            return left;
        }

        public static BigIntegerDecimal Min(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            if (left.CompareTo(right) <= 0)
                return left;
            return right;
        }

        public static bool operator <(BigIntegerDecimal left, BigIntegerDecimal right)
            => left.CompareTo(right) < 0;


        public static bool operator <=(BigIntegerDecimal left, BigIntegerDecimal right)
            => left.CompareTo(right) <= 0;


        public static bool operator >(BigIntegerDecimal left, BigIntegerDecimal right)
            => left.CompareTo(right) > 0;

        public static bool operator >=(BigIntegerDecimal left, BigIntegerDecimal right)
            => left.CompareTo(right) >= 0;


        public static bool operator ==(BigIntegerDecimal left, BigIntegerDecimal right)
            => left.Equals(right);


        public static bool operator !=(BigIntegerDecimal left, BigIntegerDecimal right)
            => !left.Equals(right);
        public static bool operator <(BigIntegerDecimal left, long right)
            => left.CompareTo(right) < 0;


        public static bool operator <=(BigIntegerDecimal left, long right)
            => left.CompareTo(right) <= 0;


        public static bool operator >(BigIntegerDecimal left, long right)
            => left.CompareTo(right) > 0;


        public static bool operator >=(BigIntegerDecimal left, long right)
            => left.CompareTo(right) >= 0;


        public static bool operator ==(BigIntegerDecimal left, long right)
            => left.Equals(right);


        public static bool operator !=(BigIntegerDecimal left, long right)
            => !left.Equals(right);


        public static bool operator <(long left, BigIntegerDecimal right)
            => right.CompareTo(left) > 0;


        public static bool operator <=(long left, BigIntegerDecimal right)
            => right.CompareTo(left) >= 0;


        public static bool operator >(long left, BigIntegerDecimal right)
            => right.CompareTo(left) < 0;


        public static bool operator >=(long left, BigIntegerDecimal right)
            => right.CompareTo(left) <= 0;


        public static bool operator ==(long left, BigIntegerDecimal right)
            => right.Equals(left);


        public static bool operator !=(long left, BigIntegerDecimal right)
            => !right.Equals(left);


        public static bool operator <(BigIntegerDecimal left, ulong right)
            => left.CompareTo(right) < 0;


        public static bool operator <=(BigIntegerDecimal left, ulong right)
            => left.CompareTo(right) <= 0;


        public static bool operator >(BigIntegerDecimal left, ulong right)
            => left.CompareTo(right) > 0;


        public static bool operator >=(BigIntegerDecimal left, ulong right)
            => left.CompareTo(right) >= 0;


        public static bool operator ==(BigIntegerDecimal left, ulong right)
            => left.Equals(right);


        public static bool operator !=(BigIntegerDecimal left, ulong right)
            => !left.Equals(right);


        public static bool operator <(ulong left, BigIntegerDecimal right)
            => right.CompareTo(left) > 0;


        public static bool operator <=(ulong left, BigIntegerDecimal right)
            => right.CompareTo(left) >= 0;


        public static bool operator >(ulong left, BigIntegerDecimal right)
            => right.CompareTo(left) < 0;


        public static bool operator >=(ulong left, BigIntegerDecimal right)
            => right.CompareTo(left) <= 0;


        public static bool operator ==(ulong left, BigIntegerDecimal right)
            => right.Equals(left);


        public static bool operator !=(ulong left, BigIntegerDecimal right)
            => !right.Equals(left);


        [Conditional("DEBUG")]
        private void AssertValid()
        {
            if (digits != null)
                AssertValid(digits, sign);
            else
            {
                Debug.Assert(-BASE < sign && sign < BASE);
            }
        }
        [Conditional("DEBUG")]
        static void AssertValid(ReadOnlySpan<uint> digits, int sign)
        {
            if (!digits.IsEmpty)
            {
                // sign must be +1 or -1 when digits is non-null
                Debug.Assert(sign == 1 || sign == -1);
                // digits must contain at least 1 element or be null
                Debug.Assert(digits.Length > 1);
                // Wasted space: leading zeros could have been truncated
                Debug.Assert(digits[^1] != 0);
            }
            else
            {
                Debug.Assert(-BASE < sign && sign < BASE);
            }
        }
        static void ThrowOverflowException() => throw new OverflowException();
        static void ThrowNotSupportedException() => throw new NotSupportedException();
    }
}