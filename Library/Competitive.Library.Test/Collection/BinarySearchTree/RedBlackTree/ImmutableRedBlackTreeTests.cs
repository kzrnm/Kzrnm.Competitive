using AtCoder;
using Kzrnm.Competitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class ImmutableRedBlackTreeTests
    {
        private readonly struct Starry : ISegtreeOperator<int>
        {
            public int Identity => 0;
            public int Operate(int x, int y) => x + y;
        }

        [Fact]
        public void Zero()
        {
            ImmutableRedBlackTree<int, Starry>.Empty.AllProd.Should().Be(0);
        }

        [Fact]
        public void NaiveProd()
        {
            for (int n = 0; n <= 50; n++)
            {
                var p = new int[n];
                for (int i = 0; i < n; i++)
                    p[i] = (i * i + 100) % 31;
                var tree = new ImmutableRedBlackTree<int, Starry>(p);
                var expected = new Segtree<int, Starry>(p);

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
            }
        }

        [Fact]
        public void Usage()
        {
            var tree = new ImmutableRedBlackTree<int, Starry>(new int[10]);
            tree.AllProd.Should().Be(0);
            tree.Should().Equal(new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            tree = tree.SetItem(0, 5).SetItem(1, 5).SetItem(2, 5);
            tree.Should().Equal(new[] { 5, 5, 5, 0, 0, 0, 0, 0, 0, 0 });
            tree.Prod(0, 1).Should().Be(5);
            tree.Prod(0, 2).Should().Be(10);
            tree.Prod(0, 3).Should().Be(15);
            tree.Prod(1, 2).Should().Be(5);
            tree.Prod(1, 3).Should().Be(10);
            tree.Prod(2, 3).Should().Be(5);
            tree.AllProd.Should().Be(15);
        }

        [Fact]
        public void InsertAndReverse()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = new ImmutableRedBlackTree<int>(list);
            void Insert(int index, int value)
            {
                tree = tree.Insert(index, value);
                list.Insert(index, value);
            }
            var savedTree = new List<ImmutableRedBlackTree<int>>();
            var savedExpects = new List<int[]>();
            void Test()
            {
                savedTree.Add(tree);
                savedExpects.Add(list.ToArray());
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            tree = tree.Add(-1);
            list.Add(-1);
            Test();

            Insert(2, -2);
            Test();
            Insert(0, -5);
            Insert(list.Count, 111);
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
            var tree = new ImmutableRedBlackTree<int>(list);
            void Add(int value)
            {
                tree = tree.Add(value);
                list.Add(value);
            }
            void SetValue(int index, int value)
            {
                tree = tree.SetItem(index, value);
                list[index] = value;
            }
            var savedTree = new List<ImmutableRedBlackTree<int>>();
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
            var tree = new ImmutableRedBlackTree<int>(list);
            void AddRange(params int[] values)
            {
                tree = tree.AddRange(values);
                list.AddRange(values);
            }
            var savedTree = new List<ImmutableRedBlackTree<int>>();
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

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void InsertRange()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = new ImmutableRedBlackTree<int>(list);
            void InsertRange(int index, params int[] values)
            {
                tree = tree.InsertRange(index, values);
                list.InsertRange(index, values);
            }
            var savedTree = new List<ImmutableRedBlackTree<int>>();
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

            for (int i = 0; i < savedTree.Count; i++)
                savedTree[i].Should().Equal(savedExpects[i]);
        }

        [Fact]
        public void RemoveAt()
        {
            var list = Enumerable.Range(0, 1000).ToList();
            var tree = new ImmutableRedBlackTree<int>(list);
            void RemoveAt(int index)
            {
                tree = tree.RemoveAt(index);
                list.RemoveAt(index);
            }
            var savedTree = new List<ImmutableRedBlackTree<int>>();
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
            for (int q = 0; q < 100; q++)
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
            var tree = new ImmutableRedBlackTree<int>(list);
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
            var savedTree = new List<ImmutableRedBlackTree<int>>();
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
            for (int q = 0; q < 100; q++)
            {
                if (list.Count <= 50)
                    AddRange(Enumerable.Range(0, 200).ToArray());

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
