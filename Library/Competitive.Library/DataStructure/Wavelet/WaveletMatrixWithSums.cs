// Original: https://ei1333.github.io/library/structure/wavelet/wavelet-matrix.cpp.html
using AtCoder.Operators;
using Kzrnm.Competitive.InternalWavelet;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の高さ</typeparam>
    /// <typeparam name="T">重み</typeparam>
    /// <typeparam name="TOp">重みの加算・減算オペレータ</typeparam>
    public class WaveletMatrix2DWithSums<F, T, TOp> : WaveletMatrix2D<F, T, TOp, SumOp<T, TOp>>
        where F : IComparable<F>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
    {
        public WaveletMatrix2DWithSums(ReadOnlySpan<((F x, F y) V, T d)> v) : base(v) { }
    }

    /// <summary>
    /// 2 次元平面上にある点が事前に与えられているとき、点への重みの加算と範囲内の重みの総和を求めるデータ構造。
    /// </summary>
    /// <typeparam name="F">点の高さ</typeparam>
    /// <typeparam name="T">重み</typeparam>
    /// <typeparam name="TOp">重みの加算・減算オペレータ</typeparam>
    public class WaveletMatrixWithSums<F, T, TOp> : WaveletMatrixRangeSum<F, T, TOp, SumOp<T, TOp>>
        where F : IComparable<F>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
    {
        public WaveletMatrixWithSums(ReadOnlySpan<(F V, T d)> v) : base(v) { }
    }
    namespace InternalWavelet
    {
        public struct SumOp<T, TOp> : IWabeletSumOperator<T>
            where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
        {
            public Sums<T, TOp> s;
            [凾(256)] public void Init(T[] ts) => s = new Sums<T, TOp>(ts);
            [凾(256)] public void Add(int p, T v) => throw new NotSupportedException();
            [凾(256)] public T Sum(int l, int r) => s[l, r];
        }
    }
}
