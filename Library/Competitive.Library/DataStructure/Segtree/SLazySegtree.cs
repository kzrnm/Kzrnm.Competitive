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
    /// <description>区間の要素の総積の取得</description>
    /// </item>
    /// </list>
    /// <para>を O(log N) で求めることが出来るデータ構造です。</para>
    /// <para> サイズを別途保持することで使いやすくしてます。 </para>
    /// </summary>
    [DebuggerTypeProxy(typeof(SLazySegtree<,,>.DebugView))]
    public class SLazySegtree<T, F, TOp> where TOp : struct, ISLazySegtreeOperator<T, F>
    {
        private static readonly TOp op = default;

        /// <summary>
        /// 数列 a の長さ n を返します。
        /// </summary>
        public int Length { get; }

        internal readonly int log;
        internal readonly int size;
        private readonly T[] d;
        private readonly int[] valSize;
        private readonly F[] lz;


        /// <summary>
        /// 長さ <paramref name="n"/> の数列 a　を持つ <see cref="SLazySegtree{TValue, F, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <c>Identity</c> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public SLazySegtree(int n)
        {
            Length = n;
            log = InternalBit.CeilPow2(n);
            size = 1 << log;
            d = new T[2 * size];
            lz = new F[size];
            Array.Fill(d, op.Identity);
            Array.Fill(lz, op.FIdentity);

            valSize = new int[2 * size];
            Array.Fill(valSize, 1, size, n);
            for (int i = size - 1; i >= 1; i--)
                valSize[i] = valSize[2 * i] + valSize[2 * i + 1];
        }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="SLazySegtree{TValue, F, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<c>n</c>≤10^8</para>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public SLazySegtree(T[] v) : this((ReadOnlySpan<T>)v) { }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="SLazySegtree{TValue, F, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<c>n</c>≤10^8</para>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public SLazySegtree(Span<T> v) : this((ReadOnlySpan<T>)v) { }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="SLazySegtree{TValue, F, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<c>n</c>≤10^8</para>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public SLazySegtree(ReadOnlySpan<T> v) : this(v.Length)
        {
            v.CopyTo(d.AsSpan(size));
            for (int i = size - 1; i >= 1; i--)
            {
                Update(i);
            }
        }

        [凾(256)]
        private void Update(int k) => d[k] = op.Operate(d[2 * k], d[2 * k + 1]);

        [凾(256)]
        private void AllApply(int k, F f)
        {
            d[k] = op.Mapping(f, d[k], valSize[k]);
            if (k < size) lz[k] = op.Composition(f, lz[k]);
        }
        [凾(256)]
        private void Push(int k)
        {
            AllApply(2 * k, lz[k]);
            AllApply(2 * k + 1, lz[k]);
            lz[k] = op.FIdentity;
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
                Contract.Assert((uint)p < (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(p)} && {nameof(p)} < Length");
                p += size;
                for (int i = log; i >= 1; i--) Push(p >> i);
                d[p] = value;
                for (int i = 1; i <= log; i++) Update(p >> i);
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

        [凾(256)]
        public T Slice(int l, int len) => Prod(l, l + len);

        /// <summary>
        /// <c>op.Operate</c>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1]) を返します。<paramref name="l"/> = <paramref name="r"/> のときは　<c>Identity</c> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns><c>op.Operate</c>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1])</returns>
        [凾(256)]
        public T Prod(int l, int r)
        {
            Contract.Assert(0U <= (uint)l && (uint)l <= (uint)r && (uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= {nameof(r)} && {nameof(r)} <= Length");
            if (l == r) return op.Identity;

            l += size;
            r += size;

            for (int i = log; i >= 1; i--)
            {
                if (((l >> i) << i) != l) Push(l >> i);
                if (((r >> i) << i) != r) Push(r >> i);
            }

            T sml = op.Identity, smr = op.Identity;
            while (l < r)
            {
                if ((l & 1) != 0) sml = op.Operate(sml, d[l++]);
                if ((r & 1) != 0) smr = op.Operate(d[--r], smr);
                l >>= 1;
                r >>= 1;
            }

            return op.Operate(sml, smr);
        }

        /// <summary>
        /// <c>op.Operate</c>(a[0], ..., a[n - 1]) を返します。n = 0 のときは　<c>Identity</c> を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        /// <returns><c>op.Operate</c>(a[0], ..., a[n - 1])</returns>
        public T AllProd => d[1];

        /// <summary>
        /// a[<paramref name="p"/>] = <c>Mapping</c>(<paramref name="f"/>, a[<paramref name="p"/>])
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public void Apply(int p, F f)
        {
            Contract.Assert((uint)p < (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(p)} && {nameof(p)} < Length");
            p += size;
            for (int i = log; i >= 1; i--) Push(p >> i);
            d[p] = op.Mapping(f, d[p], valSize[p]);
            for (int i = 1; i <= log; i++) Update(p >> i);
        }

        /// <summary>
        /// i = <paramref name="l"/>..<paramref name="r"/> について <paramref name="f"/> を作用させます。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public void Apply(int l, int r, F f)
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

            {
                int l2 = l, r2 = r;
                while (l < r)
                {
                    if ((l & 1) != 0) AllApply(l++, f);
                    if ((r & 1) != 0) AllApply(--r, f);
                    l >>= 1;
                    r >>= 1;
                }
                l = l2;
                r = r2;
            }

            for (int i = 1; i <= log; i++)
            {
                if (((l >> i) << i) != l) Update(l >> i);
                if (((r >> i) << i) != r) Update((r - 1) >> i);
            }
        }



        /// <summary>
        /// 以下の条件を両方満たす r を(いずれか一つ)返します。
        /// <list type="bullet">
        /// <item>
        /// <description>r = <paramref name="l"/> もしくは <paramref name="g"/>(op(a[<paramref name="l"/>], a[<paramref name="l"/> + 1], ..., a[r - 1])) = true</description>
        /// </item>
        /// <item>
        /// <description>r = n もしくは <paramref name="g"/>(op(a[<paramref name="l"/>], a[<paramref name="l"/> + 1], ..., a[r])) = false</description>
        /// </item>
        /// </list>
        /// <para><paramref name="g"/> が単調だとすれば、<paramref name="g"/>(op(a[<paramref name="l"/>], a[<paramref name="l"/> + 1], ..., a[r - 1])) = true となる最大の r、と解釈することが可能です。</para>
        /// </summary>
        /// <remarks>
        /// 制約
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="g"/> を同じ引数で呼んだ時、返り値は等しい(=副作用はない)。</description>
        /// </item>
        /// <item>
        /// <description><paramref name="g"/>(<c>Identity</c>) = true</description>
        /// </item>
        /// <item>
        /// <description>0≤<paramref name="l"/>≤n</description>
        /// </item>
        /// </list>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public int MaxRight(int l, Predicate<T> g)
        {
            Contract.Assert((uint)l <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= Length");
            Contract.Assert(g(op.Identity), reason: $"{nameof(g)}({nameof(TOp)}.{nameof(ISLazySegtreeOperator<T, F>.Identity)}) must be true.");
            if (l == Length) return Length;
            l += size;
            for (int i = log; i >= 1; i--) Push(l >> i);
            T sm = op.Identity;
            do
            {
                while (l % 2 == 0) l >>= 1;
                if (!g(op.Operate(sm, d[l])))
                {
                    while (l < size)
                    {
                        Push(l);
                        l = (2 * l);
                        if (g(op.Operate(sm, d[l])))
                        {
                            sm = op.Operate(sm, d[l]);
                            l++;
                        }
                    }
                    return l - size;
                }
                sm = op.Operate(sm, d[l]);
                l++;
            } while ((l & -l) != l);

            return Length;
        }

        /// <summary>
        /// 以下の条件を両方満たす r を(いずれか一つ)返します。
        /// <list type="bullet">
        /// <item>
        /// <description>l = <paramref name="r"/> もしくは <paramref name="g"/>(op(a[l], a[l + 1], ..., a[<paramref name="r"/> - 1])) = true</description>
        /// </item>
        /// <item>
        /// <description>l = 0 もしくは <paramref name="g"/>(op(a[l - 1], a[l], ..., a[<paramref name="r"/> - 1])) = false</description>
        /// </item>
        /// </list>
        /// <para><paramref name="g"/> が単調だとすれば、<paramref name="g"/>(op(a[l], a[l + 1], ..., a[<paramref name="r"/> - 1])) = true となる最小の l、と解釈することが可能です。</para>
        /// </summary>
        /// <remarks>
        /// 制約
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="g"/> を同じ引数で呼んだ時、返り値は等しい(=副作用はない)。</description>
        /// </item>
        /// <item>
        /// <description><paramref name="g"/>(<c>Identity</c>) = true</description>
        /// </item>
        /// <item>
        /// <description>0≤<paramref name="r"/>≤n</description>
        /// </item>
        /// </list>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public int MinLeft(int r, Predicate<T> g)
        {
            Contract.Assert((uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(r)} && {nameof(r)} <= Length");
            Contract.Assert(g(op.Identity), reason: $"{nameof(g)}({nameof(TOp)}.{nameof(ISLazySegtreeOperator<T, F>.Identity)}) must be true.");
            if (r == 0) return 0;
            r += size;
            for (int i = log; i >= 1; i--) Push((r - 1) >> i);
            T sm = op.Identity;
            do
            {
                r--;
                while (r > 1 && (r % 2) != 0) r >>= 1;
                if (!g(op.Operate(d[r], sm)))
                {
                    while (r < size)
                    {
                        Push(r);
                        r = (2 * r + 1);
                        if (g(op.Operate(d[r], sm)))
                        {
                            sm = op.Operate(d[r], sm);
                            r--;
                        }
                    }
                    return r + 1 - size;
                }
                sm = op.Operate(d[r], sm);
            } while ((r & -r) != r);
            return 0;
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
        [DebuggerDisplay("Value = {" + nameof(Value) + "}, Lazy = {" + nameof(Lazy) + "}", Name = "{" + nameof(Key) + ",nq}")]
        internal readonly struct DebugItem
        {
            public DebugItem(int l, int r, T value, F lazy)
            {
                L = l;
                R = r;
                Value = value;
                Lazy = lazy;
            }
            [DebuggerBrowsable(0)]
            public int L { get; }
            [DebuggerBrowsable(0)]
            public int R { get; }
            [DebuggerBrowsable(0)]
            public string Key => R - L == 1 ? $"[{L}]" : $"[{L}-{R})";
            public T Value { get; }
            public F Lazy { get; }
        }
#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        private class DebugView
        {
            private readonly SLazySegtree<T, F, TOp> s;
            public DebugView(SLazySegtree<T, F, TOp> segtree)
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
                                if ((uint)dataIndex < s.lz.Length)
                                    items.Add(new DebugItem(l, r, s.d[dataIndex], s.lz[dataIndex]));
                                else
                                    items.Add(new DebugItem(l, r, s.d[dataIndex], op.FIdentity));
                            }
                        }
                    }
                    return items.ToArray();
                }
            }
        }
    }
}