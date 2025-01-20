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
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class LazyRedBlackTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyRedBlackTreeNode<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyRedBlackTree() { }
        public LazyRedBlackTree(IEnumerable<T> v) : base(v) { }
        public LazyRedBlackTree(T[] v) : base(v) { }
        public LazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyRedBlackTree(LazyRedBlackTreeNode<T, F, TOp> root) : base(root) { }
        public LazyRedBlackTreeNode<T, F, TOp>.Enumerator GetEnumerator()
        {
            LazyRedBlackTreeNode<T, F, TOp>.GetEnumerator(ref root);
            return new(root);
        }
    }

    namespace Internal
    {
        public class LazyRedBlackTreeNode<T, F, TOp>
            : LazyRedBlackTreeNode<T, F, TOp, LazyRedBlackTreeNode<T, F, TOp>>
            , IRedBlackTreeNode<T, LazyRedBlackTreeNode<T, F, TOp>>, ILazyBbstNode<T, F, LazyRedBlackTreeNode<T, F, TOp>>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        {
            public LazyRedBlackTreeNode(T v) : base(v) { }
            public LazyRedBlackTreeNode(LazyRedBlackTreeNode<T, F, TOp> left, LazyRedBlackTreeNode<T, F, TOp> right) : base(left, right) { }
            [凾(256)] public static LazyRedBlackTreeNode<T, F, TOp> Create(T v) => new(v);
            [凾(256)] public static LazyRedBlackTreeNode<T, F, TOp> Create(LazyRedBlackTreeNode<T, F, TOp> left, LazyRedBlackTreeNode<T, F, TOp> right) => new(left, right);
            [凾(256)]
            static T IBbstNode<T, LazyRedBlackTreeNode<T, F, TOp>>.Sum(LazyRedBlackTreeNode<T, F, TOp> t)
                => GetSum(t);
        }
        public class LazyRedBlackTreeNode<T, F, TOp, TSelf> : RedBlackTreeNodeBase<TSelf, T>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
            where TSelf : LazyRedBlackTreeNode<T, F, TOp, TSelf>, IRedBlackTreeNode<T, TSelf>, ILazyBbstNode<T, F, TSelf>
        {
            internal static TOp op => new();
            public F Lazy;
            public bool IsReverse;

            protected LazyRedBlackTreeNode(D data)
            {
                Data = data;
            }
            public LazyRedBlackTreeNode(T v)
            {
                IsBlack = true;
                Data = new Leaf { Value = v };
                Sum = v;
                Size = 1;
                Lazy = op.FIdentity;
            }
            public LazyRedBlackTreeNode(TSelf left, TSelf right)
            {
                IsBlack = false;
                Data = new Internal { left = left, right = right, };
                Size = (left?.Size ?? 0) + (right?.Size ?? 0);
                Sum = op.Operate(GetSum(left), GetSum(right));
                Lazy = op.FIdentity;
            }
            [凾(256)]
            public static void Propagate(ref TSelf t)
            {
                if (t == null) return;
                t = TSelf.Copy(t);
                var lazy = !EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity);
                var rev = t.IsReverse;

                if (!lazy && !rev) return;

                var e = t.Data as Internal;

                if (e != null)
                {
                    if (e.left != null)
                        e.left = TSelf.Copy(e.left);
                    if (e.right != null)
                        e.right = TSelf.Copy(e.right);
                }
                if (lazy)
                {
                    if (t.Data is Leaf lv)
                    {
                        lv.Value = op.Mapping(t.Lazy, lv.Value, 1);
                    }
                    else if (e != null)
                    {
                        if (e.left != null)
                        {
                            e.left.Lazy = op.Composition(t.Lazy, e.left.Lazy);
                            e.left.Sum = op.Mapping(t.Lazy, e.left.Sum, e.left.Size);
                        }
                        if (e.right != null)
                        {
                            e.right.Lazy = op.Composition(t.Lazy, e.right.Lazy);
                            e.right.Sum = op.Mapping(t.Lazy, e.right.Sum, e.right.Size);
                        }
                    }
                    t.Lazy = op.FIdentity;
                }
                if (rev && e != null)
                {
                    if (e.left != null)
                        Reverse(e.left);
                    if (e.right != null)
                        Reverse(e.right);
                }
                t.IsReverse = false;
                TSelf.Update(t);
            }

            [凾(256)]
            public static TSelf Update(TSelf t)
            {
                if (t == null) return t;
                if (t.Data is Internal e)
                {
                    t.Size = (e.left?.Size ?? 0) + (e.right?.Size ?? 0);
                    t.Sum = op.Operate(GetSum(e.left), GetSum(e.right));
                }
                else if (t.Data is Leaf lf)
                {
                    t.Size = 1;
                    t.Sum = lf.Value;
                }
                return t;
            }

            [凾(256)]
            public static void Apply(TSelf t, F f)
            {
                if (t == null) return;
                t.Lazy = op.Composition(f, t.Lazy);
                Propagate(ref t);
            }
            [凾(256)] public static void Reverse(TSelf t) => t?.Toggle();
            [凾(256)]
            void Toggle()
            {
                if (Data is Internal e)
                    (e.left, e.right) = (e.right, e.left);
                Sum = op.Inverse(Sum);
                IsReverse = !IsReverse;
            }

            [凾(256)]
            public static T GetSum(TSelf t)
                => t != null ? t.Sum : op.Identity;

#if !LIBRARY
            [SourceExpander.NotEmbeddingSource]
#endif
            public override string ToString() => Data switch
            {
                Leaf lf => $"Size = {Size}, Value = {lf.Value}, Sum = {Sum}, Lazy = {Lazy}",
                _ => $"Size = {Size}, Sum = {Sum}, Lazy = {Lazy}",
            };
        }
    }
}