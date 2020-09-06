using System;
using System.Collections.Generic;
using static AtCoderProject.Global;

class SegmentTree : SegmentTreeAbstract<long>
{
    protected override long DefaultValue => 0;
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
        var rootLength = this.rootLength;
        Array.Copy(initArray, 0, tree, rootLength - 1, initArray.Length);
        for (int i = rootLength - 2; i >= 0; i--)
            tree[i] = Operate(tree[(i << 1) + 1], tree[(i << 1) + 2]);
    }
    public SegmentTreeAbstract(int size)
    {
        this.Length = size;
        rootLength = 1 << (MSB(size - 1) + 1);
        tree = NewArray((rootLength << 1) - 1, DefaultValue);
    }

    public void Update(int index, T value)
    {
        index += rootLength - 1;
        tree[index] = value;
        while (index > 0)
        {
            index = (index - 1) >> 1;
            tree[index] = Operate(tree[index * 2 + 1], tree[index * 2 + 2]);
        }
    }

    public T Slice(int from, int length) => Query(from, from + length);
    public T Query(int fromInclusive, int toExclusive)
    {
        var leftResult = DefaultValue;
        var rightResult = DefaultValue;
        var segSize = rootLength - 1;
        var l = fromInclusive + segSize;
        var r = toExclusive + segSize;

        while (l < r)
        {
            if ((l & 1) == 0)
                leftResult = Operate(leftResult, tree[l]);
            if ((r & 1) == 0)
                rightResult = Operate(tree[r - 1], rightResult);
            l /= 2;
            r = (r - 1) / 2;
        }

        return Operate(leftResult, rightResult);
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
                        var index = i + len - 1;
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
