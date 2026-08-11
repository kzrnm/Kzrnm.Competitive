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
    public class LazySplayTree<T> : LazySplayTree<T, byte, SingleBbstOp<T>>
    {
        public LazySplayTree() { }
        public LazySplayTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazySplayTree(T[] v) : base(v) { }
        public LazySplayTree(ReadOnlySpan<T> v) : base(v) { }
    }

    /// <summary>
    /// 遅延伝播反転可能 Splay 木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class LazySplayTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, int, LazySplayTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazySplayTree() { }
        public LazySplayTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazySplayTree(T[] v) : base(v) { }
        public LazySplayTree(ReadOnlySpan<T> v) : base(v) { }
        protected LazySplayTree(int root) : base(root) { }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct LazySplayTreeNode<T, F, TOp> : ISplayTreeNode<T, int>, ILazyBbstNode<T, F, int>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public struct Op : ILazySplayOp<T, F, TOp, LazySplayTreeNode<T, F, TOp>, int, Op, PoolStructRefOp<LazySplayTreeNode<T, F, TOp>>>, ILazyBbstOp<T, F, int, Op>
            , IBbstStructNodeOp<T, LazySplayTreeNode<T, F, TOp>, Op>
        {
            [凾(256)] public static LazySplayTreeNode<T, F, TOp> CreateNode(T v) => new(v);
        }

        public int Parent { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public T Value { get; set; }
        public T Sum { get; set; }
        public int Size { get; set; }

        public F Lazy { get; set; }
        public bool Reversed { get; set; }
        public LazySplayTreeNode(T v)
        {
            Parent = Left = Right = -1;
            Size = 1;
            Sum = Value = v;
            Lazy = new TOp().FIdentity;
        }

        [SourceExpander.NotEmbeddingSource]
        public readonly override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
    }


    public interface ILazySplayOp<T, F, TOp, Nd, R, N, C> : ISplayTreePusher<T, Nd, R, N, C>, ILazyBbstOp<T, F, R, N>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where Nd : ISplayTreeNode<T, R>, ILazyBbstNode<T, F, R>
        where N : ILazySplayOp<T, F, TOp, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        [凾(256)]
        static void ISplayTreePusher<T, Nd, R, N, C>.Push(R t)
        {
            ref var d = ref C.Load(t);
            if (!EqualityComparer<F>.Default.Equals(d.Lazy, new TOp().FIdentity))
            {
                ApplyImpl(d.Left, d.Lazy);
                ApplyImpl(d.Right, d.Lazy);
                d.Lazy = new TOp().FIdentity;
            }
            if (d.Reversed)
            {
                N.Reverse(d.Left);
                N.Reverse(d.Right);
                d.Reversed = false;
            }
        }

        [凾(256)]
        static T ISplayTreePusher<T, Nd, R, N, C>.Prod(T x, T y) => new TOp().Operate(x, y);

        [凾(256)]
        static T IBbstOp<T, R, N>.Sum(R t)
            => C.IsNull(t) ? new TOp().Identity : C.Load(t).Sum;

        [凾(256)]
        static void ApplyImpl(R t, F f)
        {
            if (!C.IsNull(t))
            {
                ref var dn = ref C.Load(t);
                dn.Lazy = new TOp().Composition(f, dn.Lazy);
                dn.Value = new TOp().Mapping(f, dn.Value, 1);
                dn.Sum = new TOp().Mapping(f, dn.Sum, dn.Size);
            }
        }

        [凾(256)]
        static R ILazyBbstOp<T, F, R, N>.Apply(R t, F f)
        {
            if (!C.IsNull(t))
            {
                Splay(t);
                ApplyImpl(t, f);
                N.Push(t);
            }
            return t;
        }

        [凾(256)]
        static R ILazyBbstOp<T, F, R, N>.Reverse(R t)
        {
            if (!C.IsNull(t))
            {
                ref var dn = ref C.Load(t);
                (dn.Left, dn.Right) = (dn.Right, dn.Left);
                dn.Sum = new TOp().Inverse(dn.Sum);
                dn.Reversed = !dn.Reversed;
            }
            return t;
        }
    }
}
