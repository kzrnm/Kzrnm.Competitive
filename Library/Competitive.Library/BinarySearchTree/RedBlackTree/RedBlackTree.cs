using AtCoder;
using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 赤黒木
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 赤黒木
    /// </summary>
    public class RedBlackTree<T> : RedBlackTree<T, SingleBbstOp<T>>
    {
        public RedBlackTree() { }
        public RedBlackTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public RedBlackTree(T[] v) : base(v.AsSpan()) { }
        public RedBlackTree(ReadOnlySpan<T> v) : base(v) { }
    }

    /// <summary>
    /// 赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RedBlackTree<T, TOp> : IBinarySearchTree<T>
        where TOp : struct, ISegtreeOperator<T>
    {
        public static BinarySearchTreeNodeOperator<T, TOp, RedBlackTreeNode<T>, RedBlackTreeNodeOperator<T, TOp, TCp>> rb => default;

        public RedBlackTreeNode<T> root;

        public RedBlackTree(RedBlackTreeNode<T> root) { this.root = root; }
        public RedBlackTree() { }
        public RedBlackTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public RedBlackTree(T[] v) : this(v.AsSpan()) { }
        public RedBlackTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }

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
        public RedBlackTreeEnumerator<T, TOp> GetEnumerator()
            => rb.im.GetEnumerator(root);
        [凾(256)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        [凾(256)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct TCp : ICopyOperator<RedBlackTreeNode<T>>
        {
            [凾(256)]
            public RedBlackTreeNode<T> Copy(RedBlackTreeNode<T> t) => t;
        }
    }
}
