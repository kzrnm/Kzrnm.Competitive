using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

#pragma warning disable IDE0251

namespace Kzrnm.Competitive.Internal
{
    public interface IRedBlackTreeNode<T, Node> : IBbstNode<T, Node> where Node : class, IRedBlackTreeNode<T, Node>
    {
        static abstract Node Create(Node left, Node right);
    }

    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 赤黒木のノード
    /// </summary>
    public class RedBlackTreeNodeBase<TSelf, T>
        where TSelf : RedBlackTreeNodeBase<TSelf, T>, IRedBlackTreeNode<T, TSelf>
    {
        public abstract class D { }
        public class Internal : D
        {
            public TSelf left, right;
            public int Level;
        }
        public class Leaf : D
        {
            public T Value { get; internal set; }
        }
        public bool IsBlack;
        public int Size { get; protected set; }
        public T Sum { get; protected set; }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public D Data;

        public static TSelf Merge(TSelf l, TSelf r)
        {
            if (l == null || r == null) return l ?? r;
            var c = SubMerge(l, r);
            c.IsBlack = true;
            return c;
        }

        [凾(256)]
        static TSelf SubMerge(TSelf l, TSelf r)
        {
            var li = l.Data as Internal;
            var ri = r.Data as Internal;

            var ld = (li?.Level ?? 0).CompareTo(ri?.Level ?? 0);

            if (ld < 0)
            {
                Debug.Assert(ri != null);
                TSelf.Propagate(ref r);
                var c = ri.left = SubMerge(l, ri.left);
                var ci = Unsafe.As<Internal>(c.Data);
                if (r.IsBlack && !c.IsBlack && (ci.left?.IsBlack == false))
                {
                    r.IsBlack = false;
                    c.IsBlack = true;
                    if (ri.right.IsBlack)
                        return RotateRight(r, c);
                    ri.right.IsBlack = true;
                }
                return TSelf.Update(r);
            }
            else if (ld > 0)
            {
                Debug.Assert(li != null);
                TSelf.Propagate(ref l);
                var c = li.right = SubMerge(li.right, r);
                var ci = Unsafe.As<Internal>(c.Data);
                if (l.IsBlack && !c.IsBlack && (ci.right?.IsBlack == false))
                {
                    l.IsBlack = false;
                    c.IsBlack = true;
                    if (li.left.IsBlack)
                        return RotateLeft(l, c);
                    li.left.IsBlack = true;
                }
                return TSelf.Update(l);
            }
            return TSelf.Create(l, r);
        }
        [凾(256)]
        static TSelf RotateRight(TSelf t, TSelf c)
        {
            /*
             *         t
             *        / \
             *       c   ...
             *      / \
             *     P   Q
             * ↓
             *       c
             *      / \
             *     P   t
             *        /  \
             *       Q   ...
             */

            TSelf.Propagate(ref t);
            TSelf.Propagate(ref c);
            Unsafe.As<Internal>(t.Data).left = Unsafe.As<Internal>(c.Data).right;
            Unsafe.As<Internal>(c.Data).right = t;
            TSelf.Update(t);
            return TSelf.Update(c);
        }
        [凾(256)]
        static TSelf RotateLeft(TSelf t, TSelf c)
        {
            /*
             *     t
             *    / \
             *  ...  c
             *      / \
             *     P   Q
             * ↓
             *      c
             *     / \
             *    t   Q
             *   / \ 
             * ...  P
             */


            TSelf.Propagate(ref t);
            TSelf.Propagate(ref c);
            Unsafe.As<Internal>(t.Data).right = Unsafe.As<Internal>(c.Data).left;
            Unsafe.As<Internal>(c.Data).left = t;
            TSelf.Update(t);
            return TSelf.Update(c);
        }

        public static (TSelf, TSelf) Split(TSelf t, int k)
        {
            if (t == null) return (null, null);
            TSelf.Propagate(ref t);
            if (k == 0) return (null, t);
            if (k >= t.Size) return (t, null);
            if (t.Data is Internal tt)
            {
                var lc = tt.left.Size;
                if (k < lc)
                {
                    var (p1, p2) = TSelf.Split(tt.left, k);
                    return (p1, TSelf.Merge(p2, tt.right));
                }
                else
                {
                    var (p1, p2) = TSelf.Split(tt.right, k - lc);
                    return (TSelf.Merge(tt.left, p1), p2);
                }
            }
            return (null, t);
        }
        public static void SetValue(ref TSelf t, int k, T x)
        {
            TSelf.Propagate(ref t);
            switch (t.Data)
            {
                case Internal tt:
                    var lc = tt.left.Size;
                    if (k < lc)
                        SetValue(ref tt.left, k, x);
                    else
                        SetValue(ref tt.right, k - lc, x);
                    break;
                case Leaf lf:
                    lf.Value = x;
                    break;
            }
            t = TSelf.Update(t);
        }

        public static T GetValue(ref TSelf t, int k)
        {
            TSelf.Propagate(ref t);
            switch (t.Data)
            {
                case Internal tt:
                    var lc = tt.left.Size;
                    if (k < lc)
                        return GetValue(ref tt.left, k);
                    else
                        return GetValue(ref tt.right, k - lc);
                case Leaf lf:
                    return lf.Value;
                default:
                    return Throw<T>();
            }
        }

        public Enumerator GetEnumerator() => new(Unsafe.As<TSelf>(this));
        public static IEnumerator<T> GetEnumerator(ref TSelf t)
        {
            TSelf.Propagate(ref t);
            return new Enumerator(t);
        }
        static U Throw<U>() => throw new InvalidOperationException();
        public struct Enumerator : IEnumerator<T>
        {
            T cur;
            Stack<TSelf> stack;
            public Enumerator(TSelf t)
            {
                cur = default;
                stack = new Stack<TSelf>();
                if (t != null)
                    stack.Push(t);
            }

            public T Current => cur;
            object IEnumerator.Current => cur;

            public bool MoveNext()
            {
                while (stack.TryPop(out var t))
                {
                    TSelf.Propagate(ref t);
                    switch (t.Data)
                    {
                        case Internal tt:
                            if (tt.right != null) stack.Push(tt.right);
                            if (tt.left != null) stack.Push(tt.left);
                            break;
                        case Leaf lf:
                            cur = lf.Value;
                            return true;
                    }
                }
                return false;
            }
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();
        }
    }
}