using AtCoder;
using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 永続伝播反転可能赤黒木
    /// </summary>
    public sealed class ImmutableLazyRedBlackTree<T>
        : ImmutableBinarySearchTreeBase<T, ImmutableLazyRedBlackTree<T>, int, ImmutableLazyRedBlackTree<T>.Mk, ImmutableLazyRedBlackTreeNodeOp<T, byte, SingleBbstOp<T>>>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableLazyRedBlackTree<T>, int>
        {
            [凾(256)]
            public static ImmutableLazyRedBlackTree<T> Create(int node) => new(node);
        }
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create(ImmutableLazyRedBlackTree<T> other) => new(other.root);
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create() => Empty;
        [凾(256)] public static ImmutableLazyRedBlackTree<T> Create(params ReadOnlySpan<T> v) => new(v);
        ImmutableLazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableLazyRedBlackTree(int root) : base(root) { }
    }

    /// <summary>
    /// 永続遅延伝播反転可能赤黒木
    /// </summary>
    public sealed class ImmutableLazyRedBlackTree<T, F, TOp>
        : ImmutableLazyBinarySearchTreeBase<T, F, ImmutableLazyRedBlackTree<T, F, TOp>, int, ImmutableLazyRedBlackTree<T, F, TOp>.Mk, ImmutableLazyRedBlackTreeNodeOp<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public struct Mk : IImmutableBbstMaker<ImmutableLazyRedBlackTree<T, F, TOp>, int>
        {
            [凾(256)]
            public static ImmutableLazyRedBlackTree<T, F, TOp> Create(int node) => new(node);
        }
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create(ImmutableLazyRedBlackTree<T, F, TOp> other) => new(other.root);
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create() => Empty;
        [凾(256)] public static ImmutableLazyRedBlackTree<T, F, TOp> Create(params ReadOnlySpan<T> v) => new(v);
        private ImmutableLazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public ImmutableLazyRedBlackTree(int root) : base(root) { }
    }


    namespace Internal
    {
        public struct ImmutableLazyRedBlackTreeNodeOp<T, F, TOp> : ILazyRbtOp<T, F, TOp, LazyRedBlackTreeNode<T, F, TOp>, int, ImmutableLazyRedBlackTreeNodeOp<T, F, TOp>, PoolStructRefOp<LazyRedBlackTreeNode<T, F, TOp>>>
                  , IBbstStructNodeOp<T, LazyRedBlackTreeNode<T, F, TOp>, ImmutableLazyRedBlackTreeNodeOp<T, F, TOp>>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        {
            [凾(256)] public static LazyRedBlackTreeNode<T, F, TOp> CreateNode(T v) => new(v);

            [凾(256)]
            public static int Copy(int t)
            {
                if (t < 0) return t;
                StructPool<LazyRedBlackTreeNode<T, F, TOp>>.Default.Rent(out var i) = new(StructPool<LazyRedBlackTreeNode<T, F, TOp>>.Default.Get(t));
                return i;
            }
        }
    }
}
