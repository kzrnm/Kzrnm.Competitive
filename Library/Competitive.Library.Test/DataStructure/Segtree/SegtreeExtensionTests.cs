using AtCoder;
using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure;

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
            seg.ToArray().ShouldBe(Enumerable.Range(0, i));
        }
    }
    readonly struct Op : ISegtreeOperator<int>
    {
        public int Identity => 0;
        public int Operate(int x, int y) => Math.Max(x, y);
    }
}
