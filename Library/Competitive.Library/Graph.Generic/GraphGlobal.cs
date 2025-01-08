using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public interface IGraphNode<out Te>
    {
        /// <summary>
        /// ノードのインデックス
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 入ってくる辺の向いてる先
        /// </summary>
        Te[] Parents { get; }
        /// <summary>
        /// 出ている辺の向いてる先
        /// </summary>
        Te[] Children { get; }
        /// <summary>
        /// 有向グラフかどうか
        /// </summary>
        bool IsDirected { get; }
    }
    public interface IGraphNode<TSelf, Te> : IGraphNode<Te>
    {
        static abstract TSelf Node(int i, Te[] parents, Te[] children);
    }
    public interface ITreeNode<Te>
    {
        /// <summary>
        /// ノードのインデックス
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 親ノード
        /// </summary>
        Te Parent { get; }
        /// <summary>
        /// 子ノード
        /// </summary>
        Te[] Children { get; }
        /// <summary>
        /// 何個遡ったら根になるか
        /// </summary>
        int Depth { get; }
        /// <summary>
        /// 部分木のサイズ
        /// </summary>
        int Size { get; }
    }
    public interface ITreeNode<TSelf, Te> : ITreeNode<Te>
    {
        static abstract TSelf Node(int i, int size, TSelf parent, Te parentEdge, Te[] children);
        static abstract TSelf RootNode(int i, int size, Te[] children);
    }
    public interface IGraph<Te>
    {
        Csr<Te> Edges { get; }
        GraphNode<Te>[] AsArray();
        GraphNode<Te> this[int index] { get; }
        int Length { get; }
    }
    public interface IGraph<Tn, Te> : IGraph<Te>
        where Tn : GraphNode<Te>
    {
        GraphNode<Te>[] IGraph<Te>.AsArray() => AsArray();
        new Tn[] AsArray();
        GraphNode<Te> IGraph<Te>.this[int index] => this[index];
        new Tn this[int index] { get; }
    }
    public interface IGraph<TSelf, Tn, Te> : IGraph<Tn, Te>
        where Tn : GraphNode<Te>
    {
        static abstract TSelf Graph(Tn[] nodes, Csr<Te> edges);
    }
    public interface ITreeGraph<TNode, Te>
        where TNode : ITreeNode<Te>
        where Te : IGraphEdge
    {
        int Root { get; }
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
        HeavyLightDecomposition<TNode, Te> HlDecomposition { get; }
    }
    public interface ITreeGraph<TSelf, TNode, Te> : ITreeGraph<TNode, Te>
        where TNode : ITreeNode<Te>
        where Te : IGraphEdge
    {
        static abstract TSelf Tree(TNode[] nodes, int root, HeavyLightDecomposition<TNode, Te> hl);
    }
    #endregion Interfaces

    #region classes
    /// <summary>
    /// グラフのノードを表す
    /// </summary>
    /// <typeparam name="Te">辺の型</typeparam>
    public class GraphNode<Te> : IGraphNode<GraphNode<Te>, Te>
    {
        public GraphNode(int i, Te[] parents, Te[] children)
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
        public Te[] Parents { get; }
        /// <summary>
        /// 出ている辺の向いてる先
        /// </summary>
        public Te[] Children { get; }
        /// <summary>
        /// 有向グラフかどうか
        /// </summary>
        public bool IsDirected => Parents != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override int GetHashCode() => Index;
        static GraphNode<Te> IGraphNode<GraphNode<Te>, Te>.Node(int i, Te[] parents, Te[] children)
            => new(i, parents, children);
    }
    /// <summary>
    /// 木のノードを表す
    /// </summary>
    /// <typeparam name="Te">辺の型</typeparam>
    public class TreeNode<Te> : ITreeNode<TreeNode<Te>, Te> where Te : IGraphEdge<Te>
    {
        public TreeNode(int i, int size, Te parent, int depth, Te[] children)
        {
            Index = i;
            Parent = parent;
            Children = children;
            Depth = depth;
            Size = size;
        }
        public int Index { get; }
        public Te Parent { get; }
        public Te[] Children { get; }
        public int Depth { get; }
        public int Size { get; }
        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override int GetHashCode() => Index;

        [凾(256)]
        static TreeNode<Te> ITreeNode<TreeNode<Te>, Te>.Node(int i, int size, TreeNode<Te> parent, Te edge, Te[] children)
            => new(i, size, edge.Reversed(parent.Index), parent.Depth + 1, children);
        [凾(256)]
        static TreeNode<Te> ITreeNode<TreeNode<Te>, Te>.RootNode(int i, int size, Te[] children)
            => new(i, size, Te.None, 0, children);
    }
    #endregion classes
    namespace Internal.Graph
    {
        internal class EdgeContainer<T> where T : IGraphEdge<T>
        {
            public int Length { get; }
            public bool IsDirected { get; }
            public readonly List<(int from, T edge)> edges;
            public readonly int[] sizes;
            public readonly int[] parentSizes;
            public EdgeContainer(int size, bool isDirected)
            {
                Length = size;
                IsDirected = isDirected;
                sizes = new int[size];
                parentSizes = isDirected ? new int[size] : sizes;
                edges = new List<(int from, T edge)>(size);
            }
            [凾(256)]
            public void Add(int from, T edge)
            {
                ++sizes[from];
                ++parentSizes[edge.To];
                edges.Add((from, edge));
            }

            [凾(256)]
            public Csr<T> ToCsr() => new Csr<T>(Length, edges);
        }

        public abstract class BuilderBase<Tg, Tr, Tn, Tt, Te>
            where Tg : IGraph<Tg, Tn, Te>
            where Tr : ITreeGraph<Tr, Tt, Te>
            where Tn : GraphNode<Te>
            where Tt : ITreeNode<Te>
            where Te : IGraphEdge<Te>
        {
            internal readonly EdgeContainer<Te> edges;

            public BuilderBase(int size, bool isDirected)
            {
                edges = new(size, isDirected);
            }
            /// <summary>
            /// グラフを構築します。
            /// </summary>
            /// <returns>構築されたグラフ</returns>
            public Tg ToGraph()
            {
                var res = new Tn[edges.Length];
                var csr = edges.ToCsr();
                var counter = new int[res.Length];
                var children = SizeToArray<Te>(edges.sizes);
                var (parentCounter, parents) = edges.IsDirected ? (new int[res.Length], SizeToArray<Te>(edges.parentSizes)) : (counter, children);
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = Node(i, parents[i], children[i]);
                    foreach (var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                    {
                        var to = e.To;
                        children[i][counter[i]++] = e;
                        parents[to][parentCounter[to]++] = e.Reversed(i);
                    }
                }
                return Tg.Graph(res, csr);
            }

            /// <summary>
            /// <paramref name="root"/> を根とする木を構築します。
            /// </summary>
            /// <param name="root">根となるノード</param>
            /// <returns>構築された木</returns>

            public Tr ToTree(int root = 0)
            {
                var eg = edges;
                int size = eg.Length;
                Contract.Assert(!eg.IsDirected, "木には無向グラフをしたほうが良い");
                var tedges = SizeToArray<Te>(eg.sizes);
                var idxs = new int[tedges.Length];
                foreach (var (from, e) in eg.edges.AsSpan())
                {
                    var to = e.To;
                    tedges[from][idxs[from]++] = e;
                    tedges[to][idxs[to]++] = e.Reversed(from);
                }

                idxs.AsSpan().Clear();
                var sz = new int[size].Fill(1); // 部分木のサイズ
                var children = SizeToArray<Te>(eg.sizes, -1, root);

                // 深さ優先探索で構築
                var parent = new int[size];
                parent[root] = -1;
                var stack = new Stack<(int Cur, int Chix)>(size);
                stack.Push((root, 0));
                while (stack.TryPop(out var cur, out var ci))
                {
                    var es = tedges[cur];
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
                var nodes = new Tt[size];
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
                            ? RootNode(cur, sz[cur], children[cur])
                            : TreeNode(cur, sz[cur], nodes[parent[cur]], tedges[cur][^1], children[cur]);
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
                var hl = new HeavyLightDecomposition<Tt, Te>(nodes, nxt, down, up);
                return Tr.Tree(nodes, root, hl);
            }


            /// <summary>
            /// a[i] の要素数が <paramref name="szs"/>[i] であるジャグ配列を返します。
            /// </summary>
            /// <param name="szs">サイズ配列</param>
            /// <param name="offset"><paramref name="root"/> 以外のオフセット</param>
            /// <param name="root">木の根</param>
            /// <returns></returns>
            [凾(256)]
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
            protected abstract Tn Node(int i, Te[] parents, Te[] children);
            protected abstract Tt RootNode(int i, int size, Te[] children);
            protected abstract Tt TreeNode(int i, int size, Tt parent, Te parentEdge, Te[] children);
        }

        public class Builder<Tg, Tr, Tn, Tt, Te> : BuilderBase<Tg, Tr, Tn, Tt, Te>
            where Tg : IGraph<Tg, Tn, Te>
            where Tr : ITreeGraph<Tr, Tt, Te>
            where Tn : GraphNode<Te>, IGraphNode<Tn, Te>
            where Tt : ITreeNode<Tt, Te>
            where Te : IGraphEdge<Te>
        {
            public Builder(int size, bool isDirected) : base(size, isDirected) { }
            protected override Tn Node(int i, Te[] parents, Te[] children)
                => Tn.Node(i, parents, children);
            protected override Tt RootNode(int i, int size, Te[] children)
                => Tt.RootNode(i, size, children);
            protected override Tt TreeNode(int i, int size, Tt parent, Te parentEdge, Te[] children)
                => Tt.Node(i, size, parent, parentEdge, children);
        }
    }

    public static class GraphExtensions
    {
        [凾(256)]
        public static int LowestCommonAncestor<TNode, Te>(this ITreeGraph<TNode, Te> tree, int u, int v)
        where TNode : ITreeNode<Te>
        where Te : IGraphEdge
            => tree.HlDecomposition.LowestCommonAncestor(u, v);
    }
}
