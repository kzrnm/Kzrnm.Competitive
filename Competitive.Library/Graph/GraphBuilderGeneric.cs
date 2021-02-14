#pragma warning disable CA1819 // Properties should not return arrays
using Kzrnm.Competitive.IO;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive
{
    public class GraphBuilder<T>
    {
        internal readonly EdgeContainer<Edge<T>> edgeContainer;
        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<Edge<T>>(size, isDirected);
        }
        public void Add(int from, int to, T data) => edgeContainer.Add(from, new Edge<T>(to, data));


        public SimpleGraph<GraphNode<T>, Edge<T>> ToGraph()
        {
            var res = new GraphNode<T>[edgeContainer.Length];
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = new Edge<T>[res.Length][];
            var roots = edgeContainer.IsDirected ? new Edge<T>[res.Length][] : children;
            for (int i = 0; i < res.Length; i++)
            {
                if (children[i] == null) children[i] = new Edge<T>[edgeContainer.sizes[i]];
                if (roots[i] == null) roots[i] = new Edge<T>[edgeContainer.rootSizes[i]];
                res[i] = new GraphNode<T>(i, roots[i], children[i]);
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    if (roots[e.To] == null)
                        roots[e.To] = new Edge<T>[edgeContainer.rootSizes[e.To]];
                    children[i][counter[i]++] = e;
                    roots[e.To][rootCounter[e.To]++] = e.Reversed(i);
                }
            }
            return new SimpleGraph<GraphNode<T>, Edge<T>>(res, csr);
        }

        public TreeGraph<TreeNode<T>, Edge<T>> ToTree(int root = 0)
        {
            Contract.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new TreeNode<T>[edgeContainer.Length];
            var children = new SimpleList<Edge<T>>[res.Length];
            foreach (var (from, e) in edgeContainer.edges)
            {
                if (children[from] == null) children[from] = new SimpleList<Edge<T>>();
                if (children[e.To] == null) children[e.To] = new SimpleList<Edge<T>>();
                children[from].Add(e);
                children[e.To].Add(e.Reversed(from));
            }

            if (edgeContainer.Length == 1)
            {
                return new TreeGraph<TreeNode<T>, Edge<T>>(
                    new TreeNode<T>[1] {
                        new TreeNode<T>(root, Edge<T>.None, 0,  Array.Empty<Edge<T>>()) }, root);
            }

            res[root] = new TreeNode<T>(root, Edge<T>.None, 0, children[root].ToArray());

            var queue = new Queue<(int parent, int child, T data)>();
            foreach (var e in res[root].Children)
            {
                queue.Enqueue((root, e.To, e.Data));
            }

            while (queue.Count > 0)
            {
                var (parent, cur, value) = queue.Dequeue();

                IList<Edge<T>> childrenBuilder;
                if (parent == -1)
                    childrenBuilder = children[cur];
                else
                {
                    childrenBuilder = new SimpleList<Edge<T>>(children[cur].Count);
                    foreach (var e in children[cur])
                        if (e.To != parent)
                            childrenBuilder.Add(e);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = new TreeNode<T>(cur, new Edge<T>(parent, value), res[parent].Depth + 1, childrenArr);
                foreach (var e in childrenArr)
                {
                    queue.Enqueue((cur, e.To, e.Data));
                }
            }

            return new TreeGraph<TreeNode<T>, Edge<T>>(res, root);
        }
    }

    public readonly struct Edge<T> : IEdge, IReversable<Edge<T>>, IEquatable<Edge<T>>
    {
        public static Edge<T> None { get; } = new Edge<T>(-1, default);
        public Edge(int to, T data)
        {
            To = to;
            Data = data;
        }
        public T Data { get; }
        public int To { get; }
        public override string ToString() => To.ToString();
        public Edge<T> Reversed(int from) => new Edge<T>(from, Data);

        public override int GetHashCode() => this.To;
        public override bool Equals(object obj) => obj is Edge<T> edge && this.Equals(edge);
        public bool Equals(Edge<T> other) => this.To == other.To;
        public static bool operator ==(Edge<T> left, Edge<T> right) => left.Equals(right);
        public static bool operator !=(Edge<T> left, Edge<T> right) => !left.Equals(right);
    }

    public class GraphNode<T> : IGraphNode<Edge<T>>, IEquatable<GraphNode<T>>
    {
        public GraphNode(int i, Edge<T>[] roots, Edge<T>[] children)
        {
            this.Index = i;
            this.Roots = roots;
            this.Children = children;
        }
        public int Index { get; }
        public Edge<T>[] Roots { get; }
        public Edge<T>[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is GraphNode<T> d && this.Equals(d);
        public bool Equals(GraphNode<T> other) => this.Index == other?.Index;
        public override int GetHashCode() => this.Index;
    }
    public class TreeNode<T> : ITreeNode<Edge<T>>, IEquatable<TreeNode<T>>
    {
        public TreeNode(int i, Edge<T> root, int depth, Edge<T>[] children)
        {
            this.Index = i;
            this.Root = root;
            this.Children = children;
            this.Depth = depth;
        }
        public int Index { get; }
        public Edge<T> Root { get; }
        public Edge<T>[] Children { get; }
        public int Depth { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is TreeNode<T> node && this.Equals(node);
        public bool Equals(TreeNode<T> other) => other != null && this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
