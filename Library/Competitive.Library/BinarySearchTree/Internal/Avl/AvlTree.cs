using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// AVL木
    /// </summary>
    public class AvlTree<T> : AvlTree<T, SingleBbstOp<T>>
    {
        public AvlTree() { }
        public AvlTree(IEnumerable<T> v) : base(v) { }
        public AvlTree(T[] v) : base(v) { }
        public AvlTree(ReadOnlySpan<T> v) : base(v) { }
        public AvlTree(AvlTreeNode<T, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// AVL木
    /// </summary>
    public class AvlTree<T, TOp> : BinarySearchTreeBase<T, AvlTreeNode<T, TOp>, AvlTreeNode<T, TOp>.Op>
        where TOp : struct, ISegtreeOperator<T>
    {
        public AvlTree() { }
        public AvlTree(IEnumerable<T> v) : base(v) { }
        public AvlTree(T[] v) : base(v) { }
        public AvlTree(ReadOnlySpan<T> v) : base(v) { }
        public AvlTree(AvlTreeNode<T, TOp> root) : base(root) { }
    }

    public class AvlTreeNode<T, TOp> : AvlTreeNodeBase<AvlTreeNode<T, TOp>, T>
        where TOp : struct, ISegtreeOperator<T>
    {
        public AvlTreeNode(T v) : base(v) { }
        public struct Op : IAvlNodeOp<T, TOp, AvlTreeNode<T, TOp>, Op>
        {
            [凾(256)]
            public static AvlTreeNode<T, TOp> Create(T v) => new(v);
        }
    }

    public interface IAvlNodeOp<T, TOp, Nd, N> : IAvlNodeOp<T, Nd, N>
        where TOp : struct, ISegtreeOperator<T>
        where Nd : AvlTreeNodeBase<Nd, T>
        where N : IAvlNodeOp<T, TOp, Nd, N>
    {
        [凾(256)]
        static void IBbstNodeOp<Nd, N>.Propagate(ref Nd t) => t = N.Copy(t);

        [凾(256)] static T IAvlNodeOp<T, Nd, N>.Prod(T l, T r) => new TOp().Operate(l, r);

        [凾(256)]
        static T IBbstNodeOp<T, Nd, N>.Sum(Nd t) => t != null ? t.Sum : new TOp().Identity;
    }
}
