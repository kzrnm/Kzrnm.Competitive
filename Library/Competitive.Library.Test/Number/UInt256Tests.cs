using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Number
{
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

        [Fact]
        [Trait("Category", "Convert")]
        public void ConvertToBigInteger()
        {
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    CreateBigInteger(s[3], s[2], s[1], s[0]));

                Impl(
                    new(0, 0, s[1], s[0]),
                    CreateBigInteger(0, 0, s[1], s[0]));

                Impl(
                    new(0, 0, 0, s[0]),
                    CreateBigInteger(0, 0, 0, s[0]));
            }
            static void Impl(UInt256 value, BigInteger bi)
            {
                ((BigInteger)value).Should().Be(bi);
            }
        }

        [Fact]
        [Trait("Category", "Convert")]
        public void ConvertTo()
        {
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                UInt256 value;
                checked
                {
                    value = new(0, 0, s[1], s[0]);
                    var other128 = new UInt128(s[1], s[0]);
                    ((UInt128)value).Should().Be(other128);

                    value = new(0, 0, 0, s[0]);
                    var other64 = s[0];
                    ((UInt64)value).Should().Be(other64);


                    ulong s0 = s[0];

                    s0 &= uint.MaxValue;
                    value = new(0, 0, 0, s0);
                    var other32 = (UInt32)s0;
                    ((UInt32)value).Should().Be(other32);

                    s0 &= ushort.MaxValue;
                    value = new(0, 0, 0, s0);
                    var other16 = (UInt16)s0;
                    ((UInt16)value).Should().Be(other16);

                    s0 &= byte.MaxValue;
                    value = new(0, 0, 0, s0);
                    var other8 = (byte)s0;
                    ((byte)value).Should().Be(other8);
                }
                unchecked
                {
                    value = new(0, 0, 0, s[0]);
                    var other32 = (UInt32)s[0];
                    ((UInt32)value).Should().Be(other32);

                    value = new(0, 0, 0, s[0]);
                    var other16 = (UInt16)s[0];
                    ((UInt16)value).Should().Be(other16);

                    value = new(0, 0, 0, s[0]);
                    var other8 = (byte)s[0];
                    ((byte)value).Should().Be(other8);
                }
            }

            checked
            {
                UInt256 value = ulong.MaxValue;
                value.Invoking(v => _ = (UInt32)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (UInt16)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (byte)v).Should().Throw<OverflowException>();

                value = new(0, 0, 1, 0);
                value.Invoking(v => _ = (UInt64)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (UInt32)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (UInt16)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (byte)v).Should().Throw<OverflowException>();

                value = new(0, 1, 0, 0);
                value.Invoking(v => _ = (UInt128)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (UInt64)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (UInt32)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (UInt16)v).Should().Throw<OverflowException>();
                value.Invoking(v => _ = (byte)v).Should().Throw<OverflowException>();
            }
        }

        [Fact]
        [Trait("Category", "Convert")]
        public void ConvertFromBigInteger()
        {
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    CreateBigInteger(s[3], s[2], s[1], s[0]));

                Impl(
                    new(0, 0, s[1], s[0]),
                    CreateBigInteger(0, 0, s[1], s[0]));

                Impl(
                    new(0, 0, 0, s[0]),
                    CreateBigInteger(0, 0, 0, s[0]));

                Impl(
                    new(s[2], s[1], s[0], 0),
                    CreateBigInteger(s[3], s[2], s[1], s[0]) << 64);
            }
            static void Impl(UInt256 value, BigInteger bi)
            {
                ((UInt256)bi).Should().Be(value);
            }
        }

        [Fact]
        [Trait("Category", "Convert")]
        public void ConvertFrom()
        {
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                UInt256 value;
                value = new(0, 0, s[1], s[0]);
                UInt256 other128 = new UInt128(s[1], s[0]);
                other128.Should().Be(value);

                value = new(0, 0, 0, s[0]);
                UInt256 other64 = s[0];
                other64.Should().Be(value);

                value = new(0, 0, 0, (uint)s[0]);
                UInt256 other32 = (UInt32)s[0];
                other32.Should().Be(value);

                value = new(0, 0, 0, (ushort)s[0]);
                UInt256 other16 = (UInt16)s[0];
                other16.Should().Be(value);

                value = new(0, 0, 0, (byte)s[0]);
                UInt256 other8 = (byte)s[0];
                other8.Should().Be(value);
            }
        }

        public static IEnumerable<(string, UInt256)> Parse_Data()
        {
            yield return ("1", (UInt256)1);
            yield return (new BigInteger(1e60).ToString(), (UInt256)new BigInteger(1e60));
            yield return ("100", (UInt256)100);
        }

        [Theory]
        [TupleMemberData(nameof(Parse_Data))]
        [Trait("Category", "String")]
        public void Parse(string input, UInt256 expected)
        {
            UInt256.Parse(input).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "String")]
        public void String()
        {
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(new(s[3], s[2], s[1], s[0]));

                Impl(new(0, 0, s[1], s[0]));

                Impl(new(0, 0, 0, s[0]));
            }

            static void Impl(UInt256 value)
            {
                var bi = (BigInteger)value;
                value.ToString().Should().Be(bi.ToString());
            }
        }


        [Fact]
        [Trait("Category", "Arthmetic")]
        public void Add()
        {
            Impl(UInt256.Zero, UInt256.Zero);
            Impl(UInt256.Zero, UInt256.One);
            Impl(UInt256.Zero, UInt256.MaxValue);

            Impl(UInt256.One, UInt256.One);
            Impl(UInt256.One, UInt256.MaxValue);

            Impl(UInt256.MaxValue, UInt256.MaxValue);

            for (int i = 0; i < 4; i++)
            {
                Impl((UInt256)(BigInteger.One << (64 * i)), 1);
                Impl((UInt256)((BigInteger.One << (64 * i)) - 1), 1);
            }

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(s[1], s[0], s[2], s[3]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, s[1], s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, 0, s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.One);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.Zero);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.MaxValue);
            }

            static void Impl(UInt256 value0, UInt256 value1)
            {
                var bi0 = (BigInteger)value0;
                var bi1 = (BigInteger)value1;
                ((BigInteger)(value0 + value1)).Should().Be(biMask & (bi0 + bi1));
                ((BigInteger)(value1 + value0)).Should().Be(biMask & (bi0 + bi1));
            }
        }

        [Fact]
        [Trait("Category", "Arthmetic")]
        public void Minus()
        {
            Impl(UInt256.Zero, UInt256.Zero);
            Impl(UInt256.Zero, UInt256.One);
            Impl(UInt256.Zero, UInt256.MaxValue);

            Impl(UInt256.One, UInt256.One);
            Impl(UInt256.One, UInt256.MaxValue);

            Impl(UInt256.MaxValue, UInt256.MaxValue);

            for (int i = 0; i < 4; i++)
            {
                Impl((UInt256)(BigInteger.One << (64 * i)), 1);
                Impl((UInt256)((BigInteger.One << (64 * i)) - 1), 1);
            }

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(s[1], s[0], s[2], s[3]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, s[1], s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, 0, s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.One);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.Zero);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.MaxValue);
            }

            static void Impl(UInt256 value0, UInt256 value1)
            {
                var bi0 = (BigInteger)value0;
                var bi1 = (BigInteger)value1;
                ((BigInteger)(value0 - value1)).Should().Be(biMask & (bi0 - bi1));
                ((BigInteger)(value1 - value0)).Should().Be(biMask & (bi1 - bi0));
            }
        }

        [Fact]
        [Trait("Category", "Arthmetic")]
        public void Multiply()
        {
            Impl(UInt256.Zero, UInt256.Zero);
            Impl(UInt256.Zero, UInt256.One);
            Impl(UInt256.Zero, UInt256.MaxValue);

            Impl(UInt256.One, UInt256.One);
            Impl(UInt256.One, UInt256.MaxValue);

            Impl(UInt256.MaxValue, UInt256.MaxValue);

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(s[1], s[0], s[2], s[3]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, s[1], s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, 0, s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.One);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.Zero);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.MaxValue);
            }

            static void Impl(UInt256 value0, UInt256 value1)
            {
                var bi0 = (BigInteger)value0;
                var bi1 = (BigInteger)value1;
                ((BigInteger)(value0 * value1)).Should().Be(biMask & (bi0 * bi1));
                ((BigInteger)(value1 * value0)).Should().Be(biMask & (bi0 * bi1));
            }
        }

        [Fact]
        [Trait("Category", "Arthmetic")]
        public void Divide()
        {
            Impl(UInt256.Zero, UInt256.Zero);
            Impl(UInt256.Zero, UInt256.One);
            Impl(UInt256.Zero, UInt256.MaxValue);

            Impl(UInt256.One, UInt256.One);
            Impl(UInt256.One, UInt256.MaxValue);

            Impl(UInt256.MaxValue, UInt256.MaxValue);

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(s[1], s[0], s[2], s[3]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, s[1], s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, 0, s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.One);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.Zero);

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    UInt256.MaxValue);
            }

            static void Impl(UInt256 value0, UInt256 value1)
            {
                var bi0 = (BigInteger)value0;
                var bi1 = (BigInteger)value1;
                if (value1 != 0) ((BigInteger)(value0 / value1)).Should().Be(biMask & (bi0 / bi1));
                if (value0 != 0) ((BigInteger)(value1 / value0)).Should().Be(biMask & (bi1 / bi0));
            }
        }

        [Fact]
        [Trait("Category", "Arthmetic")]
        public void SingleMinus()
        {
            Impl(UInt256.Zero);
            Impl(UInt256.One);
            Impl(UInt256.MaxValue);
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(new(s[3], s[2], s[1], s[0]));

                Impl(new(0, 0, s[1], s[2]));

                Impl(new(0, 0, 0, s[2]));
            }

            static void Impl(UInt256 value)
            {
                var bi = (BigInteger)value;
                if (bi != 0) ((BigInteger)(-value)).Should().Be((bi ^ biMask) + 1);
            }
        }

        [Fact]
        [Trait("Category", "Bitwise")]
        public void And()
        {
            Impl(UInt256.Zero, UInt256.Zero);
            Impl(UInt256.Zero, UInt256.One);
            Impl(UInt256.Zero, UInt256.MaxValue);

            Impl(UInt256.One, UInt256.One);
            Impl(UInt256.One, UInt256.MaxValue);

            Impl(UInt256.MaxValue, UInt256.MaxValue);

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(s[1], s[0], s[2], s[3]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, s[1], s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, 0, s[2]));
            }

            static void Impl(UInt256 value0, UInt256 value1)
            {
                var bi0 = (BigInteger)value0;
                var bi1 = (BigInteger)value1;
                ((BigInteger)(value0 & value1)).Should().Be(bi0 & bi1);
                ((BigInteger)(value1 & value0)).Should().Be(bi0 & bi1);
            }
        }

        [Fact]
        [Trait("Category", "Bitwise")]
        public void Or()
        {
            Impl(UInt256.Zero, UInt256.Zero);
            Impl(UInt256.Zero, UInt256.One);
            Impl(UInt256.Zero, UInt256.MaxValue);

            Impl(UInt256.One, UInt256.One);
            Impl(UInt256.One, UInt256.MaxValue);

            Impl(UInt256.MaxValue, UInt256.MaxValue);

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(s[1], s[0], s[2], s[3]));
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, s[1], s[2]));
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, 0, s[2]));
            }

            static void Impl(UInt256 value0, UInt256 value1)
            {
                var bi0 = (BigInteger)value0;
                var bi1 = (BigInteger)value1;
                ((BigInteger)(value0 | value1)).Should().Be(bi0 | bi1);
                ((BigInteger)(value1 | value0)).Should().Be(bi0 | bi1);
            }
        }

        [Fact]
        [Trait("Category", "Bitwise")]
        public void Xor()
        {
            Impl(UInt256.Zero, UInt256.Zero);
            Impl(UInt256.Zero, UInt256.One);
            Impl(UInt256.Zero, UInt256.MaxValue);

            Impl(UInt256.One, UInt256.One);
            Impl(UInt256.One, UInt256.MaxValue);

            Impl(UInt256.MaxValue, UInt256.MaxValue);

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(s[1], s[0], s[2], s[3]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, s[1], s[2]));

                Impl(
                    new(s[3], s[2], s[1], s[0]),
                    new(0, 0, 0, s[2]));
            }

            static void Impl(UInt256 value0, UInt256 value1)
            {
                var bi0 = (BigInteger)value0;
                var bi1 = (BigInteger)value1;
                ((BigInteger)(value0 ^ value1)).Should().Be(bi0 ^ bi1);
                ((BigInteger)(value1 ^ value0)).Should().Be(bi0 ^ bi1);
            }
        }

        [Fact]
        [Trait("Category", "Bitwise")]
        public void Not()
        {
            Impl(UInt256.Zero);
            Impl(UInt256.One);
            Impl(UInt256.MaxValue);
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(new(s[3], s[2], s[1], s[0]));
                Impl(new(0, 0, s[1], s[2]));
                Impl(new(0, 0, 0, s[2]));
            }

            static void Impl(UInt256 value)
            {
                var bi = (BigInteger)value;
                ((BigInteger)~value).Should().Be(bi ^ biMask);
            }
        }


        [Fact]
        [Trait("Category", "Shift")]
        public void LeftShift()
        {
            for (int i = 0; i < UInt256.Size * 8; i++)
            {
                Impl(UInt256.Zero, i);
                Impl(UInt256.One, i);
                Impl(UInt256.MaxValue, i);
            }
            var s = new ulong[80].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                for (int i = 0; i < UInt256.Size * 8; i++)
                {
                    Impl(new(s[3], s[2], s[1], s[0]), i);

                    Impl(new(0, 0, s[1], s[2]), i);

                    Impl(new(0, 0, 0, s[2]), i);
                }
            }

            static void Impl(UInt256 value, int shift)
            {
                var bi = (BigInteger)value;
                ((BigInteger)(value << shift)).Should().Be((bi << shift) & biMask);
            }
        }
        [Fact]
        [Trait("Category", "Shift")]
        public void RightShift()
        {
            for (int i = 0; i < UInt256.Size * 8; i++)
            {
                Impl(UInt256.Zero, i);
                Impl(UInt256.One, i);
                Impl(UInt256.MaxValue, i);
            }
            var s = new ulong[80].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                for (int i = 0; i < UInt256.Size * 8; i++)
                {
                    Impl(new(s[3], s[2], s[1], s[0]), i);

                    Impl(new(0, 0, s[1], s[2]), i);

                    Impl(new(0, 0, 0, s[2]), i);
                }
            }

            static void Impl(UInt256 value, int shift)
            {
                var bi = (BigInteger)value;
                ((BigInteger)(value >> shift)).Should().Be((bi >> shift) & biMask);
            }
        }

        [Fact]
        [Trait("Category", "BinaryInteger")]
        public void LeadingZeroCount()
        {
            Impl(UInt256.Zero);
            Impl(UInt256.One);
            Impl(UInt256.MaxValue);
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(new(s[3], s[2], s[1], s[0]));

                Impl(new(0, 0, s[1], s[2]));

                Impl(new(0, 0, 0, s[2]));

                Impl(new(0, s[2], 0, 0));

                Impl(new(s[2], 0, 0, 0));
            }

            static void Impl(UInt256 value)
            {
                UInt256.LeadingZeroCount(value).Should().Be((UInt256)Naive(value));
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

        [Fact]
        [Trait("Category", "BinaryInteger")]
        public void PopCount()
        {
            Impl(UInt256.Zero);
            Impl(UInt256.One);
            Impl(UInt256.MaxValue);
            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(new(s[3], s[2], s[1], s[0]));

                Impl(new(0, 0, s[1], s[2]));

                Impl(new(0, 0, 0, s[2]));

                Impl(new(0, s[2], 0, 0));

                Impl(new(s[2], 0, 0, 0));
            }

            static void Impl(UInt256 value)
            {
                UInt256.PopCount(value).Should().Be((UInt256)Naive(value));
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

        [Theory]
        [TupleMemberData(nameof(TrailingZeroCount_Data))]
        [Trait("Category", "BinaryInteger")]
        public void TrailingZeroCount(UInt256 input, UInt256 expected)
        {
            UInt256.TrailingZeroCount(input).Should().Be(expected);
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

        [Theory]
        [TupleMemberData(nameof(Log2_Data))]
        [Trait("Category", "BinaryInteger")]
        public void Log2(UInt256 input, UInt256 expected)
        {
            UInt256.Log2(input).Should().Be(expected);
        }


        [Fact]
        [Trait("Category", "Compare")]
        public void Compare()
        {
            (UInt256.Zero < UInt256.One).Should().BeTrue();
            (UInt256.Zero < UInt256.MaxValue).Should().BeTrue();
            (UInt256.One < UInt256.MaxValue).Should().BeTrue();
            (UInt256.Zero > UInt256.One).Should().BeFalse();
            (UInt256.Zero > UInt256.MaxValue).Should().BeFalse();
            (UInt256.One > UInt256.MaxValue).Should().BeFalse();

            (UInt256.One < UInt256.Zero).Should().BeFalse();
            (UInt256.MaxValue < UInt256.Zero).Should().BeFalse();
            (UInt256.MaxValue < UInt256.One).Should().BeFalse();
            (UInt256.One > UInt256.Zero).Should().BeTrue();
            (UInt256.MaxValue > UInt256.Zero).Should().BeTrue();
            (UInt256.MaxValue > UInt256.One).Should().BeTrue();

            var s = new ulong[1000].AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(s));
            for (; UInt256.ULongCount < s.Length; s = s[1..])
            {
                Impl(new(s[3], s[2], s[1], s[0]));

                Impl(new(0, 0, s[1], s[2]));

                Impl(new(0, 0, 0, s[2]));

                Impl(new(0, s[2], 0, 0));

                Impl(new(s[2], 0, 0, 0));
            }

            static void Impl(UInt256 value)
            {
                if (value == 0 || value == UInt256.MaxValue) return;

                (UInt256.Zero < value).Should().BeTrue();
                (value < UInt256.MaxValue).Should().BeTrue();
                (UInt256.Zero <= value).Should().BeTrue();
                (value <= UInt256.MaxValue).Should().BeTrue();

                (UInt256.Zero > value).Should().BeFalse();
                (value > UInt256.MaxValue).Should().BeFalse();
                (UInt256.Zero >= value).Should().BeFalse();
                (value >= UInt256.MaxValue).Should().BeFalse();

                (UInt256.Zero != value).Should().BeTrue();
                (value != UInt256.MaxValue).Should().BeTrue();

                (UInt256.Zero == value).Should().BeFalse();
                (value == UInt256.MaxValue).Should().BeFalse();

                (value - 1 <= value).Should().BeTrue();
                (value <= value + 1).Should().BeTrue();

                (value - 1 >= value).Should().BeFalse();
                (value >= value + 1).Should().BeFalse();
            }
        }
    }
}
