using AtCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://github.com/yosupo06/library-checker-problems/blob/master/data_structure/dynamic_sequence_range_affine_range_sum/sol/correct.cpp
    public interface IAvlNode<T, R> : IBbstNode<T, R>
    {
        int Height { get; set; }
        T Value { get; set; }
    }

    public interface IAvlOp<T, TOp, Nd, R, N, C> : IBbstOp<T, R, N>, IBbstCnv<Nd, R, N, C>
        where TOp : struct, ISegtreeOperator<T>
        where Nd : IAvlNode<T, R>
        where N : IAvlOp<T, TOp, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        [SourceExpander.NotEmbeddingSource]
        static void IBbstOp<R, N>.Validate(R t)
        {
            if (C.IsNull(t)) return;
            ref var d = ref C.Load(t);
            if (d.Height < 0)
                throw new InvalidProgramException("Height は非負");

            if (Math.Abs(HeightDiff(t)) > 1)
                throw new InvalidProgramException("左右のレベル差は1以下");

            N.Validate(d.Left);
            N.Validate(d.Right);
        }

        [凾(256)]
        static R IBbstOp<R, N>.Propagate(R t) => N.Copy(t);

        /// <summary>
        /// モノイド<paramref name="l"/>, <paramref name="r"/>] の総積を返します。
        /// </summary>
        [凾(256)]
        static T Prod(T l, T r) => new TOp().Operate(l, r);

        [凾(256)]
        static T IBbstOp<T, R, N>.Sum(R t) => C.IsNull(t) ? new TOp().Identity : C.Load(t).Sum;

        [凾(256)]
        static int Height(R t)
        {
            if (C.IsNull(t)) return 0;
            return C.Load(t).Height;
        }

        [凾(256)]
        static int HeightDiff(R t)
        {
            Debug.Assert(!C.IsNull(t));
            ref var d = ref C.Load(t);
            return Height(d.Left) - Height(d.Right);
        }


        [凾(256)]
        static R RotateRight(R t)
        {
            /*  RotateRight
             * 
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

            t = N.Propagate(t);
            ref var d = ref C.Load(t);
            var c = N.Propagate(d.Left);
            ref var cn = ref C.Load(c);
            d.Left = cn.Right;
            cn.Right = N.Update(t);
            return N.Update(c);
        }

        [凾(256)]
        static R RotateLeft(R t)
        {
            /* RotateLeft
             * 
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

            t = N.Propagate(t);
            ref var d = ref C.Load(t);
            var c = N.Propagate(d.Right);
            ref var cn = ref C.Load(c);
            d.Right = cn.Left;
            cn.Left = N.Update(t);
            return N.Update(c);
        }

        /// <summary>
        /// <paramref name="t"/> の左右のレベル差を補正します。
        /// </summary>
        [凾(256)]
        static R Balance(R t)
        {
            Debug.Assert(Math.Abs(HeightDiff(t)) <= 2);
            ref var d = ref C.Load(t);
            switch (HeightDiff(t))
            {
                case 2:
                    if (HeightDiff(d.Left) < 0)
                        d.Left = RotateLeft(d.Left);
                    return RotateRight(t);
                case -2:
                    if (HeightDiff(d.Right) > 0)
                        d.Right = RotateRight(d.Right);
                    return RotateLeft(t);
                default:
                    return N.Update(t);
            }
        }

        [凾(256)]
        static (R, R) IBbstOp<R, N>.Split(R t, int k)
        {
            if (C.IsNull(t)) return (C.Null, C.Null);
            t = N.Propagate(t);
            if (k == 0) return (C.Null, t);
            if (k >= N.Size(t)) return (t, C.Null);

            ref var d = ref C.Load(t);
            var l = d.Left;
            var r = d.Right;

            d.Left = d.Right = C.Null;
            var lc = N.Size(l);
            if (k < lc)
            {
                var (p1, p2) = N.Split(l, k);
                return (p1, MergeWithRoot(p2, t, r));
            }
            else if (k > lc)
            {
                var (p1, p2) = N.Split(r, k - lc - 1);
                return (MergeWithRoot(l, t, p1), p2);
            }
            return (l, MergeWithRoot(C.Null, t, r));
        }

        [凾(256)]
        static R IBbstOp<R, N>.Merge(R l, R r)
        {
            if (C.IsNull(l))
                return r;
            if (C.IsNull(r))
                return l;
            l = RemoveRightest(l, out R m);
            return MergeWithRoot(l, m, r);
        }
        [凾(256)]
        static R MergeWithRoot(R l, R root, R r)
        {
            Debug.Assert(!C.IsNull(root));

            switch (Height(l) - Height(r))
            {
                case <= 1 and >= -1:
                    ref var d = ref C.Load(root);
                    d.Left = l;
                    d.Right = r;
                    return N.Update(root);
                case > 0:
                    Debug.Assert(!C.IsNull(l));
                    l = N.Propagate(l);
                    ref var ld = ref C.Load(l);
                    ld.Right = MergeWithRoot(ld.Right, root, r);
                    l = Balance(l);
                    return l;
                default:
                    Debug.Assert(!C.IsNull(r));
                    r = N.Propagate(r);
                    ref var rd = ref C.Load(r);
                    rd.Left = MergeWithRoot(l, root, rd.Left);
                    r = Balance(r);
                    return r;
            }
        }

        /// <summary>
        /// <paramref name="t"/> で最も右のノードを削除します。削除したノードは <paramref name="m"/> に代入します。
        /// </summary>
        /// <returns>更新後の <paramref name="t"/></returns>
        [凾(256)]
        static R RemoveRightest(R t, out R m)
        {
            t = N.Propagate(t);
            ref var d = ref C.Load(t);
            if (C.IsNull(d.Right))
            {
                m = t;
                return d.Left;
            }
            else
            {
                d.Right = RemoveRightest(d.Right, out m);
                return Balance(t);
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

            if (l == 0 && N.Size(t) <= r)
                return N.Sum(t);

            ref Nd d = ref C.Load(t);
            var lc = N.Size(d.Left);
            R rr = d.Right;
            if (lc <= l)
            {
                try
                {
                    if (lc != l)
                        return N.Prod(ref rr, l - lc - 1, r - lc - 1);

                    return Prod(d.Value, N.Prod(ref rr, 0, r - lc - 1));
                }
                finally
                {
                    d = ref C.Load(t);
                    d.Right = rr;
                }
            }
            R ll = d.Left;
            if (r <= lc + 1)
            {
                try
                {
                    if (r <= lc)
                        return N.Prod(ref ll, l, r);

                    return Prod(N.Prod(ref ll, l, lc), d.Value);
                }
                finally
                {
                    d = ref C.Load(t);
                    d.Left = ll;
                }
            }

            try
            {
                var lt = N.Prod(ref ll, l, lc);
                var rt = N.Prod(ref rr, 0, r - lc - 1);

                return Prod(Prod(lt, d.Value), rt);
            }
            finally
            {
                d = ref C.Load(t);
                d.Left = ll;
                d.Right = rr;
            }
        }

        [凾(256)]
        static R IBbstOp<R, N>.Update(R t)
        {
            if (C.IsNull(t)) return t;

            ref var d = ref C.Load(t);
            d.Left = N.Propagate(d.Left);
            d.Right = N.Propagate(d.Right);
            d.Size = N.Size(d.Left) + N.Size(d.Right) + 1;
            d.Sum = Prod(Prod(N.Sum(d.Left), d.Value), N.Sum(d.Right));
            d.Height = 1 + Math.Max(Height(d.Left), Height(d.Right));
            return t;
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
                stack = new Stack<R>(C.Load(root).Height * 2 + 2);
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
                    node = N.Propagate(node);
                    ref var d = ref C.Load(node);
                    var next = reverse ? d.Right : d.Left;
                    stack.Push(node);
                    node = next;
                }
            }

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
                    node = N.Propagate(node);
                    ref var d = ref C.Load(node);
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
}