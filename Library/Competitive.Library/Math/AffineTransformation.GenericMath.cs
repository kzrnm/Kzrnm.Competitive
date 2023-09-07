using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    internal interface IAffineOperator<T> : IEquatable<T>
        , IMultiplyOperators<T, T, T>
        , IMultiplicativeIdentity<T, T>
        where T : IAffineOperator<T>
    { }
    /// <summary>
    /// アフィン変換. Ax+b の形で表される式を保持する。
    /// </summary>
    [DebuggerDisplay(nameof(a) + " * x + " + nameof(b))]
    public readonly struct AffineTransformation<T> : IAffineOperator<AffineTransformation<T>>
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
        private readonly T a;
        /// <summary>
        /// 定数項
        /// </summary>
        private readonly T b;

        public static AffineTransformation<T> MultiplicativeIdentity => new AffineTransformation<T>(T.MultiplicativeIdentity, T.AdditiveIdentity);

        public AffineTransformation(T a, T b)
        {
            this.a = a;
            this.b = b;
        }

        /// <summary>
        /// A <paramref name="x"/> + b を求める。
        /// </summary>
        [凾(256)]
        public T Apply(T x)
            => a * x + b;

        /// <summary>
        /// <paramref name="g"/>(<paramref name="f"/>(x))
        /// </summary>
        [凾(256)]
        public static AffineTransformation<T> operator *(AffineTransformation<T> f, AffineTransformation<T> g)
            => new AffineTransformation<T>(f.a * g.a, g.a * f.b + g.b);

        [凾(256)]
        public static bool operator ==(AffineTransformation<T> left, AffineTransformation<T> right)
            => left.Equals(right);
        [凾(256)]
        public static bool operator !=(AffineTransformation<T> left, AffineTransformation<T> right)
            => !left.Equals(right);
        public override bool Equals(object obj) => obj is AffineTransformation<T> transformation && Equals(transformation);

        [凾(256)]
        public bool Equals(AffineTransformation<T> other)
            => EqualityComparer<T>.Default.Equals(a, other.a) && EqualityComparer<T>.Default.Equals(b, other.b);
        public override int GetHashCode() => HashCode.Combine(a, b);
    }
}
