
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using Xunit.Sdk;


namespace Kzrnm.Competitive.Testing;

public readonly struct UInt24(byte v2, byte v1, byte v0) : INumber<UInt24>, INumKz<UInt24>
{

#if BIGENDIAN
    private readonly byte v2 = v2;
    private readonly byte v1 = v1;
    private readonly byte v0 = v0;
#else
    private readonly byte v0 = v0;
    private readonly byte v1 = v1;
    private readonly byte v2 = v2;
#endif

    public override string ToString() => ((uint)this).ToString();
    public override bool Equals([NotNullWhen(true)] object obj)
        => obj is UInt24 v && Equals(v);
    public override int GetHashCode() => (int)this;

    public static UInt24 One => (UInt24)1;
    public static UInt24 Abs(UInt24 value) => value;

    public static bool IsEvenInteger(UInt24 value) => uint.IsEvenInteger((uint)value);

    public static bool IsInteger(UInt24 value) => true;

    public static bool IsNaN(UInt24 value) => false;

    public static bool IsNegative(UInt24 value) => false;

    public static bool IsNormal(UInt24 value) => true;

    public static bool IsOddInteger(UInt24 value) => uint.IsOddInteger((uint)value);

    public static bool IsPositive(UInt24 value) => 0 < (uint)value;

    public static UInt24 MaxMagnitude(UInt24 x, UInt24 y) => (UInt24)uint.Max(x, y);

    public static UInt24 MaxMagnitudeNumber(UInt24 x, UInt24 y) => (UInt24)uint.Max(x, y);

    public static UInt24 MinMagnitude(UInt24 x, UInt24 y) => (UInt24)uint.Min(x, y);

    public static UInt24 MinMagnitudeNumber(UInt24 x, UInt24 y) => (UInt24)uint.Min(x, y);

    public static UInt24 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider)
        => (UInt24)uint.Parse(s, style, provider);

    public static UInt24 Parse(string s, NumberStyles style, IFormatProvider provider)
        => (UInt24)uint.Parse(s, style, provider);

    public static UInt24 Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        => (UInt24)uint.Parse(s, provider);

    public static UInt24 Parse(string s, IFormatProvider provider)
        => (UInt24)uint.Parse(s, provider);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out UInt24 result)
    {
        if (uint.TryParse(s, style, provider, out var v))
        {
            result = (UInt24)v;
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out UInt24 result)
    {
        if (uint.TryParse(s, style, provider, out var v))
        {
            result = (UInt24)v;
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, [MaybeNullWhen(false)] out UInt24 result)
    {
        if (uint.TryParse(s, provider, out var v))
        {
            result = (UInt24)v;
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out UInt24 result)
    {
        if (uint.TryParse(s, provider, out var v))
        {
            result = (UInt24)v;
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, out UInt24 result)
    {
        if (uint.TryParse(s, out var v))
        {
            result = (UInt24)v;
            return true;
        }
        result = default;
        return false;
    }

    static bool INumberBase<UInt24>.TryConvertFromChecked<TOther>(TOther value, out UInt24 result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<UInt24>.TryConvertFromSaturating<TOther>(TOther value, out UInt24 result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<UInt24>.TryConvertFromTruncating<TOther>(TOther value, out UInt24 result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<UInt24>.TryConvertToChecked<TOther>(UInt24 value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<UInt24>.TryConvertToSaturating<TOther>(UInt24 value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<UInt24>.TryConvertToTruncating<TOther>(UInt24 value, out TOther result)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(object obj) => ((uint)this).CompareTo(obj);

    public int CompareTo(UInt24 other) => ((uint)this).CompareTo(other);

    public bool Equals(UInt24 other) => v0 == other.v0 && v1 == other.v1 && v2 == other.v2;

    public string ToString(string format, IFormatProvider formatProvider)
        => ((uint)this).ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        => ((uint)this).TryFormat(destination, out charsWritten, format, provider);

    public static UInt24 operator +(UInt24 value) => value;

    public static UInt24 operator +(UInt24 left, UInt24 right)
        => (UInt24)((uint)left + right);

    public static UInt24 operator -(UInt24 value) => default(UInt24) - value;

    public static UInt24 operator -(UInt24 left, UInt24 right)
        => (UInt24)((uint)left - right);

    public static UInt24 operator ++(UInt24 value) => (UInt24)(((uint)value) + 1);

    public static UInt24 operator --(UInt24 value) => (UInt24)(((uint)value) - 1);

    public static UInt24 operator *(UInt24 left, UInt24 right)
        => (UInt24)((uint)left * right);

    public static UInt24 operator /(UInt24 left, UInt24 right)
        => (UInt24)((uint)left / right);

    public static UInt24 operator %(UInt24 left, UInt24 right)
        => (UInt24)((uint)left % right);

    public static bool operator ==(UInt24 left, UInt24 right)
        => left.Equals(right);

    public static bool operator !=(UInt24 left, UInt24 right)
        => !left.Equals(right);

    public static bool operator <(UInt24 left, UInt24 right)
        => (uint)left < right;

    public static bool operator >(UInt24 left, UInt24 right)
        => (uint)left > right;

    public static bool operator <=(UInt24 left, UInt24 right)
        => (uint)left <= right;

    public static bool operator >=(UInt24 left, UInt24 right)
        => (uint)left >= right;

    public static implicit operator uint(UInt24 n) => (uint)((n.v2 << 16) | (n.v1 << 8) | n.v0);
    public static explicit operator UInt24(uint n) => new((byte)(n & 0xFF0000), (byte)(n & 0x00FF00), (byte)(n & 0x0000FF));

    public static implicit operator int(UInt24 n) => ((n.v2 << 16) | (n.v1 << 8) | n.v0);
    public static explicit operator UInt24(int n) => new((byte)(n & 0xFF0000), (byte)(n & 0x00FF00), (byte)(n & 0x0000FF));
}