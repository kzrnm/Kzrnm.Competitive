using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    public interface IRbtNodeOp<T, Nd, N> : IBbstNodeOp<T, Nd, N>
        where Nd : RedBlackTreeNodeBase<Nd, T>
        where N : IRbtNodeOp<T, Nd, N>
    {
        static abstract Nd Create(Nd left, Nd right);

        [SourceExpander.NotEmbeddingSource]
        static void IBbstNodeOp<Nd, N>.Validate(Nd t) => t?.Validate();

        [凾(256)]
        static (Nd, Nd) IBbstNodeOp<Nd, N>.Split(Nd t, int k)
        {
            if (t == null) return (null, null);
            N.Propagate(ref t);
            if (k == 0) return (null, t);
            if (k >= t.Size) return (t, null);

            Debug.Assert(!t.IsLeaf);
            var lc = t.Left.Size;
            if (k < lc)
            {
                var (p1, p2) = N.Split(t.Left, k);
                return (p1, N.Merge(p2, t.Right));
            }
            else if (k > lc)
            {
                var (p1, p2) = N.Split(t.Right, k - lc);
                return (N.Merge(t.Left, p1), p2);
            }
            return (t.Left, t.Right);
        }

        [凾(256)]
        static Nd IBbstNodeOp<Nd, N>.Merge(Nd l, Nd r)
        {
            if (l == null || r == null) return l ?? r;
            var c = SubMerge(l, r);
            c.IsBlack = true;
            return c;
        }

        [凾(256)]
        static Nd SubMerge(Nd l, Nd r)
        {
            var ld = l.Level.CompareTo(r.Level);
            if (ld == 0)
            {
                if (l.IsBlack != r.IsBlack)
                {
                    if (l.IsBlack)
                    {
                        r = N.Copy(r);
                        r.IsBlack = true;
                    }
                    else
                    {
                        l = N.Copy(l);
                        l.IsBlack = true;
                    }
                }
                return N.Create(l, r);
            }
            else if (ld < 0)
            {
                Debug.Assert(!r.IsLeaf);
                N.Propagate(ref r);
                var c = r.Left = SubMerge(l, r.Left);

                if (r.IsBlack && !c.IsBlack && !c.Left.IsBlack)
                {
                    r.IsBlack = false;
                    c.IsBlack = true;
                    if (r.Right.IsBlack)
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

                        N.Propagate(ref r);
                        N.Propagate(ref c);
                        r.Left = c.Right;
                        c.Right = r;
                        N.Update(r);
                        return N.Update(c);
                    }
                    r.Right.IsBlack = true;
                }
                return N.Update(r);
            }
            else
            {
                Debug.Assert(!l.IsLeaf);
                N.Propagate(ref l);
                var c = l.Right = SubMerge(l.Right, r);

                if (l.IsBlack && !c.IsBlack && !c.Right.IsBlack)
                {
                    l.IsBlack = false;
                    c.IsBlack = true;
                    if (l.Left.IsBlack)
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

                        N.Propagate(ref l);
                        N.Propagate(ref c);
                        l.Right = c.Left;
                        c.Left = l;
                        N.Update(l);
                        return N.Update(c);
                    }
                    l.Left.IsBlack = true;
                }
                return N.Update(l);
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
            if (t.Left.Size <= l)
                return N.Prod(ref t.Right, l - t.Left.Size, r - t.Left.Size);
            if (r <= t.Left.Size)
                return N.Prod(ref t.Left, l, r);

            var lt = N.Prod(ref t.Left, l, t.Left.Size);
            var rt = N.Prod(ref t.Right, 0, r - t.Left.Size);

            return N.Prod(lt, rt);
        }

        /// <summary>
        /// モノイド<paramref name="l"/>, <paramref name="r"/>] の総積を返します。
        /// </summary>
        [凾(256)]
        static abstract T Prod(T l, T r);

        [凾(256)]
        static void IBbstNodeOp<T, Nd, N>.SetValue(ref Nd t, int k, T x)
        {
            N.Propagate(ref t);
            if (t.IsLeaf)
            {
                t.Sum = x;
            }
            else
            {
                var lc = t.Left.Size;
                if (k < lc)
                    N.SetValue(ref t.Left, k, x);
                else
                    N.SetValue(ref t.Right, k - lc, x);
            }
            t = N.Update(t);
        }

        [凾(256)]
        static T IBbstNodeOp<T, Nd, N>.GetValue(ref Nd t, int k)
        {
            N.Propagate(ref t);
            if (t.IsLeaf)
                return t.Sum;
            var lc = t.Left.Size;
            if (k < lc)
                return N.GetValue(ref t.Left, k);
            else
                return N.GetValue(ref t.Right, k - lc);
        }

        static IEnumerator<T> IBbstNodeOp<T, Nd, N>.GetEnumerator(ref Nd t)
        {
            N.Propagate(ref t);
            return new Enumerator(t);
        }
        public sealed class Enumerator : IEnumerator<T>
        {
            readonly Nd start;
            T cur;
            Stack<Nd> stack;
            public Enumerator(Nd t)
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
                    N.Propagate(ref t);

                    if (t.IsLeaf)
                    {
                        cur = t.Sum;
                        return true;
                    }

                    if (t.Right != null) stack.Push(t.Right);
                    if (t.Left != null) stack.Push(t.Left);
                }
                return false;
            }
            public void Dispose() { }
            public void Reset()
            {
                cur = default;
                stack = new Stack<Nd>();
                if (start != null)
                    stack.Push(start);
            }
        }
    }

    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 赤黒木のノード
    /// </summary>
    public abstract class RedBlackTreeNodeBase<Nd, T> : IBbstNode
        where Nd : RedBlackTreeNodeBase<Nd, T>
    {
        public RedBlackTreeNodeBase(Nd other)
        {
            IsBlack = other.IsBlack;
            Left = other.Left;
            Right = other.Right;
            Level = other.Level;
            Size = other.Size;
            Sum = other.Sum;
        }
        public RedBlackTreeNodeBase(T v)
        {
            IsBlack = true;
            Size = 1;
            Sum = v;
        }
        public RedBlackTreeNodeBase(Nd left, Nd right, T sum)
        {
            Debug.Assert(left is not null);
            Debug.Assert(right is not null);
            Debug.Assert(left.UpperLevel() == right.UpperLevel());

            IsBlack = false;
            Left = left;
            Right = right;
            Level = left.UpperLevel();
            Size = left.Size + right.Size;
            Sum = sum;
        }

        [凾(256)]
        internal int UpperLevel() => Level + (IsBlack ? 1 : 0);
        public int Size { get; set; }
        public bool IsBlack;
        public int Level;
        public T Sum;
        public Nd Left, Right;

        internal bool IsLeaf
#if !SOURCE_EMBEDDING
        {
            get
            {
                Debug.Assert(IsLeafImpl() == Right is null);
                Debug.Assert(!IsLeafImpl() || IsBlack);
                Debug.Assert(!IsLeafImpl() || Level == 0);
                Debug.Assert(!IsLeafImpl() || Size == 1);
                return IsLeafImpl();
            }
        }
        [凾(256)]
        bool IsLeafImpl()
#endif
            => Left == null;

        public static IEnumerator<T> GetEnumerator<N>(ref Nd t) where N : IBbstNodeOp<T, Nd, N>
            => N.GetEnumerator(ref t);

        [SourceExpander.NotEmbeddingSource]
        public override string ToString()
        {
            var black = IsBlack ? "B" : "";
            if (IsLeaf)
                return $"Value = {Sum} Size = {Size} Level = {Level}{black}";
            else
                return $"Sum = {Sum} Size = {Size} Level = {Level}{black}";
        }

        [Conditional("DEBUG")]
        public void Validate()
        {
            if (IsLeaf)
            {
                if (!IsBlack)
                    throw new InvalidProgramException("葉は Black であるべき");
                if (Size != 1)
                    throw new InvalidProgramException("葉のサイズは 1 であるべき");
                return;
            }
            if (Left is null)
                throw new InvalidProgramException("左には要素があるはず");
            if (Right is null)
                throw new InvalidProgramException("右には要素があるはず");
            if (Level <= 0)
                throw new InvalidProgramException("Level は正の数");

            if (!IsBlack)
            {
                if (!Left.IsBlack)
                    throw new InvalidProgramException("赤の親は黒");
                if (!Right.IsBlack)
                    throw new InvalidProgramException("赤の親は黒");
            }
            if (Level != Left.UpperLevel())
                throw new InvalidProgramException("左とレベルが不整合");
            if (Level != Right.UpperLevel())
                throw new InvalidProgramException("右とレベルが不整合");

            Left.Validate();
            Right.Validate();
        }
    }
}