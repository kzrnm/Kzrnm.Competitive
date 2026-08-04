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
    /// <typeparam name="M">平衡二分探索木生成型</typeparam>
    /// <typeparam name="N">ノード操作型</typeparam>
    /// <typeparam name="TSelf">自身の型</typeparam>
    public abstract class ImmutableLazyBinarySearchTreeBase<T, F, TSelf, Nd, M, N> : ImmutableBinarySearchTreeBase<T, TSelf, Nd, M, N>
        where TSelf : ImmutableLazyBinarySearchTreeBase<T, F, TSelf, Nd, M, N>
        where Nd : class, IBbstNode
        where M : IImmutableBbstMaker<TSelf, Nd>
        where N : IBbstNodeOp<T, Nd, N>, ILazyBbstNodeOp<T, F, Nd, N>
    {
        protected ImmutableLazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(N.Build(v)) { }
        protected ImmutableLazyBinarySearchTreeBase(Nd root) : base(root) { }

        [凾(256)]
        public TSelf Apply(int l, int r, F f)
        {
            var t = root;
            N.Apply(ref t, l, r, f);
            return M.Create(t);
        }
        [凾(256)]
        public TSelf Reverse()
        {
            var t = N.Copy(root);
            N.Reverse(t);
            return M.Create(t);
        }
        [凾(256)]
        public TSelf Reverse(int l, int r)
        {
            var t = root;
            N.Reverse(ref t, l, r);
            return M.Create(t);
        }
    }
}