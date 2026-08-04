using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/randomized-binary-search-tree-lazy.hpp
    public class RandomBinarySearchTreeNodeBase : IBbstNode
    {
        internal static Xoshiro256 rnd = new();
        public int Size { get; protected set; }

        [SourceExpander.NotEmbeddingSource]
        public override string ToString() => $"Size = {this.Size}";
    }

    public interface IRbstNodeOp<T, Nd, N> : IBbstNodeOp<T, Nd, N>
        where Nd : RandomBinarySearchTreeNodeBase<Nd, T>
        where N : IRbstNodeOp<T, Nd, N>
    {
        [凾(256)]
        static virtual ulong NextUInt64() => RandomBinarySearchTreeNodeBase.rnd.NextUInt64();
        [凾(256)]
        static Nd IBbstNodeOp<Nd, N>.Merge(Nd l, Nd r)
        {
            if (l == null || r == null)
                return l ?? r;
            if ((int)((N.NextUInt64() * (ulong)(l.Size + r.Size)) >> 32) < l.Size)
            {
                N.Propagate(ref l);
                l.right = N.Merge(l.right, r);
                return N.Update(l);
            }
            else
            {
                N.Propagate(ref r);
                r.left = N.Merge(l, r.left);
                return N.Update(r);
            }
        }
        [凾(256)]
        static (Nd, Nd) IBbstNodeOp<Nd, N>.Split(Nd t, int k)
        {
            if (t == null) return (null, null);
            N.Propagate(ref t);

            var l = t.left;
            var r = t.right;
            var lc = l?.Size ?? 0;
            if (k <= lc)
            {
                var (p1, p2) = N.Split(l, k);
                t.left = p2;
                return (p1, N.Update(t));
            }
            else
            {
                var (p1, p2) = N.Split(r, k - lc - 1);
                t.right = p1;
                return (N.Update(t), p2);
            }
        }

        [凾(256)]
        static void IBbstNodeOp<T, Nd, N>.SetValue(ref Nd t, int k, T x)
        {
            N.Propagate(ref t);
            var lc = t.left?.Size ?? 0;
            if (k < lc)
                N.SetValue(ref t.left, k, x);
            else if (k == lc)
                t.Value = x;
            else
                N.SetValue(ref t.right, k - lc - 1, x);
            t = N.Update(t);
        }

        [凾(256)]
        static T IBbstNodeOp<T, Nd, N>.GetValue(ref Nd t, int k)
        {
            N.Propagate(ref t);
            var lc = t.left?.Size ?? 0;
            if (k < lc)
                return N.GetValue(ref t.left, k);
            else if (k == lc)
                return t.Value;
            else
                return N.GetValue(ref t.right, k - lc - 1);
        }

        static IEnumerator<T> IBbstNodeOp<T, Nd, N>.GetEnumerator(ref Nd t)
        {
            N.Propagate(ref t);
            return new Enumerator(t);
        }

        public sealed class Enumerator : IEnumerator<T>
        {
            T cur;
            Stack<Nd> stack;
            public Enumerator(Nd t)
            {
                cur = default;
                stack = new Stack<Nd>();
                IntializeAll(t);
            }
            [凾(256)]
            void IntializeAll(Nd t)
            {
                while (t != null)
                {
                    N.Propagate(ref t);
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
                        N.Propagate(ref t);
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

    public class RandomBinarySearchTreeNodeBase<Nd, T> : RandomBinarySearchTreeNodeBase
        where Nd : RandomBinarySearchTreeNodeBase<Nd, T>
    {
        public Nd left, right;
        public T Value { get; internal set; }
        public T Sum { get; internal set; }

        public static IEnumerator<T> GetEnumerator<N>(ref Nd t) where N : IRbstNodeOp<T, Nd, N>
            => N.GetEnumerator(ref t);
    }
}