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
