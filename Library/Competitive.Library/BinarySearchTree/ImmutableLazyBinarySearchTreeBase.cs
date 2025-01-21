using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    /// <summary>
    /// 永続化した反転可能遅延伝播平衡二分探索木を実装する
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="Nd">ノード</typeparam>
    /// <typeparam name="TSelf">自身の型</typeparam>
    public abstract class ImmutableLazyBinarySearchTreeBase<T, F, Nd, TSelf> : ImmutableBinarySearchTreeBase<T, Nd, TSelf>
        where Nd : class, ILazyBbstNode<T, F, Nd>
        where TSelf : ImmutableLazyBinarySearchTreeBase<T, F, Nd, TSelf>, IImmutableBbst<T, Nd, TSelf>
    {
        protected ImmutableLazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(Nd.Build(v)) { }
        protected ImmutableLazyBinarySearchTreeBase(Nd root) : base(root) { }

        [凾(256)]
        public TSelf Apply(int l, int r, F f)
        {
            var t = root;
            Nd.Apply(ref t, l, r, f);
            return TSelf.Create(t);
        }
        [凾(256)]
        public TSelf Reverse()
        {
            var t = Nd.Copy(root);
            Nd.Reverse(t);
            return TSelf.Create(t);
        }
        [凾(256)]
        public TSelf Reverse(int l, int r)
        {
            var t = root;
            Nd.Reverse(ref t, l, r);
            return TSelf.Create(t);
        }
    }
}