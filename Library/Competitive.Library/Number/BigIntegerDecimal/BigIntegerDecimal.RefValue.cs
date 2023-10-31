// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly partial struct BigIntegerDecimal
    {
        ref struct RefValue
        {
            [Conditional("DEBUG")]
            public readonly void AssertValid() => BigIntegerDecimal.AssertValid(digits, sign);

            public int sign;
            public ReadOnlySpan<uint> digits;
            public RefValue(BigIntegerDecimal value)
            {
                digits = value.digits;
                sign = value.sign;
            }
            public RefValue(Span<uint> d, long value)
            {
                Debug.Assert(d.Length >= 3);
                if (-BASE < value && value < BASE)
                {
                    sign = (int)value;
                    digits = default;
                }
                else if (value < 0)
                {
                    sign = -1;
                    var quo1 = Math.DivRem(value, BASE, out var rem1);
                    if (-BASE < quo1)
                    {
                        d[0] = (uint)-rem1;
                        d[1] = (uint)-quo1;
                        digits = d[..2];
                    }
                    else
                    {
                        var quo2 = Math.DivRem(quo1, BASE, out var rem2);
                        d[0] = (uint)-rem1;
                        d[1] = (uint)-rem2;
                        d[2] = (uint)-quo2;
                        digits = d[..3];
                    }
                }
                else
                {
                    sign = 1;
                    var quo1 = Math.DivRem(value, BASE, out var rem1);
                    if (quo1 < BASE)
                    {
                        d[0] = (uint)rem1;
                        d[1] = (uint)quo1;
                        digits = d[..2];
                    }
                    else
                    {
                        var quo2 = Math.DivRem(quo1, BASE, out var rem2);
                        d[0] = (uint)rem1;
                        d[1] = (uint)rem2;
                        d[2] = (uint)quo2;
                        digits = d[..3];
                    }
                }
                AssertValid();
            }
            public RefValue(Span<uint> d, ulong value)
            {
                Debug.Assert(d.Length >= 3);
                digits = d;
                if (value < BASE)
                {
                    sign = (int)value;
                    digits = default;
                }
                else
                {
                    sign = 1;
                    var quo1 = value / BASE;
                    var rem1 = value - quo1 * BASE;
                    if (quo1 < BASE)
                    {
                        d[0] = (uint)rem1;
                        d[1] = (uint)quo1;
                        digits = d[..2];
                    }
                    else
                    {
                        var quo2 = quo1 / BASE;
                        var rem2 = quo1 - quo2 * BASE;
                        d[0] = (uint)rem1;
                        d[1] = (uint)rem2;
                        d[2] = (uint)quo2;
                        digits = d[..3];
                    }
                }
                AssertValid();
            }
            public RefValue(ReadOnlySpan<uint> value, bool negative)
            {
                if (value.Length > MaxLength)
                {
                    ThrowOverflowException();
                }

                int len;

                // Try to conserve space as much as possible by checking for wasted leading span entries
                // sometimes the span has leading zeros from bit manipulation operations & and ^
                for (len = value.Length; len > 0 && value[len - 1] == 0; len--) ;

                if (len == 0)
                {
                    sign = 0;
                    digits = default;
                }
                else if (len == 1 && value[0] < BASE)
                {
                    // Values like (Int32.MaxValue+1) are stored as "0x80000000" and as such cannot be packed into sign
                    sign = negative ? -(int)value[0] : (int)value[0];
                    digits = default;
                }
                else
                {
                    sign = negative ? -1 : +1;
                    digits = value[..len];
                }
                AssertValid();
            }
            [凾(256)]
            public static bool TryConvert(Span<uint> d, object value, out RefValue result)
            {
                if (value is byte actualValue_byte)
                {
                    result = new(d, actualValue_byte);
                    return true;
                }
                else if (value is char actualValue_char)
                {
                    result = new(d, actualValue_char);
                    return true;
                }
                else if (value is short actualValue_short)
                {
                    result = new(d, actualValue_short);
                    return true;
                }
                else if (value is int actualValue_int)
                {
                    result = new(d, actualValue_int);
                    return true;
                }
                else if (value is long actualValue_long)
                {
                    result = new(d, actualValue_long);
                    return true;
                }
                else if (value is nint actualValue_nint)
                {
                    result = new(d, actualValue_nint);
                    return true;
                }
                else if (value is sbyte actualValue_sbyte)
                {
                    result = new(d, actualValue_sbyte);
                    return true;
                }
                else if (value is ushort actualValue_ushort)
                {
                    result = new(d, actualValue_ushort);
                    return true;
                }
                else if (value is uint actualValue_uint)
                {
                    result = new(d, actualValue_uint);
                    return true;
                }
                else if (value is ulong actualValue_ulong)
                {
                    result = new(d, actualValue_ulong);
                    return true;
                }
                else if (value is nuint actualValue_nuint)
                {
                    result = new(d, actualValue_nuint);
                    return true;
                }
                else if (value is BigIntegerDecimal actualValue_BigIntegerDecimal)
                {
                    result = new(actualValue_BigIntegerDecimal);
                    return true;
                }
                else
                {
                    result = default;
                    return false;
                }
            }
            [凾(256)]
            public static bool TryConvert<TOther>(Span<uint> d, TOther value, out RefValue result)
            {
                if (typeof(TOther) == typeof(byte))
                {
                    var actualValue = (byte)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(char))
                {
                    var actualValue = (char)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(short))
                {
                    var actualValue = (short)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(int))
                {
                    var actualValue = (int)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(long))
                {
                    var actualValue = (long)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(nint))
                {
                    var actualValue = (nint)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(sbyte))
                {
                    var actualValue = (sbyte)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(ushort))
                {
                    var actualValue = (ushort)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(uint))
                {
                    var actualValue = (uint)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(ulong))
                {
                    var actualValue = (ulong)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(nuint))
                {
                    var actualValue = (nuint)(object)value;
                    result = new(d, actualValue);
                    return true;
                }
                else if (typeof(TOther) == typeof(BigIntegerDecimal))
                {
                    var actualValue = (BigIntegerDecimal)(object)value;
                    result = new(actualValue);
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
}