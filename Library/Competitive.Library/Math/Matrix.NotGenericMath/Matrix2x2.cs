using AtCoder.Operators;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix2x2<T, TOp> : IEquatable<Matrix2x2<T, TOp>>
        where TOp : struct, IArithmeticOperator<T>
    {
        private static TOp op = default;

        public (T Col0, T Col1) Row0 => (V00, V01);
        public (T Col0, T Col1) Row1 => (V10, V11);

        internal readonly T
            V00, V01,
            V10, V11;

        [凾(256)]
        public Matrix2x2((T Col0, T Col1) row0, (T Col0, T Col1) row1)
        {
            (V00, V01) = row0;
            (V10, V11) = row1;
        }

        public static Matrix2x2<T, TOp> MultiplicativeIdentity => new Matrix2x2<T, TOp>(
            (op.MultiplyIdentity, default),
            (default, op.MultiplyIdentity));

        [凾(256)]
        public static Matrix2x2<T, TOp> operator -(Matrix2x2<T, TOp> x)
            => new Matrix2x2<T, TOp>(
                (op.Minus(x.V00), op.Minus(x.V01)),
                (op.Minus(x.V10), op.Minus(x.V11)));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator +(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y)
            => new Matrix2x2<T, TOp>(
                (op.Add(x.V00, y.V00), op.Add(x.V01, y.V01)),
                (op.Add(x.V10, y.V10), op.Add(x.V11, y.V11)));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator -(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y)
            => new Matrix2x2<T, TOp>(
                (op.Subtract(x.V00, y.V00), op.Subtract(x.V01, y.V01)),
                (op.Subtract(x.V10, y.V10), op.Subtract(x.V11, y.V11)));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator *(Matrix2x2<T, TOp> x, Matrix2x2<T, TOp> y)
            => new Matrix2x2<T, TOp>(
                (
                    op.Add(op.Multiply(x.V00, y.V00), op.Multiply(x.V01, y.V10)),
                    op.Add(op.Multiply(x.V00, y.V01), op.Multiply(x.V01, y.V11))),
                (
                    op.Add(op.Multiply(x.V10, y.V00), op.Multiply(x.V11, y.V10)),
                    op.Add(op.Multiply(x.V10, y.V01), op.Multiply(x.V11, y.V11))));
        [凾(256)]
        public static Matrix2x2<T, TOp> operator *(Matrix2x2<T, TOp> m, T x)
            => new Matrix2x2<T, TOp>(
                (op.Multiply(x, m.V00), op.Multiply(x, m.V01)),
                (op.Multiply(x, m.V10), op.Multiply(x, m.V11))
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
                        op.Add(op.Multiply(V00, v0), op.Multiply(V01, v1)),
                        op.Add(op.Multiply(V10, v0), op.Multiply(V11, v1))
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
            return op.Subtract(op.Multiply(V00, V11), op.Multiply(V01, V10));
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
                (op.Multiply(detinv, V11), op.Multiply(detinv, op.Minus(V01))),
                (op.Multiply(detinv, op.Minus(V10)), op.Multiply(detinv, V00))
            );
        }

        public override bool Equals(object obj) => obj is Matrix2x2<T, TOp> x && Equals(x);
        public bool Equals(Matrix2x2<T, TOp> other) =>
                   EqualityComparer<T>.Default.Equals(V00, other.V00) &&
                   EqualityComparer<T>.Default.Equals(V10, other.V10) &&
                   EqualityComparer<T>.Default.Equals(V01, other.V01) &&
                   EqualityComparer<T>.Default.Equals(V11, other.V11);

        public override int GetHashCode() => HashCode.Combine(V00, V10, V01, V11);
        [凾(256)]
        public static bool operator ==(Matrix2x2<T, TOp> left, Matrix2x2<T, TOp> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(Matrix2x2<T, TOp> left, Matrix2x2<T, TOp> right) => !(left == right);

        public struct Operator : IArithmeticOperator<Matrix2x2<T, TOp>>
        {
            public Matrix2x2<T, TOp> MultiplyIdentity => MultiplicativeIdentity;

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
