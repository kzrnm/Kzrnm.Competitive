using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
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

        public bool this[int row, int col]{ [凾(256)] get => _v[row][col]; }
        [凾(256)] public BitArray RowUnsafe(int i) => _v[i];

        static BitMatrix ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind == Kd.Zero;
        [凾(256)]
        static BitArray[] NormalZeroMatrix(int row, int col)
        {
            var arr = new BitArray[row];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(col);
            return arr;
        }
        [凾(256)]
        static BitArray[] CloneArray(BitArray[] arr)
        {
            var res = new BitArray[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                res[i] = new BitArray(arr[i]);
            return res;
        }

        [凾(256)]
        BitMatrix AddIdentity()
        {
            var arr = CloneArray(_v);
            for (int i = 0; i < arr.Length; i++)
                arr[i][i] = !arr[i][i];
            return new BitMatrix(arr);
        }
        [凾(256)]
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
        [凾(256)]
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
        [凾(256)]
        public static BitMatrix operator -(BitMatrix x)
        {
            var val = x._v;
            var arr = new BitArray[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(val[i]).Not();
            return new BitMatrix(arr);
        }
        [凾(256)] public static BitMatrix operator ~(BitMatrix x) => -x;

        [凾(256)] public static BitMatrix operator -(BitMatrix x, BitMatrix y) => x + y;
        [凾(256)] public static BitMatrix operator ^(BitMatrix x, BitMatrix y) => x + y;

        [凾(256)]
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
        [凾(256)]
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
        [凾(256)] public static BitArray operator *(BitMatrix mat, bool[] vector) => mat.Multiply(new BitArray(vector));

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)] public static BitArray operator *(BitMatrix mat, BitArray vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)]
        public BitArray Multiply(BitArray vector)
        {
            var val = _v;
            var b = new BitArray(val.Length);
            var a = b.GetArray();
            for (int i = 0; i < val.Length;)
            {
                ref var t = ref a[i >> 5];
                for (int j = 0; i < val.Length && j < 32; j++, i++)
                {
                    t |= (uint)(new BitArray(val[i]).And(vector).PopCount() & 1) << j;
                }
            }
            return b;
        }


        /// <summary>
        /// 逆行列を掃き出し法で求める。求められなければ零行列を返す。
        /// </summary>
        [凾(256)]
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
        [凾(256)]
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
        [凾(256)]
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
        [凾(256)]
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
        [凾(256)]
        public BitArray[] LinearSystem(BitArray vector)
        {
            Contract.Assert(_v.Length == vector.Length);
            var impl = new BitArray[_v.Length];
            for (int i = 0; i < impl.Length; i++)
            {
                impl[i] = new(_v[i]);
                ++impl[i].Length;
                impl[i][^1] = vector[i];
            }
            return LinearSystemImpl(impl);
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
        public BitArray[] LinearSystem(ReadOnlySpan<bool> vector)
        {
            Contract.Assert(_v.Length == vector.Length);
            var impl = new BitArray[_v.Length];
            for (int i = 0; i < impl.Length; i++)
            {
                impl[i] = new(_v[i]);
                ++impl[i].Length;
                impl[i][^1] = vector[i];
            }
            return LinearSystemImpl(impl);
        }
        [凾(256)]
        BitArray[] LinearSystemImpl(BitArray[] impl)
        {
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
                    v[f] = impl[y][^1] != AndPopCnt(v, impl[y], f + 1, w);
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
                    CheckAndPopCnt(v, impl[y], f + 1, w);
                    v[f] ^= AndPopCnt(v, impl[y], f + 1, w);
                }
                lst.Add(v);
            }
            return lst.ToArray();
        }

        [SourceExpander.NotEmbeddingSource]
        [Conditional("DEBUG")]
        static void CheckAndPopCnt(BitArray a, BitArray b, int l, int r)
        {
            if (AndPopCnt(a, b, l, r) != Naive(a, b, l, r))
            {
                AndPopCnt(a, b, l, r);
                Debug.Fail("ナイーブ解と異なる");
            }
            bool Naive(BitArray a, BitArray b, int l, int r)
            {
                var rt = false;
                for (int x = l; x < r; x++)
                    rt ^= a[x] && b[x];
                return rt;
            }
        }

        [凾(256)]
        static bool AndPopCnt(BitArray a, BitArray b, int l, int r)
        {
            //var rt = false;
            //for (int x = l; x < r; x++)
            //    rt ^= a[x] && b[x];
            //return rt;
            if (l >= r) return false;

            var u = a.GetArray();
            var v = b.GetArray();

            var smask = ~((1u << (l & 31)) - 1);
            var s = l >> 5;
            var tmask = (1u << (r & 31)) - 1;
            if (tmask == 0) --tmask;
            var t = --r >> 5;
            if (s == t)
            {
                return (BitOperations.PopCount(u[s] & v[s] & smask & tmask) & 1) != 0;
            }
            var c = u[s] & v[s] & smask;
            c ^= u[t] & v[t] & tmask;

            for (int i = t - 1; i > s; i--)
                c ^= u[i] & v[i];

            return (BitOperations.PopCount(c) & 1) != 0;
        }

        /// <summary>
        /// 行列式を求めます。
        /// </summary>
        [凾(256)]
        public bool Determinant()
        {
            if (_v.Length == 0) return true;
            Contract.Assert(_v.Length == _v[0].Length);
            var arr = _v.Select(v => new BitArray(v)).ToArray();

            //上三角行列
            for (int i = 0; i < arr.Length; i++)
            {
                if (!arr[i][i])
                {
                    if (!SearchNonZero(arr, i, i))
                        return false;
                }
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j][i])
                        arr[j].Xor(arr[i]);
                }
            }
            return true;
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is BitMatrix x && Equals(x);
        [凾(256)]
        public bool Equals(BitMatrix other) =>
            kind == other.kind && (kind != Kd.Normal || Equals(_v, other._v));
        [凾(256)]
        static bool Equals(BitArray[] a, BitArray[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = a.Length - 1; i >= 0; i--)
                if (!a[i].SequenceEqual(b[i]))
                    return false;
            return true;
        }
        [凾(256)]
        public override int GetHashCode()
        {
            HashCode hs = new();
            hs.Add(kind);
            if (kind == Kd.Normal)
            {
                var len = Width >> 5;
                var ex = (1u << (Width & 31)) - 1;
                hs.Add(Height);
                hs.Add(Width);
                foreach (var row in _v)
                {
                    var a = row.GetArray();
                    hs.Add(ex == 0 ? 0 : a[len] & ex);
                    hs.AddBytes(MemoryMarshal.AsBytes(a.AsSpan(0, len)));
                }
            }
            return hs.ToHashCode();
        }
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
        {
            var arr = new BitArray[rows.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = BinaryParser.ParseBitArray(rows[i]);
                if (i > 0 && arr[i].Length != arr[i - 1].Length)
                    throw new FormatException("Row length are diffrent.");
            }
            return new(arr);
        }
        /// <summary>
        /// 0,1 で表された行列をパースします。
        /// </summary>
        [凾(256)]
        public static BitMatrix Parse(string[] rows)
        {
            var arr = new BitArray[rows.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = BinaryParser.ParseBitArray(rows[i]);
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
