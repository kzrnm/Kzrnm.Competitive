using Kzrnm.Competitive;
using Kzrnm.Competitive.Internal;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

public readonly struct SumOp : IReversibleBinarySearchTreeOperator<int, int>
{
    public int Identity => 0;
    public int FIdentity => 0;

    public int Composition(int f, int g) => f + g;

    public int Inverse(int v) => v;
    public int Mapping(int f, int x, int size) => f * size + x;
    public int Operate(int x, int y) => x + y;
}
public abstract class LazyBinarySearchTreeTestsBase<Node>
    where Node : class, ILazyBbstNode<int, int, Node>
{
    protected abstract LazyBinarySearchTreeBase<int, int, Node> Create();
    protected abstract LazyBinarySearchTreeBase<int, int, Node> Create(IEnumerable<int> values);

    [Test]
    public async Task Zero()
    {
        await Create().AllProd.Should().BeEqualTo(0);
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
                tree[i] = expected[i] = x;
            }
            await Test();

            for (int l = 0; l < n; l++)
                for (int r = l; r <= n; r++)
                {
                    var x = ((l << 5) + (l >> 2) + (r << 3) + r % 3) % 51;
                    tree.Apply(l, r, x);
                    expected.Apply(l, r, x);
                }
            await Test();
        }
    }

    [Test, MultipleAssertions]
    public async Task Usage()
    {
        const int n = 10;
        var tree = Create(new int[n]);
        var expected = new SLazySegtree<int, int, SumOp>(n);
        void Apply(int l, int r, int x)
        {
            tree.Apply(l, r, x);
            expected.Apply(l, r, x);
        }
        async Task Test()
        {
            await tree.AllProd.Should().BeEqualTo(expected.AllProd);
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
            await tree.Should().BeEquivalentOrderTo(expected.ToArray());
        }

        await Test();
        Apply(0, 3, 5);
        await Test();
        Apply(2, 3, -10);
        await Test();
    }

    [Test, MultipleAssertions]
    public async Task Reverse()
    {
        const int N = 8;
        var tree = Create(Enumerable.Range(0, N));
        for (int i = 0; i < N; i++)
            await tree[i].Should().BeEqualTo(i);
        tree.Reverse(2, 5);
        var expected = new[] { 0, 1, 4, 3, 2, 5, 6, 7 };
        tree.Reverse();
        expected.AsSpan().Reverse();

        async Task Test()
        {
            await tree.Should().BeEquivalentOrderTo(expected);
            for (int i = 0; i < N; i++)
                await tree[i].Should().BeEqualTo(expected[i]);
        }
        void Reverse(int l, int r)
        {
            tree.Reverse(l, r);
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
    }

    [Test, MultipleAssertions]
    public async Task InsertAndReverse()
    {
        var list = Enumerable.Range(0, 8).ToList();
        var tree = Create(list);
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
            int len = rnd.Next(20);
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
            Enumerable.Range(0, 50).ToList(),
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
            for (int q = 0; q < 50; q++)
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