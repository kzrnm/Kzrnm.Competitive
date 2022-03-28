using AtCoder.Operators;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix3x3<T, TOp>
        where TOp : struct, IArithmeticOperator<T>
    {
        private static TOp op = default;
        public readonly (T Col0, T Col1, T Col2) Row0;
        public readonly (T Col0, T Col1, T Col2) Row1;
        public readonly (T Col0, T Col1, T Col2) Row2;
        public Matrix3x3((T Col0, T Col1, T Col2) row0, (T Col0, T Col1, T Col2) row1, (T Col0, T Col1, T Col2) row2)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }
        public static readonly Matrix3x3<T, TOp> Identity = new Matrix3x3<T, TOp>(
            (op.MultiplyIdentity, default, default),
            (default, op.MultiplyIdentity, default),
            (default, default, op.MultiplyIdentity));

        [凾(256)]
        public static Matrix3x3<T, TOp> operator -(Matrix3x3<T, TOp> x)
            => new Matrix3x3<T, TOp>(
                (op.Minus(x.Row0.Col0), op.Minus(x.Row0.Col1), op.Minus(x.Row0.Col2)),
                (op.Minus(x.Row1.Col0), op.Minus(x.Row1.Col1), op.Minus(x.Row1.Col2)),
                (op.Minus(x.Row2.Col0), op.Minus(x.Row2.Col1), op.Minus(x.Row2.Col2)));
        [凾(256)]
        public static Matrix3x3<T, TOp> operator +(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y)
            => new Matrix3x3<T, TOp>(
                (op.Add(x.Row0.Col0, y.Row0.Col0), op.Add(x.Row0.Col1, y.Row0.Col1), op.Add(x.Row0.Col2, y.Row0.Col2)),
                (op.Add(x.Row1.Col0, y.Row1.Col0), op.Add(x.Row1.Col1, y.Row1.Col1), op.Add(x.Row1.Col2, y.Row1.Col2)),
                (op.Add(x.Row2.Col0, y.Row2.Col0), op.Add(x.Row2.Col1, y.Row2.Col1), op.Add(x.Row2.Col2, y.Row2.Col2)));
        [凾(256)]
        public static Matrix3x3<T, TOp> operator -(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y)
            => new Matrix3x3<T, TOp>(
                (op.Subtract(x.Row0.Col0, y.Row0.Col0), op.Subtract(x.Row0.Col1, y.Row0.Col1), op.Subtract(x.Row0.Col2, y.Row0.Col2)),
                (op.Subtract(x.Row1.Col0, y.Row1.Col0), op.Subtract(x.Row1.Col1, y.Row1.Col1), op.Subtract(x.Row1.Col2, y.Row1.Col2)),
                (op.Subtract(x.Row2.Col0, y.Row2.Col0), op.Subtract(x.Row2.Col1, y.Row2.Col1), op.Subtract(x.Row2.Col2, y.Row2.Col2)));
        [凾(256)]
        public static Matrix3x3<T, TOp> operator *(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y)
            => new Matrix3x3<T, TOp>(
                (
                    op.Add(op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col0), op.Multiply(x.Row0.Col1, y.Row1.Col0)), op.Multiply(x.Row0.Col2, y.Row2.Col0)),
                    op.Add(op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col1), op.Multiply(x.Row0.Col1, y.Row1.Col1)), op.Multiply(x.Row0.Col2, y.Row2.Col1)),
                    op.Add(op.Add(op.Multiply(x.Row0.Col0, y.Row0.Col2), op.Multiply(x.Row0.Col1, y.Row1.Col2)), op.Multiply(x.Row0.Col2, y.Row2.Col2))
                ),
                (
                    op.Add(op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col0), op.Multiply(x.Row1.Col1, y.Row1.Col0)), op.Multiply(x.Row1.Col2, y.Row2.Col0)),
                    op.Add(op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col1), op.Multiply(x.Row1.Col1, y.Row1.Col1)), op.Multiply(x.Row1.Col2, y.Row2.Col1)),
                    op.Add(op.Add(op.Multiply(x.Row1.Col0, y.Row0.Col2), op.Multiply(x.Row1.Col1, y.Row1.Col2)), op.Multiply(x.Row1.Col2, y.Row2.Col2))
                ),
                (
                    op.Add(op.Add(op.Multiply(x.Row2.Col0, y.Row0.Col0), op.Multiply(x.Row2.Col1, y.Row1.Col0)), op.Multiply(x.Row2.Col2, y.Row2.Col0)),
                    op.Add(op.Add(op.Multiply(x.Row2.Col0, y.Row0.Col1), op.Multiply(x.Row2.Col1, y.Row1.Col1)), op.Multiply(x.Row2.Col2, y.Row2.Col1)),
                    op.Add(op.Add(op.Multiply(x.Row2.Col0, y.Row0.Col2), op.Multiply(x.Row2.Col1, y.Row1.Col2)), op.Multiply(x.Row2.Col2, y.Row2.Col2))
                )
            );
        [凾(256)]
        public static Matrix3x3<T, TOp> operator *(T a, Matrix3x3<T, TOp> y)
            => new Matrix3x3<T, TOp>(
                (op.Multiply(a, y.Row0.Col0), op.Multiply(a, y.Row0.Col1), op.Multiply(a, y.Row0.Col2)),
                (op.Multiply(a, y.Row1.Col0), op.Multiply(a, y.Row1.Col1), op.Multiply(a, y.Row1.Col2)),
                (op.Multiply(a, y.Row2.Col0), op.Multiply(a, y.Row2.Col1), op.Multiply(a, y.Row2.Col2))
            );

        /// <summary>
        /// 3次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public static (T v0, T v1, T v2) operator *(Matrix3x3<T, TOp> mat, (T v0, T v1, T v2) vector) => mat.Multiply(vector);

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
                    op.Add(op.Add(op.Multiply(Row0.Col0, v0), op.Multiply(Row0.Col1, v1)), op.Multiply(Row0.Col2, v2)),
                    op.Add(op.Add(op.Multiply(Row1.Col0, v0), op.Multiply(Row1.Col1, v1)), op.Multiply(Row1.Col2, v2)),
                    op.Add(op.Add(op.Multiply(Row2.Col0, v0), op.Multiply(Row2.Col1, v1)), op.Multiply(Row2.Col2, v2))
               );

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        [凾(256)]
        public Matrix3x3<T, TOp> Pow(long y) => MathLibGeneric.Pow<Matrix3x3<T, TOp>, Operator>(this, y);

        /// <summary>
        /// 行列式を求める
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            return op.Subtract(
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col1, Row2.Col2)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col2, Row2.Col1)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col1, Row1.Col2)))),
             op.Add(op.Multiply(Row0.Col0, op.Multiply(Row1.Col2, Row2.Col1)),
             op.Add(op.Multiply(Row1.Col0, op.Multiply(Row0.Col1, Row2.Col2)),
                    op.Multiply(Row2.Col0, op.Multiply(Row0.Col2, Row1.Col1)))));
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix3x3<T, TOp> Inv()
        {
            var r0c0 = op.Subtract(op.Multiply(Row1.Col1, Row2.Col2), op.Multiply(Row1.Col2, Row2.Col1));
            var r1c0 = op.Subtract(op.Multiply(Row1.Col2, Row2.Col0), op.Multiply(Row1.Col0, Row2.Col2));
            var r2c0 = op.Subtract(op.Multiply(Row1.Col0, Row2.Col1), op.Multiply(Row1.Col1, Row2.Col0));

            var r0c1 = op.Subtract(op.Multiply(Row0.Col2, Row2.Col1), op.Multiply(Row0.Col1, Row2.Col2));
            var r1c1 = op.Subtract(op.Multiply(Row0.Col0, Row2.Col2), op.Multiply(Row0.Col2, Row2.Col0));
            var r2c1 = op.Subtract(op.Multiply(Row0.Col1, Row2.Col0), op.Multiply(Row0.Col0, Row2.Col1));

            var r0c2 = op.Subtract(op.Multiply(Row0.Col1, Row1.Col2), op.Multiply(Row0.Col2, Row1.Col1));
            var r1c2 = op.Subtract(op.Multiply(Row0.Col2, Row1.Col0), op.Multiply(Row0.Col0, Row1.Col2));
            var r2c2 = op.Subtract(op.Multiply(Row0.Col0, Row1.Col1), op.Multiply(Row0.Col1, Row1.Col0));

            var det = Determinant();
            var detinv = op.Divide(op.MultiplyIdentity, det);
            return new Matrix3x3<T, TOp>(
                (op.Multiply(detinv, r0c0), op.Multiply(detinv, r0c1), op.Multiply(detinv, r0c2)),
                (op.Multiply(detinv, r1c0), op.Multiply(detinv, r1c1), op.Multiply(detinv, r1c2)),
                (op.Multiply(detinv, r2c0), op.Multiply(detinv, r2c1), op.Multiply(detinv, r2c2))
            );
        }
        public struct Operator : IArithmeticOperator<Matrix3x3<T, TOp>>
        {
            public Matrix3x3<T, TOp> MultiplyIdentity => Identity;

            [凾(256)]
            public Matrix3x3<T, TOp> Add(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y) => x + y;
            [凾(256)]
            public Matrix3x3<T, TOp> Subtract(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y) => x - y;
            [凾(256)]
            public Matrix3x3<T, TOp> Multiply(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y) => x * y;
            [凾(256)]
            public Matrix3x3<T, TOp> Minus(Matrix3x3<T, TOp> x) => -x;

            [凾(256)]
            public Matrix3x3<T, TOp> Increment(Matrix3x3<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public Matrix3x3<T, TOp> Decrement(Matrix3x3<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public Matrix3x3<T, TOp> Divide(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y) => throw new NotSupportedException();
            [凾(256)]
            public Matrix3x3<T, TOp> Modulo(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y) => throw new NotSupportedException();
        }
    }
}
