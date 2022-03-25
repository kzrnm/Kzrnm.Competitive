using AtCoder.Internal;
using AtCoder.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
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
        [凾(256)]
        public void Add(int from, int to, T value, S data) => edgeContainer.Add(from, new WEdge<T, S>(to, value, data));

        public WGraph<T, TOp, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>> ToGraph()
        {
            var res = new WGraphNode<T, WEdge<T, S>>[edgeContainer.Length];
            var csr = edgeContainer.ToCSR();
            var counter = new int[res.Length];
            var rootCounter = edgeContainer.IsDirected ? new int[res.Length] : counter;
            var children = new WEdge<T, S>[res.Length][];
            var roots = edgeContainer.IsDirected ? new WEdge<T, S>[res.Length][] : children;
            for (int i = 0; i < res.Length; i++)
            {
                if (children[i] == null) children[i] = new WEdge<T, S>[edgeContainer.sizes[i]];
                if (roots[i] == null) roots[i] = new WEdge<T, S>[edgeContainer.rootSizes[i]];
                res[i] = new WGraphNode<T, WEdge<T, S>>(i, roots[i], children[i]);
                foreach (ref var e in csr.EList.AsSpan(csr.Start[i], csr.Start[i + 1] - csr.Start[i]))
                {
                    var to = e.To;
                    if (roots[to] == null)
                        roots[to] = new WEdge<T, S>[edgeContainer.rootSizes[to]];
                    children[i][counter[i]++] = e;
                    roots[to][rootCounter[to]++] = e.Reversed(i);
                }
            }
            return new WGraph<T, TOp, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>>(res, csr);
        }

        public WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>> ToTree(int root = 0)
        {
            Contract.Assert(!edgeContainer.IsDirected, "木には無向グラフをしたほうが良い");
            var res = new WTreeNode<T, WEdge<T, S>>[edgeContainer.Length];
            var children = new List<WEdge<T, S>>[res.Length];
            foreach (var (from, e) in edgeContainer.edges)
            {
                var to = e.To;
                if (children[from] == null) children[from] = new List<WEdge<T, S>>();
                if (children[to] == null) children[to] = new List<WEdge<T, S>>();
                children[from].Add(e);
                children[to].Add(e.Reversed(from));
            }

            if (edgeContainer.Length == 1)
            {
                return new WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>(
                    new WTreeNode<T, WEdge<T, S>>[1] {
                        new WTreeNode<T, WEdge<T, S>>(root, WEdge<T, S>.None, 0, default, Array.Empty<WEdge<T, S>>()) }, root);
            }

            res[root] = new WTreeNode<T, WEdge<T, S>>(root, WEdge<T, S>.None, 0, default,
                children[root]?.ToArray() ?? Array.Empty<WEdge<T, S>>());

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
                res[cur] = new WTreeNode<T, WEdge<T, S>>(cur, new WEdge<T, S>(parent, value, data), res[parent].Depth + 1, op.Add(res[parent].DepthLength, value), childrenArr);
                foreach (var e in childrenArr)
                {
                    queue.Enqueue((cur, e.To, e.Value, e.Data));
                }
            }

            return new WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>(res, root);
        }
    }

    public readonly struct WEdge<T, S> : IWGraphEdge<T>, IGraphData<S>, IReversable<WEdge<T, S>>, IEquatable<WEdge<T, S>>
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
        public override string ToString() => $"to:{To}, Value:{Value}, Data:{Data}";
        [凾(256)]
        public void Deconstruct(out int to, out T value, out S data)
        {
            to = To;
            value = Value;
            data = Data;
        }
        [凾(256)]
        public WEdge<T, S> Reversed(int from) => new WEdge<T, S>(from, Value, Data);
    }
}
