using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Number
{


    // https://github.com/dotnet/runtime/blob/808c523fd2127abb53292ff199cb8b01752d0b54/src/libraries/System.Private.CoreLib/src/System/UInt128.cs
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.
    /// <summary>
    /// 符号なし 512 bit 整数
    /// <see cref="UInt128"/> の実装をもとにしている。
    /// </summary>
    public readonly partial struct UInt512 : IBinaryInteger<UInt512>, IEquatable<UInt512>, IMinMaxValue<UInt512>
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
        private readonly ulong v7;
        private readonly ulong v6;
        private readonly ulong v5;
        private readonly ulong v4;
        private readonly ulong v3;
        private readonly ulong v2;
        private readonly ulong v1;
        private readonly ulong v0;
#else
        private readonly ulong v0;
        private readonly ulong v1;
        private readonly ulong v2;
        private readonly ulong v3;
        private readonly ulong v4;
        private readonly ulong v5;
        private readonly ulong v6;
        private readonly ulong v7;
#endif

        internal const int Size = 64; // bytes
        internal const int ULongCount = 8;

        public UInt512(ulong v7, ulong v6, ulong v5, ulong v4, ulong v3, ulong v2, ulong v1, ulong v0)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
            this.v6 = v6;
            this.v7 = v7;
        }

        [凾(256)] internal static ReadOnlySpan<ulong> AsULongSpan(in UInt512 v) => MemoryMarshal.Cast<UInt512, ulong>(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in v), 1));
        [凾(256)] internal static ReadOnlySpan<byte> AsByteSpan(in UInt512 v) => MemoryMarshal.AsBytes(AsULongSpan(v));

        static readonly UInt512 _one = new(0, 0, 0, 0, 0, 0, 0, 1);
        public static UInt512 One => _one;
        public static int Radix => 2;
        public static UInt512 Zero => default;
        public static UInt512 AdditiveIdentity => Zero;
        public static UInt512 MultiplicativeIdentity => _one;
        public static UInt512 MaxValue => new(~0ul, ~0ul, ~0ul, ~0ul, ~0ul, ~0ul, ~0ul, ~0ul);
        public static UInt512 MinValue => default;

        [凾(256)]
        public static UInt512 Abs(UInt512 value) => value;
        static bool INumberBase<UInt512>.IsCanonical(UInt512 value) => true;
        static bool INumberBase<UInt512>.IsComplexNumber(UInt512 value) => false;
        public static bool IsEvenInteger(UInt512 value) => (value.v0 & 1) == 0;
        static bool INumberBase<UInt512>.IsFinite(UInt512 value) => true;
        static bool INumberBase<UInt512>.IsImaginaryNumber(UInt512 value) => false;
        static bool INumberBase<UInt512>.IsInfinity(UInt512 value) => false;
        static bool INumberBase<UInt512>.IsInteger(UInt512 value) => true;
        static bool INumberBase<UInt512>.IsNaN(UInt512 value) => false;
        static bool INumberBase<UInt512>.IsNegative(UInt512 value) => false;
        static bool INumberBase<UInt512>.IsNegativeInfinity(UInt512 value) => false;
        static bool INumberBase<UInt512>.IsNormal(UInt512 value) => value != default;
        public static bool IsOddInteger(UInt512 value) => (value.v0 & 1) != 0;
        static bool INumberBase<UInt512>.IsPositive(UInt512 value) => true;
        static bool INumberBase<UInt512>.IsPositiveInfinity(UInt512 value) => false;
        static bool INumberBase<UInt512>.IsRealNumber(UInt512 value) => true;
        static bool INumberBase<UInt512>.IsSubnormal(UInt512 value) => false;
        static bool INumberBase<UInt512>.IsZero(UInt512 value) => (value == default);
        static UInt512 INumberBase<UInt512>.MaxMagnitude(UInt512 x, UInt512 y) => Max(x, y);
        static UInt512 INumberBase<UInt512>.MaxMagnitudeNumber(UInt512 x, UInt512 y) => Max(x, y);
        static UInt512 INumberBase<UInt512>.MinMagnitude(UInt512 x, UInt512 y) => Min(x, y);
        static UInt512 INumberBase<UInt512>.MinMagnitudeNumber(UInt512 x, UInt512 y) => Min(x, y);
        static UInt512 INumber<UInt512>.CopySign(UInt512 value, UInt512 sign) => value;
        public static UInt512 Max(UInt512 x, UInt512 y) => (x >= y) ? x : y;
        static UInt512 INumber<UInt512>.MaxNumber(UInt512 x, UInt512 y) => Max(x, y);
        public static UInt512 Min(UInt512 x, UInt512 y) => (x <= y) ? x : y;
        static UInt512 INumber<UInt512>.MinNumber(UInt512 x, UInt512 y) => Min(x, y);
        public static int Sign(UInt512 value) => (value == default) ? 0 : 1;

        static bool IBinaryInteger<UInt512>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt512 value)
        {
            UInt512 result = default;

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
                    result = Unsafe.ReadUnaligned<UInt512>(ref sourceRef);

                    if (BitConverter.IsLittleEndian)
                    {
                        result = new UInt512(
                            BinaryPrimitives.ReverseEndianness(result.v0),
                            BinaryPrimitives.ReverseEndianness(result.v1),
                            BinaryPrimitives.ReverseEndianness(result.v2),
                            BinaryPrimitives.ReverseEndianness(result.v3),
                            BinaryPrimitives.ReverseEndianness(result.v4),
                            BinaryPrimitives.ReverseEndianness(result.v5),
                            BinaryPrimitives.ReverseEndianness(result.v6),
                            BinaryPrimitives.ReverseEndianness(result.v7)
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

        static bool IBinaryInteger<UInt512>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt512 value)
        {
            UInt512 result = default;

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
                    result = Unsafe.ReadUnaligned<UInt512>(ref sourceRef);

                    if (!BitConverter.IsLittleEndian)
                    {
                        result = new UInt512(
                            BinaryPrimitives.ReverseEndianness(result.v0),
                            BinaryPrimitives.ReverseEndianness(result.v1),
                            BinaryPrimitives.ReverseEndianness(result.v2),
                            BinaryPrimitives.ReverseEndianness(result.v3),
                            BinaryPrimitives.ReverseEndianness(result.v4),
                            BinaryPrimitives.ReverseEndianness(result.v5),
                            BinaryPrimitives.ReverseEndianness(result.v6),
                            BinaryPrimitives.ReverseEndianness(result.v7)
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
                        UInt512 part = Unsafe.Add(ref sourceRef, i);
                        part <<= (i * 8);
                        result |= part;
                    }
                }
            }

            value = result;
            return true;
        }

        bool IBinaryInteger<UInt512>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
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

        bool IBinaryInteger<UInt512>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
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
        public static UInt512 operator +(UInt512 value) => value;

        [凾(256)]
        public static UInt512 operator +(UInt512 left, UInt512 right)
        {
            var v0 = left.v0 + right.v0;
            var carry = (v0 < left.v0) ? 1ul : 0ul;

            var v1 = left.v1 + right.v1 + carry;
            carry = (v1 < left.v1 || carry != 0 && right.v1 == ulong.MaxValue) ? 1ul : 0ul;

            var v2 = left.v2 + right.v2 + carry;
            carry = (v2 < left.v2 || carry != 0 && right.v2 == ulong.MaxValue) ? 1ul : 0ul;

            var v3 = left.v3 + right.v3 + carry;
            carry = (v3 < left.v3 || carry != 0 && right.v3 == ulong.MaxValue) ? 1ul : 0ul;

            var v4 = left.v4 + right.v4 + carry;
            carry = (v4 < left.v4 || carry != 0 && right.v4 == ulong.MaxValue) ? 1ul : 0ul;

            var v5 = left.v5 + right.v5 + carry;
            carry = (v5 < left.v5 || carry != 0 && right.v5 == ulong.MaxValue) ? 1ul : 0ul;

            var v6 = left.v6 + right.v6 + carry;
            carry = (v6 < left.v6 || carry != 0 && right.v6 == ulong.MaxValue) ? 1ul : 0ul;

            var v7 = left.v7 + right.v7 + carry;
            return new UInt512(v7, v6, v5, v4, v3, v2, v1, v0);
        }
        [凾(256)]
        public static UInt512 operator ++(UInt512 value) => value + One;


        [凾(256)]
        public static UInt512 operator -(UInt512 value) => Zero - value;

        [凾(256)]
        public static UInt512 operator -(UInt512 left, UInt512 right)
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
            borrow = (v3 > left.v3 || borrow != 0 && right.v3 == ulong.MaxValue) ? 1ul : 0ul;

            var v4 = left.v4 - right.v4 - borrow;
            borrow = (v4 > left.v4 || borrow != 0 && right.v4 == ulong.MaxValue) ? 1ul : 0ul;

            var v5 = left.v5 - right.v5 - borrow;
            borrow = (v5 > left.v5 || borrow != 0 && right.v5 == ulong.MaxValue) ? 1ul : 0ul;

            var v6 = left.v6 - right.v6 - borrow;
            borrow = (v6 > left.v6 || borrow != 0 && right.v6 == ulong.MaxValue) ? 1ul : 0ul;

            var v7 = left.v7 - right.v7 - borrow;
            return new UInt512(v7, v6, v5, v4, v3, v2, v1, v0);
        }
        [凾(256)]
        public static UInt512 operator --(UInt512 value) => value - One;

        [凾(256)]
        public static UInt512 operator *(UInt512 left, UInt512 right)
        {
            return (UInt512)(new BigInteger(AsByteSpan(left), isUnsigned: true) * new BigInteger(AsByteSpan(right), isUnsigned: true));
        }

        [凾(256)]
        public static UInt512 operator /(UInt512 left, UInt512 right)
        {
            return (UInt512)(new BigInteger(AsByteSpan(left), isUnsigned: true) / new BigInteger(AsByteSpan(right), isUnsigned: true));
        }

        [凾(256)]
        public static UInt512 operator %(UInt512 left, UInt512 right)
        {
            return (UInt512)(new BigInteger(AsByteSpan(left), isUnsigned: true) / new BigInteger(AsByteSpan(right), isUnsigned: true));
        }


        [凾(256)]
        public static UInt512 operator ~(UInt512 value)
            => new UInt512(~value.v7, ~value.v6, ~value.v5, ~value.v4, ~value.v3, ~value.v2, ~value.v1, ~value.v0);

        [凾(256)]
        public static UInt512 operator &(UInt512 left, UInt512 right)
            => new UInt512(left.v7 & right.v7, left.v6 & right.v6, left.v5 & right.v5, left.v4 & right.v4, left.v3 & right.v3, left.v2 & right.v2, left.v1 & right.v1, left.v0 & right.v0);

        [凾(256)]
        public static UInt512 operator |(UInt512 left, UInt512 right)
            => new UInt512(left.v7 | right.v7, left.v6 | right.v6, left.v5 | right.v5, left.v4 | right.v4, left.v3 | right.v3, left.v2 | right.v2, left.v1 | right.v1, left.v0 | right.v0);

        [凾(256)]
        public static UInt512 operator ^(UInt512 left, UInt512 right)
            => new UInt512(left.v7 ^ right.v7, left.v6 ^ right.v6, left.v5 ^ right.v5, left.v4 ^ right.v4, left.v3 ^ right.v3, left.v2 ^ right.v2, left.v1 ^ right.v1, left.v0 ^ right.v0);

        [凾(256)]
        public static UInt512 operator <<(UInt512 value, int shiftAmount)
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

            return new UInt512(v[7], v[6], v[5], v[4], v[3], v[2], v[1], v[0]);
        }

        [凾(256)]
        public static UInt512 operator >>(UInt512 value, int shiftAmount)
            => value >>> shiftAmount;

        [凾(256)]
        public static UInt512 operator >>>(UInt512 value, int shiftAmount)
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

            return new UInt512(v[7], v[6], v[5], v[4], v[3], v[2], v[1], v[0]);
        }

        [凾(256)]
        public static bool operator ==(UInt512 left, UInt512 right)
                => left.v0 == right.v0
                && left.v1 == right.v1
                && left.v2 == right.v2
                && left.v3 == right.v3
                && left.v4 == right.v4
                && left.v5 == right.v5
                && left.v6 == right.v6
                && left.v7 == right.v7;

        [凾(256)]
        public static bool operator !=(UInt512 left, UInt512 right)
                => left.v0 != right.v0
                || left.v1 != right.v1
                || left.v2 != right.v2
                || left.v3 != right.v3
                || left.v4 != right.v4
                || left.v5 != right.v5
                || left.v6 != right.v6
                || left.v7 != right.v7;

        [凾(256)]
        public static bool operator <(UInt512 left, UInt512 right)
        {
            if (left.v7 < right.v7) return true;
            if (left.v7 > right.v7) return false;
            if (left.v6 < right.v6) return true;
            if (left.v6 > right.v6) return false;
            if (left.v5 < right.v5) return true;
            if (left.v5 > right.v5) return false;
            if (left.v4 < right.v4) return true;
            if (left.v4 > right.v4) return false;
            if (left.v3 < right.v3) return true;
            if (left.v3 > right.v3) return false;
            if (left.v2 < right.v2) return true;
            if (left.v2 > right.v2) return false;
            if (left.v1 < right.v1) return true;
            if (left.v1 > right.v1) return false;
            return left.v0 < right.v0;
        }

        [凾(256)]
        public static bool operator <=(UInt512 left, UInt512 right)
        {
            if (left.v7 < right.v7) return true;
            if (left.v7 > right.v7) return false;
            if (left.v6 < right.v6) return true;
            if (left.v6 > right.v6) return false;
            if (left.v5 < right.v5) return true;
            if (left.v5 > right.v5) return false;
            if (left.v4 < right.v4) return true;
            if (left.v4 > right.v4) return false;
            if (left.v3 < right.v3) return true;
            if (left.v3 > right.v3) return false;
            if (left.v2 < right.v2) return true;
            if (left.v2 > right.v2) return false;
            if (left.v1 < right.v1) return true;
            if (left.v1 > right.v1) return false;
            return left.v0 <= right.v0;
        }

        [凾(256)]
        public static bool operator >(UInt512 left, UInt512 right)
        {
            if (left.v7 > right.v7) return true;
            if (left.v7 < right.v7) return false;
            if (left.v6 > right.v6) return true;
            if (left.v6 < right.v6) return false;
            if (left.v5 > right.v5) return true;
            if (left.v5 < right.v5) return false;
            if (left.v4 > right.v4) return true;
            if (left.v4 < right.v4) return false;
            if (left.v3 > right.v3) return true;
            if (left.v3 < right.v3) return false;
            if (left.v2 > right.v2) return true;
            if (left.v2 < right.v2) return false;
            if (left.v1 > right.v1) return true;
            if (left.v1 < right.v1) return false;
            return left.v0 > right.v0;
        }

        [凾(256)]
        public static bool operator >=(UInt512 left, UInt512 right)
        {
            if (left.v7 > right.v7) return true;
            if (left.v7 < right.v7) return false;
            if (left.v6 > right.v6) return true;
            if (left.v6 < right.v6) return false;
            if (left.v5 > right.v5) return true;
            if (left.v5 < right.v5) return false;
            if (left.v4 > right.v4) return true;
            if (left.v4 < right.v4) return false;
            if (left.v3 > right.v3) return true;
            if (left.v3 < right.v3) return false;
            if (left.v2 > right.v2) return true;
            if (left.v2 < right.v2) return false;
            if (left.v1 > right.v1) return true;
            if (left.v1 < right.v1) return false;
            return left.v0 >= right.v0;
        }

        public override bool Equals(object obj) => (obj is UInt512 other) && Equals(other);

        public bool Equals(UInt512 other) => this == other;

        public override int GetHashCode() => HashCode.Combine(v7, v6, v5, v4, v3, v2, v1, v0);

        public int GetByteCount() => Size;
        public int GetShortestBitLength()
            => (Size * 8) - LeadingZeroCountInt(this);

        static int LeadingZeroCountInt(UInt512 value)
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
        public static UInt512 LeadingZeroCount(UInt512 value)
        {
            return (UInt512)LeadingZeroCountInt(value);
        }

        public static UInt512 PopCount(UInt512 value)
        {
            var r = 0;
            foreach (var s in AsULongSpan(value))
                r += BitOperations.PopCount(s);
            return (UInt512)r;
        }

        public static UInt512 TrailingZeroCount(UInt512 value)
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
            return (UInt512)r;
        }

        static UInt512 IBinaryNumber<UInt512>.AllBitsSet => MaxValue;


        public static bool IsPow2(UInt512 value) => PopCount(value) == 1U;

        public static UInt512 Log2(UInt512 value)
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
            return (UInt512)r;
        }

        public int CompareTo(object value)
        {
            if (value is UInt512 other)
            {
                return CompareTo(other);
            }
            else if (value is null)
            {
                return 1;
            }
            else
            {
                throw new ArgumentException("need UInt512", nameof(value));
            }
        }

        public int CompareTo(UInt512 value)
        {
            if (this < value)
                return -1;
            else if (this > value)
                return 1;
            else
                return 0;
        }


        public static UInt512 Parse(string s)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static UInt512 Parse(string s, NumberStyles style)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static UInt512 Parse(string s, IFormatProvider provider)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static UInt512 Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s.AsSpan(), style, NumberFormatInfo.GetInstance(provider));
        }
        public static UInt512 Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, provider);
        }
        public static UInt512 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
        {
            if (TryParse(s, style, NumberFormatInfo.GetInstance(provider), out var result))
                return result;
            throw new FormatException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out UInt512 result)
        {
            return TryParse(s, NumberStyles.Integer, provider, out result);
        }
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, [MaybeNullWhen(false)] out UInt512 result)
        {
            return TryParse(s, NumberStyles.Integer, provider, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string s, out UInt512 result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, out UInt512 result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, out UInt512 result)
        {
            return TryParse(s.AsSpan(), style, NumberFormatInfo.GetInstance(provider), out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out UInt512 result)
        {
            // https://github.com/dotnet/runtime/issues/83619
            // style.HasFlag(NumberStyles.AllowBinarySpecifier);

            if (BigInteger.TryParse(s, style, provider, out var bi))
            {
                result = (UInt512)bi;
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
