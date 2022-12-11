using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix2x2<T> : Internal.IMatrixOperator<Matrix2x2<T>>
        , IMultiplyOperators<Matrix2x2<T>, T, Matrix2x2<T>>
        where T : INumberBase<T>
    {

        public readonly (T Col0, T Col1) Row0;
        public readonly (T Col0, T Col1) Row1;
        public Matrix2x2((T Col0, T Col1) row0, (T Col0, T Col1) row1)
        {
            Row0 = row0;
            Row1 = row1;
        }
        public static readonly Matrix2x2<T> Identity = new Matrix2x2<T>(
            (T.MultiplicativeIdentity, default),
            (default, T.MultiplicativeIdentity));
        public static Matrix2x2<T> AdditiveIdentity => default;
        public static Matrix2x2<T> MultiplicativeIdentity => Identity;

        [凾(256)] public static Matrix2x2<T> operator +(Matrix2x2<T> x) => x;
        [凾(256)]
        public static Matrix2x2<T> operator -(Matrix2x2<T> x)
            => new Matrix2x2<T>(
                (-x.Row0.Col0, -x.Row0.Col1),
                (-x.Row1.Col0, -x.Row1.Col1));
        [凾(256)]
        public static Matrix2x2<T> operator +(Matrix2x2<T> x, Matrix2x2<T> y)
            => new Matrix2x2<T>(
                (x.Row0.Col0 + y.Row0.Col0, x.Row0.Col1 + y.Row0.Col1),
                (x.Row1.Col0 + y.Row1.Col0, x.Row1.Col1 + y.Row1.Col1));
        [凾(256)]
        public static Matrix2x2<T> operator -(Matrix2x2<T> x, Matrix2x2<T> y)
            => new Matrix2x2<T>(
                (x.Row0.Col0 - y.Row0.Col0, x.Row0.Col1 - y.Row0.Col1),
                (x.Row1.Col0 - y.Row1.Col0, x.Row1.Col1 - y.Row1.Col1));
        [凾(256)]
        public static Matrix2x2<T> operator *(Matrix2x2<T> x, Matrix2x2<T> y)
            => new Matrix2x2<T>(
                (
                    x.Row0.Col0 * y.Row0.Col0 + x.Row0.Col1 * y.Row1.Col0,
                    x.Row0.Col0 * y.Row0.Col1 + x.Row0.Col1 * y.Row1.Col1),
                (
                    x.Row1.Col0 * y.Row0.Col0 + x.Row1.Col1 * y.Row1.Col0,
                    x.Row1.Col0 * y.Row0.Col1 + x.Row1.Col1 * y.Row1.Col1));
        [凾(256)]
        public static Matrix2x2<T> operator *(Matrix2x2<T> m, T x)
            => new Matrix2x2<T>(
                (x * m.Row0.Col0, x * m.Row0.Col1),
                (x * m.Row1.Col0, x * m.Row1.Col1)
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
                        (Row0.Col0 * v0) + (Row0.Col1 * v1),
                        (Row1.Col0 * v0) + (Row1.Col1 * v1)
                   );

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            return (Row0.Col0 * Row1.Col1) - (Row0.Col1 * Row1.Col0);
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix2x2<T> Inv()
        {
            var det = Determinant();
            var detinv = T.MultiplicativeIdentity / det;
            return new Matrix2x2<T>(
                (detinv * Row1.Col1, detinv * -Row0.Col1),
                (detinv * -Row1.Col0, detinv * Row0.Col0)
            );
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is Matrix2x2<T> x && Equals(x);
        [凾(256)]
        public bool Equals(Matrix2x2<T> other) => Row0.Equals(other.Row0) &&
                   Row1.Equals(other.Row1);
        [凾(256)]
        public override int GetHashCode() => HashCode.Combine(Row0, Row1);
        [凾(256)]
        public static bool operator ==(Matrix2x2<T> left, Matrix2x2<T> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(Matrix2x2<T> left, Matrix2x2<T> right) => !(left == right);
    }
}
