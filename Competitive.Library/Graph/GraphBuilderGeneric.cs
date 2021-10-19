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
        internal readonly EdgeContainer<GraphEdge<T>> edgeContainer;
        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<GraphEdge<T>>(size, isDirected);
        }
        public void Add(int from, int to, T data) => edgeContainer.Add(from, new GraphEdge<T>(to, data));


        public SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>> ToGraph()
        {
            var res = new GraphNode<GraphEdge<T>>[edgeContainer.Length];
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = new GraphEdge<T>[res.Length][];
            var roots = edgeContainer.IsDirected ? new GraphEdge<T>[res.Length][] : children;
            for (int i = 0; i < res.Length; i++)
            {
                if (children[i] == null) children[i] = new GraphEdge<T>[edgeContainer.sizes[i]];
                if (roots[i] == null) roots[i] = new GraphEdge<T>[edgeContainer.rootSizes[i]];
                res[i] = new GraphNode<GraphEdge<T>>(i, roots[i], children[i]);
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    var to = e.To;
                    if (roots[to] == null)
                        roots[to] = new GraphEdge<T>[edgeContainer.rootSizes[to]];
                    children[i][counter[i]++] = e;
                    roots[to][rootCounter[to]++] = e.Reversed(i);
                }
            }
            return new SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>>(res, csr);
        }

        public TreeGraph<TreeNode<T>, GraphEdge<T>> ToTree(int root = 0)
        {
            Contract.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new TreeNode<T>[edgeContainer.Length];
            var children = new SimpleList<GraphEdge<T>>[res.Length];
            foreach (var (from, e) in edgeContainer.edges)
            {
                var to = e.To;
                if (children[from] == null) children[from] = new SimpleList<GraphEdge<T>>();
                if (children[to] == null) children[to] = new SimpleList<GraphEdge<T>>();
                children[from].Add(e);
                children[to].Add(e.Reversed(from));
            }

            if (edgeContainer.Length == 1)
            {
                return new TreeGraph<TreeNode<T>, GraphEdge<T>>(
                    new TreeNode<T>[1] {
                        new TreeNode<T>(root, GraphEdge<T>.None, 0,  Array.Empty<GraphEdge<T>>()) }, root);
            }

            res[root] = new TreeNode<T>(root, GraphEdge<T>.None, 0,
                children[root]?.ToArray() ?? Array.Empty<GraphEdge<T>>());

            var queue = new Queue<(int parent, int child, T data)>();
            foreach (var e in res[root].Children)
            {
                queue.Enqueue((root, e.To, e.Data));
            }

            while (queue.Count > 0)
            {
                var (parent, cur, value) = queue.Dequeue();

                IList<GraphEdge<T>> childrenBuilder;
                if (parent == -1)
                    childrenBuilder = children[cur];
                else
                {
                    childrenBuilder = new SimpleList<GraphEdge<T>>(children[cur].Count);
                    foreach (var e in children[cur])
                        if (e.To != parent)
                            childrenBuilder.Add(e);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = new TreeNode<T>(cur, new GraphEdge<T>(parent, value), res[parent].Depth + 1, childrenArr);
                foreach (var e in childrenArr)
                {
                    queue.Enqueue((cur, e.To, e.Data));
                }
            }

            return new TreeGraph<TreeNode<T>, GraphEdge<T>>(res, root);
        }
    }

    public readonly struct GraphEdge<T> : IGraphEdge, IReversable<GraphEdge<T>>, IEquatable<GraphEdge<T>>
    {
        public static GraphEdge<T> None { get; } = new GraphEdge<T>(-1, default);
        public GraphEdge(int to, T data)
        {
            To = to;
            Data = data;
        }
        public T Data { get; }
        public int To { get; }
        public override string ToString() => $"to:{To}, Data:{Data}";
        public void Deconstruct(out int to, out T data)
        {
            to = To;
            data = Data;
        }
        public GraphEdge<T> Reversed(int from) => new GraphEdge<T>(from, Data);

        public override int GetHashCode() => this.To;
        public override bool Equals(object obj) => obj is GraphEdge<T> edge && this.Equals(edge);
        public bool Equals(GraphEdge<T> other) => this.To == other.To;
        public static bool operator ==(GraphEdge<T> left, GraphEdge<T> right) => left.Equals(right);
        public static bool operator !=(GraphEdge<T> left, GraphEdge<T> right) => !left.Equals(right);
    }
}
