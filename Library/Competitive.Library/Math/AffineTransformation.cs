using System;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// アフィン変換. Ax+b の形で表される式を保持する。
    /// </summary>
    [DebuggerDisplay("{" + nameof(a) + "} x + {" + nameof(b) + "}")]
    public readonly record struct AffineTransformation<T> : IEquatable<AffineTransformation<T>>
        where T : struct
        , IAdditionOperators<T, T, T>
        , IMultiplyOperators<T, T, T>
        , IUnaryNegationOperators<T, T>
        , IAdditiveIdentity<T, T>
        , IMultiplicativeIdentity<T, T>
    {
        /// <summary>
        /// 一次の係数
        /// </summary>
        public readonly T a;
        /// <summary>
        /// 定数項
        /// </summary>
        public readonly T b;
        public static AffineTransformation<T> Identity => new(T.MultiplicativeIdentity, T.AdditiveIdentity);

        public AffineTransformation(T a, T b)
        {
            this.a = a;
            this.b = b;
        }

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
