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
    /// 遅延伝播反転可能赤黒木
    /// </summary>
    public class LazyRedBlackTree<T> : LazyRedBlackTree<T, ValueTuple, SingleBbstOp<T>>
    {
        public LazyRedBlackTree() { }
        public LazyRedBlackTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazyRedBlackTree(T[] v) : base(v.AsSpan()) { }
        public LazyRedBlackTree(ReadOnlySpan<T> v) : base(v) { }
    }

    /// <summary>
    /// 遅延伝播反転可能赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class LazyRedBlackTree<T, F, TOp> : ILazyBinarySearchTree<T, F>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public static LazyBinarySearchTreeNodeOperator<T, F, TOp, LazyRedBlackTreeNode<T, F>, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> rb => default;

        public LazyRedBlackTreeNode<T, F> root;

        public LazyRedBlackTree(LazyRedBlackTreeNode<T, F> root) { this.root = root; }
        public LazyRedBlackTree() { }
        public LazyRedBlackTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public LazyRedBlackTree(T[] v) : this(v.AsSpan()) { }
        public LazyRedBlackTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }

        public T this[int index]
        {
            get => rb.im.GetValue(ref root, index);
            set => rb.im.SetValue(ref root, index, value);
        }

        [凾(256)] public T Prod(int l, int r) => rb.Prod(ref root, l, r);
        [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);

        public T AllProd => rb.Sum(root);
        public int Count => rb.Size(root);
        bool ICollection<T>.IsReadOnly => false;

        [凾(256)]
        public void Apply(int l, int r, F f)
        {
            rb.Apply(ref root, l, r, f);
        }

        [凾(256)]
        public void Add(T item)
        {
            rb.AddLast(ref root, item);
        }
        [凾(256)]
        public void AddRange(IEnumerable<T> items)
        {
            root = rb.im.Merge(root, rb.im.Build(items.ToArray()));
        }

        [凾(256)]
        public void Insert(int index, T item)
        {
            rb.Insert(ref root, index, item);
        }
        [凾(256)]
        public void InsertRange(int index, IEnumerable<T> items)
        {
            var (t1, t2) = rb.im.Split(root, index);
            root = rb.Merge3(t1, rb.im.Build(items.ToArray()), t2);
        }

        void IList<T>.RemoveAt(int index) { RemoveAt(index); }
        [凾(256)]
        public T RemoveAt(int index)
            => rb.Erase(ref root, index);

        [凾(256)]
        public void RemoveRange(int index, int count)
        {
            var (t1, _, t3) = rb.Split3(root, index, index + count);
            root = rb.im.Merge(t1, t3);
        }
        [凾(256)]
        public void Reverse()
        {
            rb.Reverse(ref root, 0, Count);
        }

        [凾(256)]
        public void Reverse(int l, int r)
        {
            rb.Reverse(ref root, l, r);
        }

        [凾(256)]
        public void Clear()
        {
            root = null;
        }

        [凾(256)]
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var v in this)
                array[arrayIndex++] = v;
        }

        [凾(256)]
        public LazyRedBlackTreeEnumerator<T, F, TOp, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> GetEnumerator()
            => rb.im.GetEnumerator(root);
        [凾(256)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        [凾(256)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly struct TCp : ICopyOperator<LazyRedBlackTreeNode<T, F>>
        {
            [凾(256)]
            public LazyRedBlackTreeNode<T, F> Copy(LazyRedBlackTreeNode<T, F> t) => t;
        }

    }
}
