using Kzrnm.Competitive.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class PersistentSetBase<T, TKey, TSet, TNOp> : ICollection, ICollection<T>, IReadOnlyCollection<T>
        where TSet : PersistentSetBase<T, TKey, TSet, TNOp>
        where TNOp : struct, IPersistentSetNodeOperator<T, TKey>, IPersistentSetOperator<T, TKey, TSet, TNOp>
    {
        /// <summary>
        /// The root node of the AVL tree that stores this set.
        /// </summary>
        private readonly PersistentSetNode<T, TKey, TNOp> _root;

        /// <summary>
        /// The comparer used to sort elements in this set.
        /// </summary>
        private readonly IComparer<TKey> _comparer;

        private readonly bool _isMulti;

        internal static TNOp op => new TNOp();
        private TSet AsGeneric => Unsafe.As<TSet>(this);

        public static TSet Empty(bool isMulti = true, IComparer<TKey> comparer = null) => op.Empty(isMulti, comparer);

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentSetBase{T, TKey, TSet, TNOp}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        internal PersistentSetBase(IComparer<TKey> comparer = null)
        {
            _root = PersistentSetNode<T, TKey, TNOp>.EmptyNode;
            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentSetBase{T, TKey, TSet, TNOp}"/> class.
        /// </summary>
        /// <param name="root">The root of the AVL tree with the contents of this set.</param>
        /// <param name="isMulti">Accept same value.</param>
        /// <param name="comparer">The comparer.</param>
        protected PersistentSetBase(PersistentSetNode<T, TKey, TNOp> root, bool isMulti, IComparer<TKey> comparer)
        {
            root.Freeze();
            _root = root;
            _comparer = comparer;
            _isMulti = isMulti;
        }

        public TSet Clear()
        {
            return _root.IsEmpty ? AsGeneric : op.Empty(_isMulti, _comparer);
        }

        /// <summary>
        /// Gets the maximum value in the collection, as defined by the comparer.
        /// </summary>
        /// <value>The maximum value in the set.</value>
        public T Max
        {
            get { return _root.Max.value; }
        }

        /// <summary>
        /// Gets the minimum value in the collection, as defined by the comparer.
        /// </summary>
        /// <value>The minimum value in the set.</value>
        public T Min
        {
            get { return _root.Min.value; }
        }

        #region IImmutableSet<T> Properties

        /// <summary>
        /// See the <see cref="IImmutableSet{T}"/> interface.
        /// </summary>
        public bool IsEmpty
        {
            get { return _root.IsEmpty; }
        }

        /// <summary>
        /// See the <see cref="IImmutableSet{T}"/> interface.
        /// </summary>
        public int Count
        {
            get { return _root.Count; }
        }


        public IComparer<TKey> KeyComparer
        {
            get { return _comparer; }
        }

        #endregion

        #region IReadOnlyList<T> Indexers

        /// <summary>
        /// Gets the element of the set at the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the element in the set to return.</param>
        /// <returns>The element at the given position.</returns>
        public PersistentSetNode<T, TKey, TNOp> this[int index] => _root[index];


        #endregion

        #region Public methods
        [凾(256)]
        public bool Contains(TKey value)
        {
            return _root.Contains(value, _comparer);
        }

        /// <summary>
        /// See the <see cref="IImmutableSet{T}"/> interface.
        /// </summary>
        [凾(256)]
        public TSet Add(T value)
        {
            return Wrap(_root.Add(value, _comparer, _isMulti, out _));
        }

        /// <summary>
        /// See the <see cref="IImmutableSet{T}"/> interface.
        /// </summary>
        [凾(256)]
        public TSet Remove(TKey value)
        {
            return Wrap(_root.Remove(value, _comparer, out _));
        }


        /// <summary>
        /// 最大値を削除します。
        /// </summary>
        /// <returns>The new tree.</returns>
        public TSet RemoveMax(out T maxValue)
        {
            var rt = Wrap(_root.RemoveMax(out var n));
            maxValue = n.value;
            return rt;
        }

        /// <summary>
        /// 最小値を削除します。
        /// </summary>
        /// <returns>The new tree.</returns>
        public TSet RemoveMin(out T minValue)
        {
            var rt = Wrap(_root.RemoveMin(out var n));
            minValue = n.value;
            return rt;
        }

        /// <summary>
        /// 最大値を削除します。
        /// </summary>
        /// <returns>The new tree.</returns>
        public TSet RemoveMax() => RemoveMax(out _);

        /// <summary>
        /// 最小値を削除します。
        /// </summary>
        /// <returns>The new tree.</returns>
        public TSet RemoveMin() => RemoveMin(out _);

        /// <summary>
        /// Searches the set for a given value and returns the equal value it finds, if any.
        /// </summary>
        /// <param name="equalValue">The value to search for.</param>
        /// <param name="actualValue">The value from the set that the search found, or the original value if the search yielded no match.</param>
        /// <returns>A value indicating whether the search was successful.</returns>
        /// <remarks>
        /// This can be useful when you want to reuse a previously stored reference instead of
        /// a newly constructed one (so that more sharing of references can occur) or to look up
        /// a value that has more complete data than the value you currently have, although their
        /// comparer functions indicate they are equal.
        /// </remarks>
        public bool TryGetValue(TKey equalValue, out T actualValue)
        {
            var searchResult = _root.FindNode(equalValue, _comparer);
            if (searchResult.IsEmpty)
            {
                actualValue = default;
                return false;
            }
            else
            {
                actualValue = searchResult.value;
                return true;
            }
        }

        /// <summary>
        /// See the <see cref="IImmutableSet{T}"/> interface.
        /// </summary>
        public TSet Intersect(IEnumerable<T> other)
        {
            var newSet = Clear();
            foreach (var item in other)
            {
                if (Contains(op.GetCompareKey(item)))
                {
                    newSet = newSet.Add(item);
                }
            }

            Debug.Assert(newSet != null);
            return newSet;
        }

        /// <summary>
        /// See the <see cref="IImmutableSet{T}"/> interface.
        /// </summary>
        public TSet Except(IEnumerable<T> other)
        {
            var result = _root;
            foreach (T item in other)
            {
                result = result.Remove(op.GetCompareKey(item), _comparer, out _);
            }

            return Wrap(result);
        }

        /// <summary>
        /// Produces a set that contains elements either in this set or a given sequence, but not both.
        /// </summary>
        /// <param name="other">The other sequence of items.</param>
        /// <returns>The new set.</returns>
        public TSet SymmetricExcept(IEnumerable<T> other)
        {
            var otherAsSet = op.Empty(_isMulti, _comparer).Union(other);

            var result = this.Clear();
            foreach (var item in this)
            {
                if (!otherAsSet.Contains(item))
                {
                    result = result.Add(item);
                }
            }

            foreach (T item in otherAsSet)
            {
                if (!this.Contains(item))
                {
                    result = result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// See the <see cref="IImmutableSet{T}"/> interface.
        /// </summary>
        public TSet Union(IEnumerable<T> other)
        {
            if (TryCastToPersistentSetBase(other, out var immutableSortedSet) && immutableSortedSet.KeyComparer == this.KeyComparer) // argument is a compatible immutable sorted set
            {
                if (immutableSortedSet.IsEmpty)
                {
                    return AsGeneric;
                }
                else if (this.IsEmpty)
                {
                    // Adding the argument to this collection is equivalent to returning the argument.
                    return immutableSortedSet;
                }
                else if (immutableSortedSet.Count > this.Count)
                {
                    // We're adding a larger set to a smaller set, so it would be faster to simply
                    // add the smaller set to the larger set.
                    return immutableSortedSet.Union(this);
                }
            }

            if (this.IsEmpty)
            {
                // The payload being added is so large compared to this collection's current size
                // that we likely won't see much memory reuse in the node tree by performing an
                // incremental update.  So just recreate the entire node tree since that will
                // likely be faster.
                return this.LeafToRootRefill(other);
            }

            return this.UnionIncremental(other);
        }

        /// <summary>
        /// Determines whether the current set is a correct superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>true if the current set is a correct superset of other; otherwise, false.</returns>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (this.IsEmpty)
            {
                return false;
            }

            int count = 0;
            foreach (T item in other)
            {
                count++;
                if (!this.Contains(item))
                {
                    return false;
                }
            }

            return this.Count > count;
        }

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>true if the current set is a superset of other; otherwise, false.</returns>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            foreach (T item in other)
            {
                if (!this.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>true if the current set and other share at least one common element; otherwise, false.</returns>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (this.IsEmpty)
            {
                return false;
            }

            foreach (T item in other)
            {
                if (this.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> that iterates over this
        /// collection in reverse order.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates over the <see cref="PersistentSetBase{T, TKey, TSet, TNOp}"/>
        /// in reverse order.
        /// </returns>
        public IEnumerable<T> Reverse()
        {
            return new ReverseEnumerable(_root);
        }

        /// <summary>
        /// Gets the position within this set that the specified value does or would appear.
        /// </summary>
        /// <param name="item">The value whose position is being sought.</param>
        /// <returns>
        /// The index of the specified <paramref name="item"/> in the sorted set,
        /// if <paramref name="item"/> is found.  If <paramref name="item"/> is not
        /// found and <paramref name="item"/> is less than one or more elements in this set,
        /// a negative number which is the bitwise complement of the index of the first
        /// element that is larger than value. If <paramref name="item"/> is not found
        /// and <paramref name="item"/> is greater than any of the elements in the set,
        /// a negative number which is the bitwise complement of (the index of the last
        /// element plus 1).
        /// </returns>
        public int IndexOf(T item)
        {
            var (node, index) = _root.BinarySearch<PersistentSetNode<T, TKey, TNOp>.L>(op.GetCompareKey(item), _comparer);
            if (EqualityComparer<T>.Default.Equals(item, node.value))
                return index;
            return -1;
        }

        #endregion

        #region ICollection<T> members

        /// <summary>
        /// See the <see cref="ICollection{T}"/> interface.
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// See the <see cref="ICollection{T}"/> interface.
        /// </summary>
        void ICollection<T>.CopyTo(T[] array, int index)
        {
            foreach (var node in _root)
            {
                array[index++] = node.value;
            }
        }

        /// <summary>
        /// See the <see cref="IList{T}"/> interface.
        /// </summary>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// See the <see cref="ICollection{T}"/> interface.
        /// </summary>
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// See the <see cref="IList{T}"/> interface.
        /// </summary>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// See the <see cref="IList{T}"/> interface.
        /// </summary>
        bool ICollection<T>.Contains(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region ICollection Properties

        /// <summary>
        /// See <see cref="ICollection"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object ICollection.SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// See the <see cref="ICollection"/> interface.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection.IsSynchronized
        {
            get
            {
                // This is immutable, so it is always thread-safe.
                return true;
            }
        }

        #endregion

        #region ICollection Methods

        /// <summary>
        /// Copies the elements of the <see cref="ICollection"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ICollection"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection<T>)this).CopyTo((T[])array, index);
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.IsEmpty ?
                Enumerable.Empty<T>().GetEnumerator() :
                this.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        /// <remarks>
        /// CAUTION: when this enumerator is actually used as a valuetype (not boxed) do NOT copy it by assigning to a second variable
        /// or by passing it to another method.  When this enumerator is disposed of it returns a mutable reference type stack to a resource pool,
        /// and if the value type enumerator is copied (which can easily happen unintentionally if you pass the value around) there is a risk
        /// that a stack that has already been returned to the resource pool may still be in use by one of the enumerator copies, leading to data
        /// corruption and/or exceptions.
        /// </remarks>
        public PersistentSetNode<T, TKey, TNOp>.ValueEnumerator GetEnumerator()
        {
            return _root.GetEnumerator().ToValueEnumerator();
        }

        /// <summary>
        /// Discovers an immutable sorted set for a given value, if possible.
        /// </summary>
        private static bool TryCastToPersistentSetBase(IEnumerable<T> sequence, [NotNullWhen(true)] out TSet other)
        {
            other = sequence as TSet;
            if (other != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds items to this collection using the standard spine rewrite and tree rebalance technique.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>The new collection.</returns>
        /// <remarks>
        /// This method is least demanding on memory, providing the great chance of memory reuse
        /// and does not require allocating memory large enough to store all items contiguously.
        /// It's performance is optimal for additions that do not significantly dwarf the existing
        /// size of this collection.
        /// </remarks>
        private TSet UnionIncremental(IEnumerable<T> items)
        {
            // Let's not implement in terms of PersistentSetBase.Add so that we're
            // not unnecessarily generating a new wrapping set object for each item.
            var result = _root;
            foreach (var item in items)
            {
                result = result.Add(item, _comparer, _isMulti, out _);
            }

            return Wrap(result);
        }

        /// <summary>
        /// Creates a wrapping collection type around a root node.
        /// </summary>
        /// <param name="root">The root node to wrap.</param>
        /// <returns>A wrapping collection type for the new tree.</returns>
        private TSet Wrap(PersistentSetNode<T, TKey, TNOp> root)
        {
            if (root != _root)
            {
                return root.IsEmpty ? this.Clear() : op.Create(root, _isMulti, _comparer);
            }
            else
            {
                return AsGeneric;
            }
        }

        /// <summary>
        /// Creates an immutable sorted set with the contents from this collection and a sequence of elements.
        /// </summary>
        /// <param name="addedItems">The sequence of elements to add to this set.</param>
        /// <returns>The immutable sorted set.</returns>
        private TSet LeafToRootRefill(IEnumerable<T> addedItems)
        {
            // Rather than build up the immutable structure in the incremental way,
            // build it in such a way as to generate minimal garbage, by assembling
            // the immutable binary tree from leaf to root.  This requires
            // that we know the length of the item sequence in advance, sort it,
            // and can index into that sequence like a list, so the limited
            // garbage produced is a temporary mutable data structure we use
            // as a reference when creating the immutable one.

            // Produce the initial list containing all elements, including any duplicates.
            List<T> list;
            if (this.IsEmpty)
            {
                // Otherwise, construct a list from the items.  The Count could still
                // be zero, in which case, again, just return this empty instance.
                list = new List<T>(addedItems);
                if (list.Count == 0)
                {
                    return AsGeneric;
                }
            }
            else
            {
                // Build the list from this set and then add the additional items.
                // Even if the additional items is empty, this set isn't, so we know
                // the resulting list will not be empty.
                list = new List<T>(this);
                list.AddRange(addedItems);
            }
            Debug.Assert(list.Count > 0);

            // Sort the list and remove duplicate entries.
            var array = list.ToArray();
            var comparer = KeyComparer;
            Array.Sort(array.Select(op.GetCompareKey).ToArray(), array, comparer);
            var sp = array.AsSpan();
            int index = 1;
            if (!_isMulti)
            {
                for (int i = 1; i < sp.Length; i++)
                {
                    if (comparer.Compare(op.GetCompareKey(sp[i]), op.GetCompareKey(sp[i - 1])) != 0)
                    {
                        sp[index++] = sp[i];
                    }
                }
                sp = sp[..index];
            }

            // Use the now sorted list of unique items to construct a new sorted set.
            var root = PersistentSetNode<T, TKey, TNOp>.NodeTreeFromList(sp);
            return Wrap(root);
        }

        /// <summary>
        /// An reverse enumerable of a sorted set.
        /// </summary>
        private sealed class ReverseEnumerable : IEnumerable<T>
        {
            /// <summary>
            /// The root node to enumerate.
            /// </summary>
            private readonly PersistentSetNode<T, TKey, TNOp> _root;

            /// <summary>
            /// Initializes a new instance of the <see cref="PersistentSetBase{T, TKey, TSet, TNOp}.ReverseEnumerable"/> class.
            /// </summary>
            /// <param name="root">The root of the data structure to reverse enumerate.</param>
            internal ReverseEnumerable(PersistentSetNode<T, TKey, TNOp> root)
            {
                _root = root;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
            /// </returns>
            public PersistentSetNode<T, TKey, TNOp>.ValueEnumerator GetEnumerator()
            {
                return _root.Reverse().ToValueEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
