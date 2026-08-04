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
    public class RedBlackTree<T, TOp> : BinarySearchTreeBase<T, RedBlackTreeNode<T, TOp>, RedBlackTreeNode<T, TOp>.Op>
        where TOp : struct, ISegtreeOperator<T>
    {
        public RedBlackTree() { }
        public RedBlackTree(IEnumerable<T> v) : base(v) { }
        public RedBlackTree(T[] v) : base(v) { }
        public RedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public RedBlackTree(RedBlackTreeNode<T, TOp> root) : base(root) { }
    }

    namespace Internal
    {
        public class RedBlackTreeNode<T, TOp> : RedBlackTreeNodeBase<RedBlackTreeNode<T, TOp>, T>
            where TOp : struct, ISegtreeOperator<T>
        {
            public RedBlackTreeNode(T v) : base(v) { }
            public RedBlackTreeNode(RedBlackTreeNode<T, TOp> left, RedBlackTreeNode<T, TOp> right) 
                : base(left, right, new TOp().Operate(left != null ? left.Sum : new TOp().Identity, right != null ? right.Sum : new TOp().Identity)) { }

            public struct Op : IRbtNodeOp<T, TOp, RedBlackTreeNode<T, TOp>, Op>
            {
                [凾(256)]
                public static RedBlackTreeNode<T, TOp> Create(RedBlackTreeNode<T, TOp> left, RedBlackTreeNode<T, TOp> right) => new(left, right);
                [凾(256)]
                public static RedBlackTreeNode<T, TOp> Create(T v) => new(v);
            }
        }

        public interface IRbtNodeOp<T, TOp, Nd, N> : IRbtNodeOp<T, Nd, N>
            where TOp : struct, ISegtreeOperator<T>
            where Nd : RedBlackTreeNodeBase<Nd, T>
            where N : IRbtNodeOp<T, TOp, Nd, N>
        {
            [凾(256)]
            static void IBbstNodeOp<Nd, N>.Propagate(ref Nd t) => t = N.Copy(t);

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