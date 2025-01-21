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
    public abstract class LazyBinarySearchTreeBase<T, F, Nd> : BinarySearchTreeBase<T, Nd>
        where Nd : class, ILazyBbstNode<T, F, Nd>
    {
        protected LazyBinarySearchTreeBase()
        {
        }
        protected LazyBinarySearchTreeBase(IEnumerable<T> v) : base(v) { }
        protected LazyBinarySearchTreeBase(T[] v) : base(v) { }
        protected LazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(v) { }
        protected LazyBinarySearchTreeBase(Nd root) : base(root) { }

        [凾(256)] public void Apply(int l, int r, F f) => Nd.Apply(ref root, l, r, f);
        [凾(256)] public void Reverse() => Nd.Reverse(root);
        [凾(256)] public void Reverse(int l, int r) => Nd.Reverse(ref root, l, r);
    }

    /// <summary>
    /// 反転可能遅延伝播平衡二分探索木のノード
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="Nd">ノード</typeparam>
    public interface ILazyBbstNode<T, F, Nd> : IBbstNode<T, Nd> where Nd : class, ILazyBbstNode<T, F, Nd>
    {
        static abstract void Apply(Nd t, F f);
        [凾(256)]
        static virtual void Apply(ref Nd t, int l, int r, F f)
        {
            if (l >= r) return;
            var (x, y1, y2) = Nd.Split(t, l, r);
            y1 = Nd.Copy(y1);
            Nd.Apply(y1, f);
            t = Nd.Merge(x, y1, y2);
        }
        static abstract void Reverse(Nd t);
        [凾(256)]
        static virtual void Reverse(ref Nd t, int l, int r)
        {
            if (l >= r) return;
            var (x, y1, y2) = Nd.Split(t, l, r);
            y1 = Nd.Copy(y1);
            Nd.Reverse(y1);
            t = Nd.Merge(x, y1, y2);
        }
    }
}