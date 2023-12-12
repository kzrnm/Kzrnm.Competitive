using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive.Internal.Bbst
{
    /// <summary>
    /// なにかしらを遅延伝播させる
    /// </summary>
    public interface ISplayTreePusher<Node, T>
    {
        /// <summary>
        /// なにかしらを遅延伝播させる
        /// </summary>
        static abstract void Push(Node t);

        /// <summary>
        /// モノイドの加算
        /// </summary>
        static abstract T Operate(T x, T y);
    }

    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    public class SplayTreeNodeBase<TSelf, T>
        where TSelf : SplayTreeNodeBase<TSelf, T>, IBbstNode<T, TSelf>, ISplayTreePusher<TSelf, T>
    {
        public TSelf left, right;
        internal TSelf Parent;
        public int Size { get; protected set; }
        public T Value { get; protected set; }
        public T Sum { get; protected set; }
        public bool IsRoot => Parent == null || Parent.left != this && Parent.right != this;

        [凾(256)]
        public static TSelf Merge(TSelf l, TSelf r)
        {
            if (l == null || r == null)
            {
                var t = l ?? r;
                if (t != null)
                    Splay(t);
                return t;
            }

            Splay(l); Splay(r);
            while (l.right != null)
            {
                TSelf.Push(l);
                l = l.right;
            }
            Splay(l);
            l.right = r;
            r.Parent = l;
            TSelf.Update(l);
            return l;
        }
        [凾(256)]
        public static (TSelf, TSelf) Split(TSelf t, int k)
        {
            if (t == null) return (null, null);
            TSelf.Push(t);
            var lc = t.left?.Size ?? 0;
            if (k <= lc)
            {
                var (x1, x2) = Split(t.left, k);
                t.left = x2;
                t.Parent = null;
                if (x2 != null) x2.Parent = t;
                return (x1, TSelf.Update(t));
            }
            else
            {
                var (x1, x2) = Split(t.right, k - lc - 1);
                t.right = x1;
                t.Parent = null;
                if (x1 != null) x1.Parent = t;
                return (TSelf.Update(t), x2);
            }
        }

        /// <summary>
        /// 先頭に <paramref name="newNode"/> を追加します。
        /// </summary>
        [凾(256)]
        public static void AddFirst(ref TSelf t, TSelf newNode)
        {
            if (t == null)
            {
                t = newNode;
            }
            else
            {
                Splay(t);
                TSelf cur = t, z = newNode;
                while (cur.left != null)
                {
                    TSelf.Push(cur);
                    cur = cur.left;
                }
                Splay(cur);
                z.Parent = cur;
                cur.left = z;
                Splay(z);
                t = z;
            }
        }

        [凾(256)]
        public static void AddLast(ref TSelf t, TSelf newNode)
        {
            if (t == null)
            {
                t = newNode;
            }
            else
            {
                Splay(t);
                TSelf cur = t, z = newNode;
                while (cur.right != null)
                {
                    TSelf.Push(cur);
                    cur = cur.right;
                }
                Splay(cur);
                z.Parent = cur;
                cur.right = z;
                Splay(z);
                t = z;
            }
        }

        [凾(256)]
        public static void SetValue(ref TSelf t, int k, T x)
        {
            ElementAt(ref t, k).Value = x;
            Splay(t);
        }

        [凾(256)] public static T GetValue(ref TSelf t, int k) => ElementAt(ref t, k).Value;

        [凾(256)]
        static TSelf ElementAt(ref TSelf t, int k)
        {
            Splay(t);
            return t = SubElementAt(t, k);
        }
        [凾(256)]
        static TSelf SubElementAt(TSelf t, int k)
        {
            TSelf.Push(t);
            var lc = t.left?.Size ?? 0;
            if (k < lc)
                return SubElementAt(t.left, k);

            else if (k == lc)
            {
                Splay(t);
                return t;
            }
            else
                return SubElementAt(t.right, k - lc - 1);
        }

        [凾(256)]
        public static void Propagate(ref TSelf t)
        {
            if (t != null)
                Splay(t);
        }
        [凾(256)]
        internal static void Splay(TSelf t)
        {
            TSelf.Push(t);
            while (!t.IsRoot)
            {
                var q = t.Parent;
                if (q.IsRoot)
                {
                    TSelf.Push(q); TSelf.Push(t);
                    if (q.left == t) RotateR(t);
                    else RotateL(t);
                }
                else
                {
                    var r = q.Parent;
                    TSelf.Push(r); TSelf.Push(q); TSelf.Push(t);
                    if (r.left == q)
                    {
                        if (q.left == t) { RotateR(q); RotateR(t); }
                        else { RotateL(t); RotateR(t); }
                    }
                    else
                    {
                        if (q.right == t) { RotateL(q); RotateL(t); }
                        else { RotateR(t); RotateL(t); }
                    }
                }
            }
        }
        [凾(256)]
        static void RotateR(TSelf t)
        {
            var x = t.Parent;
            var y = x.Parent;
            if ((x.left = t.right) != null) t.right.Parent = x;
            t.right = x;
            x.Parent = t;
            TSelf.Update(x);
            TSelf.Update(t);
            if ((t.Parent = y) != null)
            {
                if (y.left == x) y.left = t;
                if (y.right == x) y.right = t;
                TSelf.Update(y);
            }
        }
        [凾(256)]
        static void RotateL(TSelf t)
        {
            var x = t.Parent;
            var y = x.Parent;
            if ((x.right = t.left) != null)
                t.left.Parent = x;

            t.left = x;
            x.Parent = t;
            TSelf.Update(x);
            TSelf.Update(t);
            if ((t.Parent = y) != null)
            {
                if (y.left == x) y.left = t;
                if (y.right == x) y.right = t;
                TSelf.Update(y);
            }
        }


        [凾(256)]
        public static TSelf Update(TSelf t)
        {
            if (t == null) return t;
            t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
            t.Sum = TSelf.Operate(TSelf.Operate(TSelf.Sum(t.left), t.Value), TSelf.Sum(t.right));
            return t;
        }

        public Enumerator GetEnumerator() => new(Unsafe.As<TSelf>(this));
        public static IEnumerator<T> GetEnumerator(ref TSelf t)
        {
            TSelf.Propagate(ref t);
            return new Enumerator(t);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
        public struct Enumerator : IEnumerator<T>
        {
            readonly Stack<TSelf> stack;
            TSelf current, root;

            readonly bool reverse;
            internal Enumerator(TSelf root) : this(root, false) { }
            internal Enumerator(TSelf root, bool reverse)
            {
                this.root = root;
                stack = new Stack<TSelf>(2 * Log2((root?.Size ?? 0) + 1));
                current = null;
                this.reverse = reverse;
                IntializeAll();
            }
            [凾(256)]
            void IntializeAll()
            {
                var node = root;
                while (node != null)
                {
                    TSelf.Push(node);
                    TSelf.Update(node);
                    var next = reverse ? node.right : node.left;
                    stack.Push(node);
                    node = next;
                }
            }

            [凾(256)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public T Current => current.Value;

            [凾(256)]
            public bool MoveNext()
            {
                if (!stack.TryPop(out current))
                {
                    current = null;
                    return false;
                }
                var node = reverse ? current.left : current.right;
                while (node != null)
                {
                    TSelf.Push(node);
                    TSelf.Update(node);
                    var next = reverse ? node.right : node.left;
                    stack.Push(node);
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