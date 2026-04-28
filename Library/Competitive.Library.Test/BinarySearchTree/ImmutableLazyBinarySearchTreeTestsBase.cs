using Kzrnm.Competitive;
using Kzrnm.Competitive.Internal;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

public abstract class ImmutableLazyBinarySearchTreeTestsBase<Node, TBbst>
    where Node : class, ILazyBbstNode<int, int, Node>
    where TBbst : ImmutableLazyBinarySearchTreeBase<int, int, Node, TBbst>, IImmutableBbst<int, Node, TBbst>
{
    protected abstract TBbst Empty { get; }
    protected abstract TBbst Create(IEnumerable<int> values);

    [Test]
    public async Task Zero()
    {
        await Empty.AllProd.Should().BeEqualTo(0);
    }
    [Test, MultipleAssertions]
    public async Task NaiveProd()
    {
        for (int n = 0; n <= 30; n++)
        {
            var p = new int[n];
            for (int i = 0; i < n; i++)
                p[i] = (i * i + 100) % 31;
            var tree = Create(p);
            var expected = new SLazySegtree<int, int, SumOp>(p);

            async Task Test()
            {
                for (int l = 0; l <= n; l++)
                    for (int r = l; r <= n; r++)
                    {
                        await tree.Prod(l, r).Should().BeEqualTo(expected[l..r]);
                        await tree[l..r].Should().BeEqualTo(expected[l..r]);
                    }
                for (int i = 0; i < n; i++)
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
                tree = tree.SetItem(i, x);
                expected[i] = x;
            }
            await Test();

            for (int l = 0; l < n; l++)
                for (int r = l; r <= n; r++)
                {
                    var x = ((l << 5) + (l >> 2) + (r << 3) + r % 3) % 51;
                    tree = tree.Apply(l, r, x);
                    expected.Apply(l, r, x);
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
        tree = tree.Apply(0, 3, 5);
        await tree.Should().BeEquivalentOrderTo([5, 5, 5, 0, 0, 0, 0, 0, 0, 0]);
        await tree.Prod(0, 1).Should().BeEqualTo(5);
        await tree.AllProd.Should().BeEqualTo(15);
        tree = tree.Apply(2, 3, -10);
        await tree.Should().BeEquivalentOrderTo([5, 5, -5, 0, 0, 0, 0, 0, 0, 0]);
        await tree.Prod(2, 3).Should().BeEqualTo(-5);
        await tree.Prod(2, 4).Should().BeEqualTo(-5);
        await tree.Prod(1, 4).Should().BeEqualTo(0);
        await tree[2..3].Should().BeEqualTo(-5);
        await tree[2..4].Should().BeEqualTo(-5);
        await tree[1..4].Should().BeEqualTo(0);
    }

    [Test, MultipleAssertions]
    public async Task Reverse()
    {
        const int N = 8;
        var tree = Create(Enumerable.Range(0, N));
        for (int i = 0; i < N; i++)
            await tree[i].Should().BeEqualTo(i);
        tree = tree.Reverse(2, 5);
        var expected = new[] { 0, 1, 4, 3, 2, 5, 6, 7 };
        tree = tree.Reverse();
        expected.AsSpan().Reverse();

        var savedTree = new List<TBbst>();
        var savedExpects = new List<int[]>();
        async Task Test()
        {
            savedTree.Add(tree);
            savedExpects.Add(expected.ToArray());
            await tree.Should().BeEquivalentOrderTo(expected);
            for (int i = 0; i < N; i++)
                await tree[i].Should().BeEqualTo(expected[i]);
        }
        void Reverse(int l, int r)
        {
            tree = tree.Reverse(l, r);
            expected.AsSpan()[l..r].Reverse();
        }

        await Test();

        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            int l = rnd.Next(N);
            int r = rnd.Next(l + 1, N + 1);
            Reverse(l, r);
            await Test();
        }

        for (int i = 0; i < savedTree.Count; i++)
            await savedTree[i].Should().BeEquivalentOrderTo(savedExpects[i]);
    }

    [Test, MultipleAssertions]
    public async Task InsertAndReverse()
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
        async Task Test()
        {
            savedTree.Add(tree);
            savedExpects.Add(list.ToArray());
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }


        tree = tree.AddLast(-1);
        list.Add(-1);
        await Test();

        Insert(2, -2);
        await Test();
        Insert(0, -5);
        Insert(list.Count, 111);
        await Test();

        Reverse(2, 5);
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
            if (rnd.Next(2) == 0)
                Reverse(l, rnd.Next(l, list.Count) + 1);
            else
                Insert(l, rnd.Next(list.Count) - 100);
            await Test();
        }

        for (int i = 0; i < savedTree.Count; i++)
            await savedTree[i].Should().BeEquivalentOrderTo(savedExpects[i]);
    }

    [Test, MultipleAssertions]
    public async Task AddAndSetValue()
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
        async Task Test()
        {
            savedTree.Add(tree);
            savedExpects.Add(list.ToArray());
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
                SetValue(l, rnd.Next(1000000));
            else
                Add(rnd.Next(1000000));
            await Test();
        }

        for (int i = 0; i < savedTree.Count; i++)
            await savedTree[i].Should().BeEquivalentOrderTo(savedExpects[i]);
    }

    [Test, MultipleAssertions]
    public async Task AddRange()
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
        async Task Test()
        {
            savedTree.Add(tree);
            savedExpects.Add(list.ToArray());
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

        for (int i = 0; i < savedTree.Count; i++)
            await savedTree[i].Should().BeEquivalentOrderTo(savedExpects[i]);
    }

    [Test, MultipleAssertions]
    public async Task InsertRange()
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
        async Task Test()
        {
            savedTree.Add(tree);
            savedExpects.Add(list.ToArray());
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }

        await Test();

        InsertRange(1, [1, 2, 2, 3]);
        await Test();
        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            int len = rnd.Next(50);
            var array = new int[len];
            rnd.NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
            var l = rnd.Next(list.Count);
            InsertRange(l, array);
            await Test();
        }

        for (int i = 0; i < savedTree.Count; i++)
            await savedTree[i].Should().BeEquivalentOrderTo(savedExpects[i]);
    }

    [Test, MultipleAssertions]
    public async Task RemoveAt()
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
        async Task Test()
        {
            savedTree.Add(tree);
            savedExpects.Add(list.ToArray());
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }

        var rnd = new Random(227);
        for (int q = 0; q < 50; q++)
        {
            var l = rnd.Next(list.Count);
            RemoveAt(l);
            await Test();
        }

        for (int i = 0; i < savedTree.Count; i++)
            await savedTree[i].Should().BeEquivalentOrderTo(savedExpects[i]);
    }

    [Test, MultipleAssertions]
    public async Task RemoveRange()
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
        async Task Test()
        {
            savedTree.Add(tree);
            savedExpects.Add(list.ToArray());
            await tree.Should().BeEquivalentOrderTo(list);
            for (int i = 0; i < list.Count; i++)
                await tree[i].Should().BeEqualTo(list[i]);
        }

        var rnd = new Random(227);
        for (int q = 0; q < 50; q++)
        {
            if (list.Count <= 50)
                AddRange(Enumerable.Range(0, 100).ToArray());

            var l = rnd.Next(list.Count - 50);
            int len = rnd.Next(50);
            RemoveRange(l, len);
            await Test();
        }

        for (int i = 0; i < savedTree.Count; i++)
            await savedTree[i].Should().BeEquivalentOrderTo(savedExpects[i]);
    }
}