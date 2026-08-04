using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/randomized-binary-search-tree-lazy.hpp
    /// <summary>
    /// 乱択平衡二分探索木
    /// </summary>
    public class RandomBinarySearchTree<T> : RandomBinarySearchTree<T, SingleBbstOp<T>>
    {
        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : base(v) { }
        public RandomBinarySearchTree(T[] v) : base(v) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public RandomBinarySearchTree(RandomBinarySearchTreeNode<T, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// 乱択平衡二分探索木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RandomBinarySearchTree<T, TOp> : BinarySearchTreeBase<T, RandomBinarySearchTreeNode<T, TOp>, RandomBinarySearchTreeNode<T, TOp>.Op>
        where TOp : struct, ISegtreeOperator<T>
    {
        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : base(v) { }
        public RandomBinarySearchTree(T[] v) : base(v) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public RandomBinarySearchTree(RandomBinarySearchTreeNode<T, TOp> root) : base(root) { }
    }

    public class RandomBinarySearchTreeNode<T, TOp> : RandomBinarySearchTreeNodeBase<RandomBinarySearchTreeNode<T, TOp>, T>
        where TOp : struct, ISegtreeOperator<T>
    {
        public struct Op : IRbstNodeOp<T, RandomBinarySearchTreeNode<T, TOp>, Op>
        {
            [凾(256)]
            public static RandomBinarySearchTreeNode<T, TOp> Create(T v) => new(v);

            [凾(256)]
            public static T Operate(T x, T y) => op.Operate(x, y);

            [凾(256)]
            public static void Propagate(ref RandomBinarySearchTreeNode<T, TOp> t)
            {
            }

            [凾(256)]
            public static void Push(RandomBinarySearchTreeNode<T, TOp> t)
            {
            }

            [凾(256)]
            public static T Sum(RandomBinarySearchTreeNode<T, TOp> t)
                => t != null ? t.Sum : op.Identity;

            [凾(256)]
            public static RandomBinarySearchTreeNode<T, TOp> Update(RandomBinarySearchTreeNode<T, TOp> t)
            {
                if (t == null) return t;
                t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
                t.Sum = op.Operate(op.Operate(Sum(t.left), t.Value), Sum(t.right));
                return t;
            }
        }

        static TOp op => new();
        public RandomBinarySearchTreeNode(T v)
        {
            Size = 1;
            Sum = Value = v;
        }

        [SourceExpander.NotEmbeddingSource]
        public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
    }
}
