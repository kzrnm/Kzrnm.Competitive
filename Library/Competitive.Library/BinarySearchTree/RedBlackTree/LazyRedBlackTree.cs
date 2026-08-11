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
    /// 遅延伝播反転可能赤黒木
    /// </summary>
    public class LazyRedBlackTree<T> : LazyRedBlackTree<T, byte, SingleBbstOp<T>>
    {
        public LazyRedBlackTree() { }
        public LazyRedBlackTree(IEnumerable<T> v) : base(v) { }
        public LazyRedBlackTree(T[] v) : base(v) { }
        public LazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyRedBlackTree(int root) : base(root) { }
    }

    /// <summary>
    /// 遅延伝播反転可能赤黒木
    /// </summary>
    public class LazyRedBlackTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, int, LazyRedBlackTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyRedBlackTree() { }
        public LazyRedBlackTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazyRedBlackTree(T[] v) : base(v) { }
        public LazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyRedBlackTree(int root) : base(root) { }
    }

    namespace Internal
    {
        [StructLayout(LayoutKind.Auto)]
        public struct LazyRedBlackTreeNode<T, F, TOp> : IRbtNode<T, int>, ILazyBbstNode<T, F, int>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        {
            public LazyRedBlackTreeNode(LazyRedBlackTreeNode<T, F, TOp> other)
            {
                Left = other.Left;
                Right = other.Right;
                IsBlack = other.IsBlack;
                Level = other.Level;
                Size = other.Size;
                Sum = other.Sum;
                Reversed = other.Reversed;
                Lazy = other.Lazy;
            }

            public LazyRedBlackTreeNode(T v)
            {
                Left = Right = -1;
                IsBlack = true;
                Size = 1;
                Sum = v;
                Lazy = new TOp().FIdentity;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Left { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Right { get; set; }
            public bool IsBlack { get; set; }
            public int Level { get; set; }
            public T Sum { get; set; }
            public int Size { get; set; }

            public F Lazy { get; set; }
            public bool Reversed { get; set; }

            [SourceExpander.NotEmbeddingSource]
            public readonly override string ToString() => $"Lazy = {Lazy}{(Reversed ? "!" : "")} {((IRbtNode<T, int>)this).ToStringImpl()}";

            [SourceExpander.NotEmbeddingSource]
            readonly object DebugLeft => BbstNodeConv.Load(new Op(), Left);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugRight => BbstNodeConv.Load(new Op(), Right);

            public struct Op : ILazyRbtOp<T, F, TOp, LazyRedBlackTreeNode<T, F, TOp>, int, Op, PoolStructRefOp<LazyRedBlackTreeNode<T, F, TOp>>>
                , IBbstStructNodeOp<T, LazyRedBlackTreeNode<T, F, TOp>, Op>
            {
                [凾(256)] public static LazyRedBlackTreeNode<T, F, TOp> CreateNode(T v) => new(v);
            }
        }

        public interface ILazyRbtOp<T, F, TOp, Nd, R, N, C> : IRbtOp<T, TOp, Nd, R, N, C>, ILazyBbstOp<T, F, TOp, Nd, R, N, C>
            where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
            where Nd : IRbtNode<T, R>, ILazyBbstNode<T, F, R>
            where N : ILazyRbtOp<T, F, TOp, Nd, R, N, C>
            where C : IPoolRefOp<Nd, R>
        {
            [凾(256)]
            static R ILazyBbstOp<T, F, R, N>.Apply(R t, int l, int r, F f)
            {
                if (l >= r)
                    return t;
                Debug.Assert(!C.IsNull(t));

                t = N.Propagate(t);
                ref Nd d = ref C.Load(t);
                if (l == 0 && d.Size <= r)
                {
                    if (IsLeaf(t))
                    {
                        d.Sum = new TOp().Mapping(f, d.Sum, 1);
                        d.Lazy = new TOp().FIdentity;
                    }
                    else
                    {
                        d.Sum = new TOp().Mapping(f, d.Sum, d.Size);
                        d.Lazy = f;
                    }
                    return t;
                }

                Debug.Assert(!IsLeaf(t));

                var lc = N.Size(d.Left);

                var ll = d.Left;
                var rr = d.Right;
                try
                {
                    if (lc <= l)
                        rr = N.Apply(rr, l - lc, r - lc, f);
                    else if (r <= lc)
                        ll = N.Apply(ll, l, r, f);
                    else
                    {
                        ll = N.Apply(ll, l, lc, f);
                        rr = N.Apply(rr, 0, r - lc, f);
                    }
                }
                finally
                {
                    d = ref C.Load(t);
                    d.Left = ll;
                    d.Right = rr;
                    N.Update(t);
                }
                return t;
            }


            [凾(256)]
            static R IBbstOp<R, N>.Propagate(R t)
            {
                t = N.Copy(t);
                if (C.IsNull(t) || IsLeaf(t)) return t;

                ref Nd d = ref C.Load(t);
                var op = new TOp();
                var lazy = !EqualityComparer<F>.Default.Equals(d.Lazy, op.FIdentity);
                var rev = d.Reversed;

                if (lazy || rev)
                {
                    var ll = N.Copy(d.Left);
                    var rr = N.Copy(d.Right);
                    d = ref C.Load(t);
                    d.Left = ll;
                    d.Right = rr;

                    if (lazy)
                    {
                        if (!C.IsNull(ll))
                        {
                            ref var ln = ref C.Load(ll);
                            ln.Lazy = op.Composition(d.Lazy, ln.Lazy);
                            ln.Sum = op.Mapping(d.Lazy, ln.Sum, ln.Size);
                        }
                        if (!C.IsNull(rr))
                        {
                            ref var rn = ref C.Load(rr);
                            rn.Lazy = op.Composition(d.Lazy, rn.Lazy);
                            rn.Sum = op.Mapping(d.Lazy, rn.Sum, rn.Size);
                        }
                        C.Load(t).Lazy = op.FIdentity;
                    }
                    if (rev)
                    {
                        N.Reverse(ll);
                        N.Reverse(rr);
                    }
                    C.Load(t).Reversed = false;
                    N.Update(t);
                }
                return t;
            }
        }
    }
}