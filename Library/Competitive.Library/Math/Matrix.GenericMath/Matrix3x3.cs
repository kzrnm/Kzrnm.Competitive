using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix3x3<T>
        : IAdditionOperators<Matrix3x3<T>, Matrix3x3<T>, Matrix3x3<T>>
        , IAdditiveIdentity<Matrix3x3<T>, Matrix3x3<T>>
        , IMultiplicativeIdentity<Matrix3x3<T>, Matrix3x3<T>>
        , IMultiplyOperators<Matrix3x3<T>, Matrix3x3<T>, Matrix3x3<T>>
        , IMultiplyOperators<Matrix3x3<T>, T, Matrix3x3<T>>
        , ISubtractionOperators<Matrix3x3<T>, Matrix3x3<T>, Matrix3x3<T>>
        , IUnaryPlusOperators<Matrix3x3<T>, Matrix3x3<T>>
        , IUnaryNegationOperators<Matrix3x3<T>, Matrix3x3<T>>
        , IEquatable<Matrix3x3<T>>
        , IEqualityOperators<Matrix3x3<T>, Matrix3x3<T>, bool>
        where T : INumberBase<T>
    {
        public readonly (T Col0, T Col1, T Col2) Row0;
        public readonly (T Col0, T Col1, T Col2) Row1;
        public readonly (T Col0, T Col1, T Col2) Row2;
        public Matrix3x3((T Col0, T Col1, T Col2) row0, (T Col0, T Col1, T Col2) row1, (T Col0, T Col1, T Col2) row2)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }
        public static readonly Matrix3x3<T> Identity = new Matrix3x3<T>(
            (T.MultiplicativeIdentity, default, default),
            (default, T.MultiplicativeIdentity, default),
            (default, default, T.MultiplicativeIdentity));
        public static Matrix3x3<T> AdditiveIdentity => default;
        public static Matrix3x3<T> MultiplicativeIdentity => Identity;

        [凾(256)] public static Matrix3x3<T> operator +(Matrix3x3<T> x) => x;
        [凾(256)]
        public static Matrix3x3<T> operator -(Matrix3x3<T> x)
            => new Matrix3x3<T>(
                (-x.Row0.Col0, -x.Row0.Col1, -x.Row0.Col2),
                (-x.Row1.Col0, -x.Row1.Col1, -x.Row1.Col2),
                (-x.Row2.Col0, -x.Row2.Col1, -x.Row2.Col2));
        [凾(256)]
        public static Matrix3x3<T> operator +(Matrix3x3<T> x, Matrix3x3<T> y)
            => new Matrix3x3<T>(
                (x.Row0.Col0 + y.Row0.Col0, x.Row0.Col1 + y.Row0.Col1, x.Row0.Col2 + y.Row0.Col2),
                (x.Row1.Col0 + y.Row1.Col0, x.Row1.Col1 + y.Row1.Col1, x.Row1.Col2 + y.Row1.Col2),
                (x.Row2.Col0 + y.Row2.Col0, x.Row2.Col1 + y.Row2.Col1, x.Row2.Col2 + y.Row2.Col2));
        [凾(256)]
        public static Matrix3x3<T> operator -(Matrix3x3<T> x, Matrix3x3<T> y)
            => new Matrix3x3<T>(
                (x.Row0.Col0 - y.Row0.Col0, x.Row0.Col1 - y.Row0.Col1, x.Row0.Col2 - y.Row0.Col2),
                (x.Row1.Col0 - y.Row1.Col0, x.Row1.Col1 - y.Row1.Col1, x.Row1.Col2 - y.Row1.Col2),
                (x.Row2.Col0 - y.Row2.Col0, x.Row2.Col1 - y.Row2.Col1, x.Row2.Col2 - y.Row2.Col2));
        [凾(256)]
        public static Matrix3x3<T> operator *(Matrix3x3<T> x, Matrix3x3<T> y)
            => new Matrix3x3<T>(
                (
                    x.Row0.Col0 * y.Row0.Col0 + x.Row0.Col1 * y.Row1.Col0 + x.Row0.Col2 * y.Row2.Col0,
                    x.Row0.Col0 * y.Row0.Col1 + x.Row0.Col1 * y.Row1.Col1 + x.Row0.Col2 * y.Row2.Col1,
                    x.Row0.Col0 * y.Row0.Col2 + x.Row0.Col1 * y.Row1.Col2 + x.Row0.Col2 * y.Row2.Col2
                ),
                (
                    x.Row1.Col0 * y.Row0.Col0 + x.Row1.Col1 * y.Row1.Col0 + x.Row1.Col2 * y.Row2.Col0,
                    x.Row1.Col0 * y.Row0.Col1 + x.Row1.Col1 * y.Row1.Col1 + x.Row1.Col2 * y.Row2.Col1,
                    x.Row1.Col0 * y.Row0.Col2 + x.Row1.Col1 * y.Row1.Col2 + x.Row1.Col2 * y.Row2.Col2
                ),
                (
                    x.Row2.Col0 * y.Row0.Col0 + x.Row2.Col1 * y.Row1.Col0 + x.Row2.Col2 * y.Row2.Col0,
                    x.Row2.Col0 * y.Row0.Col1 + x.Row2.Col1 * y.Row1.Col1 + x.Row2.Col2 * y.Row2.Col1,
                    x.Row2.Col0 * y.Row0.Col2 + x.Row2.Col1 * y.Row1.Col2 + x.Row2.Col2 * y.Row2.Col2
                )
            );
        [凾(256)]
        public static Matrix3x3<T> operator *(Matrix3x3<T> m, T x)
            => new Matrix3x3<T>(
                (x * m.Row0.Col0, x * m.Row0.Col1, x * m.Row0.Col2),
                (x * m.Row1.Col0, x * m.Row1.Col1, x * m.Row1.Col2),
                (x * m.Row2.Col0, x * m.Row2.Col1, x * m.Row2.Col2)
            );

        /// <summary>
        /// 3次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public static (T v0, T v1, T v2) operator *(Matrix3x3<T> mat, (T v0, T v1, T v2) vector) => mat.Multiply(vector);

        /// <summary>
        /// 3次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T v0, T v1, T v2) Multiply((T v0, T v1, T v2) vector) => Multiply(vector.v0, vector.v1, vector.v2);

        /// <summary>
        /// 3次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T v0, T v1, T v2) Multiply(T v0, T v1, T v2)
            => (
                    Row0.Col0 * v0 + Row0.Col1 * v1 + Row0.Col2 * v2,
                    Row1.Col0 * v0 + Row1.Col1 * v1 + Row1.Col2 * v2,
                    Row2.Col0 * v0 + Row2.Col1 * v1 + Row2.Col2 * v2
               );

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            return
             (Row0.Col0 * Row1.Col1 * Row2.Col2
             + Row1.Col0 * Row0.Col2 * Row2.Col1
             + Row2.Col0 * Row0.Col1 * Row1.Col2)
             -
             (Row0.Col0 * Row1.Col2 * Row2.Col1
             + Row1.Col0 * Row0.Col1 * Row2.Col2
             + Row2.Col0 * Row0.Col2 * Row1.Col1);
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix3x3<T> Inv()
        {
            var r0c0 = Row1.Col1 * Row2.Col2 - Row1.Col2 * Row2.Col1;
            var r1c0 = Row1.Col2 * Row2.Col0 - Row1.Col0 * Row2.Col2;
            var r2c0 = Row1.Col0 * Row2.Col1 - Row1.Col1 * Row2.Col0;

            var r0c1 = Row0.Col2 * Row2.Col1 - Row0.Col1 * Row2.Col2;
            var r1c1 = Row0.Col0 * Row2.Col2 - Row0.Col2 * Row2.Col0;
            var r2c1 = Row0.Col1 * Row2.Col0 - Row0.Col0 * Row2.Col1;

            var r0c2 = Row0.Col1 * Row1.Col2 - Row0.Col2 * Row1.Col1;
            var r1c2 = Row0.Col2 * Row1.Col0 - Row0.Col0 * Row1.Col2;
            var r2c2 = Row0.Col0 * Row1.Col1 - Row0.Col1 * Row1.Col0;

            var det = Determinant();
            var detinv = T.MultiplicativeIdentity / det;
            return new Matrix3x3<T>(
                (detinv * r0c0, detinv * r0c1, detinv * r0c2),
                (detinv * r1c0, detinv * r1c1, detinv * r1c2),
                (detinv * r2c0, detinv * r2c1, detinv * r2c2)
            );
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is Matrix3x3<T> x && Equals(x);
        [凾(256)]
        public bool Equals(Matrix3x3<T> other)
            => Row0.Equals(other.Row0)
            && Row1.Equals(other.Row1)
            && Row2.Equals(other.Row2);
        [凾(256)]
        public override int GetHashCode() => HashCode.Combine(Row0, Row1, Row2);
        [凾(256)]
        public static bool operator ==(Matrix3x3<T> left, Matrix3x3<T> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(Matrix3x3<T> left, Matrix3x3<T> right) => !(left == right);
    }
}
