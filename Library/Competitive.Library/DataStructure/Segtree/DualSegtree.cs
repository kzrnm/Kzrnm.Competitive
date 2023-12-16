using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 長さ N の配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>区間の要素に一括で <typeparamref name="TOp"/> の要素 f を作用 ( x=f(x) )</description>
    /// </item>
    /// <item>
    /// <description>要素の取得</description>
    /// </item>
    /// </list>
    /// <para>を O(log N) で求めることが出来るデータ構造です。</para>
    /// <para> サイズを別途保持することで使いやすくしてます。 </para>
    /// </summary>
    [DebuggerTypeProxy(typeof(DualSegtree<,>.DebugView))]
    public class DualSegtree<T, TOp> where TOp : struct, IDualSegtreeOperator<T>
    {
        static readonly TOp op = default;

        /// <summary>
        /// 数列 a の長さ n を返します。
        /// </summary>
        public int Length { get; }

        internal readonly int log;
        internal readonly int size;
        internal readonly T[] d;

        /// <summary>
        /// 長さ <paramref name="n"/> の数列 a　を持つ <see cref="DualSegtree{T, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <c>Identity</c> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public DualSegtree(int n)
        {
            Length = n;
            log = InternalBit.CeilPow2(n);
            size = 1 << log;
            d = new T[2 * size];
            Array.Fill(d, op.FIdentity);
        }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="DualSegtree{T, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<c>n</c>≤10^8</para>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public DualSegtree(T[] v) : this((ReadOnlySpan<T>)v) { }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="DualSegtree{T, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<c>n</c>≤10^8</para>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public DualSegtree(Span<T> v) : this((ReadOnlySpan<T>)v) { }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="DualSegtree{T, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<c>n</c>≤10^8</para>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public DualSegtree(ReadOnlySpan<T> v) : this(v.Length)
        {
            v.CopyTo(d.AsSpan(size));
        }

        [凾(256)]
        void AllApply(int k, T f)
        {
            d[k] = op.Composition(f, d[k]);
        }
        [凾(256)]
        void Push(int k)
        {
            AllApply(2 * k, d[k]);
            AllApply(2 * k + 1, d[k]);
            d[k] = op.FIdentity;
        }

        /// <summary>
        /// a[<paramref name="p"/>] を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns></returns>
        public T this[int p]
        {
            [凾(256)]
            set
            {
                Apply(p, op.FIdentity);
                d[p + size] = value;
            }
            [凾(256)]
            get
            {
                Contract.Assert((uint)p < (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(p)} && {nameof(p)} < Length");
                p += size;
                for (int i = log; i >= 1; i--) Push(p >> i);
                return d[p];
            }
        }

        /// <summary>
        /// a[<paramref name="p"/>] = <c>Mapping</c>(<paramref name="f"/>, a[<paramref name="p"/>])
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public void Apply(int p, T f)
        {
            Contract.Assert((uint)p < (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(p)} && {nameof(p)} < Length");
            p += size;
            for (int i = log; i >= 1; i--) Push(p >> i);
            d[p] = op.Composition(f, d[p]);
        }

        /// <summary>
        /// i = <paramref name="l"/>..<paramref name="r"/> について <paramref name="f"/> を作用させます。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public void Apply(int l, int r, T f)
        {
            Contract.Assert(0U <= (uint)l && (uint)l <= (uint)r && (uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= {nameof(r)} && {nameof(r)} <= Length");
            if (l == r) return;

            l += size;
            r += size;

            for (int i = log; i >= 1; i--)
            {
                if (((l >> i) << i) != l) Push(l >> i);
                if (((r >> i) << i) != r) Push((r - 1) >> i);
            }

            while (l < r)
            {
                if ((l & 1) != 0) AllApply(l++, f);
                if ((r & 1) != 0) AllApply(--r, f);
                l >>= 1;
                r >>= 1;
            }
        }

        /// <summary>
        /// 現在のセグ木の中身を配列にコピーして返します。
        /// </summary>
        public T[] ToArray()
        {
            var data = d;
            var p = data.Length / 2;
            for (int i = 0; i < p; i++)
                Push(i);
            return data.AsSpan(p, Length).ToArray();
        }

#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        [DebuggerDisplay("Lazy = {" + nameof(Lazy) + "}", Name = "{" + nameof(Key) + ",nq}")]
        internal readonly struct DebugItem
        {
            public DebugItem(int l, int r, T lazy)
            {
                L = l;
                R = r;
                Lazy = lazy;
            }
            [DebuggerBrowsable(0)]
            public int L { get; }
            [DebuggerBrowsable(0)]
            public int R { get; }
            [DebuggerBrowsable(0)]
            public string Key => R - L == 1 ? $"[{L}]" : $"[{L}-{R})";
            public T Lazy { get; }
        }
#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        class DebugView
        {
            readonly DualSegtree<T, TOp> s;
            public DebugView(DualSegtree<T, TOp> segtree)
            {
                s = segtree;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var items = new List<DebugItem>(s.Length);
                    for (int len = s.size; len > 0; len >>= 1)
                    {
                        int unit = s.size / len;
                        for (int i = 0; i < len; i++)
                        {
                            int l = i * unit;
                            int r = l + unit;
                            if (l < s.Length)
                            {
                                int dataIndex = i + len;
                                items.Add(new DebugItem(l, r, s.d[dataIndex]));
                            }
                        }
                    }
                    return items.ToArray();
                }
            }
        }
    }
}