using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 永続赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class ImmutableRedBlackTree<T>
        : ImmutableBinarySearchTreeBase<T, ImmutableRedBlackTreeNode<T, SingleBbstOp<T>>, ImmutableRedBlackTree<T>>
        , IImmutableBbst<T, ImmutableRedBlackTreeNode<T, SingleBbstOp<T>>, ImmutableRedBlackTree<T>>
    {
        [凾(256)] public static ImmutableRedBlackTree<T> Create(ImmutableRedBlackTreeNode<T, SingleBbstOp<T>> node) => new(node);
        [凾(256)] public static ImmutableRedBlackTree<T> Create() => Empty;
#if NET9_0_OR_GREATER
        [凾(256)] public static ImmutableRedBlackTree<T> Create(params ReadOnlySpan<T> v) => new(v);
#else
        [凾(256)] public static ImmutableRedBlackTree<T> Create(params T[] v) => new(v);
        [凾(256)] public static ImmutableRedBlackTree<T> Create(ReadOnlySpan<T> v) => new(v);
#endif
        private ImmutableRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableRedBlackTree(ImmutableRedBlackTreeNode<T, SingleBbstOp<T>> root) : base(root) { }
        public ImmutableRedBlackTreeNode<T, SingleBbstOp<T>>.Enumerator GetEnumerator()
        {
            ImmutableRedBlackTreeNode<T, SingleBbstOp<T>>.GetEnumerator(ref root);
            return new(root);
        }
    }

    /// <summary>
    /// 永続赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class ImmutableRedBlackTree<T, TOp>
        : ImmutableBinarySearchTreeBase<T, ImmutableRedBlackTreeNode<T, TOp>, ImmutableRedBlackTree<T, TOp>>
        , IImmutableBbst<T, ImmutableRedBlackTreeNode<T, TOp>, ImmutableRedBlackTree<T, TOp>>
        where TOp : struct, ISegtreeOperator<T>
    {
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
        public ImmutableRedBlackTreeNode<T, TOp>.Enumerator GetEnumerator()
        {
            ImmutableRedBlackTreeNode<T, TOp>.GetEnumerator(ref root);
            return new(root);
        }
    }

    namespace Internal
    {
        public class ImmutableRedBlackTreeNode<T, TOp>
            : RedBlackTreeNode<T, TOp, ImmutableRedBlackTreeNode<T, TOp>>
            , IRedBlackTreeNode<T, ImmutableRedBlackTreeNode<T, TOp>>, IBbstNode<T, ImmutableRedBlackTreeNode<T, TOp>>
            where TOp : struct, ISegtreeOperator<T>
        {
            ImmutableRedBlackTreeNode(D d) : base(
                d is Internal e
                ? new Internal { left = e.left, right = e.right, Level = e.Level }
                : new Leaf { Value = Unsafe.As<Leaf>(d).Value }
            )
            { }
            public ImmutableRedBlackTreeNode(T v) : base(v) { }
            public ImmutableRedBlackTreeNode(ImmutableRedBlackTreeNode<T, TOp> left, ImmutableRedBlackTreeNode<T, TOp> right) : base(left, right) { }
            [凾(256)] public static ImmutableRedBlackTreeNode<T, TOp> Create(T v) => new(v);
            [凾(256)] public static ImmutableRedBlackTreeNode<T, TOp> Create(ImmutableRedBlackTreeNode<T, TOp> left, ImmutableRedBlackTreeNode<T, TOp> right) => new(left, right);
            [凾(256)]
            static T IBbstNode<T, ImmutableRedBlackTreeNode<T, TOp>>.Sum(ImmutableRedBlackTreeNode<T, TOp> t)
                => GetSum(t);
            [凾(256)]
            static ImmutableRedBlackTreeNode<T, TOp> IBbstNode<T, ImmutableRedBlackTreeNode<T, TOp>>.Copy(ImmutableRedBlackTreeNode<T, TOp> t)
            {
                if (t == null) return t;
                return new(t.Data)
                {
                    IsBlack = t.IsBlack,
                    Size = t.Size,
                    Sum = t.Sum,
                };
            }
        }
    }
}