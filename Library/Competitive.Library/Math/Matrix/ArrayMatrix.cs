using AtCoder.Internal;
using AtCoder.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = ArrayMatrixKind;
    public readonly struct ArrayMatrix<T, TOp>
        where TOp : struct, IArithmeticOperator<T>
    {
        public T this[int row, int col] => Value[row][col];

        private static TOp op = default;
        public readonly T[][] Value;

        public static readonly ArrayMatrix<T, TOp> Zero = new ArrayMatrix<T, TOp>(Kd.Zero);
        public static readonly ArrayMatrix<T, TOp> Identity = new ArrayMatrix<T, TOp>(Kd.Identity);
        internal readonly Kd kind;
        internal ArrayMatrix(Kd kind)
        {
            this.kind = kind;
            Value = null;
        }

        public ArrayMatrix(T[][] value)
        {
            Value = value;
            kind = Kd.Normal;
        }
        public ArrayMatrix(T[,] value)
        {
            var len0 = value.GetLength(0);
            var len1 = value.GetLength(1);
            var arr = Value = new T[len0][];
            var span = MemoryMarshal.CreateReadOnlySpan(ref value[0, 0], len0 * len1);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = span.Slice(i * len1, len1).ToArray();
            }
            kind = Kd.Normal;
        }

        private static ArrayMatrix<T, TOp> ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;
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
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => new ArrayMatrix<T, TOp>(CloneArray(y.Value)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new ArrayMatrix<T, TOp>(CloneArray(x.Value)),
                    Kd.Identity => x.AddIdentity(),
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
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => new ArrayMatrix<T, TOp>(CloneArray(y.Value)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => (-y).AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new ArrayMatrix<T, TOp>(CloneArray(x.Value)),
                    Kd.Identity => x.SubtractIdentity(),
                    _ => x.Subtract(y),
                },
            };
        }
        private ArrayMatrix<T, TOp> Multiply(ArrayMatrix<T, TOp> other)
        {
            var arr = Value;
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
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new ArrayMatrix<T, TOp>(NormalZeroMatrix(y.Value.Length, y.Value[0].Length)),
                    _ => Zero,
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => y,
                },
                _ => y.kind switch
                {
                    Kd.Zero => new ArrayMatrix<T, TOp>(NormalZeroMatrix(x.Value.Length, x.Value[0].Length)),
                    Kd.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        private ArrayMatrix<T, TOp> MultiplyScalar(T scalar)
        {
            var arr = Value;
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
                Kd.Normal => y.MultiplyScalar(x),
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
        /// 行列式を求める。行列式
        /// </summary>
        public T Determinant()
        {
            var arr = CloneArray(Value);
            Contract.Assert(arr.Length == arr[0].Length);

            bool swap = false;
            //上三角行列
            for (int i = 0; i < arr.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(arr[i][i], default))
                {
                    if (!SearchNonZero(arr, i, i))
                        return default;
                    swap = !swap;
                }
                var t = arr[i][i];
                for (int j = i + 1; j < arr.Length; j++)
                {
                    var c = op.Divide(arr[j][i], t);
                    for (int k = 0; k < arr[i].Length; k++)
                        arr[j][k] = op.Subtract(arr[j][k], op.Multiply(c, arr[i][k]));
                }
            }

            T det = op.MultiplyIdentity;
            if (swap)
                det = op.Minus(det);
            for (int i = 0; i < arr.Length; i++)
                det = op.Multiply(det, arr[i][i]);
            return det;
        }

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        public ArrayMatrix<T, TOp> Pow(long y) => MathLibGeneric.Pow<ArrayMatrix<T, TOp>, Operator>(this, y);

        /// <summary>
        /// 逆行列を掃き出し法で求める。求められなければ零行列を返す。
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
            GaussianEliminationImpl(arr, true);

            var ix = arr.Length - 1;
            if (EqualityComparer<T>.Default.Equals(arr[ix][ix], default))
                return Zero;

            var res = new T[arr.Length][];
            for (int i = 0; i < res.Length; i++)
                res[i] = arr[i][res.Length..];
            return new ArrayMatrix<T, TOp>(res);
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        public ArrayMatrix<T, TOp> GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = CloneArray(Value);
            GaussianEliminationImpl(arr, isReduced);
            return new ArrayMatrix<T, TOp>(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>0ではない列のインデックス</returns>
        private static List<int> GaussianEliminationImpl(T[][] arr, bool isReduced)
        {
            var idx = new List<int>(arr.Length);
            int r = 0;
            int width = arr[0].Length;
            int ys = isReduced ? 0 : r + 1;
            for (int x = 0; x < width && r < arr.Length; x++)
            {
                if (!SearchNonZero(arr, r, x))
                    continue;
                var arrR = arr[r];
                {
                    var inv = op.Divide(op.MultiplyIdentity, arrR[x]);
                    arrR[x] = op.MultiplyIdentity;
                    for (int i = x + 1; i < arrR.Length; i++)
                        arrR[i] = op.Multiply(inv, arrR[i]);
                }
                for (int y = ys; y < arr.Length; y++)
                {
                    var arrY = arr[y];
                    if (y == r || EqualityComparer<T>.Default.Equals(arrY[x], default))
                        continue;
                    var freq = arrY[x];
                    for (int k = x + 1; k < arrY.Length; k++)
                        arrY[k] = op.Subtract(arrY[k], op.Multiply(freq, arrR[k]));
                    arrY[x] = default;
                }
                ++r;
                idx.Add(x);
            }
            return idx;
        }
        /// <summary>
        /// <paramref name="r"/> より下で <paramref name="x"/> 列が 0 ではない行を探して、<paramref name="r"/> 行に置く。
        /// </summary>
        /// <returns>0 ではない行が見つかったかどうか</returns>
        private static bool SearchNonZero(T[][] mat, int r, int x)
        {
            for (int y = r; y < mat.Length; y++)
                if (!EqualityComparer<T>.Default.Equals(mat[y][x], default))
                {
                    (mat[r], mat[y]) = (mat[y], mat[r]);
                    return true;
                }
            return false;
        }
        /// <summary>
        /// 連立一次方程式 <see langword="this"/>・X=<paramref name="vector"/> を満たす ベクトル X を求める。
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item><description>最初の配列: 特殊解</description></item>
        /// <item><description>2番目以降の配列: 解空間の基底ベクトル</description></item>
        /// <item><description>ただし解無しのときは空配列を返す</description></item>
        /// </list>
        /// </returns>
        public T[][] LinearSystem(T[] vector)
        {
            Contract.Assert(Value.Length == vector.Length);
            var impl = Value.Zip(vector, (row, v) => row.Append(v).ToArray()).ToArray();
            var idxs = GaussianEliminationImpl(impl, false).AsSpan();
            var r = idxs.Length;
            int w = Value[0].Length;

            // 解があるかチェック
            // a×0+b×0+c×0..+z×0≠0 になっていたら解無し
            if (idxs[^1] == w)
                return Array.Empty<T[]>();
            for (int i = r; i < impl.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(impl[i][^1], default))
                    return Array.Empty<T[]>();
            }

            var used = new HashSet<int>(Enumerable.Range(0, w));
            var lst = new List<T[]>(w);
            {
                var v = new T[w];
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    used.Remove(f);
                    v[f] = impl[y][^1];
                    for (int x = f + 1; x < w; x++)
                        v[f] = op.Subtract(v[f], op.Multiply(impl[y][x], v[x]));
                    v[f] = op.Divide(v[f], impl[y][f]);
                }
                lst.Add(v);
            }

            foreach (var s in used)
            {
                var v = new T[w];
                v[s] = op.MultiplyIdentity;
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    for (int x = f + 1; x < w; x++)
                        v[f] = op.Subtract(v[f], op.Multiply(impl[y][x], v[x]));
                    v[f] = op.Divide(v[f], impl[y][f]);
                }
                lst.Add(v);
            }
            return lst.ToArray();
        }
        public struct Operator : IArithmeticOperator<ArrayMatrix<T, TOp>>
        {
            public ArrayMatrix<T, TOp> MultiplyIdentity => Identity;

            [凾(256)]
            public ArrayMatrix<T, TOp> Add(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => x + y;
            [凾(256)]
            public ArrayMatrix<T, TOp> Subtract(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => x - y;
            [凾(256)]
            public ArrayMatrix<T, TOp> Multiply(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => x * y;
            [凾(256)]
            public ArrayMatrix<T, TOp> Minus(ArrayMatrix<T, TOp> x) => -x;

            [凾(256)]
            public ArrayMatrix<T, TOp> Increment(ArrayMatrix<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public ArrayMatrix<T, TOp> Decrement(ArrayMatrix<T, TOp> x) => throw new NotSupportedException();
            [凾(256)]
            public ArrayMatrix<T, TOp> Divide(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => throw new NotSupportedException();
            [凾(256)]
            public ArrayMatrix<T, TOp> Modulo(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y) => throw new NotSupportedException();
        }
    }
    internal enum ArrayMatrixKind
    {
        Zero,
        Identity,
        Normal,
    }
}
