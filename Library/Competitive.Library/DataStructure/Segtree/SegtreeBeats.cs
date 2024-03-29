// https://rsm9.hatenablog.com/entry/2021/02/01/220408
using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// モノイドを定義するインターフェイスです。
    /// </summary>
    /// <typeparam name="T">操作を行う型。</typeparam>
    /// <typeparam name="F">写像の型。</typeparam>
    [IsOperator]
    public interface ISegtreeBeatsOperator<T, F>
    {
        /// <summary>
        /// <c>Operate(x, Identity) = x</c> を満たす単位元。
        /// </summary>
        T Identity { get; }
        /// <summary>
        /// <c>Mapping(FIdentity, x) = x</c> を満たす恒等写像。
        /// </summary>
        F FIdentity { get; }
        /// <summary>
        /// 結合律 Operate(Operate(a, b), c) = Operate(a, Operate(b, c)) を満たす写像。
        /// </summary>
        T Operate(T x, T y);
        /// <summary>
        /// 写像　<paramref name="f"/> を <paramref name="x"/> に作用させる関数。
        /// </summary>
        /// <returns>作用に成功したかどうかを返す</returns>
        bool Mapping(F f, T x, out T res);
        /// <summary>
        /// 写像　<paramref name="nf"/> を既存の写像 <paramref name="cf"/> に対して合成した写像 <paramref name="nf"/>∘<paramref name="cf"/>。
        /// </summary>
        F Composition(F nf, F cf);
    }
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
    /// </summary>
    [DebuggerTypeProxy(typeof(SegtreeBeats<,,>.DebugView))]
    public class SegtreeBeats<TValue, F, TOp> where TOp : struct, ISegtreeBeatsOperator<TValue, F>
    {
        static readonly TOp op = default;

        /// <summary>
        /// 数列 a の長さ n を返します。
        /// </summary>
        public int Length { get; }

        internal readonly int log;
        internal readonly int size;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly TValue[] d;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly F[] lz;


        /// <summary>
        /// 長さ <paramref name="n"/> の数列 a　を持つ <see cref="SegtreeBeats{TValue, F, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <c>op.Identity</c> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public SegtreeBeats(int n)
        {
            Length = n;
            log = InternalBit.CeilPow2(n);
            size = 1 << log;
            d = new TValue[2 * size];
            lz = new F[size];
            Array.Fill(d, op.Identity);
            Array.Fill(lz, op.FIdentity);
        }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="SegtreeBeats{TValue, F, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>0≤n≤10^8</c></para>
        /// <para>計算量: <c>O(n)</c></para>
        /// </remarks>
        /// <param name="v">初期配列</param>
        public SegtreeBeats(TValue[] v) : this(v.Length)
        {
            for (int i = 0; i < v.Length; i++) d[size + i] = v[i];
            for (int i = size - 1; i >= 1; i--)
            {
                Update(i);
            }
        }


        [凾(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Update(int k) => d[k] = op.Operate(d[2 * k], d[2 * k + 1]);


        [凾(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AllApply(int k, F f)
        {
            var success = op.Mapping(f, d[k], out var res);
            d[k] = res;
            if (k < size)
            {
                lz[k] = op.Composition(f, lz[k]);
                if (!success)
                {
                    Push(k);
                    Update(k);
                }
            }
        }

        [凾(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Push(int k)
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
        public TValue this[int p]
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
        public TValue Slice(int l, int len) => Prod(l, l + len);

        /// <summary>
        /// <c>Operate</c>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1]) を返します。<paramref name="l"/> = <paramref name="r"/> のときは　<c>Identity</c> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns><c>Operate</c>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1])</returns>
        [凾(256)]
        public TValue Prod(int l, int r)
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

            TValue sml = op.Identity, smr = op.Identity;
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
        /// <c>Operate</c>(a[0], ..., a[n - 1]) を返します。n = 0 のときは　<c>Identity</c> を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        /// <returns><c>Operate</c>(a[0], ..., a[n - 1])</returns>
        public TValue AllProd => d[1];

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
            var success = op.Mapping(f, d[p], out var res);
            Contract.Assert(success);
            d[p] = res;
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
        /// <description><c><paramref name="g"/>(Identity)</c> = true</description>
        /// </item>
        /// <item>
        /// <description>0≤<paramref name="l"/>≤n</description>
        /// </item>
        /// </list>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public int MaxRight(int l, Predicate<TValue> g)
        {
            Contract.Assert((uint)l <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= Length");
            Contract.Assert(g(op.Identity), reason: $"{nameof(g)}({nameof(TOp)}.{nameof(ISegtreeBeatsOperator<TValue, F>.Identity)}) must be true.");
            if (l == Length) return Length;
            l += size;
            for (int i = log; i >= 1; i--) Push(l >> i);
            TValue sm = op.Identity;
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
        /// <description><c><paramref name="g"/>(Identity)</c> = true</description>
        /// </item>
        /// <item>
        /// <description>0≤<paramref name="r"/>≤n</description>
        /// </item>
        /// </list>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public int MinLeft(int r, Predicate<TValue> g)
        {
            Contract.Assert((uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(r)} && {nameof(r)} <= Length");
            Contract.Assert(g(op.Identity), reason: $"{nameof(g)}({nameof(TOp)}.{nameof(ISegtreeBeatsOperator<TValue, F>.Identity)}) must be true.");
            if (r == 0) return 0;
            r += size;
            for (int i = log; i >= 1; i--) Push((r - 1) >> i);
            TValue sm = op.Identity;
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

#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        [DebuggerDisplay("Value = {" + nameof(Value) + "}, Lazy = {" + nameof(Lazy) + "}", Name = "{" + nameof(Key) + ",nq}")]
        internal readonly struct DebugItem
        {
            public DebugItem(int l, int r, TValue value, F lazy)
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
            public TValue Value { get; }
            public F Lazy { get; }
        }
#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        class DebugView
        {
            readonly SegtreeBeats<TValue, F, TOp> segtree;
            public DebugView(SegtreeBeats<TValue, F, TOp> segtree)
            {
                this.segtree = segtree;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var items = new List<DebugItem>(segtree.Length);
                    for (int len = segtree.size; len > 0; len >>= 1)
                    {
                        int unit = segtree.size / len;
                        for (int i = 0; i < len; i++)
                        {
                            int l = i * unit;
                            int r = Math.Min(l + unit, segtree.Length);
                            if (l < segtree.Length)
                            {
                                int dataIndex = i + len;
                                if ((uint)dataIndex < segtree.lz.Length)
                                    items.Add(new DebugItem(l, r, segtree.d[dataIndex], segtree.lz[dataIndex]));
                                else
                                    items.Add(new DebugItem(l, r, segtree.d[dataIndex], op.FIdentity));
                            }
                        }
                    }
                    return items.ToArray();
                }
            }
        }
    }
}