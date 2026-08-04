using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 永続赤黒木
    /// </summary>
    public sealed class ImmutableRedBlackTree<T>
        : ImmutableBinarySearchTreeBase<T, ImmutableRedBlackTree<T>, ImmutableRedBlackTreeNode<T, SingleBbstOp<T>>, ImmutableRedBlackTree<T>.Mk, ImmutableRedBlackTreeNode<T, SingleBbstOp<T>>.Op>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableRedBlackTree<T>, ImmutableRedBlackTreeNode<T, SingleBbstOp<T>>>
        {
            [凾(256)]
            public static ImmutableRedBlackTree<T> Create(ImmutableRedBlackTreeNode<T, SingleBbstOp<T>> node) => new(node);
        }
        [凾(256)] public static ImmutableRedBlackTree<T> Create(ImmutableRedBlackTreeNode<T, SingleBbstOp<T>> node) => new(node);
        [凾(256)] public static ImmutableRedBlackTree<T> Create() => Empty;
#if NET9_0_OR_GREATER
        [凾(256)] public static ImmutableRedBlackTree<T> Create(params ReadOnlySpan<T> v) => new(v);
#else
        [凾(256)] public static ImmutableRedBlackTree<T> Create(params T[] v) => new(v);
        [凾(256)] public static ImmutableRedBlackTree<T> Create(ReadOnlySpan<T> v) => new(v);
#endif
        ImmutableRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableRedBlackTree(ImmutableRedBlackTreeNode<T, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// 永続赤黒木
    /// </summary>
    public sealed class ImmutableRedBlackTree<T, TOp>
        : ImmutableBinarySearchTreeBase<T, ImmutableRedBlackTree<T, TOp>, ImmutableRedBlackTreeNode<T, TOp>, ImmutableRedBlackTree<T, TOp>.Mk, ImmutableRedBlackTreeNode<T, TOp>.Op>
        where TOp : struct, ISegtreeOperator<T>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableRedBlackTree<T, TOp>, ImmutableRedBlackTreeNode<T, TOp>>
        {
            [凾(256)]
            public static ImmutableRedBlackTree<T, TOp> Create(ImmutableRedBlackTreeNode<T, TOp> node) => new(node);
        }
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create(ImmutableRedBlackTreeNode<T, TOp> node) => new(node);
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create() => Empty;
#if NET9_0_OR_GREATER
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create(params ReadOnlySpan<T> v) => new(v);
#else
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create(params T[] v) => new(v);
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create(ReadOnlySpan<T> v) => new(v);
#endif
        private ImmutableRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableRedBlackTree(ImmutableRedBlackTreeNode<T, TOp> root) : base(root) { }
    }

    namespace Internal
    {
        public class ImmutableRedBlackTreeNode<T, TOp> : RedBlackTreeNodeBase<ImmutableRedBlackTreeNode<T, TOp>, T>
            where TOp : struct, ISegtreeOperator<T>
        {
            public ImmutableRedBlackTreeNode(ImmutableRedBlackTreeNode<T, TOp> other) : base(other) { }
            public ImmutableRedBlackTreeNode(T v) : base(v) { }
            public ImmutableRedBlackTreeNode(ImmutableRedBlackTreeNode<T, TOp> left, ImmutableRedBlackTreeNode<T, TOp> right)
                : base(left, right, new TOp().Operate(left != null ? left.Sum : new TOp().Identity, right != null ? right.Sum : new TOp().Identity))
            { }
            public struct Op : IRbtNodeOp<T, TOp, ImmutableRedBlackTreeNode<T, TOp>, Op>
            {
                [凾(256)]
                public static ImmutableRedBlackTreeNode<T, TOp> Create(ImmutableRedBlackTreeNode<T, TOp> left, ImmutableRedBlackTreeNode<T, TOp> right) => new(left, right);
                [凾(256)]
                public static ImmutableRedBlackTreeNode<T, TOp> Create(T v) => new(v);
                [凾(256)]
                public static ImmutableRedBlackTreeNode<T, TOp> Copy(ImmutableRedBlackTreeNode<T, TOp> t) => t == null ? t : new(t);
            }
        }
    }
}