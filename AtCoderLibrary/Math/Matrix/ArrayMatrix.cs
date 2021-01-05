using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AtCoder
{
    using static MethodImplOptions;
    public readonly struct ArrayMatrix<T, TOp>
        where TOp : struct, IArithmeticOperator<T>
    {
        public T this[int row, int col] => this.Value[row][col];

        private static TOp op = default;
        private readonly ArrayMatrixKind kind;
        public readonly T[][] Value;
        public ArrayMatrix(ArrayMatrixKind kind)
        {
            this.kind = kind;
            this.Value = null;
        }
        public ArrayMatrix(T[][] value)
        {
            this.Value = value;
            kind = ArrayMatrixKind.Normal;
        }
        public ArrayMatrix(T[,] value)
        {
            var len0 = value.GetLength(0);
            var len1 = value.GetLength(1);
            var arr = this.Value = new T[len0][];
            var span = MemoryMarshal.CreateReadOnlySpan(ref value[0, 0], len0 * len1);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = span.Slice(i * len0, len1).ToArray();
            }
            kind = ArrayMatrixKind.Normal;
        }

        private static ArrayMatrix<T, TOp> ThrowNotSupportResponse() => throw new NotSupportedException();

        private static T[][] NormalZeroMatrix(int row, int col)
        {
            var arr = new T[row][];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new T[col];
            return arr;
        }
        private static T[][] CloneArray(T[][] arr)
        {
            var res = new T[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
                res[i] = (T[])arr.Clone();
            return res;
        }

        private ArrayMatrix<T, TOp> AddIdentity()
        {
            var arr = CloneArray(Value);
            for (int i = 0; i < arr.Length; i++)
                arr[i][i] = op.Add(arr[i][i], op.MultiplyIdentity);
            return new ArrayMatrix<T, TOp>(arr);
        }
        private ArrayMatrix<T, TOp> Add(ArrayMatrix<T, TOp> other)
        {
            DebugUtil.Assert(Value.Length == other.Value.Length);
            DebugUtil.Assert(Value[0].Length == other.Value[0].Length);
            var otherArr = other.Value;
            var arr = CloneArray(Value);
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < arr[i].Length; j++)
                    arr[i][j] = op.Add(arr[i][j], otherArr[i][j]);
            return new ArrayMatrix<T, TOp>(arr);
        }
        public static ArrayMatrix<T, TOp> operator +(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y)
        {
            return x.kind switch
            {
                ArrayMatrixKind.Zero => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Zero),
                    ArrayMatrixKind.Identity => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Identity),
                    _ => new ArrayMatrix<T, TOp>(CloneArray(y.Value)),
                },
                ArrayMatrixKind.Identity => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Identity),
                    ArrayMatrixKind.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(CloneArray(x.Value)),
                    ArrayMatrixKind.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }
        private ArrayMatrix<T, TOp> SubtractIdentity()
        {
            var arr = CloneArray(Value);
            for (int i = 0; i < arr.Length; i++)
                arr[i][i] = op.Subtract(arr[i][i], op.MultiplyIdentity);
            return new ArrayMatrix<T, TOp>(arr);
        }
        private ArrayMatrix<T, TOp> Subtract(ArrayMatrix<T, TOp> other)
        {
            DebugUtil.Assert(Value.Length == other.Value.Length);
            DebugUtil.Assert(Value[0].Length == other.Value[0].Length);
            var otherArr = other.Value;
            var arr = CloneArray(Value);
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < arr[i].Length; j++)
                    arr[i][j] = op.Subtract(arr[i][j], otherArr[i][j]);
            return new ArrayMatrix<T, TOp>(arr);
        }
        public static ArrayMatrix<T, TOp> operator -(ArrayMatrix<T, TOp> x)
        {
            var arr = CloneArray(x.Value);
            for (int i = 0; i < arr.Length; i++)
                arr[i][i] = op.Minus(arr[i][i]);
            return new ArrayMatrix<T, TOp>(arr);
        }

        public static ArrayMatrix<T, TOp> operator -(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y)
        {
            return x.kind switch
            {
                ArrayMatrixKind.Zero => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Zero),
                    ArrayMatrixKind.Identity => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Identity),
                    _ => new ArrayMatrix<T, TOp>(CloneArray(y.Value)),
                },
                ArrayMatrixKind.Identity => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Identity),
                    ArrayMatrixKind.Identity => ThrowNotSupportResponse(),
                    _ => (-y).AddIdentity(),
                },
                _ => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(CloneArray(x.Value)),
                    ArrayMatrixKind.Identity => x.SubtractIdentity(),
                    _ => x.Subtract(y),
                },
            };
        }
        private ArrayMatrix<T, TOp> Multiply(ArrayMatrix<T, TOp> other)
        {
            var arr = this.Value;
            var otherArr = other.Value;
            var res = NormalZeroMatrix(Value.Length, other.Value[0].Length);
            DebugUtil.Assert(Value[0].Length == other.Value.Length);
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < arr[i].Length; j++)
                    for (var k = 0; k < otherArr.Length; k++)
                        res[i][j] = op.Add(res[i][j], op.Multiply(arr[i][k], otherArr[k][j]));
            return new ArrayMatrix<T, TOp>(res);
        }
        public static ArrayMatrix<T, TOp> operator *(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y)
        {
            return x.kind switch
            {
                ArrayMatrixKind.Zero => y.kind switch
                {
                    ArrayMatrixKind.Normal => new ArrayMatrix<T, TOp>(NormalZeroMatrix(y.Value.Length, y.Value[0].Length)),
                    _ => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Zero),
                },
                ArrayMatrixKind.Identity => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Zero),
                    ArrayMatrixKind.Identity => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Identity),
                    _ => y,
                },
                _ => y.kind switch
                {
                    ArrayMatrixKind.Zero => new ArrayMatrix<T, TOp>(NormalZeroMatrix(x.Value.Length, x.Value[0].Length)),
                    ArrayMatrixKind.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        public ArrayMatrix<T, TOp> Pow(long y) => MathLibGeneric.Pow<ArrayMatrix<T, TOp>, ArrayMatrixOperator<T, TOp>>(this, y);



        /// <summary>
        /// ガウスの消去法(掃き出し法)で一次方程式を解く。
        /// </summary>
        public ArrayMatrix<T, TOp> GaussianElimination()
        {
            static bool SearchNonZero(T[][] mat, int i)
            {
                for (int j = i + 1; j < mat.Length; j++)
                    if (!EqualityComparer<T>.Default.Equals(mat[j][i], default))
                    {
                        (mat[i], mat[j]) = (mat[j], mat[i]);
                        return true;
                    }
                return false;
            }
            var op = default(TOp);
            var arr = CloneArray(Value);
            DebugUtil.Assert(this.kind == ArrayMatrixKind.Normal);
            DebugUtil.Assert(arr[0].Length == arr.Length + 1);
            for (int i = 0; i < arr.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(arr[i][i], default))
                {
                    if (!SearchNonZero(arr, i))
                        continue;
                }
                var inv = op.Divide(op.MultiplyIdentity, arr[i][i]);

                for (int k = i; k < arr[i].Length; k++)
                    arr[i][k] = op.Multiply(arr[i][k], inv);

                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = i + 1; k < arr[j].Length; k++)
                        arr[j][k] = op.Subtract(arr[j][k], op.Multiply(arr[i][k], arr[j][i]));
                    arr[j][i] = default;
                }
            }
            return new ArrayMatrix<T, TOp>(arr);
        }
    }
    public enum ArrayMatrixKind
    {
        Zero,
        Identity,
        Normal,
    }
    public struct ArrayMatrixOperator<T, TOp> : IArithmeticOperator<ArrayMatrix<T, TOp>>
        where TOp : struct, IArithmeticOperator<T>
    {
        public ArrayMatrix<T, TOp> MultiplyIdentity => new ArrayMatrix<T, TOp>(ArrayMatrixKind.Identity);

        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Add(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Subtract(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Multiply(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Minus(ArrayMatrix<T, TOp> x) => -x;

        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Increment(ArrayMatrix<T, TOp> x) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Decrement(ArrayMatrix<T, TOp> x) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Divide(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public ArrayMatrix<T, TOp> Modulo(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => throw new NotSupportedException();
    }
}
