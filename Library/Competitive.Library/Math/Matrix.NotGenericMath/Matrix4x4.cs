using AtCoder.Operators;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix4x4<T, TOp> : IEquatable<Matrix4x4<T, TOp>> where TOp : struct, IArithmeticOperator<T>
    {
        private static TOp op = default;
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
        public static Matrix4x4<T, TOp> MultiplicativeIdentity => new Matrix4x4<T, TOp>(
            (op.MultiplyIdentity, default, default, default),
            (default, op.MultiplyIdentity, default, default),
            (default, default, op.MultiplyIdentity, default),
            (default, default, default, op.MultiplyIdentity));

        [凾(256)]
        public static Matrix4x4<T, TOp> operator -(Matrix4x4<T, TOp> x)
            => new Matrix4x4<T, TOp>(
                (op.Minus(x.V00), op.Minus(x.V01), op.Minus(x.V02), op.Minus(x.V03)),
                (op.Minus(x.V10), op.Minus(x.V11), op.Minus(x.V12), op.Minus(x.V13)),
                (op.Minus(x.V20), op.Minus(x.V21), op.Minus(x.V22), op.Minus(x.V23)),
                (op.Minus(x.V30), op.Minus(x.V31), op.Minus(x.V32), op.Minus(x.V33)));
        [凾(256)]
        public static Matrix4x4<T, TOp> operator +(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (op.Add(x.V00, y.V00), op.Add(x.V01, y.V01), op.Add(x.V02, y.V02), op.Add(x.V03, y.V03)),
                (op.Add(x.V10, y.V10), op.Add(x.V11, y.V11), op.Add(x.V12, y.V12), op.Add(x.V13, y.V13)),
                (op.Add(x.V20, y.V20), op.Add(x.V21, y.V21), op.Add(x.V22, y.V22), op.Add(x.V23, y.V23)),
                (op.Add(x.V30, y.V30), op.Add(x.V31, y.V31), op.Add(x.V32, y.V32), op.Add(x.V33, y.V33)));
        [凾(256)]
        public static Matrix4x4<T, TOp> operator -(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (op.Subtract(x.V00, y.V00), op.Subtract(x.V01, y.V01), op.Subtract(x.V02, y.V02), op.Subtract(x.V03, y.V03)),
                (op.Subtract(x.V10, y.V10), op.Subtract(x.V11, y.V11), op.Subtract(x.V12, y.V12), op.Subtract(x.V13, y.V13)),
                (op.Subtract(x.V20, y.V20), op.Subtract(x.V21, y.V21), op.Subtract(x.V22, y.V22), op.Subtract(x.V23, y.V23)),
                (op.Subtract(x.V30, y.V30), op.Subtract(x.V31, y.V31), op.Subtract(x.V32, y.V32), op.Subtract(x.V33, y.V33)));
        [凾(256)]
        public static Matrix4x4<T, TOp> operator *(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.V00, y.V00), op.Multiply(x.V01, y.V10)), op.Multiply(x.V02, y.V20)), op.Multiply(x.V03, y.V30)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V00, y.V01), op.Multiply(x.V01, y.V11)), op.Multiply(x.V02, y.V21)), op.Multiply(x.V03, y.V31)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V00, y.V02), op.Multiply(x.V01, y.V12)), op.Multiply(x.V02, y.V22)), op.Multiply(x.V03, y.V32)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V00, y.V03), op.Multiply(x.V01, y.V13)), op.Multiply(x.V02, y.V23)), op.Multiply(x.V03, y.V33))
                ),
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.V10, y.V00), op.Multiply(x.V11, y.V10)), op.Multiply(x.V12, y.V20)), op.Multiply(x.V13, y.V30)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V10, y.V01), op.Multiply(x.V11, y.V11)), op.Multiply(x.V12, y.V21)), op.Multiply(x.V13, y.V31)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V10, y.V02), op.Multiply(x.V11, y.V12)), op.Multiply(x.V12, y.V22)), op.Multiply(x.V13, y.V32)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V10, y.V03), op.Multiply(x.V11, y.V13)), op.Multiply(x.V12, y.V23)), op.Multiply(x.V13, y.V33))
                ),
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.V20, y.V00), op.Multiply(x.V21, y.V10)), op.Multiply(x.V22, y.V20)), op.Multiply(x.V23, y.V30)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V20, y.V01), op.Multiply(x.V21, y.V11)), op.Multiply(x.V22, y.V21)), op.Multiply(x.V23, y.V31)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V20, y.V02), op.Multiply(x.V21, y.V12)), op.Multiply(x.V22, y.V22)), op.Multiply(x.V23, y.V32)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V20, y.V03), op.Multiply(x.V21, y.V13)), op.Multiply(x.V22, y.V23)), op.Multiply(x.V23, y.V33))
                ),
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.V30, y.V00), op.Multiply(x.V31, y.V10)), op.Multiply(x.V32, y.V20)), op.Multiply(x.V33, y.V30)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V30, y.V01), op.Multiply(x.V31, y.V11)), op.Multiply(x.V32, y.V21)), op.Multiply(x.V33, y.V31)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V30, y.V02), op.Multiply(x.V31, y.V12)), op.Multiply(x.V32, y.V22)), op.Multiply(x.V33, y.V32)),
                    op.Add(op.Add(op.Add(op.Multiply(x.V30, y.V03), op.Multiply(x.V31, y.V13)), op.Multiply(x.V32, y.V23)), op.Multiply(x.V33, y.V33))
                )
            );
        [凾(256)]
        public static Matrix4x4<T, TOp> operator *(Matrix4x4<T, TOp> m, T x)
             => new Matrix4x4<T, TOp>(
                 (op.Multiply(x, m.V00), op.Multiply(x, m.V01), op.Multiply(x, m.V02), op.Multiply(x, m.V03)),
                 (op.Multiply(x, m.V10), op.Multiply(x, m.V11), op.Multiply(x, m.V12), op.Multiply(x, m.V13)),
                 (op.Multiply(x, m.V20), op.Multiply(x, m.V21), op.Multiply(x, m.V22), op.Multiply(x, m.V23)),
                 (op.Multiply(x, m.V30), op.Multiply(x, m.V31), op.Multiply(x, m.V32), op.Multiply(x, m.V33))
             );

        /// <summary>
        /// 4次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public static (T v0, T v1, T v2, T v3) operator *(Matrix4x4<T, TOp> mat, (T v0, T v1, T v2, T v3) vector) => mat.Multiply(vector);

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
                        op.Add(op.Add(op.Add(op.Multiply(V00, v0), op.Multiply(V01, v1)), op.Multiply(V02, v2)), op.Multiply(V03, v3)),
                        op.Add(op.Add(op.Add(op.Multiply(V10, v0), op.Multiply(V11, v1)), op.Multiply(V12, v2)), op.Multiply(V13, v3)),
                        op.Add(op.Add(op.Add(op.Multiply(V20, v0), op.Multiply(V21, v1)), op.Multiply(V22, v2)), op.Multiply(V23, v3)),
                        op.Add(op.Add(op.Add(op.Multiply(V30, v0), op.Multiply(V31, v1)), op.Multiply(V32, v2)), op.Multiply(V33, v3))
                   );

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        [凾(256)]
        public Matrix4x4<T, TOp> Pow(long y) => MathLibGeneric.Pow<Matrix4x4<T, TOp>, Operator>(this, y);

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            var r0c0 = op.Subtract(
             op.Add(op.Multiply(V11, op.Multiply(V22, V33)),
             op.Add(op.Multiply(V21, op.Multiply(V13, V32)),
                    op.Multiply(V31, op.Multiply(V12, V23)))),
             op.Add(op.Multiply(V11, op.Multiply(V23, V32)),
             op.Add(op.Multiply(V21, op.Multiply(V12, V33)),
                    op.Multiply(V31, op.Multiply(V13, V22)))));
            var r1c0 = op.Subtract(
             op.Add(op.Multiply(V10, op.Multiply(V23, V32)),
             op.Add(op.Multiply(V20, op.Multiply(V12, V33)),
                    op.Multiply(V30, op.Multiply(V13, V22)))),
             op.Add(op.Multiply(V10, op.Multiply(V22, V33)),
             op.Add(op.Multiply(V20, op.Multiply(V13, V32)),
                    op.Multiply(V30, op.Multiply(V12, V23)))));
            var r2c0 = op.Subtract(
             op.Add(op.Multiply(V10, op.Multiply(V21, V33)),
             op.Add(op.Multiply(V20, op.Multiply(V13, V31)),
                    op.Multiply(V30, op.Multiply(V11, V23)))),
             op.Add(op.Multiply(V10, op.Multiply(V23, V31)),
             op.Add(op.Multiply(V20, op.Multiply(V11, V33)),
                    op.Multiply(V30, op.Multiply(V13, V21)))));
            var r3c0 = op.Subtract(
             op.Add(op.Multiply(V10, op.Multiply(V22, V31)),
             op.Add(op.Multiply(V20, op.Multiply(V11, V32)),
                    op.Multiply(V30, op.Multiply(V12, V21)))),
             op.Add(op.Multiply(V10, op.Multiply(V21, V32)),
             op.Add(op.Multiply(V20, op.Multiply(V12, V31)),
                    op.Multiply(V30, op.Multiply(V11, V22)))));
            return op.Add(op.Add(op.Multiply(V00, r0c0), op.Multiply(V01, r1c0)), op.Add(op.Multiply(V02, r2c0), op.Multiply(V03, r3c0)));
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix4x4<T, TOp> Inv()
        {
            var r0c0 = op.Subtract(
             op.Add(op.Multiply(V11, op.Multiply(V22, V33)),
             op.Add(op.Multiply(V21, op.Multiply(V13, V32)),
                    op.Multiply(V31, op.Multiply(V12, V23)))),
             op.Add(op.Multiply(V11, op.Multiply(V23, V32)),
             op.Add(op.Multiply(V21, op.Multiply(V12, V33)),
                    op.Multiply(V31, op.Multiply(V13, V22)))));
            var r1c0 = op.Subtract(
             op.Add(op.Multiply(V10, op.Multiply(V23, V32)),
             op.Add(op.Multiply(V20, op.Multiply(V12, V33)),
                    op.Multiply(V30, op.Multiply(V13, V22)))),
             op.Add(op.Multiply(V10, op.Multiply(V22, V33)),
             op.Add(op.Multiply(V20, op.Multiply(V13, V32)),
                    op.Multiply(V30, op.Multiply(V12, V23)))));
            var r2c0 = op.Subtract(
             op.Add(op.Multiply(V10, op.Multiply(V21, V33)),
             op.Add(op.Multiply(V20, op.Multiply(V13, V31)),
                    op.Multiply(V30, op.Multiply(V11, V23)))),
             op.Add(op.Multiply(V10, op.Multiply(V23, V31)),
             op.Add(op.Multiply(V20, op.Multiply(V11, V33)),
                    op.Multiply(V30, op.Multiply(V13, V21)))));
            var r3c0 = op.Subtract(
             op.Add(op.Multiply(V10, op.Multiply(V22, V31)),
             op.Add(op.Multiply(V20, op.Multiply(V11, V32)),
                    op.Multiply(V30, op.Multiply(V12, V21)))),
             op.Add(op.Multiply(V10, op.Multiply(V21, V32)),
             op.Add(op.Multiply(V20, op.Multiply(V12, V31)),
                    op.Multiply(V30, op.Multiply(V11, V22)))));
            var r0c1 = op.Subtract(
             op.Add(op.Multiply(V01, op.Multiply(V23, V32)),
             op.Add(op.Multiply(V21, op.Multiply(V02, V33)),
                    op.Multiply(V31, op.Multiply(V03, V22)))),
             op.Add(op.Multiply(V01, op.Multiply(V22, V33)),
             op.Add(op.Multiply(V21, op.Multiply(V03, V32)),
                    op.Multiply(V31, op.Multiply(V02, V23)))));
            var r1c1 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V22, V33)),
             op.Add(op.Multiply(V20, op.Multiply(V03, V32)),
                    op.Multiply(V30, op.Multiply(V02, V23)))),
             op.Add(op.Multiply(V00, op.Multiply(V23, V32)),
             op.Add(op.Multiply(V20, op.Multiply(V02, V33)),
                    op.Multiply(V30, op.Multiply(V03, V22)))));
            var r2c1 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V23, V31)),
             op.Add(op.Multiply(V20, op.Multiply(V01, V33)),
                    op.Multiply(V30, op.Multiply(V03, V21)))),
             op.Add(op.Multiply(V00, op.Multiply(V21, V33)),
             op.Add(op.Multiply(V20, op.Multiply(V03, V31)),
                    op.Multiply(V30, op.Multiply(V01, V23)))));
            var r3c1 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V21, V32)),
             op.Add(op.Multiply(V20, op.Multiply(V02, V31)),
                    op.Multiply(V30, op.Multiply(V01, V22)))),
             op.Add(op.Multiply(V00, op.Multiply(V22, V31)),
             op.Add(op.Multiply(V20, op.Multiply(V01, V32)),
                    op.Multiply(V30, op.Multiply(V02, V21)))));
            var r0c2 = op.Subtract(
             op.Add(op.Multiply(V01, op.Multiply(V12, V33)),
             op.Add(op.Multiply(V11, op.Multiply(V03, V32)),
                    op.Multiply(V31, op.Multiply(V02, V13)))),
             op.Add(op.Multiply(V01, op.Multiply(V13, V32)),
             op.Add(op.Multiply(V11, op.Multiply(V02, V33)),
                    op.Multiply(V31, op.Multiply(V03, V12)))));
            var r1c2 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V13, V32)),
             op.Add(op.Multiply(V10, op.Multiply(V02, V33)),
                    op.Multiply(V30, op.Multiply(V03, V12)))),
             op.Add(op.Multiply(V00, op.Multiply(V12, V33)),
             op.Add(op.Multiply(V10, op.Multiply(V03, V32)),
                    op.Multiply(V30, op.Multiply(V02, V13)))));
            var r2c2 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V11, V33)),
             op.Add(op.Multiply(V10, op.Multiply(V03, V31)),
                    op.Multiply(V30, op.Multiply(V01, V13)))),
             op.Add(op.Multiply(V00, op.Multiply(V13, V31)),
             op.Add(op.Multiply(V10, op.Multiply(V01, V33)),
                    op.Multiply(V30, op.Multiply(V03, V11)))));
            var r3c2 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V12, V31)),
             op.Add(op.Multiply(V10, op.Multiply(V01, V32)),
                    op.Multiply(V30, op.Multiply(V02, V11)))),
             op.Add(op.Multiply(V00, op.Multiply(V11, V32)),
             op.Add(op.Multiply(V10, op.Multiply(V02, V31)),
                    op.Multiply(V30, op.Multiply(V01, V12)))));
            var r0c3 = op.Subtract(
             op.Add(op.Multiply(V01, op.Multiply(V13, V22)),
             op.Add(op.Multiply(V11, op.Multiply(V02, V23)),
                    op.Multiply(V21, op.Multiply(V03, V12)))),
             op.Add(op.Multiply(V01, op.Multiply(V12, V23)),
             op.Add(op.Multiply(V11, op.Multiply(V03, V22)),
                    op.Multiply(V21, op.Multiply(V02, V13)))));
            var r1c3 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V12, V23)),
             op.Add(op.Multiply(V10, op.Multiply(V03, V22)),
                    op.Multiply(V20, op.Multiply(V02, V13)))),
             op.Add(op.Multiply(V00, op.Multiply(V13, V22)),
             op.Add(op.Multiply(V10, op.Multiply(V02, V23)),
                    op.Multiply(V20, op.Multiply(V03, V12)))));
            var r2c3 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V13, V21)),
             op.Add(op.Multiply(V10, op.Multiply(V01, V23)),
                    op.Multiply(V20, op.Multiply(V03, V11)))),
             op.Add(op.Multiply(V00, op.Multiply(V11, V23)),
             op.Add(op.Multiply(V10, op.Multiply(V03, V21)),
                    op.Multiply(V20, op.Multiply(V01, V13)))));
            var r3c3 = op.Subtract(
             op.Add(op.Multiply(V00, op.Multiply(V11, V22)),
             op.Add(op.Multiply(V10, op.Multiply(V02, V21)),
                    op.Multiply(V20, op.Multiply(V01, V12)))),
             op.Add(op.Multiply(V00, op.Multiply(V12, V21)),
             op.Add(op.Multiply(V10, op.Multiply(V01, V22)),
                    op.Multiply(V20, op.Multiply(V02, V11)))));
            var det = op.Add(op.Add(op.Multiply(V00, r0c0), op.Multiply(V01, r1c0)), op.Add(op.Multiply(V02, r2c0), op.Multiply(V03, r3c0)));
            var detinv = op.Divide(op.MultiplyIdentity, det);
            return new Matrix4x4<T, TOp>(
                (op.Multiply(detinv, r0c0), op.Multiply(detinv, r0c1), op.Multiply(detinv, r0c2), op.Multiply(detinv, r0c3)),
                (op.Multiply(detinv, r1c0), op.Multiply(detinv, r1c1), op.Multiply(detinv, r1c2), op.Multiply(detinv, r1c3)),
                (op.Multiply(detinv, r2c0), op.Multiply(detinv, r2c1), op.Multiply(detinv, r2c2), op.Multiply(detinv, r2c3)),
                (op.Multiply(detinv, r3c0), op.Multiply(detinv, r3c1), op.Multiply(detinv, r3c2), op.Multiply(detinv, r3c3))
            );
        }

        [凾(256)]
        public static bool operator ==(Matrix4x4<T, TOp> left, Matrix4x4<T, TOp> right)
            => left.Equals(right);

        [凾(256)]
        public static bool operator !=(Matrix4x4<T, TOp> left, Matrix4x4<T, TOp> right)
            => !(left == right);

        [凾(256)]
        public override bool Equals(object obj)
            => obj is Matrix4x4<T, TOp> x && Equals(x);


        [凾(256)]
        public bool Equals(Matrix4x4<T, TOp> other) =>
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

        public struct Operator : IArithmeticOperator<Matrix4x4<T, TOp>>
        {
            public Matrix4x4<T, TOp> MultiplyIdentity => MultiplicativeIdentity;

            [凾(256)]
            public Matrix4x4<T, TOp> Add(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => x + y;
            [凾(256)]
            public Matrix4x4<T, TOp> Subtract(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => x - y;
            [凾(256)]
            public Matrix4x4<T, TOp> Multiply(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => x * y;
            [凾(256)]
            public Matrix4x4<T, TOp> Minus(Matrix4x4<T, TOp> x) => -x;

            [凾(256)]
            public Matrix4x4<T, TOp> Increment(Matrix4x4<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public Matrix4x4<T, TOp> Decrement(Matrix4x4<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public Matrix4x4<T, TOp> Divide(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => throw new NotSupportedException();
            [凾(256)]
            public Matrix4x4<T, TOp> Modulo(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => throw new NotSupportedException();
        }
    }
}
