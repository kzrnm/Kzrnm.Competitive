using AtCoder;
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
    public class SplayTree<T, TOp> : BinarySearchTreeBase<T, SplayTreeNode<T, TOp>, SplayTreeNode<T, TOp>.Op>
        where TOp : struct, ISegtreeOperator<T>
    {
        public SplayTree() { }
        public SplayTree(IEnumerable<T> v) : base(v) { }
        public SplayTree(T[] v) : base(v) { }
        public SplayTree(ReadOnlySpan<T> v) : base(v) { }
        public SplayTree(SplayTreeNode<T, TOp> root) : base(root) { }
    }
    public class SplayTreeNode<T, TOp> : SplayTreeNodeBase<SplayTreeNode<T, TOp>, T>
        where TOp : struct, ISegtreeOperator<T>
    {
        public struct Op : ISplayTreePusher<T, SplayTreeNode<T, TOp>, Op>
        {
            [凾(256)]
            public static SplayTreeNode<T, TOp> Create(T v) => new(v);

            [凾(256)]
            public static T Operate(T x, T y) => op.Operate(x, y);

            [凾(256)]
            public static void Push(SplayTreeNode<T, TOp> t)
            {
            }

            [凾(256)]
            public static T Sum(SplayTreeNode<T, TOp> t)
                => t != null ? t.Sum : op.Identity;
        }

        static TOp op => new();
        public SplayTreeNode(T v)
        {
            Size = 1;
            Sum = Value = v;
        }

        [SourceExpander.NotEmbeddingSource]
        [凾(256)]
        public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
    }
}
