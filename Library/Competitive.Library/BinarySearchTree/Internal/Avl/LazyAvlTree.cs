using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    /// <summary>
    /// 遅延伝播反転可能AVL木
    /// </summary>
    public class LazyAvlTree<T> : LazyAvlTree<T, byte, SingleBbstOp<T>>
    {
        public LazyAvlTree() { }
        public LazyAvlTree(IEnumerable<T> v) : base(v) { }
        public LazyAvlTree(T[] v) : base(v) { }
        public LazyAvlTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyAvlTree(LazyAvlTreeNode<T, byte, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// 遅延伝播反転可能AVL木
    /// </summary>
    public class LazyAvlTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyAvlTreeNode<T, F, TOp>, LazyAvlTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyAvlTree() { }
        public LazyAvlTree(IEnumerable<T> v) : base(v) { }
        public LazyAvlTree(T[] v) : base(v) { }
        public LazyAvlTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyAvlTree(LazyAvlTreeNode<T, F, TOp> root) : base(root) { }
    }

    public class LazyAvlTreeNode<T, F, TOp> : AvlTreeNodeBase<LazyAvlTreeNode<T, F, TOp>, T>, ILazyAvlNode<F>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public F Lazy { get; set; }
        public bool IsReverse { get; set; }
        public LazyAvlTreeNode(T v) : base(v)
        {
            Lazy = new TOp().FIdentity;
        }

        [SourceExpander.NotEmbeddingSource]
        public override string ToString() => $"Lazy = {Lazy}{(IsReverse ? "!" : "")} {base.ToString()}";

        public struct Op : IAvlNodeOp<T, F, TOp, LazyAvlTreeNode<T, F, TOp>, Op>
        {
            [凾(256)]
            public static LazyAvlTreeNode<T, F, TOp> Create(T v) => new(v);
        }
    }

    public interface ILazyAvlNode<F> : IBbstNode
    {
        F Lazy { get; set; }
        bool IsReverse { get; set; }
    }

    public interface IAvlNodeOp<T, F, TOp, Nd, N> : IAvlNodeOp<T, Nd, N>, ILazyBbstNodeOp<T, F, Nd, N>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where Nd : AvlTreeNodeBase<Nd, T>, ILazyAvlNode<F>
        where N : IAvlNodeOp<T, F, TOp, Nd, N>
    {
        [凾(256)]
        static Nd ILazyBbstNodeOp<T, F, Nd, N>.Apply(Nd t, F f)
        {
            t.Lazy = new TOp().Composition(f, t.Lazy);
            N.Propagate(ref t);
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
            if (t == null) return;
            var op = new TOp();
            var lazy = !EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity);
            var rev = t.IsReverse;

            if (lazy || rev)
            {
                t.Left = N.Copy(t.Left);
                t.Right = N.Copy(t.Right);

                if (lazy)
                {
                    t.Value = op.Mapping(t.Lazy, t.Value, 1);
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

        [凾(256)] static T IAvlNodeOp<T, Nd, N>.Prod(T l, T r) => new TOp().Operate(l, r);

        [凾(256)]
        static T IBbstNodeOp<T, Nd, N>.Sum(Nd t) => t != null ? t.Sum : new TOp().Identity;
    }
}
