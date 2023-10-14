// Original: https://ei1333.github.io/library/structure/wavelet/wavelet-matrix.cpp.html
using AtCoder;
using AtCoder.Extension;
using AtCoder.Operators;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// competitive-verifier: TITLE 2次元WaveletMatrix(抽象)
namespace Kzrnm.Competitive
{
    namespace Internal
    {
        /// <summary>
        /// 範囲演算データ構造
        /// </summary>
        [IsOperator]
        public interface IWabeletSumOperator<T>
        {
            /// <summary>
            /// 範囲演算データ構造を <paramref name="ts"/> で初期化します。
            /// </summary>
            /// <param name="ts"></param>
            void Init(T[] ts);
            /// <summary>
            /// a[<paramref name="l"/>] + a[<paramref name="l"/> - 1] + ... + a[<paramref name="r"/> - 1] を返します。
            /// </summary>
            T Sum(int l, int r);
            /// <summary>
            /// a[<paramref name="p"/>] に <paramref name="v"/> を加算します。
            /// </summary>
            void Add(int p, T v);
        }

    }

    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の座標</typeparam>
    /// <typeparam name="T">重み</typeparam>
    /// <typeparam name="TOp">重みの加算・減算オペレータ</typeparam>
    /// <typeparam name="ROp">重みの総和を求めるデータ構造</typeparam>
    public class WaveletMatrix2D<F, T, TOp, ROp>
        where F : IComparable<F>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
        where ROp : struct, IWabeletSumOperator<T>
    {
        readonly WaveletMatrixRangeSumCompressed<T, TOp, ROp> mat;
        readonly Dictionary<(F x, F y), int> pos;
        readonly (F x, F y)[] ps;
        readonly F[] ys;

        /// <summary>
        /// <para>各要素の座標と重み <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(N) log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrix2D(ReadOnlySpan<((F x, F y) p, T d)> v)
        {
            var zahyoCompressXY = ZahyoCompress.Create(v.Select(t => t.p));
            var zahyoCompressY = ZahyoCompress.Create(v.Select(t => t.p.y));
            pos = zahyoCompressXY.NewTable;
            var posY = zahyoCompressY.NewTable;
            ps = zahyoCompressXY.Original;
            ys = zahyoCompressY.Original;

            var op = new TOp();
            var compressed = new (int V, T d)[ps.Length];
            foreach (var (p, d) in v)
            {
                int ix = pos[p];
                var dd = op.Add(compressed[ix].d, d);
                compressed[ix] = (posY[p.y], dd);
            }
            mat = new WaveletMatrixRangeSumCompressed<T, TOp, ROp>(compressed, ys.Length);
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [<paramref name="lower"/>, <paramref name="upper"/>) である要素の重みの和を返す</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </remarks>
        [凾(256)]
        public T RectSum(F l, F r, F lower, F upper)
            => mat.RectSum(
                ps.LowerBound(new TupleComp(l)),
                ps.LowerBound(new TupleComp(r)),
                ys.LowerBound(lower),
                ys.LowerBound(upper));

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [0, <paramref name="upper"/>) である要素の重みの和を返す</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </remarks>
        [凾(256)]
        public T RectSum(F l, F r, F upper)
            => mat.RectSum(
                ps.LowerBound(new TupleComp(l)),
                ps.LowerBound(new TupleComp(r)),
                ys.LowerBound(upper));

        /// <summary>
        /// <para>座標 (<paramref name="l"/>, <paramref name="r"/>) の要素に <paramref name="x"/> を加算する。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約: (<paramref name="l"/>, <paramref name="r"/>) は定義済みの座標</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </remarks>
        [凾(256)]
        public void PointAdd(F l, F r, T x) => mat.PointAdd(pos[(l, r)], x);
        readonly struct TupleComp : IComparable<(F, F)>
        {
            private readonly F v;
            public TupleComp(F v) { this.v = v; }
            [凾(256)] public int CompareTo((F, F) other) => -(other.Item1.CompareTo(v) | 1); // 3 > (2, ~), 3 < (3,~) となるようにする
        }
    }

    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の高さ</typeparam>
    /// <typeparam name="T">重み</typeparam>
    /// <typeparam name="TOp">重みの加算・減算オペレータ</typeparam>
    /// <typeparam name="ROp">重みの総和を求めるデータ構造</typeparam>
    public class WaveletMatrixRangeSum<F, T, TOp, ROp>
        where F : IComparable<F>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
        where ROp : struct, IWabeletSumOperator<T>
    {
        readonly WaveletMatrixRangeSumCompressed<T, TOp, ROp> mat;
        readonly F[] ys;

        /// <summary>
        /// <para>各要素の高さと重み <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(N) log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrixRangeSum(ReadOnlySpan<(F V, T d)> v)
        {
            var zahyoCompress = ZahyoCompress.Create(v.Select(t => t.V));
            var pos = zahyoCompress.NewTable;
            ys = zahyoCompress.Original;

            var compressed = v.Select(t => (pos[t.V], t.d)).ToArray();
            mat = new WaveletMatrixRangeSumCompressed<T, TOp, ROp>(compressed, ys.Length);
        }

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [<paramref name="lower"/>, <paramref name="upper"/>) である要素の重みの和を返す</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </remarks>
        [凾(256)]
        public T RectSum(int l, int r, F lower, F upper)
            => mat.RectSum(l, r, ys.LowerBound(lower), ys.LowerBound(upper));

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [0, <paramref name="upper"/>) である要素の重みの和を返す</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </remarks>
        [凾(256)]
        public T RectSum(int l, int r, F upper)
            => mat.RectSum(l, r, ys.LowerBound(upper));

        /// <summary>
        /// <para><paramref name="k"/> 番目の要素に <paramref name="x"/> を加算する。</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [凾(256)]
        public void PointAdd(int k, T x) => mat.PointAdd(k, x);
    }

    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="T">重み</typeparam>
    /// <typeparam name="TOp">重みの加算・減算オペレータ</typeparam>
    /// <typeparam name="ROp">重みの総和を求めるデータ構造</typeparam>
    public class WaveletMatrixRangeSumCompressed<T, TOp, ROp>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
        where ROp : struct, IWabeletSumOperator<T>
    {
        private static TOp op => default;
        private SuccinctIndexableDictionary[] matrix;
        private int[] v;
        private int[] mid;
        private ROp[] ds;
        private int Length { get; }

        /// <summary>
        /// <para>各要素の高さと重み <paramref name="v"/> を初期値として構築する。</para>
        /// <para>計算量: O(N log(N) log(V))</para>
        /// <para>  N は要素数。 V は最大値。</para>
        /// </summary>
        public WaveletMatrixRangeSumCompressed(ReadOnlySpan<(int V, T d)> v, int max)
        {
            this.v = v.Select(t => t.V);
            Length = v.Length;
            var l = new int[Length];
            var r = new int[Length];
            var ord = Enumerable.Range(0, Length).ToArray();
            var log = BitOperations.Log2((uint)max) + 1;
            matrix = new SuccinctIndexableDictionary[log];
            ds = new ROp[log];
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
                ds[level].Init(dd);
            }
        }

        [凾(256)]
        private (int, int) Succ(bool f, int l, int r, int level)
                     => (matrix[level].Rank(f, l) + (f ? mid[level] : 0), matrix[level].Rank(f, r) + (f ? mid[level] : 0));


        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [<paramref name="lower"/>, <paramref name="upper"/>) である要素の重みを返す</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </remarks>
        [凾(256)]
        public T RectSum(int l, int r, int lower, int upper)
            => op.Subtract(RectSum(l, r, upper), RectSum(l, r, lower));

        /// <summary>
        /// <para>区間 [<paramref name="l"/>, <paramref name="r"/>)、高さ [0, <paramref name="upper"/>) である要素の重みを返す</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log(N) log(V))</para>
        /// <para>  V は最大値。</para>
        /// </remarks>
        [凾(256)]
        public T RectSum(int l, int r, int upper)
        {
            T ret = default;
            for (int level = ds.Length - 1; level >= 0; level--)
            {
                if (((upper >> level) & 1) != 0)
                {
                    var (f, t) = Succ(false, l, r, level);
                    ret = op.Add(ret, ds[level].Sum(f, t));
                    l += mid[level] - f;
                    r += mid[level] - t;
                }
                else
                    (l, r) = Succ(false, l, r, level);
            }
            return ret;
        }

        /// <summary>
        /// <para><paramref name="k"/> 番目の要素に <paramref name="x"/> を加算する。</para>
        /// <para>計算量: O(log(V))</para>
        /// <para>  V は最大値。</para>
        /// </summary>
        [凾(256)]
        public void PointAdd(int k, T x)
        {
            var y = v[k];
            for (int level = ds.Length - 1; level >= 0; level--)
            {
                bool f = ((y >> level) & 1) != 0;
                k = matrix[level].Rank(f, k) + (f ? mid[level] : 0);
                ds[level].Add(k, x);
            }
        }
    }
}
