using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

#pragma warning disable IDE0251

namespace Kzrnm.Competitive.Internal
{
    public interface IRedBlackTreeNode<T, Nd> : IBbstNode<T, Nd> where Nd : class, IRedBlackTreeNode<T, Nd>
    {
        static abstract Nd Create(Nd left, Nd right);
    }

    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp
    /// <summary>
    /// 赤黒木のノード
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
    public class RedBlackTreeNodeBase<TSelf, T>
        where TSelf : RedBlackTreeNodeBase<TSelf, T>, IRedBlackTreeNode<T, TSelf>
    {
        public interface D
        {
            int Level { get; }
        }
        public class Internal : D
        {
            public TSelf left, right;
            public int Level { get; set; }
        }
        public class Leaf : D
        {
            public T Value { get; internal set; }
            public int Level => 0;
        }
        protected bool IsBlack;
        protected int UpperLevel => Data.Level + (IsBlack ? 1 : 0);
        public int Size { get; protected set; }
        public T Sum { get; protected set; }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public D Data;

        [SourceExpander.NotEmbeddingSource]
        string DebuggerDisplay()
        {
            var black = IsBlack ? "B" : "";
            if (Data is Leaf lf)
                return $"Value = {lf.Value}, Size = {Size}, Level = {Data.Level}{black}";
            else
                return $"Sum = {Sum}, Size = {Size}, Level = {Data.Level}{black}";
        }

        [Conditional("DEBUG")]
        public void Validate()
        {
            if (Data is Leaf)
            {
                if (!IsBlack)
                    throw new InvalidProgramException("葉は Black であるべき");
                if (Size != 1)
                    throw new InvalidProgramException("葉のサイズは 1 であるべき");
                return;
            }
            var e = (Internal)Data;
            if (e.left is null)
                throw new InvalidProgramException("左には要素があるはず");
            if (e.right is null)
                throw new InvalidProgramException("右には要素があるはず");
            if (e.Level <= 0)
                throw new InvalidProgramException("Level は正の数");

            if (!IsBlack)
            {
                if (!e.left.IsBlack)
                    throw new InvalidProgramException("赤の親は黒");
                if (!e.right.IsBlack)
                    throw new InvalidProgramException("赤の親は黒");
            }
            if (e.Level != e.left.UpperLevel)
                throw new InvalidProgramException("左とレベルが不整合");
            if (e.Level != e.right.UpperLevel)
                throw new InvalidProgramException("右とレベルが不整合");

            e.left.Validate();
            e.right.Validate();
        }

        public static (TSelf, TSelf) Split(TSelf t, int k)
        {
            if (t == null) return (null, null);
            TSelf.Propagate(ref t);
            if (k == 0) return (null, t);
            if (k >= t.Size) return (t, null);
            Debug.Assert(t.Data is Internal);
            var tt = Unsafe.As<Internal>(t.Data);
            var lc = tt.left.Size;
            if (k < lc)
            {
                var (p1, p2) = TSelf.Split(tt.left, k);
                return (p1, TSelf.Merge(p2, tt.right));
            }
            else if (k > lc)
            {
                var (p1, p2) = TSelf.Split(tt.right, k - lc);
                return (TSelf.Merge(tt.left, p1), p2);
            }
            return (tt.left, tt.right);
        }
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
            var ld = l.Data.Level.CompareTo(r.Data.Level);
            if (ld == 0)
            {
                if (l.IsBlack != r.IsBlack)
                {
                    if (l.IsBlack)
                    {
                        r = TSelf.Copy(r);
                        r.IsBlack = true;
                    }
                    else
                    {
                        l = TSelf.Copy(l);
                        l.IsBlack = true;
                    }
                }
                return TSelf.Create(l, r);
            }
            else if (ld < 0)
            {
                Debug.Assert(r.Data is Internal);
                TSelf.Propagate(ref r);
                var ri = Unsafe.As<Internal>(r.Data);
                ref var c = ref ri.left;
                c = SubMerge(l, c);
                var ci = Unsafe.As<Internal>(c.Data);

                if (r.IsBlack && !c.IsBlack && !ci.left.IsBlack)
                {
                    r.IsBlack = false;
                    c.IsBlack = true;
                    if (ri.right.IsBlack)
                        return RotateRight(r, c);
                    ri.right.IsBlack = true;
                }
                return TSelf.Update(r);
            }
            else
            {
                Debug.Assert(l.Data is Internal);
                TSelf.Propagate(ref l);
                var li = Unsafe.As<Internal>(l.Data);
                ref var c = ref li.right;
                c = SubMerge(c, r);
                var ci = Unsafe.As<Internal>(c.Data);

                if (l.IsBlack && !c.IsBlack && !ci.right.IsBlack)
                {
                    l.IsBlack = false;
                    c.IsBlack = true;
                    if (li.left.IsBlack)
                        return RotateLeft(l, c);
                    li.left.IsBlack = true;
                }
                return TSelf.Update(l);
            }
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