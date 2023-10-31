using AtCoder.Internal;
using AtCoder.Operators;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    public readonly struct ArrayMatrix<T, TOp> : IEquatable<ArrayMatrix<T, TOp>>
        where T : IEquatable<T>
        where TOp : struct, IArithmeticOperator<T>
    {
        public T this[int row, int col] => Value[Index(row, col)];
        [凾(256)] int Index(int row, int col) => row * Width + col;

        private static TOp op = default;
        public readonly int Height, Width;
        internal readonly T[] Value;
        public T[][] ToArray()
        {
            var arr = new T[Height][];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = Value.AsSpan(i * Width, Width).ToArray();
            return arr;
        }

        public static readonly ArrayMatrix<T, TOp> Zero = new ArrayMatrix<T, TOp>(Kd.Zero);
        public static readonly ArrayMatrix<T, TOp> Identity = new ArrayMatrix<T, TOp>(Kd.Identity);
        internal readonly Kd kind;
        internal ArrayMatrix(Kd kind)
        {
            this.kind = kind;
            Value = null;
            Height = 0;
            Width = 0;
        }

        public ArrayMatrix(T[] value, int height, int width)
        {
            kind = Kd.Normal;
            Height = height;
            Width = width;
            Value = value;
        }
        public ArrayMatrix(ReadOnlySpan<T> span, int height, int width) : this(span.ToArray(), height, width) { }
        public ArrayMatrix(T[][] m)
        {
            kind = Kd.Normal;
            var height = m.Length;
            var width = m[0].Length;
            kind = Kd.Normal;
            Height = height;
            Width = width;
            var val = new T[height * width];
            for (int i = 0; i < m.Length; i++)
            {
                Contract.Assert(m[i].Length == width);
                m[i].AsSpan().CopyTo(val.AsSpan(i * width));
            }
            Value = val;
        }
        public ArrayMatrix(T[,] m)
        {
            var height = m.GetLength(0);
            var width = m.GetLength(1);
            kind = Kd.Normal;
            Height = height;
            Width = width;
            Value = MemoryMarshal.CreateReadOnlySpan(ref m[0, 0], m.Length).ToArray();
        }

        private static ArrayMatrix<T, TOp> ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;
        [凾(256)] T[] CloneArray() => (T[])Value.Clone();

        private ArrayMatrix<T, TOp> AddIdentity()
        {
            var arr = CloneArray();
            for (int i = Math.Min(Height, Width) - 1; i >= 0; i--)
                arr[Index(i, i)] = op.Add(arr[Index(i, i)], op.MultiplyIdentity);
            return new ArrayMatrix<T, TOp>(arr, Height, Width);
        }
        [凾(256)]
        private ArrayMatrix<T, TOp> Add(ArrayMatrix<T, TOp> other)
        {
            Contract.Assert(Height == other.Height && Width == other.Width);
            var otherArr = other.Value;
            var arr = CloneArray();
            for (int i = 0; i < arr.Length; i++)
                arr[i] = op.Add(arr[i], otherArr[i]);
            return new ArrayMatrix<T, TOp>(arr, Height, Width);
        }
        [凾(256)]
        public static ArrayMatrix<T, TOp> operator +(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => y,
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => x,
                    Kd.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }
        [凾(256)]
        private ArrayMatrix<T, TOp> SubtractIdentity()
        {
            var arr = CloneArray();
            for (int i = Math.Min(Height, Width) - 1; i >= 0; i--)
                arr[Index(i, i)] = op.Subtract(arr[Index(i, i)], op.MultiplyIdentity);
            return new ArrayMatrix<T, TOp>(arr, Height, Width);
        }
        [凾(256)]
        private ArrayMatrix<T, TOp> Subtract(ArrayMatrix<T, TOp> other)
        {
            Contract.Assert(Height == other.Height && Width == other.Width);
            var otherArr = other.Value;
            var arr = CloneArray();
            for (int i = 0; i < arr.Length; i++)
                arr[i] = op.Subtract(arr[i], otherArr[i]);
            return new ArrayMatrix<T, TOp>(arr, Height, Width);
        }
        [凾(256)]
        public static ArrayMatrix<T, TOp> operator -(ArrayMatrix<T, TOp> x)
        {
            var arr = x.CloneArray();
            for (int i = 0; i < arr.Length; i++)
                arr[i] = op.Minus(arr[i]);
            return new ArrayMatrix<T, TOp>(arr, x.Height, x.Width);
        }

        [凾(256)]
        public static ArrayMatrix<T, TOp> operator -(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => -y,
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => (-y).AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => x,
                    Kd.Identity => x.SubtractIdentity(),
                    _ => x.Subtract(y),
                },
            };
        }
        [凾(256)]
        private ArrayMatrix<T, TOp> Multiply(ArrayMatrix<T, TOp> other)
        {
            var rh = Height;
            var rw = other.Width;
            var mid = Width;
            var res = new T[rh * rw];
            Contract.Assert(Width == other.Height);
            for (int i = 0; i < rh; i++)
                for (var k = 0; k < mid; k++)
                    for (int j = 0; j < rw; j++)
                        res[i * rw + j] = op.Add(res[i * rw + j], op.Multiply(this[i, k], other[k, j]));
            return new ArrayMatrix<T, TOp>(res, rh, rw);
        }
        [凾(256)]
        public static ArrayMatrix<T, TOp> operator *(ArrayMatrix<T, TOp> x, ArrayMatrix<T, TOp> y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new ArrayMatrix<T, TOp>(new T[y.Value.Length], y.Height, y.Width),
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
                    Kd.Zero => new ArrayMatrix<T, TOp>(new T[x.Value.Length], x.Height, x.Width),
                    Kd.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        [凾(256)]
        private ArrayMatrix<T, TOp> MultiplyScalar(T scalar)
        {
            var arr = Value;
            var res = new T[arr.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = op.Multiply(scalar, arr[i]);
            return new ArrayMatrix<T, TOp>(res, Height, Width);
        }
        [凾(256)]
        public static ArrayMatrix<T, TOp> operator *(ArrayMatrix<T, TOp> m, T x)
        {
            return m.kind switch
            {
                Kd.Normal => m.MultiplyScalar(x),
                _ => ThrowNotSupportResponse(),
            };
        }

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)] public static T[] operator *(ArrayMatrix<T, TOp> mat, T[] vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)]
        public T[] Multiply(T[] vector)
        {
            Contract.Assert(Width == vector.Length);
            return (this * new ArrayMatrix<T, TOp>(vector, vector.Length, 1)).Value;
        }

        /// <summary>
        /// 行列式を求める。
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            Contract.Assert(Height == Width);
            return DeterminantImpl(ToArray());
        }

        /// <summary>
        /// <c>(<paramref name="i"/>, <paramref name="j"/>)</c> 余因子を求める。
        /// </summary>
        [凾(256)]
        public T Cofactor(int i, int j)
        {
            Contract.Assert(Height == Width);
            var arr = new T[Height - 1][];
            for (int h = 0; h < arr.Length; h++)
            {
                var p = h;
                if (h >= i) ++p;
                var row = Value.AsSpan(p * Width, Width);
                arr[h] = new T[Width - 1];
                row[..j].CopyTo(arr[h]);
                row[(j + 1)..].CopyTo(arr[h].AsSpan(j));
            }
            var rt = DeterminantImpl(arr);
            if (((i ^ j) & 1) != 0)
                rt = op.Minus(rt);
            return rt;
        }

        [凾(256)]
        internal static T DeterminantImpl(T[][] arr)
        {
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
        [凾(256)] public ArrayMatrix<T, TOp> Pow(long y) => MathLibGeneric.Pow<ArrayMatrix<T, TOp>, Operator>(this, y);

        /// <summary>
        /// 逆行列を掃き出し法で求める。求められなければ零行列を返す。
        /// </summary>
        [凾(256)]
        public ArrayMatrix<T, TOp> Inv()
        {
            Contract.Assert(Height == Width);
            var len1 = Width * 2;
            var arr = new T[Height][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new T[len1];
                Value.AsSpan(i * Width, Width).CopyTo(arr[i]);
                arr[i][Width + i] = op.MultiplyIdentity;
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
        [凾(256)]
        public ArrayMatrix<T, TOp> GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = ToArray();
            GaussianEliminationImpl(arr, isReduced);
            return new ArrayMatrix<T, TOp>(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>0ではない列のインデックス</returns>
        [凾(256)]
        private static List<int> GaussianEliminationImpl(T[][] arr, bool isReduced)
        {
            var idx = new List<int>(arr.Length);
            int r = 0;
            int width = arr[0].Length;
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
                for (int y = isReduced ? 0 : r + 1; y < arr.Length; y++)
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
        [凾(256)]
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
        [凾(256)]
        public T[][] LinearSystem(T[] vector)
        {
            Contract.Assert(Height == vector.Length);
            var impl = new T[Height][];
            for (int i = 0; i < impl.Length; i++)
            {
                impl[i] = new T[Width + 1];
                Value.AsSpan(i * Width, Width).CopyTo(impl[i]);
                impl[i][^1] = vector[i];
            }
            var idxs = GaussianEliminationImpl(impl, false).AsSpan();
            var r = idxs.Length;
            int w = Width;

            // 解があるかチェック
            // a×0+b×0+c×0..+z×0≠0 になっていたら解無し
            for (int i = r; i < impl.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(impl[i][^1], default))
                    return Array.Empty<T[]>();
            }
            if (idxs.IsEmpty)
            {
                var eres = new T[w + 1][];
                eres[0] = new T[w];
                for (int i = 1; i < eres.Length; i++)
                {
                    eres[i] = new T[w];
                    eres[i][i - 1] = op.MultiplyIdentity;
                }
                return eres;
            }
            if (idxs[^1] == w)
                return Array.Empty<T[]>();

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

        [凾(256)]
        public override bool Equals(object obj) => obj is ArrayMatrix<T, TOp> x && Equals(x);
        [凾(256)]
        public bool Equals(ArrayMatrix<T, TOp> other)
            => kind == other.kind && (kind != Kd.Normal || Height == other.Height && Value.AsSpan().SequenceEqual(other.Value));
        [凾(256)]
        public override int GetHashCode() => kind switch
        {
            Kd.Normal => HashCode.Combine(kind, Value.Length, Value[0], Value[1], Value[^2], Value[^1]),
            _ => HashCode.Combine(kind),
        };
        [凾(256)]
        public static bool operator ==(ArrayMatrix<T, TOp> left, ArrayMatrix<T, TOp> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(ArrayMatrix<T, TOp> left, ArrayMatrix<T, TOp> right) => !(left == right);
        public readonly struct Operator : IArithmeticOperator<ArrayMatrix<T, TOp>>
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
    public static class __ArrayMatrix_WriteGrid
    {
        public static void WriteGrid<T, TOp>(this Utf8ConsoleWriter cw, ArrayMatrix<T, TOp> m)
            where T : IEquatable<T>
            where TOp : struct, IArithmeticOperator<T>
        {
            var h = m.Height;
            var w = m.Width;
            var v = m.Value;
            for (int i = 0; i < h; i++)
                cw.WriteLineJoin(v.AsSpan(i * w, w));
        }
    }
}
