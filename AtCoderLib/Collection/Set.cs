using AtCoderProject;
using System;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;
using static AtCoderProject.NumGlobal;


[System.Diagnostics.DebuggerTypeProxy(typeof(ΔDebugView<>))]
[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
class Set<T> : ICollection<T>, IReadOnlyCollection<T>
{
    public virtual bool IsMulti => false;
    public T Min { get { if (root == null) return default(T); var cur = root; for (; cur.Left != null; cur = cur.Left) { } return cur.Item; } }
    public T Max { get { if (root == null) return default(T); var cur = root; for (; cur.Right != null; cur = cur.Right) { } return cur.Item; } }
    public Node FindNode(T item) { Node current = root; while (current != null) { int order = Comparer.Compare(item, current.Item); if (order == 0) return current; else current = (order < 0) ? current.Left : current.Right; } return null; }
    public int Index(Node node) { var ret = NodeSize(node.Left); Node prev = node; node = node.Parent; while (prev != root) { if (node.Left != prev) { ret += NodeSize(node.Left) + 1; } prev = node; node = node.Parent; } return ret; }
    public Node FindByIndex(int index) { var current = root; var currentIndex = current.Size - NodeSize(current.Right) - 1; while (currentIndex != index) { if (currentIndex > index) { current = current.Left; if (current == null) break; currentIndex -= NodeSize(current.Right) + 1; } else { current = current.Right; if (current == null) break; currentIndex += NodeSize(current.Left) + 1; } } return current; }
    public Node FindNodeLowerBound(T item) { Node right = null; Node current = root; while (current != null) { if (Comparer.Compare(item, current.Item) <= 0) { right = current; current = current.Left; } else { current = current.Right; } } return right; }
    public Node FindNodeUpperBound(T item) { Node right = null; Node current = root; while (current != null) { if (Comparer.Compare(item, current.Item) < 0) { right = current; current = current.Left; } else { current = current.Right; } } return right; }

    public IEnumerable<T> Reversed() { var e = new Enumerator(this, true, null); while (e.MoveNext()) yield return e.Current; }
    public IEnumerable<T> Enumerate(Node from) => Enumerate(from, false);
    public IEnumerable<T> Enumerate(Node from, bool reverse) { var e = new Enumerator(this, reverse, from); while (e.MoveNext()) yield return e.Current; }

    public Set() : this(Comparer<T>.Default) { }
    public Set(IEnumerable<T> collection) : this(collection, Comparer<T>.Default) { }
    public Set(IComparer<T> comparer) { this.Comparer = comparer; }
    public Set(IEnumerable<T> collection, IComparer<T> comparer) { this.Comparer = comparer; var arr = InitArray(collection); this.root = ConstructRootFromSortedArray(arr, 0, arr.Length - 1, null); }
    protected T[] InitArray(IEnumerable<T> collection) { T[] arr; if (this.IsMulti) { arr = collection.ToArray(); Array.Sort(arr, this.Comparer); } else { var list = new List<T>(collection); list.Sort(this.Comparer); for (int i = list.Count - 1; i > 0; i--) if (this.Comparer.Compare(list[i - 1], list[i]) == 0) list.RemoveAt(i); arr = list.ToArray(); } return arr; }

    public int Count => NodeSize(root);
    protected static int NodeSize(Node node) => node == null ? 0 : node.Size;
    public IComparer<T> Comparer { get; }
    bool ICollection<T>.IsReadOnly => false;

    private Node root;

    private static Node ConstructRootFromSortedArray(T[] arr, int startIndex, int endIndex, Node redNode) { int size = endIndex - startIndex + 1; if (size == 0) { return null; } Node root; if (size == 1) { root = new Node(arr[startIndex], false); if (redNode != null) { root.Left = redNode; } } else if (size == 2) { root = new Node(arr[startIndex], false) { Right = new Node(arr[endIndex], false) }; root.Right.IsRed = true; if (redNode != null) { root.Left = redNode; } } else if (size == 3) { root = new Node(arr[startIndex + 1], false) { Right = new Node(arr[endIndex], false) }; var left = new Node(arr[startIndex], false); if (redNode != null) { left.Left = redNode; } root.Left = left; } else { int midpt = ((startIndex + endIndex) / 2); root = new Node(arr[midpt], false) { Left = ConstructRootFromSortedArray(arr, startIndex, midpt - 1, redNode) }; if (size % 2 == 0) { root.Right = ConstructRootFromSortedArray(arr, midpt + 2, endIndex, new Node(arr[midpt + 1], true)); } else { root.Right = ConstructRootFromSortedArray(arr, midpt + 1, endIndex, null); } } return root; }
    public void Add(T item) { if (root == null) { root = new Node(item, false); return; } Node current = root; Node parent = null; Node grandParent = null; Node greatGrandParent = null; int order = 0; while (current != null) { order = Comparer.Compare(item, current.Item); if (order == 0 && !this.IsMulti) { root.IsRed = false; return; } if (Is4Node(current)) { Split4Node(current); if (IsRed(parent) == true) { InsertionBalance(current, ref parent, grandParent, greatGrandParent); } } greatGrandParent = grandParent; grandParent = parent; parent = current; current = (order < 0) ? current.Left : current.Right; } Node node = new Node(item); if (order > 0) parent.Right = node; else parent.Left = node; if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent); root.IsRed = false; }
    public bool Remove(T item) { if (root == null) return false; Node current = root; Node parent = null; Node grandParent = null; Node match = null; Node parentOfMatch = null; bool foundMatch = false; while (current != null) { if (Is2Node(current)) { if (parent == null) { current.IsRed = true; } else { Node sibling = GetSibling(current, parent); if (sibling.IsRed) { if (parent.Right == sibling) RotateLeft(parent); else RotateRight(parent); parent.IsRed = true; sibling.IsRed = false; ReplaceChildOfNodeOrRoot(grandParent, parent, sibling); grandParent = sibling; if (parent == match) parentOfMatch = sibling; sibling = (parent.Left == current) ? parent.Right : parent.Left; } if (Is2Node(sibling)) { Merge2Nodes(parent, current, sibling); } else { TreeRotation rotation = RotationNeeded(parent, current, sibling); Node newGrandParent = null; switch (rotation) { case TreeRotation.RightRotation: sibling.Left.IsRed = false; newGrandParent = RotateRight(parent); break; case TreeRotation.LeftRotation: sibling.Right.IsRed = false; newGrandParent = RotateLeft(parent); break; case TreeRotation.RightLeftRotation: newGrandParent = RotateRightLeft(parent); break; case TreeRotation.LeftRightRotation: newGrandParent = RotateLeftRight(parent); break; } newGrandParent.IsRed = parent.IsRed; parent.IsRed = false; current.IsRed = true; ReplaceChildOfNodeOrRoot(grandParent, parent, newGrandParent); if (parent == match) { parentOfMatch = newGrandParent; } } } } int order = foundMatch ? -1 : Comparer.Compare(item, current.Item); if (order == 0) { foundMatch = true; match = current; parentOfMatch = parent; } grandParent = parent; parent = current; if (order < 0) { current = current.Left; } else { current = current.Right; } } if (match != null) { ReplaceNode(match, parentOfMatch, parent, grandParent); } if (root != null) { root.IsRed = false; } return foundMatch; }
    public virtual void Clear() { root = null; }
    public virtual bool Contains(T item) => FindNode(item) != null;
    public void CopyTo(T[] array, int arrayIndex) { foreach (var item in this) array[arrayIndex++] = item; }

    public Enumerator GetEnumerator() => new Enumerator(this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

    #region private
    private static bool Is2Node(Node node) => IsBlack(node) && IsNullOrBlack(node.Left) && IsNullOrBlack(node.Right); private static bool Is4Node(Node node) => IsRed(node.Left) && IsRed(node.Right); private static bool IsRed(Node node) => node != null && node.IsRed; private static bool IsBlack(Node node) => node != null && !node.IsRed; private static bool IsNullOrBlack(Node node) => node == null || !node.IsRed; private void ReplaceNode(Node match, Node parentOfMatch, Node succesor, Node parentOfSuccesor) { if (succesor == match) { succesor = match.Left; } else { if (succesor.Right != null) { succesor.Right.IsRed = false; } if (parentOfSuccesor != match) { parentOfSuccesor.Left = succesor.Right; succesor.Right = match.Right; } succesor.Left = match.Left; } if (succesor != null) { succesor.IsRed = match.IsRed; } ReplaceChildOfNodeOrRoot(parentOfMatch, match, succesor); }
    private static void Merge2Nodes(Node parent, Node child1, Node child2) { parent.IsRed = false; child1.IsRed = true; child2.IsRed = true; }
    private static void Split4Node(Node node) { node.IsRed = true; node.Left.IsRed = false; node.Right.IsRed = false; }
    private static Node GetSibling(Node node, Node parent) { if (parent.Left == node) { return parent.Right; } return parent.Left; }
    private void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent) { bool parentIsOnRight = grandParent.Right == parent; bool currentIsOnRight = parent.Right == current; Node newChildOfGreatGrandParent; if (parentIsOnRight == currentIsOnRight) { newChildOfGreatGrandParent = currentIsOnRight ? RotateLeft(grandParent) : RotateRight(grandParent); } else { newChildOfGreatGrandParent = currentIsOnRight ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent); parent = greatGrandParent; } grandParent.IsRed = true; newChildOfGreatGrandParent.IsRed = false; ReplaceChildOfNodeOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent); }
    private static Node RotateLeft(Node node) { Node x = node.Right; node.Right = x.Left; x.Left = node; return x; }
    private static Node RotateLeftRight(Node node) { Node child = node.Left; Node grandChild = child.Right; node.Left = grandChild.Right; grandChild.Right = node; child.Right = grandChild.Left; grandChild.Left = child; return grandChild; }
    private static Node RotateRight(Node node) { Node x = node.Left; node.Left = x.Right; x.Right = node; return x; }
    private static Node RotateRightLeft(Node node) { Node child = node.Right; Node grandChild = child.Left; node.Right = grandChild.Left; grandChild.Left = node; child.Left = grandChild.Right; grandChild.Right = child; return grandChild; }
    private void ReplaceChildOfNodeOrRoot(Node parent, Node child, Node newChild) { if (parent != null) { if (parent.Left == child) { parent.Left = newChild; } else { parent.Right = newChild; } } else { root = newChild; } }
    private static TreeRotation RotationNeeded(Node parent, Node current, Node sibling) { if (IsRed(sibling.Left)) { if (parent.Left == current) { return TreeRotation.RightLeftRotation; } return TreeRotation.RightRotation; } else { if (parent.Left == current) { return TreeRotation.LeftRotation; } return TreeRotation.LeftRightRotation; } }
    #endregion private

    public struct Enumerator : IEnumerator<T>
    {
        private Set<T> tree;

        private Stack<Node> stack;
        private Node current;

        private bool reverse;
        internal Enumerator(Set<T> set) : this(set, false, null) { }
        internal Enumerator(Set<T> set, bool reverse, Node startNode) { tree = set; stack = new Stack<Node>(2 * (MSB(tree.Count + 1) + 1)); current = null; this.reverse = reverse; if (startNode == null) IntializeAll(); else Intialize(startNode); }
        private void IntializeAll() { var node = tree.root; while (node != null) { var next = reverse ? node.Right : node.Left; stack.Push(node); node = next; } }
        private void Intialize(Node startNode) { current = null; var node = startNode; var list = new List<Node>(MSB(tree.Count + 1) + 1); while (node != null) { list.Add(node); var parent = node.Parent; if (parent == null) break; if (reverse) { if (parent.Left == node) break; } else { if (parent.Right == node) break; } node = parent; } list.Reverse(); foreach (var n in list) stack.Push(n); }

        public T Current => current == null ? default(T) : current.Item;

        public bool MoveNext() { if (stack.Count == 0) { current = null; return false; } current = stack.Pop(); var node = reverse ? current.Left : current.Right; while (node != null) { var next = reverse ? node.Right : node.Left; stack.Push(node); node = next; } return true; }

        object IEnumerator.Current => this.Current;
        public void Dispose() { }
        public void Reset() { throw new NotSupportedException(); }
    }
    public class Node
    {
        public bool IsRed;
        public T Item;
        public Node Parent { get; private set; }
        private Node _left;
        public Node Left { get { return _left; } set { _left = value; if (value != null) value.Parent = this; for (var cur = this; cur != null; cur = cur.Parent) { if (!cur.UpdateSize()) break; if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur) { cur.Parent = null; break; } } } }
        private Node _right;
        public Node Right { get { return _right; } set { _right = value; if (value != null) value.Parent = this; for (var cur = this; cur != null; cur = cur.Parent) { if (!cur.UpdateSize()) break; if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur) { cur.Parent = null; break; } } } }

        public int Size { get; private set; } = 1;
        public Node(T item) { this.Item = item; IsRed = true; }
        public Node(T item, bool isRed) { this.Item = item; this.IsRed = isRed; }
        public bool UpdateSize() { var oldsize = this.Size; var size = 1; if (Left != null) size += Left.Size; if (Right != null) size += Right.Size; this.Size = size; return oldsize != size; }
        public override string ToString() => $"Size = {Size}, Item = {Item}";
    }
    private enum TreeRotation
    {
        LeftRotation = 1,
        RightRotation = 2,
        RightLeftRotation = 3,
        LeftRightRotation = 4,
    }
}
class MultiSet<T> : Set<T> { public override bool IsMulti => true; }
