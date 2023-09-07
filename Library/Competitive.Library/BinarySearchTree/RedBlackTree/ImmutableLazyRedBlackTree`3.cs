using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 永続遅延伝搬反転可能赤黒木
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp

    /// <summary>
    /// 永続遅延伝搬反転可能赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ImmutableLazyRedBlackTree<T, F, TOp> : IImmutableLazyBinarySearchTree<T, F, ImmutableLazyRedBlackTree<T, F, TOp>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public static LazyBinarySearchTreeNodeOperator<T, F, TOp, LazyRedBlackTreeNode<T, F>, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> rb => default;

        public LazyRedBlackTreeNode<T, F> root;

        public static ImmutableLazyRedBlackTree<T, F, TOp> Empty { get; } = new ImmutableLazyRedBlackTree<T, F, TOp>();
        protected ImmutableLazyRedBlackTree() { }

        public ImmutableLazyRedBlackTree(LazyRedBlackTreeNode<T, F> root) { this.root = root; }
        public ImmutableLazyRedBlackTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public ImmutableLazyRedBlackTree(T[] v) : this(v.AsSpan()) { }
        public ImmutableLazyRedBlackTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }

        [凾(256)] public T Prod(int l, int r) => rb.Prod(ref root, l, r);
        [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);
        public T AllProd => rb.Sum(root);
        public int Count => rb.Size(root);
        public T this[int index] => rb.im.GetValue(ref root, index);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Add(T item)
        {
            var t = root;
            rb.AddLast(ref t, item);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> AddRange(IEnumerable<T> items)
        {
            var t = rb.im.Merge(root, rb.im.Build(items.ToArray()));
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Clear() => Empty;

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Insert(int index, T item)
        {
            var t = root;
            rb.Insert(ref t, index, item);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> InsertRange(int index, IEnumerable<T> items)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(rb.Merge3(t1, rb.im.Build(items.ToArray()), t2));
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> AddRange(ImmutableLazyRedBlackTree<T, F, TOp> other)
        {
            var t = rb.im.Merge(root, other.root);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> InsertRange(int index, ImmutableLazyRedBlackTree<T, F, TOp> other)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            rb.Merge3(t1, other.root, t2);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> RemoveAt(int index)
        {
            var t = root;
            rb.Erase(ref t, index);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> RemoveRange(int index, int count)
        {
            var t = root;
            var (t1, _, t3) = rb.Split3(t, index, index + count);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(rb.im.Merge(t1, t3));
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> SetItem(int index, T value)
        {
            var t = root;
            rb.im.SetValue(ref t, index, value);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Apply(int l, int r, F f)
        {
            var t = root;
            rb.Apply(ref t, l, r, f);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Reverse()
        {
            var t = root;
            rb.Reverse(ref t, 0, Count);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Reverse(int l, int r)
        {
            var t = root;
            rb.Reverse(ref t, l, r);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public LazyRedBlackTreeEnumerator<T, F, TOp, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> GetEnumerator()
            => rb.im.GetEnumerator(root);
        [凾(256)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        [凾(256)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct TCp : ICopyOperator<LazyRedBlackTreeNode<T, F>>
        {
            [凾(256)]
            public LazyRedBlackTreeNode<T, F> Copy(LazyRedBlackTreeNode<T, F> t) => new LazyRedBlackTreeNode<T, F>(t.Key, t.Lazy)
            {
                Level = t.Level,
                Color = t.Color,
                Left = t.Left,
                Right = t.Right,
                Sum = t.Sum,
                Size = t.Size,
                IsReverse = t.IsReverse,
            };
        }
    }
}
