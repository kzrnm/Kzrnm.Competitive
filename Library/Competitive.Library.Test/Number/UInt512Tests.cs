using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Number;

public class UInt512Tests
{
    static BigInteger biMask = (new BigInteger(1) << (UInt512.Size * 8)) - 1;
    Random rnd = new Random(227);
    static BigInteger CreateBigInteger(ulong v7, ulong v6, ulong v5, ulong v4, ulong v3, ulong v2, ulong v1, ulong v0)
    {
        var bi = new BigInteger();
        bi |= v7; bi <<= 64;
        bi |= v6; bi <<= 64;
        bi |= v5; bi <<= 64;
        bi |= v4; bi <<= 64;
        bi |= v3; bi <<= 64;
        bi |= v2; bi <<= 64;
        bi |= v1; bi <<= 64;
        bi |= v0;
        return bi;
    }

    [Fact]
    [Trait("Category", "Convert")]
    public void ConvertToBigInteger()
    {
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                CreateBigInteger(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(
                new(0, 0, s[5], s[4], s[3], s[2], s[1], s[0]),
                CreateBigInteger(0, 0, s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(
                new(0, 0, 0, 0, 0, 0, 0, s[0]),
                CreateBigInteger(0, 0, 0, 0, 0, 0, 0, s[0]));
        }
        static void Impl(UInt512 value, BigInteger bi)
        {
            ((BigInteger)value).ShouldBe(bi);
        }
    }

    [Fact]
    [Trait("Category", "Convert")]
    public void ConvertTo()
    {
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            UInt512 value;
            checked
            {
                value = new(0, 0, 0, 0, 0, 0, s[1], s[0]);
                var other128 = new UInt128(s[1], s[0]);
                ((UInt128)value).ShouldBe(other128);

                value = new(0, 0, 0, 0, 0, 0, 0, s[0]);
                var other64 = s[0];
                ((UInt64)value).ShouldBe(other64);


                ulong s0 = s[0];

                s0 &= uint.MaxValue;
                value = new(0, 0, 0, 0, 0, 0, 0, s0);
                var other32 = (UInt32)s0;
                ((UInt32)value).ShouldBe(other32);

                s0 &= ushort.MaxValue;
                value = new(0, 0, 0, 0, 0, 0, 0, s0);
                var other16 = (UInt16)s0;
                ((UInt16)value).ShouldBe(other16);

                s0 &= byte.MaxValue;
                value = new(0, 0, 0, 0, 0, 0, 0, s0);
                var other8 = (byte)s0;
                ((byte)value).ShouldBe(other8);
            }
            unchecked
            {
                value = new(0, 0, 0, 0, 0, 0, 0, s[0]);
                var other32 = (UInt32)s[0];
                ((UInt32)value).ShouldBe(other32);

                value = new(0, 0, 0, 0, 0, 0, 0, s[0]);
                var other16 = (UInt16)s[0];
                ((UInt16)value).ShouldBe(other16);

                value = new(0, 0, 0, 0, 0, 0, 0, s[0]);
                var other8 = (byte)s[0];
                ((byte)value).ShouldBe(other8);
            }
        }

        checked
        {
            UInt512 value = ulong.MaxValue;
            Should.Throw<OverflowException>(() => _ = (UInt32)value);
            Should.Throw<OverflowException>(() => _ = (UInt16)value);
            Should.Throw<OverflowException>(() => _ = (byte)value);

            value = new(0, 0, 0, 0, 0, 0, 1, 0);
            Should.Throw<OverflowException>(() => _ = (UInt64)value);
            Should.Throw<OverflowException>(() => _ = (UInt32)value);
            Should.Throw<OverflowException>(() => _ = (UInt16)value);
            Should.Throw<OverflowException>(() => _ = (byte)value);

            value = new(0, 0, 0, 0, 0, 1, 0, 0);
            Should.Throw<OverflowException>(() => _ = (UInt128)value);
            Should.Throw<OverflowException>(() => _ = (UInt64)value);
            Should.Throw<OverflowException>(() => _ = (UInt32)value);
            Should.Throw<OverflowException>(() => _ = (UInt16)value);
            Should.Throw<OverflowException>(() => _ = (byte)value);
        }
    }

    [Fact]
    [Trait("Category", "Convert")]
    public void ConvertFromBigInteger()
    {
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                CreateBigInteger(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(
                new(0, 0, s[5], s[4], s[3], s[2], s[1], s[0]),
                CreateBigInteger(0, 0, s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(
                new(0, 0, 0, 0, 0, 0, 0, s[0]),
                CreateBigInteger(0, 0, 0, 0, 0, 0, 0, s[0]));

            Impl(
                new(s[6], s[5], s[4], s[3], s[2], s[1], s[0], 0),
                CreateBigInteger(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]) << 64);
        }
        static void Impl(UInt512 value, BigInteger bi)
        {
            ((UInt512)bi).ShouldBe(value);
        }
    }

    [Fact]
    [Trait("Category", "Convert")]
    public void ConvertFrom()
    {
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            UInt512 value;
            value = new(0, 0, 0, 0, 0, 0, s[1], s[0]);
            UInt512 other128 = new UInt128(s[1], s[0]);
            other128.ShouldBe(value);

            value = new(0, 0, 0, 0, 0, 0, 0, s[0]);
            UInt512 other64 = s[0];
            other64.ShouldBe(value);

            value = new(0, 0, 0, 0, 0, 0, 0, (uint)s[0]);
            UInt512 other32 = (UInt32)s[0];
            other32.ShouldBe(value);

            value = new(0, 0, 0, 0, 0, 0, 0, (ushort)s[0]);
            UInt512 other16 = (UInt16)s[0];
            other16.ShouldBe(value);

            value = new(0, 0, 0, 0, 0, 0, 0, (byte)s[0]);
            UInt512 other8 = (byte)s[0];
            other8.ShouldBe(value);
        }
    }

    public static IEnumerable<TheoryDataRow<string, UInt512>> Parse_Data()
    {
        yield return ("1", (UInt512)1);
        yield return (new BigInteger(1e60).ToString(), (UInt512)new BigInteger(1e60));
        yield return ("100", (UInt512)100);
    }

    [Theory]
    [MemberData(nameof(Parse_Data))]
    [Trait("Category", "String")]
    public void Parse(string input, UInt512 expected)
    {
        UInt512.Parse(input).ShouldBe(expected);
    }

    [Fact]
    [Trait("Category", "String")]
    public void String()
    {
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(new(0, 0, s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(new(0, 0, 0, 0, 0, 0, 0, s[0]));
        }

        static void Impl(UInt512 value)
        {
            var bi = (BigInteger)value;
            value.ToString().ShouldBe(bi.ToString());
        }
    }


    [Fact]
    [Trait("Category", "Arthmetic")]
    public void Add()
    {
        Impl(UInt512.Zero, UInt512.Zero);
        Impl(UInt512.Zero, UInt512.One);
        Impl(UInt512.Zero, UInt512.MaxValue);

        Impl(UInt512.One, UInt512.One);
        Impl(UInt512.One, UInt512.MaxValue);

        Impl(UInt512.MaxValue, UInt512.MaxValue);

        for (int i = 0; i < 4; i++)
        {
            Impl((UInt512)(BigInteger.One << (64 * i)), 1);
            Impl((UInt512)((BigInteger.One << (64 * i)) - 1), 1);
        }

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(s[1], s[6], s[0], s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, 0, 0, 0, 0, s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.One);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.Zero);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.MaxValue);
        }

