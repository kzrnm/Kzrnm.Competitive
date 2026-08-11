using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        public RedBlackTree(int root) : base(root) { }
    }

    /// <summary>
    /// 赤黒木
    /// </summary>
    public class RedBlackTree<T, TOp> : BinarySearchTreeBase<T, int, RedBlackTreeNode<T, TOp>.Op>
        where TOp : struct, ISegtreeOperator<T>
    {
        public RedBlackTree() { }
        public RedBlackTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public RedBlackTree(T[] v) : base(v) { }
        public RedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public RedBlackTree(int root) : base(root) { }
    }

    namespace Internal
    {
        [StructLayout(LayoutKind.Auto)]
        public struct RedBlackTreeNode<T, TOp> : IRbtNode<T, int>
            where TOp : struct, ISegtreeOperator<T>
        {
            public RedBlackTreeNode(RedBlackTreeNode<T, TOp> other)
            {
                Left = other.Left;
                Right = other.Right;
                IsBlack = other.IsBlack;
                Level = other.Level;
                Size = other.Size;
                Sum = other.Sum;
            }

            public RedBlackTreeNode(T v)
            {
                Left = Right = -1;
                IsBlack = true;
                Size = 1;
                Sum = v;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Left { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Right { get; set; }
            public bool IsBlack { get; set; }
            public int Level { get; set; }
            public T Sum { get; set; }
            public int Size { get; set; }

            [SourceExpander.NotEmbeddingSource]
            public readonly override string ToString() => ((IRbtNode<T, int>)this).ToStringImpl();

            [SourceExpander.NotEmbeddingSource]
            readonly object DebugLeft => BbstNodeConv.Load(new Op(), Left);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugRight => BbstNodeConv.Load(new Op(), Right);

            public struct Op : IRbtOp<T, TOp, RedBlackTreeNode<T, TOp>, int, Op, PoolStructRefOp<RedBlackTreeNode<T, TOp>>>
                , IBbstStructNodeOp<T, RedBlackTreeNode<T, TOp>, Op>
            {
                [凾(256)] public static RedBlackTreeNode<T, TOp> CreateNode(T v) => new(v);
            }
        }
    }
}