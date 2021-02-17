using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    using static SetNodeBase;
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RangeSetLong : RangeSet<long, LongOperator>
    {
        public RangeSetLong() : base() { }
        public RangeSetLong(IEnumerable<(long From, long ToExclusive)> collection) : base(collection) { }
    }
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RangeSetInt : RangeSet<int, IntOperator>
    {
        public RangeSetInt() : base() { }
        public RangeSetInt(IEnumerable<(int From, int ToExclusive)> collection) : base(collection) { }
    }

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RangeSet<T, TOp>
        : SetBase<(T From, T ToExclusive), T, RangeSet<T, TOp>.Node, RangeSet<T, TOp>.NodeOperator>, ICollection<(T From, T ToExclusive)>
        where TOp : struct, IComparer<T>, IUnaryNumOperator<T>
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

        public RangeSet() : this(default(TOp)) { }
        public RangeSet(IEnumerable<(T From, T ToExclusive)> collection) : this(collection, default(TOp)) { }
        public RangeSet(TOp comparer) : base(false, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public RangeSet(IEnumerable<(T From, T ToExclusive)> collection, TOp comparer)
            : base(false, new NodeOperator(comparer), collection) { }
        protected override ((T From, T ToExclusive)[] array, int arrayCount) InitArray(IEnumerable<(T From, T ToExclusive)> collection)
        {
            var list = new SimpleList<(T From, T ToExclusive)>(
                collection.Where(t => comparer.Compare(t.From, t.ToExclusive) < 0));
            if (list.Count == 0) return (Array.Empty<(T From, T ToExclusive)>(), 0);

            list.Sort();
            var resList = new SimpleList<(T From, T ToExclusive)>(list.Count)
            {
                list[0]
            };
            for (int i = 1; i < list.Count; i++)
            {
                var pt = resList[^1].ToExclusive;
                var (f, t) = list[i];
                if (comparer.Compare(pt, f) >= 0)
                {
                    if (comparer.Compare(pt, t) < 0)
                        resList[^1].ToExclusive = t;
                }
                else
                    resList.Add((f, t));
            }

            return (resList.ToArray(), resList.Count);
        }

        protected readonly TOp comparer;

        #region Search
        public Node FindNode(T item)
        {
            Node current = root;
            while (current != null)
            {
                int order = Compare(item, current.From, current.ToExclusive);
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
                var order = Compare(item, current.From, current.ToExclusive);
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
        public (T From, T ToExclusive) LowerBoundItem(T item) => BinarySearch(item, true).node.Pair;
        public (T From, T ToExclusive) UpperBoundItem(T item) => BinarySearch(item, false).node.Pair;
        #endregion Search


        #region ICollection<T> members
        void ICollection<(T From, T ToExclusive)>.Add((T From, T ToExclusive) item) => Add(item.From, item.ToExclusive);
        public bool Add(T from, T toExclusive)
        {
            Node left = null, right = null;
            if (FindNodeLowerBound(comparer.Decrement(from)) is { } n)
            {
                if (comparer.Compare(from, n.From) >= 0)
                {
                    if (comparer.Compare(toExclusive, n.ToExclusive) <= 0)
                        return false;
                    left = n;
                    right = FindNodeLowerBound(toExclusive);
                    if (right != null && comparer.Compare(toExclusive, right.From) < 0)
                        right = null;
                }
                else if (comparer.Compare(toExclusive, n.ToExclusive) <= 0 && comparer.Compare(toExclusive, n.From) >= 0)
                {
                    right = n;
                }
            }
            if (left != null && right != null)
            {
                var pt = right.ToExclusive;
                Remove(left.ToExclusive, pt);
                left.ToExclusive = pt;
            }
            else
            {
                Remove(comparer.Increment(from), comparer.Decrement(toExclusive));
                if (left != null)
                    left.ToExclusive = toExclusive;
                else if (right != null)
                    right.From = from;
                else
                    AddImpl(from, toExclusive);
            }
            return true;
        }
        private void AddImpl(T from, T toExclusive)
        {
            if (root == null)
            {
                root = new Node(from, toExclusive, NodeColor.Black);
                return;
            }

            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;
            int order = 0;
            while (current != null)
            {
                order = comparer.Compare(from, current.From);
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
            Node node = new Node(from, toExclusive, NodeColor.Red);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.ColorBlack();
            return;
        }
        bool ICollection<(T From, T ToExclusive)>.Remove((T From, T ToExclusive) item) => Remove(item.From, item.ToExclusive);
        public bool Remove(T from, T toExclusive)
        {
            if (root == null) return false;
            bool resultMatch = false;
            bool foundMatch = true;
            while (foundMatch)
            {
                foundMatch = false;
                Node current = root;
                Node parent = null;
                Node grandParent = null;
                Node match = null;
                Node parentOfMatch = null;
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
                                if (parent.Right == sibling) parent.RotateLeft();
                                else parent.RotateRight();

                                parent.ColorRed();
                                sibling.ColorBlack();
                                ReplaceChildOrRoot(grandParent, parent, sibling);
                                grandParent = sibling;
                                if (parent == match) parentOfMatch = sibling;
                                sibling = (Node)parent.GetSibling(current);
                            }
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
                    int order = foundMatch ? -1 : comparer.Compare(from, current.From);
                    if (!foundMatch && order <= 0 && comparer.Compare(toExclusive, current.ToExclusive) >= 0)
                    {
                        resultMatch = foundMatch = true;
                        match = current;
                        parentOfMatch = parent;
                        order = 0;
                    }
                    grandParent = parent;
                    parent = current;
                    current = (Node)(order < 0 ? current.Left : current.Right);
                }
                if (match != null)
                {
                    ReplaceNode(match, parentOfMatch, parent, grandParent);
                }
            }
            {
                var match = FindNodeLowerBound(from);
                if (match != null)
                {
                    int order = comparer.Compare(from, match.From);
                    if (order <= 0)
                    {
                        if (comparer.Compare(toExclusive, match.From) > 0) // 右側
                        {
                            resultMatch = true;
                            match.From = toExclusive;
                        }
                    }
                    else
                    {
                        if (comparer.Compare(toExclusive, match.ToExclusive) >= 0) // 左側
                        {
                            resultMatch = true;
                            match.ToExclusive = from;

                            match = FindNodeLowerBound(toExclusive);
                            if (match != null && comparer.Compare(toExclusive, match.From) > 0)
                                match.From = toExclusive;
                        }
                        else // 分割
                        {
                            resultMatch = true;
                            var prevTo = match.ToExclusive;
                            match.ToExclusive = from;
                            root?.ColorBlack();
                            AddImpl(toExclusive, prevTo);
                        }
                    }
                }
            }
            root?.ColorBlack();
            return resultMatch;
        }

        bool ICollection<(T From, T ToExclusive)>.Contains((T From, T ToExclusive) item) => Contains(item.From, item.ToExclusive);
        public bool Contains(T from, T toExclusive)
        {
            var node = FindNode(from);
            return node != null && comparer.Compare(toExclusive, node.ToExclusive) <= 0;
        }
        public bool Contains(T item) => FindNode(item) != null;
        #endregion ICollection<T> members

        [MethodImpl(AggressiveInlining)]
        int Compare(T value, T from, T toExclusive)
        {
            int forder = comparer.Compare(from, value);
            if (forder > 0) return -1;
            int torder = comparer.Compare(toExclusive, value);
            if (torder > 0)
                return 0;
            return 1;
        }
        bool ICollection<(T From, T ToExclusive)>.IsReadOnly => false;
        public class Node : SetNodeBase
        {
            public T From;
            public T ToExclusive;
            public (T From, T ToExclusive) Pair => (From, ToExclusive);
            internal Node(T from, T toExclusive, NodeColor color) : base(color)
            {
                this.From = from;
                this.ToExclusive = toExclusive;
            }
            public override string ToString() => $"Range = [{From}, {ToExclusive}), Size = {Size}";
        }
        public struct NodeOperator : INodeOperator<(T From, T ToExclusive), Node>
        {
            private readonly TOp comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [MethodImpl(AggressiveInlining)]
            public Node Create((T From, T ToExclusive) item, NodeColor color) => new Node(item.From, item.ToExclusive, color);
            [MethodImpl(AggressiveInlining)]
            public (T From, T ToExclusive) GetValue(Node node) => node.Pair;
            public int Compare((T From, T ToExclusive) node1, (T From, T ToExclusive) node2) => comparer.Compare(node1.From, node2.From);
        }
    }
}
