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
    // https://ei1333.github.io/library/structure/bbst/randomized-binary-search-tree-lazy.hpp
    /// <summary>
    /// 乱択平衡二分探索木
    /// </summary>
    public class RandomBinarySearchTree<T> : RandomBinarySearchTree<T, SingleBbstOp<T>>
    {
        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public RandomBinarySearchTree(T[] v) : base(v.AsSpan()) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
    }

    /// <summary>
    /// 乱択平衡二分探索木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RandomBinarySearchTree<T, TOp> : IBinarySearchTree<T>
        where TOp : struct, ISegtreeOperator<T>
    {
        public static BinarySearchTreeNodeOperator<T, TOp, RandomBinarySearchTreeNode<T>, RandomBinarySearchTreeNodeOperator<T, TOp>> rb => default;

        public RandomBinarySearchTreeNode<T> root;

        public RandomBinarySearchTree(RandomBinarySearchTreeNode<T> root) { this.root = root; }
        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public RandomBinarySearchTree(T[] v) : this(v.AsSpan()) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }

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
        public RandomBinarySearchTreeEnumerator<T, TOp> GetEnumerator()
            => rb.im.GetEnumerator(root);
        [凾(256)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        [凾(256)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct TCp : ICopyOperator<RandomBinarySearchTreeNode<T>>
        {
            [凾(256)]
            public RandomBinarySearchTreeNode<T> Copy(RandomBinarySearchTreeNode<T> t) => t;
        }
    }
}
