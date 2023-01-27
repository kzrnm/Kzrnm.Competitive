using System;
using System.Collections;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    using static NodeColor;
    public class LazyRedBlackTreeNode<T, F> : IBbstNode<T, F, LazyRedBlackTreeNode<T, F>>
    {
        internal LazyRedBlackTreeNode<T, F> left;
        internal LazyRedBlackTreeNode<T, F> right;
        public LazyRedBlackTreeNode<T, F> Left { get => left; set => left = value; }
        public LazyRedBlackTreeNode<T, F> Right { get => right; set => right = value; }
        public T Key { get; set; }
        public T Sum { get; set; }
        public F Lazy { get; set; }
        public int Size { get; set; }
        public bool IsReverse { get; set; }
        public bool IsLeaf => Left == null;

        public int Level;
        public NodeColor Color;

        public LazyRedBlackTreeNode(T v, F f)
        {
            Color = Black;
            Size = 1;
            Key = Sum = v;
            Lazy = f;
        }
        public LazyRedBlackTreeNode(LazyRedBlackTreeNode<T, F> l, LazyRedBlackTreeNode<T, F> r, T v, F f)
        {
            Left = l;
            Right = r;
            Color = Red;
            Size = 1;
            Key = Sum = v;
            Lazy = f;
        }
        public override string ToString()
        {
            if (IsLeaf)
                return $"Key = {Key}, Sum = {Sum}, Lazy = {Lazy}";
            return $"Size = {Size}";
        }
    }
    /// <summary>
    /// 反転可能遅延伝搬赤黒木のオペレータ
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">作用素</typeparam>
    /// <typeparam name="TOp">モノイドの操作</typeparam>
    /// <typeparam name="TCp">コピー操作の実装</typeparam>
    public struct LazyRedBlackTreeNodeOperator<T, F, TOp, TCp> : IBbstImplOperator<T, F, LazyRedBlackTreeNode<T, F>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where TCp : struct, ICopyOperator<LazyRedBlackTreeNode<T, F>>
    {
        public static TOp op => default;
        public static LazyReversibleBinarySearchTreeNodeOperator<T, F, TOp, LazyRedBlackTreeNode<T, F>, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> np => default;

        [凾(256)]
        public LazyRedBlackTreeNode<T, F> Copy(LazyRedBlackTreeNode<T, F> t)
            => new TCp().Copy(t);

        [凾(256)]
        public LazyRedBlackTreeNode<T, F> Create(T v)
            => new LazyRedBlackTreeNode<T, F>(v, op.FIdentity);
        [凾(256)]
        public LazyRedBlackTreeNode<T, F> Create(LazyRedBlackTreeNode<T, F> l, LazyRedBlackTreeNode<T, F> r)
        {
            var t = new LazyRedBlackTreeNode<T, F>(l, r, op.Identity, op.FIdentity);
            return Update(t);
        }

        [凾(256)]
        public LazyRedBlackTreeNode<T, F> Merge(LazyRedBlackTreeNode<T, F> l, LazyRedBlackTreeNode<T, F> r)
        {
            if (l == null || r == null) return l ?? r;
            var c = SubMerge(l, r);
            c.Color = Black;
            return c;
        }

        [凾(256)]
        LazyRedBlackTreeNode<T, F> SubMerge(LazyRedBlackTreeNode<T, F> l, LazyRedBlackTreeNode<T, F> r)
        {
            if (l.Level < r.Level)
            {
                r = Propagate(ref r);
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
                l = Propagate(ref l);
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
        LazyRedBlackTreeNode<T, F> RotateRight(LazyRedBlackTreeNode<T, F> t)
        {
            t = Propagate(ref t);
            var s = Propagate(ref t.left);
            t.Left = s.Right;
            s.Right = t;
            Update(t);
            return Update(s);
        }
        [凾(256)]
        LazyRedBlackTreeNode<T, F> RotateLeft(LazyRedBlackTreeNode<T, F> t)
        {
            t = Propagate(ref t);
            var s = Propagate(ref t.right);
            t.Right = s.Left;
            s.Left = t;
            Update(t);
            return Update(s);
        }


        [凾(256)]
        public (LazyRedBlackTreeNode<T, F>, LazyRedBlackTreeNode<T, F>) Split(LazyRedBlackTreeNode<T, F> t, int p)
        {
            if (t == null) return (null, null);
            t = Propagate(ref t);
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
        public LazyRedBlackTreeNode<T, F> Propagate(ref LazyRedBlackTreeNode<T, F> t)
        {
            t = Copy(t);
            var lazy = !EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity);
            var rev = t.IsReverse;

            if (lazy || rev)
            {
                if (t.Left != null)
                    t.Left = Copy(t.Left);
                if (t.Right != null)
                    t.Right = Copy(t.Right);
            }
            if (lazy)
            {
                if (t.IsLeaf)
                {
                    t.Key = op.Mapping(t.Lazy, t.Key, t.Size);
                }
                else
                {
                    if (t.Left != null)
                    {
                        t.Left.Lazy = op.Composition(t.Lazy, t.Left.Lazy);
                        t.Left.Sum = op.Mapping(t.Lazy, t.Left.Sum, t.Left.Size);
                    }
                    if (t.Right != null)
                    {
                        t.Right.Lazy = op.Composition(t.Lazy, t.Right.Lazy);
                        t.Right.Sum = op.Mapping(t.Lazy, t.Right.Sum, t.Right.Size);
                    }
                }
                t.Lazy = op.FIdentity;
            }
            if (rev)
            {
                if (t.Left != null)
                    np.Toggle(t.Left);
                if (t.Right != null)
                    np.Toggle(t.Right);
                t.IsReverse = false;
            }

            return Update(t);
        }

        [凾(256)]
        static LazyRedBlackTreeNode<T, F> Update(LazyRedBlackTreeNode<T, F> t)
        {
            t.Size = np.Size(t.Left) + np.Size(t.Right) + (t.IsLeaf ? 1 : 0);
            t.Level = t.IsLeaf ? 0 : (t.Left.Level + (t.Left.Color == Black ? 1 : 0));
            t.Sum = op.Operate(op.Operate(np.Sum(t.Left), t.Key), np.Sum(t.Right));
            return t;
        }

        [凾(256)]
        public LazyRedBlackTreeNode<T, F> Build(ReadOnlySpan<T> vs)
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
        public void SetValue(ref LazyRedBlackTreeNode<T, F> t, int k, T x)
        {
            t = Propagate(ref t);
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
        public T GetValue(ref LazyRedBlackTreeNode<T, F> t, int k)
        {
            t = Propagate(ref t);
            if (t.IsLeaf)
                return t.Key;
            if (k < np.Size(t.Left))
                return GetValue(ref t.left, k);
            else
                return GetValue(ref t.right, k - np.Size(t.left));
        }

        [凾(256)]
        public LazyRedBlackTreeEnumerator<T, F, TOp, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> GetEnumerator(LazyRedBlackTreeNode<T, F> t)
            => new LazyRedBlackTreeEnumerator<T, F, TOp, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>>(t);
    }
    public struct LazyRedBlackTreeEnumerator<T, F, TOp, TRb> : IEnumerator<T>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where TRb : struct, IBbstImplOperator<T, F, LazyRedBlackTreeNode<T, F>>
    {
        static TRb rb => default;
        T cur;
        Stack<LazyRedBlackTreeNode<T, F>> stack;
        public LazyRedBlackTreeEnumerator(LazyRedBlackTreeNode<T, F> t)
        {
            cur = default;
            stack = new Stack<LazyRedBlackTreeNode<T, F>>();
            if (t != null)
                stack.Push(rb.Propagate(ref t));
        }

        public T Current => cur;
        object IEnumerator.Current => cur;

        public bool MoveNext()
        {
            while (stack.TryPop(out var t))
            {
                rb.Propagate(ref t);
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