using AtCoder.Operators;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix3x3<T, TOp> : IEquatable<Matrix3x3<T, TOp>> where TOp : struct, IArithmeticOperator<T>
    {
        private static TOp op = default;
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
        public static Matrix3x3<T, TOp> MultiplicativeIdentity => new Matrix3x3<T, TOp>(
            (op.MultiplyIdentity, default, default),
            (default, op.MultiplyIdentity, default),
            (default, default, op.MultiplyIdentity));

        [凾(256)]
        public static Matrix3x3<T, TOp> operator -(Matrix3x3<T, TOp> x)
            => new Matrix3x3<T, TOp>(
                (op.Minus(x.V00), op.Minus(x.V01), op.Minus(x.V02)),
                (op.Minus(x.V10), op.Minus(x.V11), op.Minus(x.V12)),
                (op.Minus(x.V20), op.Minus(x.V21), op.Minus(x.V22)));
        [凾(256)]
        public static Matrix3x3<T, TOp> operator +(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y)
            => new Matrix3x3<T, TOp>(
                (op.Add(x.V00, y.V00), op.Add(x.V01, y.V01), op.Add(x.V02, y.V02)),
                (op.Add(x.V10, y.V10), op.Add(x.V11, y.V11), op.Add(x.V12, y.V12)),
                (op.Add(x.V20, y.V20), op.Add(x.V21, y.V21), op.Add(x.V22, y.V22)));
        [凾(256)]
        public static Matrix3x3<T, TOp> operator -(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y)
            => new Matrix3x3<T, TOp>(
                (op.Subtract(x.V00, y.V00), op.Subtract(x.V01, y.V01), op.Subtract(x.V02, y.V02)),
                (op.Subtract(x.V10, y.V10), op.Subtract(x.V11, y.V11), op.Subtract(x.V12, y.V12)),
                (op.Subtract(x.V20, y.V20), op.Subtract(x.V21, y.V21), op.Subtract(x.V22, y.V22)));
        [凾(256)]
        public static Matrix3x3<T, TOp> operator *(Matrix3x3<T, TOp> x, Matrix3x3<T, TOp> y)
            => new Matrix3x3<T, TOp>(
                (
                    op.Add(op.Add(op.Multiply(x.V00, y.V00), op.Multiply(x.V01, y.V10)), op.Multiply(x.V02, y.V20)),
                    op.Add(op.Add(op.Multiply(x.V00, y.V01), op.Multiply(x.V01, y.V11)), op.Multiply(x.V02, y.V21)),
                    op.Add(op.Add(op.Multiply(x.V00, y.V02), op.Multiply(x.V01, y.V12)), op.Multiply(x.V02, y.V22))
                ),
                (
                    op.Add(op.Add(op.Multiply(x.V10, y.V00), op.Multiply(x.V11, y.V10)), op.Multiply(x.V12, y.V20)),
                    op.Add(op.Add(op.Multiply(x.V10, y.V01), op.Multiply(x.V11, y.V11)), op.Multiply(x.V12, y.V21)),
                    op.Add(op.Add(op.Multiply(x.V10, y.V02), op.Multiply(x.V11, y.V12)), op.Multiply(x.V12, y.V22))
                ),
                (
                    op.Add(op.Add(op.Multiply(x.V20, y.V00), op.Multiply(x.V21, y.V10)), op.Multiply(x.V22, y.V20)),
                    op.Add(op.Add(op.Multiply(x.V20, y.V01), op.Multiply(x.V21, y.V11)), op.Multiply(x.V22, y.V21)),
                    op.Add(op.Add(op.Multiply(x.V20, y.V02), op.Multiply(x.V21, y.V12)), op.Multiply(x.V22, y.V22))
                )
            );
        [凾(256)]
        public static Matrix3x3<T, TOp> operator *(Matrix3x3<T, TOp> m, T x)
            => new Matrix3x3<T, TOp>(
                (op.Multiply(x, m.V00), op.Multiply(x, m.V01), op.Multiply(x, m.V02)),
                (op.Multiply(x, m.V10), op.Multiply(x, m.V11), op.Multiply(x, m.V12)),
                (op.Multiply(x, m.V20), op.Multiply(x, m.V21), op.Multiply(x, m.V22))
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
                    op.Add(op.Add(op.Multiply(V00, v0), op.Multiply(V01, v1)), op.Multiply(V02, v2)),
                    op.Add(op.Add(op.Multiply(V10, v0), op.Multiply(V11, v1)), op.Multiply(V12, v2)),
                    op.Add(op.Add(op.Multiply(V20, v0), op.Multiply(V21, v1)), op.Multiply(V22, v2))
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
             op.Add(op.Multiply(V00, op.Multiply(V11, V22)),
             op.Add(op.Multiply(V10, op.Multiply(V02, V21)),
                    op.Multiply(V20, op.Multiply(V01, V12)))),
             op.Add(op.Multiply(V00, op.Multiply(V12, V21)),
             op.Add(op.Multiply(V10, op.Multiply(V01, V22)),
                    op.Multiply(V20, op.Multiply(V02, V11)))));
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        [凾(256)]
        public Matrix3x3<T, TOp> Inv()
        {
            var r0c0 = op.Subtract(op.Multiply(V11, V22), op.Multiply(V12, V21));
            var r1c0 = op.Subtract(op.Multiply(V12, V20), op.Multiply(V10, V22));
            var r2c0 = op.Subtract(op.Multiply(V10, V21), op.Multiply(V11, V20));

            var r0c1 = op.Subtract(op.Multiply(V02, V21), op.Multiply(V01, V22));
            var r1c1 = op.Subtract(op.Multiply(V00, V22), op.Multiply(V02, V20));
            var r2c1 = op.Subtract(op.Multiply(V01, V20), op.Multiply(V00, V21));

            var r0c2 = op.Subtract(op.Multiply(V01, V12), op.Multiply(V02, V11));
            var r1c2 = op.Subtract(op.Multiply(V02, V10), op.Multiply(V00, V12));
            var r2c2 = op.Subtract(op.Multiply(V00, V11), op.Multiply(V01, V10));

            var det = Determinant();
            var detinv = op.Divide(op.MultiplyIdentity, det);
            return new Matrix3x3<T, TOp>(
                (op.Multiply(detinv, r0c0), op.Multiply(detinv, r0c1), op.Multiply(detinv, r0c2)),
                (op.Multiply(detinv, r1c0), op.Multiply(detinv, r1c1), op.Multiply(detinv, r1c2)),
                (op.Multiply(detinv, r2c0), op.Multiply(detinv, r2c1), op.Multiply(detinv, r2c2))
            );
        }

        [凾(256)] public static bool operator ==(Matrix3x3<T, TOp> left, Matrix3x3<T, TOp> right) => left.Equals(right);
        [凾(256)] public static bool operator !=(Matrix3x3<T, TOp> left, Matrix3x3<T, TOp> right) => !(left == right);
        [凾(256)] public override bool Equals(object obj) => obj is Matrix3x3<T, TOp> x && Equals(x);
        [凾(256)]
        public bool Equals(Matrix3x3<T, TOp> other) =>
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

        public struct Operator : IArithmeticOperator<Matrix3x3<T, TOp>>
        {
            public Matrix3x3<T, TOp> MultiplyIdentity => MultiplicativeIdentity;

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
