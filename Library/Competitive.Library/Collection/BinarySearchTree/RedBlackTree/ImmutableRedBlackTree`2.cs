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
    // competitive-verifier: TITLE 永続赤黒木(範囲取得)
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp

    /// <summary>
    /// 永続赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ImmutableRedBlackTree<T, TOp> : IImmutableBinarySearchTree<T, ImmutableRedBlackTree<T, TOp>>
        where TOp : struct, ISegtreeOperator<T>
    {
        private static BinarySearchTreeNodeOperator<T, TOp, RedBlackTreeNode<T>, RedBlackTreeNodeOperator<T, TOp, TCp>> rb => default;

        private RedBlackTreeNode<T> root;

        public static ImmutableRedBlackTree<T, TOp> Empty { get; } = new ImmutableRedBlackTree<T, TOp>();
        protected ImmutableRedBlackTree() { }

        private ImmutableRedBlackTree(RedBlackTreeNode<T> root) { this.root = root; }
        public ImmutableRedBlackTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public ImmutableRedBlackTree(T[] v) : this(v.AsSpan()) { }
        public ImmutableRedBlackTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }

        [凾(256)] public T Prod(int l, int r) => rb.Prod(ref root, l, r);
        [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);
        public T AllProd => rb.Sum(root);
        public int Count => rb.Size(root);
        public T this[int index] => rb.im.GetValue(ref root, index);

        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> Add(T item)
        {
            var t = root;
            rb.AddLast(ref t, item);
            return new ImmutableRedBlackTree<T, TOp>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> AddRange(IEnumerable<T> items)
        {
            var t = rb.im.Merge(root, rb.im.Build(items.ToArray()));
            return new ImmutableRedBlackTree<T, TOp>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> Clear() => Empty;

        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> Insert(int index, T item)
        {
            var t = root;
            rb.Insert(ref t, index, item);
            return new ImmutableRedBlackTree<T, TOp>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> InsertRange(int index, IEnumerable<T> items)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            rb.Merge3(t1, rb.im.Build(items.ToArray()), t2);
            return new ImmutableRedBlackTree<T, TOp>(t);
        }


        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> RemoveAt(int index)
        {
            var t = root;
            rb.Erase(ref t, index);
            return new ImmutableRedBlackTree<T, TOp>(t);
        }

        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> RemoveRange(int index, int count)
        {
            var t = root;
            var (t1, _, t3) = rb.Split3(t, index, index + count);
            return new ImmutableRedBlackTree<T, TOp>(rb.im.Merge(t1, t3));
        }

        [凾(256)]
        public ImmutableRedBlackTree<T, TOp> SetItem(int index, T value)
        {
            var t = root;
            rb.im.SetValue(ref t, index, value);
            return new ImmutableRedBlackTree<T, TOp>(t);
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
