// Original: https://ei1333.github.io/library/structure/wavelet/wavelet-matrix.cpp.html
using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の高さ</typeparam>
    /// <typeparam name="T">重み</typeparam>
    public class WaveletMatrix2DWithFenwickTree<F, T> : WaveletMatrix2D<F, T, WaveletFwOp<T>>
        where F : IComparable<F>
        where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        public WaveletMatrix2DWithFenwickTree(ReadOnlySpan<((F x, F y) V, T d)> v) : base(v) { }
    }

    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の高さ</typeparam>
    /// <typeparam name="T">重み</typeparam>
    public class WaveletMatrixWithFenwickTree<F, T> : WaveletMatrixRangeSum<F, T, WaveletFwOp<T>>
        where F : IComparable<F>
        where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        public WaveletMatrixWithFenwickTree(ReadOnlySpan<(F V, T d)> v) : base(v) { }
    }
    namespace Internal
    {
        public readonly struct WaveletFwOp<T> : IWabeletSumOperator<T, WaveletFwOp<T>>
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            readonly FenwickTree<T> f;
            public WaveletFwOp(FenwickTree<T> fw)
            {
                f = fw;
            }

            [凾(256)]
            public static WaveletFwOp<T> Init(T[] ts)
            {
                var f = new FenwickTree<T>(ts.Length);
                for (int i = 0; i < ts.Length; i++)
                    f.Add(i, ts[i]);
                return new(f);
            }

            [凾(256)] public void Add(int p, T v) => f.Add(p, v);
            [凾(256)] public T Sum(int l, int r) => f.Sum(l, r);
        }
    }
}
