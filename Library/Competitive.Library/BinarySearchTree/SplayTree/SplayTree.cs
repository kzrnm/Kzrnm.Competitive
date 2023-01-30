using AtCoder;
using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE Splay 木
    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    /// <summary>
    /// Splay 木
    /// </summary>
    public class SplayTree<T> : SplayTree<T, SingleBbstOp<T>>
    {
        public SplayTree() { }
        public SplayTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public SplayTree(T[] v) : base(v.AsSpan()) { }
        public SplayTree(ReadOnlySpan<T> v) : base(v) { }
    }

    /// <summary>
    /// Splay 木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SplayTree<T, TOp> : IBinarySearchTree<T>
        where TOp : struct, ISegtreeOperator<T>
    {
        private static TOp op => new TOp();
        public SplayTree() { }
        public SplayTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public SplayTree(T[] v) : this(v.AsSpan()) { }
        public SplayTree(ReadOnlySpan<T> v)
        {
            root = Build(v);
        }
        private static Node Build(ReadOnlySpan<T> v)
        {
            if (v.IsEmpty)
                return null;
            if (v.Length == 1)
                return new Node(v[0]);
            int m = v.Length / 2;
            return Merge(
                Build(v[..m]),
                Build(v[m..])
            );
        }

        [DebuggerDisplay("Size = {" + nameof(cnt) + "}, Key = {" + nameof(Key) + "}, Sum = {" + nameof(Sum) + "}")]
        public class Node
        {
            public Node Left, Right, Parent;
            public T Key, Sum;
            public int cnt;

            public Node() : this(op.Identity) { }
            public Node(T v)
            {
                Key = Sum = v;
                cnt = 1;
            }
            public bool IsRoot => Parent == null || Parent.Left != this && Parent.Right != this;
        }


        public Node root;
        public int Count => Size(root);
        public T AllProd => Sum(root);
        bool ICollection<T>.IsReadOnly => false;

        [凾(256)] public static int Size(Node n) => n?.cnt ?? 0;
        [凾(256)] public static T Sum(Node n) => n != null ? n.Sum : op.Identity;

        public static void Splay(Node t)
        {
            while (!t.IsRoot)
            {
                var q = t.Parent;
                if (q.IsRoot)
                {
                    if (q.Left == t) RotateR(t);
                    else RotateL(t);
                }
                else
                {
                    var r = q.Parent;
                    if (r.Left == q)
                    {
                        if (q.Left == t) { RotateR(q); RotateR(t); }
                        else { RotateL(t); RotateR(t); }
                    }
                    else
                    {
                        if (q.Right == t) { RotateL(q); RotateL(t); }
                        else { RotateR(t); RotateL(t); }
                    }
                }
            }
        }
        [凾(256)]
        public static void PushFront(ref Node t, T v)
        {
            if (t == null)
            {
                t = new Node(v);
            }
            else
            {
                Splay(t);
                Node cur = GetLeft(t), z = new Node(v);
                Splay(cur);
                z.Parent = cur;
                cur.Left = z;
                Splay(z);
                t = z;
            }
        }

        [凾(256)]
        public static void PushBack(ref Node t, T v)
        {
            if (t == null)
            {
                t = new Node(v);
            }
            else
            {
                Splay(t);
                Node cur = GetRight(t), z = new Node(v);
                Splay(cur);
                z.Parent = cur;
                cur.Right = z;
                Splay(z);
                t = z;
            }
        }
        [凾(256)]
        public static Node Erase(Node t)
        {
            Splay(t);
            Node x = t.Left, y = t.Right;
            if (x == null)
            {
                t = y;
                if (t != null) t.Parent = null;
            }
            else if (y == null)
            {
                t = x;
                t.Parent = null;
            }
            else
            {
                x.Parent = null;
                t = GetRight(x);
                Splay(t);
                t.Right = y;
                y.Parent = t;
            }
            return t;
        }
        [凾(256)]
        public static (Node, Node) Split(Node t, int k)
        {
            if (t == null) return (null, null);
            if (k <= Size(t.Left))
            {
                var (x1, x2) = Split(t.Left, k);
                t.Left = x2;
                t.Parent = null;
                if (x2 != null) x2.Parent = t;
                return (x1, Update(t));
            }
            else
            {
                var (x1, x2) = Split(t.Right, k - Size(t.Left) - 1);
                t.Right = x1;
                t.Parent = null;
                if (x1 != null) x1.Parent = t;
                return (Update(t), x2);
            }
        }
        [凾(256)]
        public static (Node, Node, Node) Split3(Node t, int a, int b)
        {
            Splay(t);
            var (x1, x2) = Split(t, a);
            var (y1, y2) = Split(x2, b - a);
            return (x1, y1, y2);
        }
        [凾(256)]
        public static Node Merge(Node l, Node r)
        {
            if (l == null || r == null)
            {
                var t = l ?? r;
                if (t != null)
                    Splay(t);
                return t;
            }

            Splay(l); Splay(r);
            l = GetRight(l);
            Splay(l);
            l.Right = r;
            r.Parent = l;
            Update(l);
            return l;
        }
        [凾(256)]
        public static void Insert(ref Node t, int k, T v)
        {
            Splay(t);
            var (x1, x2) = Split(t, k);
            t = Merge(x1, Merge(new Node(v), x2));
        }
        [凾(256)]
        public static void Erase(ref Node t, int k)
        {
            Splay(t);
            var (x1, x2) = Split(t, k);
            var (_, y2) = Split(x2, 1);
            t = Merge(x1, y2);
        }
        [凾(256)]
        public static T Query(ref Node t, int a, int b)
        {
            if (a == b)
                return op.Identity;
            Splay(t);
            var (x1, x2) = Split(t, a);
            var (y1, y2) = Split(x2, b - a);
            var ret = Sum(y1);
            t = Merge(x1, Merge(y1, y2));
            return ret;
        }

        [凾(256)]
        public static Node Update(Node t)
        {
            if (t == null) return null;
            t.cnt = 1;
            t.Sum = t.Key;
            if (t.Left != null)
            {
                t.cnt += t.Left.cnt;
                t.Sum = op.Operate(t.Sum, t.Left.Sum);
            }
            if (t.Right != null)
            {
                t.cnt += t.Right.cnt;
                t.Sum = op.Operate(t.Sum, t.Right.Sum);
            }
            return t;
        }

        [凾(256)]
        public static Node GetLeft(Node t)
        {
            while (t.Left != null)
            {
                t = t.Left;
            }
            return t;
        }
        [凾(256)]
        public static Node GetRight(Node t)
        {
            while (t.Right != null)
            {
                t = t.Right;
            }
            return t;
        }

        [凾(256)]
        private static void RotateR(Node t)
        {
            var x = t.Parent;
            var y = x.Parent;
            if ((x.Left = t.Right) != null) t.Right.Parent = x;
            t.Right = x;
            x.Parent = t;
            Update(x);
            Update(t);
            if ((t.Parent = y) != null)
            {
                if (y.Left == x) y.Left = t;
                if (y.Right == x) y.Right = t;
                Update(y);
            }
        }
        [凾(256)]
        private static void RotateL(Node t)
        {
            var x = t.Parent;
            var y = x.Parent;
            if ((x.Right = t.Left) != null)
                t.Left.Parent = x;

            t.Left = x;
            x.Parent = t;
            Update(x);
            Update(t);
            if ((t.Parent = y) != null)
            {
                if (y.Left == x) y.Left = t;
                if (y.Right == x) y.Right = t;
                Update(y);
            }
        }

        [凾(256)]
        public static void SetElement(ref Node t, int k, T x)
        {
            t = ElementAt(ref t, k);
            t.Key = x;
            Splay(t);
        }

        public static Node ElementAt(ref Node t, int k)
        {
            Splay(t);
            return t = SubElementAt(t, k);
        }
        private static Node SubElementAt(Node t, int k)
        {
            if (k < Size(t.Left))
                return SubElementAt(t.Left, k);

            else if (k == Size(t.Left))
            {
                Splay(t);
                return t;
            }
            else
                return SubElementAt(t.Right, k - Size(t.Left) - 1);
        }

        /// <summary>
        /// <paramref name="index"/> の値を読み書きします。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            [凾(256)]
            get => ElementAt(ref root, index).Key;
            [凾(256)]
            set => SetElement(ref root, index, value);
        }
        [凾(256)]
        public void Add(T item) => PushBack(ref root, item);
        [凾(256)]
        public void Insert(int index, T item)
        {
            if (index == 0)
                PushFront(ref root, item);
            else
                Insert(ref root, index, item);
        }
        public int IndexOf(T item) => throw new NotImplementedException();

        [凾(256)]
        public void RemoveAt(int index)
        {
            Erase(ref root, index);
        }

        [凾(256)]
        public void Clear()
        {
            root = null;
        }

        /// <summary>
        /// [<paramref name="l"/>, <paramref name="r"/>) の総積を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public T Prod(int l, int r) => Query(ref root, l, r);

        [凾(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public T Slice(int p, int length) => Query(ref root, p, p + length);

        bool ICollection<T>.Contains(T item)
            => throw new NotImplementedException();

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            foreach (var v in this)
                array[arrayIndex++] = v;
        }

        bool ICollection<T>.Remove(T item)
            => throw new NotImplementedException();

        public Enumerator GetEnumerator()
            => new Enumerator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void AddRange(IEnumerable<T> items)
        {
            var newItems = Build(items.ToArray());
            root = Merge(root, newItems);
        }

        public void InsertRange(int index, IEnumerable<T> items)
        {
            var newItems = Build(items.ToArray());
            var (x, y) = Split(root, index);
            root = Merge(x, Merge(newItems, y));
        }

        public void RemoveRange(int index, int count)
        {
            var (x, _, y) = Split3(root, index, index + count);
            root = Merge(x, y);
        }

        public struct Enumerator : IEnumerator<T>
        {
            internal readonly SplayTree<T, TOp> tree;
            readonly Deque<Node> stack;
            Node current;

            readonly bool reverse;
            internal Enumerator(SplayTree<T, TOp> tree) : this(tree, false) { }
            internal Enumerator(SplayTree<T, TOp> tree, bool reverse)
            {
                this.tree = tree;
                stack = new Deque<Node>(2 * Log2(this.tree.Count + 1));
                current = null;
                this.reverse = reverse;
                IntializeAll();
            }
            [凾(256)]
            void IntializeAll()
            {
                var node = tree.root;
                while (node != null)
                {
                    Update(node);
                    var next = reverse ? node.Right : node.Left;
                    stack.AddLast(node);
                    node = next;
                }
            }

            [凾(256)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public T Current => current.Key;

            [凾(256)]
            public bool MoveNext()
            {
                if (stack.Count == 0)
                {
                    current = null;
                    return false;
                }
                current = stack.PopLast();
                var node = reverse ? current.Left : current.Right;
                while (node != null)
                {
                    Update(node);
                    var next = reverse ? node.Right : node.Left;
                    stack.AddLast(node);
                    node = next;
                }
                return true;
            }

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();
        }
    }
}
