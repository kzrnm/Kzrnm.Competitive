using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
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
    public class RandomBinarySearchTree<T, TOp> : BinarySearchTreeBase<T, RandomBinarySearchTreeNode<T, TOp>>
        where TOp : struct, ISegtreeOperator<T>
    {
        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : base(v) { }
        public RandomBinarySearchTree(T[] v) : base(v) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public RandomBinarySearchTree(RandomBinarySearchTreeNode<T, TOp> root) : base(root) { }
        public RandomBinarySearchTreeNode<T, TOp>.Enumerator GetEnumerator()
        {
            RandomBinarySearchTreeNode<T, TOp>.GetEnumerator(ref root);
            return new(root);
        }
    }

    namespace Internal
    {
        public class RandomBinarySearchTreeNode<T, TOp>
            : RandomBinarySearchTreeNodeBase<RandomBinarySearchTreeNode<T, TOp>, T>
            , IBbstNode<T, RandomBinarySearchTreeNode<T, TOp>>
            where TOp : struct, ISegtreeOperator<T>
        {
            static TOp op => new();
            public RandomBinarySearchTreeNode(T v)
            {
                Size = 1;
                Sum = Value = v;
            }

            [凾(256)]
            public static RandomBinarySearchTreeNode<T, TOp> Create(T v) => new(v);

            [凾(256)]
            public static void Propagate(ref RandomBinarySearchTreeNode<T, TOp> t)
            {
            }

            [凾(256)]
            public static RandomBinarySearchTreeNode<T, TOp> Update(RandomBinarySearchTreeNode<T, TOp> t)
            {
                if (t == null) return t;
                t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
                t.Sum = op.Operate(op.Operate(GetSum(t.left), t.Value), GetSum(t.right));
                return t;
            }

            [凾(256)]
            static T GetSum(RandomBinarySearchTreeNode<T, TOp> t)
                => t != null ? t.Sum : op.Identity;
            static T IBbstNode<T, RandomBinarySearchTreeNode<T, TOp>>.Sum(RandomBinarySearchTreeNode<T, TOp> t)
                => GetSum(t);
            public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
        }
    }
}