using AtCoder.Internal;
using System;
using System.Linq;


namespace Kzrnm.Competitive.Testing.DataStructure;

public class SLazySegtreeTest
{
    private readonly struct Starry : ISLazySegtreeOperator<int, int>
    {
        public int Identity => -1_000_000_000;
        public int FIdentity => 0;

        public int Composition(int f, int g) => f + g;

        public int Mapping(int f, int x, int size) => f + x;

        public int Operate(int x, int y) => Math.Max(x, y);
    }

    [Fact]
    public void Zero()
    {
        new SLazySegtree<int, int, Starry>(0).AllProd.ShouldBe(-1_000_000_000);
        new SLazySegtree<int, int, Starry>(10).AllProd.ShouldBe(-1_000_000_000);
    }

    [Fact]
    public void Invalid()
    {
        var s = new SLazySegtree<int, int, Starry>(10);
        Should.Throw<ContractAssertException>(() => s[-1]);
        Should.Throw<ContractAssertException>(() => s[10]);
        Should.NotThrow(() => s[0]);
        Should.NotThrow(() => s[9]);

        Should.Throw<ContractAssertException>(() => s.Prod(-1, -1));
        Should.Throw<ContractAssertException>(() => s.Prod(3, 2));
        Should.Throw<ContractAssertException>(() => s.Prod(0, 11));
        Should.Throw<ContractAssertException>(() => s.Prod(-1, 11));
        Should.NotThrow(() => s.Prod(0, 0));
        Should.NotThrow(() => s.Prod(10, 10));
        Should.NotThrow(() => s.Prod(0, 10));

        Should.Throw<ContractAssertException>(() => s.MaxRight(11, s => true));
        Should.Throw<ContractAssertException>(() => s.MaxRight(-1, s => true));
        Should.Throw<ContractAssertException>(() => s.MaxRight(0, s => false));
        Should.NotThrow(() => s.MaxRight(0, s => true));
        Should.NotThrow(() => s.MaxRight(10, s => true));

        Should.Throw<ContractAssertException>(() => s.MinLeft(11, s => true));
        Should.Throw<ContractAssertException>(() => s.MinLeft(-1, s => true));
        Should.Throw<ContractAssertException>(() => s.MinLeft(0, s => false));
        Should.NotThrow(() => s.MinLeft(0, s => true));
        Should.NotThrow(() => s.MinLeft(10, s => true));
    }

    [Fact]
    public void NaiveProd()
    {
        for (int n = 0; n <= 50; n++)
        {
            var seg = new SLazySegtree<int, int, Starry>(n);
            var p = new int[n];
            for (int i = 0; i < n; i++)
            {
                p[i] = (i * i + 100) % 31;
                seg[i] = p[i];
            }
            for (int l = 0; l <= n; l++)
            {
                for (int r = l; r <= n; r++)
                {
                    int e = -1_000_000_000;
                    for (int i = l; i < r; i++)
                    {
                        e = Math.Max(e, p[i]);
                    }
                    seg.Prod(l, r).ShouldBe(e);
                    seg[l..r].ShouldBe(e);
                }
            }
        }
    }

    [Fact]
    public void Usage()
    {
        var seg = new SLazySegtree<int, int, Starry>(new int[10]);
        seg.AllProd.ShouldBe(0);
        seg.Apply(0, 3, 5);
        seg.AllProd.ShouldBe(5);
        seg.Apply(2, -10);
        seg.Prod(2, 3).ShouldBe(-5);
        seg[2..3].ShouldBe(-5);
        seg.Prod(2, 4).ShouldBe(0);
        seg[2..4].ShouldBe(0);
    }


    private readonly struct SumOp : ISLazySegtreeOperator<int, Guid>
    {
        public int Identity => 0;
        public Guid FIdentity => Guid.Empty;
        public Guid Composition(Guid nf, Guid cf) => Guid.Empty;
        public int Mapping(Guid f, int x, int size)
        {
            if (x != size)
                throw new InvalidOperationException();
            return x;
        }
        public int Operate(int x, int y) => x + y;
    }

    [Fact]
    public void Size()
    {
        var seg = new SLazySegtree<int, Guid, SumOp>(Enumerable.Repeat(1, 35).ToArray());
        for (int l = 0; l < seg.Length; l++)
            for (int r = l; r <= seg.Length; r++)
            {
                seg.Apply(l, r, Guid.Empty);
                seg[l..r].ShouldBe(r - l);
            }
    }

    [Fact]
    public void ToArray()
    {
        for (int i = 0; i < 20; i++)
        {
            var seg = new SLazySegtree<int, int, Starry>(new int[i]);
            for (int j = 0; j < seg.Length - j; j++)
                seg.Apply(j, seg.Length - j, 1);
            seg.ToArray().ShouldBe(CreateExpected(i));
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
}
