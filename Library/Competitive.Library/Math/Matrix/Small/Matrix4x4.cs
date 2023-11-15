using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix4x4<T> : Internal.IMatrix<Matrix4x4<T>>
        , IMultiplyOperators<Matrix4x4<T>, T, Matrix4x4<T>>
        where T : INumberBase<T>
    {
        public int Height => 4;
        public int Width => 4;
        public (T Col0, T Col1, T Col2, T Col3) Row0 => (V00, V01, V02, V03);
        public (T Col0, T Col1, T Col2, T Col3) Row1 => (V10, V11, V12, V13);
        public (T Col0, T Col1, T Col2, T Col3) Row2 => (V20, V21, V22, V23);
        public (T Col0, T Col1, T Col2, T Col3) Row3 => (V30, V31, V32, V33);
        internal readonly T
            V00, V01, V02, V03,
            V10, V11, V12, V13,
            V20, V21, V22, V23,
            V30, V31, V32, V33;
        [凾(256)]
        public Matrix4x4((T Col0, T Col1, T Col2, T Col3) row0, (T Col0, T Col1, T Col2, T Col3) row1, (T Col0, T Col1, T Col2, T Col3) row2, (T Col0, T Col1, T Col2, T Col3) row3)
        {
            (V00, V01, V02, V03) = row0;
            (V10, V11, V12, V13) = row1;
            (V20, V21, V22, V23) = row2;
            (V30, V31, V32, V33) = row3;
        }
        public static Matrix4x4<T> AdditiveIdentity => default;
        public static Matrix4x4<T> MultiplicativeIdentity => new(
            (T.MultiplicativeIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.MultiplicativeIdentity, T.AdditiveIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.AdditiveIdentity, T.MultiplicativeIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.MultiplicativeIdentity));

        [凾(256)] public static Matrix4x4<T> operator +(Matrix4x4<T> x) => x;
        [凾(256)]
        public static Matrix4x4<T> operator -(Matrix4x4<T> x)
            => new(
                (-x.V00, -x.V01, -x.V02, -x.V03),
                (-x.V10, -x.V11, -x.V12, -x.V13),
                (-x.V20, -x.V21, -x.V22, -x.V23),
                (-x.V30, -x.V31, -x.V32, -x.V33));
        [凾(256)]
        public static Matrix4x4<T> operator +(Matrix4x4<T> x, Matrix4x4<T> y)
            => new(
                (x.V00 + y.V00, x.V01 + y.V01, x.V02 + y.V02, x.V03 + y.V03),
                (x.V10 + y.V10, x.V11 + y.V11, x.V12 + y.V12, x.V13 + y.V13),
                (x.V20 + y.V20, x.V21 + y.V21, x.V22 + y.V22, x.V23 + y.V23),
                (x.V30 + y.V30, x.V31 + y.V31, x.V32 + y.V32, x.V33 + y.V33));
        [凾(256)]
        public static Matrix4x4<T> operator -(Matrix4x4<T> x, Matrix4x4<T> y)
            => new(
                (x.V00 - y.V00, x.V01 - y.V01, x.V02 - y.V02, x.V03 - y.V03),
                (x.V10 - y.V10, x.V11 - y.V11, x.V12 - y.V12, x.V13 - y.V13),
                (x.V20 - y.V20, x.V21 - y.V21, x.V22 - y.V22, x.V23 - y.V23),
                (x.V30 - y.V30, x.V31 - y.V31, x.V32 - y.V32, x.V33 - y.V33));
        [凾(256)]
        public static Matrix4x4<T> operator *(Matrix4x4<T> x, Matrix4x4<T> y)
            => new(
                (
                    x.V00 * y.V00 + x.V01 * y.V10 + x.V02 * y.V20 + x.V03 * y.V30,
                    x.V00 * y.V01 + x.V01 * y.V11 + x.V02 * y.V21 + x.V03 * y.V31,
                    x.V00 * y.V02 + x.V01 * y.V12 + x.V02 * y.V22 + x.V03 * y.V32,
                    x.V00 * y.V03 + x.V01 * y.V13 + x.V02 * y.V23 + x.V03 * y.V33
                ),
                (
                    x.V10 * y.V00 + x.V11 * y.V10 + x.V12 * y.V20 + x.V13 * y.V30,
                    x.V10 * y.V01 + x.V11 * y.V11 + x.V12 * y.V21 + x.V13 * y.V31,
                    x.V10 * y.V02 + x.V11 * y.V12 + x.V12 * y.V22 + x.V13 * y.V32,
                    x.V10 * y.V03 + x.V11 * y.V13 + x.V12 * y.V23 + x.V13 * y.V33
                ),
                (
                    x.V20 * y.V00 + x.V21 * y.V10 + x.V22 * y.V20 + x.V23 * y.V30,
                    x.V20 * y.V01 + x.V21 * y.V11 + x.V22 * y.V21 + x.V23 * y.V31,
                    x.V20 * y.V02 + x.V21 * y.V12 + x.V22 * y.V22 + x.V23 * y.V32,
                    x.V20 * y.V03 + x.V21 * y.V13 + x.V22 * y.V23 + x.V23 * y.V33
                ),
                (
                    x.V30 * y.V00 + x.V31 * y.V10 + x.V32 * y.V20 + x.V33 * y.V30,
                    x.V30 * y.V01 + x.V31 * y.V11 + x.V32 * y.V21 + x.V33 * y.V31,
                    x.V30 * y.V02 + x.V31 * y.V12 + x.V32 * y.V22 + x.V33 * y.V32,
                    x.V30 * y.V03 + x.V31 * y.V13 + x.V32 * y.V23 + x.V33 * y.V33
                )
            );
        [凾(256)]
        public static Matrix4x4<T> operator *(Matrix4x4<T> m, T x)
             => new(
                 (x * m.V00, x * m.V01, x * m.V02, x * m.V03),
                 (x * m.V10, x * m.V11, x * m.V12, x * m.V13),
                 (x * m.V20, x * m.V21, x * m.V22, x * m.V23),
                 (x * m.V30, x * m.V31, x * m.V32, x * m.V33)
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
                        V00 * v0 + V01 * v1 + V02 * v2 + V03 * v3,
                        V10 * v0 + V11 * v1 + V12 * v2 + V13 * v3,
                        V20 * v0 + V21 * v1 + V22 * v2 + V23 * v3,
                        V30 * v0 + V31 * v1 + V32 * v2 + V33 * v3
                   );

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            var r0c0 = (
             V11 * V22 * V33
             + V21 * V13 * V32
             + V31 * V12 * V23)
             -
             (V11 * V23 * V32
             + V21 * V12 * V33
             + V31 * V13 * V22);
            var r1c0 = (
             V10 * V23 * V32
             + V20 * V12 * V33
             + V30 * V13 * V22)
             -
             (V10 * V22 * V33
             + V20 * V13 * V32
             + V30 * V12 * V23);
            var r2c0 = (
             V10 * V21 * V33
             + V20 * V13 * V31
             + V30 * V11 * V23)
             -
             (V10 * V23 * V31
             + V20 * V11 * V33
             + V30 * V13 * V21);
            var r3c0 = (
             V10 * V22 * V31
             + V20 * V11 * V32
             + V30 * V12 * V21)
             -
             (V10 * V21 * V32
             + V20 * V12 * V31
             + V30 * V11 * V22);
            return V00 * r0c0 + V01 * r1c0 + V02 * r2c0 + V03 * r3c0;
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix4x4<T> Inv()
        {
            var r0c0 = (
             V11 * V22 * V33
             + V21 * V13 * V32
             + V31 * V12 * V23)
             -
             (V11 * V23 * V32
             + V21 * V12 * V33
             + V31 * V13 * V22);
            var r1c0 = (
             V10 * V23 * V32
             + V20 * V12 * V33
             + V30 * V13 * V22)
             -
             (V10 * V22 * V33
             + V20 * V13 * V32
             + V30 * V12 * V23);
            var r2c0 = (
             V10 * V21 * V33
             + V20 * V13 * V31
             + V30 * V11 * V23)
             -
             (V10 * V23 * V31
             + V20 * V11 * V33
             + V30 * V13 * V21);
            var r3c0 = (
             V10 * V22 * V31
             + V20 * V11 * V32
             + V30 * V12 * V21)
             -
             (V10 * V21 * V32
             + V20 * V12 * V31
             + V30 * V11 * V22);
            var r0c1 = (
             V01 * V23 * V32
             + V21 * V02 * V33
             + V31 * V03 * V22)
             -
             (V01 * V22 * V33
             + V21 * V03 * V32
             + V31 * V02 * V23);
            var r1c1 = (
             V00 * V22 * V33
             + V20 * V03 * V32
             + V30 * V02 * V23)
             -
             (V00 * V23 * V32
             + V20 * V02 * V33
             + V30 * V03 * V22);
            var r2c1 = (
             V00 * V23 * V31
             + V20 * V01 * V33
             + V30 * V03 * V21)
             -
             (V00 * V21 * V33
             + V20 * V03 * V31
             + V30 * V01 * V23);
            var r3c1 = (
             V00 * V21 * V32
             + V20 * V02 * V31
             + V30 * V01 * V22)
             -
             (V00 * V22 * V31
             + V20 * V01 * V32
             + V30 * V02 * V21);
            var r0c2 = (
             V01 * V12 * V33
             + V11 * V03 * V32
             + V31 * V02 * V13)
             -
             (V01 * V13 * V32
             + V11 * V02 * V33
             + V31 * V03 * V12);
            var r1c2 = (
             V00 * V13 * V32
             + V10 * V02 * V33
             + V30 * V03 * V12)
             -
             (V00 * V12 * V33
             + V10 * V03 * V32
             + V30 * V02 * V13);
            var r2c2 = (
             V00 * V11 * V33
             + V10 * V03 * V31
             + V30 * V01 * V13)
             -
             (V00 * V13 * V31
             + V10 * V01 * V33
             + V30 * V03 * V11);
            var r3c2 = (
             V00 * V12 * V31
             + V10 * V01 * V32
             + V30 * V02 * V11)
             -
             (V00 * V11 * V32
             + V10 * V02 * V31
             + V30 * V01 * V12);
            var r0c3 = (
             V01 * V13 * V22
             + V11 * V02 * V23
             + V21 * V03 * V12)
             -
             (V01 * V12 * V23
             + V11 * V03 * V22
             + V21 * V02 * V13);
            var r1c3 = (
             V00 * V12 * V23
             + V10 * V03 * V22
             + V20 * V02 * V13)
             -
             (V00 * V13 * V22
             + V10 * V02 * V23
             + V20 * V03 * V12);
            var r2c3 = (
             V00 * V13 * V21
             + V10 * V01 * V23
             + V20 * V03 * V11)
             -
             (V00 * V11 * V23
             + V10 * V03 * V21
             + V20 * V01 * V13);
            var r3c3 = (
             V00 * V11 * V22
             + V10 * V02 * V21
             + V20 * V01 * V12)
             -
             (V00 * V12 * V21
             + V10 * V01 * V22
             + V20 * V02 * V11);
            var det = V00 * r0c0 + V01 * r1c0 + V02 * r2c0 + V03 * r3c0;
            var detinv = T.MultiplicativeIdentity / det;
            return new(
                (detinv * r0c0, detinv * r0c1, detinv * r0c2, detinv * r0c3),
                (detinv * r1c0, detinv * r1c1, detinv * r1c2, detinv * r1c3),
                (detinv * r2c0, detinv * r2c1, detinv * r2c2, detinv * r2c3),
                (detinv * r3c0, detinv * r3c1, detinv * r3c2, detinv * r3c3)
            );
        }


        [凾(256)] public static bool operator ==(Matrix4x4<T> left, Matrix4x4<T> right) => left.Equals(right);
        [凾(256)] public static bool operator !=(Matrix4x4<T> left, Matrix4x4<T> right) => !(left == right);
        [凾(256)] public override bool Equals(object obj) => obj is Matrix4x4<T> x && Equals(x);

        [凾(256)]
        public bool Equals(Matrix4x4<T> other) =>
            EqualityComparer<T>.Default.Equals(V00, other.V00) &&
            EqualityComparer<T>.Default.Equals(V01, other.V01) &&
            EqualityComparer<T>.Default.Equals(V02, other.V02) &&
            EqualityComparer<T>.Default.Equals(V03, other.V03) &&
            EqualityComparer<T>.Default.Equals(V10, other.V10) &&
            EqualityComparer<T>.Default.Equals(V11, other.V11) &&
            EqualityComparer<T>.Default.Equals(V12, other.V12) &&
            EqualityComparer<T>.Default.Equals(V13, other.V13) &&
            EqualityComparer<T>.Default.Equals(V20, other.V20) &&
            EqualityComparer<T>.Default.Equals(V21, other.V21) &&
            EqualityComparer<T>.Default.Equals(V22, other.V22) &&
            EqualityComparer<T>.Default.Equals(V23, other.V23) &&
            EqualityComparer<T>.Default.Equals(V30, other.V30) &&
            EqualityComparer<T>.Default.Equals(V31, other.V31) &&
            EqualityComparer<T>.Default.Equals(V32, other.V32) &&
            EqualityComparer<T>.Default.Equals(V33, other.V33);
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(V00);
            hash.Add(V01);
            hash.Add(V02);
            hash.Add(V03);
            hash.Add(V10);
            hash.Add(V11);
            hash.Add(V12);
            hash.Add(V13);
            hash.Add(V20);
            hash.Add(V21);
            hash.Add(V22);
            hash.Add(V23);
            hash.Add(V30);
            hash.Add(V31);
            hash.Add(V32);
            hash.Add(V33);
            return hash.ToHashCode();
        }
    }
}
