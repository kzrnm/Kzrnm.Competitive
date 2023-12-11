using Kzrnm.Competitive;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public abstract class ImmutableLazyBinarySearchTreeTestsBase<Node, TBbst>
        where Node : class, ILazyBbstNode<int, int, Node>
        where TBbst : ImmutableLazyBinarySearchTreeBase<int, int, Node, TBbst>, IImmutableBbst<int, Node, TBbst>
    {
        protected abstract TBbst Empty { get; }
        protected abstract TBbst Create(IEnumerable<int> values);

        [Fact]
        public void Zero()
        {
            Empty.AllProd.Should().Be(0);
        }
        [Fact]
        public void NaiveProd()
        {
            for (int n = 0; n <= 30; n++)
            {
                var p = new int[n];
                for (int i = 0; i < n; i++)
                    p[i] = (i * i + 100) % 31;
                var tree = Create(p);
                var expected = new SLazySegtree<int, int, SumOp>(p);

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
                    tree = tree.SetItem(i, x);
                    expected[i] = x;
                }
                Test();

                for (int l = 0; l < n; l++)
                    for (int r = l; r <= n; r++)
                    {
                        var x = ((l << 5) + (l >> 2) + (r << 3) + r % 3) % 51;
                        tree = tree.Apply(l, r, x);
                        expected.Apply(l, r, x);
                    }
                Test();
            }
        }

        [Fact]
        public void Usage()
        {
            var tree = Create(new int[10]);
            tree.AllProd.Should().Be(0);
            tree.Should().Equal([0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
            tree = tree.Apply(0, 3, 5);
            tree.Should().Equal([5, 5, 5, 0, 0, 0, 0, 0, 0, 0]);
            tree.Prod(0, 1).Should().Be(5);
            tree.AllProd.Should().Be(15);
            tree = tree.Apply(2, 3, -10);
            tree.Should().Equal([5, 5, -5, 0, 0, 0, 0, 0, 0, 0]);
            tree.Prod(2, 3).Should().Be(tree[2..3]).And.Be(-5);
            tree.Prod(2, 4).Should().Be(tree[2..4]).And.Be(-5);
            tree.Prod(1, 4).Should().Be(tree[1..4]).And.Be(0);
        }

        [Fact]
        public void Reverse()
        {
            const int N = 8;
            var tree = Create(Enumerable.Range(0, N));
            for (int i = 0; i < N; i++)
                tree[i].Should().Be(i);
            tree = tree.Reverse(2, 5);
            var expected = new[] { 0, 1, 4, 3, 2, 5, 6, 7 };
            tree = tree.Reverse();
            expected.AsSpan().Reverse();

            var savedTree = new List<TBbst>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(expected.ToArray());
                tree.Should().Equal(expected);
                for (int i = 0; i < N; i++)
                    tree[i].Should().Be(expected[i]);
            }
            void Reverse(int l, int r)
            {
                tree = tree.Reverse(l, r);
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

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void InsertAndReverse()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = Create(list);
            void Insert(int index, int value)
            {
                tree = tree.Insert(index, value);
                list.Insert(index, value);
            }
            void Reverse(int l, int r)
            {
                tree = tree.Reverse(l, r);
                list.AsSpan()[l..r].Reverse();
            }
            var savedTree = new List<TBbst>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(list.ToArray());
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            tree = tree.AddLast(-1);
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

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void AddAndSetValue()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = Create(list);
            void Add(int value)
            {
                tree = tree.AddLast(value);
                list.Add(value);
            }
            void SetValue(int index, int value)
            {
                tree = tree.SetItem(index, value);
                list[index] = value;
            }
            var savedTree = new List<TBbst>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(list.ToArray());
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
                    SetValue(l, rnd.Next(1000000));
                else
                    Add(rnd.Next(1000000));
                Test();
            }

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void AddRange()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = Create(list);
            void AddRange(params int[] values)
            {
                tree = tree.AddRange(values);
                list.AddRange(values);
            }
            var savedTree = new List<TBbst>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(list.ToArray());
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            Test();

            AddRange([1, 2, 2, 3]);
            Test();
            var rnd = new Random(227);
            for (int q = 0; q < 50; q++)
            {
                int len = rnd.Next(50);
                var array = new int[len];
                rnd.NextBytes(MemoryMarshal.AsBytes<int>(array));
                AddRange(array);
                Test();
            }

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void InsertRange()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = Create(list);
            void InsertRange(int index, params int[] values)
            {
                tree = tree.InsertRange(index, values);
                list.InsertRange(index, values);
            }
            var savedTree = new List<TBbst>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(list.ToArray());
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }

            Test();

            InsertRange(1, [1, 2, 2, 3]);
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

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void RemoveAt()
        {
            var list = Enumerable.Range(0, 1000).ToList();
            var tree = Create(list);
            void RemoveAt(int index)
            {
                tree = tree.RemoveAt(index);
                list.RemoveAt(index);
            }
            var savedTree = new List<TBbst>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(list.ToArray());
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }

            var rnd = new Random(227);
            for (int q = 0; q < 50; q++)
            {
                var l = rnd.Next(list.Count);
                RemoveAt(l);
                Test();
            }

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void RemoveRange()
        {
            var list = Enumerable.Range(0, 100).ToList();
            var tree = Create(list);
            void AddRange(params int[] values)
            {
                tree = tree.AddRange(values);
                list.AddRange(values);
            }
            void RemoveRange(int index, int count)
            {
                tree = tree.RemoveRange(index, count);
                list.RemoveRange(index, count);
            }
            var savedTree = new List<TBbst>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(list.ToArray());
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }

            var rnd = new Random(227);
            for (int q = 0; q < 50; q++)
            {
                if (list.Count <= 50)
                    AddRange(Enumerable.Range(0, 100).ToArray());

                var l = rnd.Next(list.Count - 50);
                int len = rnd.Next(50);
                RemoveRange(l, len);
                Test();
            }

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }
    }
}
