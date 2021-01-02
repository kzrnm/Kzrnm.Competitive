#pragma warning disable CA1819 // Properties should not return arrays
using Kzrnm.Competitive.IO;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder
{
    public class WIntGraphBuilder : WGraphBuilder<int, IntOperator>
    {
        public WIntGraphBuilder(int count, bool isOriented) : base(count, isOriented) { }
        public static WIntGraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isOriented)
        {
            var gb = new WIntGraphBuilder(count, isOriented);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Int);
            return gb;
        }
    }
    public class WLongGraphBuilder : WGraphBuilder<long, LongOperator>
    {
        public WLongGraphBuilder(int count, bool isOriented) : base(count, isOriented) { }
        public static WLongGraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isOriented)
        {
            var gb = new WLongGraphBuilder(count, isOriented);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Long);
            return gb;
        }
    }
    public class WGraphBuilder<T, TOp>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
    {
        protected static readonly TOp op = default;
        internal List<WEdge<T>>[] roots;
        internal List<WEdge<T>>[] children;
        public WGraphBuilder(int count, bool isOriented)
        {
            this.roots = new List<WEdge<T>>[count];
            this.children = new List<WEdge<T>>[count];
            for (var i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    this.roots[i] = new List<WEdge<T>>();
                    this.children[i] = new List<WEdge<T>>();
                }
                else
                {
                    this.roots[i] = this.children[i] = new List<WEdge<T>>();
                }
            }
        }
        public void Add(int from, int to, T value)
        {
            children[from].Add(new WEdge<T>(to, value));
            roots[to].Add(new WEdge<T>(from, value));
        }

        public WGraph<T, TOp, WNode<T, TOp>, WEdge<T>> ToGraph()
        {
            DebugUtil.Assert(roots.Length == children.Length);
            var res = new WNode<T, TOp>[roots.Length];
            for (int i = 0; i < res.Length; i++)
            {
                if (roots[i] == children[i])
                    res[i] = new WNode<T, TOp>(i, children[i].ToArray());
                else
                    res[i] = new WNode<T, TOp>(i, roots[i].ToArray(), children[i].ToArray());
            }
            return new WGraph<T, TOp, WNode<T, TOp>, WEdge<T>>(res);
        }

        public WTreeGraph<T, TOp, WTreeNode<T, TOp>, WEdge<T>> ToTree(int root = 0)
        {
            if (this.roots[0] != this.children[0])
                throw new Exception("木には無向グラフをしたほうが良い");
            var res = new WTreeNode<T, TOp>[this.children.Length];
            res[root] = new WTreeNode<T, TOp>(root, WEdge<T>.None, 0, default, this.children[root].ToArray());

            var queue = new Queue<(int parent, int child, T value)>();
            foreach (var e in res[root].Children)
            {
                queue.Enqueue((root, e.To, e.Value));
            }

            while (queue.Count > 0)
            {
                var (parent, cur, value) = queue.Dequeue();

                IList<WEdge<T>> childrenBuilder;
                if (parent == -1)
                    childrenBuilder = this.children[cur];
                else
                {
                    childrenBuilder = new List<WEdge<T>>(this.children[cur].Count);
                    foreach (var e in this.children[cur])
                        if (e.To != parent)
                            childrenBuilder.Add(e);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = new WTreeNode<T, TOp>(cur, new WEdge<T>(parent, value), res[parent].Depth + 1, op.Add(res[parent].DepthLength, value), childrenArr);
                foreach (var e in childrenArr)
                {
                    queue.Enqueue((cur, e.To, e.Value));
                }
            }

            return new WTreeGraph<T, TOp, WTreeNode<T, TOp>, WEdge<T>>(res);
        }

        public WGraphBuilder<T, TOp> Clone()
        {
            var count = this.roots.Length;
            var isOriented = this.roots[0] != this.children[0];
            var cl = new WGraphBuilder<T, TOp>(count, isOriented);
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

    public readonly struct WEdge<T> : IWEdge<T>, IReversable<WEdge<T>>, IEquatable<WEdge<T>>
    {
        public static WEdge<T> None { get; } = new WEdge<T>(-1, default);
        public WEdge(int to, T value)
        {
            To = to;
            Value = value;
        }
        public int To { get; }
        public T Value { get; }

        public override bool Equals(object obj) => obj is WEdge<T> edge && this.Equals(edge);
        public bool Equals(WEdge<T> other) => this.To == other.To &&
                   EqualityComparer<T>.Default.Equals(this.Value, other.Value);
        public override int GetHashCode() => HashCode.Combine(this.To, this.Value);
        public static bool operator ==(WEdge<T> left, WEdge<T> right) => left.Equals(right);
        public static bool operator !=(WEdge<T> left, WEdge<T> right) => !(left == right);
        public override string ToString() => (To, Value).ToString();
        public WEdge<T> Reversed(int from) => new WEdge<T>(from, Value);
    }

    public class WNode<T, TOp> : IWNode<T, WEdge<T>, TOp>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
    {
        public WNode(int i, WEdge<T>[] children)
        {
            this.Index = i;
            this.Roots = this.Children = children;
        }
        public WNode(int i, WEdge<T>[] roots, WEdge<T>[] children)
        {
            this.Index = i;
            this.Roots = roots;
            this.Children = children;
        }
        public int Index { get; }
        public WEdge<T>[] Roots { get; }
        public WEdge<T>[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is WNode<T, TOp> d && this.Equals(d);
        public bool Equals(WNode<T, TOp> other) => this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
    public class WTreeNode<T, TOp> : ITreeNode<WEdge<T>>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
    {
        public WTreeNode(int i, WEdge<T> root, int depth, T depthLength, WEdge<T>[] children)
        {
            this.Index = i;
            this.Root = root;
            this.Children = children;
            this.Depth = depth;
            this.DepthLength = depthLength;
        }
        public int Index { get; }
        public WEdge<T> Root { get; }
        public WEdge<T>[] Children { get; }
        public int Depth { get; }
        public T DepthLength { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is WTreeNode<T, TOp> node && this.Equals(node);
        public bool Equals(WTreeNode<T, TOp> other) => this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
