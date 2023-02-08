using AtCoder;
using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using static Internal.NodeColor;
    // competitive-verifier: TITLE 赤黒木(Operator)
    public class RedBlackTreeNode<T> : IBbstNode<T, RedBlackTreeNode<T>>
    {
        internal RedBlackTreeNode<T> left;
        internal RedBlackTreeNode<T> right;
        public RedBlackTreeNode<T> Left { get => left; set => left = value; }
        public RedBlackTreeNode<T> Right { get => right; set => right = value; }
        public T Key { get; set; }
        public T Sum { get; set; }
        public int Size { get; set; }
        public bool IsLeaf => Left == null;

        public int Level;
        public Internal.NodeColor Color;

        public RedBlackTreeNode(T v)
        {
            Color = Black;
            Size = 1;
            Key = Sum = v;
        }
        public RedBlackTreeNode(RedBlackTreeNode<T> l, RedBlackTreeNode<T> r, T v)
        {
            Left = l;
            Right = r;
            Color = Red;
            Size = 1;
            Key = Sum = v;
        }
        public override string ToString()
        {
            if (IsLeaf)
                return $"Key = {Key}, Sum = {Sum}";
            return $"Size = {Size}";
        }
    }

    namespace Internal.Bbst
    {
        /// <summary>
        /// 赤黒木のオペレータ
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="TOp">モノイドの操作</typeparam>
        /// <typeparam name="TCp">コピー操作の実装</typeparam>
        public struct RedBlackTreeNodeOperator<T, TOp, TCp> : IBbstImplOperator<T, RedBlackTreeNode<T>>
            where TOp : struct, ISegtreeOperator<T>
            where TCp : struct, ICopyOperator<RedBlackTreeNode<T>>
        {
            public static TOp op => default;
            public static BinarySearchTreeNodeOperator<T, TOp, RedBlackTreeNode<T>, RedBlackTreeNodeOperator<T, TOp, TCp>> np => default;

            [凾(256)]
            public RedBlackTreeNode<T> Copy(RedBlackTreeNode<T> t)
                => new TCp().Copy(t);

            [凾(256)]
            public RedBlackTreeNode<T> Create(T v)
                => new RedBlackTreeNode<T>(v);
            [凾(256)]
            public RedBlackTreeNode<T> Create(RedBlackTreeNode<T> l, RedBlackTreeNode<T> r)
            {
                var t = new RedBlackTreeNode<T>(l, r, op.Identity);
                return Update(t);
            }

            [凾(256)]
            public RedBlackTreeNode<T> Merge(RedBlackTreeNode<T> l, RedBlackTreeNode<T> r)
            {
                if (l == null || r == null) return l ?? r;
                var c = SubMerge(l, r);
                c.Color = Black;
                return c;
            }

            [凾(256)]
            RedBlackTreeNode<T> SubMerge(RedBlackTreeNode<T> l, RedBlackTreeNode<T> r)
            {
                if (l.Level < r.Level)
                {
                    r = np.im.Copy(r);
                    var c = (r.Left = SubMerge(l, r.Left));
                    if (r.Color == Black && c.Color == Red && c.Left?.Color == Red)
                    {
                        r.Color = Red;
                        c.Color = Black;
                        if (r.Right.Color == Black)
                            return RotateRight(r);
                        r.Right.Color = Black;
                    }
                    return Update(r);
                }
                if (l.Level > r.Level)
                {
                    l = np.im.Copy(l);
                    var c = (l.Right = SubMerge(l.Right, r));
                    if (l.Color == Black && c.Color == Red && c.Right?.Color == Red)
                    {
                        l.Color = Red;
                        c.Color = Black;
                        if (l.Left.Color == Black)
                            return RotateLeft(l);
                        l.Left.Color = Black;
                    }
                    return Update(l);
                }
                return Create(l, r);
            }
            [凾(256)]
            static RedBlackTreeNode<T> RotateRight(RedBlackTreeNode<T> t)
            {
                t = np.im.Copy(t);
                var s = np.im.Copy(t.left);
                t.Left = s.Right;
                s.Right = t;
                Update(t);
                return Update(s);
            }
            [凾(256)]
            static RedBlackTreeNode<T> RotateLeft(RedBlackTreeNode<T> t)
            {
                t = np.im.Copy(t);
                var s = np.im.Copy(t.right);
                t.Right = s.Left;
                s.Left = t;
                Update(t);
                return Update(s);
            }


            [凾(256)]
            public (RedBlackTreeNode<T>, RedBlackTreeNode<T>) Split(RedBlackTreeNode<T> t, int p)
            {
                if (t == null) return (null, null);
                t = np.im.Copy(t);
                if (p == 0) return (null, t);
                if (p >= t.Size) return (t, null);
                var l = t.Left;
                var r = t.Right;
                if (p < np.Size(l))
                {
                    var (p1, p2) = Split(l, p);
                    return (p1, Merge(p2, r));
                }
                if (p > np.Size(l))
                {
                    var (p1, p2) = Split(r, p - np.Size(l));
                    return (Merge(l, p1), p2);
                }
                return (l, r);
            }

            [凾(256)]
            static RedBlackTreeNode<T> Update(RedBlackTreeNode<T> t)
            {
                t.Size = np.Size(t.Left) + np.Size(t.Right) + (t.IsLeaf ? 1 : 0);
                t.Level = t.IsLeaf ? 0 : (t.Left.Level + (t.Left.Color == Black ? 1 : 0));
                t.Sum = op.Operate(op.Operate(np.Sum(t.Left), t.Key), np.Sum(t.Right));
                return t;
            }

            [凾(256)]
            public RedBlackTreeNode<T> Build(ReadOnlySpan<T> vs)
            {
                switch (vs.Length)
                {
                    case 0: return null;
                    case 1: return Create(vs[0]);
                }

                var half = vs.Length >> 1;
                return Merge(
                    Build(vs[..half]),
                    Build(vs[half..])
                );
            }

            [凾(256)]
            public void SetValue(ref RedBlackTreeNode<T> t, int k, T x)
            {
                t = np.im.Copy(t);
                if (t.IsLeaf)
                {
                    t.Key = t.Sum = x;
                    return;
                }
                if (k < np.Size(t.Left))
                    SetValue(ref t.left, k, x);
                else
                    SetValue(ref t.right, k - np.Size(t.left), x);
                t = Update(t);
            }

            [凾(256)]
            public T GetValue(ref RedBlackTreeNode<T> t, int k)
            {
                if (t.IsLeaf)
                    return t.Key;
                if (k < np.Size(t.Left))
                    return GetValue(ref t.left, k);
                else
                    return GetValue(ref t.right, k - np.Size(t.left));
            }

            [凾(256)]
            public RedBlackTreeEnumerator<T, TOp> GetEnumerator(RedBlackTreeNode<T> t)
                => new RedBlackTreeEnumerator<T, TOp>(t);
        }
        public struct RedBlackTreeEnumerator<T, TOp> : IEnumerator<T>
            where TOp : struct, ISegtreeOperator<T>
        {
            T cur;
            Stack<RedBlackTreeNode<T>> stack;
            public RedBlackTreeEnumerator(RedBlackTreeNode<T> t)
            {
                cur = default;
                stack = new Stack<RedBlackTreeNode<T>>();
                if (t != null)
                    stack.Push(t);
            }

            public T Current => cur;
            object IEnumerator.Current => cur;

            public bool MoveNext()
            {
                while (stack.TryPop(out var t))
                {
                    if (t.IsLeaf)
                    {
                        cur = t.Key;
                        return true;
                    }
                    if (t.right != null) stack.Push(t.right);
                    if (t.left != null) stack.Push(t.left);
                }
                return false;
            }

            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();
        }
    }
}