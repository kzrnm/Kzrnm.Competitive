using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    /// <summary>
    /// 遅延伝播乱択平衡二分探索木
    /// </summary>
    public class LazyRandomBinarySearchTree<T> : LazyRandomBinarySearchTree<T, byte, SingleBbstOp<T>>
    {
        public LazyRandomBinarySearchTree() { }
        public LazyRandomBinarySearchTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazyRandomBinarySearchTree(T[] v) : base(v.AsSpan()) { }
        public LazyRandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
    }

    /// <summary>
    /// 遅延伝播乱択平衡二分探索木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class LazyRandomBinarySearchTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyRandomBinarySearchTreeNode<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyRandomBinarySearchTree() { }
        public LazyRandomBinarySearchTree(IEnumerable<T> v) : base(v) { }
        public LazyRandomBinarySearchTree(T[] v) : base(v) { }
        public LazyRandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyRandomBinarySearchTree(LazyRandomBinarySearchTreeNode<T, F, TOp> root) : base(root) { }

        [凾(256)]
        public LazyRandomBinarySearchTreeNode<T, F, TOp>.Enumerator GetEnumerator()
        {
            LazyRandomBinarySearchTreeNode<T, F, TOp>.Propagate(ref root);
            return new(root);
        }
    }

    public class LazyRandomBinarySearchTreeNode<T, F, TOp>
        : RandomBinarySearchTreeNodeBase<LazyRandomBinarySearchTreeNode<T, F, TOp>, T>
        , ILazyBbstNode<T, F, LazyRandomBinarySearchTreeNode<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        static TOp op => new();
        public F Lazy;
        public bool IsReverse;
        public LazyRandomBinarySearchTreeNode(T v)
        {
            Size = 1;
            Sum = Value = v;
            Lazy = op.FIdentity;
        }

        [凾(256)]
        public static LazyRandomBinarySearchTreeNode<T, F, TOp> Create(T v) => new(v);

        [凾(256)]
        public static void Propagate(ref LazyRandomBinarySearchTreeNode<T, F, TOp> t)
        {
            if (t == null) return;
            var lazy = !EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity);
            var rev = t.IsReverse;

            if (lazy)
            {
                t.Value = op.Mapping(t.Lazy, t.Value, 1);
                if (t.left != null)
                {
                    t.left.Lazy = op.Composition(t.Lazy, t.left.Lazy);
                    t.left.Sum = op.Mapping(t.Lazy, t.left.Sum, t.left.Size);
                }
                if (t.right != null)
                {
                    t.right.Lazy = op.Composition(t.Lazy, t.right.Lazy);
                    t.right.Sum = op.Mapping(t.Lazy, t.right.Sum, t.right.Size);
                }
                t.Lazy = op.FIdentity;
            }
            if (rev)
            {
                t.left?.Toggle();
                t.right?.Toggle();
                t.IsReverse = false;
            }

            t = Update(t);
        }

        [凾(256)]
        public static LazyRandomBinarySearchTreeNode<T, F, TOp> Update(LazyRandomBinarySearchTreeNode<T, F, TOp> t)
        {
            if (t == null) return t;
            t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
            t.Sum = op.Operate(op.Operate(GetSum(t.left), t.Value), GetSum(t.right));
            return t;
        }

        [凾(256)]
        static T GetSum(LazyRandomBinarySearchTreeNode<T, F, TOp> t)
            => t != null ? t.Sum : op.Identity;
        static T IBbstNode<T, LazyRandomBinarySearchTreeNode<T, F, TOp>>.Sum(LazyRandomBinarySearchTreeNode<T, F, TOp> t)
            => GetSum(t);

#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}, Lazy = {Lazy}";

        [凾(256)] public static void Reverse(LazyRandomBinarySearchTreeNode<T, F, TOp> t) => t?.Toggle();
        [凾(256)]
        void Toggle()
        {
            (left, right) = (right, left);
            Sum = op.Inverse(Sum);
            IsReverse = !IsReverse;
        }

        [凾(256)]
        public static void Apply(LazyRandomBinarySearchTreeNode<T, F, TOp> t, F f)
        {
            if (t == null) return;
            t.Lazy = op.Composition(f, t.Lazy);
            Propagate(ref t);
        }
    }
}
