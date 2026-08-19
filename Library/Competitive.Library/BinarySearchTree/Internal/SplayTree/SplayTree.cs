using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
    }

    /// <summary>
    /// Splay 木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SplayTree<T, TOp> : BinarySearchTreeBase<T, int, SplayTreeNode<T, TOp>.__SpltOp>
        where TOp : struct, ISegtreeOperator<T>
    {
        public SplayTree() { }
        public SplayTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public SplayTree(T[] v) : base(v) { }
        public SplayTree(ReadOnlySpan<T> v) : base(v) { }
        protected SplayTree(int root) : base(root) { }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct SplayTreeNode<T, TOp> : ISplayTreeNode<T, int>
        where TOp : struct, ISegtreeOperator<T>
    {
        public struct __SpltOp : ISplayTreePusher<T, SplayTreeNode<T, TOp>, int, __SpltOp, PoolStructRefOp<SplayTreeNode<T, TOp>>>
                , IBbstStructNodeOp<T, SplayTreeNode<T, TOp>, __SpltOp>
        {
            [凾(256)] public static SplayTreeNode<T, TOp> CreateNode(T v) => new(v);

            [凾(256)]
            public static T Prod(T x, T y) => op.Operate(x, y);

            [凾(256)]
            public static T Sum(int t)
                => t < 0 ? op.Identity : StructPool<SplayTreeNode<T, TOp>>.Default.Get(t).Sum;
        }

        static TOp op => new();

        public int Parent { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public T Value { get; set; }
        public T Sum { get; set; }
        public int Size { get; set; }

        public SplayTreeNode(T v)
        {
            Parent = Left = Right = -1;
            Size = 1;
            Sum = Value = v;
        }

        [SourceExpander.NotEmbeddingSource]
        [凾(256)]
        public readonly override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
    }
}
