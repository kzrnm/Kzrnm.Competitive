using System;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
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
            this.Row0 = row0;
            this.Row1 = row1;
            this.Row2 = row2;
            this.Row3 = row3;
        }
        public static readonly Matrix4x4<T, TOp> Identity = new Matrix4x4<T, TOp>(
            (op.MultiplyIdentity, default(T), default(T), default(T)),
            (default(T), op.MultiplyIdentity, default(T), default(T)),
            (default(T), default(T), op.MultiplyIdentity, default(T)),
            (default(T), default(T), default(T), op.MultiplyIdentity));

        public static Matrix4x4<T, TOp> operator -(Matrix4x4<T, TOp> x)
            => new Matrix4x4<T, TOp>(
                (op.Minus(x.Row0.Col0), op.Minus(x.Row0.Col1), op.Minus(x.Row0.Col2), op.Minus(x.Row0.Col3)),
                (op.Minus(x.Row1.Col0), op.Minus(x.Row1.Col1), op.Minus(x.Row1.Col2), op.Minus(x.Row1.Col3)),
                (op.Minus(x.Row2.Col0), op.Minus(x.Row2.Col1), op.Minus(x.Row2.Col2), op.Minus(x.Row2.Col3)),
                (op.Minus(x.Row3.Col0), op.Minus(x.Row3.Col1), op.Minus(x.Row3.Col2), op.Minus(x.Row3.Col3)));
        public static Matrix4x4<T, TOp> operator +(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (op.Add(x.Row0.Col0, y.Row0.Col0), op.Add(x.Row0.Col1, y.Row0.Col1), op.Add(x.Row0.Col2, y.Row0.Col2), op.Add(x.Row0.Col3, y.Row0.Col3)),
                (op.Add(x.Row1.Col0, y.Row1.Col0), op.Add(x.Row1.Col1, y.Row1.Col1), op.Add(x.Row1.Col2, y.Row1.Col2), op.Add(x.Row1.Col3, y.Row1.Col3)),
                (op.Add(x.Row2.Col0, y.Row2.Col0), op.Add(x.Row2.Col1, y.Row2.Col1), op.Add(x.Row2.Col2, y.Row2.Col2), op.Add(x.Row2.Col3, y.Row2.Col3)),
                (op.Add(x.Row3.Col0, y.Row3.Col0), op.Add(x.Row3.Col1, y.Row3.Col1), op.Add(x.Row3.Col2, y.Row3.Col2), op.Add(x.Row3.Col3, y.Row3.Col3)));
        public static Matrix4x4<T, TOp> operator -(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y)
            => new Matrix4x4<T, TOp>(
                (op.Subtract(x.Row0.Col0, y.Row0.Col0), op.Subtract(x.Row0.Col1, y.Row0.Col1), op.Subtract(x.Row0.Col2, y.Row0.Col2), op.Subtract(x.Row0.Col3, y.Row0.Col3)),
                (op.Subtract(x.Row1.Col0, y.Row1.Col0), op.Subtract(x.Row1.Col1, y.Row1.Col1), op.Subtract(x.Row1.Col2, y.Row1.Col2), op.Subtract(x.Row1.Col3, y.Row1.Col3)),
                (op.Subtract(x.Row2.Col0, y.Row2.Col0), op.Subtract(x.Row2.Col1, y.Row2.Col1), op.Subtract(x.Row2.Col2, y.Row2.Col2), op.Subtract(x.Row2.Col3, y.Row2.Col3)),
                (op.Subtract(x.Row3.Col0, y.Row3.Col0), op.Subtract(x.Row3.Col1, y.Row3.Col1), op.Subtract(x.Row3.Col2, y.Row3.Col2), op.Subtract(x.Row3.Col3, y.Row3.Col3)));
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

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        public Matrix4x4<T, TOp> Pow(long y) => MathLibGeneric.Pow<Matrix4x4<T, TOp>, Matrix4x4Operator<T, TOp>>(this, y);
    }

    public struct Matrix4x4Operator<T, TOp> : IArithmeticOperator<Matrix4x4<T, TOp>>
        where TOp : struct, IArithmeticOperator<T>
    {
        public Matrix4x4<T, TOp> MultiplyIdentity => Matrix4x4<T, TOp>.Identity;

        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Add(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Subtract(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Multiply(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Minus(Matrix4x4<T, TOp> x) => -x;

        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Increment(Matrix4x4<T, TOp> x) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Decrement(Matrix4x4<T, TOp> x) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Divide(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public Matrix4x4<T, TOp> Modulo(Matrix4x4<T, TOp> x, Matrix4x4<T, TOp> y) => throw new NotSupportedException();
    }
}
