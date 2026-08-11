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
    /// 遅延伝播乱択平衡二分探索木
    /// </summary>
    public class LazyRandomBinarySearchTree<T> : LazyRandomBinarySearchTree<T, byte, SingleBbstOp<T>>
    {
        public LazyRandomBinarySearchTree() { }
        public LazyRandomBinarySearchTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazyRandomBinarySearchTree(T[] v) : base(v.AsSpan()) { }
        public LazyRandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
    }

    /// <summary>
    /// 遅延伝播乱択平衡二分探索木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class LazyRandomBinarySearchTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, int, LazyRbstNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyRandomBinarySearchTree() { }
        public LazyRandomBinarySearchTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazyRandomBinarySearchTree(T[] v) : base(v) { }
        public LazyRandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        protected LazyRandomBinarySearchTree(int root) : base(root) { }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct LazyRbstNode<T, F, TOp> : IRbstNode<T, int>, ILazyBbstNode<T, F, int>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public struct Op : ILazyRbstOp<T, F, TOp, LazyRbstNode<T, F, TOp>, int, Op, PoolStructRefOp<LazyRbstNode<T, F, TOp>>>
                , IBbstStructNodeOp<T, LazyRbstNode<T, F, TOp>, Op>
        {
            [凾(256)] public static LazyRbstNode<T, F, TOp> CreateNode(T v) => new(v);
        }

        static TOp op => new();

        public int Parent { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public T Value { get; set; }
        public T Sum { get; set; }
        public int Size { get; set; }

        public F Lazy { get; set; }
        public bool Reversed { get; set; }
        public LazyRbstNode(T v)
        {
            Parent = Left = Right = -1;
            Size = 1;
            Sum = Value = v;
            Lazy = op.FIdentity;
        }

        [SourceExpander.NotEmbeddingSource]
        public readonly override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}, Lazy = {Lazy}";
    }

    public interface ILazyRbstOp<T, F, TOp, Nd, R, N, C> : IRbstOp<T, TOp, Nd, R, N, C>, ILazyBbstOp<T, F, TOp, Nd, R, N, C>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where Nd : IRbstNode<T, R>, ILazyBbstNode<T, F, R>
        where N : ILazyRbstOp<T, F, TOp, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        [凾(256)]
        static R IBbstOp<R, N>.Propagate(R t)
        {
            if (C.IsNull(t)) return t;
            var op = new TOp();
            ref var d = ref C.Load(t);
            var lazy = !EqualityComparer<F>.Default.Equals(d.Lazy, op.FIdentity);
            var rev = d.Reversed;

            if (lazy)
            {
                d.Value = op.Mapping(d.Lazy, d.Value, 1);
                if (!C.IsNull(d.Left))
                {
                    ref var ln = ref C.Load(d.Left);
                    ln.Lazy = op.Composition(d.Lazy, ln.Lazy);
                    ln.Sum = op.Mapping(d.Lazy, ln.Sum, ln.Size);
                }
                if (!C.IsNull(d.Right))
                {
                    ref var rn = ref C.Load(d.Right);
                    rn.Lazy = op.Composition(d.Lazy, rn.Lazy);
                    rn.Sum = op.Mapping(d.Lazy, rn.Sum, rn.Size);
                }
                d.Lazy = op.FIdentity;
            }
            if (rev)
            {
                N.Reverse(d.Left);
                N.Reverse(d.Right);
                d.Reversed = false;
            }

            return N.Update(t);
        }
    }
}
