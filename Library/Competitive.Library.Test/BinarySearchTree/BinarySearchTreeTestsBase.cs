using AtCoder;
using Kzrnm.Competitive;
using Kzrnm.Competitive.Internal;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

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

    [Test]
    public async Task Zero()
    {
        await Create().AllProd.Should().BeEqualTo(0);
    }

    [Test]
    public async Task NaiveProd()
    {
        for (int n = 0; n <= 30; n++)
        {
            var p = new int[n];
            for (int i = 0; i < n; i++)
                p[i] = (i * i + 100) % 31;
            var tree = Create(p);
            var expected = new Segtree<int, Starry>(p);

            async Task Test()
            {
                for (int l = 0; l <= n; l++)
                    for (int r = l; r <= n; r++)
                        using (Assert.Multiple())
                        {
                            await tree.Prod(l, r).Should().BeEqualTo(expected[l..r]);
                            await tree[l..r].Should().BeEqualTo(expected[l..r]);
                        }
                for (int i = 0; i < n; i++)
                    using (Assert.Multiple())
                    {
                        await tree[i..(i + 1)].Should().BeEqualTo(expected[i]);
                        await tree[i].Should().BeEqualTo(expected[i]);
                    }
            }
            await tree.Should().BeEquivalentOrderTo(p);
            await Test();


            for (int i = 0; i < n; i++)
            {
                var x = ((i << 5) + (i >> 2) + (i << 3) + i % 3) % 51;
                tree[i] = expected[i] = x;
            }
            await Test();
        }
    }

    [Test, MultipleAssertions]
    public async Task Usage()
    {
        var tree = Create(new int[10]);
        await tree.AllProd.Should().BeEqualTo(0);
        await tree.Should().BeEquivalentOrderTo([0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
        tree[0] = tree[1] = tree[2] = 5;
        await tree.Should().BeEquivalentOrderTo([5, 5, 5, 0, 0, 0, 0, 0, 0, 0]);
        await tree.Prod(0, 1).Should().BeEqualTo(5);
        await tree.Prod(0, 2).Should().BeEqualTo(10);
        await tree.Prod(0, 3).Should().BeEqualTo(15);
        await tree.Prod(1, 2).Should().BeEqualTo(5);
        await tree.Prod(1, 3).Should().BeEqualTo(10);
        await tree.Prod(2, 3).Should().BeEqualTo(5);
        await tree.AllProd.Should().BeEqualTo(15);
    }

    [Test, MultipleAssertions]
    public async Task Insert()
    {
        var list = Enumerable.Range(0, 8).ToList();
        var tree = Create(list);
        void Insert(int index, int value)
        {
            tree.Insert(index, value);
            list.Insert(index, value);
        }
        async Task Test()
        {
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }


        tree.AddLast(-1);
        list.Add(-1);
        await Test();

        Insert(2, -2);
        await Test();
        Insert(0, -5);
        Insert(list.Count, 111);
        await Test();

        Insert(2, -12);
        await Test();
        Insert(0, -15);
        Insert(list.Count, 1111);
        await Test();

        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            int l = rnd.Next(list.Count);
            Insert(l, rnd.Next(list.Count) - 100);
            await Test();
        }
    }

    [Test, MultipleAssertions]
    public async Task AddAndSetValue()
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
        async Task Test()
        {
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }


        Add(-1);
        await Test();

        SetValue(2, -2);
        await Test();
        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            int l = rnd.Next(list.Count);
            if (rnd.Next(2) == 0)
                SetValue(l, rnd.Next(100000));
            else
                Add(rnd.Next(1000000));
            await Test();
        }
    }

    [Test, MultipleAssertions]
    public async Task AddRange()
    {
        var list = Enumerable.Range(0, 8).ToList();
        var tree = Create(list);
        void AddRange(params int[] values)
        {
            tree.AddRange(values);
            list.AddRange(values);
        }
        async Task Test()
        {
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }


        await Test();

        AddRange([1, 2, 2, 3]);
        await Test();
        var rnd = new Random(227);
        for (int q = 0; q < 50; q++)
        {
            int len = rnd.Next(50);
            var array = new int[len];
            rnd.NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
            AddRange(array);
            await Test();
        }
    }

    [Test, MultipleAssertions]
    public async Task InsertRange()
    {
        var list = Enumerable.Range(0, 8).ToList();
        var tree = Create(list);
        void InsertRange(int index, params int[] values)
        {
            tree.InsertRange(index, values);
            list.InsertRange(index, values);
        }
        async Task Test()
        {
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }


        await Test();

        InsertRange(1, [1, 2, 2, 3]);
        await Test();
        var rnd = new Random(227);
        for (int q = 0; q < 50; q++)
        {
            int len = rnd.Next(50);
            var array = new int[len];
            rnd.NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
            var l = rnd.Next(list.Count);
            InsertRange(l, array);
            await Test();
        }
    }

    [Test, MultipleAssertions]
    public async Task RemoveAt()
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
            async Task Test()
            {
                await tree.Should().BeEquivalentOrderTo(list);
                for (int i = 0; i < list.Count; i++)
                    await tree[i].Should().BeEqualTo(list[i]);
            }

            var rnd = new Random(227);
            for (int q = 0; q < 80; q++)
            {
                var l = rnd.Next(list.Count);
                RemoveAt(l);
                await Test();
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task RemoveRange()
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
        async Task Test()
        {
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }

        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            if (list.Count <= 50)
                AddRange(Enumerable.Range(0, 200).ToArray());

            var l = rnd.Next(list.Count - 50);
            int len = rnd.Next(50);
            RemoveRange(l, len);
            await Test();
        }
    }
}