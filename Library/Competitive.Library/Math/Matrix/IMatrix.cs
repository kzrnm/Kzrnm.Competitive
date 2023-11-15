using System;
using System.Numerics;

namespace Kzrnm.Competitive.Internal
{
    public interface IMatrix<T>
        : IAdditionOperators<T, T, T>
        , IAdditiveIdentity<T, T>
        , IMultiplicativeIdentity<T, T>
        , IMultiplyOperators<T, T, T>
        , ISubtractionOperators<T, T, T>
        , IUnaryPlusOperators<T, T>
        , IUnaryNegationOperators<T, T>
        , IEquatable<T>
        , IEqualityOperators<T, T, bool>
        where T : IMatrix<T>
    {
        int Height { get; }
        int Width { get; }
    }
    public interface IMatrixGet<T>
    {
        ReadOnlySpan<T> AsSpan();
        T[][] ToArray();
    }
    public interface IMatrix<TSelf, T> : IMatrix<TSelf>, IMatrixGet<T>
        where TSelf : IMatrix<TSelf>
    {
    }
}
