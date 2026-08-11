using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // https://github.com/yosupo06/library-checker-problems/blob/master/data_structure/dynamic_sequence_range_affine_range_sum/sol/correct.cpp
    public interface IAvlNodeOp<T, Nd, N> : IBbstNodeOp<T, Nd, N>
        where Nd : AvlTreeNodeBase<Nd, T>
        where N : IAvlNodeOp<T, Nd, N>
    {
        [SourceExpander.NotEmbeddingSource]
        static void IBbstNodeOp<Nd, N>.Validate(Nd t) => t?.Validate();

        [凾(256)]
        static Nd RotateRight(Nd t)
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

            N.Propagate(ref t);
            var c = t.Left;
            N.Propagate(ref c);
            t.Left = c.Right;
            c.Right = N.Update(t);
            return N.Update(c);
        }

        [凾(256)]
        static Nd RotateLeft(Nd t)
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

            N.Propagate(ref t);
            var c = t.Right;
            N.Propagate(ref c);
            t.Right = c.Left;
            c.Left = N.Update(t);
            return N.Update(c);
        }

        /// <summary>
        /// <paramref name="t"/> の左右のレベル差を補正します。
        /// </summary>
        [凾(256)]
        static void Balance(ref Nd t)
        {
            Debug.Assert(Math.Abs(t.HeightDiff()) <= 2);
            switch (t.HeightDiff())
            {
                case 2:
                    if (t.Left.HeightDiff() < 0)
                        t.Left = RotateLeft(t.Left);
                    t = RotateRight(t);
                    break;
                case -2:
                    if (t.Right.HeightDiff() > 0)
                        t.Right = RotateRight(t.Right);
                    t = RotateLeft(t);
                    break;
                default:
                    t = N.Update(t);
                    break;
            }
        }

        /// <summary>
        /// <paramref name="t"/> で最も右のノードを削除します。
        /// </summary>
        /// <returns>削除したノード</returns>
        [凾(256)]
        static Nd RemoveRightest(ref Nd t)
        {
            N.Propagate(ref t);
            Nd tt;
            if (t.Right == null)
            {
                tt = t;
                t = tt.Left;
            }
            else
            {
                tt = RemoveRightest(ref t.Right);
                Balance(ref t);
            }
            return tt;
        }

        [凾(256)]
        static (Nd, Nd) IBbstNodeOp<Nd, N>.Split(Nd t, int k)
        {
            if (t == null) return (null, null);
            N.Propagate(ref t);
            if (k == 0) return (null, t);
            if (k >= t.Size) return (t, null);

            var l = t.Left;
            var r = t.Right;

            t.Left = t.Right = null;
            var lc = l?.Size ?? 0;
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
            return (l, MergeWithRoot(null, t, r));
        }

        [凾(256)]
        static Nd IBbstNodeOp<Nd, N>.Merge(Nd l, Nd r)
        {
            if (l == null || r == null) return l ?? r;
            var m = RemoveRightest(ref l);
            return MergeWithRoot(l, m, r);
        }
        [凾(256)]
        static Nd MergeWithRoot(Nd l, Nd root, Nd r)
        {
            Debug.Assert(root != null);

            switch ((l?.Height ?? 0) - (r?.Height ?? 0))
            {
                case <= 1 and >= -1:
                    root.Left = l;
                    root.Right = r;
                    return N.Update(root);
                case > 0:
                    Debug.Assert(l != null);
                    N.Propagate(ref l);
                    l.Right = MergeWithRoot(l.Right, root, r);
                    Balance(ref l);
                    return l;
                default:
                    Debug.Assert(r != null);
                    N.Propagate(ref r);
                    r.Left = MergeWithRoot(l, root, r.Left);
                    Balance(ref r);
                    return r;
            }
        }

        /// <summary>
        /// <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
        /// </summary>
        [凾(256)]
        static T IBbstNodeOp<T, Nd, N>.Prod(ref Nd t, int l, int r)
        {
            if (l >= r)
                return N.Sum(null);

            N.Propagate(ref t);

            if (l == 0 && t.Size <= r)
                return N.Sum(t);

            var lc = t.Left?.Size ?? 0;
            if (lc <= l)
            {
                if (lc != l)
                    return N.Prod(ref t.Right, l - lc - 1, r - lc - 1);

                return N.Prod(t.Value, N.Prod(ref t.Right, 0, r - lc - 1));
            }
            if (r <= lc + 1)
            {
                if (r <= lc)
                    return N.Prod(ref t.Left, l, r);

                return N.Prod(N.Prod(ref t.Left, l, lc), t.Value);
            }

            var lt = N.Prod(ref t.Left, l, lc);
            var rt = N.Prod(ref t.Right, 0, r - lc - 1);

            return N.Prod(N.Prod(lt, t.Value), rt);
        }

        /// <summary>
        /// モノイド<paramref name="l"/>, <paramref name="r"/>] の総積を返します。
        /// </summary>
        [凾(256)]
        static abstract T Prod(T l, T r);

        [凾(256)]
        static Nd IBbstNodeOp<Nd, N>.Update(Nd t)
        {
            if (t == null) return t;
            N.Propagate(ref t.Left);
            N.Propagate(ref t.Right);
            t.Size = (t.Left?.Size ?? 0) + (t.Right?.Size ?? 0) + 1;
            t.Sum = N.Prod(N.Prod(N.Sum(t.Left), t.Value), N.Sum(t.Right));
            t.Height = 1 + Math.Max(t.Left?.Height ?? 0, t.Right?.Height ?? 0);
            return t;
        }

        [凾(256)]
        static void IBbstNodeOp<T, Nd, N>.SetValue(ref Nd t, int k, T x)
        {
            N.Propagate(ref t);
            var lc = t.Left?.Size ?? 0;
            if (k <= lc)
            {
                if (k == lc)
                    t.Value = x;
                else
                    N.SetValue(ref t.Left, k, x);
            }
            else
                N.SetValue(ref t.Right, k - lc - 1, x);
            t = N.Update(t);
        }

        [凾(256)]
        static T IBbstNodeOp<T, Nd, N>.GetValue(ref Nd t, int k)
        {
            N.Propagate(ref t);
            var lc = t.Left?.Size ?? 0;
            if (k <= lc)
            {
                if (k == lc)
                    return t.Value;
                return N.GetValue(ref t.Left, k);
            }
            else
                return N.GetValue(ref t.Right, k - lc - 1);
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
                stack = new Stack<Nd>(root.Height * 2 + 2);
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
                    N.Propagate(ref node);
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node);
                    node = next;
                }
            }

            public T Current => current.Value;

            [凾(256)]
            public bool MoveNext()
            {
                if (!stack.TryPop(out current))
                {
                    current = null;
                    return false;
                }
                var node = reverse ? current.Left : current.Right;
                while (node != null)
                {
                    N.Propagate(ref node);
                    var next = reverse ? node.Right : node.Left;
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

    /// <summary>
    /// AVL木のノード
    /// </summary>
    public abstract class AvlTreeNodeBase<Nd, T> : IBbstNode
        where Nd : AvlTreeNodeBase<Nd, T>
    {
        protected AvlTreeNodeBase(T v)
        {
            Size = 1;
            Height = 1;
            Value = Sum = v;
        }

        [凾(256)]
        internal int HeightDiff() => (Left?.Height ?? 0) - (Right?.Height ?? 0);
        public int Size { get; set; }
        public int Height;
        public T Value;
        public T Sum;
        public Nd Left, Right;

        public static IEnumerator<T> GetEnumerator<N>(ref Nd t) where N : IBbstNodeOp<T, Nd, N>
            => N.GetEnumerator(ref t);

        [SourceExpander.NotEmbeddingSource]
        public override string ToString()
        {
            return $"Value = {Value} Sum={Sum} Size = {Size} Height = {Height}";
        }

        [Conditional("DEBUG")]
        public void Validate()
        {
            if (Height < 0)
                throw new InvalidProgramException("Height は非負");

            if (Math.Abs(HeightDiff()) > 1)
                throw new InvalidProgramException("左右のレベル差は1以下");

            Left?.Validate();
            Right?.Validate();
        }
    }
}