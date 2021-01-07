#pragma warning disable CA1819 // Properties should not return arrays
using Kzrnm.Competitive.IO;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder
{
    public class GraphBuilder
    {
        internal readonly EdgeContainer<Edge> edgeContainer;
        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<Edge>(size, isDirected);
        }
        public static GraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isOriented)
        {
            var gb = new GraphBuilder(count, isOriented);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0);
            return gb;
        }
        public void Add(int from, int to) => edgeContainer.Add(from, new Edge(to));


        public SimpleGraph<Node, Edge> ToGraph()
        {
            var res = new Node[edgeContainer.Length];
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = new Edge[res.Length][];
            var roots = edgeContainer.IsDirected ? new Edge[res.Length][] : children;
            for (int i = 0; i < res.Length; i++)
            {
                if (children[i] == null) children[i] = new Edge[edgeContainer.sizes[i]];
                if (roots[i] == null) roots[i] = new Edge[edgeContainer.rootSizes[i]];
                res[i] = new Node(i, roots[i], children[i]);
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    if (roots[e.To] == null)
                        roots[e.To] = new Edge[edgeContainer.rootSizes[e.To]];
                    children[i][counter[i]++] = e;
                    roots[e.To][rootCounter[e.To]++] = e.Reversed(i);
                }
            }
            return new SimpleGraph<Node, Edge>(res, csr);
        }

        public TreeGraph<TreeNode, Edge> ToTree(int root = 0)
        {
            DebugUtil.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new TreeNode[edgeContainer.Length];
            var children = new List<Edge>[res.Length];
            foreach (var (from, e) in edgeContainer.edges)
            {
                if (children[from] == null) children[from] = new List<Edge>();
                if (children[e.To] == null) children[e.To] = new List<Edge>();
                children[from].Add(e);
                children[e.To].Add(e.Reversed(from));
            }

            res[root] = new TreeNode(root, Edge.None, 0, children[root].ToArray());

            var queue = new Queue<(int parent, int child)>();
            foreach (var child in res[root].Children)
            {
                queue.Enqueue((root, child));
            }

            while (queue.Count > 0)
            {
                var (parent, cur) = queue.Dequeue();

                IList<Edge> childrenBuilder;
                if (parent == -1)
                    childrenBuilder = children[cur];
                else
                {
                    childrenBuilder = new List<Edge>(children[cur].Count);
                    foreach (var c in children[cur])
                        if (c != parent)
                            childrenBuilder.Add(c);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = new TreeNode(cur, new Edge(parent), res[parent].Depth + 1, childrenArr);
                foreach (var child in childrenArr)
                {
                    queue.Enqueue((cur, child));
                }
            }

            return new TreeGraph<TreeNode, Edge>(res);
        }
    }

    public readonly struct Edge : IEdge, IReversable<Edge>, IEquatable<Edge>
    {
        public static Edge None { get; } = new Edge(-1);
        public Edge(int to)
        {
            To = to;
        }
        public int To { get; }
        public static implicit operator int(Edge e) => e.To;
        public override string ToString() => To.ToString();
        public Edge Reversed(int from) => new Edge(from);

        public override int GetHashCode() => this.To;
        public override bool Equals(object obj) => obj is Edge edge && this.Equals(edge);
        public bool Equals(Edge other) => this.To == other.To;
        public static bool operator ==(Edge left, Edge right) => left.Equals(right);
        public static bool operator !=(Edge left, Edge right) => !left.Equals(right);
    }

    public class Node : INode<Edge>, IEquatable<Node>
    {
        public Node(int i, Edge[] roots, Edge[] children)
        {
            this.Index = i;
            this.Roots = roots;
            this.Children = children;
        }
        public int Index { get; }
        public Edge[] Roots { get; }
        public Edge[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is Node d && this.Equals(d);
        public bool Equals(Node other) => this.Index == other?.Index;
        public override int GetHashCode() => this.Index;
    }
    public class TreeNode : ITreeNode<Edge>, IEquatable<TreeNode>
    {
        public TreeNode(int i, Edge root, int depth, Edge[] children)
        {
            this.Index = i;
            this.Root = root;
            this.Children = children;
            this.Depth = depth;
        }
        public int Index { get; }
        public Edge Root { get; }
        public Edge[] Children { get; }
        public int Depth { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is TreeNode node && this.Equals(node);
        public bool Equals(TreeNode other) => other != null && this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
