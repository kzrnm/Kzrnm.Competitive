// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly partial struct BigIntegerDecimal
    {
        //
        // INumberBase
        //

        //
        // Implicit Conversions To BigIntegerDecimal
        //
        public static implicit operator BigIntegerDecimal(byte value) => new(value);
        public static implicit operator BigIntegerDecimal(char value) => new(value);
        public static implicit operator BigIntegerDecimal(short value) => new(value);
        public static implicit operator BigIntegerDecimal(int value) => new(value);
        public static implicit operator BigIntegerDecimal(long value) => new(value);
        public static implicit operator BigIntegerDecimal(sbyte value) => new(value);
        public static implicit operator BigIntegerDecimal(ushort value) => new(value);
        public static implicit operator BigIntegerDecimal(uint value) => new(value);
        public static implicit operator BigIntegerDecimal(ulong value) => new(value);


        //
        // Explicit Conversions From BigIntegerDecimal
        //

        public static explicit operator byte(BigIntegerDecimal value) => checked((byte)(int)value);
        public static explicit operator char(BigIntegerDecimal value) => checked((char)(int)value);
        public static explicit operator short(BigIntegerDecimal value) => checked((short)(int)value);
        public static explicit operator int(BigIntegerDecimal value)
        {
            value.AssertValid();
            if (value.digits == null)
            {
                return value.sign;  // Value packed into int32 sign
            }
            if (value.digits.Length > 2)
            {
                // More than 32 bits
                throw new OverflowException("Overflow_Int32");
            }
            if (value.sign > 0)
            {
                return checked((int)((long)value.digits[1] * BASE + value.digits[0]));
            }
            return checked((int)(-value.digits[1] * BASE - value.digits[0]));
        }

        public static explicit operator long(BigIntegerDecimal value)
        {
            value.AssertValid();
            if (value.digits == null)
            {
                return value.sign;
            }

            if (value.digits.Length > 3)
            {
                throw new OverflowException("Overflow_Int64");
            }

            if (value.digits.Length == 2)
            {
                if (value.sign > 0)
                {
                    return checked((long)value.digits[1] * BASE + (long)value.digits[0]);
                }
                return checked(-value.digits[1] * BASE + -value.digits[0]);
            }
            if (value.sign > 0)
            {
                return checked(((long)value.digits[2] * BASE + value.digits[1]) * BASE + value.digits[0]);
            }
            return checked((-value.digits[2] * BASE - value.digits[1]) * BASE - value.digits[0]);
            throw new OverflowException("Overflow_Int64");
        }

        public static explicit operator sbyte(BigIntegerDecimal value) => checked((sbyte)(int)value);
        public static explicit operator ushort(BigIntegerDecimal value) => checked((ushort)(int)value);
        public static explicit operator uint(BigIntegerDecimal value)
        {
            value.AssertValid();
            if (value.digits == null)
            {
                return checked((uint)value.sign);
            }
            if (value.digits.Length > 2 || value.sign < 0)
            {
                throw new OverflowException("Overflow_UInt32");
            }
            if (value.digits.Length == 1)
            {
                return value.digits[0];
            }
            return checked(value.digits[1] * BASE + value.digits[0]);
        }

        public static explicit operator ulong(BigIntegerDecimal value)
        {
            value.AssertValid();
            if (value.digits == null)
            {
                return checked((ulong)value.sign);
            }
            if (value.digits.Length > 3 || value.sign < 0)
            {
                throw new OverflowException("Overflow_UInt64");
            }
            if (value.digits.Length == 1)
            {
                return value.digits[0];
            }
            if (value.digits.Length == 2)
            {
                return checked((ulong)value.digits[1] * BASE + value.digits[0]);
            }
            return checked(((ulong)value.digits[2] * BASE + value.digits[1]) * BASE + value.digits[0]);
        }

        public static explicit operator nint(BigIntegerDecimal value)
        {
            if (Environment.Is64BitProcess)
            {
                return (nint)(long)value;
            }
            else
            {
                return (int)value;
            }
        }
        public static explicit operator nuint(BigIntegerDecimal value)
        {
            if (Environment.Is64BitProcess)
            {
                return (nuint)(ulong)value;
            }
            else
            {
                return (uint)value;
            }
        }


        static int INumberBase<BigIntegerDecimal>.Radix => 10;

        [凾(256)]
        public static BigIntegerDecimal CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
        {
            BigIntegerDecimal result;

            if (typeof(TOther) == typeof(BigIntegerDecimal))
            {
                result = (BigIntegerDecimal)(object)value;
            }
            else if (!TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked(value, out result))
            {
                ThrowNotSupportedException();
            }

            return result;
        }

        [凾(256)]
        public static BigIntegerDecimal CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
        {
            BigIntegerDecimal result;

            if (typeof(TOther) == typeof(BigIntegerDecimal))
            {
                result = (BigIntegerDecimal)(object)value;
            }
            else if (!TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating(value, out result))
            {
                ThrowNotSupportedException();
            }

            return result;
        }

        [凾(256)]
        public static BigIntegerDecimal CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
        {
            BigIntegerDecimal result;

            if (typeof(TOther) == typeof(BigIntegerDecimal))
            {
                result = (BigIntegerDecimal)(object)value;
            }
            else if (!TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating(value, out result))
            {
                ThrowNotSupportedException();
            }

            return result;
        }

        static bool INumberBase<BigIntegerDecimal>.IsCanonical(BigIntegerDecimal value) => true;

        static bool INumberBase<BigIntegerDecimal>.IsComplexNumber(BigIntegerDecimal value) => false;

        public static bool IsEvenInteger(BigIntegerDecimal value)
        {
            value.AssertValid();

            if (value.digits is null)
            {
                return (value.sign & 1) == 0;
            }
            return (value.digits[0] & 1) == 0;
        }

        static bool INumberBase<BigIntegerDecimal>.IsFinite(BigIntegerDecimal value) => true;

        static bool INumberBase<BigIntegerDecimal>.IsImaginaryNumber(BigIntegerDecimal value) => false;

        static bool INumberBase<BigIntegerDecimal>.IsInfinity(BigIntegerDecimal value) => false;

        static bool INumberBase<BigIntegerDecimal>.IsInteger(BigIntegerDecimal value) => true;

        static bool INumberBase<BigIntegerDecimal>.IsNaN(BigIntegerDecimal value) => false;

        public static bool IsNegative(BigIntegerDecimal value)
        {
            value.AssertValid();
            return value.sign < 0;
        }

        static bool INumberBase<BigIntegerDecimal>.IsNegativeInfinity(BigIntegerDecimal value) => false;

        static bool INumberBase<BigIntegerDecimal>.IsNormal(BigIntegerDecimal value) => (value != 0);

        public static bool IsOddInteger(BigIntegerDecimal value)
        {
            value.AssertValid();

            if (value.digits is null)
            {
                return (value.sign & 1) != 0;
            }
            return (value.digits[0] & 1) != 0;
        }

        public static bool IsPositive(BigIntegerDecimal value)
        {
            value.AssertValid();
            return value.sign >= 0;
        }

        static bool INumberBase<BigIntegerDecimal>.IsPositiveInfinity(BigIntegerDecimal value) => false;

        static bool INumberBase<BigIntegerDecimal>.IsRealNumber(BigIntegerDecimal value) => true;

        static bool INumberBase<BigIntegerDecimal>.IsSubnormal(BigIntegerDecimal value) => false;

        static bool INumberBase<BigIntegerDecimal>.IsZero(BigIntegerDecimal value)
        {
            value.AssertValid();
            return value.sign == 0;
        }

        public static BigIntegerDecimal MaxMagnitude(BigIntegerDecimal x, BigIntegerDecimal y)
        {
            x.AssertValid();
            y.AssertValid();

            BigIntegerDecimal ax = Abs(x);
            BigIntegerDecimal ay = Abs(y);

            if (ax > ay)
            {
                return x;
            }

            if (ax == ay)
            {
                return IsNegative(x) ? y : x;
            }

            return y;
        }

        static BigIntegerDecimal INumberBase<BigIntegerDecimal>.MaxMagnitudeNumber(BigIntegerDecimal x, BigIntegerDecimal y) => MaxMagnitude(x, y);

        public static BigIntegerDecimal MinMagnitude(BigIntegerDecimal x, BigIntegerDecimal y)
        {
            x.AssertValid();
            y.AssertValid();

            BigIntegerDecimal ax = Abs(x);
            BigIntegerDecimal ay = Abs(y);

            if (ax < ay)
            {
                return x;
            }

            if (ax == ay)
            {
                return IsNegative(x) ? x : y;
            }

            return y;
        }

        static BigIntegerDecimal INumberBase<BigIntegerDecimal>.MinMagnitudeNumber(BigIntegerDecimal x, BigIntegerDecimal y) => MinMagnitude(x, y);

        [凾(256)]
        static bool INumberBase<BigIntegerDecimal>.TryConvertFromChecked<TOther>(TOther value, out BigIntegerDecimal result) => TryConvertFromChecked(value, out result);

        [凾(256)]
        private static bool TryConvertFromChecked<TOther>(TOther value, out BigIntegerDecimal result)
            where TOther : INumberBase<TOther>
        {
            if (RefValue.TryConvert(stackalloc uint[3], value, out var rr))
            {
                result = new(rr);
                return true;
            }
            result = default;
            return false;
        }

        [凾(256)]
        static bool INumberBase<BigIntegerDecimal>.TryConvertFromSaturating<TOther>(TOther value, out BigIntegerDecimal result) => TryConvertFromSaturating(value, out result);

        [凾(256)]
        private static bool TryConvertFromSaturating<TOther>(TOther value, out BigIntegerDecimal result)
            where TOther : INumberBase<TOther>
        {
            if (RefValue.TryConvert(stackalloc uint[3], value, out var rr))
            {
                result = new(rr);
                return true;
            }
            result = default;
            return false;
        }

        [凾(256)]
        static bool INumberBase<BigIntegerDecimal>.TryConvertFromTruncating<TOther>(TOther value, out BigIntegerDecimal result) => TryConvertFromTruncating(value, out result);

        [凾(256)]
        private static bool TryConvertFromTruncating<TOther>(TOther value, out BigIntegerDecimal result)
            where TOther : INumberBase<TOther>
        {
            if (RefValue.TryConvert(stackalloc uint[3], value, out var rr))
            {
                result = new(rr);
                return true;
            }
            result = default;
            return false;
        }

        [凾(256)]
        static bool INumberBase<BigIntegerDecimal>.TryConvertToChecked<TOther>(BigIntegerDecimal value, [MaybeNullWhen(false)] out TOther result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualResult = checked((byte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualResult = checked((char)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualResult = checked((short)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualResult = checked((int)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualResult = checked((long)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualResult = checked((nint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualResult = checked((sbyte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualResult = checked((ushort)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualResult = checked((uint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualResult = checked((ulong)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualResult = checked((nuint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<BigIntegerDecimal>.TryConvertToSaturating<TOther>(BigIntegerDecimal value, [MaybeNullWhen(false)] out TOther result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? byte.MinValue : byte.MaxValue;
                }
                else
                {
                    actualResult = (value.sign >= byte.MaxValue) ? byte.MaxValue :
                                   (value.sign <= byte.MinValue) ? byte.MinValue : (byte)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? char.MinValue : char.MaxValue;
                }
                else
                {
                    actualResult = (value.sign >= char.MaxValue) ? char.MaxValue :
                                   (value.sign <= char.MinValue) ? char.MinValue : (char)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? short.MinValue : short.MaxValue;
                }
                else
                {
                    actualResult = (value.sign >= short.MaxValue) ? short.MaxValue :
                                   (value.sign <= short.MinValue) ? short.MinValue : (short)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualResult = (value.sign >= int.MaxValue) ? int.MaxValue :
                                   (value.sign <= int.MinValue) ? int.MinValue : (int)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualResult = (value >= long.MaxValue) ? long.MaxValue :
                                    (value <= long.MinValue) ? long.MinValue : (long)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? sbyte.MinValue : sbyte.MaxValue;
                }
                else
                {
                    actualResult = (value.sign >= sbyte.MaxValue) ? sbyte.MaxValue :
                                   (value.sign <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? ushort.MinValue : ushort.MaxValue;
                }
                else
                {
                    actualResult = (value.sign >= ushort.MaxValue) ? ushort.MaxValue :
                                   (value.sign <= ushort.MinValue) ? ushort.MinValue : (ushort)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualResult = (value >= uint.MaxValue) ? uint.MaxValue :
                                    IsNegative(value) ? uint.MinValue : (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualResult = (value >= ulong.MaxValue) ? ulong.MaxValue :
                                     IsNegative(value) ? ulong.MinValue : (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<BigIntegerDecimal>.TryConvertToTruncating<TOther>(BigIntegerDecimal value, [MaybeNullWhen(false)] out TOther result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualResult;

                if (value.digits is not null)
                {
                    uint bits = value.digits[0];

                    if (IsNegative(value))
                    {
                        bits = ~bits + 1;
                    }

                    actualResult = (byte)bits;
                }
                else
                {
                    actualResult = (byte)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualResult;

                if (value.digits is not null)
                {
                    uint bits = value.digits[0];

                    if (IsNegative(value))
                    {
                        bits = ~bits + 1;
                    }

                    actualResult = (char)bits;
                }
                else
                {
                    actualResult = (char)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? (short)(~value.digits[0] + 1) : (short)value.digits[0];
                }
                else
                {
                    actualResult = (short)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? (int)(~value.digits[0] + 1) : (int)value.digits[0];
                }
                else
                {
                    actualResult = value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualResult;

                if (value.digits is not null)
                {
                    ulong bits = 0;

                    if (value.digits.Length >= 2)
                    {
                        bits = value.digits[1];
                        bits <<= 32;
                    }

                    bits |= value.digits[0];

                    if (IsNegative(value))
                    {
                        bits = ~bits + 1;
                    }

                    actualResult = (long)bits;
                }
                else
                {
                    actualResult = value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualResult;

                if (value.digits is not null)
                {
                    actualResult = IsNegative(value) ? (sbyte)(~value.digits[0] + 1) : (sbyte)value.digits[0];
                }
                else
                {
                    actualResult = (sbyte)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualResult;

                if (value.digits is not null)
                {
                    uint bits = value.digits[0];

                    if (IsNegative(value))
                    {
                        bits = ~bits + 1;
                    }

                    actualResult = (ushort)bits;
                }
                else
                {
                    actualResult = (ushort)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualResult;

                if (value.digits is not null)
                {
                    uint bits = value.digits[0];

                    if (IsNegative(value))
                    {
                        bits = ~bits + 1;
                    }

                    actualResult = bits;
                }
                else
                {
                    actualResult = (uint)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualResult;

                if (value.digits is not null)
                {
                    ulong bits = 0;

                    if (value.digits.Length >= 2)
                    {
                        bits = value.digits[1];
                        bits <<= 32;
                    }

                    bits |= value.digits[0];

                    if (IsNegative(value))
                    {
                        bits = ~bits + 1;
                    }

                    actualResult = bits;
                }
                else
                {
                    actualResult = (ulong)value.sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        public static BigIntegerDecimal operator -(BigIntegerDecimal value)
        {
            value.AssertValid();
            return new BigIntegerDecimal(-value.sign, value.digits);
        }

        [凾(256)]
        public static BigIntegerDecimal operator +(BigIntegerDecimal value)
        {
            value.AssertValid();
            return value;
        }

        static BigIntegerDecimal IAdditiveIdentity<BigIntegerDecimal, BigIntegerDecimal>.AdditiveIdentity => Zero;
        static BigIntegerDecimal IMultiplicativeIdentity<BigIntegerDecimal, BigIntegerDecimal>.MultiplicativeIdentity => One;
    }
}