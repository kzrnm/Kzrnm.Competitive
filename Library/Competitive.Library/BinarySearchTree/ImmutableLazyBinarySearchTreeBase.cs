using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    /// <summary>
    /// 永続化した反転可能遅延伝播平衡二分探索木を実装する
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="Node">ノード</typeparam>
    /// <typeparam name="TSelf">自身の型</typeparam>
    public abstract class ImmutableLazyBinarySearchTreeBase<T, F, Node, TSelf> : ImmutableBinarySearchTreeBase<T, Node, TSelf>
        where Node : class, ILazyBbstNode<T, F, Node>
        where TSelf : ImmutableLazyBinarySearchTreeBase<T, F, Node, TSelf>, IImmutableBbst<T, Node, TSelf>
    {
        protected ImmutableLazyBinarySearchTreeBase()
        {
        }
        protected ImmutableLazyBinarySearchTreeBase(IEnumerable<T> v) : base(v) { }
        protected ImmutableLazyBinarySearchTreeBase(T[] v) : base(v) { }
        protected ImmutableLazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(v) { }
        protected ImmutableLazyBinarySearchTreeBase(Node root) : base(root) { }

        [凾(256)]
        public TSelf Apply(int l, int r, F f)
        {
            var t = root;
            Node.Apply(ref t, l, r, f);
            return TSelf.Create(t);
        }
        [凾(256)]
        public TSelf Reverse()
        {
            var t = Node.Copy(root);
            Node.Reverse(t);
            return TSelf.Create(t);
        }
        [凾(256)]
        public TSelf Reverse(int l, int r)
        {
            var t = root;
            Node.Reverse(ref t, l, r);
            return TSelf.Create(t);
        }
    }
}