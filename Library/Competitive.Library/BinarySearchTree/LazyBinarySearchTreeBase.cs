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
    /// <typeparam name="Nd">ノード</typeparam>
    /// <typeparam name="N">ノード操作型</typeparam>
    public abstract class LazyBinarySearchTreeBase<T, F, Nd, N> : BinarySearchTreeBase<T, Nd, N>
        where Nd : class, IBbstNode
        where N : ILazyBbstNodeOp<T, F, Nd, N>
    {
        protected LazyBinarySearchTreeBase()
        {
        }
        protected LazyBinarySearchTreeBase(IEnumerable<T> v) : base(v) { }
        protected LazyBinarySearchTreeBase(T[] v) : base(v) { }
        protected LazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(v) { }
        protected LazyBinarySearchTreeBase(Nd root) : base(root) { }

        [凾(256)] public void Apply(int l, int r, F f) => N.Apply(ref root, l, r, f);
        [凾(256)] public void Reverse() => N.Reverse(root);
        [凾(256)] public void Reverse(int l, int r) => N.Reverse(ref root, l, r);
    }

    /// <summary>
    /// 反転可能遅延伝播平衡二分探索木のノード
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="Nd">ノード</typeparam>
    /// <typeparam name="N">自身の型</typeparam>
    [IsOperator]
    public interface ILazyBbstNodeOp<T, F, Nd, N> : IBbstNodeOp<T, Nd, N>
        where Nd : class, IBbstNode
        where N : ILazyBbstNodeOp<T, F, Nd, N>
    {
        static abstract Nd Apply(Nd t, F f);
        [凾(256)]
        static virtual void Apply(ref Nd t, int l, int r, F f)
        {
            if (l >= r) return;
            var (x, y1, y2) = N.Split(t, l, r);
            y1 = N.Apply(N.Copy(y1), f);
            t = N.Merge(x, y1, y2);
        }
        static abstract Nd Reverse(Nd t);
        [凾(256)]
        static virtual void Reverse(ref Nd t, int l, int r)
        {
            if (l >= r) return;
            var (x, y1, y2) = N.Split(t, l, r);
            y1 = N.Reverse(N.Copy(y1));
            t = N.Merge(x, y1, y2);
        }
    }
}