using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 遅延伝播反転可能赤黒木
    /// </summary>
    public class LazyRedBlackTree<T> : LazyRedBlackTree<T, byte, SingleBbstOp<T>>
    {
        public LazyRedBlackTree() { }
        public LazyRedBlackTree(IEnumerable<T> v) : base(v) { }
        public LazyRedBlackTree(T[] v) : base(v) { }
        public LazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyRedBlackTree(LazyRedBlackTreeNode<T, byte, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// 遅延伝播反転可能赤黒木
    /// </summary>
    public class LazyRedBlackTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyRedBlackTreeNode<T, F, TOp>, LazyRedBlackTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyRedBlackTree() { }
        public LazyRedBlackTree(IEnumerable<T> v) : base(v) { }
        public LazyRedBlackTree(T[] v) : base(v) { }
        public LazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyRedBlackTree(LazyRedBlackTreeNode<T, F, TOp> root) : base(root) { }
    }

    namespace Internal
    {
        public class LazyRedBlackTreeNode<T, F, TOp> : RedBlackTreeNodeBase<LazyRedBlackTreeNode<T, F, TOp>, T>, ILazyRbtNode<F>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        {
            public F Lazy { get; set; }
            public bool IsReverse { get; set; }
            public LazyRedBlackTreeNode(T v) : base(v)
            {
                Lazy = new TOp().FIdentity;
            }
            public LazyRedBlackTreeNode(LazyRedBlackTreeNode<T, F, TOp> left, LazyRedBlackTreeNode<T, F, TOp> right)
                : base(left, right, new TOp().Operate(left != null ? left.Sum : new TOp().Identity, right != null ? right.Sum : new TOp().Identity))
            {
                Lazy = new TOp().FIdentity;
            }

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => $"Lazy = {Lazy}{(IsReverse ? "!" : "")} {base.ToString()}";

            public struct Op : IRbtNodeOp<T, F, TOp, LazyRedBlackTreeNode<T, F, TOp>, Op>
            {
                [凾(256)]
                public static LazyRedBlackTreeNode<T, F, TOp> Create(LazyRedBlackTreeNode<T, F, TOp> left, LazyRedBlackTreeNode<T, F, TOp> right) => new(left, right);
                [凾(256)]
                public static LazyRedBlackTreeNode<T, F, TOp> Create(T v) => new(v);
            }
        }

        public interface ILazyRbtNode<F> : IBbstNode
        {
            F Lazy { get; set; }
            bool IsReverse { get; set; }
        }

        public interface IRbtNodeOp<T, F, TOp, Nd, N> : IRbtNodeOp<T, Nd, N>, ILazyBbstNodeOp<T, F, Nd, N>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
            where Nd : RedBlackTreeNodeBase<Nd, T>, ILazyRbtNode<F>
            where N : IRbtNodeOp<T, F, TOp, Nd, N>
        {
            [凾(256)]
            static Nd ILazyBbstNodeOp<T, F, Nd, N>.Apply(Nd t, F f)
            {
                if (t != null)
                {
                    var op = new TOp();
                    if (t.IsLeaf)
                    {
                        t.Sum = op.Mapping(f, t.Sum, 1);
                        t.Lazy = op.FIdentity;
                    }
                    else
                        t.Lazy = op.Composition(f, t.Lazy);
                    N.Propagate(ref t);
                }
                return t;
            }

            [凾(256)]
            static Nd ILazyBbstNodeOp<T, F, Nd, N>.Reverse(Nd t)
            {
                if (t != null)
                {
                    (t.Left, t.Right) = (t.Right, t.Left);
                    t.Sum = new TOp().Inverse(t.Sum);
                    t.IsReverse = !t.IsReverse;
                }
                return t;
            }

            [凾(256)]
            static void IBbstNodeOp<Nd, N>.Propagate(ref Nd t)
            {
                t = N.Copy(t);
                if (t == null || t.IsLeaf) return;
                var op = new TOp();
                var lazy = !EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity);
                var rev = t.IsReverse;

                if (lazy || rev)
                {
                    t.Left = N.Copy(t.Left);
                    t.Right = N.Copy(t.Right);

                    if (lazy)
                    {
                        if (t.Left is { } tl)
                        {
                            tl.Lazy = op.Composition(t.Lazy, tl.Lazy);
                            tl.Sum = op.Mapping(t.Lazy, tl.Sum, tl.Size);
                        }
                        if (t.Right is { } tr)
                        {
                            tr.Lazy = op.Composition(t.Lazy, tr.Lazy);
                            tr.Sum = op.Mapping(t.Lazy, tr.Sum, tr.Size);
                        }
                        t.Lazy = op.FIdentity;
                    }
                    if (rev)
                    {
                        N.Reverse(t.Left);
                        N.Reverse(t.Right);
                    }
                    t.IsReverse = false;
                    N.Update(t);
                }
            }

            [凾(256)]
            static T IBbstNodeOp<T, Nd, N>.Sum(Nd t) => t != null ? t.Sum : new TOp().Identity;

            [凾(256)]
            static Nd IBbstNodeOp<Nd, N>.Update(Nd t)
            {
                if (t == null) return t;

                Debug.Assert(!t.IsLeaf || t.Size == 1);

                if (!t.IsLeaf)
                {
                    TOp op = new();
                    t.Sum = op.Operate(t.Left != null ? t.Left.Sum : op.Identity, t.Right != null ? t.Right.Sum : op.Identity);
                    t.Size = t.Left.Size + t.Right.Size;
                    t.Level = t.Left.UpperLevel();
                }
                return t;
            }
        }
    }
}