using System.Numerics;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Number;

public class UInt256Tests
{
    static BigInteger biMask = (new BigInteger(1) << (UInt256.Size * 8)) - 1;
    Random rnd = new Random(227);
    static BigInteger CreateBigInteger(ulong v3, ulong v2, ulong v1, ulong v0)
    {
        var bi = new BigInteger();
        bi |= v3; bi <<= 64;
        bi |= v2; bi <<= 64;
        bi |= v1; bi <<= 64;
        bi |= v0;
        return bi;
    }

    [Test, MultipleAssertions]
    [Property("Category", "Convert")]
    public async Task ConvertToBigInteger()
    {
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                CreateBigInteger(s[3], s[2], s[1], s[0]));

            await Impl(
                new(0, 0, s[1], s[0]),
                CreateBigInteger(0, 0, s[1], s[0]));

            await Impl(
                new(0, 0, 0, s[0]),
                CreateBigInteger(0, 0, 0, s[0]));
        }
        static async Task Impl(UInt256 value, BigInteger bi)
        {
            await ((BigInteger)value).Should().BeEqualTo(bi);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Convert")]
    public async Task ConvertTo()
    {
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            UInt256 value;
            checked
            {
                value = new(0, 0, s[1], s[0]);
                var other128 = new UInt128(s[1], s[0]);
                await ((UInt128)value).Should().BeEqualTo(other128);

                value = new(0, 0, 0, s[0]);
                var other64 = s[0];
                await ((UInt64)value).Should().BeEqualTo(other64);


                ulong s0 = s[0];

                s0 &= uint.MaxValue;
                value = new(0, 0, 0, s0);
                var other32 = (UInt32)s0;
                await ((UInt32)value).Should().BeEqualTo(other32);

                s0 &= ushort.MaxValue;
                value = new(0, 0, 0, s0);
                var other16 = (UInt16)s0;
                await ((UInt16)value).Should().BeEqualTo(other16);

                s0 &= byte.MaxValue;
                value = new(0, 0, 0, s0);
                var other8 = (byte)s0;
                await ((byte)value).Should().BeEqualTo(other8);
            }
            unchecked
            {
                value = new(0, 0, 0, s[0]);
                var other32 = (UInt32)s[0];
                await ((UInt32)value).Should().BeEqualTo(other32);

                value = new(0, 0, 0, s[0]);
                var other16 = (UInt16)s[0];
                await ((UInt16)value).Should().BeEqualTo(other16);

                value = new(0, 0, 0, s[0]);
                var other8 = (byte)s[0];
                await ((byte)value).Should().BeEqualTo(other8);
            }
        }

        checked
        {
            UInt256 value = ulong.MaxValue;
            Assert.Throws<OverflowException>(() => _ = (UInt32)value);
            Assert.Throws<OverflowException>(() => _ = (UInt16)value);
            Assert.Throws<OverflowException>(() => _ = (byte)value);

            value = new(0, 0, 1, 0);
            Assert.Throws<OverflowException>(() => _ = (UInt64)value);
            Assert.Throws<OverflowException>(() => _ = (UInt32)value);
            Assert.Throws<OverflowException>(() => _ = (UInt16)value);
            Assert.Throws<OverflowException>(() => _ = (byte)value);

            value = new(0, 1, 0, 0);
            Assert.Throws<OverflowException>(() => _ = (UInt128)value);
            Assert.Throws<OverflowException>(() => _ = (UInt64)value);
            Assert.Throws<OverflowException>(() => _ = (UInt32)value);
            Assert.Throws<OverflowException>(() => _ = (UInt16)value);
            Assert.Throws<OverflowException>(() => _ = (byte)value);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Convert")]
    public async Task ConvertFromBigInteger()
    {
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                CreateBigInteger(s[3], s[2], s[1], s[0]));

            await Impl(
                new(0, 0, s[1], s[0]),
                CreateBigInteger(0, 0, s[1], s[0]));

            await Impl(
                new(0, 0, 0, s[0]),
                CreateBigInteger(0, 0, 0, s[0]));

            await Impl(
                new(s[2], s[1], s[0], 0),
                CreateBigInteger(s[3], s[2], s[1], s[0]) << 64);
        }
        static async Task Impl(UInt256 value, BigInteger bi)
        {
            await ((UInt256)bi).Should().BeEqualTo(value);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Convert")]
    public async Task ConvertFrom()
    {
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            UInt256 value;
            value = new(0, 0, s[1], s[0]);
            UInt256 other128 = new UInt128(s[1], s[0]);
            await other128.Should().BeEqualTo(value);

            value = new(0, 0, 0, s[0]);
            UInt256 other64 = s[0];
            await other64.Should().BeEqualTo(value);

            value = new(0, 0, 0, (uint)s[0]);
            UInt256 other32 = (UInt32)s[0];
            await other32.Should().BeEqualTo(value);

            value = new(0, 0, 0, (ushort)s[0]);
            UInt256 other16 = (UInt16)s[0];
            await other16.Should().BeEqualTo(value);

            value = new(0, 0, 0, (byte)s[0]);
            UInt256 other8 = (byte)s[0];
            await other8.Should().BeEqualTo(value);
        }
    }

    public static IEnumerable<(string, UInt256)> Parse_Data()
    {
        yield return ("1", (UInt256)1);
        yield return (new BigInteger(1e60).ToString(), (UInt256)new BigInteger(1e60));
        yield return ("100", (UInt256)100);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Parse_Data))]
    [Property("Category", "String")]
    public async Task Parse(string input, UInt256 expected)
    {
        await UInt256.Parse(input).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "String")]
    public async Task String()
    {
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(new(s[3], s[2], s[1], s[0]));

            await Impl(new(0, 0, s[1], s[0]));

            await Impl(new(0, 0, 0, s[0]));
        }

        static async Task Impl(UInt256 value)
        {
            var bi = (BigInteger)value;
            await value.ToString().Should().BeEqualTo(bi.ToString());
        }
    }


    [Test, MultipleAssertions]
    [Property("Category", "Arthmetic")]
    public async Task Add()
    {
        await Impl(UInt256.Zero, UInt256.Zero);
        await Impl(UInt256.Zero, UInt256.One);
        await Impl(UInt256.Zero, UInt256.MaxValue);

        await Impl(UInt256.One, UInt256.One);
        await Impl(UInt256.One, UInt256.MaxValue);

        await Impl(UInt256.MaxValue, UInt256.MaxValue);

        for (int i = 0; i < 4; i++)
        {
            await Impl((UInt256)(BigInteger.One << (64 * i)), 1);
            await Impl((UInt256)((BigInteger.One << (64 * i)) - 1), 1);
        }

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(s[1], s[0], s[2], s[3]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, s[1], s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.One);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.Zero);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.MaxValue);
        }

        static async Task Impl(UInt256 value0, UInt256 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            await ((BigInteger)(value0 + value1)).Should().BeEqualTo(biMask & (bi0 + bi1));
            await ((BigInteger)(value1 + value0)).Should().BeEqualTo(biMask & (bi0 + bi1));
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Arthmetic")]
    public async Task Minus()
    {
        await Impl(UInt256.Zero, UInt256.Zero);
        await Impl(UInt256.Zero, UInt256.One);
        await Impl(UInt256.Zero, UInt256.MaxValue);

        await Impl(UInt256.One, UInt256.One);
        await Impl(UInt256.One, UInt256.MaxValue);

        await Impl(UInt256.MaxValue, UInt256.MaxValue);

        for (int i = 0; i < 4; i++)
        {
            await Impl((UInt256)(BigInteger.One << (64 * i)), 1);
            await Impl((UInt256)((BigInteger.One << (64 * i)) - 1), 1);
        }

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(s[1], s[0], s[2], s[3]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, s[1], s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.One);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.Zero);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.MaxValue);
        }

        static async Task Impl(UInt256 value0, UInt256 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            await ((BigInteger)(value0 - value1)).Should().BeEqualTo(biMask & (bi0 - bi1));
            await ((BigInteger)(value1 - value0)).Should().BeEqualTo(biMask & (bi1 - bi0));
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Arthmetic")]
    public async Task Multiply()
    {
        await Impl(UInt256.Zero, UInt256.Zero);
        await Impl(UInt256.Zero, UInt256.One);
        await Impl(UInt256.Zero, UInt256.MaxValue);

        await Impl(UInt256.One, UInt256.One);
        await Impl(UInt256.One, UInt256.MaxValue);

        await Impl(UInt256.MaxValue, UInt256.MaxValue);

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(s[1], s[0], s[2], s[3]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, s[1], s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.One);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.Zero);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.MaxValue);
        }

        static async Task Impl(UInt256 value0, UInt256 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            await ((BigInteger)(value0 * value1)).Should().BeEqualTo(biMask & (bi0 * bi1));
            await ((BigInteger)(value1 * value0)).Should().BeEqualTo(biMask & (bi0 * bi1));
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Arthmetic")]
    public async Task Divide()
    {
        await Impl(UInt256.Zero, UInt256.Zero);
        await Impl(UInt256.Zero, UInt256.One);
        await Impl(UInt256.Zero, UInt256.MaxValue);

        await Impl(UInt256.One, UInt256.One);
        await Impl(UInt256.One, UInt256.MaxValue);

        await Impl(UInt256.MaxValue, UInt256.MaxValue);

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(s[1], s[0], s[2], s[3]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, s[1], s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.One);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.Zero);

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                UInt256.MaxValue);
        }

        static async Task Impl(UInt256 value0, UInt256 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            if (value1 != 0)
                await ((BigInteger)(value0 / value1)).Should().BeEqualTo(biMask & (bi0 / bi1));
            if (value0 != 0)
                await ((BigInteger)(value1 / value0)).Should().BeEqualTo(biMask & (bi1 / bi0));
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Arthmetic")]
    public async Task SingleMinus()
    {
        await Impl(UInt256.Zero);
        await Impl(UInt256.One);
        await Impl(UInt256.MaxValue);
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(new(s[3], s[2], s[1], s[0]));

            await Impl(new(0, 0, s[1], s[2]));

            await Impl(new(0, 0, 0, s[2]));
        }

        static async Task Impl(UInt256 value)
        {
            var bi = (BigInteger)value;
            if (bi != 0)
                await ((BigInteger)(-value)).Should().BeEqualTo((bi ^ biMask) + 1);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Bitwise")]
    public async Task And()
    {
        await Impl(UInt256.Zero, UInt256.Zero);
        await Impl(UInt256.Zero, UInt256.One);
        await Impl(UInt256.Zero, UInt256.MaxValue);

        await Impl(UInt256.One, UInt256.One);
        await Impl(UInt256.One, UInt256.MaxValue);

        await Impl(UInt256.MaxValue, UInt256.MaxValue);

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(s[1], s[0], s[2], s[3]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, s[1], s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2]));
        }

        static async Task Impl(UInt256 value0, UInt256 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            await ((BigInteger)(value0 & value1)).Should().BeEqualTo(bi0 & bi1);
            await ((BigInteger)(value1 & value0)).Should().BeEqualTo(bi0 & bi1);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Bitwise")]
    public async Task Or()
    {
        await Impl(UInt256.Zero, UInt256.Zero);
        await Impl(UInt256.Zero, UInt256.One);
        await Impl(UInt256.Zero, UInt256.MaxValue);

        await Impl(UInt256.One, UInt256.One);
        await Impl(UInt256.One, UInt256.MaxValue);

        await Impl(UInt256.MaxValue, UInt256.MaxValue);

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(s[1], s[0], s[2], s[3]));
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, s[1], s[2]));
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2]));
        }

        static async Task Impl(UInt256 value0, UInt256 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            await ((BigInteger)(value0 | value1)).Should().BeEqualTo(bi0 | bi1);
            await ((BigInteger)(value1 | value0)).Should().BeEqualTo(bi0 | bi1);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Bitwise")]
    public async Task Xor()
    {
        await Impl(UInt256.Zero, UInt256.Zero);
        await Impl(UInt256.Zero, UInt256.One);
        await Impl(UInt256.Zero, UInt256.MaxValue);

        await Impl(UInt256.One, UInt256.One);
        await Impl(UInt256.One, UInt256.MaxValue);

        await Impl(UInt256.MaxValue, UInt256.MaxValue);

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(s[1], s[0], s[2], s[3]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, s[1], s[2]));

            await Impl(
                new(s[3], s[2], s[1], s[0]),
                new(0, 0, 0, s[2]));
        }

        static async Task Impl(UInt256 value0, UInt256 value1)
        {
            var bi0 = (BigInteger)value0;
            var bi1 = (BigInteger)value1;
            await ((BigInteger)(value0 ^ value1)).Should().BeEqualTo(bi0 ^ bi1);
            await ((BigInteger)(value1 ^ value0)).Should().BeEqualTo(bi0 ^ bi1);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Bitwise")]
    public async Task Not()
    {
        await Impl(UInt256.Zero);
        await Impl(UInt256.One);
        await Impl(UInt256.MaxValue);
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(new(s[3], s[2], s[1], s[0]));
            await Impl(new(0, 0, s[1], s[2]));
            await Impl(new(0, 0, 0, s[2]));
        }

        static async Task Impl(UInt256 value)
        {
            var bi = (BigInteger)value;
            await ((BigInteger)~value).Should().BeEqualTo(bi ^ biMask);
        }
    }


    [Test, MultipleAssertions]
    [Property("Category", "Shift")]
    public async Task LeftShift()
    {
        for (int i = 0; i < UInt256.Size * 8; i++)
        {
            await Impl(UInt256.Zero, i);
            await Impl(UInt256.One, i);
            await Impl(UInt256.MaxValue, i);
        }
        var s = new ulong[80];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            for (int i = 0; i < UInt256.Size * 8; i++)
            {
                await Impl(new(s[3], s[2], s[1], s[0]), i);

                await Impl(new(0, 0, s[1], s[2]), i);

                await Impl(new(0, 0, 0, s[2]), i);
            }
        }

        static async Task Impl(UInt256 value, int shift)
        {
            var bi = (BigInteger)value;
            await ((BigInteger)(value << shift)).Should().BeEqualTo((bi << shift) & biMask);
        }
    }
    [Test, MultipleAssertions]
    [Property("Category", "Shift")]
    public async Task RightShift()
    {
        for (int i = 0; i < UInt256.Size * 8; i++)
        {
            await Impl(UInt256.Zero, i);
            await Impl(UInt256.One, i);
            await Impl(UInt256.MaxValue, i);
        }
        var s = new ulong[80];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            for (int i = 0; i < UInt256.Size * 8; i++)
            {
                await Impl(new(s[3], s[2], s[1], s[0]), i);

                await Impl(new(0, 0, s[1], s[2]), i);

                await Impl(new(0, 0, 0, s[2]), i);
            }
        }

        static async Task Impl(UInt256 value, int shift)
        {
            var bi = (BigInteger)value;
            await ((BigInteger)(value >> shift)).Should().BeEqualTo((bi >> shift) & biMask);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "BinaryInteger")]
    public async Task LeadingZeroCount()
    {
        await Impl(UInt256.Zero);
        await Impl(UInt256.One);
        await Impl(UInt256.MaxValue);
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(new(s[3], s[2], s[1], s[0]));

            await Impl(new(0, 0, s[1], s[2]));

            await Impl(new(0, 0, 0, s[2]));

            await Impl(new(0, s[2], 0, 0));

            await Impl(new(s[2], 0, 0, 0));
        }

        static async Task Impl(UInt256 value)
        {
            await UInt256.LeadingZeroCount(value).Should().BeEqualTo((UInt256)Naive(value));
        }
        static int Naive(UInt256 value)
        {
            for (int i = UInt256.Size * 8 - 1; i >= 0; i--)
            {
                if (((value >> i) & 1) != 0)
                {
                    return UInt256.Size * 8 - 1 - i;
                }
            }
            return UInt256.Size * 8;
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "BinaryInteger")]
    public async Task PopCount()
    {
        await Impl(UInt256.Zero);
        await Impl(UInt256.One);
        await Impl(UInt256.MaxValue);
        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(new(s[3], s[2], s[1], s[0]));

            await Impl(new(0, 0, s[1], s[2]));

            await Impl(new(0, 0, 0, s[2]));

            await Impl(new(0, s[2], 0, 0));

            await Impl(new(s[2], 0, 0, 0));
        }

        static async Task Impl(UInt256 value)
        {
            await UInt256.PopCount(value).Should().BeEqualTo((UInt256)Naive(value));
        }
        static int Naive(UInt256 value)
        {
            int r = 0;
            foreach (var v in UInt256.AsULongSpan(value))
            {
                r += BitOperations.PopCount(v);
            }
            return r;
        }
    }

    public static IEnumerable<(UInt256, UInt256)> TrailingZeroCount_Data()
    {
        yield return (0b1, 0);
        yield return (0b101, 0);
        yield return (0b10, 1);
        yield return (0b100, 2);
        yield return (new(0, 0, 0, 0), 256);
        yield return (new(0, 0, 1, 1), 0);
        yield return (new(0, 1, 1, 0), 64);
        yield return (new(1, 1, 0, 0), 128);
        yield return (new(1, 0, 0, 0), 192);

        yield return (new(0, 0, 1, 2), 1);
        yield return (new(0, 1, 2, 0), 1 + 64);
        yield return (new(1, 2, 0, 0), 1 + 128);
        yield return (new(2, 0, 0, 0), 1 + 192);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(TrailingZeroCount_Data))]
    [Property("Category", "BinaryInteger")]
    public async Task TrailingZeroCount(UInt256 input, UInt256 expected)
    {
        await UInt256.TrailingZeroCount(input).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(UInt256, UInt256)> Log2_Data()
    {
        yield return (0b0, 0);
        yield return (0b1, 0);
        yield return (0b101, 2);
        yield return (0b10, 1);
        yield return (0b100, 2);
        yield return (new(0, 0, 1, 0), 64);
        yield return (new(0, 0, 1, 1), 64);
        yield return (new(0, 1, 1, 0), 128);
        yield return (new(1, 1, 0, 0), 192);
        yield return (new(1, 0, 0, 0), 192);

        yield return (new(0, 0, 1, 2), 64);
        yield return (new(0, 1, 2, 0), 128);
        yield return (new(1, 2, 0, 0), 192);
        yield return (new(2, 0, 0, 0), 193);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Log2_Data))]
    [Property("Category", "BinaryInteger")]
    public async Task Log2(UInt256 input, UInt256 expected)
    {
        await UInt256.Log2(input).Should().BeEqualTo(expected);
    }


    [Test, MultipleAssertions]
    [Property("Category", "Compare")]
    public async Task Compare()
    {
        await (UInt256.Zero < UInt256.One).Should().BeTrue();
        await (UInt256.Zero < UInt256.MaxValue).Should().BeTrue();
        await (UInt256.One < UInt256.MaxValue).Should().BeTrue();
        await (UInt256.Zero > UInt256.One).Should().BeFalse();
        await (UInt256.Zero > UInt256.MaxValue).Should().BeFalse();
        await (UInt256.One > UInt256.MaxValue).Should().BeFalse();

        await (UInt256.One < UInt256.Zero).Should().BeFalse();
        await (UInt256.MaxValue < UInt256.Zero).Should().BeFalse();
        await (UInt256.MaxValue < UInt256.One).Should().BeFalse();
        await (UInt256.One > UInt256.Zero).Should().BeTrue();
        await (UInt256.MaxValue > UInt256.Zero).Should().BeTrue();
        await (UInt256.MaxValue > UInt256.One).Should().BeTrue();

        var s = new ulong[1000];
        rnd.NextBytes(MemoryMarshal.AsBytes(s.AsSpan()));
        for (; UInt256.ULongCount < s.Length; s = s[1..])
        {
            await Impl(new(s[3], s[2], s[1], s[0]));

            await Impl(new(0, 0, s[1], s[2]));

            await Impl(new(0, 0, 0, s[2]));

            await Impl(new(0, s[2], 0, 0));

            await Impl(new(s[2], 0, 0, 0));
        }

        static async Task Impl(UInt256 value)
        {
            if (value == 0 || value == UInt256.MaxValue) return;

            await (UInt256.Zero < value).Should().BeTrue();
            await (value < UInt256.MaxValue).Should().BeTrue();
            await (UInt256.Zero <= value).Should().BeTrue();
            await (value <= UInt256.MaxValue).Should().BeTrue();

            await (UInt256.Zero > value).Should().BeFalse();
            await (value > UInt256.MaxValue).Should().BeFalse();
            await (UInt256.Zero >= value).Should().BeFalse();
            await (value >= UInt256.MaxValue).Should().BeFalse();

            await (UInt256.Zero != value).Should().BeTrue();
            await (value != UInt256.MaxValue).Should().BeTrue();

            await (UInt256.Zero == value).Should().BeFalse();
            await (value == UInt256.MaxValue).Should().BeFalse();

            await (value - 1 <= value).Should().BeTrue();
            await (value <= value + 1).Should().BeTrue();

            await (value - 1 >= value).Should().BeFalse();
            await (value >= value + 1).Should().BeFalse();
        }
    }
}