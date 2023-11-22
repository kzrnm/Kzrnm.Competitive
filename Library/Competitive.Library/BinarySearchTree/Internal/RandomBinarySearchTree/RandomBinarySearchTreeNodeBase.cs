using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/randomized-binary-search-tree-lazy.hpp
    public class RandomBinarySearchTreeNodeBase
    {
        internal static Xoshiro256 rnd = new();
    }

    public class RandomBinarySearchTreeNodeBase<TSelf, T> : RandomBinarySearchTreeNodeBase
        where TSelf : RandomBinarySearchTreeNodeBase<TSelf, T>, IBbstNode<T, TSelf>
    {
        public TSelf left, right;
        public int Size { get; protected set; }
        public T Value { get; protected set; }
        public T Sum { get; protected set; }

        public static TSelf Merge(TSelf l, TSelf r)
        {
            if (l == null || r == null)
                return l ?? r;
            if ((int)((rnd.NextUInt64() * (ulong)(l.Size + r.Size)) >> 32) < l.Size)
            {
                TSelf.Propagate(ref l);
                l.right = Merge(l.right, r);
                return TSelf.Update(l);
            }
            else
            {
                TSelf.Propagate(ref r);
                r.left = Merge(l, r.left);
                return TSelf.Update(r);
            }
        }
        public static (TSelf, TSelf) Split(TSelf t, int k)
        {
            if (t == null) return (null, null);
            TSelf.Propagate(ref t);

            var l = t.left;
            var r = t.right;
            var lc = l?.Size ?? 0;
            if (k <= lc)
            {
                var (p1, p2) = Split(l, k);
                t.left = p2;
                return (p1, TSelf.Update(t));
            }
            else
            {
                var (p1, p2) = Split(r, k - lc - 1);
                t.right = p1;
                return (TSelf.Update(t), p2);
            }
        }

        [凾(256)]
        public static void SetValue(ref TSelf t, int k, T x)
        {
            TSelf.Propagate(ref t);
            var lc = t.left?.Size ?? 0;
            if (k < lc)
                SetValue(ref t.left, k, x);
            else if (k == lc)
                t.Value = x;
            else
                SetValue(ref t.right, k - lc - 1, x);
            t = TSelf.Update(t);
        }

        [凾(256)]
        public static T GetValue(ref TSelf t, int k)
        {
            TSelf.Propagate(ref t);
            var lc = t.left?.Size ?? 0;
            if (k < lc)
                return GetValue(ref t.left, k);
            else if (k == lc)
                return t.Value;
            else
                return GetValue(ref t.right, k - lc - 1);
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
            T cur;
            Stack<TSelf> stack;
            public Enumerator(TSelf t)
            {
                cur = default;
                stack = new Stack<TSelf>();
                IntializeAll(t);
            }
            [凾(256)]
            void IntializeAll(TSelf t)
            {
                while (t != null)
                {
                    TSelf.Propagate(ref t);
                    //var next = reverse ? t.right : t.left;
                    stack.Push(t);
                    t = t.left;
                }
            }

            public T Current => cur;
            object IEnumerator.Current => cur;

            public bool MoveNext()
            {
                if (stack.TryPop(out var t))
                {
                    cur = t.Value;
                    //t = reverse ? t.left : t.right;
                    t = t.right;
                    while (t != null)
                    {
                        TSelf.Propagate(ref t);
                        //var next = reverse ? t.right : t.left;
                        stack.Push(t);
                        t = t.left;
                    }
                    return true;
                }
                return false;
            }
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();
        }
    }
}