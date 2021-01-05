#pragma warning disable CA1819 // Properties should not return arrays
using Kzrnm.Competitive.IO;
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
        internal List<WEdge<T, S>>[] roots;
        internal List<WEdge<T, S>>[] children;
        public WGraphBuilder(int count, bool isOriented)
        {
            this.roots = new List<WEdge<T, S>>[count];
            this.children = new List<WEdge<T, S>>[count];
            for (var i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    this.roots[i] = new List<WEdge<T, S>>();
                    this.children[i] = new List<WEdge<T, S>>();
                }
                else
                {
                    this.roots[i] = this.children[i] = new List<WEdge<T, S>>();
                }
            }
        }
        public void Add(int from, int to, T value, S data)
        {
            children[from].Add(new WEdge<T, S>(to, value, data));
            roots[to].Add(new WEdge<T, S>(from, value, data));
        }

        public WGraph<T, TOp, WNode<T, S, TOp>, WEdge<T, S>> ToGraph()
        {
            DebugUtil.Assert(roots.Length == children.Length);
            var res = new WNode<T, S, TOp>[roots.Length];
            for (int i = 0; i < res.Length; i++)
            {
                if (roots[i] == children[i])
                    res[i] = new WNode<T, S, TOp>(i, children[i].ToArray());
                else
                    res[i] = new WNode<T, S, TOp>(i, roots[i].ToArray(), children[i].ToArray());
            }
            return new WGraph<T, TOp, WNode<T, S, TOp>, WEdge<T, S>>(res);
        }

        public WTreeGraph<T, TOp, WTreeNode<T, S, TOp>, WEdge<T, S>> ToTree(int root = 0)
        {
            if (this.roots[0] != this.children[0])
                throw new Exception("木には無向グラフをしたほうが良い");
            var res = new WTreeNode<T, S, TOp>[this.children.Length];
            res[root] = new WTreeNode<T, S, TOp>(root, WEdge<T, S>.None, 0, default, this.children[root].ToArray());

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
                    childrenBuilder = this.children[cur];
                else
                {
                    childrenBuilder = new List<WEdge<T, S>>(this.children[cur].Count);
                    foreach (var e in this.children[cur])
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

        public WGraphBuilder<T, S, TOp> Clone()
        {
            var count = this.roots.Length;
            var isOriented = this.roots[0] != this.children[0];
            var cl = new WGraphBuilder<T, S, TOp>(count, isOriented);
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
        public static bool operator ==(WEdge<T> left, WEdge<T, S> right) => left.Equals(right);
        public static bool operator !=(WEdge<T> left, WEdge<T, S> right) => !(left == right);
        public override string ToString() => (To, Value).ToString();
        public WEdge<T, S> Reversed(int from) => new WEdge<T, S>(from, Value, Data);
    }

    public class WNode<T, S, TOp> : IWNode<T, WEdge<T, S>, TOp>, IEquatable<WNode<T, S, TOp>>
        where TOp : struct, IAdditionOperator<T>
    {
        public WNode(int i, WEdge<T, S>[] children)
        {
            this.Index = i;
            this.Roots = this.Children = children;
        }
        public WNode(int i, WEdge<T, S>[] roots, WEdge<T, S>[] children)
        {
            this.Index = i;
            if (roots == children)
                children = (WEdge<T, S>[])children.Clone();
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
        public bool Equals(WTreeNode<T, S, TOp> other) => this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
