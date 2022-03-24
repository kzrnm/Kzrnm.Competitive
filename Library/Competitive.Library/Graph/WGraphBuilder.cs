using AtCoder.Internal;
using AtCoder.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraphBuilder<T, TOp>
        where TOp : struct, IAdditionOperator<T>
    {
        protected static readonly TOp op = default;
        internal readonly EdgeContainer<WEdge<T>> edgeContainer;
        public WGraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<WEdge<T>>(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to, T value) => edgeContainer.Add(from, new WEdge<T>(to, value));

        public WGraph<T, TOp, WGraphNode<T, WEdge<T>>, WEdge<T>> ToGraph()
        {
            var res = new WGraphNode<T, WEdge<T>>[edgeContainer.Length];
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = new WEdge<T>[res.Length][];
            var roots = edgeContainer.IsDirected ? new WEdge<T>[res.Length][] : children;
            for (int i = 0; i < res.Length; i++)
            {
                if (children[i] == null) children[i] = new WEdge<T>[edgeContainer.sizes[i]];
                if (roots[i] == null) roots[i] = new WEdge<T>[edgeContainer.rootSizes[i]];
                res[i] = new WGraphNode<T, WEdge<T>>(i, roots[i], children[i]);
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    var to = e.To;
                    if (roots[to] == null)
                        roots[to] = new WEdge<T>[edgeContainer.rootSizes[to]];
                    children[i][counter[i]++] = e;
                    roots[to][rootCounter[to]++] = e.Reversed(i);
                }
            }
            return new WGraph<T, TOp, WGraphNode<T, WEdge<T>>, WEdge<T>>(res, csr);
        }

        public WTreeGraph<T, TOp, WTreeNode<T, WEdge<T>>, WEdge<T>> ToTree(int root = 0)
        {
            Contract.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new WTreeNode<T, WEdge<T>>[edgeContainer.Length];
            var children = new SimpleList<WEdge<T>>[res.Length];
            foreach (var (from, e) in edgeContainer.edges)
            {
                var to = e.To;
                if (children[from] == null) children[from] = new SimpleList<WEdge<T>>();
                if (children[to] == null) children[to] = new SimpleList<WEdge<T>>();
                children[from].Add(e);
                children[to].Add(e.Reversed(from));
            }

            if (edgeContainer.Length == 1)
            {
                return new WTreeGraph<T, TOp, WTreeNode<T, WEdge<T>>, WEdge<T>>(
                    new WTreeNode<T, WEdge<T>>[1] {
                        new WTreeNode<T,WEdge<T>>(root, WEdge<T>.None, 0, default, Array.Empty<WEdge<T>>()) }, root);
            }
            res[root] = new WTreeNode<T, WEdge<T>>(root, WEdge<T>.None, 0, default,
                children[root]?.ToArray() ?? Array.Empty<WEdge<T>>());

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
                    childrenBuilder = children[cur];
                else
                {
                    childrenBuilder = new SimpleList<WEdge<T>>(children[cur].Count);
                    foreach (var e in children[cur])
                        if (e.To != parent)
                            childrenBuilder.Add(e);
                }

                var childrenArr = childrenBuilder.ToArray();
                res[cur] = new WTreeNode<T, WEdge<T>>(cur, new WEdge<T>(parent, value), res[parent].Depth + 1, op.Add(res[parent].DepthLength, value), childrenArr);
                foreach (var e in childrenArr)
                {
                    queue.Enqueue((cur, e.To, e.Value));
                }
            }

            return new WTreeGraph<T, TOp, WTreeNode<T, WEdge<T>>, WEdge<T>>(res, root);
        }
    }

    public readonly struct WEdge<T> : IWGraphEdge<T>, IReversable<WEdge<T>>, IEquatable<WEdge<T>>
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
        [凾(256)]
        public void Deconstruct(out int to, out T value)
        {
            to = To;
            value = Value;
        }
        [凾(256)]
        public WEdge<T> Reversed(int from) => new WEdge<T>(from, Value);
    }
}
