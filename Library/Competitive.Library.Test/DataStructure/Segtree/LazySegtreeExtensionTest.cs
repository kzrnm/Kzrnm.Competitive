using AtCoder;
using FluentAssertions;
using System;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class LazySegtreeExtensionTests
    {
        [Fact]
        public void ToArray()
        {
            for (int i = 0; i < 20; i++)
            {
                var seg = new LazySegtree<int, int, Op>(i);
                for (int j = 0; j < seg.Length - j; j++)
                    seg.Apply(j, seg.Length - j, 1);
                seg.ToArray().Should().Equal(CreateExpected(i));
            }

            static int[] CreateExpected(int length)
            {
                var result = new int[length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = Math.Min(i + 1, length - i);
                }
                return result;
            }
        }
        struct Op : ILazySegtreeOperator<int, int>
        {
            public int Identity => 0;
            public int FIdentity => 0;

            public int Composition(int nf, int cf) => nf + cf;

            public int Mapping(int f, int x) => f + x;
            public int Operate(int x, int y) => Math.Max(x, y);
        }
    }
}
