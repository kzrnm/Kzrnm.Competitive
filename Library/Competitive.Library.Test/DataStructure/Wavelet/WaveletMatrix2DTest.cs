using AtCoder;
using FluentAssertions;
using System;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class WaveletMatrixRangeSumTests
    {
        [Fact]
        public void Sums()
        {
            var orig = new (int V, long d)[] {
                (4, 0b00000001),
                (3, 0b00000010),
                (2, 0b00000100),
                (1, 0b00001000),
                (2, 0b00010000),
                (3, 0b00100000),
                (4, 0b01000000),
                (5, 0b10000000),
            };
            LongWaveletMatrixWithSums matrix = new(orig);
            for (int l = 0; l < orig.Length; l++)
                for (int r = l + 1; r <= orig.Length; r++)
                    for (int u = 0; u <= 7; u++)
                    {
                        matrix.RectSum(l, r, u).Should().Be(RectSumNative(l, r, u));
                        for (int d = 0; d <= u; d++)
                        {
                            matrix.RectSum(l, r, d, u).Should().Be(RectSumNative2(l, r, d, u));
                        }
                    }
            //matrix.RectSum(0, 1, 

            long RectSumNative(int l, int r, long upper)
            {
                long sum = 0;
                foreach (var (v, d) in orig.AsSpan()[l..r])
                    if (v < upper)
                        sum += d;
                return sum;
            }
            long RectSumNative2(int l, int r, long lower, long upper)
            {
                long sum = 0;
                foreach (var (v, d) in orig.AsSpan()[l..r])
                    if (lower <= v && v < upper)
                        sum += d;
                return sum;
            }
        }

        [Fact]
        public void AddAndSums()
        {
            var orig = new (int V, long d)[] {
                (4, 0b00000001),
                (3, 0b00000010),
                (2, 0b00000100),
                (1, 0b00001000),
                (2, 0b00010000),
                (3, 0b00100000),
                (4, 0b01000000),
                (5, 0b10000000),
            };
            LongWaveletMatrixWithFenwickTree matrix = new(orig);
            for (int l = 0; l < orig.Length; l++)
                for (int r = l + 1; r <= orig.Length; r++)
                    for (int u = 0; u <= 7; u++)
                    {
                        matrix.RectSum(l, r, u).Should().Be(RectSumNative(l, r, u));
                        for (int d = 0; d <= u; d++)
                        {
                            matrix.RectSum(l, r, d, u).Should().Be(RectSumNative2(l, r, d, u));
                        }
                    }

            for (int i = 0; i < orig.Length; i++)
            {
                orig[i].d += 1;
                matrix.PointAdd(i, 1);
            }

            for (int l = 0; l < orig.Length; l++)
                for (int r = l + 1; r <= orig.Length; r++)
                    for (int u = 0; u <= 7; u++)
                    {
                        matrix.RectSum(l, r, u).Should().Be(RectSumNative(l, r, u));
                        for (int d = 0; d <= u; d++)
                        {
                            matrix.RectSum(l, r, d, u).Should().Be(RectSumNative2(l, r, d, u));
                        }
                    }


            long RectSumNative(int l, int r, long upper)
            {
                long sum = 0;
                foreach (var (v, d) in orig.AsSpan()[l..r])
                    if (v < upper)
                        sum += d;
                return sum;
            }
            long RectSumNative2(int l, int r, long lower, long upper)
            {
                long sum = 0;
                foreach (var (v, d) in orig.AsSpan()[l..r])
                    if (lower <= v && v < upper)
                        sum += d;
                return sum;
            }
        }
    }
}

