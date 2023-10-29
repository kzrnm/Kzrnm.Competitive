using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 永続反転可能赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ImmutableLazyRedBlackTree<T> : IImmutableLazyBinarySearchTree<T, T, ImmutableLazyRedBlackTree<T>>
    {
        public static LazyBinarySearchTreeNodeOperator<T, T, SingleBbstOp<T>, LazyRedBlackTreeNode<T, T>, LazyRedBlackTreeNodeOperator<T, T, SingleBbstOp<T>, TCp>> rb => default;

        public LazyRedBlackTreeNode<T, T> root;

        public static ImmutableLazyRedBlackTree<T> Empty { get; } = new ImmutableLazyRedBlackTree<T>();
        protected ImmutableLazyRedBlackTree() { }

        public ImmutableLazyRedBlackTree(LazyRedBlackTreeNode<T, T> root) { this.root = root; }
        public ImmutableLazyRedBlackTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public ImmutableLazyRedBlackTree(T[] v) : this(v.AsSpan()) { }
        public ImmutableLazyRedBlackTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }
        public T AllProd => rb.Sum(root);
        public int Count => rb.Size(root);
        public T this[int index] => rb.im.GetValue(ref root, index);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> Add(T item)
        {
            var t = root;
            rb.AddLast(ref t, item);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> AddRange(IEnumerable<T> items)
        {
            var t = rb.im.Merge(root, rb.im.Build(items.ToArray()));
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> Clear() => Empty;

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> Insert(int index, T item)
        {
            var t = root;
            rb.Insert(ref t, index, item);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> InsertRange(int index, IEnumerable<T> items)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            t = rb.Merge3(t1, rb.im.Build(items.ToArray()), t2);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> AddRange(ImmutableLazyRedBlackTree<T> other)
        {
            var t = rb.im.Merge(root, other.root);
            return new ImmutableLazyRedBlackTree<T>(t);
        }
        [凾(256)]
        public ImmutableLazyRedBlackTree<T> InsertRange(int index, ImmutableLazyRedBlackTree<T> other)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            rb.Merge3(t1, other.root, t2);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> RemoveAt(int index)
        {
            var t = root;
            rb.Erase(ref t, index);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> RemoveRange(int index, int count)
        {
            var t = root;
            var (t1, _, t3) = rb.Split3(t, index, index + count);
            return new ImmutableLazyRedBlackTree<T>(rb.im.Merge(t1, t3));
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> SetItem(int index, T value)
        {
            var t = root;
            rb.im.SetValue(ref t, index, value);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> Reverse()
        {
            var t = root;
            rb.Reverse(ref t, 0, Count);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T> Reverse(int l, int r)
        {
            var t = root;
            rb.Reverse(ref t, l, r);
            return new ImmutableLazyRedBlackTree<T>(t);
        }

        [凾(256)]
        public LazyRedBlackTreeEnumerator<T, T, SingleBbstOp<T>, LazyRedBlackTreeNodeOperator<T, T, SingleBbstOp<T>, TCp>> GetEnumerator()
            => rb.im.GetEnumerator(root);
        [凾(256)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        [凾(256)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        ImmutableLazyRedBlackTree<T> IImmutableLazyBinarySearchTree<T, T, ImmutableLazyRedBlackTree<T>>.Apply(int l, int r, T f) { throw new NotSupportedException(); }
        T IImmutableBinarySearchTree<T, ImmutableLazyRedBlackTree<T>>.Prod(int l, int r) { throw new NotSupportedException(); }
        T IImmutableBinarySearchTree<T, ImmutableLazyRedBlackTree<T>>.Slice(int l, int length) { throw new NotSupportedException(); }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
        public struct TCp : ICopyOperator<LazyRedBlackTreeNode<T, T>>
        {
            [凾(256)]
            public LazyRedBlackTreeNode<T, T> Copy(LazyRedBlackTreeNode<T, T> t) => new LazyRedBlackTreeNode<T, T>(t.Key, t.Lazy)
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
