using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// AVL木
    /// </summary>
    public class AvlTree<T> : AvlTree<T, SingleBbstOp<T>>
    {
        public AvlTree() { }
        public AvlTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public AvlTree(T[] v) : base(v) { }
        public AvlTree(ReadOnlySpan<T> v) : base(v) { }
        public AvlTree(int root) : base(root) { }
    }

    /// <summary>
    /// AVL木
    /// </summary>
    public class AvlTree<T, TOp> : BinarySearchTreeBase<T, int, AvlTreeNode<T, TOp>.Op>
        where TOp : struct, ISegtreeOperator<T>
    {
        public AvlTree() { }
        public AvlTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public AvlTree(T[] v) : base(v) { }
        public AvlTree(ReadOnlySpan<T> v) : base(v) { }
        public AvlTree(int root) : base(root) { }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct AvlTreeNode<T, TOp> : IAvlNode<T, int>
        where TOp : struct, ISegtreeOperator<T>
    {
        public struct Op : IAvlOp<T, TOp, AvlTreeNode<T, TOp>, int, Op, PoolStructRefOp<AvlTreeNode<T, TOp>>>
                , IBbstStructNodeOp<T, AvlTreeNode<T, TOp>, Op>
        {
            [凾(256)] public static AvlTreeNode<T, TOp> CreateNode(T v) => new(v);
        }

        public int Left { get; set; }
        public int Right { get; set; }
        public T Value { get; set; }
        public T Sum { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }

        public AvlTreeNode(T v)
        {
            Left = Right = -1;
            Height = 1;
            Size = 1;
            Sum = Value = v;
        }

        [SourceExpander.NotEmbeddingSource]
        public readonly override string ToString() => $"Size = {Size} Value = {Value} Sum = {Sum}";
    }
}
