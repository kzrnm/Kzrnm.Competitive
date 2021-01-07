#pragma warning disable CA1819 // Properties should not return arrays
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder
{
    public class WGraphBuilder<T, S, TOp>
        where TOp : struct, IAdditionOperator<T>
    {
        protected static readonly TOp op = default;
        internal readonly EdgeContainer<WEdge<T, S>> edgeContainer;
        public WGraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<WEdge<T, S>>(size, isDirected);
        }
        public void Add(int from, int to, T value, S data) => edgeContainer.Add(from, new WEdge<T, S>(to, value, data));

        public WGraph<T, TOp, WNode<T, S, TOp>, WEdge<T, S>> ToGraph()
        {
            var res = new WNode<T, S, TOp>[edgeContainer.Length];
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = new WEdge<T, S>[res.Length][];
            var roots = edgeContainer.IsDirected ? new WEdge<T, S>[res.Length][] : children;
            for (int i = 0; i < res.Length; i++)
            {
                if (children[i] == null) children[i] = new WEdge<T, S>[edgeContainer.sizes[i]];
                if (roots[i] == null) roots[i] = new WEdge<T, S>[edgeContainer.rootSizes[i]];
                res[i] = new WNode<T, S, TOp>(i, roots[i], children[i]);
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    if (roots[e.To] == null)
                        roots[e.To] = new WEdge<T, S>[edgeContainer.rootSizes[e.To]];
                    children[i][counter[i]++] = e;
                    roots[e.To][rootCounter[e.To]++] = e.Reversed(i);
                }
            }
            return new WGraph<T, TOp, WNode<T, S, TOp>, WEdge<T, S>>(res, csr);
        }

        public WTreeGraph<T, TOp, WTreeNode<T, S, TOp>, WEdge<T, S>> ToTree(int root = 0)
        {
            DebugUtil.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new WTreeNode<T, S, TOp>[edgeContainer.Length];
            var children = new List<WEdge<T, S>>[res.Length];
            foreach (var (from, e) in edgeContainer.edges)
            {
                if (children[from] == null) children[from] = new List<WEdge<T, S>>();
                if (children[e.To] == null) children[e.To] = new List<WEdge<T, S>>();
                children[from].Add(e);
                children[e.To].Add(e.Reversed(from));
            }

            res[root] = new WTreeNode<T, S, TOp>(root, WEdge<T, S>.None, 0, default, children[root].ToArray());

            var queue = new Queue<(int parent, int child, T value, S data)>();
            foreach (var e in res[root].Children)
            {
                queue.Enqueue((root, e.To, e.Value, e.Data));
            }

            while (queue.Count > 0)
            {
                var (parent, cur, value, data) = queue.Dequeue();

                IList<WEdge<T, S>> childrenBuilder;
                if (parent == -1)
                    childrenBuilder = children[cur];
                else
                {
                    childrenBuilder = new List<WEdge<T, S>>(children[cur].Count);
                    foreach (var e in children[cur])
                        if (e.To != parent)
                            childrenBuilder.Add(e);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = new WTreeNode<T, S, TOp>(cur, new WEdge<T, S>(parent, value, data), res[parent].Depth + 1, op.Add(res[parent].DepthLength, value), childrenArr);
                foreach (var e in childrenArr)
                {
                    queue.Enqueue((cur, e.To, e.Value, e.Data));
                }
            }

            return new WTreeGraph<T, TOp, WTreeNode<T, S, TOp>, WEdge<T, S>>(res);
        }
    }

    public readonly struct WEdge<T, S> : IWEdge<T>, IReversable<WEdge<T, S>>, IEquatable<WEdge<T, S>>
    {
        public static WEdge<T, S> None { get; } = new WEdge<T, S>(-1, default, default);
        public WEdge(int to, T value, S data)
        {
            To = to;
            Value = value;
            Data = data;
        }
        public int To { get; }
        public T Value { get; }
        public S Data { get; }

        public override bool Equals(object obj) => obj is WEdge<T, S> edge && this.Equals(edge);
        public bool Equals(WEdge<T, S> other) => this.To == other.To &&
            EqualityComparer<T>.Default.Equals(this.Value, other.Value) &&
            EqualityComparer<S>.Default.Equals(this.Data, other.Data);
        public override int GetHashCode() => HashCode.Combine(this.To, this.Value);
        public static bool operator ==(WEdge<T, S> left, WEdge<T, S> right) => left.Equals(right);
        public static bool operator !=(WEdge<T, S> left, WEdge<T, S> right) => !(left == right);
        public override string ToString() => (To, Value).ToString();
        public WEdge<T, S> Reversed(int from) => new WEdge<T, S>(from, Value, Data);
    }

    public class WNode<T, S, TOp> : IWNode<T, WEdge<T, S>, TOp>, IEquatable<WNode<T, S, TOp>>
        where TOp : struct, IAdditionOperator<T>
    {
        public WNode(int i, WEdge<T, S>[] roots, WEdge<T, S>[] children)
        {
            this.Index = i;
            this.Roots = roots;
            this.Children = children;
        }
        public int Index { get; }
        public WEdge<T, S>[] Roots { get; }
        public WEdge<T, S>[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is WNode<T, TOp> d && this.Equals(d);
        public bool Equals(WNode<T, S, TOp> other) => this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
    public class WTreeNode<T, S, TOp> : ITreeNode<WEdge<T, S>>, IEquatable<WTreeNode<T, S, TOp>>
        where TOp : struct, IAdditionOperator<T>
    {
        public WTreeNode(int i, WEdge<T, S> root, int depth, T depthLength, WEdge<T, S>[] children)
        {
            this.Index = i;
            this.Root = root;
            this.Children = children;
            this.Depth = depth;
            this.DepthLength = depthLength;
        }
        public int Index { get; }
        public WEdge<T, S> Root { get; }
        public WEdge<T, S>[] Children { get; }
        public int Depth { get; }
        public T DepthLength { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is WTreeNode<T, TOp> node && this.Equals(node);
        public bool Equals(WTreeNode<T, S, TOp> other) => other != null && this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
