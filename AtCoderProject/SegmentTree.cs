using System;
using System.Collections.Generic;
using System.Linq;
using static Global;
using static NumGlobal;

namespace AtCoderProject.Hide
{

    [System.Diagnostics.DebuggerTypeProxy(typeof(BinaryIndexedTreeDebugView))]
    class BinaryIndexedTree
    {
        public void Add(int i, long w)
        {
            for (++i; i < tree.Length; i += (i & -i))
                tree[i] += w;
        }
        public long Sum(int toExclusive)
        {
            long res = 0;
            for (var i = toExclusive; i > 0; i -= (i & -i))
                res += tree[i];
            return res;
        }
        public long Sum(int from, int toExclusive) => Sum(toExclusive) - Sum(from);

        private long[] tree;

        public BinaryIndexedTree(int size)
        {
            tree = new long[size + 1];
        }

        public class BinaryIndexedTreeDebugView
        {
            private BinaryIndexedTree bit;
            public BinaryIndexedTreeDebugView(BinaryIndexedTree bit)
            {
                this.bit = bit;
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public string[] Tree
            {
                get
                {
                    var res = new string[bit.tree.Length - 1];
                    for (var i = 0; i < res.Length; i++)
                    {
                        res[i] = $"real:{bit.tree[i + 1]} num:{bit.Sum(i, i + 1)}";
                    }
                    return res;
                }
            }
        }
    }

    [System.Diagnostics.DebuggerTypeProxy(typeof(SegmentTreeDebugView))]
    class SegmentTree
    {
        // この辺は場合によって変える
        private long defaultValue = 0;
        private long Operate(long v1, long v2)
        {
            return v1 + v2;
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

}
