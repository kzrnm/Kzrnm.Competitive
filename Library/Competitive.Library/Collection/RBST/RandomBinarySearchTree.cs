using AtCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [IsOperator]
    public interface IRandomBinarySearchTreeOperator<T, F> : ILazySegtreeOperator<T, F>
    {
        /// <summary>
        /// <paramref name="v"/> を左右反転します。
        /// </summary>
        T Inverse(T v);
    }

    // competitive-verifier: TITLE 乱択平衡二分探索木
    // https://nyaannyaan.github.io/library/rbst/rbst-base.hpp
    // https://nyaannyaan.github.io/library/rbst/lazy-reversible-rbst.hpp
    /// <summary>
    /// 乱択平衡二分探索木
    /// </summary>
    public class RandomBinarySearchTree<T> : RandomBinarySearchTree<T, T, RandomBinarySearchTree<T>.EmptyOp>
    {
        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public RandomBinarySearchTree(T[] v) : base(v.AsSpan()) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v) : base(v) { }
        public struct EmptyOp : IRandomBinarySearchTreeOperator<T, T>
        {
            public T Identity => default;
            public T FIdentity => default;
            [凾(256)] public T Composition(T nf, T cf) => nf;
            [凾(256)] public T Inverse(T v) => v;
            [凾(256)] public T Mapping(T f, T x) => x;
            [凾(256)] public T Operate(T x, T y) => x;
        }
    }

    // https://nyaannyaan.github.io/library/rbst/rbst-base.hpp
    // https://nyaannyaan.github.io/library/rbst/lazy-reversible-rbst.hpp
    /// <summary>
    /// 乱択平衡二分探索木
    /// </summary>
    public class RandomBinarySearchTree<T, F, TOp> : IEnumerable<T>
        where TOp : struct, IRandomBinarySearchTreeOperator<T, F>
    {
        private static TOp op => new TOp();
        public class Node
        {
            public Node Left, Right;
            public T Key, Sum;
            public F Lazy;
            public int cnt;
            public bool rev;

            public Node() : this(op.Identity, op.FIdentity) { }
            public Node(T v) : this(v, op.FIdentity) { }
            public Node(T v, F f)
            {
                Key = Sum = v;
                Lazy = f;
                cnt = 1;
            }
        }

        public Node root;
        public int Count => root?.cnt ?? 0;

        public T AllProd => Sum(root);

        public RandomBinarySearchTree() { }
        public RandomBinarySearchTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public RandomBinarySearchTree(T[] v) : this(v.AsSpan()) { }
        public RandomBinarySearchTree(ReadOnlySpan<T> v)
        {
            root = Build(v);
        }
        private static Node Build(ReadOnlySpan<T> v)
        {
            switch (v.Length)
            {
                case 0: return null;
                case 1: return new Node(v[0]);
            }
            int m = v.Length / 2;
            return Update(new Node(v[m])
            {
                Left = Build(v[..m]),
                Right = Build(v[(m + 1)..])
            });
        }

        /// <summary>
        /// <paramref name="l"/>, <paramref name="r"/> をマージして新たに出来た木を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public static Node Merge(Node l, Node r)
        {
            if (l == null) return r;
            if (r == null) return l;

            if ((int)((Random() * (ulong)(l.cnt + r.cnt)) >> 32) < l.cnt)
            {
                Push(l);
                l.Right = Merge(l.Right, r);
                return Update(l);
            }
            else
            {
                Push(r);
                r.Left = Merge(l, r.Left);
                return Update(r);
            }
        }

        /// <summary>
        /// <paramref name="t"/> を [0, <paramref name="k"/>) と [<paramref name="k"/>, |<paramref name="t"/>|) の二つの木に分割する。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public static (Node Left, Node Right) Split(Node t, int k)
        {
            if (t == null) return (null, null);
            Push(t);
            if (k <= Size(t?.Left))
            {
                var (l, r) = Split(t.Left, k);
                t.Left = r;
                return (l, Update(t));
            }
            else
            {
                var (l, r) = Split(t.Right, k - 1 - Size(t?.Left));
                t.Right = l;
                return (Update(t), r);
            }
        }

        /// <summary>
        /// <paramref name="index"/> 番目に <paramref name="item"/> を挿入します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public void Insert(int index, T item)
        {
            var (l, r) = Split(root, index);
            root = Merge(Merge(l, new Node(item)), r);
        }

        /// <summary>
        /// <paramref name="index"/> 番目の要素を削除します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public void RemoveAt(int index)
        {
            var (xl, xr) = Split(root, index);
            var (_, yr) = Split(xr, 1);
            root = Merge(xl, yr);
        }

        /// <summary>
        /// 部分木を反転します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        private static void Reverse(Node t)
        {
            (t.Left, t.Right) = (t.Right, t.Left);
            t.Sum = op.Inverse(t.Sum);
            t.rev = !t.rev;
        }

        /// <summary>
        /// 部分木を反転します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Reverse() => Reverse(root);

        /// <summary>
        /// [<paramref name="l"/>, <paramref name="r"/>) を反転します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public void Reverse(int l, int r)
        {
            var (xl, xr) = Split(root, l);
            var (yl, yr) = Split(xr, r - l);
            Reverse(yl);
            root = Merge(xl, Merge(yl, yr));
        }

        /// <summary>
        /// [<paramref name="l"/>, <paramref name="r"/>) の総積を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public T Prod(int l, int r)
            => Slice(l, r - l);

        [凾(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public T Slice(int p, int length)
        {
            var (xl, xr) = Split(root, p);
            var (yl, yr) = Split(xr, length);
            var ret = Sum(yl);
            root = Merge(xl, Merge(yl, yr));
            return ret;
        }

        [凾(256)]
        private static void Propagate(Node t, F f)
        {
            t.Lazy = op.Composition(f, t.Lazy);
            t.Key = op.Mapping(f, t.Key);
            t.Sum = op.Mapping(f, t.Sum);
        }

        /// <summary>
        /// [<paramref name="l"/>, <paramref name="r"/>) に <paramref name="f"/> を作用させます。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public void Apply(int l, int r, F f)
        {
            var (xl, xr) = Split(root, l);
            var (yl, yr) = Split(xr, r - l);
            Propagate(yl, f);
            root = Merge(xl, Merge(yl, yr));
        }

        [凾(256)]
        private static void Push(Node t)
        {
            if (t.rev)
            {
                if (t.Left != null) Reverse(t.Left);
                if (t.Right != null) Reverse(t.Right);
                t.rev = false;
            }
            if (!EqualityComparer<F>.Default.Equals(t.Lazy, op.FIdentity))
            {
                if (t.Left != null) Propagate(t.Left, t.Lazy);
                if (t.Right != null) Propagate(t.Right, t.Lazy);
                t.Lazy = op.FIdentity;
            }
        }
        [凾(256)]
        private static Node Update(Node t)
        {
            Push(t);
            t.cnt = 1;
            t.Sum = t.Key;
            if (t.Left != null)
            {
                t.cnt += t.Left.cnt;
                t.Sum = op.Operate(t.Left.Sum, t.Sum);
            }
            if (t.Right != null)
            {
                t.cnt += t.Right.cnt;
                t.Sum = op.Operate(t.Sum, t.Right.Sum);
            }
            return t;
        }

        [凾(256)] private static int Size(Node node) => node?.cnt ?? 0;
        [凾(256)] private static T Sum(Node node) => node != null ? node.Sum : op.Identity;

        [凾(256)]
        private static ulong Random()
        {
            Span<byte> b = stackalloc byte[8];
            rnd.NextBytes(b);
            return MemoryMarshal.Cast<byte, ulong>(b)[0];
        }
#if NET7_0_OR_GREATER
        private static Random rnd = System.Random.Shared;
#else
        private static Random rnd = new Random();
#endif

        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public struct Enumerator : IEnumerator<T>
        {
            internal readonly RandomBinarySearchTree<T, F, TOp> tree;
            readonly Deque<Node> stack;
            Node current;

            readonly bool reverse;
            internal Enumerator(RandomBinarySearchTree<T, F, TOp> tree) : this(tree, false) { }
            internal Enumerator(RandomBinarySearchTree<T, F, TOp> tree, bool reverse)
            {
                this.tree = tree;
                stack = new Deque<Node>(2 * Log2(this.tree.Count + 1));
                current = null;
                this.reverse = reverse;
                IntializeAll();
            }
            [凾(256)]
            void IntializeAll()
            {
                var node = tree.root;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.AddLast(Update(node));
                    node = next;
                }
            }

            [凾(256)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public T Current => current.Key;

            [凾(256)]
            public bool MoveNext()
            {
                if (stack.Count == 0)
                {
                    current = null;
                    return false;
                }
                current = stack.PopLast();
                var node = reverse ? current.Left : current.Right;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.AddLast(Update(node));
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
