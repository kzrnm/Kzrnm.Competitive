using System;

namespace AtCoder.Graph
{
    /**
     * <summary>最小共通祖先</summary> 
     */
    public class LowestCommonAncestor<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IEdge
    {
        readonly TNode[] tree;

        /// <summary>
        /// kprv[u][k] 頂点uの2^k個上の祖先頂点v, 0&lt;=k&lt;logN
        /// </summary>
        readonly int[][] kprv;
        readonly int logN;
        public LowestCommonAncestor(TNode[] tree)
        {
            if (tree.Length == 0) throw new ArgumentException(nameof(tree));

            this.tree = tree;
            this.logN = Global.MSB(tree.Length) + 1;
            this.kprv = Global.NewArray(tree.Length, logN, 0);
            for (int v = 0; v < tree.Length; v++)
            {
                this.kprv[v][0] = tree[v].Root.To;
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

        /// <summary>
        /// 2つの頂点の共通祖先を取得する
        /// </summary>
        public int GetLca(int u, int v)
        {
            if (tree[u].Depth > tree[v].Depth)
            {
                (u, v) = (v, u);
            }
            for (int k = 0; k <= logN; k++)
            {
                if ((((tree[v].Depth - tree[u].Depth) >> k) & 1) == 1)
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
    }

    public static class LowestCommonAncestorExt
    {
        public static LowestCommonAncestor<TNode, TEdge> LowestCommonAncestor<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IEdge
            => new LowestCommonAncestor<TNode, TEdge>(tree.AsArray());
    }
}