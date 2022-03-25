using AtCoder.Operators;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix4x4<T, TOp>
        where TOp : struct, IArithmeticOperator<T>
    {
        private static TOp op = default;
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
        public static readonly Matrix4x4<T, TOp> Identity = new Matrix4x4<T, TOp>(
            (op.MultiplyIdentity, default, default, default),
            (default, op.MultiplyIdentity, default, default),
            (default, default, op.MultiplyIdentity, default),
            (default, default, default, op.MultiplyIdentity));

        [凾(256)]
        public static Matrix4x4<T, TOp> operator -(Matrix4x4<T, TOp> x)
            => new Matrix4x4<T, TOp>(
                (op.Minus(x.Row0.Col0), op.Minus(x.Row0.Col1), op.Minus(x.Row0.Col2), op.Minus(x.Row0.Col3)),
                (op.Minus(x.Row1.Col0), op.Minus(x.Row1.Col1), op.Minus(x.Row1.Col2), op.Minus(x.Row1.Col3)),
                (op.Minus(x.Row2.Col0), op.Minus(x.Row2.Col1), op.Minus(x.Row2.Col2), op.Minus(x.Row2.Col3)),
                (op.Minus(x.Row3.Col0), op.Minus(x.Row3.Col1), op.Minus(x.Row3.Col2), op.Minus(x.Row3.Col3)));
        [凾(256)]
        public static Matrix4x4<T, TOp> operator +(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (op.Add(x.Row0.Col0, y.Row0.Col0), op.Add(x.Row0.Col1, y.Row0.Col1), op.Add(x.Row0.Col2, y.Row0.Col2), op.Add(x.Row0.Col3, y.Row0.Col3)),
                (op.Add(x.Row1.Col0, y.Row1.Col0), op.Add(x.Row1.Col1, y.Row1.Col1), op.Add(x.Row1.Col2, y.Row1.Col2), op.Add(x.Row1.Col3, y.Row1.Col3)),
                (op.Add(x.Row2.Col0, y.Row2.Col0), op.Add(x.Row2.Col1, y.Row2.Col1), op.Add(x.Row2.Col2, y.Row2.Col2), op.Add(x.Row2.Col3, y.Row2.Col3)),
                (op.Add(x.Row3.Col0, y.Row3.Col0), op.Add(x.Row3.Col1, y.Row3.Col1), op.Add(x.Row3.Col2, y.Row3.Col2), op.Add(x.Row3.Col3, y.Row3.Col3)));
        [凾(256)]
        public static Matrix4x4<T, TOp> operator -(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (op.Subtract(x.Row0.Col0, y.Row0.Col0), op.Subtract(x.Row0.Col1, y.Row0.Col1), op.Subtract(x.Row0.Col2, y.Row0.Col2), op.Subtract(x.Row0.Col3, y.Row0.Col3)),
                (op.Subtract(x.Row1.Col0, y.Row1.Col0), op.Subtract(x.Row1.Col1, y.Row1.Col1), op.Subtract(x.Row1.Col2, y.Row1.Col2), op.Subtract(x.Row1.Col3, y.Row1.Col3)),
                (op.Subtract(x.Row2.Col0, y.Row2.Col0), op.Subtract(x.Row2.Col1, y.Row2.Col1), op.Subtract(x.Row2.Col2, y.Row2.Col2), op.Subtract(x.Row2.Col3, y.Row2.Col3)),
                (op.Subtract(x.Row3.Col0, y.Row3.Col0), op.Subtract(x.Row3.Col1, y.Row3.Col1), op.Subtract(x.Row3.Col2, y.Row3.Col2), op.Subtract(x.Row3.Col3, y.Row3.Col3)));
        [凾(256)]
        public static Matrix4x4<T, TOp> operator *(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col0), op.Multiply(x.Row0.Col1, y.Row1.Col0)), op.Multiply(x.Row0.Col2, y.Row2.Col0)), op.Multiply(x.Row0.Col3, y.Row3.Col0)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col1), op.Multiply(x.Row0.Col1, y.Row1.Col1)), op.Multiply(x.Row0.Col2, y.Row2.Col1)), op.Multiply(x.Row0.Col3, y.Row3.Col1)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col2), op.Multiply(x.Row0.Col1, y.Row1.Col2)), op.Multiply(x.Row0.Col2, y.Row2.Col2)), op.Multiply(x.Row0.Col3, y.Row3.Col2)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col3), op.Multiply(x.Row0.Col1, y.Row1.Col3)), op.Multiply(x.Row0.Col2, y.Row2.Col3)), op.Multiply(x.Row0.Col3, y.Row3.Col3))
                ),
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col0), op.Multiply(x.Row1.Col1, y.Row1.Col0)), op.Multiply(x.Row1.Col2, y.Row2.Col0)), op.Multiply(x.Row1.Col3, y.Row3.Col0)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col1), op.Multiply(x.Row1.Col1, y.Row1.Col1)), op.Multiply(x.Row1.Col2, y.Row2.Col1)), op.Multiply(x.Row1.Col3, y.Row3.Col1)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col2), op.Multiply(x.Row1.Col1, y.Row1.Col2)), op.Multiply(x.Row1.Col2, y.Row2.Col2)), op.Multiply(x.Row1.Col3, y.Row3.Col2)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col3), op.Multiply(x.Row1.Col1, y.Row1.Col3)), op.Multiply(x.Row1.Col2, y.Row2.Col3)), op.Multiply(x.Row1.Col3, y.Row3.Col3))
                ),
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.Row2.Col0, y.Row0.Col0), op.Multiply(x.Row2.Col1, y.Row1.Col0)), op.Multiply(x.Row2.Col2, y.Row2.Col0)), op.Multiply(x.Row2.Col3, y.Row3.Col0)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row2.Col0, y.Row0.Col1), op.Multiply(x.Row2.Col1, y.Row1.Col1)), op.Multiply(x.Row2.Col2, y.Row2.Col1)), op.Multiply(x.Row2.Col3, y.Row3.Col1)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row2.Col0, y.Row0.Col2), op.Multiply(x.Row2.Col1, y.Row1.Col2)), op.Multiply(x.Row2.Col2, y.Row2.Col2)), op.Multiply(x.Row2.Col3, y.Row3.Col2)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row2.Col0, y.Row0.Col3), op.Multiply(x.Row2.Col1, y.Row1.Col3)), op.Multiply(x.Row2.Col2, y.Row2.Col3)), op.Multiply(x.Row2.Col3, y.Row3.Col3))
                ),
                (
                    op.Add(op.Add(op.Add(op.Multiply(x.Row3.Col0, y.Row0.Col0), op.Multiply(x.Row3.Col1, y.Row1.Col0)), op.Multiply(x.Row3.Col2, y.Row2.Col0)), op.Multiply(x.Row3.Col3, y.Row3.Col0)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row3.Col0, y.Row0.Col1), op.Multiply(x.Row3.Col1, y.Row1.Col1)), op.Multiply(x.Row3.Col2, y.Row2.Col1)), op.Multiply(x.Row3.Col3, y.Row3.Col1)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row3.Col0, y.Row0.Col2), op.Multiply(x.Row3.Col1, y.Row1.Col2)), op.Multiply(x.Row3.Col2, y.Row2.Col2)), op.Multiply(x.Row3.Col3, y.Row3.Col2)),
                    op.Add(op.Add(op.Add(op.Multiply(x.Row3.Col0, y.Row0.Col3), op.Multiply(x.Row3.Col1, y.Row1.Col3)), op.Multiply(x.Row3.Col2, y.Row2.Col3)), op.Multiply(x.Row3.Col3, y.Row3.Col3))
                )
            );
        [凾(256)]
        public static Matrix4x4<T, TOp> operator *(T a, Matrix4x4<T, TOp> y)
             => new Matrix4x4<T, TOp>(
                 (op.Multiply(a, y.Row0.Col0), op.Multiply(a, y.Row0.Col1), op.Multiply(a, y.Row0.Col2), op.Multiply(a, y.Row0.Col3)),
                 (op.Multiply(a, y.Row1.Col0), op.Multiply(a, y.Row1.Col1), op.Multiply(a, y.Row1.Col2), op.Multiply(a, y.Row1.Col3)),
                 (op.Multiply(a, y.Row2.Col0), op.Multiply(a, y.Row2.Col1), op.Multiply(a, y.Row2.Col2), op.Multiply(a, y.Row2.Col3)),
                 (op.Multiply(a, y.Row3.Col0), op.Multiply(a, y.Row3.Col1), op.Multiply(a, y.Row3.Col2), op.Multiply(a, y.Row3.Col3))
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
                        op.Add(op.Add(op.Add(op.Multiply(Row0.Col0, v0), op.Multiply(Row0.Col1, v1)), op.Multiply(Row0.Col2, v2)), op.Multiply(Row0.Col3, v3)),
                        op.Add(op.Add(op.Add(op.Multiply(Row1.Col0, v0), op.Multiply(Row1.Col1, v1)), op.Multiply(Row1.Col2, v2)), op.Multiply(Row1.Col3, v3)),
                        op.Add(op.Add(op.Add(op.Multiply(Row2.Col0, v0), op.Multiply(Row2.Col1, v1)), op.Multiply(Row2.Col2, v2)), op.Multiply(Row2.Col3, v3)),
                        op.Add(op.Add(op.Add(op.Multiply(Row3.Col0, v0), op.Multiply(Row3.Col1, v1)), op.Multiply(Row3.Col2, v2)), op.Multiply(Row3.Col3, v3))
                   );

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        [凾(256)]
        public Matrix4x4<T, TOp> Pow(long y) => MathLibGeneric.Pow<Matrix4x4<T, TOp>, Matrix4x4Operator<T, TOp>>(this, y);

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            var r0c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row2.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col1, op.Multiply(Row1.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col1, op.Multiply(Row1.Col2, Row2.Col3)))),
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row2.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col1, op.Multiply(Row1.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col1, op.Multiply(Row1.Col3, Row2.Col2)))));
            var r1c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col3, Row2.Col2)))),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col2, Row2.Col3)))));
            var r2c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col1, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col3, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col1, Row2.Col3)))),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col3, Row3.Col1)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col1, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col3, Row2.Col1)))));
            var r3c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col2, Row3.Col1)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col1, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col2, Row2.Col1)))),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col1, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col2, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col1, Row2.Col2)))));
            return op.Add(op.Add(op.Multiply(Row0.Col0, r0c0), op.Multiply(Row0.Col1, r1c0)), op.Add(op.Multiply(Row0.Col2, r2c0), op.Multiply(Row0.Col3, r3c0)));
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix4x4<T, TOp> Inv()
        {
            var r0c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row2.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col1, op.Multiply(Row1.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col1, op.Multiply(Row1.Col2, Row2.Col3)))),
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row2.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col1, op.Multiply(Row1.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col1, op.Multiply(Row1.Col3, Row2.Col2)))));
            var r1c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col3, Row2.Col2)))),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col2, Row2.Col3)))));
            var r2c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col1, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col3, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col1, Row2.Col3)))),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col3, Row3.Col1)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col1, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col3, Row2.Col1)))));
            var r3c0 = op.Subtract(
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col2, Row3.Col1)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col1, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col2, Row2.Col1)))),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row2.Col1, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row1.Col2, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row1.Col1, Row2.Col2)))));
            var r0c1 = op.Subtract(
             op.Add(op.Multiply(Row0.Col1, op.Multiply(Row2.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col1, op.Multiply(Row0.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col1, op.Multiply(Row0.Col3, Row2.Col2)))),
             op.Add(op.Multiply(Row0.Col1, op.Multiply(Row2.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col1, op.Multiply(Row0.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col1, op.Multiply(Row0.Col2, Row2.Col3)))));
            var r1c1 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row2.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row0.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col2, Row2.Col3)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row2.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row0.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col3, Row2.Col2)))));
            var r2c1 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row2.Col3, Row3.Col1)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row0.Col1, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col3, Row2.Col1)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row2.Col1, Row3.Col3)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row0.Col3, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col1, Row2.Col3)))));
            var r3c1 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row2.Col1, Row3.Col2)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row0.Col2, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col1, Row2.Col2)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row2.Col2, Row3.Col1)),
             op.Add(op.Multiply(Row2.Col0, op.Multiply(Row0.Col1, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col2, Row2.Col1)))));
            var r0c2 = op.Subtract(
             op.Add(op.Multiply(Row0.Col1, op.Multiply(Row1.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row0.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col1, op.Multiply(Row0.Col2, Row1.Col3)))),
             op.Add(op.Multiply(Row0.Col1, op.Multiply(Row1.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row0.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col1, op.Multiply(Row0.Col3, Row1.Col2)))));
            var r1c2 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col3, Row3.Col2)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col2, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col3, Row1.Col2)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col2, Row3.Col3)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col3, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col2, Row1.Col3)))));
            var r2c2 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col1, Row3.Col3)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col3, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col1, Row1.Col3)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col3, Row3.Col1)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col1, Row3.Col3)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col3, Row1.Col1)))));
            var r3c2 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col2, Row3.Col1)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col1, Row3.Col2)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col2, Row1.Col1)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col1, Row3.Col2)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col2, Row3.Col1)),
                    op.Multiply(Row3.Col0, op.Multiply(Row0.Col1, Row1.Col2)))));
            var r0c3 = op.Subtract(
             op.Add(op.Multiply(Row0.Col1, op.Multiply(Row1.Col3, Row2.Col2)),
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row0.Col2, Row2.Col3)),
                    op.Multiply(Row2.Col1, op.Multiply(Row0.Col3, Row1.Col2)))),
             op.Add(op.Multiply(Row0.Col1, op.Multiply(Row1.Col2, Row2.Col3)),
             op.Add(op.Multiply(Row1.Col1, op.Multiply(Row0.Col3, Row2.Col2)),
                    op.Multiply(Row2.Col1, op.Multiply(Row0.Col2, Row1.Col3)))));
            var r1c3 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col2, Row2.Col3)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col3, Row2.Col2)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col2, Row1.Col3)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col3, Row2.Col2)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col2, Row2.Col3)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col3, Row1.Col2)))));
            var r2c3 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col3, Row2.Col1)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col1, Row2.Col3)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col3, Row1.Col1)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col1, Row2.Col3)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col3, Row2.Col1)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col1, Row1.Col3)))));
            var r3c3 = op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col1, Row2.Col2)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col2, Row2.Col1)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col1, Row1.Col2)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col2, Row2.Col1)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col1, Row2.Col2)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col2, Row1.Col1)))));
            var det = op.Add(op.Add(op.Multiply(Row0.Col0, r0c0), op.Multiply(Row0.Col1, r1c0)), op.Add(op.Multiply(Row0.Col2, r2c0), op.Multiply(Row0.Col3, r3c0)));
            var detinv = op.Divide(op.MultiplyIdentity, det);
            return new Matrix4x4<T, TOp>(
                (op.Multiply(detinv, r0c0), op.Multiply(detinv, r0c1), op.Multiply(detinv, r0c2), op.Multiply(detinv, r0c3)),
                (op.Multiply(detinv, r1c0), op.Multiply(detinv, r1c1), op.Multiply(detinv, r1c2), op.Multiply(detinv, r1c3)),
                (op.Multiply(detinv, r2c0), op.Multiply(detinv, r2c1), op.Multiply(detinv, r2c2), op.Multiply(detinv, r2c3)),
                (op.Multiply(detinv, r3c0), op.Multiply(detinv, r3c1), op.Multiply(detinv, r3c2), op.Multiply(detinv, r3c3))
            );
        }
    }

    public struct Matrix4x4Operator<T, TOp> : IArithmeticOperator<Matrix4x4<T, TOp>>
        where TOp : struct, IArithmeticOperator<T>
    {
        public Matrix4x4<T, TOp> MultiplyIdentity => Matrix4x4<T, TOp>.Identity;

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
