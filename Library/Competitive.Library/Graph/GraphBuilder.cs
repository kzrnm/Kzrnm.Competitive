using Kzrnm.Competitive.IO;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class GraphBuilder
    {
        internal readonly EdgeContainer<GraphEdge> edgeContainer;
        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<GraphEdge>(size, isDirected);
        }
        public static GraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new GraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0);
            return gb;
        }
        public static GraphBuilder<int> CreateWithEdgeIndex(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new GraphBuilder<int>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, i);
            return gb;
        }
        public static GraphBuilder CreateTree(int count, PropertyConsoleReader cr)
        {
            var gb = new GraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0, cr.Int0);
            return gb;
        }
        [凾(256)]
        public void Add(int from, int to) => edgeContainer.Add(from, new GraphEdge(to));


        public SimpleGraph<GraphNode, GraphEdge> ToGraph()
        {
            var res = new GraphNode[edgeContainer.Length];
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = new GraphEdge[res.Length][];
            var roots = edgeContainer.IsDirected ? new GraphEdge[res.Length][] : children;
            for (int i = 0; i < res.Length; i++)
            {
                if (children[i] == null) children[i] = new GraphEdge[edgeContainer.sizes[i]];
                if (roots[i] == null) roots[i] = new GraphEdge[edgeContainer.rootSizes[i]];
                res[i] = new GraphNode(i, roots[i], children[i]);
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    var to = e.To;
                    if (roots[to] == null)
                        roots[to] = new GraphEdge[edgeContainer.rootSizes[to]];
                    children[i][counter[i]++] = e;
                    roots[to][rootCounter[to]++] = e.Reversed(i);
                }
            }
            return new SimpleGraph<GraphNode, GraphEdge>(res, csr);
        }

        public TreeGraph<TreeNode, GraphEdge> ToTree(int root = 0)
        {
            Contract.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new TreeNode[edgeContainer.Length];
            var children = new List<GraphEdge>[res.Length];
            foreach (var (from, e) in edgeContainer.edges)
            {
                var to = e.To;
                if (children[from] == null) children[from] = new List<GraphEdge>();
                if (children[to] == null) children[to] = new List<GraphEdge>();
                children[from].Add(e);
                children[to].Add(e.Reversed(from));
            }

            if (edgeContainer.Length == 1)
            {
                return new TreeGraph<TreeNode, GraphEdge>(
                    new TreeNode[1] { new TreeNode(root, GraphEdge.None, 0, Array.Empty<GraphEdge>()) }, root);
            }

            res[root] = new TreeNode(root, GraphEdge.None, 0,
                children[root]?.ToArray() ?? Array.Empty<GraphEdge>());

            var queue = new Queue<(int parent, int child)>();
            foreach (var child in res[root].Children)
            {
                queue.Enqueue((root, child));
            }

            while (queue.Count > 0)
            {
                var (parent, cur) = queue.Dequeue();

                IList<GraphEdge> childrenBuilder;
                if (parent == -1)
                    childrenBuilder = children[cur];
                else
                {
                    childrenBuilder = new List<GraphEdge>(children[cur].Count);
                    foreach (var c in children[cur])
                        if (c != parent)
                            childrenBuilder.Add(c);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = new TreeNode(cur, new GraphEdge(parent), res[parent].Depth + 1, childrenArr);
                foreach (var child in childrenArr)
                {
                    queue.Enqueue((cur, child));
                }
            }

            return new TreeGraph<TreeNode, GraphEdge>(res, root);
        }
    }

    public readonly struct GraphEdge : IGraphEdge, IReversable<GraphEdge>, IEquatable<GraphEdge>
    {
        public static GraphEdge None { get; } = new GraphEdge(-1);
        public GraphEdge(int to)
        {
            To = to;
        }
        public int To { get; }
        [凾(256)]
        public static implicit operator int(GraphEdge e) => e.To;
        public override string ToString() => To.ToString();
        [凾(256)]
        public GraphEdge Reversed(int from) => new GraphEdge(from);

        public override int GetHashCode() => this.To;
        public override bool Equals(object obj) => obj is GraphEdge edge && this.Equals(edge);
        public bool Equals(GraphEdge other) => this.To == other.To;
        public static bool operator ==(GraphEdge left, GraphEdge right) => left.Equals(right);
        public static bool operator !=(GraphEdge left, GraphEdge right) => !left.Equals(right);
    }

    public class GraphNode : IGraphNode<GraphEdge>, IEquatable<GraphNode>
    {
        public GraphNode(int i, GraphEdge[] roots, GraphEdge[] children)
        {
            this.Index = i;
            this.Roots = roots;
            this.Children = children;
        }
        public int Index { get; }
        public GraphEdge[] Roots { get; }
        public GraphEdge[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is GraphNode d && this.Equals(d);
        public bool Equals(GraphNode other) => this.Index == other?.Index;
        public override int GetHashCode() => this.Index;
    }
    public class TreeNode : ITreeNode<GraphEdge>, IEquatable<TreeNode>
    {
        public TreeNode(int i, GraphEdge root, int depth, GraphEdge[] children)
        {
            this.Index = i;
            this.Root = root;
            this.Children = children;
            this.Depth = depth;
        }
        public int Index { get; }
        public GraphEdge Root { get; }
        public GraphEdge[] Children { get; }
        public int Depth { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is TreeNode node && this.Equals(node);
        public bool Equals(TreeNode other) => other != null && this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
