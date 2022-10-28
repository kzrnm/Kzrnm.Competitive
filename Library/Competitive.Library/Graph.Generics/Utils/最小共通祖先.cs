using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /**
     * <summary>最小共通祖先</summary> 
     */
    public class LowestCommonAncestor<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
    {
        internal readonly TNode[] tree;

        /// <summary>
        /// kprv[u][k] 頂点uの2^k個上の祖先頂点v, 0&lt;=k&lt;logN
        /// </summary>
        private readonly PathDoubling doubling;
        public LowestCommonAncestor(TNode[] tree)
        {
            if (tree.Length == 0) throw new ArgumentException(nameof(tree));

            this.tree = tree;
            var parents = new int[tree.Length];
            for (int v = 0; v < tree.Length; v++)
                parents[v] = tree[v].Parent.To;
            doubling = new PathDoubling(parents, tree.Length);
        }

        /// <summary>
        /// 2つの頂点の共通祖先を取得する
        /// </summary>
        public int this[int u, int v]
        {
            [凾(256)]
            get => Lca(u, v);
        }

        /// <summary>
        /// 2つの頂点の共通祖先を取得する
        /// </summary>
        [凾(256)]
        public int Lca(int u, int v)
        {
            int dd = tree[u].Depth - tree[v].Depth;
            if (dd > 0) u = doubling.Move(u, dd);
            else if (dd < 0) v = doubling.Move(v, -dd);
            if (u == v)
                return u;

            var paths = doubling.paths;
            for (int i = paths.Length - 1; i >= 0; i--)
            {
                var path = paths[i];
                if (path[u] != path[v])
                {
                    u = path[u];
                    v = path[v];
                }
            }
            return paths[0][u];
        }

        /// <summary>
        /// <para>2つの頂点の共通祖先のそれぞれの子(最小非共通祖先)を返します。</para>
        /// <para>一方がもう一方の祖先の場合は祖先の方は自身を返します。</para>
        /// </summary>
        [凾(256)]
        public (int uAncestor, int vAncestor) ChildOfLca(int u, int v)
        {
            int dd = tree[u].Depth - tree[v].Depth;
            if (dd > 0)
            {
                var u2 = doubling.Move(u, dd);
                if (u2 == v)
                    return (doubling.Move(u, dd - 1), v);
                u = u2;
            }
            else if (dd < 0)
            {
                var v2 = doubling.Move(v, -dd);
                if (v2 == u)
                    return (u, doubling.Move(v, -dd - 1));
                v = v2;
            }
            else if (u == v)
                return (u, u);

            var paths = doubling.paths;
            for (int i = paths.Length - 1; i >= 0; i--)
            {
                var path = paths[i];
                if (path[u] != path[v])
                {
                    u = path[u];
                    v = path[v];
                }
            }
            return (u, v);
        }

        /// <summary>
        /// 2つの頂点の距離を取得する
        /// </summary>
        [凾(256)]
        public int Distance(int u, int v)
        {
            var lca = this[u, v];
            return tree[u].Depth + tree[v].Depth - 2 * tree[lca].Depth;
        }
    }

    public static class LowestCommonAncestorExt
    {
        public static LowestCommonAncestor<TNode, TEdge> LowestCommonAncestorDoubling<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
            => new LowestCommonAncestor<TNode, TEdge>(tree.AsArray());
    }
}