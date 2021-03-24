#define ATCODER_CONTRACT
using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    public readonly struct ArrayMatrix<T, TOp>
        where TOp : struct, IArithmeticOperator<T>
    {
        public T this[int row, int col] => this.Value[row][col];

        private static TOp op = default;
        public readonly T[][] Value;


        public static readonly ArrayMatrix<T, TOp> Zero = new ArrayMatrix<T, TOp>(ArrayMatrixKind.Zero);
        public static readonly ArrayMatrix<T, TOp> Identity = new ArrayMatrix<T, TOp>(ArrayMatrixKind.Identity);
        private readonly ArrayMatrixKind kind;
        private ArrayMatrix(ArrayMatrixKind kind)
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
                arr[i] = span.Slice(i * len1, len1).ToArray();
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
                res[i] = (T[])arr[i].Clone();
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
            Contract.Assert(Value.Length == other.Value.Length);
            Contract.Assert(Value[0].Length == other.Value[0].Length);
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
                    ArrayMatrixKind.Zero => ArrayMatrix<T, TOp>.Zero,
                    ArrayMatrixKind.Identity => ArrayMatrix<T, TOp>.Identity,
                    _ => new ArrayMatrix<T, TOp>(CloneArray(y.Value)),
                },
                ArrayMatrixKind.Identity => y.kind switch
                {
                    ArrayMatrixKind.Zero => ArrayMatrix<T, TOp>.Identity,
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
            Contract.Assert(Value.Length == other.Value.Length);
            Contract.Assert(Value[0].Length == other.Value[0].Length);
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
                for (int j = 0; j < arr[i].Length; j++)
                    arr[i][j] = op.Minus(arr[i][j]);
            return new ArrayMatrix<T, TOp>(arr);
        }

        public static ArrayMatrix<T, TOp> operator -(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y)
        {
            return x.kind switch
            {
                ArrayMatrixKind.Zero => y.kind switch
                {
                    ArrayMatrixKind.Zero => ArrayMatrix<T, TOp>.Zero,
                    ArrayMatrixKind.Identity => ArrayMatrix<T, TOp>.Identity,
                    _ => new ArrayMatrix<T, TOp>(CloneArray(y.Value)),
                },
                ArrayMatrixKind.Identity => y.kind switch
                {
                    ArrayMatrixKind.Zero => ArrayMatrix<T, TOp>.Identity,
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
            Contract.Assert(Value[0].Length == other.Value.Length);
            for (int i = 0; i < arr.Length; i++)
                for (var k = 0; k < arr[i].Length; k++)
                    for (int j = 0; j < otherArr[k].Length; j++)
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
                    _ => ArrayMatrix<T, TOp>.Zero,
                },
                ArrayMatrixKind.Identity => y.kind switch
                {
                    ArrayMatrixKind.Zero => ArrayMatrix<T, TOp>.Zero,
                    ArrayMatrixKind.Identity => ArrayMatrix<T, TOp>.Identity,
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

        private ArrayMatrix<T, TOp> MultiplyScalar(T scalar)
        {
            var arr = this.Value;
            var res = NormalZeroMatrix(arr.Length, arr[0].Length);
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < arr[i].Length; j++)
                    res[i][j] = op.Multiply(scalar, arr[i][j]);
            return new ArrayMatrix<T, TOp>(res);
        }
        public static ArrayMatrix<T, TOp> operator *(T x, ArrayMatrix<T, TOp> y)
        {
            return y.kind switch
            {
                ArrayMatrixKind.Normal => y.MultiplyScalar(x),
                _ => ThrowNotSupportResponse(),
            };
        }

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static T[] operator *(ArrayMatrix<T, TOp> mat, T[] vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public T[] Multiply(T[] vector)
        {
            var arr = new T[vector.Length, 1];
            vector.CopyTo(MemoryMarshal.CreateSpan(ref arr[0, 0], vector.Length));
            var resMat = (this * new ArrayMatrix<T, TOp>(arr)).Value;
            var res = new T[resMat.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = resMat[i][0];
            }
            return res;
        }

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        public ArrayMatrix<T, TOp> Pow(long y) => MathLibGeneric.Pow<ArrayMatrix<T, TOp>, ArrayMatrixOperator<T, TOp>>(this, y);

        /// <summary>
        /// 逆行列を掃き出し法で求める
        /// </summary>
        public ArrayMatrix<T, TOp> Inv()
        {
            Contract.Assert(Value.Length == Value[0].Length);
            var orig = Value;
            var len1 = orig.Length * 2;
            var arr = new T[orig.Length][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new T[len1];
                Array.Copy(orig[i], arr[i], orig.Length);
                arr[i][arr.Length + i] = op.MultiplyIdentity;
            }
            GaussianEliminationImpl(arr);
            var res = new T[arr.Length][];
            for (int i = 0; i < res.Length; i++)
                res[i] = arr[i][res.Length..];
            return new ArrayMatrix<T, TOp>(res);
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        public ArrayMatrix<T, TOp> GaussianElimination()
        {
            Contract.Assert(this.kind == ArrayMatrixKind.Normal);
            var arr = CloneArray(Value);
            GaussianEliminationImpl(arr);
            return new ArrayMatrix<T, TOp>(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        private static void GaussianEliminationImpl(T[][] arr)
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
            var len = Math.Min(arr.Length, arr[0].Length);
            for (int i = 0; i < len; i++)
            {
                if (EqualityComparer<T>.Default.Equals(arr[i][i], default))
                {
                    if (!SearchNonZero(arr, i))
                        continue;
                }
                var inv = op.Divide(op.MultiplyIdentity, arr[i][i]);

                for (int k = i; k < arr[i].Length; k++)
                    arr[i][k] = op.Multiply(arr[i][k], inv);

                for (int j = 0; j < arr.Length; j++)
                {
                    if (i == j) continue;
                    for (int k = i + 1; k < arr[j].Length; k++)
                        arr[j][k] = op.Subtract(arr[j][k], op.Multiply(arr[i][k], arr[j][i]));
                    arr[j][i] = default;
                }
            }
        }
        private enum ArrayMatrixKind
        {
            Zero,
            Identity,
            Normal,
        }
    }
    public struct ArrayMatrixOperator<T, TOp> : IArithmeticOperator<ArrayMatrix<T, TOp>>
        where TOp : struct, IArithmeticOperator<T>
    {
        public ArrayMatrix<T, TOp> MultiplyIdentity => ArrayMatrix<T, TOp>.Identity;

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
