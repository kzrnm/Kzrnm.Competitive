using System;
using System.Collections.Generic;
using static AtCoderProject.Global;




class SegmentTreeLazy : SegmentTreeLazyAbstract<long, long>
{
    protected override long DefaultValue => 0;
    protected override long DefaultLazy => 0;
    protected override long Operate(long v1, long v2) => Math.Max(v1, v2);
    protected override long ApplyLazy(long v, long l) => v + l;
    protected override long Merge(long l1, long l2) => l1 + l2;
    public SegmentTreeLazy(long[] initArray) : base(initArray) { }
    public SegmentTreeLazy(int size) : base(size) { }
}
[System.Diagnostics.DebuggerTypeProxy(typeof(SegmentTreeLazyAbstract<,>.SegmentTreeLazyDebugView))]
abstract class SegmentTreeLazyAbstract<TValue, TOp> where TValue : struct where TOp : struct
{
    protected virtual TValue DefaultValue => default;
    protected virtual TOp DefaultLazy => default;
    protected abstract TValue Operate(TValue v1, TValue v2);
    protected abstract TValue ApplyLazy(TValue v, TOp l);
    protected abstract TOp Merge(TOp l1, TOp l2);

    private TValue[] tree;
    private TOp[] lazy;
    public readonly int rootLength;
    public int Length { get; }

    public SegmentTreeLazyAbstract(TValue[] initArray) : this(initArray.Length)
    {
        var rootLength = this.rootLength;
        Array.Copy(initArray, 0, tree, rootLength - 1, initArray.Length);
        for (int i = rootLength - 2; i >= 0; i--)
            tree[i] = Operate(tree[(i << 1) + 1], tree[(i << 1) + 2]);
    }
    public SegmentTreeLazyAbstract(int size)
    {
        this.Length = size;
        rootLength = 1 << (MSB(size - 1) + 1);
        lazy = NewArray((rootLength << 1) - 1, DefaultLazy);
        tree = NewArray((rootLength << 1) - 1, DefaultValue);
    }
    protected void Eval(int k)
    {
        if (EqualityComparer<TOp>.Default.Equals(lazy[k], DefaultLazy)) return;
        if (k < rootLength - 1)
        {
            lazy[k * 2 + 1] = Merge(lazy[k * 2 + 1], lazy[k]);
            lazy[k * 2 + 2] = Merge(lazy[k * 2 + 2], lazy[k]);
        }
        tree[k] = ApplyLazy(tree[k], lazy[k]);
        lazy[k] = DefaultLazy;
    }
    public void Apply(int fromInclusive, int toExclusive, TOp value)
    {
        void Apply(int fromInclusive, int toExclusive, TOp value, int k, int l, int r)
        {
            Eval(k);
            if (fromInclusive <= l && r <= toExclusive)
            {
                lazy[k] = Merge(lazy[k], value);
                Eval(k);
            }
            else if (fromInclusive < r && l < toExclusive)
            {
                Apply(fromInclusive, toExclusive, value, k * 2 + 1, l, (l + r) >> 1);
                Apply(fromInclusive, toExclusive, value, k * 2 + 2, (l + r) >> 1, r);
                tree[k] = Operate(tree[k * 2 + 1], tree[k * 2 + 2]);
            }
        }
        Apply(fromInclusive, toExclusive, value, 0, 0, rootLength);
    }

    public TValue Slice(int from, int length) => Query(from, from + length);
    public TValue Query(int fromInclusive, int toExclusive)
    {
        TValue Query(int fromInclusive, int toExclusive, int k, int l, int r)
        {
            Eval(k);
            if (r <= fromInclusive || toExclusive <= l) return DefaultValue;
            else if (fromInclusive <= l && r <= toExclusive) return tree[k];
            else return Operate(
                Query(fromInclusive, toExclusive, k * 2 + 1, l, (l + r) >> 1),
                Query(fromInclusive, toExclusive, k * 2 + 2, (l + r) >> 1, r)
                );
        }
        return Query(fromInclusive, toExclusive, 0, 0, rootLength);
    }


    [System.Diagnostics.DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
    struct KeyValuePairs
    {
        private string key;
        private (TValue value, TOp lazy) value;

        public KeyValuePairs(string key, (TValue value, TOp lazy) value)
        {
            this.key = key;
            this.value = value;
        }
    }
    class SegmentTreeLazyDebugView
    {
        private SegmentTreeLazyAbstract<TValue, TOp> segmentTree;
        public SegmentTreeLazyDebugView(SegmentTreeLazyAbstract<TValue, TOp> segmentTree)
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
