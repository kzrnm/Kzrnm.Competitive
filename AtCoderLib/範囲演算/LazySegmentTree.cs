using System;
using System.Collections.Generic;
using static AtCoderProject.Global;

class SegmentTreeLazy : SegmentTreeLazyImpl<long, long>
{
    protected override long DefaultValue => 0;
    protected override long DefaultLazy => 0;
    protected override long Operate(long v1, long v2) => Math.Max(v1, v2);
    protected override long Apply(long v, long l) => v + l;
    protected override long Merge(long l1, long l2) => l1 + l2;
    public SegmentTreeLazy(long[] initArray) : base(initArray) { }
    public SegmentTreeLazy(int size) : base(size) { }
}


[System.Diagnostics.DebuggerTypeProxy(typeof(SegmentTreeLazyImpl<,>.SegmentTreeLazyDebugView))]
abstract class SegmentTreeLazyImpl<T, L> where T : struct where L : struct
{
    protected abstract T DefaultValue { get; }
    protected abstract L DefaultLazy { get; }
    protected abstract T Operate(T v1, T v2);
    protected abstract T Apply(T v, L l);
    protected abstract L Merge(L l1, L l2);

    private T[] tree;
    private L[] lazy;
    public readonly int rootLength;
    public int Length { get; }

    public SegmentTreeLazyImpl(T[] initArray) : this(initArray.Length)
    {
        var rootLength = this.rootLength;
        Array.Copy(initArray, 0, tree, rootLength - 1, initArray.Length);
        for (int i = rootLength - 2; i >= 0; i--)
            tree[i] = Operate(tree[(i << 1) + 1], tree[(i << 1) + 2]);
    }
    public SegmentTreeLazyImpl(int size)
    {
        this.Length = size;
        rootLength = 1 << (MSB(size - 1) + 1);
        lazy = NewArray((rootLength << 1) - 1, DefaultLazy);
        tree = NewArray((rootLength << 1) - 1, DefaultValue);
    }
    protected void Eval(int k)
    {
        if (k < rootLength - 1)
        {
            lazy[k * 2 + 1] = Merge(lazy[k * 2 + 1], lazy[k]);
            lazy[k * 2 + 2] = Merge(lazy[k * 2 + 2], lazy[k]);
        }
        tree[k] = Apply(tree[k], lazy[k]);
        lazy[k] = DefaultLazy;
    }
    public void Update(int fromInclusive, int toExclusive, L value)
    {
        void Update(int fromInclusive, int toExclusive, L value, int k, int l, int r)
        {
            Eval(k);
            if (fromInclusive <= l && r <= toExclusive)
            {
                lazy[k] = Merge(lazy[k], value);
                Eval(k);
            }
            else if (fromInclusive < r && l < toExclusive)
            {
                Update(fromInclusive, toExclusive, value, k * 2 + 1, l, (l + r) / 2);
                Update(fromInclusive, toExclusive, value, k * 2 + 2, (l + r) / 2, r);
                tree[k] = Operate(tree[k * 2 + 1], tree[k * 2 + 2]);
            }
        }
        Update(fromInclusive, toExclusive, value, 0, 0, rootLength);
    }

    public T Slice(int from, int length) => Query(from, from + length);
    public T Query(int fromInclusive, int toExclusive)
    {
        T Query(int fromInclusive, int toExclusive, int k, int l, int r)
        {
            Eval(k);
            if (r <= fromInclusive || toExclusive <= l) return DefaultValue;
            else if (fromInclusive <= l && r <= toExclusive) return tree[k];
            else return Operate(
                Query(fromInclusive, toExclusive, k * 2 + 1, l, (l + r) / 2),
                Query(fromInclusive, toExclusive, k * 2 + 2, (l + r) / 2, r)
                );
        }
        return Query(fromInclusive, toExclusive, 0, 0, rootLength);
    }


    [System.Diagnostics.DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
    struct KeyValuePairs
    {
        private string key;
        private (T value, L lazy) value;

        public KeyValuePairs(string key, (T value, L lazy) value)
        {
            this.key = key;
            this.value = value;
        }
    }
    class SegmentTreeLazyDebugView
    {
        private SegmentTreeLazyImpl<T, L> segmentTree;
        public SegmentTreeLazyDebugView(SegmentTreeLazyImpl<T, L> segmentTree)
        {
            this.segmentTree = segmentTree;
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        public KeyValuePairs[] Tree
        {
            get
            {
                var keys = new List<KeyValuePairs>(segmentTree.tree.Length);
                for (var len = segmentTree.rootLength; len > 0; len >>= 1)
                {
                    var unit = segmentTree.rootLength / len;
                    for (var i = 0; i < len; i++)
                    {
                        var index = i + len - 1;
                        if (unit == 1)
                            keys.Add(new KeyValuePairs($"[{i}]", (segmentTree.tree[index], segmentTree.lazy[index])));
                        else
                        {
                            var from = i * unit;
                            keys.Add(new KeyValuePairs($"[{from}-{from + unit})", (segmentTree.tree[index], segmentTree.lazy[index])));
                        }
                    }
                }
                return keys.ToArray();
            }
        }
    }
}
