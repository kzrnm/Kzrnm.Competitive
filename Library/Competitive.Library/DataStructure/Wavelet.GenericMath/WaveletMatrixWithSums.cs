// Original: https://ei1333.github.io/library/structure/wavelet/wavelet-matrix.cpp.html
using System.Numerics;
using Kzrnm.Competitive.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の高さ</typeparam>
    /// <typeparam name="T">重み</typeparam>
    public class WaveletMatrix2DWithSums<F, T> : WaveletMatrix2D<F, T, WaveletSumOp<T>>
        where F : IComparable<F>
        where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        public WaveletMatrix2DWithSums(ReadOnlySpan<((F x, F y) V, T d)> v) : base(v) { }
    }

    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の高さ</typeparam>
    /// <typeparam name="T">重み</typeparam>
    public class WaveletMatrixWithSums<F, T> : WaveletMatrixRangeSum<F, T, WaveletSumOp<T>>
        where F : IComparable<F>
        where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        public WaveletMatrixWithSums(ReadOnlySpan<(F V, T d)> v) : base(v) { }
    }
    namespace Internal
    {
        public struct WaveletSumOp<T> : IWabeletSumOperator<T>
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            public Sums<T> s;
            [凾(256)] public void Init(T[] ts) => s = new Sums<T>(ts);
            [凾(256)] public void Add(int p, T v) => throw new NotSupportedException();
            [凾(256)] public T Sum(int l, int r) => s[l, r];
        }
    }
}
