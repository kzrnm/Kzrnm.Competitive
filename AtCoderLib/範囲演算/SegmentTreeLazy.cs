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

    TValue[] tree;
    TOp[] lazy;
    public readonly int rootLength;
    public int Length { get; }
    private readonly int log;
    public SegmentTreeLazyAbstract(TValue[] initArray) : this(initArray.Length)
    {
        Array.Copy(initArray, 0, tree, rootLength, initArray.Length);
        for (int i = rootLength - 1; i >= 1; i--)
            Update(i);
    }
    public SegmentTreeLazyAbstract(int size)
    {
        this.Length = size;
        this.log = MSB(size - 1) + 1;
        rootLength = 1 << log;
        lazy = NewArray(rootLength, DefaultLazy);
        tree = NewArray(rootLength << 1, DefaultValue);
    }
    void Update(int index) => tree[index] = Operate(tree[2 * index], tree[2 * index + 1]);

    void AllApply(int k, TOp f)
    {
        tree[k] = ApplyLazy(tree[k], f);
        if (k < rootLength) lazy[k] = Merge(f, lazy[k]);
    }
    void Push(int k)
    {
        AllApply(2 * k, lazy[k]);
        AllApply(2 * k + 1, lazy[k]);
        lazy[k] = DefaultLazy;
    }
    public TValue this[int index]
    {
        set
        {
            index += rootLength;
            for (int i = log; i >= 1; i--) Push(index >> i);
            tree[index] = value;
            for (int i = 1; i <= log; i++) Update(index >> i);
        }
        get
        {
            index += rootLength;
            for (int i = log; i >= 1; i--) Push(index >> i);
            return tree[index];
        }
    }

    public TValue Slice(int from, int length) => Query(from, from + length);
    public TValue Query(int from, int toExclusive)
    {

        if (from == toExclusive) return DefaultValue;

        from += rootLength;
        toExclusive += rootLength;

        for (int i = log; i >= 1; i--)
        {
            if (((from >> i) << i) != from) Push(from >> i);
            if (((toExclusive >> i) << i) != toExclusive) Push(toExclusive >> i);
        }

        TValue sml = DefaultValue, smr = DefaultValue;
        while (from < toExclusive)
        {
            if ((from & 1) != 0) sml = Operate(sml, tree[from++]);
            if ((toExclusive & 1) != 0) smr = Operate(tree[--toExclusive], smr);
            from >>= 1;
            toExclusive >>= 1;
        }

        return Operate(sml, smr);
    }
    public TValue QueryAll() => tree[1];


    public void Apply(int index, TOp f)
    {
        index += rootLength;
        for (int i = log; i >= 1; i--) Push(index >> i);
        tree[index] = ApplyLazy(tree[index], f);
        for (int i = 1; i <= log; i++) Update(index >> i);
    }
    public void Apply(int from, int toExclusive, TOp f)
    {
        if (from == toExclusive) return;

        from += rootLength;
        toExclusive += rootLength;

        for (int i = log; i >= 1; i--)
        {
            if (((from >> i) << i) != from) Push(from >> i);
            if (((toExclusive >> i) << i) != toExclusive) Push((toExclusive - 1) >> i);
        }

        {
            int l2 = from, r2 = toExclusive;
            while (from < toExclusive)
            {
                if ((from & 1) != 0) AllApply(from++, f);
                if ((toExclusive & 1) != 0) AllApply(--toExclusive, f);
                from >>= 1;
                toExclusive >>= 1;
            }
            from = l2;
            toExclusive = r2;
        }

        for (int i = 1; i <= log; i++)
        {
            if (((from >> i) << i) != from) Update(from >> i);
            if (((toExclusive >> i) << i) != toExclusive) Update((toExclusive - 1) >> i);
        }
    }

    /** <summary>二分探索</summary><returns>[r = l もしくは f(op(a[l], a[l + 1], ..., a[r - 1])) = true]&amp;&amp;[r = n もしくは f(op(a[l], a[l + 1], ..., a[r])) = false]となるrのいずれか。aが単調ならば前者を満たす最大のr</returns>*/
    public int MaxRight(int left, Predicate<TValue> ok)
    {
        throw new NotImplementedException();
    }
    /** <summary>二分探索</summary><returns>[l = r もしくは f(op(a[l], a[l + 1], ..., a[r - 1])) = true]&amp;&amp;[l = 0 もしくは f(op(a[l - 1], a[l + 1], ..., a[r - 1])) = false]となるlのいずれか。aが単調ならば前者を満たす最小のl</returns>*/
    public int MinLeft(int rightExclude, Predicate<TValue> ok)
    {
        throw new NotImplementedException();
    }

    [System.Diagnostics.DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
    struct KeyValuePairs
    {
        string key;
        (TValue value, TOp lazy) value;

        public KeyValuePairs(string key, (TValue value, TOp lazy) value)
        {
            this.key = key;
            this.value = value;
        }
    }
    class SegmentTreeLazyDebugView
    {
        SegmentTreeLazyAbstract<TValue, TOp> segmentTree;
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
                        var index = i + len;
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
