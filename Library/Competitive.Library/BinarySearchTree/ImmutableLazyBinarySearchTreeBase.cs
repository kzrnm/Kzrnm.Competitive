using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    /// <summary>
    /// 永続化した反転可能遅延伝播平衡二分探索木を実装する
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="R">ノード参照</typeparam>
    /// <typeparam name="M">平衡二分探索木生成型</typeparam>
    /// <typeparam name="N">ノード操作型</typeparam>
    /// <typeparam name="TSelf">自身の型</typeparam>
    public abstract class ImmutableLazyBinarySearchTreeBase<T, F, TSelf, R, M, N> : ImmutableBinarySearchTreeBase<T, TSelf, R, M, N>
        where TSelf : ImmutableLazyBinarySearchTreeBase<T, F, TSelf, R, M, N>
        where M : IImmutableBbstMaker<TSelf, R>
        where N : IBbstOp<T, R, N>, ILazyBbstOp<T, F, R, N>
    {
        protected ImmutableLazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(N.Build(v)) { }
        protected ImmutableLazyBinarySearchTreeBase(R root) : base(root) { }

        [凾(256)]
        public TSelf Apply(int l, int r, F f) => M.Create(N.Apply(root, l, r, f));
        [凾(256)]
        public TSelf Reverse()
        {
            var t = N.Copy(root);
            N.Reverse(t);
            return M.Create(t);
        }
        [凾(256)]
        public TSelf Reverse(int l, int r) => M.Create(N.Reverse(root, l, r));
    }
}