using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive.Internal.Bbst
{
    /// <summary>
    /// なにかしらを遅延伝播させる
    /// </summary>
    public interface ISplayTreePusher<T, Nd, N> : IBbstNodeOp<T, Nd, N>
        where Nd : SplayTreeNodeBase<Nd, T>
        where N : ISplayTreePusher<T, Nd, N>
    {
        /// <summary>
        /// なにかしらを遅延伝播させる
        /// </summary>
        static abstract void Push(Nd t);

        /// <summary>
        /// モノイドの加算
        /// </summary>
        static abstract T Operate(T x, T y);

        [凾(256)]
        static void RotateR(Nd t)
        {
            var x = t.Parent;
            var y = x.Parent;
            if ((x.left = t.right) != null) t.right.Parent = x;
            t.right = x;
            x.Parent = t;
            N.Update(x);
            N.Update(t);
            if ((t.Parent = y) != null)
            {
                if (y.left == x) y.left = t;
                if (y.right == x) y.right = t;
                N.Update(y);
            }
        }
        [凾(256)]
        static void RotateL(Nd t)
        {
            var x = t.Parent;
            var y = x.Parent;
            if ((x.right = t.left) != null)
                t.left.Parent = x;

            t.left = x;
            x.Parent = t;
            N.Update(x);
            N.Update(t);
            if ((t.Parent = y) != null)
            {
                if (y.left == x) y.left = t;
                if (y.right == x) y.right = t;
                N.Update(y);
            }
        }

        [凾(256)]
        static void Splay(Nd t)
        {
            N.Push(t);
            while (!t.IsRoot)
            {
                var q = t.Parent;
                if (q.IsRoot)
                {
                    N.Push(q); N.Push(t);
                    if (q.left == t) RotateR(t);
                    else RotateL(t);
                }
                else
                {
                    var r = q.Parent;
                    N.Push(r); N.Push(q); N.Push(t);
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
        static Nd ElementAt(ref Nd t, int k)
        {
            Splay(t);
            return t = SubElementAt(t, k);
        }
        [凾(256)]
        static Nd SubElementAt(Nd t, int k)
        {
            N.Push(t);
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


        /// <summary>
        /// 先頭に <paramref name="newNode"/> を追加します。
        /// </summary>
        [凾(256)]
        static void IBbstNodeOp<T, Nd, N>.AddFirst(ref Nd t, Nd newNode)
        {
            if (t == null)
            {
                t = newNode;
            }
            else
            {
                Splay(t);
                Nd cur = t, z = newNode;
                while (cur.left != null)
                {
                    N.Push(cur);
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
        static void IBbstNodeOp<T, Nd, N>.AddLast(ref Nd t, Nd newNode)
        {
            if (t == null)
            {
                t = newNode;
            }
            else
            {
                Splay(t);
                Nd cur = t, z = newNode;
                while (cur.right != null)
                {
                    N.Push(cur);
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
        static Nd IBbstNodeOp<Nd, N>.Merge(Nd l, Nd r)
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
                N.Push(l);
                l = l.right;
            }
            Splay(l);
            l.right = r;
            r.Parent = l;
            N.Update(l);
            return l;
        }
        [凾(256)]
        static (Nd, Nd) IBbstNodeOp<Nd, N>.Split(Nd t, int k)
        {
            if (t == null) return (null, null);
            N.Push(t);
            var lc = t.left?.Size ?? 0;
            if (k <= lc)
            {
                var (x1, x2) = N.Split(t.left, k);
                t.left = x2;
                t.Parent = null;
                if (x2 != null) x2.Parent = t;
                return (x1, N.Update(t));
            }
            else
            {
                var (x1, x2) = N.Split(t.right, k - lc - 1);
                t.right = x1;
                t.Parent = null;
                if (x1 != null) x1.Parent = t;
                return (N.Update(t), x2);
            }
        }
        [凾(256)]
        static void IBbstNodeOp<T, Nd, N>.SetValue(ref Nd t, int k, T x)
        {
            ElementAt(ref t, k).Value = x;
            Splay(t);
        }

        [凾(256)]
        static T IBbstNodeOp<T, Nd, N>.GetValue(ref Nd t, int k) => ElementAt(ref t, k).Value;

        [凾(256)]
        static void IBbstNodeOp<Nd, N>.Propagate(ref Nd t)
        {
            if (t != null)
                Splay(t);
        }

        [凾(256)]
        static Nd IBbstNodeOp<Nd, N>.Update(Nd t)
        {
            if (t == null) return t;
            t.Size = (t.left?.Size ?? 0) + (t.right?.Size ?? 0) + 1;
            t.Sum = N.Operate(N.Operate(N.Sum(t.left), t.Value), N.Sum(t.right));
            return t;
        }

        static IEnumerator<T> IBbstNodeOp<T, Nd, N>.GetEnumerator(ref Nd t)
        {
            N.Propagate(ref t);
            return new Enumerator(t);
        }

        public sealed class Enumerator : IEnumerator<T>
        {
            readonly Stack<Nd> stack;
            Nd current, root;

            readonly bool reverse;
            internal Enumerator(Nd root) : this(root, false) { }
            internal Enumerator(Nd root, bool reverse)
            {
                this.root = root;
                stack = new Stack<Nd>(2 * Log2((root?.Size ?? 0) + 1));
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
                    N.Push(node);
                    N.Update(node);
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
                    N.Push(node);
                    N.Update(node);
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


    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    public class SplayTreeNodeBase<Nd, T> : IBbstNode
        where Nd : SplayTreeNodeBase<Nd, T>
    {
        public Nd left, right;
        internal Nd Parent;
        public int Size { get; internal set; }
        public T Value { get; internal set; }
        public T Sum { get; internal set; }
        public bool IsRoot => Parent == null || Parent.left != this && Parent.right != this;
        public static IEnumerator<T> GetEnumerator<N>(ref Nd t) where N : ISplayTreePusher<T, Nd, N>
            => N.GetEnumerator(ref t);

        [SourceExpander.NotEmbeddingSource]
        public override string ToString() => $"Size = {this.Size}";
    }
}