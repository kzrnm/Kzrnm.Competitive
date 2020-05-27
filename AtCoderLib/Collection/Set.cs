using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Linq;
using static AtCoderProject.Global;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;


[System.Diagnostics.DebuggerTypeProxy(typeof(ΔDebugView<>))]
[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
class Set<T> : ICollection<T>, IReadOnlyCollection<T>
{
    /*
     * Original
     *
     * Copyright (c) .NET Foundation and Contributors
     * Released under the MIT license
     * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
     */

    public virtual bool IsMulti => false;
    public T Min
    {
        get
        {
            if (root == null) return default;
            var cur = root;
            while (cur.Left != null) { cur = cur.Left; }
            return cur.Item;
        }
    }
    public T Max
    {
        get
        {
            if (root == null) return default;
            var cur = root;
            while (cur.Right != null) { cur = cur.Right; }
            return cur.Item;
        }
    }
    public Node FindNode(T item)
    {
        Node current = root;
        while (current != null)
        {
            int order = comparer.Compare(item, current.Item);
            if (order == 0) return current;
            current = order < 0 ? current.Left : current.Right;
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
        if (current == null) return (null, -1);
        int ri = -1;
        int ci = NodeSize(current.Left);
        while (true)
        {
            var order = comparer.Compare(item, current.Item);
            if (order < 0 || (isLowerBound && order == 0))
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
    public T LowerBoundItem(T item) => BinarySearch(item, true).node.Item;
    public T UpperBoundItem(T item) => BinarySearch(item, false).node.Item;
    public int LowerBoundIndex(T item) => BinarySearch(item, true).index;
    public int UpperBoundIndex(T item) => BinarySearch(item, false).index;

    public IEnumerable<T> Reversed()
    {
        var e = new Enumerator(this, true, null);
        while (e.MoveNext()) yield return e.Current;
    }
    public IEnumerable<T> Enumerate(Node from) => Enumerate(from, false);
    public IEnumerable<T> Enumerate(Node from, bool reverse)
    {
        if (from == null) yield break;
        var e = new Enumerator(this, reverse, from);
        while (e.MoveNext()) yield return e.Current;
    }

    public Set() : this(Comparer<T>.Default) { }
    public Set(IEnumerable<T> collection) : this(collection, Comparer<T>.Default) { }
    public Set(IComparer<T> comparer)
    {
        this.comparer = comparer;
    }
    public Set(IEnumerable<T> collection, IComparer<T> comparer)
    {
        this.comparer = comparer; var arr = InitArray(collection);
        this.root = ConstructRootFromSortedArray(arr, 0, arr.Length - 1, null);
    }
    protected T[] InitArray(IEnumerable<T> collection)
    {
        T[] arr;
        if (this.IsMulti)
        {
            arr = collection.ToArray();
            Array.Sort(arr, this.comparer);
        }
        else
        {
            var list = new List<T>(collection);
            list.Sort(this.comparer);
            for (int i = list.Count - 1; i > 0; i--)
                if (this.comparer.Compare(list[i - 1], list[i]) == 0)
                    list.RemoveAt(i);
            arr = list.ToArray();
        }
        return arr;
    }

    public int Count => NodeSize(root);
    protected static int NodeSize(Node node) => node == null ? 0 : node.Size;
    protected readonly IComparer<T> comparer;
    bool ICollection<T>.IsReadOnly => false;

    private Node root;

    private static Node ConstructRootFromSortedArray(T[] arr, int startIndex, int endIndex, Node redNode)
    {
        int size = endIndex - startIndex + 1;
        Node root;

        switch (size)
        {
            case 0:
                return null;
            case 1:
                root = new Node(arr[startIndex], false);
                if (redNode != null)
                {
                    root.Left = redNode;
                }
                break;
            case 2:
                root = new Node(arr[startIndex], false)
                {
                    Right = new Node(arr[endIndex], true)
                };
                if (redNode != null)
                {
                    root.Left = redNode;
                }
                break;
            case 3:
                root = new Node(arr[startIndex + 1], false)
                {
                    Left = new Node(arr[startIndex], false),
                    Right = new Node(arr[endIndex], false)
                };
                if (redNode != null)
                {
                    root.Left.Left = redNode;
                }
                break;
            default:
                int midpt = ((startIndex + endIndex) / 2);
                root = new Node(arr[midpt], false)
                {
                    Left = ConstructRootFromSortedArray(arr, startIndex, midpt - 1, redNode),
                    Right = size % 2 == 0 ?
                    ConstructRootFromSortedArray(arr, midpt + 2, endIndex, new Node(arr[midpt + 1], true)) :
                    ConstructRootFromSortedArray(arr, midpt + 1, endIndex, null)
                };
                break;
        }
        return root;
    }
    public void Add(T item)
    {
        if (root == null)
        {
            root = new Node(item, false);
            return;
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
                root.IsRed = false;
                return;
            }
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
        Node node = new Node(item, true);
        if (order >= 0) parent.Right = node;
        else parent.Left = node;
        if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
        root.IsRed = false;
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
            int order = foundMatch ? -1 : comparer.Compare(item, current.Item);
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
        if (root != null)
        {
            root.IsRed = false;
        }
        return foundMatch;
    }
    public virtual void Clear()
    {
        root = null;
    }
    public virtual bool Contains(T item) => FindNode(item) != null;
    public void CopyTo(T[] array, int arrayIndex)
    {
        foreach (var item in this) array[arrayIndex++] = item;
    }

    public Enumerator GetEnumerator() => new Enumerator(this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

    #region private
    private static bool Is2Node(Node node) => IsNonNullBlack(node) && IsNullOrBlack(node.Left) && IsNullOrBlack(node.Right); private static bool Is4Node(Node node) => IsNonNullRed(node.Left) && IsNonNullRed(node.Right);
    private static bool IsNonNullRed(Node node) => node != null && node.IsRed;
    private static bool IsNonNullBlack(Node node) => node != null && !node.IsRed;
    private static bool IsNullOrBlack(Node node) => node == null || !node.IsRed;
    private void ReplaceNode(Node match, Node parentOfMatch, Node succesor, Node parentOfSuccesor)
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
    private static void Merge2Nodes(Node parent)
    {
        parent.IsRed = false;
        parent.Left.IsRed = true;
        parent.Right.IsRed = true;
    }
    private static void Split4Node(Node node)
    {
        node.IsRed = true;
        node.Left.IsRed = false;
        node.Right.IsRed = false;
    }
    private static Node GetSibling(Node node, Node parent)
    {
        return parent.Left == node ? parent.Right : parent.Left;
    }
    private void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
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
    private static Node Rotate(Node node, TreeRotation rotation)
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
    private static Node RotateLeft(Node node)
    {
        Node child = node.Right;
        node.Right = child.Left;
        child.Left = node;
        return child;
    }
    private static Node RotateLeftRight(Node node)
    {
        Node child = node.Left;
        Node grandChild = child.Right;

        node.Left = grandChild.Right;
        grandChild.Right = node;
        child.Right = grandChild.Left;
        grandChild.Left = child;
        return grandChild;
    }
    private static Node RotateRight(Node node)
    {
        Node child = node.Left;
        node.Left = child.Right;
        child.Right = node;
        return child;
    }
    private static Node RotateRightLeft(Node node)
    {
        Node child = node.Right;
        Node grandChild = child.Left;

        node.Right = grandChild.Left;
        grandChild.Left = node;
        child.Left = grandChild.Right;
        grandChild.Right = child;
        return grandChild;
    }
    private void ReplaceChildOrRoot(Node parent, Node child, Node newChild)
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
    private static TreeRotation GetRotation(Node parent, Node current, Node sibling)
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

    public struct Enumerator : IEnumerator<T>
    {

        private Set<T> tree;

        private Stack<Node> stack;
        private Node current;

        private bool reverse;
        internal Enumerator(Set<T> set) : this(set, false, null) { }
        internal Enumerator(Set<T> set, bool reverse, Node startNode)
        {
            tree = set;
            stack = new Stack<Node>(2 * Log2(tree.Count + 1));
            current = null;
            this.reverse = reverse;
            if (startNode == null) IntializeAll();
            else Intialize(startNode);

        }
        private void IntializeAll()
        {
            var node = tree.root;
            while (node != null)
            {
                var next = reverse ? node.Right : node.Left;
                stack.Push(node); node = next;
            }
        }
        private void Intialize(Node startNode)
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
                    if (comparer.Compare(startNode.Item, node.Item) >= 0)
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
                    if (comparer.Compare(startNode.Item, node.Item) <= 0)
                        list.Add(node);
                    node = node.Parent;
                }
            }

            list.Reverse();
            foreach (var n in list) stack.Push(n);
        }
        private static int Log2(int num) => num == 0 ? 0 : MSB(num) + 1;
        public T Current => current == null ? default : current.Item;

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
        public T Item;
        public Node Parent
        {
            get; private set;
        }
        private Node _left;
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
        private Node _right;
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
        public Node(T item, bool isRed)
        {
            this.Item = item; this.IsRed = isRed;
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
        public override string ToString() => $"Size = {Size}, Item = {Item}";
    }
    private enum TreeRotation : byte
    {
        Left = 1,
        Right = 2,
        RightLeft = 3,
        LeftRight = 4,
    }
}
class MultiSet<T> : Set<T>
{
    public override bool IsMulti => true;
    public MultiSet() : base() { }
    public MultiSet(IEnumerable<T> collection) : base(collection) { }
    public MultiSet(IComparer<T> comparer) : base(comparer) { }
    public MultiSet(IEnumerable<T> collection, IComparer<T> comparer) : base(collection, comparer) { }
}
