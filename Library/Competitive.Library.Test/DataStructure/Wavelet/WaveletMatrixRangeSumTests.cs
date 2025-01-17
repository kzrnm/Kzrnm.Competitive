using System;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class WaveletMatrix2DTests
    {
        [Fact]
        public void Sums()
        {
            var rnd = new Random(227);
            var orig = new long[8, 12];
            var pts = new ((int, int), long)[100000];
            for (int i = 0; i < pts.Length; i++)
            {
                int x = rnd.Next(orig.GetLength(0));
                int y = rnd.Next(orig.GetLength(1));
                long w = rnd.Next();
                orig[x, y] += w;
                pts[i] = ((x, y), w);
            }
            LongWaveletMatrix2DWithSums matrix = new(pts);
            for (int l = 0; l < orig.GetLength(0); l++)
                for (int r = l + 1; r <= orig.GetLength(0); r++)
                    for (int u = 0; u < orig.GetLength(1); u++)
                    {
                        matrix.RectSum(l, r, u).ShouldBe(RectSumNative(l, r, u));
                        for (int d = 0; d <= u; d++)
                        {
                            matrix.RectSum(l, r, d, u).ShouldBe(RectSumNative2(l, r, d, u));
                        }
                    }


            long RectSumNative(int l, int r, int upper)
            {
                long sum = 0;
                for (int i = l; i < r; i++)
                    for (int j = 0; j < upper; j++)
                        sum += orig[i, j];
                return sum;
            }
            long RectSumNative2(int l, int r, int lower, int upper)
            {
                long sum = 0;
                for (int i = l; i < r; i++)
                    for (int j = lower; j < upper; j++)
                        sum += orig[i, j];
                return sum;
            }
        }

        [Fact]
        public void AddAndSums()
        {
            var rnd = new Random(227);
            var orig = new long[8, 12];
            var pts = new ((int, int), long)[100000];
            for (int i = 0; i < pts.Length; i++)
            {
                int x = rnd.Next(orig.GetLength(0));
                int y = rnd.Next(orig.GetLength(1));
                long w = rnd.Next();
                orig[x, y] += w;
                pts[i] = ((x, y), w);
            }
            LongWaveletMatrix2DWithFenwickTree matrix = new(pts);
            for (int l = 0; l < orig.GetLength(0); l++)
                for (int r = l + 1; r <= orig.GetLength(0); r++)
                    for (int u = 0; u < orig.GetLength(1); u++)
                    {
                        matrix.RectSum(l, r, u).ShouldBe(RectSumNative(l, r, u));
                        for (int d = 0; d <= u; d++)
                        {
                            matrix.RectSum(l, r, d, u).ShouldBe(RectSumNative2(l, r, d, u));
                        }
                    }
            for (int i = 0; i < 2000; i++)
            {
                int x = rnd.Next(orig.GetLength(0));
                int y = rnd.Next(orig.GetLength(1));
                int w = rnd.Next();
                orig[x, y] += w;
                matrix.PointAdd(x, y, w);
            }

            for (int l = 0; l < orig.GetLength(0); l++)
                for (int r = l + 1; r <= orig.GetLength(0); r++)
                    for (int u = 0; u < orig.GetLength(1); u++)
                    {
                        matrix.RectSum(l, r, u).ShouldBe(RectSumNative(l, r, u));
                        for (int d = 0; d <= u; d++)
                        {
                            matrix.RectSum(l, r, d, u).ShouldBe(RectSumNative2(l, r, d, u));
                        }
                    }

            long RectSumNative(int l, int r, int upper)
            {
                long sum = 0;
                for (int i = l; i < r; i++)
                    for (int j = 0; j < upper; j++)
                        sum += orig[i, j];
                return sum;
            }
            long RectSumNative2(int l, int r, int lower, int upper)
            {
                long sum = 0;
                for (int i = l; i < r; i++)
                    for (int j = lower; j < upper; j++)
                        sum += orig[i, j];
                return sum;
            }
        }
    }
}

