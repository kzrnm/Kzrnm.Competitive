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
    public class Set<T> : Set<T, DefaultComparerStruct<T>>
        where T : IComparable<T>
    {
        public Set(bool isMulti = false) : base(new DefaultComparerStruct<T>(), isMulti) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : base(collection, new DefaultComparerStruct<T>(), isMulti) { }
    }

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Set<T, TOp> : SetBase<T, T, Set<T, TOp>.Node, Set<T, TOp>.NodeOperator>, ICollection<T>
        where TOp : struct, IComparer<T>
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

        public Set(bool isMulti = false) : this(default(TOp), isMulti) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : this(collection, default(TOp), isMulti) { }
        public Set(TOp comparer, bool isMulti = false) : base(isMulti, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public Set(IEnumerable<T> collection, TOp comparer, bool isMulti = false)
            : base(isMulti, new NodeOperator(comparer), collection) { }

        #region Search
        public Node FindNode(T item)
        {
            Node current = root;
            while (current != null)
            {
                int order = comparer.Compare(item, current.Item);
                if (order == 0) return current;
                current = (Node)(order < 0 ? current.Left : current.Right);
            }
            return null;
        }
        public (Node node, int index) BinarySearch(T item, bool isLowerBound)
        {
            Node right = null;
            Node current = root;
            if (current == null) return (null, 0);
            int ri = Count;
            int ci = NodeSize(current.Left);
            while (true)
            {
                var order = comparer.Compare(item, current.Item);
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
        public Node FindNodeLowerBound(T item) => BinarySearch(item, true).node;
        public Node FindNodeUpperBound(T item) => BinarySearch(item, false).node;
        public int LowerBoundIndex(T item) => BinarySearch(item, true).index;
        public int UpperBoundIndex(T item) => BinarySearch(item, false).index;
        public T LowerBoundItem(T item) => BinarySearch(item, true).node.Item;
        public T UpperBoundItem(T item) => BinarySearch(item, false).node.Item;
        #endregion Search

        #region ICollection<T> members
        void ICollection<T>.Add(T item) => Add(item);
        public bool Add(T item)
        {
            if (root == null)
            {
                root = new Node(item, NodeColor.Black);
                return true;
            }
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;
            int order = 0;
            while (current != null)
            {
                order = comparer.Compare(item, current.Item);
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
            Node node = new Node(item, NodeColor.Red);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.ColorBlack();
            return true;
        }

        public bool Remove(T item)
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
                int order = foundMatch ? -1 : comparer.Compare(item, current.Item);
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
        public bool Contains(T item) => FindNode(item) != null;
        #endregion ICollection<T> members

        protected readonly TOp comparer;
        bool ICollection<T>.IsReadOnly => false;
        public class Node : SetNodeBase
        {
            public T Item;
            internal Node(T item, NodeColor color) : base(color)
            {
                Item = item;
            }
            public override string ToString() => $"Item = {Item}, Size = {Size}";
        }
        public readonly struct NodeOperator : INodeOperator<T, Node>
        {
            private readonly TOp comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [MethodImpl(AggressiveInlining)]
            public Node Create(T item, NodeColor color) => new Node(item, color);
            [MethodImpl(AggressiveInlining)]
            public T GetValue(Node node) => node.Item;
            [MethodImpl(AggressiveInlining)]
            public int Compare(T x, T y) => comparer.Compare(x, y);
        }
    }
}
