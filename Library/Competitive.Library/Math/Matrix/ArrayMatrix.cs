using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    [DebuggerTypeProxy(typeof(ArrayMatrix<>.DebugView))]
    public readonly struct ArrayMatrix<T> : Internal.IArrayMatrix<ArrayMatrix<T>, T>
        , IMultiplyOperators<ArrayMatrix<T>, T, ArrayMatrix<T>>
        where T : INumberBase<T>
    {
        public T this[int row, int col] => _v[Index(row, col)];
        [凾(256)] int Index(int row, int col) => row * _w + col;

        public int Height => _h;
        public int Width => _w;
        private readonly int _h, _w;
        internal readonly T[] _v;
        public ReadOnlySpan<T> Value => _v;
        public T[][] ToArray()
        {
            var arr = new T[_h][];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = _v.AsSpan(i * _w, _w).ToArray();
            return arr;
        }

        public static readonly ArrayMatrix<T> Zero = new(Kd.Zero);
        public static readonly ArrayMatrix<T> Identity = new(Kd.Identity);
        public static ArrayMatrix<T> AdditiveIdentity => Zero;
        public static ArrayMatrix<T> MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal ArrayMatrix(Kd kind)
        {
            this.kind = kind;
            _v = null;
        }

        public ArrayMatrix(T[] value, int height, int width)
        {
            Contract.Assert(value.Length == height * width, "value.Length が不正です。");
            kind = Kd.Normal;
            _h = height;
            _w = width;
            _v = value;
        }
        public ArrayMatrix(ReadOnlySpan<T> span, int height, int width) : this(span.ToArray(), height, width) { }
        public ArrayMatrix(T[][] m)
        {
            if (m.Length == 0)
            {
                kind = Kd.Normal;
                _v = Array.Empty<T>();
            }
            else
            {
                kind = Kd.Normal;
                var height = m.Length;
                var width = m[0].Length;
                kind = Kd.Normal;
                _h = height;
                _w = width;
                var val = new T[height * width];
                for (int i = 0; i < m.Length; i++)
                {
                    Contract.Assert(m[i].Length == width);
                    m[i].AsSpan().CopyTo(val.AsSpan(i * width));
                }
                _v = val;
            }
        }
        public ArrayMatrix(T[,] m)
        {
            var height = m.GetLength(0);
            var width = m.GetLength(1);
            kind = Kd.Normal;
            _h = height;
            _w = width;
            _v = MemoryMarshal.CreateReadOnlySpan(ref m[0, 0], m.Length).ToArray();
        }

        private static ArrayMatrix<T> ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 大きさ <paramref name="s"/> の単位行列を返します。
        /// </summary>
        [凾(256)]
        public static ArrayMatrix<T> NormalIdentity(int s)
        {
            var v = new T[s * s];
            for (int i = 0; i < v.Length; i += s + 1)
                v[i] = T.One;
            return new(v, s, s);
        }

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;
        [凾(256)] T[] CloneArray() => (T[])_v.Clone();

        private ArrayMatrix<T> AddIdentity()
        {
            var arr = CloneArray();
            for (int i = Math.Min(_h, _w) - 1; i >= 0; i--)
                arr[Index(i, i)] += T.MultiplicativeIdentity;
            return new(arr, _h, _w);
        }
        [凾(256)]
        private ArrayMatrix<T> Add(ArrayMatrix<T> other)
        {
            Contract.Assert(_h == other._h && _w == other._w);
            if (_v.Length == 0) return new();
            ref var op = ref other._v[0];
            var arr = CloneArray();
            for (int i = 0; i < arr.Length; i++)
                arr[i] += Unsafe.Add(ref op, i);

            return new(arr, _h, _w);
        }
        [凾(256)]
        public static ArrayMatrix<T> operator +(ArrayMatrix<T> x, ArrayMatrix<T> y)
        {
            return x.kind switch
            {
                Kd.Normal => y.kind switch
                {
                    Kd.Normal => x.Add(y),
                    Kd.Identity => x.AddIdentity(),
                    _ => x,
                },
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => y,
                },
                _ => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
            };
        }
        [凾(256)]
        private ArrayMatrix<T> SubtractIdentity()
        {
            var arr = CloneArray();
            for (int i = Math.Min(_h, _w) - 1; i >= 0; i--)
                arr[Index(i, i)] -= T.MultiplicativeIdentity;
            return new(arr, _h, _w);
        }
        [凾(256)]
        private ArrayMatrix<T> Subtract(ArrayMatrix<T> other)
        {
            Contract.Assert(_h == other._h && _w == other._w);
            if (_v.Length == 0) return new();
            ref var op = ref other._v[0];
            var arr = CloneArray();
            for (int i = 0; i < arr.Length; i++)
                arr[i] -= Unsafe.Add(ref op, i);
            return new(arr, _h, _w);
        }

        [凾(256)] public static ArrayMatrix<T> operator +(ArrayMatrix<T> x) => x;
        [凾(256)]
        public static ArrayMatrix<T> operator -(ArrayMatrix<T> x)
        {
            var arr = x.CloneArray();
            for (int i = 0; i < arr.Length; i++)
                arr[i] = -arr[i];
            return new(arr, x._h, x._w);
        }

        [凾(256)]
        public static ArrayMatrix<T> operator -(ArrayMatrix<T> x, ArrayMatrix<T> y)
        {
            return x.kind switch
            {
                Kd.Normal => y.kind switch
                {
                    Kd.Normal => x.Subtract(y),
                    Kd.Identity => x.SubtractIdentity(),
                    _ => x,
                },
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => -y,
                },
                _ => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => (-y).AddIdentity(),
                },
            };
        }
        [凾(256)]
        private ArrayMatrix<T> Multiply(ArrayMatrix<T> other)
        {
            var rh = _h;
            var rw = other._w;
            var mid = _w;
            Contract.Assert(_w == other._h);
            var res = new T[rh * rw];
            if (res.Length == 0) return new();

            ref var rp = ref res[0];
            ref var tp = ref _v[0];
            ref var op = ref other._v[0];

            for (int i = 0; i < rh; i++)
                for (int j = 0; j < rw; j++)
                {
                    ref var v = ref Unsafe.Add(ref rp, i * rw + j);
                    for (var k = 0; k < mid; k++)
                        v += Unsafe.Add(ref tp, i * mid + k) * Unsafe.Add(ref op, k * rw + j);
                }
            return new(res, rh, rw);
        }
        [凾(256)]
        public static ArrayMatrix<T> operator *(ArrayMatrix<T> x, ArrayMatrix<T> y)
        {
            return x.kind switch
            {
                Kd.Normal => y.kind switch
                {
                    Kd.Normal => x.Multiply(y),
                    Kd.Zero => new(new T[x._v.Length], x._h, x._w),
                    _ => x,
                },
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new(new T[y._v.Length], y._h, y._w),
                    _ => Zero,
                },
                _ => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => y,
                },
            };
        }
        [凾(256)]
        private ArrayMatrix<T> MultiplyScalar(T scalar)
        {
            var arr = _v;
            var res = new T[arr.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = scalar * arr[i];
            return new(res, _h, _w);
        }
        [凾(256)]
        public static ArrayMatrix<T> operator *(ArrayMatrix<T> m, T x)
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
        [凾(256)] public static T[] operator *(ArrayMatrix<T> mat, T[] vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)]
        public T[] Multiply(T[] vector)
        {
            Contract.Assert(_w == vector.Length);
            return (this * new ArrayMatrix<T>(vector, vector.Length, 1))._v;
        }

        /// <summary>
        /// 行列式を求める。
        /// </summary>
        [凾(256)]
        public T Determinant()
        {
            Contract.Assert(_h == _w);
            return DeterminantImpl(ToArray());
        }

        /// <summary>
        /// <c>(<paramref name="i"/>, <paramref name="j"/>)</c> 余因子を求める。
        /// </summary>
        [凾(256)]
        public T Cofactor(int i, int j)
        {
            Contract.Assert(_h == _w);
            var arr = new T[_h - 1][];
            for (int h = 0; h < arr.Length; h++)
            {
                var p = h;
                if (h >= i) ++p;
                var row = _v.AsSpan(p * _w, _w);
                arr[h] = new T[_w - 1];
                row[..j].CopyTo(arr[h]);
                row[(j + 1)..].CopyTo(arr[h].AsSpan(j));
            }
            var rt = DeterminantImpl(arr);
            if (((i ^ j) & 1) != 0)
                rt = -rt;
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
                    var c = arr[j][i] / t;
                    for (int k = 0; k < arr[i].Length; k++)
                        arr[j][k] -= c * arr[i][k];
                }
            }

            T det = T.MultiplicativeIdentity;
            if (swap)
                det = -det;
            for (int i = 0; i < arr.Length; i++)
                det *= arr[i][i];
            return det;
        }

        /// <summary>
        /// 逆行列を掃き出し法で求める。求められなければ零行列を返す。
        /// </summary>
        [凾(256)]
        public ArrayMatrix<T> Inv()
        {
            Contract.Assert(_h == _w);
            var len1 = _w * 2;
            var arr = new T[_h][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new T[len1];
                _v.AsSpan(i * _w, _w).CopyTo(arr[i]);
                arr[i][_w + i] = T.MultiplicativeIdentity;
            }
            GaussianEliminationImpl(arr, true);

            var ix = arr.Length - 1;
            if (EqualityComparer<T>.Default.Equals(arr[ix][ix], default))
                return Zero;

            var res = new T[arr.Length][];
            for (int i = 0; i < res.Length; i++)
                res[i] = arr[i][res.Length..];
            return new(res);
        }

        /// <summary>
        /// 転置行列を取得します。
        /// </summary>
        public ArrayMatrix<T> Transpose()
        {
            Contract.Assert(kind == Kd.Normal);
            var h = _h;
            var w = _w;
            var rt = new T[h * w];
            if (rt.Length == 0) return new();

            var v = _v;
            ref var rp = ref rt[0];
            for (int hh = 0; hh < h; hh++)
            {
                var f = hh * w;
                var s = v.AsSpan(f, w);
                for (int ww = 0; ww < s.Length; ww++)
                {
                    Unsafe.Add(ref rp, ww * h + hh) = s[ww];
                }
            }
            return new(rt, w, h);
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        [凾(256)]
        public ArrayMatrix<T> GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = ToArray();
            GaussianEliminationImpl(arr, isReduced);
            return new(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>(行列式, 0ではない列のインデックス)</returns>
        [凾(256)]
        //internal static (T det, List<int> idxs) GaussianEliminationImpl(T[][] arr, bool isReduced)
        internal static List<int> GaussianEliminationImpl(T[][] arr, bool isReduced)
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
                    var inv = T.MultiplicativeIdentity / arrR[x];
                    arrR[x] = T.MultiplicativeIdentity;
                    for (int i = x + 1; i < arrR.Length; i++)
                        arrR[i] *= inv;
                }
                for (int y = isReduced ? 0 : r + 1; y < arr.Length; y++)
                {
                    var arrY = arr[y];
                    if (y == r || EqualityComparer<T>.Default.Equals(arrY[x], default))
                        continue;
                    var freq = arrY[x];
                    for (int k = x + 1; k < arrY.Length; k++)
                        arrY[k] -= freq * arrR[k];
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
            Contract.Assert(_h == vector.Length);
            var impl = new T[_h][];
            for (int i = 0; i < impl.Length; i++)
            {
                impl[i] = new T[_w + 1];
                _v.AsSpan(i * _w, _w).CopyTo(impl[i]);
                impl[i][^1] = vector[i];
            }
            var idxs = GaussianEliminationImpl(impl, false).AsSpan();
            var r = idxs.Length;
            int w = _w;

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
                eres[0] = Enumerable.Repeat(T.AdditiveIdentity, w).ToArray();
                for (int i = 1; i < eres.Length; i++)
                {
                    eres[i] = Enumerable.Repeat(T.AdditiveIdentity, w).ToArray();
                    eres[i][i - 1] = T.MultiplicativeIdentity;
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
                        v[f] -= impl[y][x] * v[x];
                    v[f] /= impl[y][f];
                }
                lst.Add(v);
            }

            foreach (var s in used)
            {
                var v = new T[w];
                v[s] = T.MultiplicativeIdentity;
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    for (int x = f + 1; x < w; x++)
                        v[f] -= impl[y][x] * v[x];
                    v[f] /= impl[y][f];
                }
                lst.Add(v);
            }
            return lst.ToArray();
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is ArrayMatrix<T> x && Equals(x);
        [凾(256)]
        public bool Equals(ArrayMatrix<T> other)
            => kind == other.kind && (kind != Kd.Normal || _h == other._h && _v.AsSpan().SequenceEqual(other._v));
        [凾(256)]
        public override int GetHashCode() => kind switch
        {
            Kd.Normal => HashCode.Combine(kind, _v.Length, _v[0], _v[1], _v[^2], _v[^1]),
            _ => HashCode.Combine(kind),
        };
        [凾(256)]
        public static bool operator ==(ArrayMatrix<T> left, ArrayMatrix<T> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(ArrayMatrix<T> left, ArrayMatrix<T> right) => !(left == right);

#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        class DebugView
        {
            private readonly ArrayMatrix<T> m;
            public Kd Kind => m.kind;
            public DebugView(ArrayMatrix<T> matrix)
            {
                m = matrix;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Item[] Items
            {
                get
                {
                    if (m.kind != Kd.Normal) return null;
                    var a = m.ToArray();
                    return a.Select(r => new Item(r)).ToArray();
                }
            }

            internal readonly struct Item
            {
                [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
                public readonly T[] Row;
                public Item(T[] row)
                {
                    Row = row;
                }

                public override string ToString()
                    => Row.Length < 50
                    ? string.Join(' ', Row)
                    : (string.Join(' ', Row.Take(50)) + ",...");
            }
        }
    }
    public static class __ArrayMatrix_WriteGrid
    {
        public static void WriteGrid<T>(this Utf8ConsoleWriter cw, ArrayMatrix<T> m) where T : INumberBase<T>
        {
            var h = m.Height;
            var w = m.Width;
            var v = m._v;
            for (int i = 0; i < h; i++)
                cw.WriteLineJoin(v.AsSpan(i * w, w));
        }
    }
}
