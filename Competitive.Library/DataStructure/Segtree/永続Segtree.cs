#define ATCODER_CONTRACT
using AtCoder;
using AtCoder.Internal;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Original:
// https://github.com/ei1333/library

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    [DebuggerTypeProxy(typeof(PersistentSegtree<,>.DebugView))]
    public class PersistentSegtree<TValue, TOp> where TOp : struct, ISegtreeOperator<TValue>
    {
        private static readonly TOp op = default;

        public class Node
        {
            public readonly TValue data;
            internal Node left, right;
            public Node(TValue data, Node left = null, Node right = null)
            {
                this.data = data;
                this.left = left;
                this.right = right;
            }
        }

        /// <summary>
        /// 数列 a の長さ n を返します。
        /// </summary>
        public int Length { get; }

        internal readonly Node Root;

        /// <summary>
        /// 長さ <paramref name="n"/> の数列 a　を持つ <see cref="PersistentSegtree{TValue, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <see cref="TOp.Identity"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public PersistentSegtree(int n) : this(new TValue[n].Fill(op.Identity)) { }
        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="PersistentSegtree{TValue, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public PersistentSegtree(TValue[] v) : this(v.Length, Build(0, v.Length, v)) { }

        private PersistentSegtree(int length, Node root)
        {
            Length = length;
            Root = root;
        }

        /// <summary>
        /// 配列を元に二分木を初期化して根となるノードを返す
        /// </summary>
        private static Node Build(int l, int r, TValue[] v)
        {
            if (l + 1 >= r) return new Node(v[l]);
            return Merge(Build(l, (l + r) >> 1, v), Build((l + r) >> 1, r, v));
        }

        [MethodImpl(AggressiveInlining)]
        private static Node Merge(Node left, Node right) => new Node(op.Operate(left.data, right.data), left, right);


        /// <summary>
        /// a[<paramref name="p"/>] に <paramref name="v"/> を代入した <see cref="PersistentSegtree{TValue, TOp}"/> を返します。
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public PersistentSegtree<TValue, TOp> SetItem(int p, TValue v)
        {
            Contract.Assert((uint)p < (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(p)} && {nameof(p)} < Length");
            return new PersistentSegtree<TValue, TOp>(Length, Update(p, v, Root, 0, Length));
        }

        private static Node Update(int p, TValue v, Node n, int l, int r)
        {
            if (r <= p || p + 1 <= l)
                return n;
            else if (p <= l && r <= p + 1)
                return new Node(v);
            else
                return Merge(Update(p, v, n.left, l, (l + r) >> 1), Update(p, v, n.right, (l + r) >> 1, r));
        }


        /// <summary>
        /// a[<paramref name="p"/>] を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns></returns>
        public TValue this[int p]
        {
            [MethodImpl(AggressiveInlining)]
            get
            {
                Contract.Assert((uint)p < (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(p)} && {nameof(p)} < Length");
                return Prod(p, p + 1);
            }
        }

        [MethodImpl(AggressiveInlining)]
        public TValue Slice(int l, int len) => Prod(l, l + len);

        /// <summary>
        /// <see cref="TOp.Operate"/>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1]) を返します。<paramref name="l"/> = <paramref name="r"/> のときは　<see cref="TOp.Identity"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns><see cref="TOp.Operate"/>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1])</returns>
        [MethodImpl(AggressiveInlining)]
        public TValue Prod(int l, int r)
        {
            Contract.Assert(0U <= (uint)l && (uint)l <= (uint)r && (uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= {nameof(r)} && {nameof(r)} <= Length");
            return Query(l, r, Root, 0, Length);
        }

        [MethodImpl(AggressiveInlining)]
        private static TValue Query(int a, int b, Node k, int l, int r)
        {
            if (r <= a || b <= l)
                return op.Identity;
            else if (a <= l && r <= b)
                return k.data;
            else
                return op.Operate(Query(a, b, k.left, l, (l + r) >> 1), Query(a, b, k.right, (l + r) >> 1, r));
        }


        /// <summary>
        /// <see cref="TOp.Operate"/>(a[0], ..., a[n - 1]) を返します。n = 0 のときは　<see cref="TOp.Identity"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        /// <returns><see cref="TOp.Operate"/>(a[0], ..., a[n - 1])</returns>
        public TValue AllProd => Root.data;


        [DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
        private struct DebugItem
        {
            public DebugItem(int l, int r, TValue value)
            {
                if (r - l == 1)
                    key = $"[{l}]";
                else
                    key = $"[{l}-{r})";
                this.value = value;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly string key;
            private readonly TValue value;
        }
        private class DebugView
        {
            private readonly PersistentSegtree<TValue, TOp> segtree;
            public DebugView(PersistentSegtree<TValue, TOp> segtree)
            {
                this.segtree = segtree;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    Stack<(int r, Node n)>[] CreateTmps()
                    {
                        var tmps = new Stack<(int r, Node n)>[segtree.Length];
                        for (int i = 0; i < tmps.Length; i++)
                            tmps[i] = new Stack<(int r, Node n)>();

                        var queue = new Queue<(int l, int r, Node n)>(2 * segtree.Length);
                        queue.Enqueue((0, segtree.Length, segtree.Root));
                        while (queue.Count > 0)
                        {
                            var (l, r, n) = queue.Dequeue();
                            tmps[l].Push((r, n));
                            if (n.right != null)
                                queue.Enqueue(((l + r) >> 1, r, n.right));
                            if (n.left != null)
                                queue.Enqueue((l, (l + r) >> 1, n.left));
                        }
                        return tmps;
                    }
                    var items = new SimpleList<DebugItem>(2 * segtree.Length);
                    var tmps = CreateTmps();

                    int l = 0;
                    var (r, n) = tmps[0].Pop();
                    while (n != null)
                    {
                        items.Add(new DebugItem(l, r, n.data));
                        if (r < segtree.Length)
                        {
                            l = r;
                            (r, n) = tmps[l].Pop();
                        }
                        else if (tmps[0].Count > 0)
                        {
                            l = 0;
                            (r, n) = tmps[l].Pop();
                        }
                        else n = null;
                    }
                    return items.ToArray();
                }
            }
        }
    }
}
