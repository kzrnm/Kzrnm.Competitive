[System.Diagnostics.DebuggerTypeProxy(typeof(BinaryIndexedTreeDebugView))]
class BinaryIndexedTree
{
    private long Operate(long v, long w) => v + w;
    public void Add(int i, long w)
    {
        for (++i; i < tree.Length; i += (i & -i))
            tree[i] = Operate(tree[i], w);
    }
    public long Sum(int toExclusive)
    {
        long res = 0;
        for (var i = toExclusive; i > 0; i -= (i & -i))
            res = Operate(res, tree[i]);
        return res;
    }

    public long Sum(int from, int toExclusive) => Sum(toExclusive) - Sum(from);

    private long[] tree;
    public int Length => tree.Length - 1;
    public long Slice(int from, int length) => Sum(from, from + length);

    public BinaryIndexedTree(int size)
    {
        tree = new long[size + 1];
    }
    public BinaryIndexedTree(long[] initArray) : this(initArray.Length)
    {
        System.Array.Copy(initArray, 0, tree, 1, initArray.Length);
        for (int i = 1; i < tree.Length; i++)
        {
            var ni = i + (i & -i);
            if (ni < tree.Length)
                tree[ni] = Operate(tree[i], tree[ni]);
        }
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
                    res[i] = $"data:{bit.tree[i + 1]} single:{bit.Sum(i, i + 1)}";
                }
                return res;
            }
        }
    }
}
