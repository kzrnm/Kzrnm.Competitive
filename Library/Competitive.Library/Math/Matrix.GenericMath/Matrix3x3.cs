using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix3x3<T> : Internal.IMatrixOperator<Matrix3x3<T>>
        , IMultiplyOperators<Matrix3x3<T>, T, Matrix3x3<T>>
        where T : INumberBase<T>
    {
        public (T Col0, T Col1, T Col2) Row0 => (V00, V01, V02);
        public (T Col0, T Col1, T Col2) Row1 => (V10, V11, V12);
        public (T Col0, T Col1, T Col2) Row2 => (V20, V21, V22);
        internal readonly T
            V00, V01, V02,
            V10, V11, V12,
            V20, V21, V22;
        [凾(256)]
        public Matrix3x3((T Col0, T Col1, T Col2) row0, (T Col0, T Col1, T Col2) row1, (T Col0, T Col1, T Col2) row2)
        {
            (V00, V01, V02) = row0;
            (V10, V11, V12) = row1;
            (V20, V21, V22) = row2;
        }
        public static readonly Matrix3x3<T> Identity = new(
            (T.MultiplicativeIdentity, T.AdditiveIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.MultiplicativeIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.AdditiveIdentity, T.MultiplicativeIdentity));
        public static Matrix3x3<T> AdditiveIdentity => default;
        public static Matrix3x3<T> MultiplicativeIdentity => Identity;

        [凾(256)] public static Matrix3x3<T> operator +(Matrix3x3<T> x) => x;
        [凾(256)]
        public static Matrix3x3<T> operator -(Matrix3x3<T> x)
            => new(
                (-x.V00, -x.V01, -x.V02),
                (-x.V10, -x.V11, -x.V12),
                (-x.V20, -x.V21, -x.V22));
        [凾(256)]
        public static Matrix3x3<T> operator +(Matrix3x3<T> x, Matrix3x3<T> y)
            => new(
                (x.V00 + y.V00, x.V01 + y.V01, x.V02 + y.V02),
                (x.V10 + y.V10, x.V11 + y.V11, x.V12 + y.V12),
                (x.V20 + y.V20, x.V21 + y.V21, x.V22 + y.V22));
        [凾(256)]
        public static Matrix3x3<T> operator -(Matrix3x3<T> x, Matrix3x3<T> y)
            => new(
                (x.V00 - y.V00, x.V01 - y.V01, x.V02 - y.V02),
                (x.V10 - y.V10, x.V11 - y.V11, x.V12 - y.V12),
                (x.V20 - y.V20, x.V21 - y.V21, x.V22 - y.V22));
        [凾(256)]
        public static Matrix3x3<T> operator *(Matrix3x3<T> x, Matrix3x3<T> y)
            => new(
                (
                    x.V00 * y.V00 + x.V01 * y.V10 + x.V02 * y.V20,
                    x.V00 * y.V01 + x.V01 * y.V11 + x.V02 * y.V21,
                    x.V00 * y.V02 + x.V01 * y.V12 + x.V02 * y.V22
                ),
                (
                    x.V10 * y.V00 + x.V11 * y.V10 + x.V12 * y.V20,
                    x.V10 * y.V01 + x.V11 * y.V11 + x.V12 * y.V21,
                    x.V10 * y.V02 + x.V11 * y.V12 + x.V12 * y.V22
                ),
                (
                    x.V20 * y.V00 + x.V21 * y.V10 + x.V22 * y.V20,
                    x.V20 * y.V01 + x.V21 * y.V11 + x.V22 * y.V21,
                    x.V20 * y.V02 + x.V21 * y.V12 + x.V22 * y.V22
                )
            );
        [凾(256)]
        public static Matrix3x3<T> operator *(Matrix3x3<T> m, T x)
            => new(
                (x * m.V00, x * m.V01, x * m.V02),
                (x * m.V10, x * m.V11, x * m.V12),
                (x * m.V20, x * m.V21, x * m.V22)
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
                    V00 * v0 + V01 * v1 + V02 * v2,
                    V10 * v0 + V11 * v1 + V12 * v2,
                    V20 * v0 + V21 * v1 + V22 * v2
               );

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            return
             (V00 * V11 * V22
             + V10 * V02 * V21
             + V20 * V01 * V12)
             -
             (V00 * V12 * V21
             + V10 * V01 * V22
             + V20 * V02 * V11);
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix3x3<T> Inv()
        {
            var r0c0 = V11 * V22 - V12 * V21;
            var r1c0 = V12 * V20 - V10 * V22;
            var r2c0 = V10 * V21 - V11 * V20;

            var r0c1 = V02 * V21 - V01 * V22;
            var r1c1 = V00 * V22 - V02 * V20;
            var r2c1 = V01 * V20 - V00 * V21;

            var r0c2 = V01 * V12 - V02 * V11;
            var r1c2 = V02 * V10 - V00 * V12;
            var r2c2 = V00 * V11 - V01 * V10;

            var det = Determinant();
            var detinv = T.MultiplicativeIdentity / det;
            return new(
                (detinv * r0c0, detinv * r0c1, detinv * r0c2),
                (detinv * r1c0, detinv * r1c1, detinv * r1c2),
                (detinv * r2c0, detinv * r2c1, detinv * r2c2)
            );
        }


        [凾(256)] public static bool operator ==(Matrix3x3<T> left, Matrix3x3<T> right) => left.Equals(right);
        [凾(256)] public static bool operator !=(Matrix3x3<T> left, Matrix3x3<T> right) => !(left == right);
        [凾(256)] public override bool Equals(object obj) => obj is Matrix3x3<T> x && Equals(x);
        [凾(256)]
        public bool Equals(Matrix3x3<T> other) =>
            EqualityComparer<T>.Default.Equals(V00, other.V00) &&
            EqualityComparer<T>.Default.Equals(V01, other.V01) &&
            EqualityComparer<T>.Default.Equals(V02, other.V02) &&
            EqualityComparer<T>.Default.Equals(V10, other.V10) &&
            EqualityComparer<T>.Default.Equals(V11, other.V11) &&
            EqualityComparer<T>.Default.Equals(V12, other.V12) &&
            EqualityComparer<T>.Default.Equals(V20, other.V20) &&
            EqualityComparer<T>.Default.Equals(V21, other.V21) &&
            EqualityComparer<T>.Default.Equals(V22, other.V22);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(V00);
            hash.Add(V01);
            hash.Add(V02);
            hash.Add(V10);
            hash.Add(V11);
            hash.Add(V12);
            hash.Add(V20);
            hash.Add(V21);
            hash.Add(V22);
            return hash.ToHashCode();
        }
    }
}
