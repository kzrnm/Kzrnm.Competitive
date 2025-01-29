using Kzrnm.Competitive.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 永続伝播反転可能赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class ImmutableLazyRedBlackTree<T>
        : ImmutableBinarySearchTreeBase<T, ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>>, ImmutableLazyRedBlackTree<T>>
        , IImmutableBbst<T, ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>>, ImmutableLazyRedBlackTree<T>>
    {
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
        public ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>>.Enumerator GetEnumerator()
        {
            ImmutableLazyRedBlackTreeNode<T, byte, SingleBbstOp<T>>.GetEnumerator(ref root);
            return new(root);
        }
    }

    /// <summary>
    /// 永続遅延伝播反転可能赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class ImmutableLazyRedBlackTree<T, F, TOp>
        : ImmutableLazyBinarySearchTreeBase<T, F, ImmutableLazyRedBlackTreeNode<T, F, TOp>, ImmutableLazyRedBlackTree<T, F, TOp>>
        , IImmutableBbst<T, ImmutableLazyRedBlackTreeNode<T, F, TOp>, ImmutableLazyRedBlackTree<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
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
        public ImmutableLazyRedBlackTreeNode<T, F, TOp>.Enumerator GetEnumerator()
        {
            ImmutableLazyRedBlackTreeNode<T, F, TOp>.GetEnumerator(ref root);
            return new(root);
        }
    }

    namespace Internal
    {
        public class ImmutableLazyRedBlackTreeNode<T, F, TOp>
            : LazyRedBlackTreeNode<T, F, TOp, ImmutableLazyRedBlackTreeNode<T, F, TOp>>
            , IRedBlackTreeNode<T, ImmutableLazyRedBlackTreeNode<T, F, TOp>>, IBbstNode<T, ImmutableLazyRedBlackTreeNode<T, F, TOp>>
            , ILazyBbstNode<T, F, ImmutableLazyRedBlackTreeNode<T, F, TOp>>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        {
            ImmutableLazyRedBlackTreeNode(D d) : base(
                d is Internal e
                ? new Internal { left = e.left, right = e.right, Level = e.Level }
                : new Leaf { Value = Unsafe.As<Leaf>(d).Value }
            )
            { }
            public ImmutableLazyRedBlackTreeNode(T v) : base(v) { }
            public ImmutableLazyRedBlackTreeNode(ImmutableLazyRedBlackTreeNode<T, F, TOp> left, ImmutableLazyRedBlackTreeNode<T, F, TOp> right) : base(left, right) { }
            [凾(256)] public static ImmutableLazyRedBlackTreeNode<T, F, TOp> Create(T v) => new(v);
            [凾(256)] public static ImmutableLazyRedBlackTreeNode<T, F, TOp> Create(ImmutableLazyRedBlackTreeNode<T, F, TOp> left, ImmutableLazyRedBlackTreeNode<T, F, TOp> right) => new(left, right);
            [凾(256)]
            static T IBbstNode<T, ImmutableLazyRedBlackTreeNode<T, F, TOp>>.Sum(ImmutableLazyRedBlackTreeNode<T, F, TOp> t)
                => GetSum(t);
            [凾(256)]
            static ImmutableLazyRedBlackTreeNode<T, F, TOp> IBbstNode<T, ImmutableLazyRedBlackTreeNode<T, F, TOp>>.Copy(ImmutableLazyRedBlackTreeNode<T, F, TOp> t)
            {
                if (t == null) return t;
                return new(t.Data)
                {
                    IsBlack = t.IsBlack,
                    Size = t.Size,
                    Sum = t.Sum,
                    Lazy = t.Lazy,
                    IsReverse = t.IsReverse,
                };
            }
        }
    }
}