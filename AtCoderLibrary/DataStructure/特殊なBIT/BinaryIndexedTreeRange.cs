namespace AtCoder.DataStructure.Bit
{
    public class BinaryIndexedTreeRange
    {
        void Add(long[] tree, int i, long w)
        {
            for (++i; i < tree1.Length; i += (i & -i))
                tree[i] += w;
        }
        public void Add(int from, int to, long w)
        {
            Add(tree1, from, -w * from);
            Add(tree1, to, w * to);
            Add(tree2, from, w);
            Add(tree2, to, -w);
        }
        long Sum(long[] tree, int toExclusive)
        {
            long res = 0;
            for (var i = toExclusive; i > 0; i -= (i & -i))
                res += tree[i];
            return res;
        }
        public long Sum(int toExclusive) => Sum(tree1, toExclusive) + Sum(tree2, toExclusive) * toExclusive;
        public long Sum(int from, int toExclusive) => Sum(toExclusive) - Sum(from);

        readonly long[] tree1;
        readonly long[] tree2;
        public int Length => tree1.Length - 1;
        public long Slice(int from, int length) => Sum(from, from + length);

        public BinaryIndexedTreeRange(int size)
        {
            tree1 = new long[size + 1];
            tree2 = new long[size + 1];
        }
    }
}