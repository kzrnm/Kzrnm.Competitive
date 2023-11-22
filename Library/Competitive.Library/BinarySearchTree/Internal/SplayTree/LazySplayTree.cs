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
    public class LazySplayTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazySplayTreeNode<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazySplayTree() { }
        public LazySplayTree(IEnumerable<T> v) : base(v) { }
        public LazySplayTree(T[] v) : base(v) { }
        public LazySplayTree(ReadOnlySpan<T> v) : base(v) { }
        public LazySplayTree(LazySplayTreeNode<T, F, TOp> root) : base(root) { }
        public LazySplayTreeNode<T, F, TOp>.Enumerator GetEnumerator()
        {
            LazySplayTreeNode<T, F, TOp>.GetEnumerator(ref root);
            return new(root);
        }
    }

    public class LazySplayTreeNode<T, F, TOp>
        : SplayTreeNodeBase<LazySplayTreeNode<T, F, TOp>, T>
        , ILazyBbstNode<T, F, LazySplayTreeNode<T, F, TOp>>
        , ISplayTreePusher<LazySplayTreeNode<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
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
        public static LazySplayTreeNode<T, F, TOp> Create(T v) => new(v);


        [凾(256)]
        public static LazySplayTreeNode<T, F, TOp> Update(LazySplayTreeNode<T, F, TOp> t)
        {
            if (t == null) return t;
            t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
            t.Sum = op.Operate(op.Operate(GetSum(t.left), t.Value), GetSum(t.right));
            return t;
        }

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
        static void ILazyBbstNode<T, F, LazySplayTreeNode<T, F, TOp>>.Apply(LazySplayTreeNode<T, F, TOp> t, F f)
        {
            if (t == null) return;
            Splay(t);
            t.Apply(f);
            Push(t);
        }

        [凾(256)]
        public void Apply(F f)
        {
            Lazy = op.Composition(f, Lazy);
            Value = op.Mapping(f, Value, 1);
            Sum = op.Mapping(f, Sum, Size);
        }

        [凾(256)]
        static void ILazyBbstNode<T, F, LazySplayTreeNode<T, F, TOp>>.Reverse(LazySplayTreeNode<T, F, TOp> t)
            => t?.Reverse();
        [凾(256)]
        public void Reverse()
        {
            (left, right) = (right, left);
            Sum = op.Inverse(Sum);
            IsReverse = !IsReverse;
        }

        [凾(256)]
        static T GetSum(LazySplayTreeNode<T, F, TOp> t)
            => t != null ? t.Sum : op.Identity;
        static T IBbstNode<T, LazySplayTreeNode<T, F, TOp>>.Sum(LazySplayTreeNode<T, F, TOp> t)
            => GetSum(t);
        public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";

    }
}
