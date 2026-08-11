using AtCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    public interface IRbtOp<T, TOp, Nd, R, N, C> : IBbstOp<T, R, N>, IBbstCnv<Nd, R, N, C>
        where TOp : struct, ISegtreeOperator<T>
        where Nd : IRbtNode<T, R>
        where N : IRbtOp<T, TOp, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        static R Create(R left, R right)
        {
            var sum = Prod(N.Sum(left), N.Sum(right));
            var t = N.Create(sum);
            ref Nd d = ref C.Load(t);
            d.IsBlack = false;
            d.Left = left;
            d.Right = right;
            d.Level = UpperLevel(left);
            d.Size = N.Size(left) + N.Size(right);
            return t;

        }

        /// <summary>
        /// モノイド<paramref name="l"/>, <paramref name="r"/>] の総積を返します。
        /// </summary>
        [凾(256)]
        static T Prod(T l, T r) => new TOp().Operate(l, r);

        [凾(256)]
        static T IBbstOp<T, R, N>.Sum(R t) => C.IsNull(t) ? new TOp().Identity : C.Load(t).Sum;

        [凾(256)]
        static bool IsLeaf(R t)
        {
            Debug.Assert(!C.IsNull(t));
            ref Nd d = ref C.Load(t);
            Debug.Assert(C.IsNull(d.Left) == C.IsNull(d.Right));
            Debug.Assert(!C.IsNull(d.Left) || d.IsBlack);
            Debug.Assert(!C.IsNull(d.Left) || d.Level == 0);
            Debug.Assert(!C.IsNull(d.Left) || d.Size == 1);
            return C.IsNull(d.Left);
        }
        [凾(256)]
        static bool IsBlack(R t) => C.IsNull(t) || C.Load(t).IsBlack;
        [凾(256)]
        static int UpperLevel(R t)
        {
            if (C.IsNull(t)) return 0;
            ref Nd d = ref C.Load(t);
            return d.Level + (d.IsBlack ? 1 : 0);
        }

        [SourceExpander.NotEmbeddingSource]
        static void IBbstOp<R, N>.Validate(R t)
        {
            if (C.IsNull(t)) return;
            ref Nd d = ref C.Load(t);
            if (IsLeaf(t))
            {
                if (!d.IsBlack)
                    throw new InvalidProgramException("葉は Black であるべき");
                if (d.Size != 1)
                    throw new InvalidProgramException("葉のサイズは 1 であるべき");
                return;
            }
            if (C.IsNull(d.Left))
                throw new InvalidProgramException("左には要素があるはず");
            if (C.IsNull(d.Right))
                throw new InvalidProgramException("右には要素があるはず");
            if (d.Level <= 0)
                throw new InvalidProgramException("Level は正の数");

            if (!d.IsBlack)
            {
                if (!C.Load(d.Left).IsBlack)
                    throw new InvalidProgramException("赤の親は黒");
                if (!C.Load(d.Right).IsBlack)
                    throw new InvalidProgramException("赤の親は黒");
            }
            if (d.Level != UpperLevel(d.Left))
                throw new InvalidProgramException("左とレベルが不整合");
            if (d.Level != UpperLevel(d.Right))
                throw new InvalidProgramException("右とレベルが不整合");

            N.Validate(d.Left);
            N.Validate(d.Right);
        }

        [凾(256)]
        static R IBbstOp<R, N>.Update(R t)
        {
            if (C.IsNull(t)) return t;

            ref Nd d = ref C.Load(t);
            Debug.Assert(!IsLeaf(t) || d.Size == 1);

            if (!C.IsNull(d.Left))
            {
                Debug.Assert(!C.IsNull(d.Right));
                TOp op = new();
                ref Nd l = ref C.Load(d.Left);
                ref Nd r = ref C.Load(d.Right);

                d.Sum = op.Operate(l.Sum, r.Sum);
                d.Size = l.Size + r.Size;
                d.Level = UpperLevel(d.Left);
            }
            return t;
        }

        [凾(256)]
        static R IBbstOp<R, N>.Propagate(R t) => N.Copy(t);

        [凾(256)]
        static (R, R) IBbstOp<R, N>.Split(R t, int k)
        {
            if (C.IsNull(t)) return (C.Null, C.Null);
            t = N.Propagate(t);

            ref Nd d = ref C.Load(t);
            if (k == 0) return (C.Null, t);
            if (k >= d.Size) return (t, C.Null);

            N.Free(t);

            Debug.Assert(!IsLeaf(t));
            var ll = d.Left;
            var rr = d.Right;
            var lc = N.Size(ll);
            if (k < lc)
            {
                var (p1, p2) = N.Split(ll, k);
                return (p1, N.Merge(p2, rr));
            }
            else if (k > lc)
            {
                var (p1, p2) = N.Split(rr, k - lc);
                return (N.Merge(ll, p1), p2);
            }
            return (ll, rr);
        }

        [凾(256)]
        static R IBbstOp<R, N>.Merge(R l, R r)
        {
            if (C.IsNull(l))
                return r;
            if (C.IsNull(r))
                return l;
            var c = SubMerge(l, r);
            C.Load(c).IsBlack = true;
            return c;
        }

        [凾(256)]
        static R SubMerge(R l, R r)
        {
            var lcm = C.Load(l).Level.CompareTo(C.Load(r).Level);
            ref Nd ld = ref C.Load(l);
            ref Nd rd = ref C.Load(r);
            if (lcm == 0)
            {
                if (ld.IsBlack != rd.IsBlack)
                {
                    if (ld.IsBlack)
                    {
                        r = N.Copy(r);
                        C.Load(r).IsBlack = true;
                    }
                    else
                    {
                        l = N.Copy(l);
                        C.Load(l).IsBlack = true;
                    }
                }
                return Create(l, r);
            }
            else if (lcm < 0)
            {
                Debug.Assert(!IsLeaf(r));
                r = N.Propagate(r);
                rd = ref C.Load(r);
                var c = SubMerge(l, rd.Left);
                rd = ref C.Load(r);
                ref Nd cd = ref C.Load(c);
                rd.Left = c;

                if (rd.IsBlack && !cd.IsBlack && !C.Load(cd.Left).IsBlack)
                {
                    rd.IsBlack = false;
                    cd.IsBlack = true;
                    if (C.Load(rd.Right).IsBlack)
                    {
                        /*  RotateRight
                         * 
                         *         r
                         *        / \
                         *       c   ...
                         *      / \
                         *     P   Q
                         * ↓
                         *       c
                         *      / \
                         *     P   r
                         *        /  \
                         *       Q   ...
                         */

                        r = N.Propagate(r);
                        c = N.Propagate(c);
                        C.Load(r).Left = C.Load(c).Right;
                        C.Load(c).Right = r;
                        N.Update(r);
                        return N.Update(c);
                    }
                    C.Load(rd.Right).IsBlack = true;
                }
                return N.Update(r);
            }
            else
            {
                Debug.Assert(!IsLeaf(l));
                l = N.Propagate(l);
                ld = ref C.Load(l);
                var c = SubMerge(ld.Right, r);
                ld = ref C.Load(l);
                ref Nd cd = ref C.Load(c);
                ld.Right = c;

                if (ld.IsBlack && !cd.IsBlack && !C.Load(cd.Right).IsBlack)
                {
                    ld.IsBlack = false;
                    cd.IsBlack = true;
                    if (C.Load(ld.Left).IsBlack)
                    {
                        /* RotateLeft
                         * 
                         *     l
                         *    / \
                         *  ...  c
                         *      / \
                         *     P   Q
                         * ↓
                         *      c
                         *     / \
                         *    l   Q
                         *   / \ 
                         * ...  P
                         */

                        l = N.Propagate(l);
                        c = N.Propagate(c);
                        C.Load(l).Right = C.Load(c).Left;
                        C.Load(c).Left = l;
                        N.Update(l);
                        return N.Update(c);
                    }
                    C.Load(ld.Left).IsBlack = true;
                }
                return N.Update(l);
            }
        }

        /// <summary>
        /// <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
        /// </summary>
        [凾(256)]
        static T IBbstOp<T, R, N>.Prod(ref R t, int l, int r)
        {
            if (l >= r)
                return N.Sum(C.Null);

            t = N.Propagate(t);
            ref Nd d = ref C.Load(t);

            if (l == 0 && d.Size <= r)
                return N.Sum(t);

            int lc = C.Load(d.Left).Size;

            R ll = d.Left;
            R rr = d.Right;

            try
            {
                if (lc <= l)
                    return N.Prod(ref rr, l - lc, r - lc);
                if (r <= lc)
                    return N.Prod(ref ll, l, r);

                var lt = N.Prod(ref ll, l, lc);
                var rt = N.Prod(ref rr, 0, r - lc);

                return Prod(lt, rt);
            }
            finally
            {
                d = ref C.Load(t);
                d.Left = ll;
                d.Right = rr;
            }
        }

        [凾(256)]
        static R IBbstOp<T, R, N>.SetValue(R t, int k, T x)
        {
            t = N.Propagate(t);
            if (IsLeaf(t))
            {
                C.Load(t).Sum = x;
            }
            else
            {
                var lc = N.Size(C.Load(t).Left);
                R c;
                if (k < lc)
                {
                    c = N.SetValue(C.Load(t).Left, k, x);
                    C.Load(t).Left = c;
                }
                else
                {
                    c = N.SetValue(C.Load(t).Right, k - lc, x);
                    C.Load(t).Right = c;
                }
            }
            return N.Update(t);
        }

        [凾(256)]
        static R IBbstOp<T, R, N>.GetValue(R t, int k, out T x)
        {
            t = N.Propagate(t);
            ref Nd d = ref C.Load(t);
            if (IsLeaf(t))
                x = d.Sum;
            else
            {
                var lc = N.Size(d.Left);
                R c;
                if (k < lc)
                {
                    c = N.GetValue(d.Left, k, out x);
                    C.Load(t).Left = c;
                }
                else
                {
                    c = N.GetValue(d.Right, k - lc, out x);
                    C.Load(t).Right = c;
                }
            }
            return t;
        }

        static IEnumerator<T> IBbstOp<T, R, N>.GetEnumerator(ref R t)
        {
            t = N.Propagate(t);
            return new Enumerator(t);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする")]
        public struct Enumerator : IEnumerator<T>
        {
            R start;
            T cur;
            Stack<R> stack;
            public Enumerator(R t)
            {
                start = t;
                Reset();
            }

            public T Current => cur;
            object IEnumerator.Current => cur;

            public bool MoveNext()
            {
                while (stack.TryPop(out var t))
                {
                    t = N.Propagate(t);

                    ref Nd d = ref C.Load(t);
                    if (IsLeaf(t))
                    {
                        cur = d.Sum;
                        return true;
                    }

                    if (!C.IsNull(d.Right)) stack.Push(d.Right);
                    if (!C.IsNull(d.Left)) stack.Push(d.Left);
                }
                return false;
            }
            public void Dispose() { }
            public void Reset()
            {
                cur = default;
                stack = new Stack<R>();
                if (!C.IsNull(start))
                    stack.Push(start);
            }
        }
    }

    public interface IRbtNode<T, R> : IBbstNode<T, R>
    {
        bool IsBlack { get; set; }
        int Level { get; set; }

        [SourceExpander.NotEmbeddingSource]
        string ToStringImpl()
        {
            var black = IsBlack ? "B" : "";
            if (Size == 1)
                return $"Value = {Sum} Level = {Level}{black}";
            else
                return $"Sum = {Sum} Size = {Size} Level = {Level}{black}";
        }
    }
}
