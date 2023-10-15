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
    /// 永続赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ImmutableRedBlackTree<T> : IImmutableBinarySearchTree<T, ImmutableRedBlackTree<T>>
    {
        public static BinarySearchTreeNodeOperator<T, SingleBbstOp<T>, RedBlackTreeNode<T>, RedBlackTreeNodeOperator<T, SingleBbstOp<T>, TCp>> rb => default;

        public RedBlackTreeNode<T> root;

        public static ImmutableRedBlackTree<T> Empty { get; } = new ImmutableRedBlackTree<T>();
        protected ImmutableRedBlackTree() { }

        public ImmutableRedBlackTree(RedBlackTreeNode<T> root) { this.root = root; }
        public ImmutableRedBlackTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public ImmutableRedBlackTree(T[] v) : this(v.AsSpan()) { }
        public ImmutableRedBlackTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }
        public T AllProd => rb.Sum(root);
        public int Count => rb.Size(root);
        public T this[int index] => rb.im.GetValue(ref root, index);

        [凾(256)]
        public ImmutableRedBlackTree<T> Add(T item)
        {
            var t = root;
            rb.AddLast(ref t, item);
            return new ImmutableRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T> AddRange(IEnumerable<T> items)
        {
            var t = rb.im.Merge(root, rb.im.Build(items.ToArray()));
            return new ImmutableRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T> Clear() => Empty;

        [凾(256)]
        public ImmutableRedBlackTree<T> Insert(int index, T item)
        {
            var t = root;
            rb.Insert(ref t, index, item);
            return new ImmutableRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T> InsertRange(int index, IEnumerable<T> items)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            t = rb.Merge3(t1, rb.im.Build(items.ToArray()), t2);
            return new ImmutableRedBlackTree<T>(t);
        }
        [凾(256)]
        public ImmutableRedBlackTree<T> AddRange(ImmutableRedBlackTree<T> other)
        {
            var t = rb.im.Merge(root, other.root);
            return new ImmutableRedBlackTree<T>(t);
        }
        [凾(256)]
        public ImmutableRedBlackTree<T> InsertRange(int index, ImmutableRedBlackTree<T> other)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            rb.Merge3(t1, other.root, t2);
            return new ImmutableRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T> RemoveAt(int index)
        {
            var t = root;
            rb.Erase(ref t, index);
            return new ImmutableRedBlackTree<T>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T> RemoveRange(int index, int count)
        {
            var t = root;
            var (t1, _, t3) = rb.Split3(t, index, index + count);
            return new ImmutableRedBlackTree<T>(rb.im.Merge(t1, t3));
        }

        [凾(256)]
        public ImmutableRedBlackTree<T> SetItem(int index, T value)
        {
            var t = root;
            rb.im.SetValue(ref t, index, value);
            return new ImmutableRedBlackTree<T>(t);
        }

        [凾(256)]
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var v in this)
                array[arrayIndex++] = v;
        }

        [凾(256)]
        public RedBlackTreeEnumerator<T, SingleBbstOp<T>> GetEnumerator()
            => rb.im.GetEnumerator(root);
        [凾(256)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        [凾(256)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        T IImmutableBinarySearchTree<T, ImmutableRedBlackTree<T>>.Prod(int l, int r) { throw new NotSupportedException(); }
        T IImmutableBinarySearchTree<T, ImmutableRedBlackTree<T>>.Slice(int l, int length) { throw new NotSupportedException(); }

        public struct TCp : ICopyOperator<RedBlackTreeNode<T>>
        {
            [凾(256)]
            public RedBlackTreeNode<T> Copy(RedBlackTreeNode<T> t) => new RedBlackTreeNode<T>(t.Key)
            {
                Level = t.Level,
                Color = t.Color,
                Left = t.Left,
                Right = t.Right,
                Sum = t.Sum,
                Size = t.Size,
            };
        }
    }
}
