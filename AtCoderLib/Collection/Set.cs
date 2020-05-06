using AtCoderProject;
using System;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;
using static AtCoderProject.NumGlobal;


[System.Diagnostics.DebuggerTypeProxy(typeof(ΔDebugView<>))]
[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
class Set<T> : ICollection<T>, System.Collections.ICollection, IReadOnlyCollection<T>
{
    /*
     * Original
     * 
     * Copyright (c) .NET Foundation and Contributors
     * Released under the MIT license
     * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
     */
    public virtual bool IsMulti => false;

    #region Local variables/constants

    private Node root;
    private IComparer<T> comparer = default;
    private int count;
    private int version;

    private const string ComparerName = "Comparer"; // Do not rename (binary serialization)
    private const string CountName = "Count"; // Do not rename (binary serialization)
    private const string ItemsName = "Items"; // Do not rename (binary serialization)
    private const string VersionName = "Version"; // Do not rename (binary serialization)

    internal const int StackAllocThreshold = 100;

    #endregion

    #region Constructors

    public Set()
    {
        comparer = Comparer<T>.Default;
    }

    public Set(IComparer<T> comparer)
    {
        this.comparer = comparer ?? Comparer<T>.Default;
    }


    public Set(IEnumerable<T> collection) : this(collection, Comparer<T>.Default) { }

    public Set(IEnumerable<T> collection, IComparer<T> comparer)
        : this(comparer)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (collection is Set<T> sortedSet && !(sortedSet is TreeSubSet) && HasEqualComparer(sortedSet))
        {
            if (sortedSet.Count > 0)
            {
                this.count = sortedSet.count;
                root = sortedSet.root.DeepClone(this.count);
            }
            return;
        }

        T[] elements = collection.ToArray();
        int count = elements.Length;
        if (count > 0)
        {
            // If `comparer` is null, sets it to Comparer<T>.Default. We checked for this condition in the IComparer<T> constructor.
            // Array.Sort handles null comparers, but we need this later when we use `comparer.Compare` directly.
            comparer = this.comparer;
            Array.Sort(elements, 0, count, comparer);

            // Overwrite duplicates while shifting the distinct elements towards
            // the front of the array.
            int index = 1;
            for (int i = 1; i < count; i++)
            {
                if (comparer.Compare(elements[i], elements[i - 1]) != 0)
                {
                    elements[index++] = elements[i];
                }
            }

            count = index;
            root = ConstructRootFromSortedArray(elements, 0, count - 1, null);
            this.count = count;
        }
    }


    #endregion

    #region Bulk operation helpers

    private void AddAllElements(IEnumerable<T> collection)
    {
        foreach (T item in collection)
        {
            if (IsMulti || !Contains(item))
            {
                Add(item);
            }
        }
    }

    private void RemoveAllElements(IEnumerable<T> collection)
    {
        T min = Min;
        T max = Max;
        foreach (T item in collection)
        {
            if (!(comparer.Compare(item, min) < 0 || comparer.Compare(item, max) > 0) && Contains(item))
            {
                Remove(item);
            }
        }
    }

    private bool ContainsAllElements(IEnumerable<T> collection)
    {
        foreach (T item in collection)
        {
            if (!Contains(item))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Does an in-order tree walk and calls the delegate for each node.
    /// </summary>
    /// <param name="action">
    /// The delegate to invoke on each node.
    /// If the delegate returns <c>false</c>, the walk is stopped.
    /// </param>
    /// <returns><c>true</c> if the entire tree has been walked; otherwise, <c>false</c>.</returns>
    internal virtual bool InOrderTreeWalk(TreeWalkPredicate action)
    {
        if (root == null)
        {
            return true;
        }

        // The maximum height of a red-black tree is 2 * log2(n+1).
        // See page 264 of "Introduction to algorithms" by Thomas H. Cormen
        // Note: It's not strictly necessary to provide the stack capacity, but we don't
        // want the stack to unnecessarily allocate arrays as it grows.

        var stack = new Stack<Node>(2 * (int)Log2(Count + 1));
        Node current = root;

        while (current != null)
        {
            stack.Push(current);
            current = current.Left;
        }

        while (stack.Count != 0)
        {
            current = stack.Pop();
            if (!action(current))
            {
                return false;
            }

            Node node = current.Right;
            while (node != null)
            {
                stack.Push(node);
                node = node.Left;
            }
        }

        return true;
    }

    /// <summary>
    /// Does a left-to-right breadth-first tree walk and calls the delegate for each node.
    /// </summary>
    /// <param name="action">
    /// The delegate to invoke on each node.
    /// If the delegate returns <c>false</c>, the walk is stopped.
    /// </param>
    /// <returns><c>true</c> if the entire tree has been walked; otherwise, <c>false</c>.</returns>
    internal virtual bool BreadthFirstTreeWalk(TreeWalkPredicate action)
    {
        if (root == null)
        {
            return true;
        }

        var processQueue = new Queue<Node>();
        processQueue.Enqueue(root);

        Node current;
        while (processQueue.Count != 0)
        {
            current = processQueue.Dequeue();
            if (!action(current))
            {
                return false;
            }

            if (current.Left != null)
            {
                processQueue.Enqueue(current.Left);
            }
            if (current.Right != null)
            {
                processQueue.Enqueue(current.Right);
            }
        }

        return true;
    }

    #endregion

    #region Properties

    public int Count
    {
        get
        {
            VersionCheck(updateCount: true);
            return count;
        }
    }

    public IComparer<T> Comparer => comparer;

    bool ICollection<T>.IsReadOnly => false;

    bool System.Collections.ICollection.IsSynchronized => false;

    object System.Collections.ICollection.SyncRoot => this;

    #endregion

    #region Subclass helpers

    // Virtual function for TreeSubSet, which may need to update its count.
    internal virtual void VersionCheck(bool updateCount = false) { }
    // Virtual function for TreeSubSet, which may need the count variable of the parent set.
    internal virtual int TotalCount() { return Count; }

    // Virtual function for TreeSubSet, which may need to do range checks.
    internal virtual bool IsWithinRange(T item) => true;

    #endregion

    #region ICollection<T> members

    public bool Add(T item) => AddIfNotPresent(item); // Hack so the implementation can be made virtual

    void ICollection<T>.Add(T item) => Add(item);

    internal virtual bool AddIfNotPresent(T item)
    {
        if (root == null)
        {
            // The tree is empty and this is the first item.
            root = new Node(item, NodeColor.Black);
            count = 1;
            version++;
            return true;
        }

        // Search for a node at bottom to insert the new node.
        // If we can guarantee the node we found is not a 4-node, it would be easy to do insertion.
        // We split 4-nodes along the search path.
        Node current = root;
        Node parent = null;
        Node grandParent = null;
        Node greatGrandParent = null;

        // Even if we don't actually add to the set, we may be altering its structure (by doing rotations and such).
        // So update `_version` to disable any instances of Enumerator/TreeSubSet from working on it.
        version++;

        int order = 0;
        while (current != null)
        {
            order = comparer.Compare(item, current.Item);
            if (!IsMulti && order == 0)
            {
                // We could have changed root node to red during the search process.
                // We need to set it to black before we return.
                root.ColorBlack();
                return false;
            }

            // Split a 4-node into two 2-nodes.
            if (current.Is4Node)
            {
                current.Split4Node();
                // We could have introduced two consecutive red nodes after split. Fix that by rotation.
                if (Node.IsNonNullRed(parent))
                {
                    InsertionBalance(current, ref parent!, grandParent!, greatGrandParent!);
                }
            }

            greatGrandParent = grandParent;
            grandParent = parent;
            parent = current;
            current = (order < 0) ? current.Left : current.Right;
        }

        // We're ready to insert the new node.
        Node node = new Node(item, NodeColor.Red);
        if (order > 0)
        {
            parent.Right = node;
        }
        else
        {
            parent.Left = node;
        }

        // The new node will be red, so we will need to adjust colors if its parent is also red.
        if (parent.IsRed)
        {
            InsertionBalance(node, ref parent!, grandParent!, greatGrandParent!);
        }

        // The root node is always black.
        root.ColorBlack();
        ++count;
        return true;
    }

    public bool Remove(T item) => DoRemove(item); // Hack so the implementation can be made virtual

    internal virtual bool DoRemove(T item)
    {
        if (root == null)
        {
            return false;
        }

        // Search for a node and then find its successor.
        // Then copy the item from the successor to the matching node, and delete the successor.
        // If a node doesn't have a successor, we can replace it with its left child (if not empty),
        // or delete the matching node.
        //
        // In top-down implementation, it is important to make sure the node to be deleted is not a 2-node.
        // Following code will make sure the node on the path is not a 2-node.

        // Even if we don't actually remove from the set, we may be altering its structure (by doing rotations
        // and such). So update our version to disable any enumerators/subsets working on it.
        version++;

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
                // Fix up 2-node
                if (parent == null)
                {
                    // `current` is the root. Mark it red.
                    current.ColorRed();
                }
                else
                {
                    Node sibling = parent.GetSibling(current);
                    if (sibling.IsRed)
                    {
                        // If parent is a 3-node, flip the orientation of the red link.
                        // We can achieve this by a single rotation.
                        // This case is converted to one of the other cases below.
                        if (parent.Right == sibling)
                        {
                            parent.RotateLeft();
                        }
                        else
                        {
                            parent.RotateRight();
                        }

                        parent.ColorRed();
                        sibling.ColorBlack(); // The red parent can't have black children.
                                              // `sibling` becomes the child of `grandParent` or `root` after rotation. Update the link from that node.
                        ReplaceChildOrRoot(grandParent, parent, sibling);
                        // `sibling` will become the grandparent of `current`.
                        grandParent = sibling;
                        if (parent == match)
                        {
                            parentOfMatch = sibling;
                        }

                        sibling = parent.GetSibling(current);
                    }


                    if (sibling.Is2Node)
                    {
                        parent.Merge2Nodes();
                    }
                    else
                    {
                        // `current` is a 2-node and `sibling` is either a 3-node or a 4-node.
                        // We can change the color of `current` to red by some rotation.
                        Node newGrandParent = parent.Rotate(parent.GetRotation(current, sibling))!;

                        newGrandParent.Color = parent.Color;
                        parent.ColorBlack();
                        current.ColorRed();

                        ReplaceChildOrRoot(grandParent, parent, newGrandParent);
                        if (parent == match)
                        {
                            parentOfMatch = newGrandParent;
                        }
                        grandParent = newGrandParent;
                    }
                }
            }

            // We don't need to compare after we find the match.
            int order = foundMatch ? -1 : comparer.Compare(item, current.Item);
            if (order == 0)
            {
                // Save the matching node.
                foundMatch = true;
                match = current;
                parentOfMatch = parent;
            }

            grandParent = parent;
            parent = current;
            // If we found a match, continue the search in the right sub-tree.
            current = order < 0 ? current.Left : current.Right;
        }

        // Move successor to the matching node position and replace links.
        if (match != null)
        {
            ReplaceNode(match, parentOfMatch!, parent!, grandParent!);
            --count;
        }

        root?.ColorBlack();
        return foundMatch;
    }

    public virtual void Clear()
    {
        root = null;
        count = 0;
        ++version;
    }

    public virtual bool Contains(T item) => FindNode(item) != null;

    public void CopyTo(T[] array) => CopyTo(array, 0, Count);

    public void CopyTo(T[] array, int index) => CopyTo(array, index, Count);

    public void CopyTo(T[] array, int index, int count)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "SR.ArgumentOutOfRange_NeedNonNegNum");
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "SR.ArgumentOutOfRange_NeedNonNegNum");
        }

        if (count > array.Length - index)
        {
            throw new ArgumentException("SR.Arg_ArrayPlusOffTooSmall");
        }

        count += index; // Make `count` the upper bound.

        InOrderTreeWalk(node =>
        {
            if (index >= count)
            {
                return false;
            }

            array[index++] = node.Item;
            return true;
        });
    }

    void System.Collections.ICollection.CopyTo(Array array, int index)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (array.Rank != 1)
        {
            throw new ArgumentException("SR.Arg_RankMultiDimNotSupported", nameof(array));
        }

        if (array.GetLowerBound(0) != 0)
        {
            throw new ArgumentException("SR.Arg_NonZeroLowerBound", nameof(array));
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "SR.ArgumentOutOfRange_NeedNonNegNum");
        }

        if (array.Length - index < Count)
        {
            throw new ArgumentException("SR.Arg_ArrayPlusOffTooSmall");
        }

        T[] tarray = array as T[];
        if (tarray != null)
        {
            CopyTo(tarray, index);
        }
        else
        {
            object[] objects = array as object[];
            if (objects == null)
            {
                throw new ArgumentException("SR.Argument_InvalidArrayType", nameof(array));
            }

            try
            {
                InOrderTreeWalk(node =>
                {
                    objects[index++] = node.Item;
                    return true;
                });
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("SR.Argument_InvalidArrayType", nameof(array));
            }
        }
    }

    #endregion

    #region IEnumerable<T> members

    public Enumerator GetEnumerator() => new Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region Tree-specific operations

    // After calling InsertionBalance, we need to make sure `current` and `parent` are up-to-date.
    // It doesn't matter if we keep `grandParent` and `greatGrandParent` up-to-date, because we won't
    // need to split again in the next node.
    // By the time we need to split again, everything will be correctly set.
    private void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
    {

        bool parentIsOnRight = grandParent.Right == parent;
        bool currentIsOnRight = parent.Right == current;

        Node newChildOfGreatGrandParent;
        if (parentIsOnRight == currentIsOnRight)
        {
            // Same orientation, single rotation
            newChildOfGreatGrandParent = currentIsOnRight ? grandParent.RotateLeft() : grandParent.RotateRight();
        }
        else
        {
            // Different orientation, double rotation
            newChildOfGreatGrandParent = currentIsOnRight ? grandParent.RotateLeftRight() : grandParent.RotateRightLeft();
            // Current node now becomes the child of `greatGrandParent`
            parent = greatGrandParent;
        }

        // `grandParent` will become a child of either `parent` of `current`.
        grandParent.ColorRed();
        newChildOfGreatGrandParent.ColorBlack();

        ReplaceChildOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent);
    }

    /// <summary>
    /// Replaces the child of a parent node, or replaces the root if the parent is <c>null</c>.
    /// </summary>
    /// <param name="parent">The (possibly <c>null</c>) parent.</param>
    /// <param name="child">The child node to replace.</param>
    /// <param name="newChild">The node to replace <paramref name="child"/> with.</param>
    private void ReplaceChildOrRoot(Node parent, Node child, Node newChild)
    {
        if (parent != null)
        {
            parent.ReplaceChild(child, newChild);
        }
        else
        {
            root = newChild;
        }
    }

    /// <summary>
    /// Replaces the matching node with its successor.
    /// </summary>
    private void ReplaceNode(Node match, Node parentOfMatch, Node successor, Node parentOfSuccessor)
    {

        if (successor == match)
        {
            // This node has no successor. This can only happen if the right child of the match is null.
            successor = match.Left!;
        }
        else
        {

            successor.Right?.ColorBlack();

            if (parentOfSuccessor != match)
            {
                // Detach the successor from its parent and set its right child.
                parentOfSuccessor.Left = successor.Right;
                successor.Right = match.Right;
            }

            successor.Left = match.Left;
        }

        if (successor != null)
        {
            successor.Color = match.Color;
        }

        ReplaceChildOrRoot(parentOfMatch, match, successor!);
    }

    internal virtual Node FindNode(T item)
    {
        Node current = root;
        while (current != null)
        {
            int order = comparer.Compare(item, current.Item);
            if (order == 0)
            {
                return current;
            }

            current = order < 0 ? current.Left : current.Right;
        }

        return null;
    }

    /// <summary>
    /// Searches for an item and returns its zero-based index in this set.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The item's zero-based index in this set, or -1 if it isn't found.</returns>
    /// <remarks>
    /// <para>
    /// This implementation is based off of http://en.wikipedia.org/wiki/Binary_Tree#Methods_for_storing_binary_trees.
    /// </para>
    /// <para>
    /// This method is used with the <see cref="BitHelper"/> class. Note that this implementation is
    /// completely different from <see cref="TreeSubSet"/>'s, and that the two should not be mixed.
    /// </para>
    /// </remarks>
    internal virtual int InternalIndexOf(T item)
    {
        Node current = root;
        int count = 0;
        while (current != null)
        {
            int order = comparer.Compare(item, current.Item);
            if (order == 0)
            {
                return count;
            }

            current = order < 0 ? current.Left : current.Right;
            count = order < 0 ? (2 * count + 1) : (2 * count + 2);
        }

        return -1;
    }
    public int IndexOf(T item) => InternalIndexOf(item);

    internal Node FindRange(T from, T to) => FindRange(from, to, lowerBoundActive: true, upperBoundActive: true);

    internal Node FindRange(T from, T to, bool lowerBoundActive, bool upperBoundActive)
    {
        Node current = root;
        while (current != null)
        {
            if (lowerBoundActive && comparer.Compare(from, current.Item) > 0)
            {
                current = current.Right;
            }
            else
            {
                if (upperBoundActive && comparer.Compare(to, current.Item) < 0)
                {
                    current = current.Left;
                }
                else
                {
                    while (current.Left != null && comparer.Compare(from, current.Left.Item) == 0) current = current.Left;
                    return current;
                }
            }
        }

        return null;
    }

    internal void UpdateVersion() => ++version;

    /// <summary>
    /// Determines whether two <see cref="Set{T}"/> instances have the same comparer.
    /// </summary>
    /// <param name="other">The other <see cref="Set{T}"/>.</param>
    /// <returns>A value indicating whether both sets have the same comparer.</returns>
    private bool HasEqualComparer(Set<T> other)
    {
        // Commonly, both comparers will be the default comparer (and reference-equal). Avoid a virtual method call to Equals() in that case.
        return Comparer == other.Comparer || Comparer.Equals(other.Comparer);
    }

    #endregion

    #region ISet members

    public void UnionWith(IEnumerable<T> other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        Set<T> asSorted = other as Set<T>;
        TreeSubSet treeSubset = this as TreeSubSet;

        if (treeSubset != null)
            VersionCheck();

        if (asSorted != null && treeSubset == null && Count == 0)
        {
            Set<T> dummy = new Set<T>(asSorted, comparer);
            root = dummy.root;
            count = dummy.count;
            version++;
            return;
        }

        // This actually hurts if N is much greater than M. The / 2 is arbitrary.
        if (asSorted != null && treeSubset == null && HasEqualComparer(asSorted) && (asSorted.Count > this.Count / 2))
        {
            // First do a merge sort to an array.
            T[] merged = new T[asSorted.Count + this.Count];
            int c = 0;
            Enumerator mine = this.GetEnumerator();
            Enumerator theirs = asSorted.GetEnumerator();
            bool mineEnded = !mine.MoveNext(), theirsEnded = !theirs.MoveNext();
            while (!mineEnded && !theirsEnded)
            {
                int comp = Comparer.Compare(mine.Current, theirs.Current);
                if (comp < 0)
                {
                    merged[c++] = mine.Current;
                    mineEnded = !mine.MoveNext();
                }
                else if (comp == 0)
                {
                    merged[c++] = theirs.Current;
                    mineEnded = !mine.MoveNext();
                    theirsEnded = !theirs.MoveNext();
                }
                else
                {
                    merged[c++] = theirs.Current;
                    theirsEnded = !theirs.MoveNext();
                }
            }

            if (!mineEnded || !theirsEnded)
            {
                Enumerator remaining = (mineEnded ? theirs : mine);
                do
                {
                    merged[c++] = remaining.Current;
                }
                while (remaining.MoveNext());
            }

            // now merged has all c elements

            // safe to gc the root, we  have all the elements
            root = null;

            root = ConstructRootFromSortedArray(merged, 0, c - 1, null);
            count = c;
            version++;
        }
        else
        {
            AddAllElements(other);
        }
    }

    private static Node ConstructRootFromSortedArray(T[] arr, int startIndex, int endIndex, Node redNode)
    {
        // You're given a sorted array... say 1 2 3 4 5 6
        // There are 2 cases:
        // -  If there are odd # of elements, pick the middle element (in this case 4), and compute
        //    its left and right branches
        // -  If there are even # of elements, pick the left middle element, save the right middle element
        //    and call the function on the rest
        //    1 2 3 4 5 6 -> pick 3, save 4 and call the fn on 1,2 and 5,6
        //    now add 4 as a red node to the lowest element on the right branch
        //             3                       3
        //         1       5       ->     1        5
        //           2       6             2     4   6
        //    As we're adding to the leftmost of the right branch, nesting will not hurt the red-black properties
        //    Leaf nodes are red if they have no sibling (if there are 2 nodes or if a node trickles
        //    down to the bottom

        // This is done recursively because the iterative way to do this ends up wasting more space than it saves in stack frames
        // Only some base cases are handled below.

        int size = endIndex - startIndex + 1;
        Node root;

        switch (size)
        {
            case 0:
                return null;
            case 1:
                root = new Node(arr[startIndex], NodeColor.Black);
                if (redNode != null)
                {
                    root.Left = redNode;
                }
                break;
            case 2:
                root = new Node(arr[startIndex], NodeColor.Black);
                root.Right = new Node(arr[endIndex], NodeColor.Black);
                root.Right.ColorRed();
                if (redNode != null)
                {
                    root.Left = redNode;
                }
                break;
            case 3:
                root = new Node(arr[startIndex + 1], NodeColor.Black);
                root.Left = new Node(arr[startIndex], NodeColor.Black);
                root.Right = new Node(arr[endIndex], NodeColor.Black);
                if (redNode != null)
                {
                    root.Left.Left = redNode;
                }
                break;
            default:
                int midpt = ((startIndex + endIndex) / 2);
                root = new Node(arr[midpt], NodeColor.Black);
                root.Left = ConstructRootFromSortedArray(arr, startIndex, midpt - 1, redNode);
                root.Right = size % 2 == 0 ?
                    ConstructRootFromSortedArray(arr, midpt + 2, endIndex, new Node(arr[midpt + 1], NodeColor.Red)) :
                    ConstructRootFromSortedArray(arr, midpt + 1, endIndex, null);
                break;

        }

        return root;
    }

    /// <summary>
    /// This works similar to HashSet's CheckUniqueAndUnfound (description below), except that the bit
    /// array maps differently than in the HashSet. We can only use this for the bulk boolean checks.
    ///
    /// Determines counts that can be used to determine equality, subset, and superset. This
    /// is only used when other is an IEnumerable and not a HashSet. If other is a HashSet
    /// these properties can be checked faster without use of marking because we can assume
    /// other has no duplicates.
    ///
    /// The following count checks are performed by callers:
    /// 1. Equals: checks if UnfoundCount = 0 and uniqueFoundCount = Count; i.e. everything
    /// in other is in this and everything in this is in other
    /// 2. Subset: checks if UnfoundCount >= 0 and uniqueFoundCount = Count; i.e. other may
    /// have elements not in this and everything in this is in other
    /// 3. Proper subset: checks if UnfoundCount > 0 and uniqueFoundCount = Count; i.e
    /// other must have at least one element not in this and everything in this is in other
    /// 4. Proper superset: checks if unfound count = 0 and uniqueFoundCount strictly less
    /// than Count; i.e. everything in other was in this and this had at least one element
    /// not contained in other.
    ///
    /// An earlier implementation used delegates to perform these checks rather than returning
    /// an ElementCount struct; however this was changed due to the perf overhead of delegates.
    /// </summary>
    private unsafe ElementCount CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound)
    {
        ElementCount result;

        // need special case in case this has no elements.
        if (Count == 0)
        {
            int numElementsInOther = 0;
            foreach (T item in other)
            {
                numElementsInOther++;
                // break right away, all we want to know is whether other has 0 or 1 elements
                break;
            }
            result.UniqueCount = 0;
            result.UnfoundCount = numElementsInOther;
            return result;
        }

        int originalLastIndex = Count;

        int intArrayLength = BitHelper.ToIntArrayLength(originalLastIndex);

        Span<int> span = stackalloc int[StackAllocThreshold];
        BitHelper bitHelper = intArrayLength <= StackAllocThreshold ?
            new BitHelper(span.Slice(0, intArrayLength), clear: true) :
            new BitHelper(new int[intArrayLength], clear: false);

        // count of items in other not found in this
        int UnfoundCount = 0;
        // count of unique items in other found in this
        int uniqueFoundCount = 0;

        foreach (T item in other)
        {
            int index = InternalIndexOf(item);
            if (index >= 0)
            {
                if (!bitHelper.IsMarked(index))
                {
                    // item hasn't been seen yet
                    bitHelper.MarkBit(index);
                    uniqueFoundCount++;
                }
            }
            else
            {
                UnfoundCount++;
                if (returnIfUnfound)
                {
                    break;
                }
            }
        }

        result.UniqueCount = uniqueFoundCount;
        result.UnfoundCount = UnfoundCount;
        return result;
    }

    public int RemoveWhere(Predicate<T> match)
    {
        if (match == null)
        {
            throw new ArgumentNullException(nameof(match));
        }
        List<T> matches = new List<T>(this.Count);

        BreadthFirstTreeWalk(n =>
        {
            if (match(n.Item))
            {
                matches.Add(n.Item);
            }
            return true;
        });

        // Enumerate the results of the breadth-first walk in reverse in an attempt to lower cost.
        int actuallyRemoved = 0;
        for (int i = matches.Count - 1; i >= 0; i--)
        {
            if (Remove(matches[i]))
            {
                actuallyRemoved++;
            }
        }

        return actuallyRemoved;
    }

    #endregion

    #region ISorted members

    public T Min => MinInternal;

    internal virtual T MinInternal
    {
        get
        {
            if (root == null)
            {
                return default(T)!;
            }

            Node current = root;
            while (current.Left != null)
            {
                current = current.Left;
            }

            return current.Item;
        }
    }

    public T Max => MaxInternal;

    internal virtual T MaxInternal
    {
        get
        {
            if (root == null)
            {
                return default(T)!;
            }

            Node current = root;
            while (current.Right != null)
            {
                current = current.Right;
            }

            return current.Item;
        }
    }

    public IEnumerable<T> Reverse()
    {
        Enumerator e = new Enumerator(this, reverse: true);
        while (e.MoveNext())
        {
            yield return e.Current;
        }
    }

    public virtual Set<T> GetViewBetween(T lowerValue, T upperValue)
    {
        if (Comparer.Compare(lowerValue, upperValue) > 0)
        {
            throw new ArgumentException("SR.Set_LowerValueGreaterThanUpperValue", nameof(lowerValue));
        }
        return new TreeSubSet(this, lowerValue, upperValue, true, true);
    }


    #endregion

    #region Helper classes

    internal sealed class Node
    {
        public Node(T item, NodeColor color)
        {
            Item = item;
            Color = color;
        }

        public static bool IsNonNullBlack(Node node) => node != null && node.IsBlack;

        public static bool IsNonNullRed(Node node) => node != null && node.IsRed;

        public static bool IsNullOrBlack(Node node) => node == null || node.IsBlack;

        public T Item { get; set; }

        public Node Left { get; set; }

        public Node Right { get; set; }

        public NodeColor Color { get; set; }

        public bool IsBlack => Color == NodeColor.Black;

        public bool IsRed => Color == NodeColor.Red;

        public bool Is2Node => IsBlack && IsNullOrBlack(Left) && IsNullOrBlack(Right);

        public bool Is4Node => IsNonNullRed(Left) && IsNonNullRed(Right);

        public void ColorBlack() => Color = NodeColor.Black;

        public void ColorRed() => Color = NodeColor.Red;

        public Node DeepClone(int count)
        {
            // Breadth-first traversal to recreate nodes, preorder traversal to replicate nodes.

            var originalNodes = new Stack<Node>(2 * Log2(count) + 2);
            var newNodes = new Stack<Node>(2 * Log2(count) + 2);
            Node newRoot = ShallowClone();

            Node originalCurrent = this;
            Node newCurrent = newRoot;

            while (originalCurrent != null)
            {
                originalNodes.Push(originalCurrent);
                newNodes.Push(newCurrent);
                newCurrent.Left = originalCurrent.Left?.ShallowClone();
                originalCurrent = originalCurrent.Left;
                newCurrent = newCurrent.Left!;
            }

            while (originalNodes.Count != 0)
            {
                originalCurrent = originalNodes.Pop();
                newCurrent = newNodes.Pop();

                Node originalRight = originalCurrent.Right;
                Node newRight = originalRight?.ShallowClone();
                newCurrent.Right = newRight;

                while (originalRight != null)
                {
                    originalNodes.Push(originalRight);
                    newNodes.Push(newRight!);
                    newRight!.Left = originalRight.Left?.ShallowClone();
                    originalRight = originalRight.Left;
                    newRight = newRight.Left;
                }
            }

            return newRoot;
        }

        /// <summary>
        /// Gets the rotation this node should undergo during a removal.
        /// </summary>
        public TreeRotation GetRotation(Node current, Node sibling)
        {
            bool currentIsLeftChild = Left == current;
            return IsNonNullRed(sibling.Left) ?
                (currentIsLeftChild ? TreeRotation.RightLeft : TreeRotation.Right) :
                (currentIsLeftChild ? TreeRotation.Left : TreeRotation.LeftRight);
        }

        /// <summary>
        /// Gets the sibling of one of this node's children.
        /// </summary>
        public Node GetSibling(Node node)
        {

            return node == Left ? Right! : Left!;
        }

        public Node ShallowClone() => new Node(Item, Color);

        public void Split4Node()
        {

            ColorRed();
            Left.ColorBlack();
            Right.ColorBlack();
        }

        /// <summary>
        /// Does a rotation on this tree. May change the color of a grandchild from red to black.
        /// </summary>
        public Node Rotate(TreeRotation rotation)
        {
            Node removeRed;
            switch (rotation)
            {
                case TreeRotation.Right:
                    removeRed = Left!.Left!;
                    removeRed.ColorBlack();
                    return RotateRight();
                case TreeRotation.Left:
                    removeRed = Right!.Right!;
                    removeRed.ColorBlack();
                    return RotateLeft();
                case TreeRotation.RightLeft:
                    return RotateRightLeft();
                case TreeRotation.LeftRight:
                    return RotateLeftRight();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Does a left rotation on this tree, making this node the new left child of the current right child.
        /// </summary>
        public Node RotateLeft()
        {
            Node child = Right!;
            Right = child.Left;
            child.Left = this;
            return child;
        }

        /// <summary>
        /// Does a left-right rotation on this tree. The left child is rotated left, then this node is rotated right.
        /// </summary>
        public Node RotateLeftRight()
        {
            Node child = Left!;
            Node grandChild = child.Right!;

            Left = grandChild.Right;
            grandChild.Right = this;
            child.Right = grandChild.Left;
            grandChild.Left = child;
            return grandChild;
        }

        /// <summary>
        /// Does a right rotation on this tree, making this node the new right child of the current left child.
        /// </summary>
        public Node RotateRight()
        {
            Node child = Left!;
            Left = child.Right;
            child.Right = this;
            return child;
        }

        /// <summary>
        /// Does a right-left rotation on this tree. The right child is rotated right, then this node is rotated left.
        /// </summary>
        public Node RotateRightLeft()
        {
            Node child = Right!;
            Node grandChild = child.Left!;

            Right = grandChild.Left;
            grandChild.Left = this;
            child.Left = grandChild.Right;
            grandChild.Right = child;
            return grandChild;
        }

        /// <summary>
        /// Combines two 2-nodes into a 4-node.
        /// </summary>
        public void Merge2Nodes()
        {

            // Combine two 2-nodes into a 4-node.
            ColorBlack();
            Left.ColorRed();
            Right.ColorRed();
        }

        /// <summary>
        /// Replaces a child of this node with a new node.
        /// </summary>
        /// <param name="child">The child to replace.</param>
        /// <param name="newChild">The node to replace <paramref name="child"/> with.</param>
        public void ReplaceChild(Node child, Node newChild)
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

        private int GetCount() => 1 + (Left?.GetCount() ?? 0) + (Right?.GetCount() ?? 0);

        private bool HasChild(Node child) => child == Left || child == Right;

        private bool HasChildren(Node child1, Node child2)
        {

            return (Left == child1 && Right == child2)
                || (Left == child2 && Right == child1);
        }
    }

    public struct Enumerator : IEnumerator<T>, IEnumerator
    {
        private readonly Set<T> _tree;
        private readonly int _version;

        private readonly Stack<Node> _stack;
        private Node _current;

        private readonly bool _reverse;

        internal Enumerator(Set<T> set)
            : this(set, reverse: false)
        {
        }

        internal Enumerator(Set<T> set, bool reverse)
        {
            _tree = set;
            set.VersionCheck();
            _version = set.version;

            // 2 log(n + 1) is the maximum height.
            _stack = new Stack<Node>(2 * (int)Log2(set.TotalCount() + 1));
            _current = null;
            _reverse = reverse;

            Initialize();
        }

        private void Initialize()
        {
            _current = null;
            Node node = _tree.root;
            Node next = null, other = null;
            while (node != null)
            {
                next = (_reverse ? node.Right : node.Left);
                other = (_reverse ? node.Left : node.Right);
                if (_tree.IsWithinRange(node.Item))
                {
                    _stack.Push(node);
                    node = next;
                }
                else if (next == null || !_tree.IsWithinRange(next.Item))
                {
                    node = other;
                }
                else
                {
                    node = next;
                }
            }
        }

        public bool MoveNext()
        {
            // Make sure that the underlying subset has not been changed since
            _tree.VersionCheck();

            if (_version != _tree.version)
            {
                throw new InvalidOperationException("SR.InvalidOperation_EnumFailedVersion");
            }

            if (_stack.Count == 0)
            {
                _current = null;
                return false;
            }

            _current = _stack.Pop();
            Node node = (_reverse ? _current.Left : _current.Right);
            Node next = null, other = null;
            while (node != null)
            {
                next = (_reverse ? node.Right : node.Left);
                other = (_reverse ? node.Left : node.Right);
                if (_tree.IsWithinRange(node.Item))
                {
                    _stack.Push(node);
                    node = next;
                }
                else if (other == null || !_tree.IsWithinRange(other.Item))
                {
                    node = next;
                }
                else
                {
                    node = other;
                }
            }
            return true;
        }

        public void Dispose() { }

        public T Current
        {
            get
            {
                if (_current != null)
                {
                    return _current.Item;
                }
                return default(T)!; // Should only happen when accessing Current is undefined behavior
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (_current == null)
                {
                    throw new InvalidOperationException("SR.InvalidOperation_EnumOpCantHappen");
                }

                return _current.Item;
            }
        }

        internal bool NotStartedOrEnded => _current == null;

        internal void Reset()
        {
            if (_version != _tree.version)
            {
                throw new InvalidOperationException("SR.InvalidOperation_EnumFailedVersion");
            }

            _stack.Clear();
            Initialize();
        }

        void IEnumerator.Reset() => Reset();
    }

    internal struct ElementCount
    {
        internal int UniqueCount;
        internal int UnfoundCount;
    }

    #endregion

    #region Miscellaneous

    /// <summary>
    /// Searches the set for a given value and returns the equal value it finds, if any.
    /// </summary>
    /// <param name="equalValue">The value to search for.</param>
    /// <param name="actualValue">The value from the set that the search found, or the default value of <typeparamref name="T"/> when the search yielded no match.</param>
    /// <returns>A value indicating whether the search was successful.</returns>
    /// <remarks>
    /// This can be useful when you want to reuse a previously stored reference instead of
    /// a newly constructed one (so that more sharing of references can occur) or to look up
    /// a value that has more complete data than the value you currently have, although their
    /// comparer functions indicate they are equal.
    /// </remarks>
    public bool TryGetValue(T equalValue, out T actualValue)
    {
        Node node = FindNode(equalValue);
        if (node != null)
        {
            actualValue = node.Item;
            return true;
        }
        actualValue = default(T)!;
        return false;
    }

    // Used for set checking operations (using enumerables) that rely on counting
    private static int Log2(int value)
    {
        int result = 0;
        while (value > 0)
        {
            result++;
            value >>= 1;
        }
        return result;
    }

    #endregion

    /// <summary>
    /// This class represents a subset view into the tree. Any changes to this view
    /// are reflected in the actual tree. It uses the comparer of the underlying tree.
    /// </summary>
    internal sealed class TreeSubSet : Set<T>
    {
        private readonly Set<T> _underlying;
        private readonly T _min;
        private readonly T _max;
        // keeps track of whether the count variable is up to date
        // up to date -> _countVersion = _underlying.version
        // not up to date -> _countVersion < _underlying.version
        private int _countVersion;
        // these exist for unbounded collections
        // for instance, you could allow this subset to be defined for i > 10. The set will throw if
        // anything <= 10 is added, but there is no upper bound. These features Head(), Tail(), were punted
        // in the spec, and are not available, but the framework is there to make them available at some point.
        private readonly bool _lBoundActive, _uBoundActive;
        // used to see if the count is out of date


        public TreeSubSet(Set<T> Underlying, T Min, T Max, bool lowerBoundActive, bool upperBoundActive)
            : base(Underlying.Comparer)
        {
            _underlying = Underlying;
            _min = Min;
            _max = Max;
            _lBoundActive = lowerBoundActive;
            _uBoundActive = upperBoundActive;
            root = _underlying.FindRange(_min, _max, _lBoundActive, _uBoundActive); // root is first element within range
            count = 0;
            version = -1;
            _countVersion = -1;
        }

        internal override bool AddIfNotPresent(T item)
        {
            if (!IsWithinRange(item))
            {
                throw new ArgumentOutOfRangeException(nameof(item));
            }

            bool ret = _underlying.AddIfNotPresent(item);
            VersionCheck();
            return ret;
        }

        public override bool Contains(T item)
        {
            VersionCheck(); return base.Contains(item);
        }

        internal override bool DoRemove(T item)
        {
            if (!IsWithinRange(item))
            {
                return false;
            }

            bool ret = _underlying.Remove(item);
            VersionCheck(); return ret;
        }

        public override void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            List<T> toRemove = new List<T>();
            BreadthFirstTreeWalk(n => { toRemove.Add(n.Item); return true; });
            while (toRemove.Count != 0)
            {
                _underlying.Remove(toRemove[toRemove.Count - 1]);
                toRemove.RemoveAt(toRemove.Count - 1);
            }

            root = null;
            count = 0;
            version = _underlying.version;
        }

        internal override bool IsWithinRange(T item)
        {
            int comp = _lBoundActive ? Comparer.Compare(_min, item) : -1;
            if (comp > 0)
            {
                return false;
            }

            comp = _uBoundActive ? Comparer.Compare(_max, item) : 1;
            return comp >= 0;
        }

        internal override T MinInternal
        {
            get
            {
                Node current = root;
                T result = _min;

                while (current != null)
                {

                    int comp = _lBoundActive ? Comparer.Compare(_min, current.Item) : -1;
                    if (comp == 1)
                    {
                        current = current.Right;
                    }
                    else
                    {
                        result = current.Item;
                        if (comp == 0)
                        {
                            break;
                        }
                        current = current.Left;
                    }
                }

                return result;
            }
        }

        internal override T MaxInternal
        {
            get
            {
                Node current = root;
                T result = _max;

                while (current != null)
                {
                    int comp = _uBoundActive ? Comparer.Compare(_max, current.Item) : 1;
                    if (comp == -1)
                    {
                        current = current.Left;
                    }
                    else
                    {
                        result = current.Item;
                        if (comp == 0)
                        {
                            break;
                        }
                        current = current.Right;
                    }
                }

                return result;
            }
        }

        internal override bool InOrderTreeWalk(TreeWalkPredicate action)
        {
            VersionCheck();

            if (root == null)
            {
                return true;
            }

            // The maximum height of a red-black tree is 2*lg(n+1).
            // See page 264 of "Introduction to algorithms" by Thomas H. Cormen
            Stack<Node> stack = new Stack<Node>(2 * (int)Set<T>.Log2(count + 1)); // this is not exactly right if count is out of date, but the stack can grow
            Node current = root;
            while (current != null)
            {
                if (IsWithinRange(current.Item))
                {
                    stack.Push(current);
                    current = current.Left;
                }
                else if (_lBoundActive && Comparer.Compare(_min, current.Item) > 0)
                {
                    current = current.Right;
                }
                else
                {
                    current = current.Left;
                }
            }

            while (stack.Count != 0)
            {
                current = stack.Pop();
                if (!action(current))
                {
                    return false;
                }

                Node node = current.Right;
                while (node != null)
                {
                    if (IsWithinRange(node.Item))
                    {
                        stack.Push(node);
                        node = node.Left;
                    }
                    else if (_lBoundActive && Comparer.Compare(_min, node.Item) > 0)
                    {
                        node = node.Right;
                    }
                    else
                    {
                        node = node.Left;
                    }
                }
            }
            return true;
        }

        internal override bool BreadthFirstTreeWalk(TreeWalkPredicate action)
        {
            VersionCheck();

            if (root == null)
            {
                return true;
            }

            Queue<Node> processQueue = new Queue<Node>();
            processQueue.Enqueue(root);
            Node current;

            while (processQueue.Count != 0)
            {
                current = processQueue.Dequeue();
                if (IsWithinRange(current.Item) && !action(current))
                {
                    return false;
                }
                if (current.Left != null && (!_lBoundActive || Comparer.Compare(_min, current.Item) < 0))
                {
                    processQueue.Enqueue(current.Left);
                }
                if (current.Right != null && (!_uBoundActive || Comparer.Compare(_max, current.Item) > 0))
                {
                    processQueue.Enqueue(current.Right);
                }
            }
            return true;
        }

        internal override Set<T>.Node FindNode(T item)
        {
            if (!IsWithinRange(item))
            {
                return null;
            }

            VersionCheck();
            return base.FindNode(item);
        }

        // this does indexing in an inefficient way compared to the actual sortedset, but it saves a
        // lot of space
        internal override int InternalIndexOf(T item)
        {
            int count = -1;
            foreach (T i in this)
            {
                count++;
                if (Comparer.Compare(item, i) == 0)
                    return count;
            }
            return -1;
        }

        /// <summary>
        /// Checks whether this subset is out of date, and updates it if necessary.
        /// <param name="updateCount">Updates the count variable if necessary.</param>
        /// </summary>
        internal override void VersionCheck(bool updateCount = false) => VersionCheckImpl(updateCount);

        private void VersionCheckImpl(bool updateCount)
        {
            if (version != _underlying.version)
            {
                root = _underlying.FindRange(_min, _max, _lBoundActive, _uBoundActive);
                version = _underlying.version;
            }

            if (updateCount && _countVersion != _underlying.version)
            {
                count = 0;
                InOrderTreeWalk(n => { count++; return true; });
                _countVersion = _underlying.version;
            }
        }

        /// <summary>
        /// Returns the number of elements <c>count</c> of the parent set.
        /// </summary>
        internal override int TotalCount()
        {
            return _underlying.Count;
        }

        // This passes functionality down to the underlying tree, clipping edges if necessary
        // There's nothing gained by having a nested subset. May as well draw it from the base
        // Cannot increase the bounds of the subset, can only decrease it
        public override Set<T> GetViewBetween(T lowerValue, T upperValue)
        {
            if (_lBoundActive && Comparer.Compare(_min, lowerValue) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lowerValue));
            }
            if (_uBoundActive && Comparer.Compare(_max, upperValue) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(upperValue));
            }
            return (TreeSubSet)_underlying.GetViewBetween(lowerValue, upperValue);
        }
    }
    internal enum NodeColor : byte
    {
        Black,
        Red
    }

    internal delegate bool TreeWalkPredicate(Node node);

    internal enum TreeRotation : byte
    {
        Left,
        LeftRight,
        Right,
        RightLeft
    }
    internal ref struct BitHelper
    {
        private const int IntSize = sizeof(int) * 8;
        private readonly Span<int> _span;

        internal BitHelper(Span<int> span, bool clear)
        {
            if (clear)
            {
                span.Clear();
            }
            _span = span;
        }

        internal void MarkBit(int bitPosition)
        {
            int bitArrayIndex = bitPosition / IntSize;
            if ((uint)bitArrayIndex < (uint)_span.Length)
            {
                _span[bitArrayIndex] |= (1 << (bitPosition % IntSize));
            }
        }

        internal bool IsMarked(int bitPosition)
        {
            int bitArrayIndex = bitPosition / IntSize;
            return
                (uint)bitArrayIndex < (uint)_span.Length &&
                (_span[bitArrayIndex] & (1 << (bitPosition % IntSize))) != 0;
        }

        /// <summary>How many ints must be allocated to represent n bits. Returns (n+31)/32, but avoids overflow.</summary>
        internal static int ToIntArrayLength(int n) => n > 0 ? ((n - 1) / IntSize + 1) : 0;
    }
}
class MultiSet<T> : Set<T> { public override bool IsMulti => true; }