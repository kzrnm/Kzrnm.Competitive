using System;
using System.Collections.Generic;
using static AtCoderProject.Global;
using static AtCoderProject.NumGlobal;


[System.Diagnostics.DebuggerTypeProxy(typeof(SegmentTreeDebugView))]
class SegmentTree
{
    /* この辺は場合によって変える */
    private long defaultValue = 0;
    private long Operate(long v1, long v2)
    {
        return Math.Max(v1, v2);
    }

    private int rootLength;
    private long[] tree;

    public SegmentTree(ReadOnlySpan<long> initSpan) : this(initSpan.Length)
    {
        for (var i = 0; i < initSpan.Length; i++)
            Update(i, initSpan[i]);
    }
    public SegmentTree(int size)
    {
        rootLength = 1 << (MSB(size) + 1);
        tree = NewArray(2 * rootLength - 1, defaultValue);
    }

    public void Update(int index, long value)
    {
        index += rootLength - 1;
        tree[index] = value;
        while (index > 0)
        {
            index = (index - 1) >> 1;
            tree[index] = Operate(tree[index * 2 + 1], tree[index * 2 + 2]);
        }
    }

    public long Query(int fromInclusive, int toExclusive)
    {
        var leftResult = defaultValue;
        var rightResult = defaultValue;
        var l = fromInclusive + rootLength - 1;
        var r = toExclusive + rootLength - 1;

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


    [System.Diagnostics.DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + "}")]
    internal struct KeyValuePairs
    {
        private string key;
        private long value;

        public KeyValuePairs(string key, long value)
        {
            this.key = key;
            this.value = value;
        }
    }
    class SegmentTreeDebugView
    {
        private SegmentTree segmentTree;
        public SegmentTreeDebugView(SegmentTree segmentTree)
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
                            keys.Add(new KeyValuePairs($"{i}", segmentTree.tree[index]));
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
