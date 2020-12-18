namespace AtCoder.DataStructure
{
    public class BinaryIndexedTree2D
    {
        public void Add(int h, int w, long v)
        {
            for (var hh = h + 1; hh < tree.Length; hh += (hh & -hh))
                for (var ww = w + 1; ww < tree[hh].Length; ww += (ww & -ww))
                    tree[hh][ww] += v;
        }
        public long Query(int hExclusive, int wExclusive)
        {
            long res = 0;
            for (var h = hExclusive; h > 0; h -= (h & -h))
                for (var w = wExclusive; w > 0; w -= (w & -w))
                    res += tree[h][w];
            return res;
        }

        readonly long[][] tree;
        public int Length => tree.Length - 1;
        public Slicer Slice(int from, int length) => new Slicer(this, from, from + length);

        public BinaryIndexedTree2D(int H, int W)
        {
            tree = new long[H + 1][];
            for (int i = 0; i < tree.Length; i++) tree[i] = new long[W + 1];
        }

        public ref struct Slicer
        {
            readonly BinaryIndexedTree2D bit;
            readonly int hFrom;
            readonly int hToExclusive;
            public int Length { get; }
            public Slicer(BinaryIndexedTree2D bit, int hFrom, int hToExclusive)
            {
                this.bit = bit;
                this.hFrom = hFrom;
                this.hToExclusive = hToExclusive;
                this.Length = bit.tree[0].Length - 1;
            }
            public long Slice(int wFrom, int length)
            {
                var wToExclusive = wFrom + length;
                return bit.Query(hToExclusive, wToExclusive)
                    - bit.Query(hToExclusive, wFrom)
                    - bit.Query(hFrom, wToExclusive)
                    + bit.Query(hFrom, wFrom);
            }
        }
    }
}