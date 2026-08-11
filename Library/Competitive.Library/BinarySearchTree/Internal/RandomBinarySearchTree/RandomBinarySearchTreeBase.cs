using AtCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://ei1333.github.io/library/structure/bbst/randomized-binary-search-tree-lazy.hpp
    static class RbstRandom
    {
        internal static Xoshiro256 rnd = new();
    }

    public interface IRbstOp<T, TOp, Nd, R, N, C> : IBbstOp<T, R, N>, IBbstCnv<Nd, R, N, C>
        where TOp : struct, ISegtreeOperator<T>
        where Nd : IRbstNode<T, R>
        where N : IRbstOp<T, TOp, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        /// <summary>
        /// モノイドの積
        /// </summary>
        static virtual T Prod(T x, T y) => new TOp().Operate(x, y);

        [凾(256)] static T IBbstOp<T, R, N>.Sum(R t) => C.IsNull(t) ? new TOp().Identity : C.Load(t).Sum;

        [凾(256)]
        static ulong NextUInt64() => RbstRandom.rnd.NextUInt64();
        [凾(256)]
        static R IBbstOp<R, N>.Merge(R l, R r)
        {
            if (C.IsNull(l))
                return r;
            if (C.IsNull(r))
                return l;

            if ((int)((NextUInt64() * (ulong)(N.Size(l) + N.Size(r))) >> 32) < N.Size(l))
            {
                l = N.Propagate(l);
                ref Nd ld = ref C.Load(l);
                ld.Right = N.Merge(ld.Right, r);
                return N.Update(l);
            }
            else
            {
                r = N.Propagate(r);
                ref Nd rd = ref C.Load(r);
                rd.Left = N.Merge(l, rd.Left);
                return N.Update(r);
            }
        }
        [凾(256)]
        static (R, R) IBbstOp<R, N>.Split(R t, int k)
        {
            if (C.IsNull(t)) return (C.Null, C.Null);
            t = N.Propagate(t);

            ref Nd d = ref C.Load(t);

            var l = d.Left;
            var r = d.Right;
            var lc = N.Size(l);
            if (k <= lc)
            {
                var (p1, p2) = N.Split(l, k);
                d.Left = p2;
                return (p1, N.Update(t));
            }
            else
            {
                var (p1, p2) = N.Split(r, k - lc - 1);
                d.Right = p1;
                return (N.Update(t), p2);
            }
        }

        [凾(256)]
        static R IBbstOp<T, R, N>.SetValue(R t, int k, T x)
        {
            t = N.Propagate(t);

            ref var d = ref C.Load(t);
            var lc = N.Size(d.Left);
            if (k < lc)
                d.Left = N.SetValue(d.Left, k, x);
            else if (k == lc)
                d.Value = x;
            else
                d.Right = N.SetValue(d.Right, k - lc - 1, x);
            return N.Update(t);
        }

        [凾(256)]
        static R IBbstOp<T, R, N>.GetValue(R t, int k, out T x)
        {
            t = N.Propagate(t);

            ref var d = ref C.Load(t);
            var lc = N.Size(d.Left);
            if (k < lc)
                d.Left = N.GetValue(d.Left, k, out x);
            else if (k == lc)
                x = d.Value;
            else
                d.Right = N.GetValue(d.Right, k - lc - 1, out x);
            return t;
        }

        [凾(256)]
        static R IBbstOp<R, N>.Update(R t)
        {
            if (C.IsNull(t)) return t;
            ref var nd = ref C.Load(t);
            nd.Size = N.Size(nd.Left) + N.Size(nd.Right) + 1;
            nd.Sum = N.Prod(N.Prod(N.Sum(nd.Left), nd.Value), N.Sum(nd.Right));
            return t;
        }

        static IEnumerator<T> IBbstOp<T, R, N>.GetEnumerator(ref R t)
        {
            t = N.Propagate(t);
            return new Enumerator(t);
        }

        public sealed class Enumerator : IEnumerator<T>
        {
            T cur;
            Stack<R> stack;
            public Enumerator(R t)
            {
                cur = default;
                stack = new Stack<R>();
                IntializeAll(t);
            }
            [凾(256)]
            void IntializeAll(R t)
            {
                while (!C.IsNull(t))
                {
                    t = N.Propagate(t);
                    //var next = reverse ? t.Right : t.Left;
                    stack.Push(t);
                    t = C.Load(t).Left;
                }
            }

            public T Current => cur;
            object IEnumerator.Current => cur;

            public bool MoveNext()
            {
                if (stack.TryPop(out var t))
                {
                    cur = C.Load(t).Value;
                    //t = reverse ? t.Left : t.Right;
                    t = C.Load(t).Right;
                    while (!C.IsNull(t))
                    {
                        t = N.Propagate(t);
                        //var next = reverse ? t.Right : t.Left;
                        stack.Push(t);
                        t = C.Load(t).Left;
                    }
                    return true;
                }
                return false;
            }
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();
        }
    }

    public interface IRbstNode<T, R> : IBbstNode<T, R>
    {
        R Parent { get; set; }
        T Value { get; set; }
    }
}