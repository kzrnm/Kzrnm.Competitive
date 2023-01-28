using Kzrnm.Competitive;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class LazyRedBlackTreeTests
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
            new LazyRedBlackTree<int, int, Starry>().AllProd.Should().Be(-1_000_000_000);
        }

        [Fact]
        public void NaiveProd()
        {
            for (int n = 0; n <= 50; n++)
            {
                var p = new int[n];
                for (int i = 0; i < n; i++)
                    p[i] = (i * i + 100) % 31;
                var tree = new LazyRedBlackTree<int, int, Starry>(p);
                var expected = new SLazySegtree<int, int, Starry>(p);

                void Test()
                {
                    for (int l = 0; l <= n; l++)
                        for (int r = l; r <= n; r++)
                            tree.Prod(l, r).Should().Be(tree[l..r]).And.Be(expected[l..r]);
                    for (int i = 0; i < n; i++)
                    {
                        tree[i..(i + 1)].Should().Be(expected[i]);
                        tree[i].Should().Be(expected[i]);
                    }
                }
                tree.Should().Equal(p);
                Test();


                for (int i = 0; i < n; i++)
                {
                    var x = ((i << 5) + (i >> 2) + (i << 3) + i % 3) % 51;
                    tree[i] = expected[i] = x;
                }
                Test();

                for (int l = 0; l < n; l++)
                    for (int r = l; r <= n; r++)
                    {
                        var x = ((l << 5) + (l >> 2) + (r << 3) + r % 3) % 51;
                        tree.Apply(l, r, x);
                        expected.Apply(l, r, x);
                    }
                Test();
            }
        }

        [Fact]
        public void Usage()
        {
            var tree = new LazyRedBlackTree<int, int, Starry>(new int[10]);
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
            var tree = new LazyRedBlackTree<int>(Enumerable.Range(0, N));
            for (int i = 0; i < N; i++)
                tree[i].Should().Be(i);
            tree.Reverse(2, 5);
            var expected = new[] { 0, 1, 4, 3, 2, 5, 6, 7 };
            tree.Reverse();
            expected.AsSpan().Reverse();

            void Test()
            {
                tree.Should().Equal(expected);
                for (int i = 0; i < N; i++)
                    tree[i].Should().Be(expected[i]);
            }
            void Reverse(int l, int r)
            {
                tree.Reverse(l, r);
                expected.AsSpan()[l..r].Reverse();
            }

            Test();

            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                int l = rnd.Next(N);
                int r = rnd.Next(l + 1, N + 1);
                Reverse(l, r);
                Test();
            }
        }

        [Fact]
        public void InsertAndReverse()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = new LazyRedBlackTree<int>(list);
            void Insert(int index, int value)
            {
                tree.Insert(index, value);
                list.Insert(index, value);
            }
            void Reverse(int l, int r)
            {
                tree.Reverse(l, r);
                list.AsSpan()[l..r].Reverse();
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            tree.Add(-1);
            list.Add(-1);
            Test();

            Insert(2, -2);
            Test();
            Insert(0, -5);
            Insert(list.Count, 111);
            Test();

            Reverse(2, 5);
            Test();

            Insert(2, -12);
            Test();
            Insert(0, -15);
            Insert(list.Count, 1111);
            Test();

            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                int l = rnd.Next(list.Count);
                if (rnd.Next(2) == 0)
                    Reverse(l, rnd.Next(l, list.Count) + 1);
                else
                    Insert(l, rnd.Next(list.Count) - 100);
                Test();
            }
        }

        [Fact]
        public void AddAndSetValue()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = new LazyRedBlackTree<int>(list);
            void Add(int value)
            {
                tree.Add(value);
                list.Add(value);
            }
            void SetValue(int index, int value)
            {
                tree[index] = value;
                list[index] = value;
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            Add(-1);
            Test();

            SetValue(2, -2);
            Test();
            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                int l = rnd.Next(list.Count);
                if (rnd.Next(2) == 0)
                    SetValue(l, rnd.Next(100000));
                else
                    Add(rnd.Next(1000000));
                Test();
            }
        }

        [Fact]
        public void AddRange()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = new LazyRedBlackTree<int>(list);
            void AddRange(params int[] values)
            {
                tree.AddRange(values);
                list.AddRange(values);
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            Test();

            AddRange(new[] { 1, 2, 2, 3 });
            Test();
            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                int len = rnd.Next(50);
                var array = new int[len];
                rnd.NextBytes(MemoryMarshal.AsBytes<int>(array));
                AddRange(array);
                Test();
            }
        }

        [Fact]
        public void InsertRange()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = new LazyRedBlackTree<int>(list);
            void InsertRange(int index, params int[] values)
            {
                tree.InsertRange(index, values);
                list.InsertRange(index, values);
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            Test();

            InsertRange(1, new[] { 1, 2, 2, 3 });
            Test();
            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                int len = rnd.Next(50);
                var array = new int[len];
                rnd.NextBytes(MemoryMarshal.AsBytes<int>(array));
                var l = rnd.Next(list.Count);
                InsertRange(l, array);
                Test();
            }
        }

        [Fact]
        public void RemoveAt()
        {
            var list = Enumerable.Range(0, 1000).ToList();
            var tree = new LazyRedBlackTree<int>(list);
            void RemoveAt(int index)
            {
                tree.RemoveAt(index);
                list.RemoveAt(index);
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }

            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                var l = rnd.Next(list.Count);
                RemoveAt(l);
                Test();
            }
        }

        [Fact]
        public void RemoveRange()
        {
            var list = Enumerable.Range(0, 100).ToList();
            var tree = new LazyRedBlackTree<int>(list);
            void AddRange(params int[] values)
            {
                tree.AddRange(values);
                list.AddRange(values);
            }
            void RemoveRange(int index, int count)
            {
                tree.RemoveRange(index, count);
                list.RemoveRange(index, count);
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }

            var rnd = new Random(227);
            for (int q = 0; q < 100; q++)
            {
                if (list.Count <= 50)
                    AddRange(Enumerable.Range(0, 200).ToArray());

                var l = rnd.Next(list.Count - 50);
                int len = rnd.Next(50);
                RemoveRange(l, len);
                Test();
            }
        }
    }
}
