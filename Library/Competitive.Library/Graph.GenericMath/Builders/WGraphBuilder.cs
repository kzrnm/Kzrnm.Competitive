using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraphBuilder<T>
        where T : IAdditionOperators<T, T, T>
    {
        internal readonly EdgeContainer<WEdge<T>> edgeContainer;
        public WGraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<WEdge<T>>(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to, T value) => edgeContainer.Add(from, new WEdge<T>(to, value));

        public WGraph<T, WGraphNode<T, WEdge<T>>, WEdge<T>> ToGraph()
            => GraphBuilderLogic.ToGraph<WGraph<T, WGraphNode<T, WEdge<T>>, WEdge<T>>, WGraphNode<T, WEdge<T>>, WEdge<T>, TBOp>(edgeContainer);

        public WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>>, WTreeNode<T, WEdge<T>>, WEdge<T>, TBOp>(edgeContainer, root);
        struct TBOp :
            IGraphBuildOperator<WGraph<T, WGraphNode<T, WEdge<T>>, WEdge<T>>, WGraphNode<T, WEdge<T>>, WEdge<T>>,
            ITreeBuildOperator<WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>>, WTreeNode<T, WEdge<T>>, WEdge<T>>
        {
            [凾(256)] public WGraph<T, WGraphNode<T, WEdge<T>>, WEdge<T>> Graph(WGraphNode<T, WEdge<T>>[] nodes, CSR<WEdge<T>> edges) => new WGraph<T, WGraphNode<T, WEdge<T>>, WEdge<T>>(nodes, edges);
            [凾(256)] public WGraphNode<T, WEdge<T>> Node(int i, WEdge<T>[] parents, WEdge<T>[] children) => new WGraphNode<T, WEdge<T>>(i, parents, children);
            [凾(256)] public WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>> Tree(WTreeNode<T, WEdge<T>>[] nodes, int root, HeavyLightDecomposition<WTreeNode<T, WEdge<T>>, WEdge<T>> hl) => new WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>>(nodes, root, hl);
            [凾(256)]
            public WTreeNode<T, WEdge<T>> TreeNode(int i, int size, WTreeNode<T, WEdge<T>> parent, WEdge<T> edge, WEdge<T>[] children)
                => new WTreeNode<T, WEdge<T>>(i, size, edge.Reversed(parent.Index), parent.Depth + 1, parent.DepthLength + edge.Value, children);
            [凾(256)] public WTreeNode<T, WEdge<T>> TreeRootNode(int i, int size, WEdge<T>[] children) => new WTreeNode<T, WEdge<T>>(i, size, WEdge<T>.None, 0, default, children);
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

        [凾(256)] public static implicit operator int(WEdge<T> e) => e.To;
        public override bool Equals(object obj) => obj is WEdge<T> edge && Equals(edge);
        public bool Equals(WEdge<T> other) => To == other.To &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value);
        public override int GetHashCode() => HashCode.Combine(To, Value);
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
