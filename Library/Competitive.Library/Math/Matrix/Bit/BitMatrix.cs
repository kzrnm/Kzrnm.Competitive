using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BitArray = System.Collections.BitArray;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    /// <summary>
    /// Mod2 の行列。+: xor *: and
    /// </summary>
    [DebuggerTypeProxy(typeof(DebugView))]
    public readonly struct BitMatrix
        : Internal.IMatrix<BitMatrix>
    {
        public bool this[int row, int col] => _v[row][col];
        public readonly BitArray[] _v;
        public int Height => _v.Length;
        public int Width => _v[0].Length;

        public static readonly BitMatrix Zero = new BitMatrix(Kd.Zero);
        public static readonly BitMatrix Identity = new BitMatrix(Kd.Identity);
        public static BitMatrix AdditiveIdentity => Zero;
        public static BitMatrix MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal BitMatrix(Kd kind)
        {
            this.kind = kind;
            _v = null;
        }

        public BitMatrix(BitArray[] value)
        {
            _v = value;
            kind = Kd.Normal;
        }
        public BitMatrix(bool[][] value) : this(value.Select(a => new BitArray(a)).ToArray()) { }

        static BitMatrix ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;
        static BitArray[] NormalZeroMatrix(int row, int col)
        {
            var arr = new BitArray[row];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(col);
            return arr;
        }
        static BitArray[] CloneArray(BitArray[] arr)
        {
            var res = new BitArray[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                res[i] = new BitArray(arr[i]);
            return res;
        }

        BitMatrix AddIdentity()
        {
            var arr = CloneArray(_v);
            for (int i = 0; i < arr.Length; i++)
                arr[i][i] = !arr[i][i];
            return new BitMatrix(arr);
        }
        BitMatrix Add(BitMatrix other)
        {
            Contract.Assert(_v.Length == other._v.Length);
            Contract.Assert(_v[0].Length == other._v[0].Length);
            var otherArr = other._v;
            var val = _v;
            var arr = new BitArray[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(val[i]).Xor(otherArr[i]);

            return new BitMatrix(arr);
        }
        public static BitMatrix operator +(BitMatrix x, BitMatrix y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => new BitMatrix(CloneArray(y._v)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new BitMatrix(CloneArray(x._v)),
                    Kd.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }

        [凾(256)] public static BitMatrix operator +(BitMatrix x) => x;
        public static BitMatrix operator -(BitMatrix x)
        {
            var val = x._v;
            var arr = new BitArray[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(val[i]).Not();
            return new BitMatrix(arr);
        }
        public static BitMatrix operator ~(BitMatrix x) => -x;

        public static BitMatrix operator -(BitMatrix x, BitMatrix y) => x + y;
        public static BitMatrix operator ^(BitMatrix x, BitMatrix y) => x + y;

        BitMatrix Multiply(BitMatrix other)
        {
            var val = _v;
            var otherArr = other._v;
            Contract.Assert(val[0].Length == otherArr.Length);
            var res = new BitArray[val.Length];
            for (int i = 0; i < res.Length; i++)
            {
                var row = res[i] = new BitArray(otherArr[0].Length);
                for (int j = 0; j < val[i].Length; j++)
                    if (val[i][j])
                        row.Xor(otherArr[j]);
            }
            return new BitMatrix(res);
        }
        public static BitMatrix operator *(BitMatrix x, BitMatrix y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new BitMatrix(NormalZeroMatrix(y._v.Length, y._v[0].Length)),
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
                    Kd.Zero => new BitMatrix(NormalZeroMatrix(x._v.Length, x._v[0].Length)),
                    Kd.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static BitArray operator *(BitMatrix mat, bool[] vector) => mat.Multiply(new BitArray(vector));

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static BitArray operator *(BitMatrix mat, BitArray vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public BitArray Multiply(BitArray vector)
        {
            var val = _v;
            var res = new bool[val.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = (new BitArray(val[i]).And(vector).PopCount() & 1) != 0;

            return new BitArray(res);
        }


        /// <summary>
        /// 逆行列を掃き出し法で求める。求められなければ零行列を返す。
        /// </summary>
        public BitMatrix Inv()
        {
            Contract.Assert(_v.Length == _v[0].Length);
            var orig = _v;
            var arr = new BitArray[orig.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                var row = arr[i] = new BitArray(orig[i]);
                row.Length *= 2;
                row[arr.Length + i] = true;
            }
            GaussianEliminationImpl(arr, true);

            var ix = arr.Length - 1;
            if (!arr[ix][ix])
                return Zero;

            var res = new BitArray[arr.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new BitArray(res.Length);
                for (int j = 0; j < res.Length; j++)
                {
                    res[i][j] = arr[i][res.Length + j];
                }
            }
            return new BitMatrix(res);
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        public BitMatrix GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = CloneArray(_v);
            GaussianEliminationImpl(arr, isReduced);
            return new BitMatrix(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>0ではない列のインデックス</returns>
        static List<int> GaussianEliminationImpl(BitArray[] arr, bool isReduced)
        {
            var idx = new List<int>(arr.Length);
            int r = 0;
            int width = arr[0].Length;
            for (int x = 0; x < width && r < arr.Length; x++)
            {
                if (!SearchNonZero(arr, r, x))
                    continue;
                var arrR = arr[r];
                for (int y = isReduced ? 0 : r + 1; y < arr.Length; y++)
                {
                    var arrY = arr[y];
                    if (y == r || !arrY[x])
                        continue;
                    arrY.Xor(arrR);
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
        static bool SearchNonZero(BitArray[] mat, int r, int x)
        {
            for (int y = r; y < mat.Length; y++)
                if (mat[y][x])
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
        public BitArray[] LinearSystem(bool[] vector)
        {
            Contract.Assert(_v.Length == vector.Length);
            var impl = _v.Zip(vector, (row, v) =>
            {
                row = new BitArray(row);
                ++row.Length;
                row[^1] = v;
                return row;
            }).ToArray();
            var idxs = GaussianEliminationImpl(impl, false).AsSpan();
            var r = idxs.Length;
            int w = _v[0].Length;

            // 解があるかチェック
            // a×0+b×0+c×0..+z×0≠0 になっていたら解無し
            for (int i = r; i < impl.Length; i++)
            {
                if (impl[i][^1])
                    return Array.Empty<BitArray>();
            }
            if (idxs.IsEmpty)
            {
                var eres = new BitArray[w + 1];
                eres[0] = new BitArray(w);
                for (int i = 1; i < eres.Length; i++)
                {
                    eres[i] = new BitArray(w);
                    eres[i][i - 1] = true;
                }
                return eres;
            }
            if (idxs[^1] == w)
                return Array.Empty<BitArray>();

            var used = new HashSet<int>(Enumerable.Range(0, w));
            var lst = new List<BitArray>(w);
            {
                var v = new BitArray(w);
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    used.Remove(f);
                    v[f] = impl[y][^1];
                    for (int x = f + 1; x < w; x++)
                        v[f] ^= v[x] && impl[y][x];
                }
                lst.Add(v);
            }

            foreach (var s in used)
            {
                var v = new BitArray(w);
                v[s] = true;
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    for (int x = f + 1; x < w; x++)
                        v[f] ^= v[x] && impl[y][x];
                }
                lst.Add(v);
            }
            return lst.ToArray();
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is BitMatrix x && Equals(x);
        [凾(256)]
        public bool Equals(BitMatrix other) =>
            kind == other.kind && (kind != Kd.Normal || EqualsMat(_v, other._v));
        static bool EqualsMat(BitArray[] a, BitArray[] b)
        {
            if (a.Length != b.Length) return false;
            var array = new int[(a[0].Length + 31) / 32];
            var empty = new int[(a[0].Length + 31) / 32];
            for (int i = 0; i < a.Length; i++)
            {
                new BitArray(a[i]).Xor(b[i]).CopyTo(array, 0);
                if (!array.AsSpan().SequenceEqual(empty)) return false;
            }
            return true;
        }
        [凾(256)]
        public override int GetHashCode() => kind switch
        {
            Kd.Normal => HashCode.Combine(kind, _v.Length, _v[0][0], _v[0][^1], _v[^1][0], _v[^1][^1]),
            _ => HashCode.Combine(kind),
        };
        [凾(256)]
        public static bool operator ==(BitMatrix left, BitMatrix right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(BitMatrix left, BitMatrix right) => !(left == right);

        public override string ToString()
        {
            if (kind != Kd.Normal) return kind.ToString();
            var sb = new StringBuilder(_v.Length * (_v[0].Length + 2));
            foreach (var row in _v)
            {
                var chr = new char[row.Length];
                for (int j = 0; j < row.Length; j++)
                    chr[j] = row[j] ? '1' : '0';
                sb.Append(chr).Append('\n');
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// 0,1 で表された行列をパースします。
        /// </summary>
        [凾(256)]
        public static BitMatrix Parse(Asciis[] rows)
            => Parse((IList)rows);
        /// <summary>
        /// 0,1 で表された行列をパースします。
        /// </summary>
        [凾(256)]
        public static BitMatrix Parse(string[] rows)
            => Parse((IList)rows);
        /// <summary>
        /// 0,1 で表された行列をパースします。
        /// </summary>
        static BitMatrix Parse(IList rows)
        {
            var arr = new BitArray[rows.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rows[i] switch
                {
                    Asciis a => BinaryParser.ParseBitArray(a),
                    string s => BinaryParser.ParseBitArray(s),
                    _ => throw new InvalidCastException(),
                };
                if (i > 0 && arr[i].Length != arr[i - 1].Length)
                    throw new FormatException("Row length are diffrent.");
            }
            return new(arr);
        }

        [SourceExpander.NotEmbeddingSource]
        readonly record struct DebugView(
            [property: DebuggerBrowsable(DebuggerBrowsableState.Never)] BitMatrix Matrix)
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public BitArrayDebug Items => new(Matrix._v);
        }
    }
}
