using System;
using static AtCoderProject.Global;



[System.Diagnostics.DebuggerTypeProxy(typeof(DebugView))]
class BinaryIndexedTree
{
    long Operate(long v, long w) => v + w;
    public void Add(int i, long w)
    {
        for (++i; i < tree.Length; i += (i & -i))
            tree[i] = Operate(tree[i], w);
    }
    public long Query(int toExclusive)
    {
        long res = 0;
        for (var i = toExclusive; i > 0; i -= (i & -i))
            res = Operate(res, tree[i]);
        return res;
    }

    public long Query(int from, int toExclusive) => Query(toExclusive) - Query(from);
    ///* <summary>非負整数だけが使われる総和を求めるBITにだけ使える二分探索。0,1で便利</summary> */
    public int LowerBound(long w)
    {
        if (w <= 0) return 0;
        int x = 0;
        for (int k = 1 << MSB(tree.Length - 1); k > 0; k >>= 1)
        {
            var nx = x + k;
            if (nx < tree.Length && tree[nx] < w)
            {
                x = nx;
                w -= tree[nx];
            }
        }
        return x;
    }

    long[] tree;
    public int Length => tree.Length - 1;
    public long Slice(int from, int length) => Query(from + length) - Query(from);

    public BinaryIndexedTree(int size)
    {
        tree = new long[size + 1];
    }
    public BinaryIndexedTree(long[] initArray) : this(initArray.Length)
    {
        Array.Copy(initArray, 0, tree, 1, initArray.Length);
        for (int i = 1; i < tree.Length; i++)
        {
            var ni = i + (i & -i);
            if (ni < tree.Length)
                tree[ni] = Operate(tree[i], tree[ni]);
        }
    }

    public class DebugView
    {
        BinaryIndexedTree bit;
        public DebugView(BinaryIndexedTree bit)
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
                    res[i] = $"data:{bit.tree[i + 1]} single:{bit.Query(i, i + 1)}";
                }
                return res;
            }
        }
    }
}
