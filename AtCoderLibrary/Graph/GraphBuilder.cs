#pragma warning disable CA1819 // Properties should not return arrays
using Kzrnm.Competitive.IO;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.Graph
{
    public class GraphBuilder
    {
        internal List<Edge>[] roots;
        internal List<Edge>[] children;
        public GraphBuilder(int count, bool isOriented)
        {
            this.roots = new List<Edge>[count];
            this.children = new List<Edge>[count];
            for (var i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    this.roots[i] = new List<Edge>();
                    this.children[i] = new List<Edge>();
                }
                else
                {
                    this.roots[i] = this.children[i] = new List<Edge>();
                }
            }
        }
        public static GraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isOriented)
        {
            var gb = new GraphBuilder(count, isOriented);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0);
            return gb;
        }

        public void Add(int from, int to)
        {
            children[from].Add(new Edge(to));
            roots[to].Add(new Edge(from));
        }

        public Graph<Node, Edge> ToGraph()
        {
            DebugUtil.Assert(roots.Length == children.Length);
            var res = new Node[roots.Length];
            for (int i = 0; i < res.Length; i++)
            {
                if (roots[i] == children[i])
                    res[i] = new Node(i, children[i].ToArray());
                else
                    res[i] = new Node(i, roots[i].ToArray(), children[i].ToArray());
            }
            return new Graph<Node, Edge>(res);
        }

        public TreeGraph<TreeNode, Edge> ToTree(int root = 0)
        {
            if (this.roots[0] != this.children[0])
                throw new Exception("木には無向グラフをしたほうが良い");
            var res = new TreeNode[this.children.Length];
            res[root] = new TreeNode(root, Edge.None, 0, this.children[root].ToArray());

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
                    childrenBuilder = this.children[cur];
                else
                {
                    childrenBuilder = new List<Edge>(this.children[cur].Count);
                    foreach (var c in this.children[cur])
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

        public GraphBuilder Clone()
        {
            var count = this.roots.Length;
            var isOriented = this.roots[0] != this.children[0];
            var cl = new GraphBuilder(count, isOriented);
            for (int i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    cl.children[i] = this.children[i].ToList();
                    cl.roots[i] = this.roots[i].ToList();
                }
                else
                    cl.children[i] = cl.roots[i] = this.roots[i].ToList();
            }
            return cl;
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

    public class Node : INode<Edge>
    {
        public Node(int i, Edge[] children)
        {
            this.Index = i;
            this.Roots = this.Children = children;
        }
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
        public bool Equals(Node other) => this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
    public class TreeNode : ITreeNode<Edge>
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
        public bool Equals(TreeNode other) => this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
