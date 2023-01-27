using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class LazyReversibleSplayTreeTests
    {
        private readonly struct Starry : IReversibleBinarySearchTreeOperator<int, int>
        {
            public int Identity => -1_000_000_000;
            public int FIdentity => 0;

            public int Composition(int f, int g) => f + g;

            public int Inverse(int v) => v;
            public int Mapping(int f, int x, int size) => f + x;
            public int Operate(int x, int y) => Math.Max(x, y);
        }

        [Fact]
        public void Zero()
        {
            new LazyReversibleSplayTree<int, int, Starry>().AllProd.Should().Be(-1_000_000_000);
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
                var tree = new LazyReversibleSplayTree<int, int, Starry>(p);
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
            var tree = new LazyReversibleSplayTree<int, int, Starry>(new int[10]);
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

        [Fact]
        public void Reverse()
        {
            const int N = 8;
            var tree = new LazyReversibleSplayTree<int>(Enumerable.Range(0, N));
            for (int i = 0; i < N; i++)
                tree[i].Should().Be(i);
            tree.Reverse(2, 5);
            var expected = new[] { 0, 1, 4, 3, 2, 5, 6, 7 };
            tree.Should().Equal(expected);
            for (int i = 0; i < N; i++)
                tree[i].Should().Be(expected[i]);


            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                int l = rnd.Next(N);
                int r = rnd.Next(l + 1, N + 1);
                tree.Reverse(l, r);
                expected.AsSpan()[l..r].Reverse();
                tree.Should().Equal(expected);
            }
        }

        [Fact]
        public void InsertAndReverse()
        {
            const int N = 8;
            var list = Enumerable.Range(0, N).ToList();
            var tree = new LazyReversibleSplayTree<int>(list);
            void Insert(int index, int value)
            {
                tree.Insert(index, value);
                list.Insert(index, value);
                Test();
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < N; i++)
                    tree[i].Should().Be(list[i]);
            }

            for (int i = 0; i < N; i++)
                tree[i].Should().Be(list[i]);

            tree.Add(-1);
            list.Add(-1);
            Test();

            Insert(2, -2);
            Insert(0, -5);
            Insert(N, 111);

            tree.Reverse(2, 5);
            list.AsSpan()[2..5].Reverse();

            Insert(2, -12);
            Insert(0, -15);
            Insert(N, 1111);
        }
    }
}
