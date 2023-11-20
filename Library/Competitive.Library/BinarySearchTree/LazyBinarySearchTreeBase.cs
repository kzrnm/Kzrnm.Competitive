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
    /// <typeparam name="Node">ノード</typeparam>
    public abstract class LazyBinarySearchTreeBase<T, F, Node> : BinarySearchTreeBase<T, Node>
        where Node : class, ILazyBbstNode<T, F, Node>
    {
        protected LazyBinarySearchTreeBase()
        {
        }
        protected LazyBinarySearchTreeBase(IEnumerable<T> v) : base(v) { }
        protected LazyBinarySearchTreeBase(T[] v) : base(v) { }
        protected LazyBinarySearchTreeBase(ReadOnlySpan<T> v) : base(v) { }
        protected LazyBinarySearchTreeBase(Node root) : base(root) { }

        [凾(256)] public void Apply(int l, int r, F f) => Node.Apply(ref root, l, r, f);
        [凾(256)] public void Reverse() => Node.Reverse(root);
        [凾(256)] public void Reverse(int l, int r) => Node.Reverse(ref root, l, r);
    }

    /// <summary>
    /// 反転可能遅延伝播平衡二分探索木のノード
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">モノイドへの作用素</typeparam>
    /// <typeparam name="Node">ノード</typeparam>
    public interface ILazyBbstNode<T, F, Node> : IBbstNode<T, Node> where Node : class, ILazyBbstNode<T, F, Node>
    {
        static abstract void Apply(Node t, F f);
        [凾(256)]
        static virtual void Apply(ref Node t, int l, int r, F f)
        {
            if (l >= r) return;
            var (x, y1, y2) = Node.Split(t, l, r);
            y1 = Node.Copy(y1);
            Node.Apply(y1, f);
            t = Node.Merge(x, y1, y2);
        }
        static abstract void Reverse(Node t);
        [凾(256)]
        static virtual void Reverse(ref Node t, int l, int r)
        {
            if (l >= r) return;
            var (x, y1, y2) = Node.Split(t, l, r);
            y1 = Node.Copy(y1);
            Node.Reverse(y1);
            t = Node.Merge(x, y1, y2);
        }
    }
}