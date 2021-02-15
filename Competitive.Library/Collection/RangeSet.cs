using AtCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    public class RangeSetLong : RangeSet<long, LongOperator>
    {
        public RangeSetLong() : base() { }
        public RangeSetLong(IEnumerable<(long From, long ToExclusive)> collection) : base(collection) { }
    }
    public class RangeSetInt : RangeSet<int, IntOperator>
    {
        public RangeSetInt() : base() { }
        public RangeSetInt(IEnumerable<(int From, int ToExclusive)> collection) : base(collection) { }
    }

    [DebuggerTypeProxy(typeof(RangeSet<,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class RangeSet<T, TOp> : ICollection<(T From, T ToExclusive)>, IReadOnlyCollection<(T From, T ToExclusive)>
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
        public RangeSet(TOp comparer)
        {
            this.comparer = comparer;
        }
        public RangeSet(IEnumerable<(T From, T ToExclusive)> collection, TOp comparer)
        {
            this.comparer = comparer;
            var arr = InitArray(collection);
            this.root = ConstructRootFromSortedArray(arr, 0, arr.Length - 1, null);
        }

        public (T From, T ToExclusive) Min
        {
            get
            {
                if (root == null) return default;
                var cur = root;
                while (cur.Left != null) { cur = cur.Left; }
                return cur.Pair;
            }
        }
        public (T From, T ToExclusive) Max
        {
            get
            {
                if (root == null) return default;
                var cur = root;
                while (cur.Right != null) { cur = cur.Right; }
                return cur.Pair;
            }
        }
        public Node FindNode(T item)
        {
            Node current = root;
            while (current != null)
            {
                int order = comparer.Compare(item, current.From);
                if (order == 0) return current;
                else if (order < 0)
                    current = current.Left;
                else
                {
                    order = comparer.Compare(item, current.ToExclusive);
                    if (order == 0) return null;
                    else if (order < 0) return current;
                    current = current.Right;
                }
            }
            return null;
        }
        public int Index(Node node)
        {
            var ret = NodeSize(node.Left);
            Node prev = node;
            node = node.Parent;
            while (prev != root)
            {
                if (node.Left != prev)
                {
                    ret += NodeSize(node.Left) + 1;
                }
                prev = node;
                node = node.Parent;
            }
            return ret;
        }
        public Node FindByIndex(int index)
        {
            var current = root; var currentIndex = current.Size - NodeSize(current.Right) - 1;
            while (currentIndex != index)
            {
                if (currentIndex > index)
                {
                    current = current.Left;
                    if (current == null) break;
                    currentIndex -= NodeSize(current.Right) + 1;
                }
                else
                {
                    current = current.Right;
                    if (current == null) break;
                    currentIndex += NodeSize(current.Left) + 1;
                }
            }
            return current;
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
                var forder = comparer.Compare(item, current.From);
                if (forder < 0 || (isLowerBound && comparer.Compare(item, current.ToExclusive) < 0))
                {
                    right = current;
                    ri = ci;
                    current = current.Left;
                    if (current != null)
                        ci -= NodeSize(current.Right) + 1;
                    else break;
                }
                else
                {
                    current = current.Right;
                    if (current != null)
                        ci += NodeSize(current.Left) + 1;
                    else break;
                }
            }
            return (right, ri);
        }
        public Node FindNodeLowerBound(T item) => BinarySearch(item, true).node;
        public Node FindNodeUpperBound(T item) => BinarySearch(item, false).node;
        public (T From, T ToExclusive) LowerBoundItem(T item) => BinarySearch(item, true).node.Pair;
        public (T From, T ToExclusive) UpperBoundItem(T item) => BinarySearch(item, false).node.Pair;
        public int LowerBoundIndex(T item) => BinarySearch(item, true).index;
        public int UpperBoundIndex(T item) => BinarySearch(item, false).index;

        public IEnumerable<(T From, T ToExclusive)> Reversed()
        {
            var e = new ValueEnumerator(this, true, null);
            while (e.MoveNext()) yield return e.Current;
        }

        /// <summary>
        /// <paramref name="from"/> 以上の要素を列挙する。<paramref name="from"/> がnullならばすべて列挙する。
        /// </summary>
        /// <param name="reverse">以上ではなく以下を列挙する</param>
        /// <returns></returns>
        public IEnumerable<(T From, T ToExclusive)> EnumerateItem(Node from = null, bool reverse = false)
        {
            var e = new ValueEnumerator(this, reverse, from);
            while (e.MoveNext()) yield return e.Current;
        }
        /// <summary>
        /// <paramref name="from"/> 以上のノードを列挙する。<paramref name="from"/> がnullならばすべて列挙する。
        /// </summary>
        /// <param name="reverse">以上ではなく以下を列挙する</param>
        /// <returns></returns>
        public IEnumerable<Node> EnumerateNode(Node from = null, bool reverse = false)
        {
            var e = new Enumerator(this, reverse, from);
            while (e.MoveNext()) yield return e.Current;
        }
        protected (T From, T ToExclusive)[] InitArray(IEnumerable<(T From, T ToExclusive)> collection)
        {
            var list = new List<(T From, T ToExclusive)>(
                collection.Where(t => comparer.Compare(t.From, t.ToExclusive) < 0));
            if (list.Count == 0) return Array.Empty<(T From, T ToExclusive)>();

            list.Sort((t1, t2) => comparer.Compare(t1.From, t2.From));
            var resList = new AtCoder.Internal.SimpleList<(T From, T ToExclusive)>(list.Count)
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

            return resList.AsSpan().ToArray();
        }

        public int Count => NodeSize(root);
        protected static int NodeSize(Node node) => node == null ? 0 : node.Size;
        protected readonly TOp comparer;
        bool ICollection<(T From, T ToExclusive)>.IsReadOnly => false;

        Node root;

        static Node ConstructRootFromSortedArray((T From, T ToExclusive)[] arr, int startIndex, int endIndex, Node redNode)
        {
            int size = endIndex - startIndex + 1;
            Node root;

            switch (size)
            {
                case 0:
                    return null;
                case 1:
                    root = new Node(arr[startIndex].From, arr[startIndex].ToExclusive, false);
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 2:
                    root = new Node(arr[startIndex].From, arr[startIndex].ToExclusive, false)
                    {
                        Right = new Node(arr[endIndex].From, arr[endIndex].ToExclusive, true)
                    };
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 3:
                    root = new Node(arr[startIndex + 1].From, arr[startIndex + 1].ToExclusive, false)
                    {
                        Left = new Node(arr[startIndex].From, arr[startIndex].ToExclusive, false),
                        Right = new Node(arr[endIndex].From, arr[endIndex].ToExclusive, false)
                    };
                    if (redNode != null)
                    {
                        root.Left.Left = redNode;
                    }
                    break;
                default:
                    int midpt = ((startIndex + endIndex) / 2);
                    root = new Node(arr[midpt].From, arr[midpt].ToExclusive, false)
                    {
                        Left = ConstructRootFromSortedArray(arr, startIndex, midpt - 1, redNode),
                        Right = size % 2 == 0 ?
                        ConstructRootFromSortedArray(arr, midpt + 2, endIndex, new Node(arr[midpt + 1].From, arr[midpt + 1].ToExclusive, true)) :
                        ConstructRootFromSortedArray(arr, midpt + 1, endIndex, null)
                    };
                    break;
            }
            return root;
        }
        void ICollection<(T From, T ToExclusive)>.Add((T From, T ToExclusive) item) => Add(item);
        public bool Add((T From, T ToExclusive) item) => Add(item.From, item.ToExclusive);
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
                root = new Node(from, toExclusive, false);
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
                if (Is4Node(current))
                {
                    Split4Node(current);
                    if (IsNonNullRed(parent) == true)
                    {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = (order < 0) ? current.Left : current.Right;
            }
            Node node = new Node(from, toExclusive, true);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.IsRed = false;
            return;
        }
        /// <summary>
        /// 該当ノードを削除する。動作怪しいかも
        /// </summary>
        public void Remove(Node node)
        {
            Node match = node;
            Node parentOfMatch = node.Parent;
            Node current = node;
            Node parent = parentOfMatch;
            Node grandParent = parentOfMatch?.Parent;
            while (current != null)
            {
                if (Is2Node(current))
                {
                    if (parent == null)
                    {
                        current.IsRed = true;
                    }
                    else
                    {
                        Node sibling = GetSibling(current, parent);
                        if (sibling.IsRed)
                        {
                            if (parent.Right == sibling) RotateLeft(parent);
                            else RotateRight(parent);

                            parent.IsRed = true;
                            sibling.IsRed = false;
                            ReplaceChildOrRoot(grandParent, parent, sibling);
                            grandParent = sibling;
                            if (parent == match) parentOfMatch = sibling;
                            sibling = (parent.Left == current) ? parent.Right : parent.Left;
                        }
                        if (Is2Node(sibling))
                        {
                            Merge2Nodes(parent);
                        }
                        else
                        {
                            TreeRotation rotation = GetRotation(parent, current, sibling);
                            Node newGrandParent = Rotate(parent, rotation);
                            newGrandParent.IsRed = parent.IsRed;
                            parent.IsRed = false;
                            current.IsRed = true;
                            ReplaceChildOrRoot(grandParent, parent, newGrandParent);
                            if (parent == match)
                            {
                                parentOfMatch = newGrandParent;
                            }
                        }
                    }
                }
                grandParent = parent;
                parent = current;
                current = current == match ? current.Right : current.Left;
            }
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
            }
            if (root != null)
            {
                root.IsRed = false;
            }
        }

        bool ICollection<(T From, T ToExclusive)>.Remove((T From, T ToExclusive) item) => this.Remove(item.From, item.ToExclusive);
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
                    if (Is2Node(current))
                    {
                        if (parent == null)
                        {
                            current.IsRed = true;
                        }
                        else
                        {
                            Node sibling = GetSibling(current, parent);
                            if (sibling.IsRed)
                            {
                                if (parent.Right == sibling) RotateLeft(parent);
                                else RotateRight(parent);

                                parent.IsRed = true;
                                sibling.IsRed = false;
                                ReplaceChildOrRoot(grandParent, parent, sibling);
                                grandParent = sibling;
                                if (parent == match) parentOfMatch = sibling;
                                sibling = (parent.Left == current) ? parent.Right : parent.Left;
                            }
                            if (Is2Node(sibling))
                            {
                                Merge2Nodes(parent);
                            }
                            else
                            {
                                TreeRotation rotation = GetRotation(parent, current, sibling);
                                Node newGrandParent = Rotate(parent, rotation);
                                newGrandParent.IsRed = parent.IsRed;
                                parent.IsRed = false;
                                current.IsRed = true;
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
                    current = order < 0 ? current.Left : current.Right;
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
                            root.IsRed = false;
                            AddImpl(toExclusive, prevTo);
                        }
                    }
                }
            }
            if (root != null)
            {
                root.IsRed = false;
            }
            return resultMatch;
        }
        public void Clear()
        {
            root = null;
        }
        bool ICollection<(T From, T ToExclusive)>.Contains((T From, T ToExclusive) item)
        {
            var node = FindNode(item.From);
            return node != null && comparer.Compare(item.ToExclusive, node.ToExclusive) <= 0;
        }
        public bool Contains(T item) => FindNode(item) != null;
        public void CopyTo((T From, T ToExclusive)[] array, int arrayIndex)
        {
            foreach (var item in this) array[arrayIndex++] = item;
        }

        public ValueEnumerator GetEnumerator() => new ValueEnumerator(this);
        IEnumerator<(T From, T ToExclusive)> IEnumerable<(T From, T ToExclusive)>.GetEnumerator() => new ValueEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new ValueEnumerator(this);

        #region private
        static bool Is2Node(Node node) => IsNonNullBlack(node) && IsNullOrBlack(node.Left) && IsNullOrBlack(node.Right); static bool Is4Node(Node node) => IsNonNullRed(node.Left) && IsNonNullRed(node.Right);
        static bool IsNonNullRed(Node node) => node != null && node.IsRed;
        static bool IsNonNullBlack(Node node) => node != null && !node.IsRed;
        static bool IsNullOrBlack(Node node) => node == null || !node.IsRed;
        void ReplaceNode(Node match, Node parentOfMatch, Node succesor, Node parentOfSuccesor)
        {
            if (succesor == match)
            {
                succesor = match.Left;
            }
            else
            {
                if (succesor.Right != null)
                {
                    succesor.Right.IsRed = false;
                }
                if (parentOfSuccesor != match)
                {
                    parentOfSuccesor.Left = succesor.Right;
                    succesor.Right = match.Right;
                }
                succesor.Left = match.Left;
            }
            if (succesor != null)
            {
                succesor.IsRed = match.IsRed;
            }
            ReplaceChildOrRoot(parentOfMatch, match, succesor);
        }
        static void Merge2Nodes(Node parent)
        {
            parent.IsRed = false;
            parent.Left.IsRed = true;
            parent.Right.IsRed = true;
        }
        static void Split4Node(Node node)
        {
            node.IsRed = true;
            node.Left.IsRed = false;
            node.Right.IsRed = false;
        }
        static Node GetSibling(Node node, Node parent)
        {
            return parent.Left == node ? parent.Right : parent.Left;
        }
        void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
        {
            bool parentIsOnRight = grandParent.Right == parent;
            bool currentIsOnRight = parent.Right == current;
            Node newChildOfGreatGrandParent;
            if (parentIsOnRight == currentIsOnRight)
            {
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeft(grandParent) : RotateRight(grandParent);
            }
            else
            {
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent);
                parent = greatGrandParent;
            }
            grandParent.IsRed = true;
            newChildOfGreatGrandParent.IsRed = false;
            ReplaceChildOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent);

        }
        static Node Rotate(Node node, TreeRotation rotation)
        {
            switch (rotation)
            {
                case TreeRotation.Right:
                    node.Left.Left.IsRed = false;
                    return RotateRight(node);
                case TreeRotation.Left:
                    node.Right.Right.IsRed = false;
                    return RotateLeft(node);
                case TreeRotation.RightLeft:
                    return RotateRightLeft(node);
                case TreeRotation.LeftRight:
                    return RotateLeftRight(node);
                default:
                    throw new InvalidOperationException();
            }
        }
        static Node RotateLeft(Node node)
        {
            Node child = node.Right;
            node.Right = child.Left;
            child.Left = node;
            return child;
        }
        static Node RotateLeftRight(Node node)
        {
            Node child = node.Left;
            Node grandChild = child.Right;

            node.Left = grandChild.Right;
            grandChild.Right = node;
            child.Right = grandChild.Left;
            grandChild.Left = child;
            return grandChild;
        }
        static Node RotateRight(Node node)
        {
            Node child = node.Left;
            node.Left = child.Right;
            child.Right = node;
            return child;
        }
        static Node RotateRightLeft(Node node)
        {
            Node child = node.Right;
            Node grandChild = child.Left;

            node.Right = grandChild.Left;
            grandChild.Left = node;
            child.Left = grandChild.Right;
            grandChild.Right = child;
            return grandChild;
        }
        void ReplaceChildOrRoot(Node parent, Node child, Node newChild)
        {
            if (parent != null)
            {
                if (parent.Left == child)
                {
                    parent.Left = newChild;
                }
                else
                {
                    parent.Right = newChild;
                }
            }
            else
            {
                root = newChild;
            }
        }
        static TreeRotation GetRotation(Node parent, Node current, Node sibling)
        {
            if (IsNonNullRed(sibling.Left))
            {
                if (parent.Left == current)
                {
                    return TreeRotation.RightLeft;
                }
                return TreeRotation.Right;
            }
            else
            {
                if (parent.Left == current)
                {
                    return TreeRotation.Left;
                }
                return TreeRotation.LeftRight;
            }
        }
        #endregion private

        public struct ValueEnumerator : IEnumerator<(T From, T ToExclusive)>
        {
            private Enumerator inner;
            internal ValueEnumerator(RangeSet<T, TOp> set)
            {
                inner = new Enumerator(set);
            }
            internal ValueEnumerator(RangeSet<T, TOp> set, bool reverse, Node startNode)
            {
                inner = new Enumerator(set, reverse, startNode);
            }

            public (T From, T ToExclusive) Current => inner.Current.Pair;
            object IEnumerator.Current => Current;

            public void Dispose() { }
            public bool MoveNext() => inner.MoveNext();
            public void Reset() => throw new NotSupportedException();
        }
        public struct Enumerator : IEnumerator<Node>
        {

            readonly RangeSet<T, TOp> tree;
            readonly Stack<Node> stack;
            Node current;

            readonly bool reverse;
            internal Enumerator(RangeSet<T, TOp> set) : this(set, false, null) { }
            internal Enumerator(RangeSet<T, TOp> set, bool reverse, Node startNode)
            {
                tree = set;
                stack = new Stack<Node>(2 * Log2(tree.Count + 1));
                current = null;
                this.reverse = reverse;
                if (startNode == null) IntializeAll();
                else Intialize(startNode);

            }
            void IntializeAll()
            {
                var node = tree.root;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node); node = next;
                }
            }
            void Intialize(Node startNode)
            {
                if (startNode == null)
                    throw new InvalidOperationException(nameof(startNode) + "is null");
                current = null;
                var node = startNode;
                var list = new List<Node>(Log2(tree.Count + 1));
                var comparer = tree.comparer;

                if (reverse)
                {
                    while (node != null)
                    {
                        list.Add(node);
                        var parent = node.Parent;
                        if (parent == null || parent.Left == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        var parent = node.Parent;
                        if (parent == null || parent.Right == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        if (comparer.Compare(startNode.From, node.From) >= 0)
                            list.Add(node);
                        node = node.Parent;
                    }
                }
                else
                {
                    while (node != null)
                    {
                        list.Add(node);
                        var parent = node.Parent;
                        if (parent == null || parent.Right == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        var parent = node.Parent;
                        if (parent == null || parent.Left == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        if (comparer.Compare(startNode.From, node.From) <= 0)
                            list.Add(node);
                        node = node.Parent;
                    }
                }

                list.Reverse();
                foreach (var n in list) stack.Push(n);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public Node Current => current;

            public bool MoveNext()
            {
                if (stack.Count == 0)
                {
                    current = null; return false;
                }
                current = stack.Pop();
                var node = reverse ? current.Left : current.Right;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node);
                    node = next;
                }
                return true;
            }

            object IEnumerator.Current => this.Current;
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();

        }
        public class Node
        {
            public bool IsRed;
            public T From;
            public T ToExclusive;
            public (T From, T ToExclusive) Pair => (From, ToExclusive);
            public Node Parent
            {
                get; private set;
            }
            Node _left;
            public Node Left
            {
                get
                {
                    return _left;
                }
                set
                {
                    _left = value; if (value != null) value.Parent = this;
                    for (var cur = this; cur != null; cur = cur.Parent)
                    {
                        if (!cur.UpdateSize()) break;
                        if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur)
                        {
                            cur.Parent = null;
                            break;
                        }
                    }
                }
            }
            Node _right;
            public Node Right
            {
                get
                {
                    return _right;
                }
                set
                {
                    _right = value;
                    if (value != null) value.Parent = this;
                    for (var cur = this; cur != null; cur = cur.Parent)
                    {
                        if (!cur.UpdateSize()) break;
                        if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur)
                        {
                            cur.Parent = null; break;
                        }
                    }
                }
            }

            public int Size
            {
                get; private set;
            } = 1;
            public Node(T from, T toExclusive, bool isRed)
            {
                this.From = from;
                this.ToExclusive = toExclusive;
                this.IsRed = isRed;
            }
            public bool UpdateSize()
            {
                var oldsize = this.Size;
                var size = 1;
                if (Left != null) size += Left.Size;
                if (Right != null) size += Right.Size;
                this.Size = size;
                return oldsize != size;
            }
            public override string ToString() => $"Range = [{From}, {ToExclusive}), Size = {Size}";
        }
        enum TreeRotation : byte
        {
            Left = 1,
            Right = 2,
            RightLeft = 3,
            LeftRight = 4,
        }
        private class DebugView
        {
            private readonly ICollection<(T From, T ToExclusive)> collection;
            public DebugView(RangeSet<T, TOp> collection)
            {
                this.collection = collection;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public (T From, T ToExclusive)[] Items => collection.ToArray();
        }
    }

}
