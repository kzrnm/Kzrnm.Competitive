// https://ei1333.github.io/library/structure/heap/skew-heap.cpp
using AtCoder;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// マージ可能なヒープ
    /// </summary>
    /// <typeparam name="T">キー</typeparam>
    /// <typeparam name="F">作用素</typeparam>
    /// <typeparam name="TOp">オペレーター</typeparam>
    public class SkewHeap<T, F, TOp>
        where TOp : struct, ISkewHeapOperator<T, F>
    {
        private static TOp op => new TOp();
        public class Node
        {
            public Node(T key, int index)
            {
                this.key = key;
                this.index = index;
                lazy = op.FIdentity;
            }
            public readonly int index;
            public T key;
            public F lazy;
            public Node left, right;
        }

        /// <summary>
        /// ノードをマージする。
        /// </summary>
        [凾(256)]
        public Node Merge(Node x, Node y)
        {
            Propagate(x);
            Propagate(y);
            if (x == null || y == null) return x ?? y;
            if (op.Compare(x.key, y.key) > 0)
                (x, y) = (y, x);
            (x.right, x.left) = (x.left, Merge(y, x.right));
            return x;
        }

        /// <summary>
        /// <paramref name="root"/> に <paramref name="key"/> を追加する。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N))</para>
        /// </remarks>
        [凾(256)]
        public Node Push(Node root, T key, int idx = -1)
            => Merge(root, new Node(key, idx));

        /// <summary>
        /// <paramref name="root"/> から値を取り出す。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N))</para>
        /// </remarks>
        [凾(256)]
        public Node Pop(Node root)
            => Merge(root.left, root.right);

        /// <summary>
        /// <paramref name="root"/> に値を追加する。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N))</para>
        /// </remarks>
        [凾(256)]
        public Node Add(Node root, F lazy)
        {
            if (root != null)
            {
                root.lazy = op.Composition(lazy, root.lazy);
                Propagate(root);
            }
            return root;
        }

        /// <summary>
        /// lazy を伝播させる
        /// </summary>
        [凾(256)]
        static Node Propagate(Node t)
        {
            if (t != null)
            {
                var lazy = t.lazy;
                if (t.left is { } left) left.lazy = op.Composition(lazy, left.lazy);
                if (t.right is { } right) right.lazy = op.Composition(lazy, right.lazy);
                t.key = op.Mapping(lazy, t.key);
                t.lazy = op.FIdentity;
            }
            return t;
        }
    }
    [IsOperator]
    public interface ISkewHeapOperator<T, F> : IComparer<T>
    {
        /// <summary>
        /// <c>Mapping(FIdentity, x) = x</c> を満たす恒等写像。
        /// </summary>
        F FIdentity { get; }
        /// <summary>
        /// 写像　<paramref name="f"/> を <paramref name="x"/> に作用させる関数。
        /// </summary>
        T Mapping(F f, T x);
        /// <summary>
        /// 写像　<paramref name="nf"/> を既存の写像 <paramref name="cf"/> に対して合成した写像 <paramref name="nf"/>∘<paramref name="cf"/>。
        /// </summary>
        F Composition(F nf, F cf);
    }
}
// @brief マージ可能ヒープ
