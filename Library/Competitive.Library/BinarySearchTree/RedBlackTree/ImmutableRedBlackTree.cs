using AtCoder;
using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 永続赤黒木
    /// </summary>
    public sealed class ImmutableRedBlackTree<T>
        : ImmutableBinarySearchTreeBase<T, ImmutableRedBlackTree<T>, int, ImmutableRedBlackTree<T>.Mk, ImmutableRedBlackTreeNodeOp<T, SingleBbstOp<T>>>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableRedBlackTree<T>, int>
        {
            [凾(256)]
            public static ImmutableRedBlackTree<T> Create(int node) => new(node);
        }
        [凾(256)] public static ImmutableRedBlackTree<T> Create(ImmutableRedBlackTree<T> other) => new(other.root);
        [凾(256)] public static ImmutableRedBlackTree<T> Create() => Empty;
        [凾(256)] public static ImmutableRedBlackTree<T> Create(params ReadOnlySpan<T> v) => new(v);
        ImmutableRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableRedBlackTree(int root) : base(root) { }
    }

    /// <summary>
    /// 永続赤黒木
    /// </summary>
    public sealed class ImmutableRedBlackTree<T, TOp>
        : ImmutableBinarySearchTreeBase<T, ImmutableRedBlackTree<T, TOp>, int, ImmutableRedBlackTree<T, TOp>.Mk, ImmutableRedBlackTreeNodeOp<T, TOp>>
        where TOp : struct, ISegtreeOperator<T>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableRedBlackTree<T, TOp>, int>
        {
            [凾(256)]
            public static ImmutableRedBlackTree<T, TOp> Create(int node) => new(node);
        }
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create(ImmutableRedBlackTree<T, TOp> other) => new(other.root);
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create() => Empty;
        [凾(256)] public static ImmutableRedBlackTree<T, TOp> Create(params ReadOnlySpan<T> v) => new(v);
        private ImmutableRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableRedBlackTree(int root) : base(root) { }
    }

    namespace Internal
    {
        public struct ImmutableRedBlackTreeNodeOp<T, TOp> : IRbtOp<T, TOp, RedBlackTreeNode<T, TOp>, int, ImmutableRedBlackTreeNodeOp<T, TOp>, PoolStructRefOp<RedBlackTreeNode<T, TOp>>>
                  , IBbstStructNodeOp<T, RedBlackTreeNode<T, TOp>, ImmutableRedBlackTreeNodeOp<T, TOp>>
            where TOp : struct, ISegtreeOperator<T>
        {
            [凾(256)] public static RedBlackTreeNode<T, TOp> CreateNode(T v) => new(v);

            [凾(256)]
            public static int Create(T v)
            {
                StructPool<RedBlackTreeNode<T, TOp>>.Default.Rent(out var i) = new(v);
                return i;
            }

            [凾(256)]
            public static int Copy(int t)
            {
                if (t < 0) return t;
                StructPool<RedBlackTreeNode<T, TOp>>.Default.Rent(out var i) = new(StructPool<RedBlackTreeNode<T, TOp>>.Default.Get(t));
                return i;
            }
            [凾(256)] public static void Free(int i) => StructPool<RedBlackTreeNode<T, TOp>>.Default.Return(i);
        }
    }
}