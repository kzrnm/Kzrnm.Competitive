using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.SetInternals
{
    public interface ISetOperator<T, TCmp, Node> : IComparer<T>
    {
        Node Create(T item, NodeColor color);
        T GetValue(Node node);
        TCmp GetCompareKey(T item);
        int Compare(TCmp value, Node node);
    }
    public enum NodeColor : byte
    {
        Black,
        Red
    }
    enum TreeRotation : byte
    {
        Left = 1,
        Right = 2,
        RightLeft = 3,
        LeftRight = 4,
    }

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

    public struct SetLowerBoundOperator : ISetBinarySearchOperator
    {
        public bool ReturnLeft => false;
        [凾(256)]
        public bool IntoLeft(int order) => order <= 0;
    }
    public struct SetUpperBoundOperator : ISetBinarySearchOperator
    {
        public bool ReturnLeft => false;
        [凾(256)]
        public bool IntoLeft(int order) => order < 0;
    }
    public struct SetLowerBoundReverseOperator : ISetBinarySearchOperator
    {
        public bool ReturnLeft => true;
        [凾(256)]
        public bool IntoLeft(int order) => order < 0;
    }
    public struct SetUpperBoundReverseOperator : ISetBinarySearchOperator
    {
        public bool ReturnLeft => true;
        [凾(256)]
        public bool IntoLeft(int order) => order <= 0;
    }

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public abstract class SetBase<T, TCmp, Node, TOp> : ICollection, ICollection<T>, IReadOnlyCollection<T>
        where Node : SetNodeBase<Node>
        where TOp : struct, ISetOperator<T, TCmp, Node>
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

        private readonly TOp op;

        public bool IsMulti { get; }
        protected Node root;
        #region Constructor
        protected SetBase(bool isMulti, TOp op)
        {
            this.IsMulti = isMulti;
            this.op = op;
        }
        protected SetBase(bool isMulti, TOp op, IEnumerable<T> collection) : this(isMulti, op)
        {
            var arr = InitArray(collection);
            this.root = ConstructRootFromSortedArray(arr, null);
        }
        protected virtual ReadOnlySpan<T> InitArray(IEnumerable<T> collection)
        {
            T[] arr;
            int count;
            if (IsMulti)
            {
                arr = collection.ToArray();
                Array.Sort(arr, op);
                count = arr.Length;
            }
            else
            {
                arr = collection.ToArray();
                if (arr.Length == 0) return arr;
                count = 1;
                Array.Sort(arr, op);
                for (int i = 1; i < arr.Length; i++)
                {
                    if (op.Compare(arr[i], arr[i - 1]) != 0)
                    {
                        arr[count++] = arr[i];
                    }
                }
            }
            return arr.AsSpan(0, count);
        }
        protected virtual Node ConstructRootFromSortedArray(ReadOnlySpan<T> arr, Node redNode)
        {
            Node root;
            switch (arr.Length)
            {
                case 0:
                    return null;
                case 1:
                    root = op.Create(arr[0], NodeColor.Black);
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 2:
                    root = op.Create(arr[0], NodeColor.Black);
                    root.Right = op.Create(arr[^1], NodeColor.Red);
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 3:
                    root = op.Create(arr[1], NodeColor.Black);
                    root.Left = op.Create(arr[0], NodeColor.Black);
                    root.Right = op.Create(arr[^1], NodeColor.Black);
                    if (redNode != null)
                    {
                        root.Left.Left = redNode;
                    }
                    break;
                default:
                    int midpt = (arr.Length - 1) / 2;
                    root = op.Create(arr[midpt], NodeColor.Black);
                    root.Left = ConstructRootFromSortedArray(arr[..midpt], redNode);
                    root.Right = arr.Length % 2 == 0 ?
                        ConstructRootFromSortedArray(arr[(midpt + 2)..], op.Create(arr[midpt + 1], NodeColor.Red)) :
                        ConstructRootFromSortedArray(arr[(midpt + 1)..], null);
                    break;
            }
            return root;
        }
        #endregion Constructor
        [凾(256)]
        internal Node MinNode()
        {
            if (root == null) return null;
            var cur = root;
            while (cur.Left != null) { cur = cur.Left; }
            return cur;
        }
        [凾(256)]
        internal Node MaxNode()
        {
            if (root == null) return null;
            var cur = root;
            while (cur.Right != null) { cur = cur.Right; }
            return cur;
        }
        public T Min => MinNode() switch { { } n => op.GetValue(n), _ => default(T) };
        public T Max => MaxNode() switch { { } n => op.GetValue(n), _ => default(T) };

        #region Search
        [凾(256)]
        public Node FindNode(TCmp item)
        {
            Node current = root;
            while (current != null)
            {
                int order = op.Compare(item, current);
                if (order == 0) return current;
                current = order < 0 ? current.Left : current.Right;
            }
            return null;
        }

        [凾(256)]
        public int Index(Node node)
        {
            var _node = node;
            var ret = NodeSize(_node.Left);
            var prev = _node;
            _node = _node.Parent;
            while (prev != root)
            {
                if (_node.Left != prev)
                {
                    ret += NodeSize(_node.Left) + 1;
                }
                prev = _node;
                _node = _node.Parent;
            }
            return ret;
        }

        [凾(256)]
        public Node FindByIndex(int index)
        {
            var current = root;
            var currentIndex = current.Size - NodeSize(current.Right) - 1;
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

        /// <summary>
        /// <paramref name="item"/> 以上/超えるの要素のノードとインデックスを返します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <param name="bop">二分探索の判定オペレーター</param>
        [凾(256)]
        public (Node node, int index) BinarySearch<TBOp>(TCmp item, TBOp bop = default)
               where TBOp : struct, ISetBinarySearchOperator
        {
            Node left = null, right = null;
            Node current = root;
            if (current == null) return (null, 0);
            int li = -1;
            int ri = Count;
            int ci = NodeSize(current.Left);
            while (true)
            {
                if (bop.IntoLeft(op.Compare(item, current)))
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
                    left = current;
                    li = ci;
                    current = current.Right;
                    if (current != null)
                        ci += NodeSize(current.Left) + 1;
                    else break;
                }
            }
            return bop.ReturnLeft ? (left, li) : (right, ri);
        }
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)]
        public Node FindNodeLowerBound(TCmp item) => BinarySearch<SetLowerBoundOperator>(item).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int LowerBoundIndex(TCmp item) => BinarySearch<SetLowerBoundOperator>(item).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素を返します。
        /// </summary>
        [凾(256)]
        public T LowerBoundItem(TCmp item) => op.GetValue(BinarySearch<SetLowerBoundOperator>(item).node);
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)]
        public Node FindNodeUpperBound(TCmp item) => BinarySearch<SetUpperBoundOperator>(item).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int UpperBoundIndex(TCmp item) => BinarySearch<SetUpperBoundOperator>(item).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素を返します。
        /// </summary>
        [凾(256)]
        public T UpperBoundItem(TCmp item) => op.GetValue(BinarySearch<SetUpperBoundOperator>(item).node);

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)]
        public Node FindNodeReverseLowerBound(TCmp item) => BinarySearch<SetLowerBoundReverseOperator>(item).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int ReverseLowerBoundIndex(TCmp item) => BinarySearch<SetLowerBoundReverseOperator>(item).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素を返します。
        /// </summary>
        [凾(256)]
        public T ReverseLowerBoundItem(TCmp item) => op.GetValue(BinarySearch<SetLowerBoundReverseOperator>(item).node);

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)]
        public Node FindNodeReverseUpperBound(TCmp item) => BinarySearch<SetUpperBoundReverseOperator>(item).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)]
        public int ReverseUpperBoundIndex(TCmp item) => BinarySearch<SetUpperBoundReverseOperator>(item).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素を返します。
        /// </summary>
        [凾(256)]
        public T ReverseUpperBoundItem(TCmp item) => op.GetValue(BinarySearch<SetUpperBoundReverseOperator>(item).node);
        #endregion Search

        #region Enumerate
        [凾(256)]
        public IEnumerable<T> Reversed()
        {
            var e = new ValueEnumerator(this, true, null);
            while (e.MoveNext()) yield return e.Current;
        }

        /// <summary>
        /// <paramref name="from"/> 以上/以下の要素を列挙する。<paramref name="from"/> がnullならばすべて列挙する。
        /// </summary>
        /// <param name="from">列挙開始するノードの値</param>
        /// <param name="reverse">以上ではなく以下を列挙する</param>
        /// <returns></returns>
        [凾(256)]
        public IEnumerable<T> EnumerateItem(Node from = null, bool reverse = false)
        {
            var e = new ValueEnumerator(this, reverse, from);
            while (e.MoveNext()) yield return e.Current;
        }
        /// <summary>
        /// <paramref name="from"/> 以上/以下のノードを列挙する。<paramref name="from"/> がnullならばすべて列挙する。
        /// </summary>
        /// <param name="from">列挙開始するノードの値</param>
        /// <param name="reverse">以上ではなく以下を列挙する</param>
        /// <returns></returns>
        [凾(256)]
        public IEnumerable<Node> EnumerateNode(Node from = null, bool reverse = false)
        {
            var e = new Enumerator(this, reverse, from);
            while (e.MoveNext()) yield return e.Current;
        }
        #endregion Enumerate

        #region ICollection<T> members
        void ICollection<T>.Add(T item) => DoAdd(item);

        [凾(256)]
        public bool Add(T item) => DoAdd(item);
        protected bool DoAdd(T item)
        {
            var key = op.GetCompareKey(item);
            if (root == null)
            {
                root = op.Create(item, NodeColor.Black);
                return true;
            }
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;
            int order = 0;
            while (current != null)
            {
                order = op.Compare(key, current);
                if (order == 0 && !this.IsMulti)
                {
                    root.ColorBlack();
                    return false;
                }
                if (current.Is4Node)
                {
                    current.Split4Node();
                    if (parent.IsNonNullRed())
                    {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = order < 0 ? current.Left : current.Right;
            }
            Node node = op.Create(item, NodeColor.Red);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.ColorBlack();
            return true;
        }

        protected void Fix2Node(Node match, ref Node parentOfMatch, Node current, Node parent, Node grandParent)
        {
            Debug.Assert(current.Is2Node);
            if (parent == null)
            {
                current.ColorRed();
            }
            else
            {
                var sibling = parent.GetSibling(current);
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
                    sibling = parent.GetSibling(current);
                }
                Debug.Assert(sibling.IsNonNullBlack());
                if (sibling.Is2Node)
                {
                    parent.Merge2Nodes();
                }
                else
                {
                    Node newGrandParent = parent.Rotate(parent.GetRotation(current, sibling));
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

        /// <summary>
        /// 該当ノードを削除する。動作怪しい
        /// </summary>
        internal void Remove(Node node)
            => RemoveMatch(
                match: node,
                parentOfMatch: node.Parent,
                current: node,
                parent: node.Parent,
                grandParent: node.Parent?.Parent);

        private void RemoveMatch(Node match, Node parentOfMatch, Node current, Node parent, Node grandParent)
        {
            while (current != null)
            {
                if (current.Is2Node)
                    Fix2Node(match, ref parentOfMatch, current, parent, grandParent);
                grandParent = parent;
                parent = current;
                current = current != match ? current.Left : current.Right;
            }
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
            }
            root?.ColorBlack();
        }
        bool ICollection<T>.Remove(T item) => DoRemove(op.GetCompareKey(item));
        [凾(256)]
        public bool Remove(TCmp item) => DoRemove(item);
        protected bool DoRemove(TCmp item)
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
                    Fix2Node(match, ref parentOfMatch, current, parent, grandParent);
                int order = foundMatch ? -1 : op.Compare(item, current);
                if (order == 0)
                {
                    foundMatch = true;
                    match = current;
                    parentOfMatch = parent;
                }
                grandParent = parent;
                parent = current;
                current = order < 0 ? current.Left : current.Right;
            }
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
            }
            root?.ColorBlack();
            return foundMatch;
        }
        public void Clear()
        {
            root = null;
        }
        public bool Contains(T item) => Contains(op.GetCompareKey(item));
        public bool Contains(TCmp item) => FindNode(item) != null;
        void ICollection.CopyTo(Array array, int index) => CopyTo((T[])array, index);
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this) array[arrayIndex++] = item;
        }

        #endregion ICollection<T> members

        #region private
        protected void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
        {
            Debug.Assert(parent != null);
            Debug.Assert(grandParent != null);
            bool parentIsOnRight = grandParent.Right == parent;
            bool currentIsOnRight = parent.Right == current;
            Node newChildOfGreatGrandParent;
            if (parentIsOnRight == currentIsOnRight)
            {
                newChildOfGreatGrandParent = currentIsOnRight ? grandParent.RotateLeft() : grandParent.RotateRight();
            }
            else
            {
                newChildOfGreatGrandParent = currentIsOnRight ? grandParent.RotateLeftRight() : grandParent.RotateRightLeft();
                parent = greatGrandParent;
            }
            grandParent.ColorRed();
            newChildOfGreatGrandParent.ColorBlack();
            ReplaceChildOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent);

        }
        [凾(256)]
        protected void ReplaceChildOrRoot(Node parent, Node child, Node newChild)
        {
            if (parent != null)
                parent.ReplaceChild(child, newChild);
            else
            {
                root = newChild;
                if (root != null) root.Parent = null;
            }
        }
        [凾(256)]
        protected void ReplaceNode(Node match, Node parentOfMatch, Node successor, Node parentOfSuccessor)
        {
            Debug.Assert(match != null);
            if (successor == match)
            {
                Debug.Assert(match.Right == null);
                successor = match.Left;
            }
            else
            {
                Debug.Assert(parentOfSuccessor != null);
                Debug.Assert(successor.Left == null);
                Debug.Assert((successor.Right == null && successor.IsRed) || (successor.Right.IsRed && successor.IsBlack));

                successor.Right?.ColorBlack();

                if (parentOfSuccessor != match)
                {
                    parentOfSuccessor.Left = successor.Right;
                    successor.Right = match.Right;
                }
                successor.Left = match.Left;
            }
            if (successor != null)
            {
                successor.Color = match.Color;
            }
            ReplaceChildOrRoot(parentOfMatch, match, successor);
        }
        #endregion private
        bool ICollection<T>.IsReadOnly => false;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;
        public int Count => NodeSize(root);

        [凾(256)]
        internal static int NodeSize(Node node) => node?.Size ?? 0;


        [凾(256)]
        public ValueEnumerator GetEnumerator() => new ValueEnumerator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new ValueEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new ValueEnumerator(this);


        public struct Enumerator : IEnumerator<Node>
        {
            internal readonly SetBase<T, TCmp, Node, TOp> tree;
            readonly Stack<Node> stack;
            Node current;

            readonly bool reverse;
            internal Enumerator(SetBase<T, TCmp, Node, TOp> set) : this(set, false, null) { }
            internal Enumerator(SetBase<T, TCmp, Node, TOp> set, bool reverse, Node startNode)
            {
                tree = set;
                stack = new Stack<Node>(2 * Log2(tree.Count + 1));
                current = null;
                this.reverse = reverse;
                if (startNode == null) IntializeAll();
                else Intialize(startNode);

            }
            [凾(256)]
            void IntializeAll()
            {
                var node = tree.root;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node);
                    node = next;
                }
            }
            [凾(256)]
            void Intialize(Node startNode)
            {
                if (startNode == null)
                    throw new InvalidOperationException(nameof(startNode) + "is null");
                current = null;
                var list = reverse ? InitializeReverse(startNode) : InitializeNormal(startNode);

                list.Reverse();
                foreach (var n in list) stack.Push(n);
            }
            [凾(256)]
            SimpleList<Node> InitializeNormal(Node node)
            {
                var list = new SimpleList<Node>(2 * Log2(tree.Count + 1));

                while (node != null)
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
                }
                return list;
            }
            [凾(256)]
            SimpleList<Node> InitializeReverse(Node node)
            {
                var list = new SimpleList<Node>(2 * Log2(tree.Count + 1));

                while (node != null)
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
                }
                return list;
            }

            [凾(256)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public Node Current => current;
            [凾(256)]
            internal T CurrentValue() => tree.op.GetValue(current);

            [凾(256)]
            public bool MoveNext()
            {
                if (stack.Count == 0)
                {
                    current = null;
                    return false;
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

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();
        }
        public struct ValueEnumerator : IEnumerator<T>
        {
            private Enumerator inner;
            internal ValueEnumerator(SetBase<T, TCmp, Node, TOp> set)
            {
                inner = new Enumerator(set);
            }
            internal ValueEnumerator(SetBase<T, TCmp, Node, TOp> set, bool reverse, Node startNode)
            {
                inner = new Enumerator(set, reverse, startNode);
            }

            public T Current => inner.CurrentValue();
            object IEnumerator.Current => Current;

            public void Dispose() { }
            [凾(256)]
            public bool MoveNext() => inner.MoveNext();
            public void Reset() => throw new NotSupportedException();
        }
    }
    public static class SetNodeBaseExt
    {
        [凾(256)]
        public static bool IsNonNullBlack<TNode>(this TNode node) where TNode : SetNodeBase<TNode> => node != null && node.IsBlack;

        [凾(256)]
        public static bool IsNonNullRed<TNode>(this TNode node) where TNode : SetNodeBase<TNode> => node != null && node.IsRed;

        [凾(256)]
        public static bool IsNullOrBlack<TNode>(this TNode node) where TNode : SetNodeBase<TNode> => node == null || node.IsBlack;
    }
    public class SetNodeBase<TNode> where TNode : SetNodeBase<TNode>
    {
        internal SetNodeBase(NodeColor color)
        {
            Contract.Assert(this.GetType() == typeof(TNode));
            this.Color = color;
        }
        private TNode AsGeneric => Unsafe.As<TNode>(this);
        public TNode Parent { get; internal set; }
        TNode _left;
        public TNode Left
        {
            get => _left;
            set
            {
                _left = value; if (value != null) value.Parent = this.AsGeneric;
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
        TNode _right;
        public TNode Right
        {
            get => _right;
            set
            {
                _right = value;
                if (value != null) value.Parent = this.AsGeneric;
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
        [凾(256)]
        internal bool UpdateSize()
        {
            var oldsize = this.Size;
            var size = 1;
            if (Left != null) size += Left.Size;
            if (Right != null) size += Right.Size;
            this.Size = size;
            return oldsize != size;
        }
        internal NodeColor Color { get; set; }
        internal bool IsBlack => Color == NodeColor.Black;
        internal bool IsRed => Color == NodeColor.Red;
        internal bool Is2Node => IsBlack && Left.IsNullOrBlack() && Right.IsNullOrBlack();
        internal bool Is4Node => Left.IsNonNullRed() && Right.IsNonNullRed();
        [凾(256)]
        internal void ColorBlack() => Color = NodeColor.Black;
        [凾(256)]
        internal void ColorRed() => Color = NodeColor.Red;

        [凾(256)]
        internal TreeRotation GetRotation(TNode current, TNode sibling)
        {
            Debug.Assert(sibling.Left.IsNonNullRed() || sibling.Right.IsNonNullRed());
            bool currentIsLeftChild = Left == current;
            return sibling.Left.IsNonNullRed() ?
                (currentIsLeftChild ? TreeRotation.RightLeft : TreeRotation.Right) :
                (currentIsLeftChild ? TreeRotation.Left : TreeRotation.LeftRight);
        }

        [凾(256)]
        internal TNode GetSibling(TNode node)
        {
            Debug.Assert(node != null);
            Debug.Assert(node == Left ^ node == Right);

            return node == Left ? Right : Left;
        }
        [凾(256)]
        internal void Split4Node()
        {
            Debug.Assert(Left != null);
            Debug.Assert(Right != null);

            ColorRed();
            Left.ColorBlack();
            Right.ColorBlack();
        }
        [凾(256)]
        internal TNode Rotate(TreeRotation rotation)
        {
            TNode removeRed;
            switch (rotation)
            {
                case TreeRotation.Right:
                    removeRed = Left.Left;
                    Debug.Assert(removeRed.IsRed);
                    removeRed.ColorBlack();
                    return RotateRight();
                case TreeRotation.Left:
                    removeRed = Right.Right;
                    Debug.Assert(removeRed.IsRed);
                    removeRed.ColorBlack();
                    return RotateLeft();
                case TreeRotation.RightLeft:
                    Debug.Assert(Right.Left.IsRed);
                    return RotateRightLeft();
                case TreeRotation.LeftRight:
                    Debug.Assert(Left.Right.IsRed);
                    return RotateLeftRight();
                default:
                    return Throw();
            }
            static TNode Throw() => throw new InvalidOperationException();
        }
        [凾(256)]
        internal TNode RotateLeft()
        {
            TNode child = Right;
            Right = child.Left;
            child.Left = this.AsGeneric;
            return child;
        }
        [凾(256)]
        internal TNode RotateLeftRight()
        {
            TNode child = Left;
            TNode grandChild = child.Right;

            Left = grandChild.Right;
            grandChild.Right = this.AsGeneric;
            child.Right = grandChild.Left;
            grandChild.Left = child;
            return grandChild;
        }
        [凾(256)]
        internal TNode RotateRight()
        {
            TNode child = Left;
            Left = child.Right;
            child.Right = this.AsGeneric;
            return child;
        }
        [凾(256)]
        internal TNode RotateRightLeft()
        {
            TNode child = Right;
            TNode grandChild = child.Left;

            Right = grandChild.Left;
            grandChild.Left = this.AsGeneric;
            child.Left = grandChild.Right;
            grandChild.Right = child;
            return grandChild;
        }
        [凾(256)]
        internal void Merge2Nodes()
        {
            Debug.Assert(IsRed);
            Debug.Assert(Left!.Is2Node);
            Debug.Assert(Right!.Is2Node);

            // Combine two 2-nodes into a 4-node.
            ColorBlack();
            Left.ColorRed();
            Right.ColorRed();
        }
        [凾(256)]
        internal void ReplaceChild(TNode child, TNode newChild)
        {
            if (Left == child)
            {
                Left = newChild;
            }
            else
            {
                Right = newChild;
            }
        }
        public override string ToString() => $"Size = {Size}";
    }
}
