using Kzrnm.Competitive.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 永続伝播反転可能赤黒木
    /// </summary>
    public sealed class ImmutableLazyRedBlackTree<T>
        : ImmutableBinarySearchTreeBase<T, ImmutableLazyRedBlackTree<T>, ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>>, ImmutableLazyRedBlackTree<T>.Mk, ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>>.Op>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableLazyRedBlackTree<T>, ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>>>
        {
            [凾(256)]
            public static ImmutableLazyRedBlackTree<T> Create(ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>> node) => new(node);
        }
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create(ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>> node) => new(node);
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create() => Empty;
#if NET9_0_OR_GREATER
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create(params ReadOnlySpan<T> v) => new(v);
#else
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create(params T[] v) => new(v);
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create(ReadOnlySpan<T> v) => new(v);
#endif
        ImmutableLazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableLazyRedBlackTree(ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// 永続遅延伝播反転可能赤黒木
    /// </summary>
    public sealed class ImmutableLazyRedBlackTree<T, F, TOp>
        : ImmutableLazyBinarySearchTreeBase<T, F, ImmutableLazyRedBlackTree<T, F, TOp>, ImmutableLazyRedBlackTreeNode<T, F, TOp>, ImmutableLazyRedBlackTree<T, F, TOp>.Mk, ImmutableLazyRedBlackTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableLazyRedBlackTree<T, F, TOp>, ImmutableLazyRedBlackTreeNode<T, F, TOp>>
        {
            [凾(256)]
            public static ImmutableLazyRedBlackTree<T, F, TOp> Create(ImmutableLazyRedBlackTreeNode<T, F, TOp> node) => new(node);
        }
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create(ImmutableLazyRedBlackTreeNode<T, F, TOp> node) => new(node);
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create() => Empty;
#if NET9_0_OR_GREATER
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create(params ReadOnlySpan<T> v) => new(v);
#else
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create(params T[] v) => new(v);
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create(ReadOnlySpan<T> v) => new(v);
#endif
        private ImmutableLazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableLazyRedBlackTree(ImmutableLazyRedBlackTreeNode<T, F, TOp> root) : base(root) { }
    }

    namespace Internal
    {
        public class ImmutableLazyRedBlackTreeNode<T, F, TOp>
            : RedBlackTreeNodeBase<ImmutableLazyRedBlackTreeNode<T, F, TOp>, T>, ILazyRbtNode<F>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        {
            public F Lazy { get; set; }
            public bool IsReverse { get; set; }
            public ImmutableLazyRedBlackTreeNode(ImmutableLazyRedBlackTreeNode<T, F, TOp> other) : base(other)
            {
                Lazy = other.Lazy;
                IsReverse = other.IsReverse;
            }
            public ImmutableLazyRedBlackTreeNode(T v) : base(v)
            {
                Lazy = new TOp().FIdentity;
            }
            public ImmutableLazyRedBlackTreeNode(ImmutableLazyRedBlackTreeNode<T, F, TOp> left, ImmutableLazyRedBlackTreeNode<T, F, TOp> right)
                : base(left, right, new TOp().Operate(left != null ? left.Sum : new TOp().Identity, right != null ? right.Sum : new TOp().Identity))
            {
                Lazy = new TOp().FIdentity;
            }

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => $"Lazy = {Lazy}{(IsReverse ? "!" : "")} {base.ToString()}";
            public struct Op : IRbtNodeOp<T, F, TOp, ImmutableLazyRedBlackTreeNode<T, F, TOp>, Op>
            {
                [凾(256)]
                public static ImmutableLazyRedBlackTreeNode<T, F, TOp> Create(ImmutableLazyRedBlackTreeNode<T, F, TOp> left, ImmutableLazyRedBlackTreeNode<T, F, TOp> right) => new(left, right);
                [凾(256)]
                public static ImmutableLazyRedBlackTreeNode<T, F, TOp> Create(T v) => new(v);
                [凾(256)]
                public static ImmutableLazyRedBlackTreeNode<T, F, TOp> Copy(ImmutableLazyRedBlackTreeNode<T, F, TOp> t) => t == null ? t : new(t);
            }
        }
    }
}