using AtCoder.Internal;
using FluentAssertions;
using System;
using Xunit;


namespace Kzrnm.Competitive.DataStructure.Segtree
{
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
            new SLazySegtree<int, int, Starry>(0).AllProd.Should().Be(-1_000_000_000);
            new SLazySegtree<int, int, Starry>(10).AllProd.Should().Be(-1_000_000_000);
        }

        [Fact]
        public void Invalid()
        {
            var s = new SLazySegtree<int, int, Starry>(10);
            s.Invoking(s => s[-1]).Should().Throw<ContractAssertException>();
            s.Invoking(s => s[10]).Should().Throw<ContractAssertException>();
            s.Invoking(s => s[0]).Should().NotThrow();
            s.Invoking(s => s[9]).Should().NotThrow();

            s.Invoking(s => s.Prod(-1, -1)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(3, 2)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(0, 11)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(-1, 11)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(0, 0)).Should().NotThrow();
            s.Invoking(s => s.Prod(10, 10)).Should().NotThrow();
            s.Invoking(s => s.Prod(0, 10)).Should().NotThrow();

            s.Invoking(s => s.MaxRight(11, s => true)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.MaxRight(-1, s => true)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.MaxRight(0, s => false)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.MaxRight(0, s => true)).Should().NotThrow();
            s.Invoking(s => s.MaxRight(10, s => true)).Should().NotThrow();

            s.Invoking(s => s.MinLeft(11, s => true)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.MinLeft(-1, s => true)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.MinLeft(0, s => false)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.MinLeft(0, s => true)).Should().NotThrow();
            s.Invoking(s => s.MinLeft(10, s => true)).Should().NotThrow();
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
                        seg.Prod(l, r).Should().Be(seg[l..r]).And.Be(e);
                    }
                }
            }
        }

        [Fact]
        public void Usage()
        {
            var seg = new SLazySegtree<int, int, Starry>(new int[10]);
            seg.AllProd.Should().Be(0);
            seg.Apply(0, 3, 5);
            seg.AllProd.Should().Be(5);
            seg.Apply(2, -10);
            seg.Prod(2, 3).Should().Be(seg[2..3]).And.Be(-5);
            seg.Prod(2, 4).Should().Be(seg[2..4]).And.Be(0);
        }
    }
}
