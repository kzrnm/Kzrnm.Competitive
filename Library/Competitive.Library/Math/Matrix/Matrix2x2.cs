﻿using AtCoder.Operators;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix2x2<T, TOp>
        where TOp : struct, IArithmeticOperator<T>
    {
        private static TOp op = default;
        public readonly (T Col0, T Col1) Row0;
        public readonly (T Col0, T Col1) Row1;
        public Matrix2x2((T Col0, T Col1) row0, (T Col0, T Col1) row1)
        {
            Row0 = row0;
            Row1 = row1;
        }
        public static readonly Matrix2x2<T, TOp> Identity = new Matrix2x2<T, TOp>(
            (op.MultiplyIdentity, default),
            (default, op.MultiplyIdentity));

        [凾(256)]
        public static Matrix2x2<T, TOp> operator -(Matrix2x2<T, TOp> x)
            => new Matrix2x2<T, TOp>(
                (op.Minus(x.Row0.Col0), op.Minus(x.Row0.Col1)),
                (op.Minus(x.Row1.Col0), op.Minus(x.Row1.Col1)));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator +(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y)
            => new Matrix2x2<T, TOp>(
                (op.Add(x.Row0.Col0, y.Row0.Col0), op.Add(x.Row0.Col1, y.Row0.Col1)),
                (op.Add(x.Row1.Col0, y.Row1.Col0), op.Add(x.Row1.Col1, y.Row1.Col1)));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator -(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y)
            => new Matrix2x2<T, TOp>(
                (op.Subtract(x.Row0.Col0, y.Row0.Col0), op.Subtract(x.Row0.Col1, y.Row0.Col1)),
                (op.Subtract(x.Row1.Col0, y.Row1.Col0), op.Subtract(x.Row1.Col1, y.Row1.Col1)));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator *(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y)
            => new Matrix2x2<T, TOp>(
                (
                    op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col0), op.Multiply(x.Row0.Col1, y.Row1.Col0)),
                    op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col1), op.Multiply(x.Row0.Col1, y.Row1.Col1))),
                (
                    op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col0), op.Multiply(x.Row1.Col1, y.Row1.Col0)),
                    op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col1), op.Multiply(x.Row1.Col1, y.Row1.Col1))));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator *(T a, Matrix2x2<T, TOp> y)
            => new Matrix2x2<T, TOp>(
                (op.Multiply(a, y.Row0.Col0), op.Multiply(a, y.Row0.Col1)),
                (op.Multiply(a, y.Row1.Col0), op.Multiply(a, y.Row1.Col1))
            );

        /// <summary>
        /// 2次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public static (T v0, T v1) operator *(Matrix2x2<T, TOp> mat, (T v0, T v1) vector) => mat.Multiply(vector);

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
                        op.Add(op.Multiply(Row0.Col0, v0), op.Multiply(Row0.Col1, v1)),
                        op.Add(op.Multiply(Row1.Col0, v0), op.Multiply(Row1.Col1, v1))
                   );

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        [凾(256)]
        public Matrix2x2<T, TOp> Pow(long y) => MathLibGeneric.Pow<Matrix2x2<T, TOp>, Operator>(this, y);


        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            return op.Subtract(op.Multiply(Row0.Col0, Row1.Col1), op.Multiply(Row0.Col1, Row1.Col0));
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix2x2<T, TOp> Inv()
        {
            var det = Determinant();
            var detinv = op.Divide(op.MultiplyIdentity, det);
            return new Matrix2x2<T, TOp>(
                (op.Multiply(detinv, Row1.Col1), op.Multiply(detinv, op.Minus(Row0.Col1))),
                (op.Multiply(detinv, op.Minus(Row1.Col0)), op.Multiply(detinv, Row0.Col0))
            );
        }
        public struct Operator : IArithmeticOperator<Matrix2x2<T, TOp>>
        {
            public Matrix2x2<T, TOp> MultiplyIdentity => Identity;

            [凾(256)]
            public Matrix2x2<T, TOp> Add(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y) => x + y;
            [凾(256)]
            public Matrix2x2<T, TOp> Subtract(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y) => x - y;
            [凾(256)]
            public Matrix2x2<T, TOp> Multiply(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y) => x * y;
            [凾(256)]
            public Matrix2x2<T, TOp> Minus(Matrix2x2<T, TOp> x) => -x;

            [凾(256)]
            public Matrix2x2<T, TOp> Increment(Matrix2x2<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public Matrix2x2<T, TOp> Decrement(Matrix2x2<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public Matrix2x2<T, TOp> Divide(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y) => throw new NotSupportedException();
            [凾(256)]
            public Matrix2x2<T, TOp> Modulo(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y) => throw new NotSupportedException();
        }
    }
}
