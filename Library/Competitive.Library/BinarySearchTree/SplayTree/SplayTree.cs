using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    /// <summary>
    /// Splay 木
    /// </summary>
    public class SplayTree<T> : SplayTree<T, SingleBbstOp<T>>
    {
        public SplayTree() { }
        public SplayTree(IEnumerable<T> v) : base(v) { }
        public SplayTree(T[] v) : base(v) { }
        public SplayTree(ReadOnlySpan<T> v) : base(v) { }
        public SplayTree(SplayTreeNode<T, SingleBbstOp<T>> root) : base(root) { }
    }

    /// <summary>
    /// Splay 木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SplayTree<T, TOp> : BinarySearchTreeBase<T, SplayTreeNode<T, TOp>>
        where TOp : struct, ISegtreeOperator<T>
    {
        public SplayTree() { }
        public SplayTree(IEnumerable<T> v) : base(v) { }
        public SplayTree(T[] v) : base(v) { }
        public SplayTree(ReadOnlySpan<T> v) : base(v) { }
        public SplayTree(SplayTreeNode<T, TOp> root) : base(root) { }
        public SplayTreeNode<T, TOp>.Enumerator GetEnumerator()
        {
            SplayTreeNode<T, TOp>.GetEnumerator(ref root);
            return new(root);
        }
    }

    namespace Internal
    {
        public class SplayTreeNode<T, TOp>
            : SplayTreeNodeBase<SplayTreeNode<T, TOp>, T>
            , IBbstNode<T, SplayTreeNode<T, TOp>>
            , ISplayTreePusher<SplayTreeNode<T, TOp>>
            where TOp : struct, ISegtreeOperator<T>
        {
            static TOp op => new();
            public SplayTreeNode(T v)
            {
                Size = 1;
                Sum = Value = v;
            }

            [凾(256)]
            public static SplayTreeNode<T, TOp> Create(T v) => new(v);


            [凾(256)]
            public static SplayTreeNode<T, TOp> Update(SplayTreeNode<T, TOp> t)
            {
                if (t == null) return t;
                t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
                t.Sum = op.Operate(op.Operate(GetSum(t.left), t.Value), GetSum(t.right));
                return t;
            }

            [凾(256)]
            public static void Push(SplayTreeNode<T, TOp> node) { }
            [凾(256)]
            static T GetSum(SplayTreeNode<T, TOp> t)
                => t != null ? t.Sum : op.Identity;
            static T IBbstNode<T, SplayTreeNode<T, TOp>>.Sum(SplayTreeNode<T, TOp> t)
                => GetSum(t);
            public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
        }
    }
}