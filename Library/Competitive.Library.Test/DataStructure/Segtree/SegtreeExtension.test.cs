using AtCoder;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    // verification-helper: SAMEAS Library/run.test.py
    public class SegtreeExtensionTests
    {
        [Fact]
        public void ToArray()
        {
            for (int i = 0; i < 20; i++)
            {
                var seg = new Segtree<int, Op>(i);
                for (int j = 0; j < i; j++)
                    seg[j] = j;
                seg.ToArray().Should().Equal(Enumerable.Range(0, i));
            }
        }
        struct Op : ISegtreeOperator<int>
        {
            public int Identity => 0;
            public int Operate(int x, int y) => Math.Max(x, y);
        }
    }
}
