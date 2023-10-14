using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // competitive-verifier: TITLE 二分探索木のインターフェイス
    public interface IBinarySearchTree<T> : IList<T>
    {
        void AddRange(IEnumerable<T> items);
        void InsertRange(int index, IEnumerable<T> items);
        void RemoveRange(int index, int count);
        bool ICollection<T>.IsReadOnly => false;
        bool ICollection<T>.Contains(T item) { throw new NotSupportedException(); }
        int IList<T>.IndexOf(T item) { throw new NotSupportedException(); }
        bool ICollection<T>.Remove(T item) { throw new NotSupportedException(); }
        T AllProd { get; }
        T Prod(int l, int r);
        T Slice(int l, int length);
    }
    public interface ILazyBinarySearchTree<T, F> : IBinarySearchTree<T>
    {
        void Apply(int l, int r, F f);
        void Reverse();
        void Reverse(int l, int r);
    }
    public interface IImmutableBinarySearchTree<T, TSelf> : IImmutableList<T>
        where TSelf : IImmutableBinarySearchTree<T, TSelf>
    {
        T AllProd { get; }
        T Prod(int l, int r);
        T Slice(int l, int length);

        new TSelf Add(T item);
        IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);

        new TSelf AddRange(IEnumerable<T> items);
        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items)
            => AddRange(items);

        new TSelf Clear();
        IImmutableList<T> IImmutableList<T>.Clear() => Clear();

        new TSelf Insert(int index, T item);
        IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
            => Insert(index, element);

        new TSelf InsertRange(int index, IEnumerable<T> items);
        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
            => InsertRange(index, items);

        new TSelf RemoveAt(int index);
        IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
            => RemoveAt(index);

        new TSelf RemoveRange(int index, int count);
        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count)
            => RemoveRange(index, count);

        new TSelf SetItem(int index, T value);
        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value)
            => SetItem(index, value);

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var v in this)
                array[arrayIndex++] = v;
        }

        int IImmutableList<T>.IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
        int IImmutableList<T>.LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) { throw new NotSupportedException(); }
        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
    }

    public interface IImmutableLazyBinarySearchTree<T, F, TSelf> : IImmutableBinarySearchTree<T, TSelf>
        where TSelf : IImmutableLazyBinarySearchTree<T, F, TSelf>
    {
        TSelf Apply(int l, int r, F f);
        TSelf Reverse();
        TSelf Reverse(int l, int r);
    }
}