using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    // competitive-verifier: TITLE 遅延伝搬乱択平衡二分探索木(Operator)
    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    public class LazyRandomBinarySearchTreeNode<T, F> : ILazyBbstNode<T, F, LazyRandomBinarySearchTreeNode<T, F>>
    {
        internal LazyRandomBinarySearchTreeNode<T, F> left;
        internal LazyRandomBinarySearchTreeNode<T, F> right;
        public LazyRandomBinarySearchTreeNode<T, F> Left { get => left; set => left = value; }
        public LazyRandomBinarySearchTreeNode<T, F> Right { get => right; set => right = value; }
        public T Key { get; set; }
        public T Sum { get; set; }
        public F Lazy { get; set; }
        public int Size { get; set; }
        public bool IsReverse { get; set; }
        public LazyRandomBinarySearchTreeNode(T v, F f)
        {
            Size = 1;
            Key = Sum = v;
            Lazy = f;
        }
        public override string ToString()
        {
            return $"Size = {Size}, Key = {Key}, Sum = {Sum}, Lazy = {Lazy}";
        }
    }
    /// <summary>
    /// 反転可能遅延伝搬乱択平衡二分探索木のオペレータ
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">作用素</typeparam>
    /// <typeparam name="TOp">モノイドの操作</typeparam>
#pragma warning disable IDE0250
    public struct LazyRandomBinarySearchTreeNodeOperator<T, F, TOp> : ILazyBbstImplOperator<T, LazyRandomBinarySearchTreeNode<T, F>>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
#pragma warning restore IDE0250
        public static TOp op => default;
        public static LazyBinarySearchTreeNodeOperator<T, F, TOp, LazyRandomBinarySearchTreeNode<T, F>, LazyRandomBinarySearchTreeNodeOperator<T, F, TOp>> np => default;

        [凾(256)]
        public LazyRandomBinarySearchTreeNode<T, F> Copy(LazyRandomBinarySearchTreeNode<T, F> t) => t;

        [凾(256)]
        public LazyRandomBinarySearchTreeNode<T, F> Create(T v)
            => new LazyRandomBinarySearchTreeNode<T, F>(v, op.FIdentity);

        internal static readonly Xoshiro256 rnd = new Xoshiro256();
        [凾(256)]
        public LazyRandomBinarySearchTreeNode<T, F> Merge(LazyRandomBinarySearchTreeNode<T, F> l, LazyRandomBinarySearchTreeNode<T, F> r)
        {
            if (l == null || r == null)
                return l ?? r;
            if ((int)((rnd.NextUInt64() * (ulong)(l.Size + r.Size)) >> 32) < l.Size)
            {
                Propagate(ref l);
                l.right = Merge(l.right, r);
                return Update(l);
            }
            else
            {
                Propagate(ref r);
                r.left = Merge(l, r.left);
                return Update(r);
            }
        }

        [凾(256)]
        public (LazyRandomBinarySearchTreeNode<T, F>, LazyRandomBinarySearchTreeNode<T, F>) Split(LazyRandomBinarySearchTreeNode<T, F> t, int p)
        {
            if (t == null) return (null, null);
            t = Propagate(ref t);

            var l = t.Left;
            var r = t.Right;
            if (p <= np.Size(l))
            {
                var (p1, p2) = Split(l, p);
                t.left = p2;
                return (p1, Update(t));
            }
            else
            {
                var (p1, p2) = Split(r, p - np.Size(l) - 1);
                t.right = p1;
                return (Update(t), p2);
            }
        }

        [凾(256)]
        public LazyRandomBinarySearchTreeNode<T, F> Propagate(ref LazyRandomBinarySearchTreeNode<T, F> t)
        {
            var lazy = !EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity);
            var rev = t.IsReverse;

            if (lazy)
            {
                t.Key = op.Mapping(t.Lazy, t.Key, 1);
                if (t.left != null)
                {
                    t.left.Lazy = op.Composition(t.left.Lazy, t.Lazy);
                    t.left.Sum = op.Mapping(t.Lazy, t.left.Sum, t.left.Size);
                }
                if (t.right != null)
                {
                    t.right.Lazy = op.Composition(t.right.Lazy, t.Lazy);
                    t.right.Sum = op.Mapping(t.Lazy, t.right.Sum, t.right.Size);
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
        static LazyRandomBinarySearchTreeNode<T, F> Update(LazyRandomBinarySearchTreeNode<T, F> t)
        {
            t.Size = np.Size(t.Left) + np.Size(t.Right) + 1;
            t.Sum = op.Operate(op.Operate(np.Sum(t.Left), t.Key), np.Sum(t.Right));
            return t;
        }

        [凾(256)]
        public LazyRandomBinarySearchTreeNode<T, F> Build(ReadOnlySpan<T> vs)
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
        public void SetValue(ref LazyRandomBinarySearchTreeNode<T, F> t, int k, T x)
        {
            t = Propagate(ref t);
            var lc = np.Size(t.Left);
            if (k < lc)
                SetValue(ref t.left, k, x);
            else if (k == lc)
                t.Key = t.Sum = x;
            else
                SetValue(ref t.right, k - lc - 1, x);
            t = Update(t);
        }

        [凾(256)]
        public T GetValue(ref LazyRandomBinarySearchTreeNode<T, F> t, int k)
        {
            t = Propagate(ref t);
            var lc = np.Size(t.Left);
            if (k < lc)
                return GetValue(ref t.left, k);
            else if (k == lc)
                return t.Key;
            else
                return GetValue(ref t.right, k - lc - 1);
        }

        [凾(256)]
        public LazyRandomBinarySearchTreeEnumerator<T, F, TOp, LazyRandomBinarySearchTreeNodeOperator<T, F, TOp>> GetEnumerator(LazyRandomBinarySearchTreeNode<T, F> t)
            => new LazyRandomBinarySearchTreeEnumerator<T, F, TOp, LazyRandomBinarySearchTreeNodeOperator<T, F, TOp>>(t);
    }
    public struct LazyRandomBinarySearchTreeEnumerator<T, F, TOp, TRb> : IEnumerator<T>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where TRb : struct, ILazyBbstImplOperator<T, LazyRandomBinarySearchTreeNode<T, F>>
    {
        static TRb rb => default;
        T cur;
        Stack<LazyRandomBinarySearchTreeNode<T, F>> stack;
        public LazyRandomBinarySearchTreeEnumerator(LazyRandomBinarySearchTreeNode<T, F> t)
        {
            cur = default;
            stack = new Stack<LazyRandomBinarySearchTreeNode<T, F>>();
            IntializeAll(t);
        }
        [凾(256)]
        void IntializeAll(LazyRandomBinarySearchTreeNode<T, F> t)
        {
            while (t != null)
            {
                rb.Propagate(ref t);
                //var next = reverse ? t.Right : t.Left;
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
                cur = t.Key;
                //t = reverse ? t.Left : t.Right;
                t = t.right;
                while (t != null)
                {
                    rb.Propagate(ref t);
                    //var next = reverse ? t.Right : t.Left;
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