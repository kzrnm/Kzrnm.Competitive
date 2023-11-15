using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix2x2<T> : IMatrix<Matrix2x2<T>, T>
        , IMultiplyOperators<Matrix2x2<T>, T, Matrix2x2<T>>
        where T : INumberBase<T>
    {
        public int Height => 2;
        public int Width => 2;
        public (T Col0, T Col1) Row0 => (V00, V01);
        public (T Col0, T Col1) Row1 => (V10, V11);

        internal readonly T
            V00, V01,
            V10, V11;
        [凾(256)] public ReadOnlySpan<T> AsSpan() => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Matrix2x2<T>, T>(ref Unsafe.AsRef(this)), 2 * 2);
        public T[][] ToArray() => new[]
        {
            new[]{ V00, V01 },
            new[]{ V10, V11 },
        };
        [凾(256)]
        public Matrix2x2((T Col0, T Col1) row0, (T Col0, T Col1) row1)
        {
            (V00, V01) = row0;
            (V10, V11) = row1;
        }
        public static Matrix2x2<T> AdditiveIdentity => default;
        public static Matrix2x2<T> MultiplicativeIdentity => new(
            (T.MultiplicativeIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.MultiplicativeIdentity));

        [凾(256)] public static Matrix2x2<T> operator +(Matrix2x2<T> x) => x;
        [凾(256)]
        public static Matrix2x2<T> operator -(Matrix2x2<T> x)
            => new(
                (-x.V00, -x.V01),
                (-x.V10, -x.V11));
        [凾(256)]
        public static Matrix2x2<T> operator +(Matrix2x2<T> x, Matrix2x2<T> y)
            => new(
                (x.V00 + y.V00, x.V01 + y.V01),
                (x.V10 + y.V10, x.V11 + y.V11));
        [凾(256)]
        public static Matrix2x2<T> operator -(Matrix2x2<T> x, Matrix2x2<T> y)
            => new(
                (x.V00 - y.V00, x.V01 - y.V01),
                (x.V10 - y.V10, x.V11 - y.V11));
        [凾(256)]
        public static Matrix2x2<T> operator *(Matrix2x2<T> x, Matrix2x2<T> y)
            => new(
                (
                    x.V00 * y.V00 + x.V01 * y.V10,
                    x.V00 * y.V01 + x.V01 * y.V11),
                (
                    x.V10 * y.V00 + x.V11 * y.V10,
                    x.V10 * y.V01 + x.V11 * y.V11));
        [凾(256)]
        public static Matrix2x2<T> operator *(Matrix2x2<T> m, T x)
            => new(
                (x * m.V00, x * m.V01),
                (x * m.V10, x * m.V11)
            );

        /// <summary>
        /// 2次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public static (T v0, T v1) operator *(Matrix2x2<T> mat, (T v0, T v1) vector) => mat.Multiply(vector);


        /// <summary>
        /// 2次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T v0, T v1) Multiply((T v0, T v1) vector) => Multiply(vector.v0, vector.v1);

        /// <summary>
        /// 2次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T v0, T v1) Multiply(T v0, T v1)
                => (
                        (V00 * v0) + (V01 * v1),
                        (V10 * v0) + (V11 * v1)
                   );

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            return (V00 * V11) - (V01 * V10);
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix2x2<T> Inv()
        {
            var det = Determinant();
            var detinv = T.MultiplicativeIdentity / det;
            return new(
                (detinv * V11, detinv * -V01),
                (detinv * -V10, detinv * V00)
            );
        }

        [凾(256)]
        public override int GetHashCode() => HashCode.Combine(V00, V10, V01, V11);

        [凾(256)]
        public override bool Equals(object obj) => obj is Matrix2x2<T> x && Equals(x);
        [凾(256)]
        public bool Equals(Matrix2x2<T> other) =>
                   EqualityComparer<T>.Default.Equals(V00, other.V00) &&
                   EqualityComparer<T>.Default.Equals(V10, other.V10) &&
                   EqualityComparer<T>.Default.Equals(V01, other.V01) &&
                   EqualityComparer<T>.Default.Equals(V11, other.V11);
        [凾(256)]
        public static bool operator ==(Matrix2x2<T> left, Matrix2x2<T> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(Matrix2x2<T> left, Matrix2x2<T> right) => !(left == right);
    }
}
