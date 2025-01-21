using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 赤黒木
    /// </summary>
    public class RedBlackTree<T> : RedBlackTree<T, SingleBbstOp<T>>
    {
        public RedBlackTree() { }
        public RedBlackTree(IEnumerable<T> v) : base(v) { }
        public RedBlackTree(T[] v) : base(v) { }
        public RedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public RedBlackTree(RedBlackTreeNode<T, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// 赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RedBlackTree<T, TOp> : BinarySearchTreeBase<T, RedBlackTreeNode<T, TOp>>
        where TOp : struct, ISegtreeOperator<T>
    {
        public RedBlackTree() { }
        public RedBlackTree(IEnumerable<T> v) : base(v) { }
        public RedBlackTree(T[] v) : base(v) { }
        public RedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public RedBlackTree(RedBlackTreeNode<T, TOp> root) : base(root) { }
        public RedBlackTreeNode<T, TOp>.Enumerator GetEnumerator()
        {
            RedBlackTreeNode<T, TOp>.GetEnumerator(ref root);
            return new(root);
        }
    }

    namespace Internal
    {
        public class RedBlackTreeNode<T, TOp>
            : RedBlackTreeNode<T, TOp, RedBlackTreeNode<T, TOp>>
            , IRedBlackTreeNode<T, RedBlackTreeNode<T, TOp>>, IBbstNode<T, RedBlackTreeNode<T, TOp>>
            where TOp : struct, ISegtreeOperator<T>
        {
            public RedBlackTreeNode(T v) : base(v) { }
            public RedBlackTreeNode(RedBlackTreeNode<T, TOp> left, RedBlackTreeNode<T, TOp> right) : base(left, right) { }
            [凾(256)] public static RedBlackTreeNode<T, TOp> Create(T v) => new(v);
            [凾(256)] public static RedBlackTreeNode<T, TOp> Create(RedBlackTreeNode<T, TOp> left, RedBlackTreeNode<T, TOp> right) => new(left, right);
            [凾(256)]
            static T IBbstNode<T, RedBlackTreeNode<T, TOp>>.Sum(RedBlackTreeNode<T, TOp> t)
                => GetSum(t);
        }
        public class RedBlackTreeNode<T, TOp, TSelf> : RedBlackTreeNodeBase<TSelf, T>
            where TOp : struct, ISegtreeOperator<T>
            where TSelf : RedBlackTreeNode<T, TOp, TSelf>, IRedBlackTreeNode<T, TSelf>
        {
            internal static TOp op => new();
            protected RedBlackTreeNode(D data)
            {
                Data = data;
            }
            public RedBlackTreeNode(T v)
            {
                IsBlack = true;
                Data = new Leaf { Value = v };
                Size = 1;
                Sum = v;
            }
            public RedBlackTreeNode(TSelf left, TSelf right)
            {
                IsBlack = false;
                Data = new Internal { left = left, right = right, };
                Size = (left?.Size ?? 0) + (right?.Size ?? 0);
                Sum = op.Operate(GetSum(left), GetSum(right));
            }

            [凾(256)]
            public static void Propagate(ref TSelf t)
            {
                t = TSelf.Copy(t);
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
            public static T GetSum(TSelf t)
                => t != null ? t.Sum : op.Identity;

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => Data switch
            {
                Leaf lf => $"Size = {Size}, Value = {lf.Value}, Sum = {Sum}",
                _ => $"Size = {Size}, Sum = {Sum}",
            };
        }
    }
}