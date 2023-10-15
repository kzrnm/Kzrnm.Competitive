using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


// グラフ関連で使うやつ全部入れる
namespace Kzrnm.Competitive
{
    #region Interfaces
    public interface IReversable<T> where T : IGraphEdge
    {
        /// <summary>
        /// <paramref name="from"/> と <see cref="IGraphEdge.To"/> を逆にする。
        /// </summary>
        /// <returns><see cref="IGraphEdge.To"/> が <paramref name="from"/> になった <typeparamref name="T"/></returns>
        T Reversed(int from);
    }
    public interface IGraphEdge
    {
        /// <summary>
        /// 向き先
        /// </summary>
        int To { get; }
    }
    public interface IWGraphEdge<T> : IGraphEdge
    {
        /// <summary>
        /// 重み
        /// </summary>
        T Value { get; }
    }
    public interface IGraphData<T> : IGraphEdge
    {
        /// <summary>
        /// 自由なデータ
        /// </summary>
        T Data { get; }
    }
    public interface IGraphNode<out TEdge> where TEdge : IGraphEdge
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
    public interface ITreeNode<TEdge> where TEdge : IGraphEdge
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
    [IsOperator]
    public interface IGraphBuildOperator<TGraph, TNode, TEdge>
    {
        TNode Node(int i, TEdge[] parents, TEdge[] children);
        TGraph Graph(TNode[] nodes, Csr<TEdge> edges);
    }
    [IsOperator]
    public interface ITreeBuildOperator<TTree, TNode, TEdge>
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
    {
        TNode TreeNode(int i, int size, TNode parent, TEdge parentEdge, TEdge[] children);
        TNode TreeRootNode(int i, int size, TEdge[] children);
        TTree Tree(TNode[] nodes, int root, HeavyLightDecomposition<TNode, TEdge> hl);
    }
    public interface IGraph<TNode, TEdge>
        where TNode : IGraphNode<TEdge>
        where TEdge : IGraphEdge
    {
        Csr<TEdge> Edges { get; }
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
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
    #endregion Interfaces

    #region classes
    public class EdgeContainer<TEdge> where TEdge : IGraphEdge
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
    }
    internal static class GraphBuilderLogic
    {
        [凾(512)]
        public static TGraph ToGraph<TGraph, TNode, TEdge, TOp>(EdgeContainer<TEdge> edgeContainer)
            where TGraph : IGraph<TNode, TEdge>
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge, IReversable<TEdge>
            where TOp : struct, IGraphBuildOperator<TGraph, TNode, TEdge>
        {
            var op = new TOp();
            var res = new TNode[edgeContainer.Length];
            var csr = edgeContainer.ToCsr();
            var counter = new int[res.Length];
            var parentCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = SizeToArray<TEdge>(edgeContainer.sizes);
            var parents = edgeContainer.IsDirected ? SizeToArray<TEdge>(edgeContainer.parentSizes) : children;
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = op.Node(i, parents[i], children[i]);
                foreach (var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    var to = e.To;
                    children[i][counter[i]++] = e;
                    parents[to][parentCounter[to]++] = e.Reversed(i);
                }
            }
            return op.Graph(res, csr);
        }
        [凾(512)]
        public static TGraph ToTree<TGraph, TNode, TEdge, TOp>(EdgeContainer<TEdge> edgeContainer, int root)
            where TGraph : ITreeGraph<TNode, TEdge>
            where TNode : class, ITreeNode<TEdge>
            where TEdge : IGraphEdge, IReversable<TEdge>
            where TOp : struct, ITreeBuildOperator<TGraph, TNode, TEdge>
        {
            var op = new TOp();
            int size = edgeContainer.Length;
            Contract.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var edges = SizeToArray<TEdge>(edgeContainer.sizes);
            var idxs = new int[edges.Length];
            foreach (var (from, e) in edgeContainer.edges.AsSpan())
            {
                var to = e.To;
                edges[from][idxs[from]++] = e;
                edges[to][idxs[to]++] = e.Reversed(from);
            }

            idxs.AsSpan().Clear();
            var sz = new int[size].Fill(1); // 部分木のサイズ
            var children = SizeToArray<TEdge>(edgeContainer.sizes, -1, root);

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
#if NET7_0_OR_GREATER
                        (es[ci], es[^1]) = (es[^1], es[ci]);
#else
                        // 親は末尾に置く
                        // Roslyn のバグがあるっぽいので参照を取り出してからswap https://github.com/dotnet/roslyn/issues/58472
                        var b = es.Length - 1;
                        (es[ci], es[b]) = (es[b], es[ci]);
#endif
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
                        ? op.TreeRootNode(cur, sz[cur], children[cur])
                        : op.TreeNode(cur, sz[cur], nodes[parent[cur]], edges[cur][^1], children[cur]);
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
            return op.Tree(nodes, root, hl);
        }
        static T[][] SizeToArray<T>(int[] sizeArray, int offset = 0, int root = -1)
        {
            var a = new T[sizeArray.Length][];
            for (int i = 0; i < sizeArray.Length; i++)
                a[i] = new T[sizeArray[i] + (root == i ? 0 : offset)];
            return a;
        }
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
