using AtCoder.Operators;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// アフィン変換. Ax+b の形で表される式を保持する。
    /// </summary>
    public readonly struct AffineTransformation<T, TOp> : IEquatable<AffineTransformation<T, TOp>> where TOp : struct, IAdditionOperator<T>, IMultiplicationOperator<T>, IUnaryNumOperator<T>
    {
        private static TOp op => new TOp();
        /// <summary>
        /// 一次の係数
        /// </summary>
        private readonly T a;
        /// <summary>
        /// 定数項
        /// </summary>
        private readonly T b;

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
            => op.Add(op.Multiply(a, x), b);

        /// <summary>
        /// <paramref name="g"/>(<paramref name="f"/>(x))
        /// </summary>
        [凾(256)]
        public static AffineTransformation<T, TOp> operator *(AffineTransformation<T, TOp> f, AffineTransformation<T, TOp> g)
            => new AffineTransformation<T, TOp>(op.Multiply(f.a, g.a), op.Add(op.Multiply(g.a, f.b), g.b));

        [凾(256)]
        public static bool operator ==(AffineTransformation<T, TOp> left, AffineTransformation<T, TOp> right)
            => left.Equals(right);
        [凾(256)]
        public static bool operator !=(AffineTransformation<T, TOp> left, AffineTransformation<T, TOp> right)
            => !left.Equals(right);
        public override bool Equals(object obj) => obj is AffineTransformation<T, TOp> transformation && Equals(transformation);

        [凾(256)]
        public bool Equals(AffineTransformation<T, TOp> other)
            => EqualityComparer<T>.Default.Equals(a, other.a) && EqualityComparer<T>.Default.Equals(b, other.b);
        public override int GetHashCode() => HashCode.Combine(a, b);

        public struct Operator : IMultiplicationOperator<AffineTransformation<T, TOp>>
        {
            public AffineTransformation<T, TOp> MultiplyIdentity => new AffineTransformation<T, TOp>(op.MultiplyIdentity, default);

            [凾(256)]
            public AffineTransformation<T, TOp> Multiply(AffineTransformation<T, TOp> x, AffineTransformation<T, TOp> y)
                => x * y;
        }
    }
}