        static void Impl(UInt512 value0, UInt512 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            ((BigInteger)(value0 + value1)).ShouldBe(biMask & (bi0 + bi1));
            ((BigInteger)(value1 + value0)).ShouldBe(biMask & (bi0 + bi1));
        }
    }

    [Fact]
    [Trait("Category", "Arthmetic")]
    public void Minus()
    {
        Impl(UInt512.Zero, UInt512.Zero);
        Impl(UInt512.Zero, UInt512.One);
        Impl(UInt512.Zero, UInt512.MaxValue);

        Impl(UInt512.One, UInt512.One);
        Impl(UInt512.One, UInt512.MaxValue);

        Impl(UInt512.MaxValue, UInt512.MaxValue);

        for (int i = 0; i < 4; i++)
        {
            Impl((UInt512)(BigInteger.One << (64 * i)), 1);
            Impl((UInt512)((BigInteger.One << (64 * i)) - 1), 1);
        }

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(s[1], s[6], s[0], s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, 0, 0, 0, 0, s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.One);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.Zero);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.MaxValue);
        }

        static void Impl(UInt512 value0, UInt512 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            ((BigInteger)(value0 - value1)).ShouldBe(biMask & (bi0 - bi1));
            ((BigInteger)(value1 - value0)).ShouldBe(biMask & (bi1 - bi0));
        }
    }

    [Fact]
    [Trait("Category", "Arthmetic")]
    public void Multiply()
    {
        Impl(UInt512.Zero, UInt512.Zero);
        Impl(UInt512.Zero, UInt512.One);
        Impl(UInt512.Zero, UInt512.MaxValue);

        Impl(UInt512.One, UInt512.One);
        Impl(UInt512.One, UInt512.MaxValue);

        Impl(UInt512.MaxValue, UInt512.MaxValue);

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(s[1], s[6], s[0], s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, 0, 0, 0, 0, s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.One);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.Zero);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.MaxValue);
        }

        static void Impl(UInt512 value0, UInt512 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            ((BigInteger)(value0 * value1)).ShouldBe(biMask & (bi0 * bi1));
            ((BigInteger)(value1 * value0)).ShouldBe(biMask & (bi0 * bi1));
        }
    }

    [Fact]
    [Trait("Category", "Arthmetic")]
    public void Divide()
    {
        Impl(UInt512.Zero, UInt512.Zero);
        Impl(UInt512.Zero, UInt512.One);
        Impl(UInt512.Zero, UInt512.MaxValue);

        Impl(UInt512.One, UInt512.One);
        Impl(UInt512.One, UInt512.MaxValue);

        Impl(UInt512.MaxValue, UInt512.MaxValue);

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(s[1], s[6], s[0], s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, 0, 0, 0, 0, s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.One);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.Zero);

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                UInt512.MaxValue);
        }

        static void Impl(UInt512 value0, UInt512 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            if (value1 != 0) ((BigInteger)(value0 / value1)).ShouldBe(biMask & (bi0 / bi1));
            if (value0 != 0) ((BigInteger)(value1 / value0)).ShouldBe(biMask & (bi1 / bi0));
        }
    }

    [Fact]
    [Trait("Category", "Arthmetic")]
    public void SingleMinus()
    {
        Impl(UInt512.Zero);
        Impl(UInt512.One);
        Impl(UInt512.MaxValue);
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(new(0, 0, 0, 0, 0, 0, 0, s[5]));
        }

        static void Impl(UInt512 value)
        {
            var bi = (BigInteger)value;
            if (bi != 0) ((BigInteger)(-value)).ShouldBe((bi ^ biMask) + 1);
        }
    }

    [Fact]
    [Trait("Category", "Bitwise")]
    public void And()
    {
        Impl(UInt512.Zero, UInt512.Zero);
        Impl(UInt512.Zero, UInt512.One);
        Impl(UInt512.Zero, UInt512.MaxValue);

        Impl(UInt512.One, UInt512.One);
        Impl(UInt512.One, UInt512.MaxValue);

        Impl(UInt512.MaxValue, UInt512.MaxValue);

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(s[1], s[6], s[0], s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, 0, 0, 0, 0, s[5]));
        }

        static void Impl(UInt512 value0, UInt512 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            ((BigInteger)(value0 & value1)).ShouldBe(bi0 & bi1);
            ((BigInteger)(value1 & value0)).ShouldBe(bi0 & bi1);
        }
    }

    [Fact]
    [Trait("Category", "Bitwise")]
    public void Or()
    {
        Impl(UInt512.Zero, UInt512.Zero);
        Impl(UInt512.Zero, UInt512.One);
        Impl(UInt512.Zero, UInt512.MaxValue);

        Impl(UInt512.One, UInt512.One);
        Impl(UInt512.One, UInt512.MaxValue);

        Impl(UInt512.MaxValue, UInt512.MaxValue);

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(s[1], s[6], s[0], s[2], s[4], s[3], s[7], s[5]));
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, 0, 0, 0, 0, s[5]));
        }

        static void Impl(UInt512 value0, UInt512 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            ((BigInteger)(value0 | value1)).ShouldBe(bi0 | bi1);
            ((BigInteger)(value1 | value0)).ShouldBe(bi0 | bi1);
        }
    }

    [Fact]
    [Trait("Category", "Bitwise")]
    public void Xor()
    {
        Impl(UInt512.Zero, UInt512.Zero);
        Impl(UInt512.Zero, UInt512.One);
        Impl(UInt512.Zero, UInt512.MaxValue);

        Impl(UInt512.One, UInt512.One);
        Impl(UInt512.One, UInt512.MaxValue);

        Impl(UInt512.MaxValue, UInt512.MaxValue);

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(s[1], s[6], s[0], s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(
                new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]),
                new(0, 0, 0, 0, 0, 0, 0, s[5]));
        }

        static void Impl(UInt512 value0, UInt512 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            ((BigInteger)(value0 ^ value1)).ShouldBe(bi0 ^ bi1);
            ((BigInteger)(value1 ^ value0)).ShouldBe(bi0 ^ bi1);
        }
    }

    [Fact]
    [Trait("Category", "Bitwise")]
    public void Not()
    {
        Impl(UInt512.Zero);
        Impl(UInt512.One);
        Impl(UInt512.MaxValue);
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));
            Impl(new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));
            Impl(new(0, 0, 0, 0, 0, 0, 0, s[5]));
        }

        static void Impl(UInt512 value)
        {
            var bi = (BigInteger)value;
            ((BigInteger)~value).ShouldBe(bi ^ biMask);
        }
    }


    [Fact]
    [Trait("Category", "Shift")]
    public void LeftShift()
    {
        for (int i = 0; i < UInt512.Size * 8; i++)
        {
            Impl(UInt512.Zero, i);
            Impl(UInt512.One, i);
            Impl(UInt512.MaxValue, i);
        }
        var s = new ulong[80].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            for (int i = 0; i < UInt512.Size * 8; i++)
            {
                Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]), i);

                Impl(new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]), i);

                Impl(new(0, 0, 0, 0, 0, 0, 0, s[5]), i);
            }
        }

        static void Impl(UInt512 value, int shift)
        {
            var bi = (BigInteger)value;
            ((BigInteger)(value << shift)).ShouldBe((bi << shift) & biMask);
        }
    }
    [Fact]
    [Trait("Category", "Shift")]
    public void RightShift()
    {
        for (int i = 0; i < UInt512.Size * 8; i++)
        {
            Impl(UInt512.Zero, i);
            Impl(UInt512.One, i);
            Impl(UInt512.MaxValue, i);
        }
        var s = new ulong[80].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            for (int i = 0; i < UInt512.Size * 8; i++)
            {
                Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]), i);

                Impl(new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]), i);

                Impl(new(0, 0, 0, 0, 0, 0, 0, s[5]), i);
            }
        }

        static void Impl(UInt512 value, int shift)
        {
            var bi = (BigInteger)value;
            ((BigInteger)(value >> shift)).ShouldBe((bi >> shift) & biMask);
        }
    }

    [Fact]
    [Trait("Category", "BinaryInteger")]
    public void LeadingZeroCount()
    {
        Impl(UInt512.Zero);
        Impl(UInt512.One);
        Impl(UInt512.MaxValue);
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(new(0, 0, 0, 0, 0, 0, 0, s[5]));

            Impl(new(s[5], 0, 0, 0, 0, 0, 0, 0));

            Impl(new(0, 0, 0, 0, s[5], 0, 0, 0));
        }

        static void Impl(UInt512 value)
        {
            UInt512.LeadingZeroCount(value).ShouldBe((UInt512)Naive(value));
        }
        static int Naive(UInt512 value)
        {
            for (int i = UInt512.Size * 8 - 1; i >= 0; i--)
            {
                if (((value >> i) & 1) != 0)
                {
                    return UInt512.Size * 8 - 1 - i;
                }
            }
            return UInt512.Size * 8;
        }
    }

    [Fact]
    [Trait("Category", "BinaryInteger")]
    public void PopCount()
    {
        Impl(UInt512.Zero);
        Impl(UInt512.One);
        Impl(UInt512.MaxValue);
        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(new(0, 0, 0, 0, 0, 0, 0, s[5]));

            Impl(new(s[5], 0, 0, 0, 0, 0, 0, 0));

            Impl(new(0, 0, 0, 0, s[5], 0, 0, 0));
        }

        static void Impl(UInt512 value)
        {
            UInt512.PopCount(value).ShouldBe((UInt512)Naive(value));
        }
        static int Naive(UInt512 value)
        {
            int r = 0;
            foreach (var v in UInt512.AsULongSpan(value))
            {
                r += BitOperations.PopCount(v);
            }
            return r;
        }
    }

    public static IEnumerable<TheoryDataRow<UInt512, UInt512>> TrailingZeroCount_Data()
    {
        yield return (0b1, 0);
        yield return (0b101, 0);
        yield return (0b10, 1);
        yield return (0b100, 2);
        yield return (new(0, 0, 0, 0, 0, 0, 0, 0), 512);
        yield return (new(0, 0, 0, 0, 0, 0, 1, 1), 0);
        yield return (new(0, 0, 0, 0, 0, 1, 1, 0), 64);
        yield return (new(0, 0, 0, 0, 1, 1, 0, 0), 128);
        yield return (new(0, 0, 0, 1, 1, 0, 0, 0), 192);
        yield return (new(0, 0, 1, 1, 0, 0, 0, 0), 256);
        yield return (new(0, 1, 1, 0, 0, 0, 0, 0), 320);
        yield return (new(1, 1, 0, 0, 0, 0, 0, 0), 384);
        yield return (new(1, 0, 0, 0, 0, 0, 0, 0), 448);

        yield return (new(0, 0, 0, 0, 0, 0, 1, 2), 1);
        yield return (new(0, 0, 0, 0, 0, 1, 2, 0), 1 + 64);
        yield return (new(0, 0, 0, 0, 1, 2, 0, 0), 1 + 128);
        yield return (new(0, 0, 0, 1, 2, 0, 0, 0), 1 + 192);
        yield return (new(0, 0, 1, 2, 0, 0, 0, 0), 1 + 256);
        yield return (new(0, 1, 2, 0, 0, 0, 0, 0), 1 + 320);
        yield return (new(1, 2, 0, 0, 0, 0, 0, 0), 1 + 384);
        yield return (new(2, 0, 0, 0, 0, 0, 0, 0), 1 + 448);
    }

    [Theory]
    [MemberData(nameof(TrailingZeroCount_Data))]
    [Trait("Category", "BinaryInteger")]
    public void TrailingZeroCount(UInt512 input, UInt512 expected)
    {
        UInt512.TrailingZeroCount(input).ShouldBe(expected);
    }

    public static IEnumerable<TheoryDataRow<UInt512, UInt512>> Log2_Data()
    {
        yield return (0b0, 0);
        yield return (0b1, 0);
        yield return (0b101, 2);
        yield return (0b10, 1);
        yield return (0b100, 2);
        yield return (new(0, 0, 0, 0, 0, 0, 1, 0), 64);
        yield return (new(0, 0, 0, 0, 0, 0, 1, 1), 64);
        yield return (new(0, 0, 0, 0, 0, 1, 1, 0), 128);
        yield return (new(0, 0, 0, 0, 1, 1, 0, 0), 192);
        yield return (new(0, 0, 0, 1, 1, 0, 0, 0), 256);
        yield return (new(0, 0, 1, 1, 0, 0, 0, 0), 320);
        yield return (new(0, 1, 1, 0, 0, 0, 0, 0), 384);
        yield return (new(1, 1, 0, 0, 0, 0, 0, 0), 448);
        yield return (new(1, 0, 0, 0, 0, 0, 0, 0), 448);

        yield return (new(0, 0, 0, 0, 0, 0, 1, 2), 64);
        yield return (new(0, 0, 0, 0, 0, 1, 2, 0), 128);
        yield return (new(0, 0, 0, 0, 1, 2, 0, 0), 192);
        yield return (new(0, 0, 0, 1, 2, 0, 0, 0), 256);
        yield return (new(0, 0, 1, 2, 0, 0, 0, 0), 320);
        yield return (new(0, 1, 2, 0, 0, 0, 0, 0), 384);
        yield return (new(1, 2, 0, 0, 0, 0, 0, 0), 448);
        yield return (new(2, 0, 0, 0, 0, 0, 0, 0), 449);
    }

    [Theory]
    [MemberData(nameof(Log2_Data))]
    [Trait("Category", "BinaryInteger")]
    public void Log2(UInt512 input, UInt512 expected)
    {
        UInt512.Log2(input).ShouldBe(expected);
    }


    [Fact]
    [Trait("Category", "Compare")]
    public void Compare()
    {
        (UInt512.Zero < UInt512.One).ShouldBeTrue();
        (UInt512.Zero < UInt512.MaxValue).ShouldBeTrue();
        (UInt512.One < UInt512.MaxValue).ShouldBeTrue();
        (UInt512.Zero > UInt512.One).ShouldBeFalse();
        (UInt512.Zero > UInt512.MaxValue).ShouldBeFalse();
        (UInt512.One > UInt512.MaxValue).ShouldBeFalse();

        (UInt512.One < UInt512.Zero).ShouldBeFalse();
        (UInt512.MaxValue < UInt512.Zero).ShouldBeFalse();
        (UInt512.MaxValue < UInt512.One).ShouldBeFalse();
        (UInt512.One > UInt512.Zero).ShouldBeTrue();
        (UInt512.MaxValue > UInt512.Zero).ShouldBeTrue();
        (UInt512.MaxValue > UInt512.One).ShouldBeTrue();

        var s = new ulong[1000].AsSpan();
        rnd.NextBytes(MemoryMarshal.AsBytes(s));
        for (; UInt512.ULongCount < s.Length; s = s[1..])
        {
            Impl(new(s[7], s[6], s[5], s[4], s[3], s[2], s[1], s[0]));

            Impl(new(0, 0, 0, s[2], s[4], s[3], s[7], s[5]));

            Impl(new(0, 0, 0, 0, 0, 0, 0, s[5]));

            Impl(new(s[5], 0, 0, 0, 0, 0, 0, 0));

            Impl(new(0, 0, 0, 0, s[5], 0, 0, 0));
        }

        static void Impl(UInt512 value)
        {
            if (value == 0 || value == UInt512.MaxValue) return;

            (UInt512.Zero < value).ShouldBeTrue();
            (value < UInt512.MaxValue).ShouldBeTrue();
            (UInt512.Zero <= value).ShouldBeTrue();
            (value <= UInt512.MaxValue).ShouldBeTrue();

            (UInt512.Zero > value).ShouldBeFalse();
            (value > UInt512.MaxValue).ShouldBeFalse();
            (UInt512.Zero >= value).ShouldBeFalse();
            (value >= UInt512.MaxValue).ShouldBeFalse();

            (UInt512.Zero != value).ShouldBeTrue();
            (value != UInt512.MaxValue).ShouldBeTrue();

            (UInt512.Zero == value).ShouldBeFalse();
            (value == UInt512.MaxValue).ShouldBeFalse();

            (value - 1 <= value).ShouldBeTrue();
            (value <= value + 1).ShouldBeTrue();

            (value - 1 >= value).ShouldBeFalse();
            (value >= value + 1).ShouldBeFalse();
        }
    }
}
