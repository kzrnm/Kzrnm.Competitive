using AtCoder.Internal;
using AtCoder.Operators;
using System;
using System.Collections.Generic;
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
            => GraphBuilderLogic.ToGraph<WGraph<T, TOp, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>>, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>, TBOp>(edgeContainer);

        public WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>, TBOp>(edgeContainer, root);
        struct TBOp :
            IGraphBuildOperator<WGraph<T, TOp, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>>, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>>,
            ITreeBuildOperator<WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>
        {
            [凾(256)] public WGraph<T, TOp, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>> Graph(WGraphNode<T, WEdge<T, S>>[] nodes, CSR<WEdge<T, S>> edges) => new WGraph<T, TOp, WGraphNode<T, WEdge<T, S>>, WEdge<T, S>>(nodes, edges);
            [凾(256)] public WGraphNode<T, WEdge<T, S>> Node(int i, WEdge<T, S>[] roots, WEdge<T, S>[] children) => new WGraphNode<T, WEdge<T, S>>(i, roots, children);

            [凾(256)] public WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>> Tree(WTreeNode<T, WEdge<T, S>>[] nodes, int root) => new WTreeGraph<T, TOp, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>(nodes, root);
            [凾(256)]
            public WTreeNode<T, WEdge<T, S>> TreeNode(int i, int size, WTreeNode<T, WEdge<T, S>> parent, WEdge<T, S> edge, WEdge<T, S>[] children)
                => new WTreeNode<T, WEdge<T, S>>(i, size, edge.Reversed(parent.Index), parent.Depth + 1, op.Add(parent.DepthLength, edge.Value), children);
            [凾(256)] public WTreeNode<T, WEdge<T, S>> TreeRootNode(int i, int size, WEdge<T, S>[] children) => new WTreeNode<T, WEdge<T, S>>(i, size, WEdge<T, S>.None, 0, default, children);
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

        public override bool Equals(object obj) => obj is WEdge<T, S> edge && Equals(edge);
        public bool Equals(WEdge<T, S> other) => To == other.To &&
            EqualityComparer<T>.Default.Equals(Value, other.Value) &&
            EqualityComparer<S>.Default.Equals(Data, other.Data);
        public override int GetHashCode() => HashCode.Combine(To, Value);
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
