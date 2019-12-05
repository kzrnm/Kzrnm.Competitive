using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtCoderProject.Hide
{
#pragma warning disable
    [System.Diagnostics.DebuggerTypeProxy(typeof(SegmentTreeDebugView))]
    class SegmentTree
    {
        // この辺は場合によって変える
        private long defaultValue = 0;
        private long Operate(long v1, long v2)
        {
            return v1 + v2;
        }

        private int origLength;
        private int rootLength;
        private long[] tree;

        public SegmentTree(long[] initArray) : this(initArray.Length)
        {
            for (int i = 0; i < initArray.Length; i++)
                Update(i, initArray[i]);
        }
        public SegmentTree(int size)
        {
            origLength = size;
            for (rootLength = 1; rootLength <= size; rootLength <<= 1) { }
            tree = Enumerable.Repeat(defaultValue, 2 * rootLength - 1).ToArray();
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
            var result = defaultValue;
            var l = fromInclusive + rootLength - 1;
            var r = toExclusive + rootLength - 1;

            while (l < r)
            {
                if ((l & 1) == 0)
                    result = Operate(result, tree[l]);
                if ((r & 1) == 0)
                    result = Operate(result, tree[r - 1]);
                l = l / 2;
                r = (r - 1) / 2;
            }

            return result;
        }


        [System.Diagnostics.DebuggerDisplay("{value}", Name = "{key}")]
        internal class KeyValuePairs
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
                    for (int len = segmentTree.rootLength; len > 0; len >>= 1)
                    {
                        var unit = segmentTree.rootLength / len;
                        for (int i = 0; i < len; i++)
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
