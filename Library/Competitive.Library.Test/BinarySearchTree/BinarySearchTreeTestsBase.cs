using AtCoder;
using Kzrnm.Competitive;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public readonly struct Starry : ISegtreeOperator<int>
    {
        public int Identity => 0;
        public int Operate(int x, int y) => x + y;
    }

    public abstract class BinarySearchTreeTestsBase<Node>
        where Node : class, IBbstNode<int, Node>
    {
        protected abstract BinarySearchTreeBase<int, Node> Create();
        protected abstract BinarySearchTreeBase<int, Node> Create(IEnumerable<int> values);

        [Fact]
        public void Zero()
        {
            Create().AllProd.Should().Be(0);
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
                    tree[i] = expected[i] = x;
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
            tree[0] = tree[1] = tree[2] = 5;
            tree.Should().Equal([5, 5, 5, 0, 0, 0, 0, 0, 0, 0]);
            tree.Prod(0, 1).Should().Be(5);
            tree.Prod(0, 2).Should().Be(10);
            tree.Prod(0, 3).Should().Be(15);
            tree.Prod(1, 2).Should().Be(5);
            tree.Prod(1, 3).Should().Be(10);
            tree.Prod(2, 3).Should().Be(5);
            tree.AllProd.Should().Be(15);
        }

        [Fact]
        public void Insert()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = Create(list);
            void Insert(int index, int value)
            {
                tree.Insert(index, value);
                list.Insert(index, value);
            }
            void Test()
            {
                tree.Should().Equal(list);
                for (int i = 0; i < list.Count; i++)
                    tree[i].Should().Be(list[i]);
            }


            tree.AddLast(-1);
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
        }

        [Fact]
        public void AddAndSetValue()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = Create(list);
            void Add(int value)
            {
                tree.AddLast(value);
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
            var tree = Create(list);
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
        }

        [Fact]
        public void InsertRange()
        {
            var list = Enumerable.Range(0, 8).ToList();
            var tree = Create(list);
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

            InsertRange(1, [1, 2, 2, 3]);
            Test();
            var rnd = new Random(227);
            for (int q = 0; q < 50; q++)
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
            foreach (var list in new[]
            {
                Enumerable.Range(0, 500).ToList(),
                Enumerable.Range(0, 80).ToList(),
            })
            {
                var tree = Create(list);
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
                for (int q = 0; q < 80; q++)
                {
                    var l = rnd.Next(list.Count);
                    RemoveAt(l);
                    Test();
                }
            }
        }

        [Fact]
        public void RemoveRange()
        {
            var list = Enumerable.Range(0, 100).ToList();
            var tree = Create(list);
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
