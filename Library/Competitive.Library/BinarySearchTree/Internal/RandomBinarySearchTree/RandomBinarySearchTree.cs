using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        public RandomBinarySearchTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public RandomBinarySearchTree(T[] v) : base(v) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public RandomBinarySearchTree(int root) : base(root) { }
    }

    /// <summary>
    /// 乱択平衡二分探索木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RandomBinarySearchTree<T, TOp> : BinarySearchTreeBase<T, int, RbstNode<T, TOp>.__RbtsOp>
        where TOp : struct, ISegtreeOperator<T>
    {
        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public RandomBinarySearchTree(T[] v) : base(v) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public RandomBinarySearchTree(int root) : base(root) { }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct RbstNode<T, TOp> : IRbstNode<T, int>
        where TOp : struct, ISegtreeOperator<T>
    {
        public struct __RbtsOp : IRbstOp<T, TOp, RbstNode<T, TOp>, int, __RbtsOp, PoolStructRefOp<RbstNode<T, TOp>>>
                , IBbstStructNodeOp<T, RbstNode<T, TOp>, __RbtsOp>
        {
            [凾(256)] public static RbstNode<T, TOp> CreateNode(T v) => new(v);
        }

        public int Parent { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public T Value { get; set; }
        public T Sum { get; set; }
        public int Size { get; set; }

        public RbstNode(T v)
        {
            Parent = Left = Right = -1;
            Size = 1;
            Sum = Value = v;
        }

        [SourceExpander.NotEmbeddingSource]
        public readonly override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
    }
}
