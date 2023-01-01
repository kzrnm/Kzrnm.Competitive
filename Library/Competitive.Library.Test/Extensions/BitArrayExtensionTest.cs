using System;
using System.Collections;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class BitArrayExtensionTests
    {
        [Fact]
        public void GetUnsageIntArray()
        {
            var b = new BitArray(70);
            b.SetAll(true);
            b[0] = false;
            b[64] = false;

            var arr = b.GetArray();
            arr.Should().HaveCount(3)
                .And
                .StartWith(new int[] { -2, -1 });
            (arr[2] & 0b111111).Should().Be(0b111110);
        }

        [Fact]
        public void EmptyCopyTo()
        {
            var b = new BitArray(0);
            b.CopyTo(Array.Empty<int>());
        }
        [Fact]
        public void CopyToInt()
        {
            var arr = new int[4];
            var b = new BitArray(100);
            b[0] = true;
            b[32] = true;
            b[33] = true;
            b[32 * 3 - 1] = true;
            b[99] = true;
            b.CopyTo(arr);
            arr.Should().Equal(1, 3, int.MinValue, 8);
        }
        [Fact]
        public void CopyToUInt()
        {
            var arr = new uint[4];
            var b = new BitArray(100);
            b[0] = true;
            b[32] = true;
            b[33] = true;
            b[32 * 3 - 1] = true;
            b[99] = true;
            b.CopyTo(arr);
            arr.Should().Equal(1, 3, ((uint)int.MaxValue) + 1, 8);
        }
        [Fact]
        public void CopyToBool()
        {
            var arr = new bool[100];
            var b = new BitArray(100);
            var exp = new bool[100];
            b[0] = true;
            b[32] = true;
            b[33] = true;
            b[32 * 3 - 1] = true;
            b[99] = true;
            b.CopyTo(arr);

            exp[0] = true;
            exp[32] = true;
            exp[33] = true;
            exp[32 * 3 - 1] = true;
            exp[99] = true;
            arr.Should().Equal(exp);
        }
        [Fact]
        public void ToUInt32Array()
        {
            var b = new BitArray(100);
            b[0] = true;
            b[32] = true;
            b[33] = true;
            b[32 * 3 - 1] = true;
            b[99] = true;
            b.ToUInt32Array().Should().Equal(1, 3, ((uint)int.MaxValue) + 1, 8);
        }

        [Fact]
        public void OnBits()
        {
            var b = new BitArray(100);
            b[0] = true;
            b[32] = true;
            b[33] = true;
            b[32 * 3 - 1] = true;
            b[99] = true;
            b.OnBits().Should().Equal(0, 32, 33, 95, 99);

            b.Length = 129;
            b[128] = true;
            b.OnBits().Should().Equal(0, 32, 33, 95, 99, 128);
        }

        [Fact]
        public void OnBitRandom()
        {
            var rnd = new Random(227);
            for (int len = 1; len < 5000; len *= 3)
            {
                var brr = new byte[len];
                rnd.NextBytes(brr);
                var b = new BitArray(brr);
                var bbs = b.OnBits().ToArray().AsSpan();
                for (int i = 0; i < b.Length; i++)
                {
                    if (bbs.Length > 0 && bbs[0] == i)
                    {
                        b[i].Should().BeTrue();
                        bbs = bbs[1..];
                    }
                    else
                        b[i].Should().BeFalse();
                }
                bbs.Length.Should().Be(0);
            }
        }

        [Theory]
        [InlineData(63)]
        [InlineData(64)]
        [InlineData(65)]
        [InlineData(100)]
        public void PopCount(int size)
        {
            var rnd = new Random(227);
            var b = new BitArray(size);
            b.PopCount().Should().Be(0);
            b[size - 1] = true;
            b.PopCount().Should().Be(1);
            for (int n = 0; n < 300; n++)
            {
                var ix = rnd.Next(size);
                b[ix] = true;
                var c = 0;
                for (int i = 0; i < size; i++)
                    if (b[i])
                        ++c;
                b.PopCount().Should().Be(c);
            }
        }

        public static TheoryData LeadingZeroCount_Data = new TheoryData<BitArray, int>
        {
            {
                new BitArray(
                    Enumerable.Repeat(false,16).Concat(
                        Enumerable.Repeat(true,2)
                    ).ToArray()
                ), 16
            },
            {
                new BitArray(
                    Enumerable.Repeat(false,16).Concat(
                        Enumerable.Repeat(true,2)
                    ).Concat(
                        Enumerable.Repeat(false,2)
                    ).Concat(
                        Enumerable.Repeat(true,2)
                    ).ToArray()
                ), 16
            },
        };

        [Theory]
        [MemberData(nameof(LeadingZeroCount_Data))]
        public void LeadingZeroCount(BitArray b, int lsb)
        {
            b.LeadingZeroCount().Should().Be(lsb);
        }
    }
}
