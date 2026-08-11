using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    /// <summary>
    /// 反転可能遅延伝播平衡二分探索木を実装する
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="R">ノード参照</typeparam>
    /// <typeparam name="N">ノード操作型</typeparam>
    public abstract class LazyBinarySearchTreeBase<T, F, R, N> : BinarySearchTreeBase<T, R, N>
        where N : ILazyBbstOp<T, F, R, N>
    {
        protected LazyBinarySearchTreeBase() { }
        protected LazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(v) { }
        protected LazyBinarySearchTreeBase(R root) : base(root) { }

        [凾(256)] public void Apply(int l, int r, F f) => root = N.Apply(root, l, r, f);
        [凾(256)] public void Reverse() => N.Reverse(root);
        [凾(256)] public void Reverse(int l, int r) => root = N.Reverse(root, l, r);
    }

    public interface ILazyBbstNode<T, F, R> : IBbstNode<T, R>
    {
        F Lazy { get; set; }
        bool Reversed { get; set; }
    }

    /// <summary>
    /// 反転可能遅延伝播平衡二分探索木のノード
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="TOp">モノイド操作型</typeparam>
    /// <typeparam name="Nd">ノード</typeparam>
    /// <typeparam name="R">ノード参照</typeparam>
    /// <typeparam name="N">自身の型</typeparam>
    /// <typeparam name="C">ノード参照からノードを取得</typeparam>
    [IsOperator]
    public interface ILazyBbstOp<T, F, TOp, Nd, R, N, C> : ILazyBbstOp<T, F, R, N>, IBbstCnv<Nd, R, N, C>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where Nd : IBbstNode<T, R>, ILazyBbstNode<T, F, R>
        where N : ILazyBbstOp<T, F, TOp, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        [凾(256)]
        static R ILazyBbstOp<T, F, R, N>.Apply(R t, F f)
        {
            ref Nd d = ref C.Load(t);
            d.Lazy = new TOp().Composition(f, d.Lazy);
            return N.Propagate(t);
        }

        [凾(256)]
        static R ILazyBbstOp<T, F, R, N>.Reverse(R t)
        {
            if (!C.IsNull(t))
            {
                ref Nd d = ref C.Load(t);
                (d.Left, d.Right) = (d.Right, d.Left);
                d.Sum = new TOp().Inverse(d.Sum);
                d.Reversed = !d.Reversed;
            }
            return t;
        }
    }

    /// <summary>
    /// 反転可能遅延伝播平衡二分探索木のノード
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="R">ノード参照</typeparam>
    /// <typeparam name="N">自身の型</typeparam>
    [IsOperator]
    public interface ILazyBbstOp<T, F, R, N> : IBbstOp<T, R, N>
        where N : ILazyBbstOp<T, F, R, N>
    {
        static abstract R Apply(R t, F f);
        [凾(256)]
        static virtual R Apply(R t, int l, int r, F f)
        {
            if (l >= r) return t;
            var (x, y1, y2) = N.Split(t, l, r);
            y1 = N.Apply(N.Copy(y1), f);
            return N.Merge(x, y1, y2);
        }

        static abstract R Reverse(R t);

        [凾(256)]
        static virtual R Reverse(R t, int l, int r)
        {
            if (l >= r) return t;
            var (x, y1, y2) = N.Split(t, l, r);
            y1 = N.Reverse(N.Copy(y1));
            return N.Merge(x, y1, y2);
        }
    }
}