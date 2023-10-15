using AtCoder;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /**
     * <summary>データ付き最小共通祖先</summary> 
     */
    public class LowestCommonAncestorWithData<TNode, TEdge, T, TOp>
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
        where TOp : ISegtreeOperator<T>
    {
        internal readonly TNode[] tree;

        /// <summary>
        /// kprv[u][k] 頂点uの2^k個上の祖先頂点v, 0&lt;=k&lt;logN
        /// </summary>
        private readonly PathDoubling<T, TOp> doubling;
        public LowestCommonAncestorWithData(TNode[] tree, T[] data, TOp op = default)
        {
            if (tree.Length == 0) throw new ArgumentException(nameof(tree));
            if (tree.Length != data.Length)
                throw new ArgumentException("データと木の長さが異なります", nameof(data));

            this.tree = tree;
            var parents = new int[tree.Length];
            for (int v = 0; v < tree.Length; v++)
                parents[v] = tree[v].Parent.To;
            doubling = new PathDoubling<T, TOp>(parents, data, tree.Length, op);
        }

        /// <summary>
        /// 2つの頂点の共通祖先とその経路のデータをマージした値を取得する
        /// </summary>
        public (int Index, T Data) this[int u, int v]
        {
            [凾(256)]
            get => Lca(u, v);
        }

        /// <summary>
        /// 2つの頂点の共通祖先とその経路のデータをマージした値を取得する
        /// </summary>
        [凾(256)]
        public (int Index, T Data) Lca(int u, int v)
        {
            var op = doubling.op;
            int dd = tree[u].Depth - tree[v].Depth;
            T dres = op.Identity;
            if (dd > 0)
                (u, dres) = doubling.Move(u, dd);
            else if (dd < 0)
                (v, dres) = doubling.Move(v, -dd);
            if (u == v)
                return (u, dres);

            var paths = doubling.paths;
            for (int i = paths.Length - 1; i >= 0; i--)
            {
                var path = paths[i];
                var ds = doubling.data[i];
                if (path[u] != path[v])
                {
                    dres = op.Operate(op.Operate(dres, ds[u]), ds[v]);
                    u = path[u];
                    v = path[v];
                }
            }
            return (paths[0][u], op.Operate(dres, op.Operate(doubling.data[0][v], doubling.data[0][u])));
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
                var u2 = doubling.Move(u, dd).To;
                if (u2 == v)
                    return (doubling.Move(u, dd - 1).To, v);
                u = u2;
            }
            else if (dd < 0)
            {
                var v2 = doubling.Move(v, -dd).To;
                if (v2 == u)
                    return (u, doubling.Move(v, -dd - 1).To);
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
            var lca = Lca(u, v).Index;
            return tree[u].Depth + tree[v].Depth - 2 * tree[lca].Depth;
        }

        /// <summary>
        /// 頂点 <paramref name="v"/> から <paramref name="up"/> だけ根に向かった頂点とその経路の値を返します。根を通り過ぎる場合は -1 を返します。
        /// </summary>
        [凾(256)]
        public (int Index, T Data) Ascend(int v, int up)
        {
            if (up <= tree[v].Depth)
                return doubling.Move(v, up);
            return (-1, doubling.op.Identity);
        }
    }

    public static class LowestCommonAncestorWithDataExt
    {
        public readonly struct Builder<TNode, TEdge>
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
        {
            private readonly ITreeGraph<TNode, TEdge> tree;
            public Builder(ITreeGraph<TNode, TEdge> tree)
            {
                this.tree = tree;
            }

            public LowestCommonAncestorWithData<TNode, TEdge, T, TOp>
                Build<T, TOp>(T[] data, TOp op = default)
                where TOp : ISegtreeOperator<T>
                => new LowestCommonAncestorWithData<TNode, TEdge, T, TOp>(tree.AsArray(), data, op);
        }

        /**
         * <summary>データ付き最小共通祖先</summary> 
         */
        public static Builder<TNode, TEdge>
            LowestCommonAncestorWithDataBuilder<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
            => new Builder<TNode, TEdge>(tree);
    }
}