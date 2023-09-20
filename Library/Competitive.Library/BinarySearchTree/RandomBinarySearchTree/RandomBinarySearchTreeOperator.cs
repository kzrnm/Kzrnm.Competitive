using AtCoder;
using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 乱択平衡二分探索木(Operator)
    // https://ei1333.github.io/library/structure/bbst/lazy-reversible-splay-tree.hpp
    public class RandomBinarySearchTreeNode<T> : IBbstNode<T, RandomBinarySearchTreeNode<T>>
    {
        internal RandomBinarySearchTreeNode<T> left;
        internal RandomBinarySearchTreeNode<T> right;
        public RandomBinarySearchTreeNode<T> Left { get => left; set => left = value; }
        public RandomBinarySearchTreeNode<T> Right { get => right; set => right = value; }
        public T Key { get; set; }
        public T Sum { get; set; }
        public int Size { get; set; }
        public RandomBinarySearchTreeNode(T v)
        {
            Size = 1;
            Key = Sum = v;
        }
        public override string ToString()
        {
            return $"Size = {Size}, Key = {Key}, Sum = {Sum}";
        }
    }
    namespace Internal.Bbst
    {
        /// <summary>
        /// 乱択平衡二分探索木のオペレータ
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="TOp">モノイドの操作</typeparam>
        public readonly struct RandomBinarySearchTreeNodeOperator<T, TOp> : IBbstImplOperator<T, RandomBinarySearchTreeNode<T>>
            where TOp : struct, ISegtreeOperator<T>
        {
            public static TOp op => default;
            public static BinarySearchTreeNodeOperator<T, TOp, RandomBinarySearchTreeNode<T>, RandomBinarySearchTreeNodeOperator<T, TOp>> np => default;

            [凾(256)]
            public RandomBinarySearchTreeNode<T> Copy(RandomBinarySearchTreeNode<T> t) => t;

            [凾(256)]
            public RandomBinarySearchTreeNode<T> Create(T v)
                => new RandomBinarySearchTreeNode<T>(v);

            internal static readonly Xoshiro256 rnd = new Xoshiro256();
            [凾(256)]
            public RandomBinarySearchTreeNode<T> Merge(RandomBinarySearchTreeNode<T> l, RandomBinarySearchTreeNode<T> r)
            {
                if (l == null || r == null)
                    return l ?? r;
                if ((int)((rnd.NextUInt64() * (ulong)(l.Size + r.Size)) >> 32) < l.Size)
                {
                    l.right = Merge(l.right, r);
                    return Update(l);
                }
                else
                {
                    r.left = Merge(l, r.left);
                    return Update(r);
                }
            }

            [凾(256)]
            public (RandomBinarySearchTreeNode<T>, RandomBinarySearchTreeNode<T>) Split(RandomBinarySearchTreeNode<T> t, int p)
            {
                if (t == null) return (null, null);

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
            static RandomBinarySearchTreeNode<T> Update(RandomBinarySearchTreeNode<T> t)
            {
                t.Size = np.Size(t.Left) + np.Size(t.Right) + 1;
                t.Sum = op.Operate(op.Operate(np.Sum(t.Left), t.Key), np.Sum(t.Right));
                return t;
            }

            [凾(256)]
            public RandomBinarySearchTreeNode<T> Build(ReadOnlySpan<T> vs)
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
            public void SetValue(ref RandomBinarySearchTreeNode<T> t, int k, T x)
            {
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
            public T GetValue(ref RandomBinarySearchTreeNode<T> t, int k)
            {
                var lc = np.Size(t.Left);
                if (k < lc)
                    return GetValue(ref t.left, k);
                else if (k == lc)
                    return t.Key;
                else
                    return GetValue(ref t.right, k - lc - 1);
            }

            [凾(256)]
            public RandomBinarySearchTreeEnumerator<T, TOp> GetEnumerator(RandomBinarySearchTreeNode<T> t)
                => new RandomBinarySearchTreeEnumerator<T, TOp>(t);
        }
        public struct RandomBinarySearchTreeEnumerator<T, TOp> : IEnumerator<T>
            where TOp : struct, ISegtreeOperator<T>
        {
            T cur;
            Stack<RandomBinarySearchTreeNode<T>> stack;
            public RandomBinarySearchTreeEnumerator(RandomBinarySearchTreeNode<T> t)
            {
                cur = default;
                stack = new Stack<RandomBinarySearchTreeNode<T>>();
                IntializeAll(t);
            }
            [凾(256)]
            void IntializeAll(RandomBinarySearchTreeNode<T> t)
            {
                while (t != null)
                {
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
}