using System;
using System.Collections.Generic;
using static AtCoderProject.Global;

class SegmentTree : SegmentTreeAbstract<long>
{
    protected override long DefaultValue => long.MinValue;
    protected override long Operate(long v1, long v2) => Math.Max(v1, v2);
    public SegmentTree(long[] initArray) : base(initArray) { }
    public SegmentTree(int size) : base(size) { }
}
[System.Diagnostics.DebuggerTypeProxy(typeof(SegmentTreeAbstract<>.DebugView))]
abstract class SegmentTreeAbstract<T> where T : struct
{
    protected virtual T DefaultValue => default;
    protected abstract T Operate(T v1, T v2);

    protected T[] tree;
    public readonly int rootLength;
    public int Length { get; }

    public SegmentTreeAbstract(T[] initArray) : this(initArray.Length)
    {
        Array.Copy(initArray, 0, tree, rootLength, initArray.Length);
        for (int i = rootLength - 1; i >= 1; i--)
            Update(i);
    }
    public SegmentTreeAbstract(int size)
    {
        this.Length = size;
        rootLength = 1 << (MSB(size - 1) + 1);
        tree = NewArray(rootLength << 1, DefaultValue);
    }
    void Update(int index) => tree[index] = Operate(tree[2 * index], tree[2 * index + 1]);
    public T this[int index]
    {
        set
        {
            index += rootLength;
            tree[index] = value;
            while (index > 0)
                Update(index >>= 1);
        }
        get => tree[index + rootLength];
    }

    public T Slice(int from, int length) => Query(from, from + length);
    public T Query(int fromInclusive, int toExclusive)
    {
        var leftResult = DefaultValue;
        var rightResult = DefaultValue;
        var l = fromInclusive + rootLength;
        var r = toExclusive + rootLength;

        while (l < r)
        {
            if ((l & 1) != 0)
                leftResult = Operate(leftResult, tree[l++]);
            if ((r & 1) != 0)
                rightResult = Operate(tree[--r], rightResult);
            l >>= 1;
            r >>= 1;
        }

        return Operate(leftResult, rightResult);
    }
    public T QueryAll() => tree[1];

    /** <summary>二分探索</summary><returns>[r = l もしくは f(op(a[l], a[l + 1], ..., a[r - 1])) = true]&amp;&amp;[r = n もしくは f(op(a[l], a[l + 1], ..., a[r])) = false]となるrのいずれか。aが単調ならば前者を満たす最大のr</returns>*/
    public int MaxRight(int left, Predicate<T> ok)
    {
        System.Diagnostics.Debug.Assert(ok(DefaultValue));
        if (left >= this.Length) return this.Length;
        left += rootLength;
        var sm = DefaultValue;

        do
        {
            while (left % 2 == 0) left >>= 1;
            if (!ok(Operate(sm, tree[left])))
            {
                while (left < rootLength)
                {
                    left = (2 * left);
                    if (ok(Operate(sm, tree[left])))
                    {
                        sm = Operate(sm, tree[left]);
                        left++;
                    }
                }
                return left - rootLength;
            }
            sm = Operate(sm, tree[left]);
            left++;
        } while ((left & -left) != left);
        return this.Length;
    }
    /** <summary>二分探索</summary><returns>[l = r もしくは f(op(a[l], a[l + 1], ..., a[r - 1])) = true]&amp;&amp;[l = 0 もしくは f(op(a[l - 1], a[l + 1], ..., a[r - 1])) = false]となるlのいずれか。aが単調ならば前者を満たす最小のl</returns>*/
    public int MinLeft(int rightExclude, Predicate<T> ok)
    {
        System.Diagnostics.Debug.Assert(ok(DefaultValue));
        if (rightExclude == 0) return 0;
        rightExclude += rootLength;
        var sm = DefaultValue;
        do
        {
            rightExclude--;
            while (rightExclude > 1 && (rightExclude % 2) != 0) rightExclude >>= 1;
            if (!ok(Operate(tree[rightExclude], sm)))
            {
                while (rightExclude < rootLength)
                {
                    rightExclude = (2 * rightExclude + 1);
                    if (ok(Operate(tree[rightExclude], sm)))
                    {
                        sm = Operate(tree[rightExclude], sm);
                        rightExclude--;
                    }
                }
                return rightExclude + 1 - rootLength;
            }
            sm = Operate(tree[rightExclude], sm);
        } while ((rightExclude & -rightExclude) != rightExclude);
        return 0;
    }



    [System.Diagnostics.DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
    struct KeyValuePairs
    {
        string key;
        T value;

        public KeyValuePairs(string key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }
    class DebugView
    {
        SegmentTreeAbstract<T> segmentTree;
        public DebugView(SegmentTreeAbstract<T> segmentTree)
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
                            keys.Add(new KeyValuePairs($"[{i}]", segmentTree.tree[index]));
                        else
                        {
                            var from = i * unit;
                            keys.Add(new KeyValuePairs($"[{from}-{from + unit})", segmentTree.tree[index]));
                        }
                    }
                }
                return keys.ToArray();
            }
        }
    }
}
