using System;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// アフィン変換. Ax+b の形で表される式を保持する。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a">一次の係数</param>
    /// <param name="b">定数項</param>
    [DebuggerDisplay("{" + nameof(a) + "} x + {" + nameof(b) + "}")]
    public readonly record struct AffineTransformation<T>(T a, T b)
        where T : struct
        , IAdditionOperators<T, T, T>
        , IMultiplyOperators<T, T, T>
        , IUnaryNegationOperators<T, T>
        , IAdditiveIdentity<T, T>
        , IMultiplicativeIdentity<T, T>
    {
        public static AffineTransformation<T> Identity => new(T.MultiplicativeIdentity, T.AdditiveIdentity);

        /// <summary>
        /// A <paramref name="x"/> + b を求める。
        /// </summary>
        [凾(256)]
        public T Apply(T x) => a * x + b;

        /// <summary>
        /// A * <paramref name="other"/> + b を求める。
        /// </summary>
        [凾(256)]
        public AffineTransformation<T> Apply(AffineTransformation<T> other) => new(a * other.a, a * other.b + b);
    }
}
