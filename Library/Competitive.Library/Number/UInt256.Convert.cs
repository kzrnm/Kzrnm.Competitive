using System;
using System.Numerics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly partial struct UInt256 : IBinaryInteger<UInt256>, IEquatable<UInt256>, IMinMaxValue<UInt256>
    {
        static void ThrowOverflowException() => throw new OverflowException();

        public static implicit operator UInt256(byte value) => new(0, 0, 0, value);
        public static implicit operator UInt256(char value) => new(0, 0, 0, value);
        public static implicit operator UInt256(ushort value) => new(0, 0, 0, value);
        public static implicit operator UInt256(uint value) => new(0, 0, 0, value);
        public static implicit operator UInt256(ulong value) => new(0, 0, 0, value);
        public static implicit operator UInt256(UInt128 value) => new(0, 0, (ulong)(value >>> 64), (ulong)value);
        public static implicit operator UInt256(nuint value) => new(0, 0, 0, value);

        public static explicit operator UInt256(sbyte value) => (byte)value;
        public static explicit operator UInt256(short value) => (ushort)value;
        public static explicit operator UInt256(int value) => (uint)value;
        public static explicit operator UInt256(long value) => (ulong)value;
        public static explicit operator UInt256(Int128 value) => (UInt128)value;
        public static explicit operator UInt256(nint value) => (nuint)value;

        public static explicit operator UInt256(BigInteger value)
        {
            var bytes = value.ToByteArray(isUnsigned: true);
            Span<ulong> v = stackalloc ulong[ULongCount];
            if (bytes.Length > Size)
            {
                bytes = bytes[..Size];
            }
            bytes.CopyTo(MemoryMarshal.AsBytes(v));
            return new UInt256(v[3], v[2], v[1], v[0]);
        }

        public static explicit operator checked UInt256(BigInteger value)
        {
            var bytes = value.ToByteArray(isUnsigned: true);
            if (bytes.Length > Size) ThrowOverflowException();
            Span<ulong> v = stackalloc ulong[ULongCount];
            bytes.CopyTo(MemoryMarshal.AsBytes(v));
            return new UInt256(v[3], v[2], v[1], v[0]);
        }


        public static explicit operator checked UInt256(sbyte value)
        {
            if (sbyte.IsNegative(value)) ThrowOverflowException();
            return (byte)value;
        }
        public static explicit operator checked UInt256(short value)
        {
            if (short.IsNegative(value)) ThrowOverflowException();
            return (ushort)value;
        }
        public static explicit operator checked UInt256(int value)
        {
            if (int.IsNegative(value)) ThrowOverflowException();
            return (uint)value;
        }
        public static explicit operator checked UInt256(long value)
        {
            if (long.IsNegative(value)) ThrowOverflowException();
            return (ulong)value;
        }
        public static explicit operator checked UInt256(Int128 value)
        {
            if (Int128.IsNegative(value)) ThrowOverflowException();
            return (UInt128)value;
        }
        public static explicit operator checked UInt256(nint value)
        {
            if (nint.IsNegative(value)) ThrowOverflowException();
            return (nuint)value;
        }

        public static implicit operator BigInteger(UInt256 value)
        {
            return new BigInteger(AsByteSpan(value), isUnsigned: true);
        }
        public static explicit operator byte(UInt256 value) => (byte)value.v0;
        public static explicit operator char(UInt256 value) => (char)value.v0;
        public static explicit operator ushort(UInt256 value) => (ushort)value.v0;
        public static explicit operator uint(UInt256 value) => (uint)value.v0;
        public static explicit operator ulong(UInt256 value) => value.v0;
        public static explicit operator UInt128(UInt256 value) => new UInt128(value.v1, value.v0);
        public static explicit operator nuint(UInt256 value) => (nuint)value.v0;

        public static explicit operator checked byte(UInt256 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0) ThrowOverflowException();
            return checked((byte)value.v0);
        }
        public static explicit operator checked char(UInt256 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0) ThrowOverflowException();
            return checked((char)value.v0);
        }
        public static explicit operator checked ushort(UInt256 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0) ThrowOverflowException();
            return checked((ushort)value.v0);
        }
        public static explicit operator checked uint(UInt256 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0) ThrowOverflowException();
            return checked((uint)value.v0);
        }
        public static explicit operator checked ulong(UInt256 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0) ThrowOverflowException();
            return checked(value.v0);
        }
        public static explicit operator checked UInt128(UInt256 value)
        {
            if (value.v2 != 0 || value.v3 != 0) ThrowOverflowException();
            return checked(new UInt128(value.v1, value.v0));
        }
        public static explicit operator checked nuint(UInt256 value)
        {
            if (value.v1 != 0 || value.v2 != 0 || value.v3 != 0) ThrowOverflowException();
            return checked((nuint)value.v0);
        }

        public static explicit operator sbyte(UInt256 value) => (sbyte)value.v0;
        public static explicit operator short(UInt256 value) => (short)value.v0;
        public static explicit operator int(UInt256 value) => (int)value.v0;
        public static explicit operator long(UInt256 value) => (long)value.v0;
        public static explicit operator Int128(UInt256 value) => new Int128(value.v1, value.v0);
        public static explicit operator nint(UInt256 value) => (nint)value.v0;

        public static explicit operator checked sbyte(UInt256 value) => checked((sbyte)value.v0);
        public static explicit operator checked short(UInt256 value) => checked((short)value.v0);
        public static explicit operator checked int(UInt256 value) => checked((int)value.v0);
        public static explicit operator checked long(UInt256 value) => checked((long)value.v0);
        public static explicit operator checked Int128(UInt256 value) => checked(new Int128(value.v1, value.v0));
        public static explicit operator checked nint(UInt256 value) => checked((nint)value.v0);

        [凾(256)]
        static bool INumberBase<UInt256>.TryConvertFromChecked<TOther>(TOther value, out UInt256 result)
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
                result = checked((UInt256)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = checked((UInt256)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = checked((UInt256)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = checked((UInt256)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)(object)value;
                result = checked((UInt256)actualValue);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt256>.TryConvertFromSaturating<TOther>(TOther value, out UInt256 result)
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
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt256>.TryConvertFromTruncating<TOther>(TOther value, out UInt256 result)
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
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualValue = (nint)(object)value;
                result = (UInt256)actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [凾(256)]
        static bool INumberBase<UInt256>.TryConvertToChecked<TOther>(UInt256 value, out TOther result)
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
        static bool INumberBase<UInt256>.TryConvertToSaturating<TOther>(UInt256 value, out TOther result)
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
        static bool INumberBase<UInt256>.TryConvertToTruncating<TOther>(UInt256 value, out TOther result)
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
