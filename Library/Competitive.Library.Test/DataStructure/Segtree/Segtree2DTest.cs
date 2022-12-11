using AtCoder;
using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class Segtree2DTests
    {
        struct S : ISegtreeOperator<long>
        {
            public long Identity => 0;

            public long Operate(long x, long y) => x + y;
        }
        Random rnd = new(227);
        private long[][] array;
        private Segtree2D<long, S> seg;
        public Segtree2DTests()
        {
            array = Global.NewArray(20, 35, 0L);
            for (int i = 0; i < array.Length; i++)
                for (int j = 0; j < array[i].Length; j++)
                    array[i][j] = rnd.Next();
            seg = new Segtree2D<long, S>(array);
        }

        private void Add(int h, int w, long v)
        {
            array[h][w] += v;
            seg[h, w] += v;
        }
        private void Verify(int h1, int w1, int h2, int w2)
        {
            long native = 0;
            for (int h = h1; h < h2; h++)
                for (int w = w1; w < w2; w++)
                    native += array[h][w];

            seg.Prod(h1, w1, h2, w2).Should().Be(native);
        }

        [Fact]
        public void Test()
        {
            for (int h1 = 0; h1 < array.Length; h1++)
                for (int h2 = h1; h2 <= array.Length; h2++)
                    for (int w1 = 0; w1 < array[0].Length; w1++)
                        for (int w2 = w1; w2 <= array[0].Length; w2++)
                            Verify(h1, w1, h2, w2);

            for (int n = 0; n < 100; n++)
            {
                int h = rnd.Next(array.Length);
                int w = rnd.Next(array[0].Length);
                Add(h, w, rnd.Next());
            }

            for (int h1 = 0; h1 < array.Length; h1++)
                for (int h2 = h1; h2 <= array.Length; h2++)
                    for (int w1 = 0; w1 < array[0].Length; w1++)
                        for (int w2 = w1; w2 <= array[0].Length; w2++)
                            Verify(h1, w1, h2, w2);
        }
    }
}
