using System;

namespace AtCoder.Graph
{
    /**
     * <summary>最小共通祖先</summary> 
     */
    public class LowestCommonAncestor<T> where T : ITreeNode
    {
        readonly T[] tree;

        /** <summary>
         *  kprv[u][k] 頂点uの2^k個上の祖先頂点v, 0<=k<logN
         *  </summary>
         */
        readonly int[][] kprv;
        readonly int logN;
        public LowestCommonAncestor(T[] tree)
        {
            if (tree.Length == 0) throw new ArgumentException(nameof(tree));

            this.tree = tree;
            this.logN = Global.MSB(tree.Length) + 1;
            this.kprv = Global.NewArray(tree.Length, logN, 0);
            for (int v = 0; v < tree.Length; v++)
            {
                this.kprv[v][0] = tree[v].Root;
            }
            for (int k = 0; k < logN - 1; k++)
            {
                for (int v = 0; v < tree.Length; v++)
                {
                    if (this.kprv[v][k] < 0)
                        this.kprv[v][k + 1] = -1;
                    else
                        this.kprv[v][k + 1] = this.kprv[this.kprv[v][k]][k];
                }
            }
        }

        public int GetLca(int u, int v)
        {
            if (Depth(u) > Depth(v))
            {
                (u, v) = (v, u);
            }
            for (int k = 0; k <= logN; k++)
            {
                if ((((Depth(v) - Depth(u)) >> k) & 1) == 1)
                {
                    v = kprv[v][k];
                }
            }
            if (u == v)
                return u;

            for (int k = logN - 1; k >= 0; k--)
            {
                if (kprv[u][k] != kprv[v][k] && kprv[u][k] != -1 && kprv[v][k] != -1)
                {
                    u = kprv[u][k];
                    v = kprv[v][k];
                }
            }
            return kprv[u][0];
        }

        int Depth(int index) => tree[index].Depth;
    }

    public static class LowestCommonAncestorExt
    {
        public static LowestCommonAncestor<T> LowestCommonAncestor<T>(this T[] tree) where T : ITreeNode
            => new LowestCommonAncestor<T>(tree);
    }
}