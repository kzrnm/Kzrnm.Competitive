using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    /// <summary>
    /// Splay 木
    /// </summary>
    public class LazySplayTree<T> : LazySplayTree<T, byte, SingleBbstOp<T>>
    {
        public LazySplayTree() { }
        public LazySplayTree(IEnumerable<T> v) : base(v) { }
        public LazySplayTree(T[] v) : base(v) { }
        public LazySplayTree(ReadOnlySpan<T> v) : base(v) { }
        public LazySplayTree(LazySplayTreeNode<T, byte, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// 遅延伝播反転可能 Splay 木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class LazySplayTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazySplayTreeNode<T, F, TOp>, LazySplayTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazySplayTree() { }
        public LazySplayTree(IEnumerable<T> v) : base(v) { }
        public LazySplayTree(T[] v) : base(v) { }
        public LazySplayTree(ReadOnlySpan<T> v) : base(v) { }
        public LazySplayTree(LazySplayTreeNode<T, F, TOp> root) : base(root) { }
    }

    public class LazySplayTreeNode<T, F, TOp> : SplayTreeNodeBase<LazySplayTreeNode<T, F, TOp>, T>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public struct Op : ISplayTreePusher<T, LazySplayTreeNode<T, F, TOp>, Op>, ILazyBbstNodeOp<T, F, LazySplayTreeNode<T, F, TOp>, Op>
        {
            [凾(256)]
            public static LazySplayTreeNode<T, F, TOp> Create(T v) => new(v);

            [凾(256)]
            public static T Operate(T x, T y) => op.Operate(x, y);

            [凾(256)]
            public static void Push(LazySplayTreeNode<T, F, TOp> t)
            {
                if (!EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity))
                {
                    t.left?.Apply(t.Lazy);
                    t.right?.Apply(t.Lazy);
                    t.Lazy = op.FIdentity;
                }
                if (t.IsReverse)
                {
                    t.left?.Reverse();
                    t.right?.Reverse();
                    t.IsReverse = false;
                }
            }

            [凾(256)]
            public static LazySplayTreeNode<T, F, TOp> Apply(LazySplayTreeNode<T, F, TOp> t, F f)
            {
                if (t != null)
                {
                    ISplayTreePusher<T, LazySplayTreeNode<T, F, TOp>, Op>.Splay(t);
                    t.Apply(f);
                    Push(t);
                }
                return t;
            }

            [凾(256)]
            public static LazySplayTreeNode<T, F, TOp> Reverse(LazySplayTreeNode<T, F, TOp> t)
            {
                t?.Reverse();
                return t;
            }

            [凾(256)]
            public static T Sum(LazySplayTreeNode<T, F, TOp> t)
                => t != null ? t.Sum : op.Identity;
        }

        static TOp op => new();
        public F Lazy;
        public bool IsReverse;
        public LazySplayTreeNode(T v)
        {
            Size = 1;
            Sum = Value = v;
            Lazy = op.FIdentity;
        }

        [凾(256)]
        public void Apply(F f)
        {
            Lazy = op.Composition(f, Lazy);
            Value = op.Mapping(f, Value, 1);
            Sum = op.Mapping(f, Sum, Size);
        }

        [凾(256)]
        public void Reverse()
        {
            (left, right) = (right, left);
            Sum = op.Inverse(Sum);
            IsReverse = !IsReverse;
        }

        [SourceExpander.NotEmbeddingSource]
        public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
    }
}
