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
    public class LazyRandomBinarySearchTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyRandomBinarySearchTreeNode<T, F, TOp>, LazyRandomBinarySearchTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyRandomBinarySearchTree() { }
        public LazyRandomBinarySearchTree(IEnumerable<T> v) : base(v) { }
        public LazyRandomBinarySearchTree(T[] v) : base(v) { }
        public LazyRandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyRandomBinarySearchTree(LazyRandomBinarySearchTreeNode<T, F, TOp> root) : base(root) { }
    }

    public class LazyRandomBinarySearchTreeNode<T, F, TOp>
        : RandomBinarySearchTreeNodeBase<LazyRandomBinarySearchTreeNode<T, F, TOp>, T>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public struct Op : IRbstNodeOp<T, LazyRandomBinarySearchTreeNode<T, F, TOp>, Op>, ILazyBbstNodeOp<T, F, LazyRandomBinarySearchTreeNode<T, F, TOp>, Op>
        {

            [凾(256)]
            public static LazyRandomBinarySearchTreeNode<T, F, TOp> Create(T v) => new(v);

            [凾(256)]
            public static T Operate(T x, T y) => op.Operate(x, y);

            [凾(256)]
            public static LazyRandomBinarySearchTreeNode<T, F, TOp> Apply(LazyRandomBinarySearchTreeNode<T, F, TOp> t, F f)
            {
                if (t != null)
                {
                    t.Lazy = op.Composition(f, t.Lazy);
                    Propagate(ref t);
                }
                return t;
            }

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
            public static LazyRandomBinarySearchTreeNode<T, F, TOp> Reverse(LazyRandomBinarySearchTreeNode<T, F, TOp> t)
            {
                t?.Toggle();
                return t;
            }

            [凾(256)]
            public static T Sum(LazyRandomBinarySearchTreeNode<T, F, TOp> t)
                => t != null ? t.Sum : op.Identity;

            [凾(256)]
            public static LazyRandomBinarySearchTreeNode<T, F, TOp> Update(LazyRandomBinarySearchTreeNode<T, F, TOp> t)
            {
                if (t == null) return t;
                t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
                t.Sum = op.Operate(op.Operate(Sum(t.left), t.Value), Sum(t.right));
                return t;
            }
        }

        static TOp op => new();
        public F Lazy;
        public bool IsReverse;
        public LazyRandomBinarySearchTreeNode(T v)
        {
            Size = 1;
            Sum = Value = v;
            Lazy = op.FIdentity;
        }

        [SourceExpander.NotEmbeddingSource]
        public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}, Lazy = {Lazy}";

        [凾(256)]
        void Toggle()
        {
            (left, right) = (right, left);
            Sum = op.Inverse(Sum);
            IsReverse = !IsReverse;
        }
    }
}
