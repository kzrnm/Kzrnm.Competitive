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
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
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
            Contract.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new TNode[edgeContainer.Length];

            var children = SizeToList<TEdge>(edgeContainer.sizes);

            foreach (var (from, e) in edgeContainer.edges.AsSpan())
            {
                var to = e.To;
                children[from].Add(e);
                children[to].Add(e.Reversed(from));
            }

            if (edgeContainer.Length == 1)
                return op.Tree(new[] { op.TreeRootNode(root, Array.Empty<TEdge>()) }, root);

            res[root] = op.TreeRootNode(root, children[root]?.ToArray() ?? Array.Empty<TEdge>());

            var stack = new Stack<(int parent, TEdge edge)>();
            foreach (var e in res[root].Children)
                stack.Push((root, e));

            while (stack.Count > 0)
            {
                var (parent, edge) = stack.Pop();
                var cur = edge.To;
                List<TEdge> childrenBuilder;
                if (parent == -1)
                    childrenBuilder = children[cur];
                else
                {
                    childrenBuilder = new List<TEdge>(children[cur].Count);
                    foreach (var e in children[cur].AsSpan())
                        if (e.To != parent)
                            childrenBuilder.Add(e);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = op.TreeNode(cur, res[parent], edge.Reversed(parent), childrenArr);
                foreach (var e in childrenArr)
                    stack.Push((cur, e));
            }

            return op.Tree(res, root);
        }
        static T[][] SizeToArray<T>(int[] sizeArray)
        {
            var a = new T[sizeArray.Length][];
            for (int i = 0; i < sizeArray.Length; i++)
                a[i] = new T[sizeArray[i]];
            return a;
        }
        static List<T>[] SizeToList<T>(int[] sizeArray)
        {
            var a = new List<T>[sizeArray.Length];
            for (int i = 0; i < sizeArray.Length; i++)
                a[i] = new List<T>(sizeArray[i]);
            return a;
        }
    }
}
