using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Kzrnm.Numerics.Logic;
using System.Runtime.InteropServices;
using BigInteger = Kzrnm.Numerics.BigInteger;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{


    // https://github.com/dotnet/runtime/blob/808c523fd2127abb53292ff199cb8b01752d0b54/src/libraries/System.Private.CoreLib/src/System/UInt128.cs
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.
    /// <summary>
    /// 符号なし 256 bit 整数
    /// <see cref="UInt128"/> の実装をもとにしている。
    /// </summary>
    public readonly partial struct UInt256 : IBinaryInteger<UInt256>, IEquatable<UInt256>, IMinMaxValue<UInt256>
    {
        /*
         * Original is UInt128
         *
         * Copyright (c) .NET Foundation and Contributors
         * Released under the MIT license
         * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
         */
        internal const string LISENCE = @"
Original is UInt128

Copyright (c) .NET Foundation and Contributors
Released under the MIT license
https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
";

#if BIGENDIAN
        private readonly ulong v3;
        private readonly ulong v2;
        private readonly ulong v1;
        private readonly ulong v0;
#else
        private readonly ulong v0;
        private readonly ulong v1;
        private readonly ulong v2;
        private readonly ulong v3;
#endif

        internal const int Size = 32; // bytes
        internal const int ULongCount = 4;

        public UInt256(ulong v3, ulong v2, ulong v1, ulong v0)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        [凾(256)] internal static Span<ulong> AsULongSpan(in UInt256 v) => MemoryMarshal.Cast<UInt256, ulong>(MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in v), 1));
        [凾(256)] internal static Span<uint> AsUIntSpan(in UInt256 v) => MemoryMarshal.Cast<UInt256, uint>(MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in v), 1));
        [凾(256)] internal static Span<byte> AsByteSpan(in UInt256 v) => MemoryMarshal.AsBytes(AsULongSpan(v));

        static readonly UInt256 _one = new(0, 0, 0, 1);
        public static UInt256 One => _one;
        public static int Radix => 2;
        public static UInt256 Zero => default;
        public static UInt256 AdditiveIdentity => Zero;
        public static UInt256 MultiplicativeIdentity => _one;
        public static UInt256 MaxValue => new(~0ul, ~0ul, ~0ul, ~0ul);
        public static UInt256 MinValue => default;

        [凾(256)]
        public static UInt256 Abs(UInt256 value) => value;
        static bool INumberBase<UInt256>.IsCanonical(UInt256 value) => true;
        static bool INumberBase<UInt256>.IsComplexNumber(UInt256 value) => false;
        public static bool IsEvenInteger(UInt256 value) => (value.v0 & 1) == 0;
        static bool INumberBase<UInt256>.IsFinite(UInt256 value) => true;
        static bool INumberBase<UInt256>.IsImaginaryNumber(UInt256 value) => false;
        static bool INumberBase<UInt256>.IsInfinity(UInt256 value) => false;
        static bool INumberBase<UInt256>.IsInteger(UInt256 value) => true;
        static bool INumberBase<UInt256>.IsNaN(UInt256 value) => false;
        static bool INumberBase<UInt256>.IsNegative(UInt256 value) => false;
        static bool INumberBase<UInt256>.IsNegativeInfinity(UInt256 value) => false;
        static bool INumberBase<UInt256>.IsNormal(UInt256 value) => value != default;
        public static bool IsOddInteger(UInt256 value) => (value.v0 & 1) != 0;
        static bool INumberBase<UInt256>.IsPositive(UInt256 value) => true;
        static bool INumberBase<UInt256>.IsPositiveInfinity(UInt256 value) => false;
        static bool INumberBase<UInt256>.IsRealNumber(UInt256 value) => true;
        static bool INumberBase<UInt256>.IsSubnormal(UInt256 value) => false;
        static bool INumberBase<UInt256>.IsZero(UInt256 value) => (value == default);
        static UInt256 INumberBase<UInt256>.MaxMagnitude(UInt256 x, UInt256 y) => Max(x, y);
        static UInt256 INumberBase<UInt256>.MaxMagnitudeNumber(UInt256 x, UInt256 y) => Max(x, y);
        static UInt256 INumberBase<UInt256>.MinMagnitude(UInt256 x, UInt256 y) => Min(x, y);
        static UInt256 INumberBase<UInt256>.MinMagnitudeNumber(UInt256 x, UInt256 y) => Min(x, y);
        static UInt256 INumber<UInt256>.CopySign(UInt256 value, UInt256 sign) => value;
        public static UInt256 Max(UInt256 x, UInt256 y) => (x >= y) ? x : y;
        static UInt256 INumber<UInt256>.MaxNumber(UInt256 x, UInt256 y) => Max(x, y);
        public static UInt256 Min(UInt256 x, UInt256 y) => (x <= y) ? x : y;
        static UInt256 INumber<UInt256>.MinNumber(UInt256 x, UInt256 y) => Min(x, y);
        public static int Sign(UInt256 value) => (value == default) ? 0 : 1;

        static bool IBinaryInteger<UInt256>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
        {
            UInt256 result = default;

            if (source.Length != 0)
            {
                if (!isUnsigned && sbyte.IsNegative((sbyte)source[0]))
                {
                    // When we are signed and the sign bit is set, we are negative and therefore
                    // definitely out of range

                    value = result;
                    return false;
                }

                if ((source.Length > Size) && (source[..^Size].IndexOfAnyExcept((byte)0x00) >= 0))
                {
                    // When we have any non-zero leading data, we are a large positive and therefore
                    // definitely out of range

                    value = result;
                    return false;
                }

                ref byte sourceRef = ref MemoryMarshal.GetReference(source);

                if (source.Length >= Size)
                {
                    sourceRef = ref Unsafe.Add(ref sourceRef, source.Length - Size);

                    // We have at least 16 bytes, so just read the ones we need directly
                    result = Unsafe.ReadUnaligned<UInt256>(ref sourceRef);

                    if (BitConverter.IsLittleEndian)
                    {
                        result = new UInt256(
                            BinaryPrimitives.ReverseEndianness(result.v0),
                            BinaryPrimitives.ReverseEndianness(result.v1),
                            BinaryPrimitives.ReverseEndianness(result.v2),
                            BinaryPrimitives.ReverseEndianness(result.v3)
                        );
                    }
                }
                else
                {
                    // We have between 1 and 15 bytes, so construct the relevant value directly
                    // since the data is in Big Endian format, we can just read the bytes and
                    // shift left by 8-bits for each subsequent part

                    for (int i = 0; i < source.Length; i++)
                    {
                        result <<= 8;
                        result |= Unsafe.Add(ref sourceRef, i);
                    }
                }
            }

            value = result;
            return true;
        }

        static bool IBinaryInteger<UInt256>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
        {
            UInt256 result = default;

            if (source.Length != 0)
            {
                if (!isUnsigned && sbyte.IsNegative((sbyte)source[^1]))
                {
                    // When we are signed and the sign bit is set, we are negative and therefore
                    // definitely out of range

                    value = result;
                    return false;
                }

                if ((source.Length > Size) && (source[Size..].IndexOfAnyExcept((byte)0x00) >= 0))
                {
                    // When we have any non-zero leading data, we are a large positive and therefore
                    // definitely out of range

                    value = result;
                    return false;
                }

                ref byte sourceRef = ref MemoryMarshal.GetReference(source);

                if (source.Length >= Size)
                {
                    // We have at least 16 bytes, so just read the ones we need directly
                    result = Unsafe.ReadUnaligned<UInt256>(ref sourceRef);

                    if (!BitConverter.IsLittleEndian)
                    {
                        result = new UInt256(
                            BinaryPrimitives.ReverseEndianness(result.v0),
                            BinaryPrimitives.ReverseEndianness(result.v1),
                            BinaryPrimitives.ReverseEndianness(result.v2),
                            BinaryPrimitives.ReverseEndianness(result.v3)
                        );
                    }
                }
                else
                {
                    // We have between 1 and 15 bytes, so construct the relevant value directly
                    // since the data is in Little Endian format, we can just read the bytes and
                    // shift left by 8-bits for each subsequent part, then reverse endianness to
                    // ensure the order is correct. This is more efficient than iterating in reverse
                    // due to current JIT limitations

                    for (int i = 0; i < source.Length; i++)
                    {
                        UInt256 part = Unsafe.Add(ref sourceRef, i);
                        part <<= (i * 8);
                        result |= part;
                    }
                }
            }

            value = result;
            return true;
        }

        bool IBinaryInteger<UInt256>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
        {
            if (destination.Length >= Size)
            {
                var v = AsULongSpan(this);
                ref byte address = ref MemoryMarshal.GetReference(destination);

                for (int i = 0; i < v.Length; i++)
                {
                    var vv = BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(v[i]) : v[i];
                    Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, i * sizeof(ulong)), vv);
                }

                bytesWritten = Size;
                return true;
            }
            else
            {
                bytesWritten = 0;
                return false;
            }
        }

        bool IBinaryInteger<UInt256>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
        {
            if (destination.Length >= Size)
            {
                var v = AsULongSpan(this);
                ref byte address = ref MemoryMarshal.GetReference(destination);

                for (int i = 0; i < v.Length; i++)
                {
                    var vv = !BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(v[i]) : v[i];
                    Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, i * sizeof(ulong)), vv);
                }

                bytesWritten = Size;
                return true;
            }
            else
            {
                bytesWritten = 0;
                return false;
            }
        }


        [凾(256)]
        public static UInt256 operator +(UInt256 value) => value;

        [凾(256)]
        public static UInt256 operator +(UInt256 left, UInt256 right)
        {
            var v0 = left.v0 + right.v0;
            var carry = (v0 < left.v0) ? 1ul : 0ul;

            var v1 = left.v1 + right.v1 + carry;
            carry = (v1 < left.v1 || carry != 0 && right.v1 == ulong.MaxValue) ? 1ul : 0ul;

            var v2 = left.v2 + right.v2 + carry;
            carry = (v2 < left.v2 || carry != 0 && right.v2 == ulong.MaxValue) ? 1ul : 0ul;

            var v3 = left.v3 + right.v3 + carry;
            return new UInt256(v3, v2, v1, v0);
        }
        [凾(256)]
        public static UInt256 operator ++(UInt256 value) => value + One;


        [凾(256)]
        public static UInt256 operator -(UInt256 value) => Zero - value;

        [凾(256)]
        public static UInt256 operator -(UInt256 left, UInt256 right)
        {
            // For unsigned subtract, we can detect overflow by checking `(x - y) > x`
            // This gives us the borrow to subtract from upper to compute the correct result
            var v0 = left.v0 - right.v0;
            var borrow = (v0 > left.v0) ? 1ul : 0ul;

            var v1 = left.v1 - right.v1 - borrow;
            borrow = (v1 > left.v1 || borrow != 0 && right.v1 == ulong.MaxValue) ? 1ul : 0ul;

            var v2 = left.v2 - right.v2 - borrow;
            borrow = (v2 > left.v2 || borrow != 0 && right.v2 == ulong.MaxValue) ? 1ul : 0ul;

            var v3 = left.v3 - right.v3 - borrow;
            return new UInt256(v3, v2, v1, v0);
        }
        [凾(256)]
        public static UInt256 operator --(UInt256 value) => value - One;

        [凾(256)]
        public static UInt256 operator *(UInt256 left, UInt256 right)
        {
            var lb = AsUIntSpan(left).TrimEnd(0u);
            var rb = AsUIntSpan(right).TrimEnd(0u);
            Span<uint> bits = stackalloc uint[16];
            bits.Clear();
            if (lb.Length < rb.Length)
                BigIntegerCalculator.Multiply(rb, lb, bits[..(lb.Length + rb.Length)]);
            else
                BigIntegerCalculator.Multiply(lb, rb, bits[..(lb.Length + rb.Length)]);
            UInt256 rt = default;
            bits[..8].CopyTo(AsUIntSpan(rt));
            return rt;
        }

        [凾(256)]
        public static UInt256 operator /(UInt256 left, UInt256 right)
        {
            return DivRem(left, right).Quotient;
        }

        [凾(256)]
        public static UInt256 operator %(UInt256 left, UInt256 right)
        {
            return DivRem(left, right).Remainder;
        }

        [凾(256)]
        public static (UInt256 Quotient, UInt256 Remainder) DivRem(UInt256 left, UInt256 right)
        {
            static void ThrowDivideByZeroException() => throw new DivideByZeroException();
            var lb = AsUIntSpan(left).TrimEnd(0u);
            var rb = AsUIntSpan(right).TrimEnd(0u);
            if (rb.Length == 0)
                ThrowDivideByZeroException();
            if (lb.Length < rb.Length)
                return (Zero, left);
            Span<uint> quo = stackalloc uint[8];
            Span<uint> rem = stackalloc uint[8];
            quo.Clear();
            rem.Clear();
            BigIntegerCalculator.Divide(lb, rb, quo[..(lb.Length - rb.Length + 1)], rem[..lb.Length]);

            UInt256 q = default;
            UInt256 r = default;
            quo.CopyTo(AsUIntSpan(q));
            rem.CopyTo(AsUIntSpan(r));
            return (q, r);
        }

        [凾(256)]
        public static UInt256 operator ~(UInt256 value)
            => new UInt256(~value.v3, ~value.v2, ~value.v1, ~value.v0);

        [凾(256)]
        public static UInt256 operator &(UInt256 left, UInt256 right)
            => new UInt256(left.v3 & right.v3, left.v2 & right.v2, left.v1 & right.v1, left.v0 & right.v0);

        [凾(256)]
        public static UInt256 operator |(UInt256 left, UInt256 right)
            => new UInt256(left.v3 | right.v3, left.v2 | right.v2, left.v1 | right.v1, left.v0 | right.v0);

        [凾(256)]
        public static UInt256 operator ^(UInt256 left, UInt256 right)
            => new UInt256(left.v3 ^ right.v3, left.v2 ^ right.v2, left.v1 ^ right.v1, left.v0 ^ right.v0);

        [凾(256)]
        public static UInt256 operator <<(UInt256 value, int shiftAmount)
        {
            // C# automatically masks the shift amount for UInt64 to be 0x3F. So we
            // need to specially handle things if the 7th bit is set.

            shiftAmount &= Size * 8 - 1;
            if (shiftAmount == 0) return value;

            Span<ulong> v = stackalloc ulong[ULongCount];
            AsULongSpan(value).CopyTo(v);

            var lowerShift = shiftAmount & 63;
            var remain = 64 - lowerShift;
            if (lowerShift != 0)
            {
                for (int i = v.Length - 2; i >= 0; i--)
                    v[i + 1] = (v[i + 1] << lowerShift) | (v[i] >> remain);
                v[0] <<= lowerShift;
            }
            var upperShift = shiftAmount >> 6;
            for (int i = v.Length - upperShift - 1; i >= 0; i--)
                v[i + upperShift] = v[i];
            v[..upperShift].Clear();

            return new UInt256(v[3], v[2], v[1], v[0]);
        }

        [凾(256)]
        public static UInt256 operator >>(UInt256 value, int shiftAmount)
            => value >>> shiftAmount;

        [凾(256)]
        public static UInt256 operator >>>(UInt256 value, int shiftAmount)
        {
            // C# automatically masks the shift amount for UInt64 to be 0x3F. So we
            // need to specially handle things if the 7th bit is set.

            shiftAmount &= Size * 8 - 1;
            if (shiftAmount == 0) return value;

            Span<ulong> v = stackalloc ulong[ULongCount];
            AsULongSpan(value).CopyTo(v);

            var lowerShift = shiftAmount & 63;
            var remain = 64 - lowerShift;
            if (lowerShift != 0)
            {
                for (int i = 0; i + 1 < v.Length; i++)
                    v[i] = (v[i + 1] << remain) | (v[i] >> lowerShift);
                v[^1] >>= lowerShift;
            }
            var upperShift = shiftAmount >> 6;
            if (upperShift > 0)
            {
                for (int i = 0; i + upperShift < v.Length; i++)
                    v[i] = v[i + upperShift];
                v[^upperShift..].Clear();
            }

            return new UInt256(v[3], v[2], v[1], v[0]);
        }

        [凾(256)]
        public static bool operator ==(UInt256 left, UInt256 right)
                => left.v0 == right.v0
                && left.v1 == right.v1
                && left.v2 == right.v2
                && left.v3 == right.v3;

        [凾(256)]
        public static bool operator !=(UInt256 left, UInt256 right)
                => left.v0 != right.v0
                || left.v1 != right.v1
                || left.v2 != right.v2
                || left.v3 != right.v3;

        [凾(256)]
        public static bool operator <(UInt256 left, UInt256 right)
        {
            if (left.v3 < right.v3) return true;
            if (left.v3 > right.v3) return false;
            if (left.v2 < right.v2) return true;
            if (left.v2 > right.v2) return false;
            if (left.v1 < right.v1) return true;
            if (left.v1 > right.v1) return false;
            return left.v0 < right.v0;
        }

        [凾(256)]
        public static bool operator <=(UInt256 left, UInt256 right)
        {
            if (left.v3 < right.v3) return true;
            if (left.v3 > right.v3) return false;
            if (left.v2 < right.v2) return true;
            if (left.v2 > right.v2) return false;
            if (left.v1 < right.v1) return true;
            if (left.v1 > right.v1) return false;
            return left.v0 <= right.v0;
        }

        [凾(256)]
        public static bool operator >(UInt256 left, UInt256 right)
        {
            if (left.v3 > right.v3) return true;
            if (left.v3 < right.v3) return false;
            if (left.v2 > right.v2) return true;
            if (left.v2 < right.v2) return false;
            if (left.v1 > right.v1) return true;
            if (left.v1 < right.v1) return false;
            return left.v0 > right.v0;
        }

        [凾(256)]
        public static bool operator >=(UInt256 left, UInt256 right)
        {
            if (left.v3 > right.v3) return true;
            if (left.v3 < right.v3) return false;
            if (left.v2 > right.v2) return true;
            if (left.v2 < right.v2) return false;
            if (left.v1 > right.v1) return true;
            if (left.v1 < right.v1) return false;
            return left.v0 >= right.v0;
        }

        public override bool Equals(object obj) => (obj is UInt256 other) && Equals(other);

        public bool Equals(UInt256 other) => this == other;

        public override int GetHashCode() => HashCode.Combine(v3, v2, v1, v0);

        public int GetByteCount() => Size;
        public int GetShortestBitLength()
            => (Size * 8) - LeadingZeroCountInt(this);

        static int LeadingZeroCountInt(UInt256 value)
        {
            var r = 0;
            var s = AsULongSpan(value);
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] == 0)
                    r += 64;
                else
                {
                    r += BitOperations.LeadingZeroCount(s[i]);
                    break;
                }
            }
            return r;
        }
        public static UInt256 LeadingZeroCount(UInt256 value)
        {
            return (UInt256)LeadingZeroCountInt(value);
        }

        public static UInt256 PopCount(UInt256 value)
        {
            var r = 0;
            foreach (var s in AsULongSpan(value))
                r += BitOperations.PopCount(s);
            return (UInt256)r;
        }

        public static UInt256 TrailingZeroCount(UInt256 value)
        {
            var r = 0;
            var s = AsULongSpan(value);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == 0)
                    r += 64;
                else
                {
                    r += BitOperations.TrailingZeroCount(s[i]);
                    break;
                }
            }
            return (UInt256)r;
        }

        static UInt256 IBinaryNumber<UInt256>.AllBitsSet => MaxValue;


        public static bool IsPow2(UInt256 value) => PopCount(value) == 1U;

        public static UInt256 Log2(UInt256 value)
        {
            var r = 0;
            var s = AsULongSpan(value);
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] > 0)
                {
                    r = 64 * i + BitOperations.Log2(s[i]);
                    break;
                }
            }
            return (UInt256)r;
        }

        public int CompareTo(object value)
        {
            if (value is UInt256 other)
            {
                return CompareTo(other);
            }
            else if (value is null)
            {
                return 1;
            }
            else
            {
                throw new ArgumentException("need UInt256", nameof(value));
            }
        }

        public int CompareTo(UInt256 value)
        {
            if (this < value)
                return -1;
            else if (this > value)
                return 1;
            else
                return 0;
        }


        public static UInt256 Parse(string s)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static UInt256 Parse(string s, NumberStyles style)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static UInt256 Parse(string s, IFormatProvider provider)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static UInt256 Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s.AsSpan(), style, NumberFormatInfo.GetInstance(provider));
        }
        public static UInt256 Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, provider);
        }
        public static UInt256 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
        {
            if (TryParse(s, style, NumberFormatInfo.GetInstance(provider), out var result))
                return result;
            throw new FormatException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out UInt256 result)
        {
            return TryParse(s, NumberStyles.Integer, provider, out result);
        }
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, [MaybeNullWhen(false)] out UInt256 result)
        {
            return TryParse(s, NumberStyles.Integer, provider, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string s, out UInt256 result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, out UInt256 result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, out UInt256 result)
        {
            return TryParse(s.AsSpan(), style, NumberFormatInfo.GetInstance(provider), out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out UInt256 result)
        {
            // https://github.com/dotnet/runtime/issues/83619
            // style.HasFlag(NumberStyles.AllowBinarySpecifier);

            if (BigInteger.TryParse(s, style, provider, out var bi))
            {
                result = (UInt256)bi;
                return true;
            }
            result = default;
            return false;
        }


        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            return ((BigInteger)this).TryFormat(destination, out charsWritten, format, provider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((BigInteger)this).ToString(format, formatProvider);
        }
        public override string ToString()
        {
            return ((BigInteger)this).ToString();
        }
    }
}
