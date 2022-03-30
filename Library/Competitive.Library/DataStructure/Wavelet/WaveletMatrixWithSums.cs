// Original: https://ei1333.github.io/library/structure/wavelet/wavelet-matrix.cpp.html
using AtCoder.Extension;
using AtCoder.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    public class WaveletMatrixWithSums<F, T, TOp>
        where F : IComparable<F>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
    {
        private readonly WaveletMatrixWithSumsCompressed<T, TOp> mat;
        private readonly Dictionary<F, int> pos;
        private readonly F[] ys;

        /// <summary>
        /// <para>各要素の高さと重み <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(N) log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrixWithSums(ReadOnlySpan<(F V, T d)> v)
        {
            var zahyoCompress = ZahyoCompress.Create(v.Select(t => t.V));
            pos = zahyoCompress.NewTable;
            ys = zahyoCompress.Original;

            var compressed = v.Select(t => (pos[t.V], t.d)).ToArray();
            mat = new WaveletMatrixWithSumsCompressed<T, TOp>(compressed, ys.Length);
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [<paramref name="lower"/>, <paramref name="upper"/>) である要素の重みを返す</para>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [凾(256)]
        public T RectSum(int l, int r, F lower, F upper)
            => mat.RectSum(l, r, ys.LowerBound(lower), ys.LowerBound(upper));

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [0, <paramref name="upper"/>) である要素の重みを返す</para>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [凾(256)]
        public T RectSum(int l, int r, F upper)
            => mat.RectSum(l, r, ys.LowerBound(upper));
    }


    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    public class WaveletMatrixWithSumsCompressed<T, TOp>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
    {
        private static TOp op => default;
        private SuccinctIndexableDictionary[] matrix;
        private int[] mid;
        private Sums<T, TOp>[] ds;
        private int Length { get; }

        /// <summary>
        /// <para>各要素の高さと重み <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(N) log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrixWithSumsCompressed(ReadOnlySpan<(int V, T d)> v, int max)
        {
            Length = v.Length;
            var l = new int[Length];
            var r = new int[Length];
            var ord = Enumerable.Range(0, Length).ToArray();
            var log = BitOperations.Log2((uint)max) + 1;
            matrix = new SuccinctIndexableDictionary[log];
            ds = new Sums<T, TOp>[log];
            mid = new int[log];

            for (int level = log - 1; level >= 0; level--)
            {
                matrix[level] = new SuccinctIndexableDictionary(Length + 1);

                int left = 0, right = 0;
                for (int i = 0; i < ord.Length; i++)
                {
                    if (((v[ord[i]].V >> level) & 1) != 0)
                    {
                        matrix[level].Set(i);
                        r[right++] = ord[i];
                    }
                    else
                    {
                        l[left++] = ord[i];
                    }
                }

                mid[level] = left;
                matrix[level].Build();
                (ord, l) = (l, ord);
                for (int i = 0; i < right; i++)
                    ord[left + i] = r[i];
                var dd = new T[Length];
                for (int i = 0; i < ord.Length; i++)
                    dd[i] = v[ord[i]].d;
                ds[level] = new Sums<T, TOp>(dd);
            }
        }

        [凾(256)]
        private (int, int) Succ(bool f, int l, int r, int level)
                     => (matrix[level].Rank(f, l) + (f ? mid[level] : 0), matrix[level].Rank(f, r) + (f ? mid[level] : 0));


        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [<paramref name="lower"/>, <paramref name="upper"/>) である要素の重みを返す</para>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [凾(256)]
        public T RectSum(int l, int r, int lower, int upper)
            => op.Subtract(RectSum(l, r, upper), RectSum(l, r, lower));

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [0, <paramref name="upper"/>) である要素の重みを返す</para>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [凾(256)]
        public T RectSum(int l, int r, int upper)
        {
            T ret = default;
            for (int level = ds.Length - 1; level >= 0; level--)
            {
                if (((upper >> level) & 1) != 0)
                {
                    var (f, t) = Succ(false, l, r, level);
                    ret = op.Add(ret, ds[level][f..t]);
                    l += mid[level] - f;
                    r += mid[level] - t;
                }
                else
                    (l, r) = Succ(false, l, r, level);
            }
            return ret;
        }

        private class SuccinctIndexableDictionary
        {
            readonly uint[] bit, sum;
            public SuccinctIndexableDictionary(int length)
            {
                var block = (length + 31) >> 5;
                bit = new uint[block];
                sum = new uint[block];
            }

            [凾(256)]
            public void Set(int k)
            {
                bit[k >> 5] |= 1U << (k & 0x1F);
            }
            public void Build()
            {
                sum[0] = 0U;
                for (int i = 1; i < sum.Length; i++)
                {
                    sum[i] = sum[i - 1] + (uint)BitOperations.PopCount(bit[i - 1]);
                }
            }

            public bool this[int k]
            {
                [凾(256)]
                get => ((bit[k >> 5] >> (k & 0x1F)) & 1) != 0;
            }

            [凾(256)]
            public int Rank(int k) => (int)(sum[k >> 5] + (uint)BitOperations.PopCount(bit[k >> 5] & ((1U << (k & 0x1F)) - 1)));

            [凾(256)]
            public int Rank(bool val, int k) => val ? Rank(k) : k - Rank(k);
        }
    }
}
