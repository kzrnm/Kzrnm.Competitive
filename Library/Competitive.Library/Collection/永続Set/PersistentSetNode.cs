using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 永続SetのNode

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Value = {" + nameof(value) + "}, Count = {" + nameof(Count) + "}")]
    public sealed class PersistentSetNode<T, TKey, TNOp> : IEnumerable<PersistentSetNode<T, TKey, TNOp>>
        where TNOp : struct, IPersistentSetNodeOperator<T, TKey>
    {
        /*
         * Original is ImmutableSortedSet<TKey, TValue>
         *
         * Copyright (c) .NET Foundation and Contributors
         * Released under the MIT license
         * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
         */
        internal const string LISENCE = @"
Original is ImmutableSortedSet<T>

Copyright (c) .NET Foundation and Contributors
Released under the MIT license
https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
";

        internal static TNOp op => new TNOp();
        internal static readonly PersistentSetNode<T, TKey, TNOp> EmptyNode = new PersistentSetNode<T, TKey, TNOp>();
        internal readonly T value;

        internal bool frozen;
        internal byte height;
        internal PersistentSetNode<T, TKey, TNOp> left;
        internal PersistentSetNode<T, TKey, TNOp> right;

        public int Count { get; internal set; }

        public PersistentSetNode()
        {
            frozen = true;
        }
        internal PersistentSetNode(T v, PersistentSetNode<T, TKey, TNOp> left, PersistentSetNode<T, TKey, TNOp> right, bool frozen = false)
        {
            Debug.Assert(!frozen || (left.frozen && right.frozen));

            value = v;
            this.left = left;
            this.right = right;
            height = checked((byte)(1 + Math.Max(left.height, right.height)));
            Count = 1 + left.Count + right.Count;
            this.frozen = frozen;
        }

        public bool IsEmpty => left == null;
        internal PersistentSetNode<T, TKey, TNOp> Max
        {
            get
            {
                if (IsEmpty) return null;
                var n = this;
                while (!n.right.IsEmpty) n = n.right;
                return n;
            }
        }
        internal PersistentSetNode<T, TKey, TNOp> Min
        {
            get
            {
                if (IsEmpty) return null;
                var n = this;
                while (!n.left.IsEmpty) n = n.left;
                return n;
            }
        }
        internal PersistentSetNode<T, TKey, TNOp> this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count)
                    throw new IndexOutOfRangeException("index is out of range.");
                Debug.Assert(left != null && right != null);

                if (index < left.Count)
                    return left[index];

                if (index > left.Count)
                    return right[index - left.Count - 1];

                return this;
            }
        }
        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<PersistentSetNode<T, TKey, TNOp>> IEnumerable<PersistentSetNode<T, TKey, TNOp>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds the specified key to the tree.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="multi">Accept same value.</param>
        /// <param name="mutated">Receives a value indicating whether this node tree has mutated because of this operation.</param>
        /// <returns>The new tree.</returns>
        internal PersistentSetNode<T, TKey, TNOp> Add(T value, IComparer<TKey> comparer, bool multi, out bool mutated)
        {
            if (IsEmpty)
            {
                mutated = true;
                return new PersistentSetNode<T, TKey, TNOp>(value, this, this);
            }
            else
            {
                var result = this;
                int compareResult = comparer.Compare(op.GetCompareKey(value), op.GetCompareKey(this.value));
                if (compareResult > 0)
                {
                    var newRight = right.Add(value, comparer, multi, out mutated);
                    if (mutated)
                    {
                        result = Mutate(right: newRight);
                    }
                }
                else if (multi || compareResult < 0)
                {
                    var newLeft = left.Add(value, comparer, multi, out mutated);
                    if (mutated)
                    {
                        result = Mutate(left: newLeft);
                    }
                }
                else
                {
                    mutated = false;
                    return this;
                }

                return mutated ? MakeBalanced(result) : result;
            }
        }

        /// <summary>
        /// Removes the specified key from the tree.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="mutated">Receives a value indicating whether this node tree has mutated because of this operation.</param>
        /// <returns>The new tree.</returns>
        internal PersistentSetNode<T, TKey, TNOp> Remove(TKey key, IComparer<TKey> comparer, out bool mutated)
        {
            if (IsEmpty)
            {
                mutated = false;
                return this;
            }

            Debug.Assert(left != null && right != null);
            var result = this;
            int compare = comparer.Compare(key, op.GetCompareKey(value));
            if (compare == 0)
            {
                // We have a match.
                mutated = true;

                // If this is a leaf, just remove it
                // by returning Empty.  If we have only one child,
                // replace the node with the child.
                if (right.IsEmpty && left.IsEmpty)
                {
                    result = EmptyNode;
                }
                else if (right.IsEmpty && !left.IsEmpty)
                {
                    result = left;
                }
                else if (!right.IsEmpty && left.IsEmpty)
                {
                    result = right;
                }
                else
                {
                    // We have two children. Remove the next-highest node and replace
                    // this node with it.
                    var successor = right;
                    while (!successor.left.IsEmpty)
                    {
                        successor = successor.left;
                    }

                    var newRight = right.Remove(op.GetCompareKey(successor.value), comparer, out _);
                    result = successor.Mutate(left: left, right: newRight);
                }
            }
            else if (compare < 0)
            {
                var newLeft = left.Remove(key, comparer, out mutated);
                if (mutated)
                {
                    result = Mutate(left: newLeft);
                }
            }
            else
            {
                var newRight = right.Remove(key, comparer, out mutated);
                if (mutated)
                {
                    result = Mutate(right: newRight);
                }
            }

            return result.IsEmpty ? result : MakeBalanced(result);
        }

        /// <summary>
        /// 最大値を削除します。
        /// </summary>
        /// <returns>The new tree.</returns>
        internal PersistentSetNode<T, TKey, TNOp> RemoveMax(out PersistentSetNode<T, TKey, TNOp> maxNode)
        {
            if (IsEmpty)
            {
                maxNode = this;
                return this;
            }
            Debug.Assert(left != null && right != null);

            PersistentSetNode<T, TKey, TNOp> result;
            if (right.IsEmpty)
            {
                // If this is a leaf, just remove it
                // by returning Empty.  If we have only one child,
                // replace the node with the child.

                maxNode = this;

                if (left.IsEmpty)
                    result = EmptyNode;
                else
                    result = left;
            }
            else
            {
                result = Mutate(right: right.RemoveMax(out maxNode));
            }

            return result.IsEmpty ? result : MakeBalanced(result);
        }

        /// <summary>
        /// 最小値を削除します。
        /// </summary>
        /// <returns>The new tree.</returns>
        internal PersistentSetNode<T, TKey, TNOp> RemoveMin(out PersistentSetNode<T, TKey, TNOp> minNode)
        {
            if (IsEmpty)
            {
                minNode = this;
                return this;
            }
            Debug.Assert(left != null && right != null);

            PersistentSetNode<T, TKey, TNOp> result;
            if (left.IsEmpty)
            {
                // If this is a leaf, just remove it
                // by returning Empty.  If we have only one child,
                // replace the node with the child.

                minNode = this;

                if (right.IsEmpty)
                    result = EmptyNode;
                else
                    result = right;
            }
            else
            {
                result = Mutate(left: left.RemoveMin(out minNode));
            }

            return result.IsEmpty ? result : MakeBalanced(result);
        }

        /// <summary>
        /// Determines whether the specified key is in this tree.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        ///   <c>true</c> if the tree contains the specified key; otherwise, <c>false</c>.
        /// </returns>
        internal bool Contains(TKey key, IComparer<TKey> comparer) => !FindNode(key, comparer).IsEmpty;


        /// <summary>
        /// Freezes this node and all descendant nodes so that any mutations require a new instance of the nodes.
        /// </summary>
        internal void Freeze()
        {
            // If this node is frozen, all its descendants must already be frozen.
            if (!frozen)
            {
                Debug.Assert(left != null && right != null);
                left.Freeze();
                right.Freeze();
                frozen = true;
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> that iterates over this
        /// collection in reverse order.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates over the <see cref="PersistentSetNode{T, TCmp, TNOp}"/>
        /// in reverse order.
        /// </returns>
        internal Enumerator Reverse() => new Enumerator(this, reverse: true);


        #region Tree balancing methods

        /// <summary>
        /// AVL rotate left operation.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>The rotated tree.</returns>
        private static PersistentSetNode<T, TKey, TNOp> RotateLeft(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);

            if (tree.right.IsEmpty)
            {
                return tree;
            }

            var right = tree.right;
            return right.Mutate(left: tree.Mutate(right: right.left));
        }

        /// <summary>
        /// AVL rotate right operation.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>The rotated tree.</returns>
        private static PersistentSetNode<T, TKey, TNOp> RotateRight(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);

            if (tree.left.IsEmpty)
            {
                return tree;
            }

            var left = tree.left;
            return left.Mutate(right: tree.Mutate(left: left.right));
        }

        /// <summary>
        /// AVL rotate double-left operation.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>The rotated tree.</returns>
        private static PersistentSetNode<T, TKey, TNOp> DoubleLeft(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);

            if (tree.right.IsEmpty)
            {
                return tree;
            }

            var rotatedRightChild = tree.Mutate(right: RotateRight(tree.right));
            return RotateLeft(rotatedRightChild);
        }

        /// <summary>
        /// AVL rotate double-right operation.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>The rotated tree.</returns>
        private static PersistentSetNode<T, TKey, TNOp> DoubleRight(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);

            if (tree.left.IsEmpty)
            {
                return tree;
            }

            var rotatedLeftChild = tree.Mutate(left: RotateLeft(tree.left));
            return RotateRight(rotatedLeftChild);
        }

        /// <summary>
        /// Returns a value indicating whether the tree is in balance.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>0 if the tree is in balance, a positive integer if the right side is heavy, or a negative integer if the left side is heavy.</returns>
        private static int Balance(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);

            return tree.right.height - tree.left.height;
        }

        /// <summary>
        /// Determines whether the specified tree is right heavy.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>
        /// <c>true</c> if [is right heavy] [the specified tree]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsRightHeavy(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);
            return Balance(tree) >= 2;
        }

        /// <summary>
        /// Determines whether the specified tree is left heavy.
        /// </summary>
        private static bool IsLeftHeavy(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);
            return Balance(tree) <= -2;
        }

        /// <summary>
        /// Balances the specified tree.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>A balanced tree.</returns>
        private static PersistentSetNode<T, TKey, TNOp> MakeBalanced(PersistentSetNode<T, TKey, TNOp> tree)
        {
            Debug.Assert(!tree.IsEmpty);

            if (IsRightHeavy(tree))
            {
                return Balance(tree.right) < 0 ? DoubleLeft(tree) : RotateLeft(tree);
            }

            if (IsLeftHeavy(tree))
            {
                return Balance(tree.left) > 0 ? DoubleRight(tree) : RotateRight(tree);
            }

            return tree;
        }

        #endregion

        /// <summary>
        /// Creates a node tree that contains the contents of a list.
        /// </summary>
        /// <param name="sortedItems">An indexable list with the contents that the new node tree should contain.</param>
        /// <returns>The root of the created node tree.</returns>
        internal static PersistentSetNode<T, TKey, TNOp> NodeTreeFromList(ReadOnlySpan<T> sortedItems)
        {
            if (sortedItems.Length == 0)
            {
                return EmptyNode;
            }

            int half = sortedItems.Length / 2;
            var left = NodeTreeFromList(sortedItems[..half]);
            var right = NodeTreeFromList(sortedItems[(half + 1)..]);
            return new PersistentSetNode<T, TKey, TNOp>(sortedItems[half], left, right, true);
        }

        /// <summary>
        /// Creates a node mutation, either by mutating this node (if not yet frozen) or by creating a clone of this node
        /// with the described changes.
        /// </summary>
        /// <param name="left">The left branch of the mutated node.</param>
        /// <param name="right">The right branch of the mutated node.</param>
        /// <returns>The mutated (or created) node.</returns>
        private PersistentSetNode<T, TKey, TNOp> Mutate(PersistentSetNode<T, TKey, TNOp> left = null, PersistentSetNode<T, TKey, TNOp> right = null)
        {
            Debug.Assert(this.left != null && this.right != null);
            if (frozen)
            {
                return new PersistentSetNode<T, TKey, TNOp>(value, left ?? this.left, right ?? this.right);
            }
            else
            {
                if (left != null)
                {
                    this.left = left;
                }

                if (right != null)
                {
                    this.right = right;
                }

                height = checked((byte)(1 + Math.Max(this.left.height, this.right.height)));
                Count = 1 + this.left.Count + this.right.Count;
                return this;
            }
        }

        #region Search
        [凾(256)]
        public PersistentSetNode<T, TKey, TNOp> FindNode(TKey key, IComparer<TKey> comparer)
        {
            var current = this;
            while (!current.IsEmpty)
            {
                int order = comparer.Compare(key, op.GetCompareKey(current.value));
                if (order == 0) return current;
                current = order < 0 ? current.left : current.right;
            }
            return current;
        }

        /// <summary>
        /// <paramref name="key"/> 以上/超えるの要素のノードとインデックスを返します。
        /// </summary>
        /// <param name="key">検索する要素</param>
        /// <param name="comparer">比較</param>
        /// <param name="bop">二分探索の判定オペレーター</param>
        [凾(256)]
        internal (PersistentSetNode<T, TKey, TNOp> node, int index) BinarySearch<TBOp>(TKey key, IComparer<TKey> comparer, TBOp bop = default)
            where TBOp : struct, ISetBinarySearchOperator
        {
            PersistentSetNode<T, TKey, TNOp> left = null, right = null;
            var current = this;
            if (current == null) return (EmptyNode, 0);
            int li = -1;
            int ri = Count;
            int ci = current.left.Count;
            while (true)
            {
                if (bop.IntoLeft(comparer.Compare(key, op.GetCompareKey(current.value))))
                {
                    right = current;
                    ri = ci;
                    current = current.left;
                    if (!current.IsEmpty)
                        ci -= current.right.Count + 1;
                    else break;
                }
                else
                {
                    left = current;
                    li = ci;
                    current = current.right;
                    if (!current.IsEmpty)
                        ci += current.left.Count + 1;
                    else break;
                }
            }
            return bop.ReturnLeft ? (left, li) : (right, ri);
        }
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)]
        public PersistentSetNode<T, TKey, TNOp> FindNodeLowerBound(TKey item, IComparer<TKey> comparer) => BinarySearch<L>(item, comparer).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int LowerBoundIndex(TKey item, IComparer<TKey> comparer) => BinarySearch<L>(item, comparer).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素を返します。
        /// </summary>
        [凾(256)]
        public T LowerBoundItem(TKey item, IComparer<TKey> comparer) => BinarySearch<L>(item, comparer).node.value;
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)]
        public PersistentSetNode<T, TKey, TNOp> FindNodeUpperBound(TKey item, IComparer<TKey> comparer) => BinarySearch<U>(item, comparer).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int UpperBoundIndex(TKey item, IComparer<TKey> comparer) => BinarySearch<U>(item, comparer).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素を返します。
        /// </summary>
        [凾(256)]
        public T UpperBoundItem(TKey item, IComparer<TKey> comparer) => BinarySearch<U>(item, comparer).node.value;

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)]
        public PersistentSetNode<T, TKey, TNOp> FindNodeReverseLowerBound(TKey item, IComparer<TKey> comparer) => BinarySearch<LR>(item, comparer).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int ReverseLowerBoundIndex(TKey item, IComparer<TKey> comparer) => BinarySearch<LR>(item, comparer).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素を返します。
        /// </summary>
        [凾(256)]
        public T ReverseLowerBoundItem(TKey item, IComparer<TKey> comparer) => BinarySearch<LR>(item, comparer).node.value;

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)]
        public PersistentSetNode<T, TKey, TNOp> FindNodeReverseUpperBound(TKey item, IComparer<TKey> comparer) => BinarySearch<UR>(item, comparer).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int ReverseUpperBoundIndex(TKey item, IComparer<TKey> comparer) => BinarySearch<UR>(item, comparer).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素を返します。
        /// </summary>
        [凾(256)]
        public T ReverseUpperBoundItem(TKey item, IComparer<TKey> comparer) => BinarySearch<UR>(item, comparer).node.value;
        #endregion Search

        #region ISetBinarySearchOperator
        [IsOperator]
        public interface ISetBinarySearchOperator
        {
            /// <summary>
            /// 左側を返す
            /// </summary>
            bool ReturnLeft { get; }

            /// <summary>
            /// 左側に潜る
            /// </summary>
            bool IntoLeft(int order);
        }
        public struct L : ISetBinarySearchOperator
        {
            public bool ReturnLeft => false;
            [凾(256)]
            public bool IntoLeft(int order) => order <= 0;
        }
        public struct U : ISetBinarySearchOperator
        {
            public bool ReturnLeft => false;
            [凾(256)]
            public bool IntoLeft(int order) => order < 0;
        }
        public struct LR : ISetBinarySearchOperator
        {
            public bool ReturnLeft => true;
            [凾(256)]
            public bool IntoLeft(int order) => order < 0;
        }
        public struct UR : ISetBinarySearchOperator
        {
            public bool ReturnLeft => true;
            [凾(256)]
            public bool IntoLeft(int order) => order <= 0;
        }
        #endregion ISetBinarySearchOperator


        [EditorBrowsable(EditorBrowsableState.Never)]
        public struct Enumerator : IEnumerator<PersistentSetNode<T, TKey, TNOp>>
        {
            private PersistentSetNode<T, TKey, TNOp> _root;
            private Stack<PersistentSetNode<T, TKey, TNOp>> _stack;
            private PersistentSetNode<T, TKey, TNOp> _current;
            private bool _reverse;

            internal Enumerator(PersistentSetNode<T, TKey, TNOp> root, bool reverse = false)
            {
                _root = root;
                _current = null;
                _stack = null;
                _reverse = reverse;
                _stack = new Stack<PersistentSetNode<T, TKey, TNOp>>(root.height);
                PushLeft(_root);
            }

            public PersistentSetNode<T, TKey, TNOp> Current
            {
                [凾(256)]
                get
                {
                    ThrowIfDisposed();
                    return _current;
                }
            }
            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _root = null!;
                _current = null;
                _stack = null;
            }

            [凾(256)]
            public bool MoveNext()
            {
                ThrowIfDisposed();

                if (_stack != null)
                {
                    if (_stack.TryPop(out var n))
                    {
                        _current = n;
                        PushLeft(_reverse ? n.left : n.right);
                        return true;
                    }
                }

                _current = null;
                return false;
            }

            public void Reset()
            {
                ThrowIfDisposed();

                _current = null;
                if (_stack != null)
                {
                    _stack.Clear();
                    PushLeft(_root);
                }
            }

            [凾(256)]
            internal void ThrowIfDisposed()
            {
                if (_root == null)
                    Throw();

                static void Throw() => throw new InvalidOperationException();
            }

            [凾(256)]
            private void PushLeft(PersistentSetNode<T, TKey, TNOp> node)
            {
                Debug.Assert(_stack != null);
                while (!node.IsEmpty)
                {
                    _stack.Push(node);
                    node = _reverse ? node.right : node.left;
                }
            }

            [凾(256)]
            public ValueEnumerator ToValueEnumerator() => new ValueEnumerator(this);
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public struct ValueEnumerator : IEnumerator<T>, IEnumerable<T>
        {
            private Enumerator impl;

            internal ValueEnumerator(Enumerator e)
            {
                impl = e;
            }

            public T Current => impl.Current.value;
            object IEnumerator.Current => Current;

            [凾(256)] public bool MoveNext() => impl.MoveNext();
            public void Dispose() => impl.Dispose();
            public void Reset() => impl.Reset();

            [凾(256)] public IEnumerator<T> GetEnumerator() => this;
            [凾(256)] IEnumerator IEnumerable.GetEnumerator() => this;
        }
    }
}