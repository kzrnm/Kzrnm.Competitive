using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix4x4<T>
        : IAdditionOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>
        , IAdditiveIdentity<Matrix4x4<T>, Matrix4x4<T>>
        , IMultiplicativeIdentity<Matrix4x4<T>, Matrix4x4<T>>
        , IMultiplyOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>
        , IMultiplyOperators<Matrix4x4<T>, T, Matrix4x4<T>>
        , ISubtractionOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>
        , IUnaryPlusOperators<Matrix4x4<T>, Matrix4x4<T>>
        , IUnaryNegationOperators<Matrix4x4<T>, Matrix4x4<T>>
        , IEquatable<Matrix4x4<T>>
        , IEqualityOperators<Matrix4x4<T>, Matrix4x4<T>, bool>
        where T : INumberBase<T>
    {
        public readonly (T Col0, T Col1, T Col2, T Col3) Row0;
        public readonly (T Col0, T Col1, T Col2, T Col3) Row1;
        public readonly (T Col0, T Col1, T Col2, T Col3) Row2;
        public readonly (T Col0, T Col1, T Col2, T Col3) Row3;
        public Matrix4x4((T Col0, T Col1, T Col2, T Col3) row0, (T Col0, T Col1, T Col2, T Col3) row1, (T Col0, T Col1, T Col2, T Col3) row2, (T Col0, T Col1, T Col2, T Col3) row3)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }
        public static readonly Matrix4x4<T> Identity = new Matrix4x4<T>(
            (T.MultiplicativeIdentity, default, default, default),
            (default, T.MultiplicativeIdentity, default, default),
            (default, default, T.MultiplicativeIdentity, default),
            (default, default, default, T.MultiplicativeIdentity));
        public static Matrix4x4<T> AdditiveIdentity => default;
        public static Matrix4x4<T> MultiplicativeIdentity => Identity;

        [凾(256)] public static Matrix4x4<T> operator +(Matrix4x4<T> x) => x;
        [凾(256)]
        public static Matrix4x4<T> operator -(Matrix4x4<T> x)
            => new Matrix4x4<T>(
                (-x.Row0.Col0, -x.Row0.Col1, -x.Row0.Col2, -x.Row0.Col3),
                (-x.Row1.Col0, -x.Row1.Col1, -x.Row1.Col2, -x.Row1.Col3),
                (-x.Row2.Col0, -x.Row2.Col1, -x.Row2.Col2, -x.Row2.Col3),
                (-x.Row3.Col0, -x.Row3.Col1, -x.Row3.Col2, -x.Row3.Col3));
        [凾(256)]
        public static Matrix4x4<T> operator +(Matrix4x4<T> x, Matrix4x4<T> y)
            => new Matrix4x4<T>(
                (x.Row0.Col0 + y.Row0.Col0, x.Row0.Col1 + y.Row0.Col1, x.Row0.Col2 + y.Row0.Col2, x.Row0.Col3 + y.Row0.Col3),
                (x.Row1.Col0 + y.Row1.Col0, x.Row1.Col1 + y.Row1.Col1, x.Row1.Col2 + y.Row1.Col2, x.Row1.Col3 + y.Row1.Col3),
                (x.Row2.Col0 + y.Row2.Col0, x.Row2.Col1 + y.Row2.Col1, x.Row2.Col2 + y.Row2.Col2, x.Row2.Col3 + y.Row2.Col3),
                (x.Row3.Col0 + y.Row3.Col0, x.Row3.Col1 + y.Row3.Col1, x.Row3.Col2 + y.Row3.Col2, x.Row3.Col3 + y.Row3.Col3));
        [凾(256)]
        public static Matrix4x4<T> operator -(Matrix4x4<T> x, Matrix4x4<T> y)
            => new Matrix4x4<T>(
                (x.Row0.Col0 - y.Row0.Col0, x.Row0.Col1 - y.Row0.Col1, x.Row0.Col2 - y.Row0.Col2, x.Row0.Col3 - y.Row0.Col3),
                (x.Row1.Col0 - y.Row1.Col0, x.Row1.Col1 - y.Row1.Col1, x.Row1.Col2 - y.Row1.Col2, x.Row1.Col3 - y.Row1.Col3),
                (x.Row2.Col0 - y.Row2.Col0, x.Row2.Col1 - y.Row2.Col1, x.Row2.Col2 - y.Row2.Col2, x.Row2.Col3 - y.Row2.Col3),
                (x.Row3.Col0 - y.Row3.Col0, x.Row3.Col1 - y.Row3.Col1, x.Row3.Col2 - y.Row3.Col2, x.Row3.Col3 - y.Row3.Col3));
        [凾(256)]
        public static Matrix4x4<T> operator *(Matrix4x4<T> x, Matrix4x4<T> y)
            => new Matrix4x4<T>(
                (
                    x.Row0.Col0 * y.Row0.Col0 + x.Row0.Col1 * y.Row1.Col0 + x.Row0.Col2 * y.Row2.Col0 + x.Row0.Col3 * y.Row3.Col0,
                    x.Row0.Col0 * y.Row0.Col1 + x.Row0.Col1 * y.Row1.Col1 + x.Row0.Col2 * y.Row2.Col1 + x.Row0.Col3 * y.Row3.Col1,
                    x.Row0.Col0 * y.Row0.Col2 + x.Row0.Col1 * y.Row1.Col2 + x.Row0.Col2 * y.Row2.Col2 + x.Row0.Col3 * y.Row3.Col2,
                    x.Row0.Col0 * y.Row0.Col3 + x.Row0.Col1 * y.Row1.Col3 + x.Row0.Col2 * y.Row2.Col3 + x.Row0.Col3 * y.Row3.Col3
                ),
                (
                    x.Row1.Col0 * y.Row0.Col0 + x.Row1.Col1 * y.Row1.Col0 + x.Row1.Col2 * y.Row2.Col0 + x.Row1.Col3 * y.Row3.Col0,
                    x.Row1.Col0 * y.Row0.Col1 + x.Row1.Col1 * y.Row1.Col1 + x.Row1.Col2 * y.Row2.Col1 + x.Row1.Col3 * y.Row3.Col1,
                    x.Row1.Col0 * y.Row0.Col2 + x.Row1.Col1 * y.Row1.Col2 + x.Row1.Col2 * y.Row2.Col2 + x.Row1.Col3 * y.Row3.Col2,
                    x.Row1.Col0 * y.Row0.Col3 + x.Row1.Col1 * y.Row1.Col3 + x.Row1.Col2 * y.Row2.Col3 + x.Row1.Col3 * y.Row3.Col3
                ),
                (
                    x.Row2.Col0 * y.Row0.Col0 + x.Row2.Col1 * y.Row1.Col0 + x.Row2.Col2 * y.Row2.Col0 + x.Row2.Col3 * y.Row3.Col0,
                    x.Row2.Col0 * y.Row0.Col1 + x.Row2.Col1 * y.Row1.Col1 + x.Row2.Col2 * y.Row2.Col1 + x.Row2.Col3 * y.Row3.Col1,
                    x.Row2.Col0 * y.Row0.Col2 + x.Row2.Col1 * y.Row1.Col2 + x.Row2.Col2 * y.Row2.Col2 + x.Row2.Col3 * y.Row3.Col2,
                    x.Row2.Col0 * y.Row0.Col3 + x.Row2.Col1 * y.Row1.Col3 + x.Row2.Col2 * y.Row2.Col3 + x.Row2.Col3 * y.Row3.Col3
                ),
                (
                    x.Row3.Col0 * y.Row0.Col0 + x.Row3.Col1 * y.Row1.Col0 + x.Row3.Col2 * y.Row2.Col0 + x.Row3.Col3 * y.Row3.Col0,
                    x.Row3.Col0 * y.Row0.Col1 + x.Row3.Col1 * y.Row1.Col1 + x.Row3.Col2 * y.Row2.Col1 + x.Row3.Col3 * y.Row3.Col1,
                    x.Row3.Col0 * y.Row0.Col2 + x.Row3.Col1 * y.Row1.Col2 + x.Row3.Col2 * y.Row2.Col2 + x.Row3.Col3 * y.Row3.Col2,
                    x.Row3.Col0 * y.Row0.Col3 + x.Row3.Col1 * y.Row1.Col3 + x.Row3.Col2 * y.Row2.Col3 + x.Row3.Col3 * y.Row3.Col3
                )
            );
        [凾(256)]
        public static Matrix4x4<T> operator *(Matrix4x4<T> m, T x)
             => new Matrix4x4<T>(
                 (x * m.Row0.Col0, x * m.Row0.Col1, x * m.Row0.Col2, x * m.Row0.Col3),
                 (x * m.Row1.Col0, x * m.Row1.Col1, x * m.Row1.Col2, x * m.Row1.Col3),
                 (x * m.Row2.Col0, x * m.Row2.Col1, x * m.Row2.Col2, x * m.Row2.Col3),
                 (x * m.Row3.Col0, x * m.Row3.Col1, x * m.Row3.Col2, x * m.Row3.Col3)
             );

        /// <summary>
        /// 4次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public static (T v0, T v1, T v2, T v3) operator *(Matrix4x4<T> mat, (T v0, T v1, T v2, T v3) vector) => mat.Multiply(vector);

        /// <summary>
        /// 4次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T v0, T v1, T v2, T v3) Multiply((T v0, T v1, T v2, T v3) vector) => Multiply(vector.v0, vector.v1, vector.v2, vector.v3);

        /// <summary>
        /// 4次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T v0, T v1, T v2, T v3) Multiply(T v0, T v1, T v2, T v3)
                => (
                        Row0.Col0 * v0 + Row0.Col1 * v1 + Row0.Col2 * v2 + Row0.Col3 * v3,
                        Row1.Col0 * v0 + Row1.Col1 * v1 + Row1.Col2 * v2 + Row1.Col3 * v3,
                        Row2.Col0 * v0 + Row2.Col1 * v1 + Row2.Col2 * v2 + Row2.Col3 * v3,
                        Row3.Col0 * v0 + Row3.Col1 * v1 + Row3.Col2 * v2 + Row3.Col3 * v3
                   );

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            var r0c0 = (
             Row1.Col1 * Row2.Col2 * Row3.Col3
             + Row2.Col1 * Row1.Col3 * Row3.Col2
             + Row3.Col1 * Row1.Col2 * Row2.Col3)
             -
             (Row1.Col1 * Row2.Col3 * Row3.Col2
             + Row2.Col1 * Row1.Col2 * Row3.Col3
             + Row3.Col1 * Row1.Col3 * Row2.Col2);
            var r1c0 = (
             Row1.Col0 * Row2.Col3 * Row3.Col2
             + Row2.Col0 * Row1.Col2 * Row3.Col3
             + Row3.Col0 * Row1.Col3 * Row2.Col2)
             -
             (Row1.Col0 * Row2.Col2 * Row3.Col3
             + Row2.Col0 * Row1.Col3 * Row3.Col2
             + Row3.Col0 * Row1.Col2 * Row2.Col3);
            var r2c0 = (
             Row1.Col0 * Row2.Col1 * Row3.Col3
             + Row2.Col0 * Row1.Col3 * Row3.Col1
             + Row3.Col0 * Row1.Col1 * Row2.Col3)
             -
             (Row1.Col0 * Row2.Col3 * Row3.Col1
             + Row2.Col0 * Row1.Col1 * Row3.Col3
             + Row3.Col0 * Row1.Col3 * Row2.Col1);
            var r3c0 = (
             Row1.Col0 * Row2.Col2 * Row3.Col1
             + Row2.Col0 * Row1.Col1 * Row3.Col2
             + Row3.Col0 * Row1.Col2 * Row2.Col1)
             -
             (Row1.Col0 * Row2.Col1 * Row3.Col2
             + Row2.Col0 * Row1.Col2 * Row3.Col1
             + Row3.Col0 * Row1.Col1 * Row2.Col2);
            return Row0.Col0 * r0c0 + Row0.Col1 * r1c0 + Row0.Col2 * r2c0 + Row0.Col3 * r3c0;
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix4x4<T> Inv()
        {
            var r0c0 = (
             Row1.Col1 * Row2.Col2 * Row3.Col3
             + Row2.Col1 * Row1.Col3 * Row3.Col2
             + Row3.Col1 * Row1.Col2 * Row2.Col3)
             -
             (Row1.Col1 * Row2.Col3 * Row3.Col2
             + Row2.Col1 * Row1.Col2 * Row3.Col3
             + Row3.Col1 * Row1.Col3 * Row2.Col2);
            var r1c0 = (
             Row1.Col0 * Row2.Col3 * Row3.Col2
             + Row2.Col0 * Row1.Col2 * Row3.Col3
             + Row3.Col0 * Row1.Col3 * Row2.Col2)
             -
             (Row1.Col0 * Row2.Col2 * Row3.Col3
             + Row2.Col0 * Row1.Col3 * Row3.Col2
             + Row3.Col0 * Row1.Col2 * Row2.Col3);
            var r2c0 = (
             Row1.Col0 * Row2.Col1 * Row3.Col3
             + Row2.Col0 * Row1.Col3 * Row3.Col1
             + Row3.Col0 * Row1.Col1 * Row2.Col3)
             -
             (Row1.Col0 * Row2.Col3 * Row3.Col1
             + Row2.Col0 * Row1.Col1 * Row3.Col3
             + Row3.Col0 * Row1.Col3 * Row2.Col1);
            var r3c0 = (
             Row1.Col0 * Row2.Col2 * Row3.Col1
             + Row2.Col0 * Row1.Col1 * Row3.Col2
             + Row3.Col0 * Row1.Col2 * Row2.Col1)
             -
             (Row1.Col0 * Row2.Col1 * Row3.Col2
             + Row2.Col0 * Row1.Col2 * Row3.Col1
             + Row3.Col0 * Row1.Col1 * Row2.Col2);
            var r0c1 = (
             Row0.Col1 * Row2.Col3 * Row3.Col2
             + Row2.Col1 * Row0.Col2 * Row3.Col3
             + Row3.Col1 * Row0.Col3 * Row2.Col2)
             -
             (Row0.Col1 * Row2.Col2 * Row3.Col3
             + Row2.Col1 * Row0.Col3 * Row3.Col2
             + Row3.Col1 * Row0.Col2 * Row2.Col3);
            var r1c1 = (
             Row0.Col0 * Row2.Col2 * Row3.Col3
             + Row2.Col0 * Row0.Col3 * Row3.Col2
             + Row3.Col0 * Row0.Col2 * Row2.Col3)
             -
             (Row0.Col0 * Row2.Col3 * Row3.Col2
             + Row2.Col0 * Row0.Col2 * Row3.Col3
             + Row3.Col0 * Row0.Col3 * Row2.Col2);
            var r2c1 = (
             Row0.Col0 * Row2.Col3 * Row3.Col1
             + Row2.Col0 * Row0.Col1 * Row3.Col3
             + Row3.Col0 * Row0.Col3 * Row2.Col1)
             -
             (Row0.Col0 * Row2.Col1 * Row3.Col3
             + Row2.Col0 * Row0.Col3 * Row3.Col1
             + Row3.Col0 * Row0.Col1 * Row2.Col3);
            var r3c1 = (
             Row0.Col0 * Row2.Col1 * Row3.Col2
             + Row2.Col0 * Row0.Col2 * Row3.Col1
             + Row3.Col0 * Row0.Col1 * Row2.Col2)
             -
             (Row0.Col0 * Row2.Col2 * Row3.Col1
             + Row2.Col0 * Row0.Col1 * Row3.Col2
             + Row3.Col0 * Row0.Col2 * Row2.Col1);
            var r0c2 = (
             Row0.Col1 * Row1.Col2 * Row3.Col3
             + Row1.Col1 * Row0.Col3 * Row3.Col2
             + Row3.Col1 * Row0.Col2 * Row1.Col3)
             -
             (Row0.Col1 * Row1.Col3 * Row3.Col2
             + Row1.Col1 * Row0.Col2 * Row3.Col3
             + Row3.Col1 * Row0.Col3 * Row1.Col2);
            var r1c2 = (
             Row0.Col0 * Row1.Col3 * Row3.Col2
             + Row1.Col0 * Row0.Col2 * Row3.Col3
             + Row3.Col0 * Row0.Col3 * Row1.Col2)
             -
             (Row0.Col0 * Row1.Col2 * Row3.Col3
             + Row1.Col0 * Row0.Col3 * Row3.Col2
             + Row3.Col0 * Row0.Col2 * Row1.Col3);
            var r2c2 = (
             Row0.Col0 * Row1.Col1 * Row3.Col3
             + Row1.Col0 * Row0.Col3 * Row3.Col1
             + Row3.Col0 * Row0.Col1 * Row1.Col3)
             -
             (Row0.Col0 * Row1.Col3 * Row3.Col1
             + Row1.Col0 * Row0.Col1 * Row3.Col3
             + Row3.Col0 * Row0.Col3 * Row1.Col1);
            var r3c2 = (
             Row0.Col0 * Row1.Col2 * Row3.Col1
             + Row1.Col0 * Row0.Col1 * Row3.Col2
             + Row3.Col0 * Row0.Col2 * Row1.Col1)
             -
             (Row0.Col0 * Row1.Col1 * Row3.Col2
             + Row1.Col0 * Row0.Col2 * Row3.Col1
             + Row3.Col0 * Row0.Col1 * Row1.Col2);
            var r0c3 = (
             Row0.Col1 * Row1.Col3 * Row2.Col2
             + Row1.Col1 * Row0.Col2 * Row2.Col3
             + Row2.Col1 * Row0.Col3 * Row1.Col2)
             -
             (Row0.Col1 * Row1.Col2 * Row2.Col3
             + Row1.Col1 * Row0.Col3 * Row2.Col2
             + Row2.Col1 * Row0.Col2 * Row1.Col3);
            var r1c3 = (
             Row0.Col0 * Row1.Col2 * Row2.Col3
             + Row1.Col0 * Row0.Col3 * Row2.Col2
             + Row2.Col0 * Row0.Col2 * Row1.Col3)
             -
             (Row0.Col0 * Row1.Col3 * Row2.Col2
             + Row1.Col0 * Row0.Col2 * Row2.Col3
             + Row2.Col0 * Row0.Col3 * Row1.Col2);
            var r2c3 = (
             Row0.Col0 * Row1.Col3 * Row2.Col1
             + Row1.Col0 * Row0.Col1 * Row2.Col3
             + Row2.Col0 * Row0.Col3 * Row1.Col1)
             -
             (Row0.Col0 * Row1.Col1 * Row2.Col3
             + Row1.Col0 * Row0.Col3 * Row2.Col1
             + Row2.Col0 * Row0.Col1 * Row1.Col3);
            var r3c3 = (
             Row0.Col0 * Row1.Col1 * Row2.Col2
             + Row1.Col0 * Row0.Col2 * Row2.Col1
             + Row2.Col0 * Row0.Col1 * Row1.Col2)
             -
             (Row0.Col0 * Row1.Col2 * Row2.Col1
             + Row1.Col0 * Row0.Col1 * Row2.Col2
             + Row2.Col0 * Row0.Col2 * Row1.Col1);
            var det = Row0.Col0 * r0c0 + Row0.Col1 * r1c0 + Row0.Col2 * r2c0 + Row0.Col3 * r3c0;
            var detinv = T.MultiplicativeIdentity / det;
            return new Matrix4x4<T>(
                (detinv * r0c0, detinv * r0c1, detinv * r0c2, detinv * r0c3),
                (detinv * r1c0, detinv * r1c1, detinv * r1c2, detinv * r1c3),
                (detinv * r2c0, detinv * r2c1, detinv * r2c2, detinv * r2c3),
                (detinv * r3c0, detinv * r3c1, detinv * r3c2, detinv * r3c3)
            );
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is Matrix4x4<T> x && Equals(x);
        [凾(256)]
        public bool Equals(Matrix4x4<T> other)
            => Row0.Equals(other.Row0)
            && Row1.Equals(other.Row1)
            && Row2.Equals(other.Row2)
            && Row3.Equals(other.Row3);
        [凾(256)]
        public override int GetHashCode() => HashCode.Combine(Row0, Row1, Row2, Row3);
        [凾(256)]
        public static bool operator ==(Matrix4x4<T> left, Matrix4x4<T> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(Matrix4x4<T> left, Matrix4x4<T> right) => !(left == right);
    }
}
