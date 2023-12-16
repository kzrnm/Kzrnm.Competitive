using AtCoder;
using AtCoder.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/data-structure-2d/2d-segment-tree.hpp
namespace Kzrnm.Competitive
{
    ///// <summary>
    ///// <see cref="AtCoder.Segtree{TValue, TOp}"/> を2次元配列上で扱います。
    ///// </summary>
    //public class Segtree2D
    //{
    //}
    /// <summary>
    /// 大きさ H × W の2次元配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>要素の 1 点変更</description>
    /// </item>
    /// <item>
    /// <description>領域の要素の総積の取得</description>
    /// </item>
    /// </list>
    /// <para>を O(log H log W) で求めることが出来るデータ構造です。</para>
    /// </summary>
    public class Segtree2D<TValue, TOp> where TOp : struct, ISegtreeOperator<TValue>
    {
        static readonly TOp op = default;
        internal readonly int logH, logW, H, W;
        public readonly TValue[] d;


        /// <summary>
        /// 大きさ <paramref name="h"/> × <paramref name="w"/> の2次元配列 a　を持つ <see cref="Segtree2D{TValue, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <code>TOp.Identity</code> です。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(<paramref name="h"/>×<paramref name="w"/>)</para>
        /// </remarks>
        /// <param name="h">配列の高さ</param>
        /// <param name="w">配列の幅</param>
        public Segtree2D(int h, int w)
        {
            logH = InternalBit.CeilPow2(h);
            logW = InternalBit.CeilPow2(w);
            H = 1 << logH;
            W = 1 << logW;
            d = new TValue[4 * H * W];
            d.AsSpan().Fill(op.Identity);
        }

        /// <summary>
        /// 大きさ h=<paramref name="v"/>.Length × w=<paramref name="v"/>[0].Length の2次元配列 a　を持つ <see cref="Segtree2D{TValue, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <param name="v">初期配列</param>
        public Segtree2D(TValue[][] v) : this(v.Length, v[0].Length)
        {
            for (int h = 0; h < v.Length; h++)
                v[h].CopyTo(d.AsSpan(2 * (h + H) * W + W, W));

            // w in [W, 2W)
            for (int w = W; w < 2 * W; w++)
                for (int h = H - 1; h > 0; h--)
                    d[Index(h, w)] = op.Operate(d[Index(2 * h + 0, w)], d[Index(2 * h + 1, w)]);
            // h in [0, 2H)
            for (int h = 0; h < 2 * H; h++)
                for (int w = W - 1; w > 0; w--)
                    d[Index(h, w)] = op.Operate(d[Index(h, 2 * w + 0)], d[Index(h, 2 * w + 1)]);
        }

        [凾(256)] int Index(int h, int w) => 2 * h * W + w;

        /// <summary>
        /// a[<paramref name="h"/>, <paramref name="w"/>] を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="h"/>&lt;H, 0≤<paramref name="w"/>&lt;W</para>
        /// <para>計算量(set): O(log H log W)</para>
        /// <para>計算量(get): O(1)</para>
        /// </remarks>
        /// <returns></returns>
        public TValue this[int h, int w]
        {
            [凾(256)]
            set
            {
                h += H;
                w += W;
                d[Index(h, w)] = value;
                for (int i = h >> 1; i > 0; i >>= 1)
                    d[Index(i, w)] = op.Operate(d[Index(2 * i + 0, w)], d[Index(2 * i + 1, w)]);
                for (; h > 0; h >>= 1)
                    for (int j = w >> 1; j > 0; j >>= 1)
                        d[Index(h, j)] = op.Operate(d[Index(h, 2 * j + 0)], d[Index(h, 2 * j + 1)]);
            }
            [凾(256)]
            get => d[Index(h + H, w + W)];
        }

        /// <summary>
        /// [h1, h2) かつ [w1, w2) の半開区間の総積を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log H log W)</para>
        /// </remarks>
        [凾(256)]
        public TValue Prod(int h1, int w1, int h2, int w2)
        {
            var res = op.Identity;
            if (h1 >= h2 || w1 >= w2) return res;
            h1 += H; h2 += H; w1 += W; w2 += W;
            for (; h1 < h2; h1 >>= 1, h2 >>= 1)
            {
                if ((h1 & 1) != 0)
                    res = op.Operate(res, Prod(h1++, w1, w2));
                if ((h2 & 1) != 0)
                    res = op.Operate(res, Prod(--h2, w1, w2));
            }
            return res;
        }

        /// <summary>
        /// 内部用
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log W)</para>
        /// </remarks>
        [凾(256)]
        TValue Prod(int h, int w1, int w2)
        {
            var res = op.Identity;
            for (; w1 < w2; w1 >>= 1, w2 >>= 1)
            {
                if ((w1 & 1) != 0)
                    res = op.Operate(res, d[Index(h, w1++)]);
                if ((w2 & 1) != 0)
                    res = op.Operate(res, d[Index(h, --w2)]);
            }
            return res;
        }
    }
}
