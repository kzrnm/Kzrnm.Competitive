using System;
using System.Numerics;

namespace Kzrnm.Competitive.Internal
{
    internal interface IMatrixOperator<T>
        : IAdditionOperators<T, T, T>
        , IAdditiveIdentity<T, T>
        , IMultiplicativeIdentity<T, T>
        , IMultiplyOperators<T, T, T>
        , ISubtractionOperators<T, T, T>
        , IUnaryPlusOperators<T, T>
        , IUnaryNegationOperators<T, T>
        , IEquatable<T>
        , IEqualityOperators<T, T, bool>
        where T : IMatrixOperator<T>
    { }
}
