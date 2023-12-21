using AtCoder.Internal;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


// グラフ関連で使うやつ全部入れる
namespace Kzrnm.Competitive
{
    #region Interfaces
    public interface IGraphEdge
    {
        /// <summary>
        /// 向き先
        /// </summary>
        int To { get; }
    }
    public interface IGraphEdge<TSelf> : IGraphEdge where TSelf : IGraphEdge<TSelf>
    {
        /// <summary>
        /// <paramref name="from"/> と <see cref="IGraphEdge.To"/> を逆にします。
        /// </summary>
        /// <returns><see cref="IGraphEdge.To"/> が <paramref name="from"/> になった <typeparamref name="TSelf"/></returns>
        TSelf Reversed(int from);

        /// <summary>
        /// 木の根の親(存在しない)に向かう存在しない辺を返します。
        /// </summary>
        static abstract TSelf None { get; }
    }
    public interface IGraphData<T> : IGraphEdge
    {
        /// <summary>
        /// 自由なデータ
        /// </summary>
        T Data { get; }
    }
    public interface IGraphNode<out TEdge>
    {
        /// <summary>
        /// ノードのインデックス
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 入ってくる辺の向いてる先
        /// </summary>
        TEdge[] Parents { get; }
        /// <summary>
        /// 出ている辺の向いてる先
        /// </summary>
        TEdge[] Children { get; }
        /// <summary>
        /// 有向グラフかどうか
        /// </summary>
        bool IsDirected { get; }
    }
    public interface ITreeNode<TEdge>
    {
        /// <summary>
        /// ノードのインデックス
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 親ノード
        /// </summary>
        TEdge Parent { get; }
        /// <summary>
        /// 子ノード
        /// </summary>
        TEdge[] Children { get; }
        /// <summary>
        /// 何個遡ったら根になるか
        /// </summary>
        int Depth { get; }
        /// <summary>
        /// 部分木のサイズ
        /// </summary>
        int Size { get; }
    }
    public interface ITreeNode<TSelf, TEdge> : ITreeNode<TEdge>
    {
        static abstract TSelf Node(int i, int size, TSelf parent, TEdge parentEdge, TEdge[] children);
        static abstract TSelf RootNode(int i, int size, TEdge[] children);
    }
    public interface IGraph<TEdge>
    {
        Csr<TEdge> Edges { get; }
        GraphNode<TEdge>[] AsArray();
        GraphNode<TEdge> this[int index] { get; }
        int Length { get; }
    }
    public interface IGraph<TSelf, TEdge> : IGraph<TEdge>
    {
        static abstract TSelf Graph(GraphNode<TEdge>[] nodes, Csr<TEdge> edges);
    }
    public interface ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
    {
        int Root { get; }
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
        HeavyLightDecomposition<TNode, TEdge> HlDecomposition { get; }
    }
    public interface ITreeGraph<TSelf, TNode, TEdge> : ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
    {
        static abstract TSelf Tree(TNode[] nodes, int root, HeavyLightDecomposition<TNode, TEdge> hl);
    }
    #endregion Interfaces

    #region classes
    public class EdgeContainer<TEdge> where TEdge : IGraphEdge<TEdge>
    {
        public int Length { get; }
        public bool IsDirected { get; }
        public readonly List<(int from, TEdge edge)> edges;
        public readonly int[] sizes;
        public readonly int[] parentSizes;
        public EdgeContainer(int size, bool isDirected)
        {
            Length = size;
            IsDirected = isDirected;
            sizes = new int[size];
            parentSizes = isDirected ? new int[size] : sizes;
            edges = new List<(int from, TEdge edge)>(size);
        }
        [凾(256)]
        public void Add(int from, TEdge edge)
        {
            ++sizes[from];
            ++parentSizes[edge.To];
            edges.Add((from, edge));
        }

        [凾(256)]
        public Csr<TEdge> ToCsr() => new Csr<TEdge>(Length, edges);

        public TGraph ToGraph<TGraph>() where TGraph : IGraph<TGraph, TEdge>
        {
            var res = new GraphNode<TEdge>[Length];
            var csr = ToCsr();
            var counter = new int[res.Length];
            var parentCounter = IsDirected ? new int[res.Length] : counter;
            var children = SizeToArray<TEdge>(sizes);
            var parents = IsDirected ? SizeToArray<TEdge>(parentSizes) : children;
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new(i, parents[i], children[i]);
                foreach (var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    var to = e.To;
                    children[i][counter[i]++] = e;
                    parents[to][parentCounter[to]++] = e.Reversed(i);
                }
            }
            return TGraph.Graph(res, csr);
        }

