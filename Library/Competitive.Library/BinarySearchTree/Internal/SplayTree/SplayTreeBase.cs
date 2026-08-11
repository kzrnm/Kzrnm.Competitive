using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    /// <summary>
    /// なにかしらを遅延伝播させる
    /// </summary>
    public interface ISplayTreePusher<T, Nd, R, N, C> : IBbstOp<T, R, N>, IBbstCnv<Nd, R, N, C>
        where Nd : ISplayTreeNode<T, R>
        where N : ISplayTreePusher<T, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        /// <summary>
        /// なにかしらを遅延伝播させる
        /// </summary>
        static virtual void Push(R t) { }

        /// <summary>
        /// モノイドの積
        /// </summary>
        static abstract T Prod(T x, T y);

        [凾(256)]
        static void RotateR(R t)
        {
            ref Nd d = ref C.Load(t);
            var x = d.Parent;
            var y = C.Load(x).Parent;
            if (!C.IsNull(C.Load(x).Left = d.Right))
                C.Load(d.Right).Parent = x;

            d.Right = x;
            C.Load(x).Parent = t;
            N.Update(x);
            N.Update(t);
            if (!C.IsNull(d.Parent = y))
            {
                if (EqualityComparer<R>.Default.Equals(C.Load(y).Left, x)) C.Load(y).Left = t;
                if (EqualityComparer<R>.Default.Equals(C.Load(y).Right, x)) C.Load(y).Right = t;
                N.Update(y);
            }
        }
        [凾(256)]
        static void RotateL(R t)
        {
            ref Nd d = ref C.Load(t);
            var x = d.Parent;
            var y = C.Load(x).Parent;
            if (!C.IsNull(C.Load(x).Right = d.Left))
                C.Load(d.Left).Parent = x;

            d.Left = x;
            C.Load(x).Parent = t;
            N.Update(x);
            N.Update(t);
            if (!C.IsNull(d.Parent = y))
            {
                if (EqualityComparer<R>.Default.Equals(C.Load(y).Left, x)) C.Load(y).Left = t;
                if (EqualityComparer<R>.Default.Equals(C.Load(y).Right, x)) C.Load(y).Right = t;
                N.Update(y);
            }
        }

        [凾(256)]
        static void Splay(R t)
        {
            N.Push(t);
            ref Nd d = ref C.Load(t);
            while (!IsRoot(t))
            {
                var q = d.Parent;
                ref Nd qd = ref C.Load(q);
                if (IsRoot(q))
                {
                    N.Push(q); N.Push(t);
                    if (EqualityComparer<R>.Default.Equals(qd.Left, t)) RotateR(t);
                    else RotateL(t);
                }
                else
                {
                    var r = qd.Parent;
                    N.Push(r); N.Push(q); N.Push(t);
                    if (EqualityComparer<R>.Default.Equals(C.Load(r).Left, q))
                    {
                        if (EqualityComparer<R>.Default.Equals(qd.Left, t)) { RotateR(q); RotateR(t); }
                        else { RotateL(t); RotateR(t); }
                    }
                    else
                    {
                        if (EqualityComparer<R>.Default.Equals(qd.Right, t)) { RotateL(q); RotateL(t); }
                        else { RotateR(t); RotateL(t); }
                    }
                }
            }
        }


        /// <summary>
        /// 先頭に <paramref name="newNode"/> を追加します。
        /// </summary>
        [凾(256)]
        static void IBbstOp<T, R, N>.AddFirst(ref R t, R newNode)
        {
            if (C.IsNull(t))
            {
                t = newNode;
            }
            else
            {
                Splay(t);
                R cur = t, z = newNode;
                while (!C.IsNull(C.Load(cur).Left))
                {
                    N.Push(cur);
                    cur = C.Load(cur).Left;
                }
                Splay(cur);
                C.Load(z).Parent = cur;
                C.Load(cur).Left = z;
                Splay(z);
                t = z;
            }
        }

        [凾(256)]
        static void IBbstOp<T, R, N>.AddLast(ref R t, R newNode)
        {
            if (C.IsNull(t))
            {
                t = newNode;
            }
            else
            {
                Splay(t);
                R cur = t, z = newNode;
                while (!C.IsNull(C.Load(cur).Right))
                {
                    N.Push(cur);
                    cur = C.Load(cur).Right;
                }
                Splay(cur);
                C.Load(z).Parent = cur;
                C.Load(cur).Right = z;
                Splay(z);
                t = z;
            }
        }

        [凾(256)]
        static R IBbstOp<R, N>.Merge(R l, R r)
        {
            if (C.IsNull(l))
            {
                if (!C.IsNull(r))
                    Splay(r);
                return r;
            }
            if (C.IsNull(r))
            {
                Splay(l);
                return l;
            }

            Splay(l); Splay(r);
            while (!C.IsNull(C.Load(l).Right))
            {
                N.Push(l);
                l = C.Load(l).Right;
            }
            Splay(l);
            C.Load(l).Right = r;
            C.Load(r).Parent = l;
            N.Update(l);
            return l;
        }
        [凾(256)]
        static (R, R) IBbstOp<R, N>.Split(R t, int k)
        {
            if (C.IsNull(t)) return (C.Null, C.Null);
            N.Push(t);
            var lc = N.Size(C.Load(t).Left);
            if (k <= lc)
            {
                var (x1, x2) = N.Split(C.Load(t).Left, k);
                ref Nd d = ref C.Load(t);
                d.Left = x2;
                d.Parent = C.Null;
                if (!C.IsNull(x2)) C.Load(x2).Parent = t;
                return (x1, N.Update(t));
            }
            else
            {
                var (x1, x2) = N.Split(C.Load(t).Right, k - lc - 1);
                ref Nd d = ref C.Load(t);
                d.Right = x1;
                d.Parent = C.Null;
                if (!C.IsNull(x1)) C.Load(x1).Parent = t;
                return (N.Update(t), x2);
            }
        }
        [凾(256)]
        static R IBbstOp<T, R, N>.SetValue(R t, int k, T x)
        {
            Splay(t);
            return SetValueSub(t, k, x);
        }

        [凾(256)]
        static R SetValueSub(R t, int k, T x)
        {
            N.Push(t);
            ref Nd d = ref C.Load(t);
            var lc = N.Size(d.Left);
            if (k < lc)
                return SetValueSub(d.Left, k, x);

            else if (k == lc)
            {
                d.Value = x;
                Splay(t);
                return t;
            }
            else
                return SetValueSub(d.Right, k - lc - 1, x);
        }
        [凾(256)]
        static R IBbstOp<T, R, N>.GetValue(R t, int k, out T x)
        {
            Splay(t);
            x = C.Load(t = ElementAt(t, k)).Value;
            return t;
        }

        [凾(256)]
        static R ElementAt(R t, int k)
        {
            N.Push(t);
            ref Nd d = ref C.Load(t);
            var lc = N.Size(d.Left);
            if (k < lc)
                return ElementAt(d.Left, k);

            else if (k == lc)
            {
                Splay(t);
                return t;
            }
            else
                return ElementAt(d.Right, k - lc - 1);
        }

        [凾(256)]
        static R IBbstOp<R, N>.Propagate(R t)
        {
            if (!C.IsNull(t))
                Splay(t);
            return t;
        }

        [凾(256)]
        static R IBbstOp<R, N>.Update(R t)
        {
            if (C.IsNull(t)) return t;
            ref Nd d = ref C.Load(t);
            var lsum = N.Sum(d.Left);
            var rsum = N.Sum(d.Right);
            d = ref C.Load(t);
            d.Size = N.Size(d.Left) + N.Size(d.Right) + 1;
            d.Sum = N.Prod(N.Prod(lsum, d.Value), rsum);
            return t;
        }

        [凾(256)]
        static bool IsRoot(R t)
        {
            var p = C.Load(t).Parent;
            if (C.IsNull(p))
                return true;
            ref Nd pd = ref C.Load(p);
            return EqualityComparer<R>.Default.Equals(pd.Left, t) && EqualityComparer<R>.Default.Equals(pd.Right, t);
        }

        static IEnumerator<T> IBbstOp<T, R, N>.GetEnumerator(ref R t)
        {
            t = N.Propagate(t);
            return new Enumerator(t);
        }

        public sealed class Enumerator : IEnumerator<T>
        {
            readonly Stack<R> stack;
            R current, root;

            readonly bool reverse;
            internal Enumerator(R root) : this(root, false) { }
            internal Enumerator(R root, bool reverse)
            {
                this.root = root;
                stack = new Stack<R>(2 * Log2(N.Size(root) + 1));
                current = C.Null;
                this.reverse = reverse;
                IntializeAll();
            }
            [凾(256)]
            void IntializeAll()
            {
                var node = root;
                while (!C.IsNull(node))
                {
                    N.Push(node);
                    N.Update(node);
                    ref Nd d = ref C.Load(node);
                    var next = reverse ? d.Right : d.Left;
                    stack.Push(node);
                    node = next;
                }
            }

            [凾(256)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public T Current => C.Load(current).Value;

            [凾(256)]
            public bool MoveNext()
            {
                if (!stack.TryPop(out current))
                {
                    current = C.Null;
                    return false;
                }
                var node = reverse ? C.Load(current).Left : C.Load(current).Right;
                while (!C.IsNull(node))
                {
                    N.Push(node);
                    N.Update(node);
                    ref Nd d = ref C.Load(node);
                    var next = reverse ? d.Right : d.Left;
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

    public interface ISplayTreeNode<T, R> : IBbstNode<T, R>
    {
        R Parent { get; set; }
        T Value { get; set; }
    }
}