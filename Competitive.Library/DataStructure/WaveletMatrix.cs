// Original: https://ei1333.github.io/library/structure/wavelet/wavelet-matrix.cpp.html
using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、オンラインでいろいろなクエリを処理するデータ構造
    /// </summary>
    public class WaveletMatrix<T> : WaveletMatrix<T, DefaultComparerStruct<T>> where T : struct, IComparable<T>
    {
        /// <summary>
        /// <para>各要素の高さ <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrix(ReadOnlySpan<T> v) : base(v, new DefaultComparerStruct<T>()) { }
    }

    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、オンラインでいろいろなクエリを処理するデータ構造
    /// </summary>
    public class WaveletMatrix<T, TOp>
        where T : struct
        where TOp : IComparer<T>
    {
        private TOp comparer;
        private readonly WaveletMatrixCompressed mat;
        private readonly Dictionary<T, int> pos;
        private readonly T[] ys;

        /// <summary>
        /// <para>各要素の高さ <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrix(ReadOnlySpan<T> v, TOp comparer)
        {
            var zahyoCompress = new ZahyoCompress<T>(v).Compress();
            pos = zahyoCompress.NewTable;
            ys = zahyoCompress.Original;
            mat = new WaveletMatrixCompressed(zahyoCompress.Replace(v), zahyoCompress.Original.Length);
            this.comparer = comparer;
        }

        /// <summary>
        /// <para><paramref name="k"/> 番目の要素を取得する。</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        public T this[int k]
        {
            [MethodImpl(AggressiveInlining)]
            get => ys[mat[k]];
        }

        /// <summary>
        /// <para>区間 [0, <paramref name="r"/>) に含まれる <paramref name="x"/> の個数を返す。</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        public int Rank(T x, int r)
            => pos.TryGetValue(x, out var p) ? mat.Rank(p, r) : 0;

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="k"/> 番目(0-indexed) に小さいものを返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public T KthSmallest(int l, int r, int k)
            => ys[mat.KthSmallest(l, r, k)];

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="k"/> 番目(0-indexed) に大きいものを返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public T KthLargest(int l, int r, int k)
            => ys[mat.KthLargest(l, r, k)];

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち [<paramref name="lower"/>, <paramref name="upper"/>) である要素数を返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public int RangeFreq(int l, int r, T lower, T upper)
            => mat.RangeFreq(l, r, ys.LowerBound(lower, comparer), ys.LowerBound(upper, comparer));

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち [0, <paramref name="upper"/>) である要素数を返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public int RangeFreq(int l, int r, T upper)
            => mat.RangeFreq(l, r, ys.LowerBound(upper, comparer));

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="upper"/> の次に小さいものを返す</para>
        /// <para>見つからなければnullを返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public T? PrevValue(int l, int r, T upper)
        {
            var res = mat.PrevValue(l, r, ys.LowerBound(upper, comparer));
            if (res >= 0)
                return ys[res];
            return null;
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="lower"/> の次に大きいものを返す</para>
        /// <para>見つからなければnullを返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public T? NextValue(int l, int r, T lower)
        {
            var res = mat.NextValue(l, r, ys.LowerBound(lower, comparer));
            if (res >= 0)
                return ys[res];
            return null;
        }
    }


    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、オンラインでいろいろなクエリを処理するデータ構造
    /// </summary>
    public class WaveletMatrixCompressed
    {
        private SuccinctIndexableDictionary[] matrix;
        private int[] mid;
        private int Length { get; }

        /// <summary>
        /// <para>各要素の高さ <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrixCompressed(ReadOnlySpan<int> v, int max)
        {
            var vv = v.ToArray();
            Length = vv.Length;
            var l = new int[Length];
            var r = new int[Length];
            var log = BitOperations.Log2((uint)max) + 1;

            matrix = new SuccinctIndexableDictionary[log];
            mid = new int[log];

            for (int level = log - 1; level >= 0; level--)
            {
                matrix[level] = new SuccinctIndexableDictionary(Length + 1);
                int left = 0, right = 0;
                for (int i = 0; i < Length; i++)
                {
                    if (((vv[i] >> level) & 1) != 0)
                    {
                        matrix[level].Set(i);
                        r[right++] = vv[i];
                    }
                    else
                    {
                        l[left++] = vv[i];
                    }
                }
                mid[level] = left;
                matrix[level].Build();
                (vv, l) = (l, vv);
                for (int i = 0; i < right; i++)
                {
                    vv[left + i] = r[i];
                }
            }
        }


        private (int, int) Succ(bool f, int l, int r, int level)
             => (matrix[level].Rank(f, l) + (f ? mid[level] : 0), matrix[level].Rank(f, r) + (f ? mid[level] : 0));

        /// <summary>
        /// <para><paramref name="k"/> 番目の要素を取得する。</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        public int this[int k]
        {
            get
            {
                int ret = 0;
                for (int level = matrix.Length - 1; level >= 0; level--)
                {
                    bool f = matrix[level][k];
                    if (f) ret |= 1 << level;
                    k = matrix[level].Rank(f, k) + (f ? mid[level] : 0);
                }
                return ret;
            }
        }

        /// <summary>
        /// <para>区間 [0, <paramref name="r"/>) に含まれる <paramref name="x"/> の個数を返す。</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        public int Rank(int x, int r)
        {
            int l = 0;
            for (int level = matrix.Length - 1; level >= 0; level--)
            {
                (l, r) = Succ(((x >> level) & 1) != 0, l, r, level);
            }
            return r - l;
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="k"/> 番目(0-indexed) に小さいものを返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        public int KthSmallest(int l, int r, int k)
        {
            Contract.Assert(0 <= k && k < r - l, reason: $"IndexOutOfRange: 0 <= k && k < r - l");
            int res = 0;
            for (int level = matrix.Length - 1; level >= 0; level--)
            {
                int cnt = matrix[level].Rank(false, r) - matrix[level].Rank(false, l);
                bool f = cnt <= k;
                if (f)
                {
                    res |= 1 << level;
                    k -= cnt;
                }
                (l, r) = Succ(f, l, r, level);
            }
            return res;
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="k"/> 番目(0-indexed) に大きいものを返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public int KthLargest(int l, int r, int k) => KthSmallest(l, r, r - l - k - 1);

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち [<paramref name="lower"/>, <paramref name="upper"/>) である要素数を返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public int RangeFreq(int l, int r, int lower, int upper)
            => RangeFreq(l, r, upper) - RangeFreq(l, r, lower);

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち [0, <paramref name="upper"/>) である要素数を返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        public int RangeFreq(int l, int r, int upper)
        {
            int res = 0;
            for (int level = matrix.Length - 1; level >= 0; level--)
            {
                bool f = ((upper >> level) & 1) != 0;
                if (f) res += matrix[level].Rank(false, r) - matrix[level].Rank(false, l);
                (l, r) = Succ(f, l, r, level);
            }
            return res;
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="upper"/> の次に小さいものを返す</para>
        /// <para>見つからなければ-1を返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public int PrevValue(int l, int r, int upper)
        {
            int cnt = RangeFreq(l, r, upper);
            return cnt == 0 ? -1 : KthSmallest(l, r, cnt - 1);
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>) に含まれる要素のうち <paramref name="lower"/> 以上で最小のものを返す</para>
        /// <para>見つからなければ-1を返す</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public int NextValue(int l, int r, int lower)
        {
            int cnt = RangeFreq(l, r, lower);
            return cnt == r - l ? -1 : KthSmallest(l, r, cnt);
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

            [MethodImpl(AggressiveInlining)]
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
                [MethodImpl(AggressiveInlining)]
                get => ((bit[k >> 5] >> (k & 0x1F)) & 1) != 0;
            }

            [MethodImpl(AggressiveInlining)]
            public int Rank(int k) => (int)(sum[k >> 5] + (uint)BitOperations.PopCount(bit[k >> 5] & ((1U << (k & 0x1F)) - 1)));

            [MethodImpl(AggressiveInlining)]
            public int Rank(bool val, int k) => val ? Rank(k) : k - Rank(k);
        }
    }
}