        public TGraph ToTree<TGraph, TNode>(int root)
            where TGraph : ITreeGraph<TGraph, TNode, TEdge>
            where TNode : class, ITreeNode<TNode, TEdge>
        {
            int size = Length;
            Contract.Assert(!IsDirected, "木には無向グラフをしたほうが良い");
            var edges = SizeToArray<TEdge>(sizes);
            var idxs = new int[edges.Length];
            foreach (var (from, e) in this.edges.AsSpan())
            {
                var to = e.To;
                edges[from][idxs[from]++] = e;
                edges[to][idxs[to]++] = e.Reversed(from);
            }

            idxs.AsSpan().Clear();
            var sz = new int[size].Fill(1); // 部分木のサイズ
            var children = SizeToArray<TEdge>(sizes, -1, root);

            // 深さ優先探索で構築
            var parent = new int[size];
            parent[root] = -1;
            var stack = new Stack<(int Cur, int Chix)>(size);
            stack.Push((root, 0));
            while (stack.TryPop(out var cur, out var ci))
            {
                var es = edges[cur];
                var cs = children[cur];
                if (--ci >= 0)
                {
                    // まず一つ前の子をチェック
                    var tp = es[ci].To;
                    sz[cur] += sz[tp];
                    // HL分解のため最大の子を先頭に出しておく
                    if (sz[tp] > sz[cs[0].To])
                        (cs[0], cs[ci]) = (cs[ci], cs[0]);
                }
                if (++ci < children[cur].Length)
                {
                    var to = es[ci].To;
                    if (parent[cur] == to)
                    {
                        (es[ci], es[^1]) = (es[^1], es[ci]);
                        to = es[ci].To;
                    }
                    stack.Push((cur, ci + 1));
                    stack.Push((to, 0));
                    children[cur][ci] = es[ci];
                    parent[to] = cur;
                }
            }

            var etid = 0;
            var down = new int[size];
            var up = new int[size];
            var nxt = new int[size];
            var nodes = new TNode[size];
            stack.Push((root, 0));
            while (stack.TryPop(out var cur, out var ci))
            {
                var ch = children[cur];
                if (ci == 0)
                {
                    down[cur] = etid++;
                    System.Diagnostics.Debug.Assert(
                        root == cur
                        || (uint)parent[cur] < (uint)nodes.Length
                        && nodes[parent[cur]] != null);
                    nodes[cur] = root == cur
                        ? TNode.RootNode(cur, sz[cur], children[cur])
                        : TNode.Node(cur, sz[cur], nodes[parent[cur]], edges[cur][^1], children[cur]);
                }

                if (ci < ch.Length)
                {
                    var to = ch[ci].To;
                    nxt[to] = ci == 0 ? nxt[cur] : to;
                    stack.Push((cur, ci + 1));
                    stack.Push((to, 0));
                }
                else
                    up[cur] = etid;
            }
            var hl = new HeavyLightDecomposition<TNode, TEdge>(nodes, nxt, down, up);
            return TGraph.Tree(nodes, root, hl);
        }

        /// <summary>
        /// a[i] の要素数が <paramref name="szs"/>[i] であるジャグ配列を返します。
        /// </summary>
        /// <param name="szs">サイズ配列</param>
        /// <param name="offset"><paramref name="root"/> 以外のオフセット</param>
        /// <param name="root">木の根</param>
        /// <returns></returns>
        static T[][] SizeToArray<T>(int[] szs, int offset = 0, int root = -1)
        {
            int r = 0;
            ref var sr = ref r;
            if ((uint)root < (uint)szs.Length)
                sr = ref szs[root];
            sr -= offset;
            var a = new T[szs.Length][];
            for (int i = 0; i < szs.Length; i++)
                a[i] = new T[szs[i] + offset];
            sr += offset;
            return a;
        }
    }
    public class GraphNode<TEdge> : IGraphNode<TEdge>
    {
        public GraphNode(int i, TEdge[] parents, TEdge[] children)
        {
            Index = i;
            Parents = parents;
            Children = children;
        }
        /// <summary>
        /// ノードのインデックス
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// 入ってくる辺の向いてる先
        /// </summary>
        public TEdge[] Parents { get; }
        /// <summary>
        /// 出ている辺の向いてる先
        /// </summary>
        public TEdge[] Children { get; }
        /// <summary>
        /// 有向グラフかどうか
        /// </summary>
        public bool IsDirected => Parents != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override int GetHashCode() => Index;
    }
    public class TreeNode<TEdge> : ITreeNode<TreeNode<TEdge>, TEdge> where TEdge : IGraphEdge<TEdge>
    {
        public TreeNode(int i, int size, TEdge parent, int depth, TEdge[] children)
        {
            Index = i;
            Parent = parent;
            Children = children;
            Depth = depth;
            Size = size;
        }
        public int Index { get; }
        public TEdge Parent { get; }
        public TEdge[] Children { get; }
        public int Depth { get; }
        public int Size { get; }
        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override int GetHashCode() => Index;

        [凾(256)]
        static TreeNode<TEdge> ITreeNode<TreeNode<TEdge>, TEdge>.Node(int i, int size, TreeNode<TEdge> parent, TEdge edge, TEdge[] children)
            => new(i, size, edge.Reversed(parent.Index), parent.Depth + 1, children);
        [凾(256)]
        static TreeNode<TEdge> ITreeNode<TreeNode<TEdge>, TEdge>.RootNode(int i, int size, TEdge[] children)
            => new(i, size, TEdge.None, 0, children);
    }
    #endregion classes

    public static class GraphExtensions
    {
        [凾(256)]
        public static int LowestCommonAncestor<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree, int u, int v)
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
            => tree.HlDecomposition.LowestCommonAncestor(u, v);
    }
}
