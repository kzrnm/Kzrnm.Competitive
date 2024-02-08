using System;
using System.Numerics;
using System.Runtime.InteropServices;
using BigInteger = Kzrnm.Numerics.BigInteger;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly partial struct UInt512 : IBinaryInteger<UInt512>, IEquatable<UInt512>, IMinMaxValue<UInt512>
    {
        static void ThrowOverflowException() => throw new OverflowException();

        public static implicit operator UInt512(byte value) => new(0, 0, 0, 0, 0, 0, 0, value);
        public static implicit operator UInt512(char value) => new(0, 0, 0, 0, 0, 0, 0, value);
        public static implicit operator UInt512(ushort value) => new(0, 0, 0, 0, 0, 0, 0, value);
        public static implicit operator UInt512(uint value) => new(0, 0, 0, 0, 0, 0, 0, value);
        public static implicit operator UInt512(ulong value) => new(0, 0, 0, 0, 0, 0, 0, value);
        public static implicit operator UInt512(UInt128 value) => new(0, 0, 0, 0, 0, 0, (ulong)(value >>> 64), (ulong)value);
        public static implicit operator UInt512(nuint value) => new(0, 0, 0, 0, 0, 0, 0, value);

        public static explicit operator UInt512(sbyte value) => (byte)value;
        public static explicit operator UInt512(short value) => (ushort)value;
        public static explicit operator UInt512(int value) => (uint)value;
        public static explicit operator UInt512(long value) => (ulong)value;
        public static explicit operator UInt512(Int128 value) => (UInt128)value;
        public static explicit operator UInt512(nint value) => (nuint)value;

        public static explicit operator UInt512(BigInteger value)
        {
            var bytes = value.ToByteArray(isUnsigned: true);
            Span<ulong> v = stackalloc ulong[ULongCount];
            if (bytes.Length > Size)
            {
                bytes = bytes[..Size];
            }
            bytes.CopyTo(MemoryMarshal.AsBytes(v));
            return new UInt512(v[7], v[6], v[5], v[4], v[3], v[2], v[1], v[0]);
        }

        public static explicit operator checked UInt512(BigInteger value)
        {
            var bytes = value.ToByteArray(isUnsigned: true);
            if (bytes.Length > Size) ThrowOverflowException();
            Span<ulong> v = stackalloc ulong[ULongCount];
            bytes.CopyTo(MemoryMarshal.AsBytes(v));
            return new UInt512(v[7], v[6], v[5], v[4], v[3], v[2], v[1], v[0]);
        }


        public static explicit operator checked UInt512(sbyte value)
        {
            if (sbyte.IsNegative(value)) ThrowOverflowException();
            return (byte)value;
        }
        public static explicit operator checked UInt512(short value)
        {
            if (short.IsNegative(value)) ThrowOverflowException();
            return (ushort)value;
        }
        public static explicit operator checked UInt512(int value)
        {
            if (int.IsNegative(value)) ThrowOverflowException();
            return (uint)value;
        }
        public static explicit operator checked UInt512(long value)
        {
            if (long.IsNegative(value)) ThrowOverflowException();
            return (ulong)value;
        }
        public static explicit operator checked UInt512(Int128 value)
        {
            if (Int128.IsNegative(value)) ThrowOverflowException();
            return (UInt128)value;
        }
        public static explicit operator checked UInt512(nint value)
        {
            if (nint.IsNegative(value)) ThrowOverflowException();
            return (nuint)value;
        }

        public static implicit operator BigInteger(UInt512 value)
        {
            return new BigInteger(AsByteSpan(value), isUnsigned: true);
        }
        public static explicit operator byte(UInt512 value) => (byte)value.v0;
        public static explicit operator char(UInt512 value) => (char)value.v0;
        public static explicit operator ushort(UInt512 value) => (ushort)value.v0;
        public static explicit operator uint(UInt512 value) => (uint)value.v0;
        public static explicit operator ulong(UInt512 value) => value.v0;
        public static explicit operator UInt128(UInt512 value) => new UInt128(value.v1, value.v0);
        public static explicit operator nuint(UInt512 value) => (nuint)value.v0;

        public static explicit operator checked byte(UInt512 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0 || value.v4 != 0 || value.v5 != 0 || value.v6 != 0 || value.v7 != 0) ThrowOverflowException();
            return checked((byte)value.v0);
        }
        public static explicit operator checked char(UInt512 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0 || value.v4 != 0 || value.v5 != 0 || value.v6 != 0 || value.v7 != 0) ThrowOverflowException();
            return checked((char)value.v0);
        }
        public static explicit operator checked ushort(UInt512 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0 || value.v4 != 0 || value.v5 != 0 || value.v6 != 0 || value.v7 != 0) ThrowOverflowException();
            return checked((ushort)value.v0);
        }
        public static explicit operator checked uint(UInt512 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0 || value.v4 != 0 || value.v5 != 0 || value.v6 != 0 || value.v7 != 0) ThrowOverflowException();
            return checked((uint)value.v0);
        }
        public static explicit operator checked ulong(UInt512 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0 || value.v4 != 0 || value.v5 != 0 || value.v6 != 0 || value.v7 != 0) ThrowOverflowException();
            return checked(value.v0);
        }
        public static explicit operator checked UInt128(UInt512 value)
        {
            if (value.v2 != 0 || value.v3 != 0 || value.v4 != 0 || value.v5 != 0 || value.v6 != 0 || value.v7 != 0) ThrowOverflowException();
            return checked(new UInt128(value.v1, value.v0));
        }
        public static explicit operator checked nuint(UInt512 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0 || value.v4 != 0 || value.v5 != 0 || value.v6 != 0) ThrowOverflowException();
            return checked((nuint)value.v0);
        }

        public static explicit operator sbyte(UInt512 value) => (sbyte)value.v0;
        public static explicit operator short(UInt512 value) => (short)value.v0;
        public static explicit operator int(UInt512 value) => (int)value.v0;
        public static explicit operator long(UInt512 value) => (long)value.v0;
        public static explicit operator Int128(UInt512 value) => new Int128(value.v1, value.v0);
        public static explicit operator nint(UInt512 value) => (nint)value.v0;

        public static explicit operator checked sbyte(UInt512 value) => checked((sbyte)value.v0);
        public static explicit operator checked short(UInt512 value) => checked((short)value.v0);
        public static explicit operator checked int(UInt512 value) => checked((int)value.v0);
        public static explicit operator checked long(UInt512 value) => checked((long)value.v0);
        public static explicit operator checked Int128(UInt512 value) => checked(new Int128(value.v1, value.v0));
        public static explicit operator checked nint(UInt512 value) => checked((nint)value.v0);

        [凾(256)]
        static bool INumberBase<UInt512>.TryConvertFromChecked<TOther>(TOther value, out UInt512 result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)(object)value;
                result = checked((UInt512)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = checked((UInt512)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = checked((UInt512)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = checked((UInt512)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)(object)value;
                result = checked((UInt512)actualValue);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt512>.TryConvertFromSaturating<TOther>(TOther value, out UInt512 result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt512>.TryConvertFromTruncating<TOther>(TOther value, out UInt512 result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)(object)value;
                result = (UInt512)actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt512>.TryConvertToChecked<TOther>(UInt512 value, out TOther result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = checked((byte)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualValue = checked((char)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = checked((ushort)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = checked((uint)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = checked((ulong)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualValue = checked((nuint)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = checked((sbyte)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = checked((short)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = checked((int)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = checked((long)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = checked((nint)value);
                result = (TOther)(object)actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt512>.TryConvertToSaturating<TOther>(UInt512 value, out TOther result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualValue = (char)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualValue = (nuint)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt512>.TryConvertToTruncating<TOther>(UInt512 value, out TOther result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualValue = (char)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualValue = (nuint)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)value;
                result = (TOther)(object)actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
}
