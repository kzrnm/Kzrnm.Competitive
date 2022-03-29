using AtCoder.Internal;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
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
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = SizeToArray<TEdge>(edgeContainer.sizes);
            var roots = edgeContainer.IsDirected ? SizeToArray<TEdge>(edgeContainer.rootSizes) : children;
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = op.Node(i, roots[i], children[i]);
                foreach (var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    var to = e.To;
                    children[i][counter[i]++] = e;
                    roots[to][rootCounter[to]++] = e.Reversed(i);
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
                if (--ci >= 0)
                {
                    // まず一つ前の子をチェック
                    var tp = es[ci].To;
                    sz[cur] += sz[tp];
                    // HL分解のため最大の子を先頭に出しておく
                    if (sz[tp] > sz[es[0].To])
                        (children[cur][0], children[cur][ci]) = (children[cur][ci], children[cur][0]);
                }
                if (++ci < children[cur].Length)
                {
                    var to = es[ci].To;
                    if (parent[cur] == to)
                    {
                        // 親は末尾に置く
                        // Roslyn のバグがあるっぽいので参照を取り出してからswap https://github.com/dotnet/roslyn/issues/58472
                        var b = es.Length - 1;
                        (es[ci], es[b]) = (es[b], es[ci]);
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
}
