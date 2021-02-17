using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static SetNodeBase;
    using static MethodImplOptions;
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue> : SetDictionary<TKey, TValue, DefaultComparerStruct<TKey>>
        where TKey : IComparable<TKey>
    {
        public SetDictionary(bool isMulti = false) : base(new DefaultComparerStruct<TKey>(), isMulti) { }
        public SetDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dict, bool isMulti = false) : base(dict, new DefaultComparerStruct<TKey>(), isMulti) { }
    }

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue, TOp>
        : SetBase<KeyValuePair<TKey, TValue>, TKey, SetDictionary<TKey, TValue, TOp>.Node, SetDictionary<TKey, TValue, TOp>.NodeOperator>,
        IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TOp : struct, IComparer<TKey>
    {
        /*
         * Original is SortedSet<T>
         *
         * Copyright (c) .NET Foundation and Contributors
         * Released under the MIT license
         * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
         */
        internal const string LISENCE = @"
Original is SortedSet<T>

Copyright (c) .NET Foundation and Contributors
Released under the MIT license
https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
";

        public SetDictionary(bool isMulti = false) : this(default(TOp), isMulti) { }
        public SetDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dict, bool isMulti = false) : this(dict, default(TOp), isMulti) { }
        public SetDictionary(TOp comparer, bool isMulti = false) : base(isMulti, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public SetDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dict, TOp comparer, bool isMulti = false)
            : base(isMulti, new NodeOperator(comparer), dict) { }

        protected readonly TOp comparer;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                var res = new TKey[Count];
                var e = new Enumerator(this, false, null);
                for (int i = 0; i < res.Length; i++)
                {
                    e.MoveNext();
                    res[i] = e.Current.Key;
                }
                return res;
            }
        }
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                var res = new TValue[Count];
                var e = new Enumerator(this, false, null);
                for (int i = 0; i < res.Length; i++)
                {
                    e.MoveNext();
                    res[i] = e.Current.Value;
                }
                return res;
            }
        }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IDictionary<TKey, TValue>)this).Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IDictionary<TKey, TValue>)this).Values;
        public TValue this[TKey key]
        {
            get => FindNode(key).Value;
            set => FindNode(key).Value = value;
        }

        #region Search
        public Node FindNode(TKey key)
        {
            Node current = root;
            while (current != null)
            {
                int order = comparer.Compare(key, current.Key);
                if (order == 0) return current;
                current = (Node)(order < 0 ? current.Left : current.Right);
            }
            return null;
        }
        public (Node node, int index) BinarySearch(TKey key, bool isLowerBound)
        {
            Node right = null;
            Node current = root;
            if (current == null) return (null, 0);
            int ri = Count;
            int ci = NodeSize(current.Left);
            while (true)
            {
                var order = comparer.Compare(key, current.Key);
                if (order < 0 || (isLowerBound && order == 0))
                {
                    right = current;
                    ri = ci;
                    current = (Node)current.Left;
                    if (current != null)
                        ci -= NodeSize(current.Right) + 1;
                    else break;
                }
                else
                {
                    current = (Node)current.Right;
                    if (current != null)
                        ci += NodeSize(current.Left) + 1;
                    else break;
                }
            }
            return (right, ri);
        }
        public Node FindNodeLowerBound(TKey key) => BinarySearch(key, true).node;
        public Node FindNodeUpperBound(TKey key) => BinarySearch(key, false).node;
        public int LowerBoundIndex(TKey key) => BinarySearch(key, true).index;
        public int UpperBoundIndex(TKey key) => BinarySearch(key, false).index;
        public KeyValuePair<TKey, TValue> LowerBoundItem(TKey key) => BinarySearch(key, true).node.Pair;
        public KeyValuePair<TKey, TValue> UpperBoundItem(TKey key) => BinarySearch(key, false).node.Pair;
        #endregion Search

        #region ICollection<T> members
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => Add(key, value);
        public bool Add(TKey key, TValue value)
        {
            if (root == null)
            {
                root = new Node(key, value, NodeColor.Black);
                return true;
            }
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;
            int order = 0;
            while (current != null)
            {
                order = comparer.Compare(key, current.Key);
                if (order == 0 && !this.IsMulti)
                {
                    //op.SetValue(ref current, item);
                    root.ColorBlack();
                    return false;
                }
                if (current.Is4Node)
                {
                    current.Split4Node();
                    if (IsNonNullRed(parent))
                    {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = (Node)(order < 0 ? current.Left : current.Right);
            }
            Node node = new Node(key, value, NodeColor.Red);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.ColorBlack();
            return true;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
        public bool Remove(TKey key)
        {
            if (root == null) return false;
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node match = null;
            Node parentOfMatch = null;
            bool foundMatch = false;
            while (current != null)
            {
                if (current.Is2Node)
                {
                    if (parent == null)
                    {
                        current.ColorRed();
                    }
                    else
                    {
                        Node sibling = (Node)parent.GetSibling(current);
                        if (sibling.IsRed)
                        {
                            Debug.Assert(parent.IsBlack);
                            if (parent.Right == sibling) parent.RotateLeft();
                            else parent.RotateRight();

                            parent.ColorRed();
                            sibling.ColorBlack();
                            ReplaceChildOrRoot(grandParent, parent, sibling);
                            grandParent = sibling;
                            if (parent == match) parentOfMatch = sibling;
                            sibling = (Node)parent.GetSibling(current);
                        }
                        Debug.Assert(IsNonNullBlack(sibling));
                        if (sibling.Is2Node)
                        {
                            parent.Merge2Nodes();
                        }
                        else
                        {
                            Node newGrandParent = (Node)parent.Rotate(parent.GetRotation(current, sibling));
                            newGrandParent.Color = parent.Color;
                            parent.ColorBlack();
                            current.ColorRed();
                            ReplaceChildOrRoot(grandParent, parent, newGrandParent);
                            if (parent == match)
                            {
                                parentOfMatch = newGrandParent;
                            }
                        }
                    }
                }
                int order = foundMatch ? -1 : comparer.Compare(key, current.Key);
                if (order == 0)
                {
                    foundMatch = true;
                    match = current;
                    parentOfMatch = parent;
                }
                grandParent = parent;
                parent = current;
                current = (Node)(order < 0 ? current.Left : current.Right);
            }
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
            }
            root?.ColorBlack();
            return foundMatch;
        }
        public bool Contains(TKey key) => FindNode(key) != null;
        #endregion ICollection<T> members


        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        public bool ContainsKey(TKey key) => FindNode(key) != null;
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            var node = FindNodeLowerBound(pair.Key);
            if (node == null) return false;
            var e = new Enumerator(this, false, node);
            while (e.MoveNext())
            {
                if (comparer.Compare(pair.Key, e.Current.Key) != 0) break;
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, e.Current.Value)) return true;
            }
            return false;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(key);
            if (node == null)
            {
                value = default;
                return false;
            }
            value = node.Value;
            return true;
        }

        public class Node : SetNodeBase
        {
            public TKey Key;
            public TValue Value;
            public KeyValuePair<TKey, TValue> Pair => KeyValuePair.Create(Key, Value);
            internal Node(TKey key, TValue value, NodeColor color) : base(color)
            {
                Key = key;
                Value = value;
            }
            public override string ToString() => $"Key = {Key}, Value = {Value}, Size = {Size}";
        }
        public struct NodeOperator : INodeOperator<KeyValuePair<TKey, TValue>, Node>
        {
            private readonly TOp comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [MethodImpl(AggressiveInlining)]
            public Node Create(KeyValuePair<TKey, TValue> item, NodeColor color) => new Node(item.Key, item.Value, color);
            [MethodImpl(AggressiveInlining)]
            public KeyValuePair<TKey, TValue> GetValue(Node node) => node.Pair;
            [MethodImpl(AggressiveInlining)]
            public int Compare(KeyValuePair<TKey, TValue> node1, KeyValuePair<TKey, TValue> node2) => comparer.Compare(node1.Key, node2.Key);
        }
    }
}
