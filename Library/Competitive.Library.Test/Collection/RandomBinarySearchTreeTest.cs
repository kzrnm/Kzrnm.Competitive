using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class RandomBinarySearchTreeTests
    {
        private readonly struct Starry : IRandomBinarySearchTreeOperator<int, int>
        {
            public int Identity => -1_000_000_000;
            public int FIdentity => 0;

            public int Composition(int f, int g) => f + g;

            public int Inverse(int v) => v;
            public int Mapping(int f, int x) => f + x;

            public int Operate(int x, int y) => Math.Max(x, y);
        }

        [Fact]
        public void Zero()
        {
            new RandomBinarySearchTree<int, int, Starry>().AllProd.Should().Be(-1_000_000_000);
        }

        [Fact]
        public void NaiveProd()
        {
            for (int n = 0; n <= 50; n++)
            {
                var p = new int[n];
                for (int i = 0; i < n; i++)
                {
                    p[i] = (i * i + 100) % 31;
                }
                var tree = new RandomBinarySearchTree<int, int, Starry>(p);
                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        int e = -1_000_000_000;
                        for (int i = l; i < r; i++)
                        {
                            e = Math.Max(e, p[i]);
                        }
                        tree.Prod(l, r).Should().Be(tree[l..r]).And.Be(e);
                    }
                }
                tree.Should().Equal(p);
                for (int i = 0; i < n; i++)
                    tree[i..(i + 1)].Should().Be(p[i]);
            }
        }

        [Fact]
        public void Usage()
        {
            var tree = new RandomBinarySearchTree<int, int, Starry>(new int[10]);
            tree.AllProd.Should().Be(0);
            tree.Should().Equal(new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            tree.Apply(0, 3, 5);
            tree.Should().Equal(new[] { 5, 5, 5, 0, 0, 0, 0, 0, 0, 0 });
            tree.Prod(0, 1).Should().Be(5);
            tree.AllProd.Should().Be(5);
            tree.Apply(2, 3, -10);
            tree.Should().Equal(new[] { 5, 5, -5, 0, 0, 0, 0, 0, 0, 0 });
            tree.Prod(2, 3).Should().Be(tree[2..3]).And.Be(-5);
            tree.Prod(2, 4).Should().Be(tree[2..4]).And.Be(0);
        }
    }
}
