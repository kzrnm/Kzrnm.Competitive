using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtCoderProject.Hide
{
#pragma warning disable
    class SegmentTree
    {
        // この辺は場合によって変える
        private long defaultValue = 0;
        private long Which(long left, long right)
        {
            return Math.Max(left, right);
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
                tree[index] = Which(tree[index * 2 + 1], tree[index * 2 + 2]);
            }
        }

        public long Query(int fromInclusive, int toExclusive) => Query(fromInclusive, toExclusive, 0, 0, rootLength);
        public long Query(int fromInclusive, int toExclusive, int nodeIndex, int nodeStart, int nodeEnd)
        {
            if (nodeEnd <= fromInclusive || toExclusive <= nodeStart) return defaultValue;
            if (fromInclusive <= nodeStart && nodeEnd <= toExclusive) return tree[nodeIndex];

            var left = Query(fromInclusive, toExclusive, nodeIndex * 2 + 1, nodeStart, (nodeStart + nodeEnd) / 2);
            var right = Query(fromInclusive, toExclusive, nodeIndex * 2 + 2, (nodeStart + nodeEnd) / 2, nodeEnd);
            return Which(left, right);
        }
    }
}
