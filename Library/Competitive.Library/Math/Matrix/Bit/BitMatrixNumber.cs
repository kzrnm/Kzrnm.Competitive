using AtCoder.Internal;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    /// <summary>
    /// Mod2 の行列。+: xor *: and
    /// </summary>
    [DebuggerTypeProxy(typeof(BitMatrix<>.DebugView))]
    public readonly struct BitMatrix<T> : Internal.IMatrix<BitMatrix<T>>
        where T : unmanaged, IBinaryInteger<T>
    {
        public bool this[int row, int col] => (uint.CreateTruncating(_v[row] >> col) & 1) != 0;
        public readonly T[] _v;
        public int Height => _v.Length;
        public int Width => Unsafe.SizeOf<T>() * 8;

        public static readonly BitMatrix<T> Zero = new BitMatrix<T>(Kd.Zero);
        public static readonly BitMatrix<T> Identity = new BitMatrix<T>(Kd.Identity);
        public static BitMatrix<T> AdditiveIdentity => Zero;
        public static BitMatrix<T> MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal BitMatrix(Kd kind)
        {
            this.kind = kind;
            _v = null;
        }

        public BitMatrix(T[] value)
        {
            _v = value;
            kind = Kd.Normal;
        }
        public BitMatrix(bool[][] value) : this(value.Select(BoolArrayToNumber).ToArray()) { }
        static T BoolArrayToNumber(bool[] arr)
        {
            var res = T.Zero;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i])
                    res |= T.One << i;
            return res;
        }
        static BitMatrix<T> ThrowNotSupportResponse() => throw new NotSupportedException();
        [凾(256)]
        static int BitSize() => Unsafe.SizeOf<T>() << 3;


        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;

        static T[] CloneArray(T[] arr) => arr.ToArray();

        BitMatrix<T> AddIdentity()
        {
            var arr = CloneArray(_v);
            for (int i = 0; i < arr.Length; i++)
                arr[i] ^= T.One << i;
            return new BitMatrix<T>(arr);
        }
        BitMatrix<T> Add(BitMatrix<T> other)
        {
            Contract.Assert(_v.Length == other._v.Length);
            var otherArr = other._v;
            var val = _v;
            var arr = new T[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = val[i] ^ otherArr[i];

            return new BitMatrix<T>(arr);
        }
        public static BitMatrix<T> operator +(BitMatrix<T> x, BitMatrix<T> y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => new BitMatrix<T>(CloneArray(y._v)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new BitMatrix<T>(CloneArray(x._v)),
                    Kd.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }

        [凾(256)] public static BitMatrix<T> operator +(BitMatrix<T> x) => x;
        public static BitMatrix<T> operator -(BitMatrix<T> x)
        {
            var val = x._v;
            var arr = new T[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = ~val[i];
            return new BitMatrix<T>(arr);
        }
        public static BitMatrix<T> operator ~(BitMatrix<T> x) => -x;

        public static BitMatrix<T> operator -(BitMatrix<T> x, BitMatrix<T> y) => x + y;
        public static BitMatrix<T> operator ^(BitMatrix<T> x, BitMatrix<T> y) => x + y;

        BitMatrix<T> Multiply(BitMatrix<T> other)
        {
            var val = _v;
            var otherArr = other._v;
            Contract.Assert(otherArr.Length <= BitSize());
            var res = new T[val.Length];
            for (int i = 0; i < res.Length; i++)
            {
                ref var row = ref res[i];
                for (int j = 0; j < otherArr.Length; j++)
                    if ((uint.CreateTruncating(val[i] >> j) & 1) != 0)
                        row ^= otherArr[^(j + 1)];
            }
            return new BitMatrix<T>(res);
        }
        public static BitMatrix<T> operator *(BitMatrix<T> x, BitMatrix<T> y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new BitMatrix<T>(new T[y._v.Length]),
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
                    Kd.Zero => new BitMatrix<T>(new T[x._v.Length]),
                    Kd.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static T operator *(BitMatrix<T> mat, bool[] vector) => mat.Multiply(BoolArrayToNumber(vector));

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static T operator *(BitMatrix<T> mat, T vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public T Multiply(T vector)
        {
            var val = _v;
            var res = default(T);
            for (int i = 0; i < val.Length; i++)
                res |= (T.PopCount(val[i] & vector) & T.One) << i;

            return res;
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        public BitMatrix<T> GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = CloneArray(_v);
            GaussianEliminationImpl(arr, isReduced);
            return new BitMatrix<T>(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>0ではない列のインデックス</returns>
        static List<int> GaussianEliminationImpl(T[] arr, bool isReduced)
        {
            var idx = new List<int>(arr.Length);
            int r = 0;
            int width = BitSize();
            for (int x = 0; x < width && r < arr.Length; x++)
            {
                if (!SearchNonZero(arr, r, x))
                    continue;
                var arrR = arr[r];
                for (int y = isReduced ? 0 : r + 1; y < arr.Length; y++)
                {
                    var arrY = arr[y];
                    if (y == r || (uint.CreateTruncating(arrY >> x) & 1) == 0)
                        continue;
                    arr[y] ^= arrR;
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
        static bool SearchNonZero(T[] mat, int r, int x)
        {
            for (int y = r; y < mat.Length; y++)
                if ((uint.CreateTruncating(mat[y] >> x) & 1) != 0)
                {
                    (mat[r], mat[y]) = (mat[y], mat[r]);
                    return true;
                }
            return false;
        }
        /// <summary>
        /// <para>連立一次方程式 <see langword="this"/>・X=<paramref name="vector"/> を満たす ベクトル X を求める。</para>
        /// <para>最上位ビットは計算で使うのでベクトルは <see cref="Unsafe.SizeOf{T}"/> より小さくなければならない。</para>
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item><description>最初の配列: 特殊解</description></item>
        /// <item><description>2番目以降の配列: 解空間の基底ベクトル</description></item>
        /// <item><description>ただし解無しのときは空配列を返す</description></item>
        /// </list>
        /// </returns>
        public T[] LinearSystem(T vector)
        {
            int log2 = 0;
            foreach (var v in _v)
            {
                var s = int.CreateTruncating(T.Log2(v));
                if (log2 < s) log2 = s;
            }
            {
                var s = int.CreateTruncating(T.Log2(vector));
                if (log2 < s) log2 = s;
            }
            return LinearSystem(vector, log2 + 1);
        }

        /// <summary>
        /// <para>連立一次方程式 <see langword="this"/>・X=<paramref name="vector"/> を満たす ベクトル X を求める。</para>
        /// <para>最上位ビットは計算で使うのでベクトルは <see cref="Unsafe.SizeOf{T}"/> より小さくなければならない。</para>
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item><description>最初の配列: 特殊解</description></item>
        /// <item><description>2番目以降の配列: 解空間の基底ベクトル</description></item>
        /// <item><description>ただし解無しのときは空配列を返す</description></item>
        /// </list>
        /// </returns>
        public T[] LinearSystem(T vector, int width)
        {
            Contract.Assert(width < BitSize());
            var impl = (T[])_v.Clone();
            {
                var last = T.One << width;
                for (int i = 0; i < impl.Length; i++)
                {
                    Contract.Assert(T.IsZero(impl[i] >> width));
                    if (T.IsOddInteger(vector >> i))
                        impl[i] |= last;
                }
            }

            var idxs = GaussianEliminationImpl(impl, false).AsSpan();
            var r = idxs.Length;
            int w = width;

            // 解があるかチェック
            // a×0+b×0+c×0..+z×0≠0 になっていたら解無し
            for (int i = r; i < impl.Length; i++)
            {
                if (T.IsOddInteger(impl[i] >> width))
                    return Array.Empty<T>();
            }
            if (idxs.IsEmpty)
            {
                var eres = new T[w + 1];
                for (int i = 1; i < eres.Length; i++)
                {
                    eres[i] = T.One << (i - 1);
                }
                return eres;
            }
            if (idxs[^1] == width)
                return Array.Empty<T>();

            var used = new HashSet<int>(Enumerable.Range(0, w));
            var lst = new List<T>(w);
            {
                var v = default(T);
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    used.Remove(f);
                    v |= (impl[y] >> width) << f;
                    for (int x = f + 1; x < w; x++)
                        v ^= (((v & impl[y]) >> x) & T.One) << f;
                }
                lst.Add(v);
            }

            foreach (var s in used)
            {
                var v = T.One << s;
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    for (int x = f + 1; x < w; x++)
                        v ^= (((v & impl[y]) >> x) & T.One) << f;
                }
                lst.Add(v);
            }
            return lst.ToArray();
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is BitMatrix<T> x && Equals(x);
        [凾(256)]
        public bool Equals(BitMatrix<T> other) =>
            kind == other.kind && (kind != Kd.Normal || EqualsMat(_v, other._v));
        static bool EqualsMat(T[] a, T[] b)
            => a.AsSpan().SequenceEqual(b);

        [凾(256)]
        public override int GetHashCode() => kind switch
        {
            Kd.Normal => HashCode.Combine(kind, _v.Length, _v[0], _v[^1], _v[1], _v[^2]),
            _ => HashCode.Combine(kind),
        };
        [凾(256)]
        public static bool operator ==(BitMatrix<T> left, BitMatrix<T> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(BitMatrix<T> left, BitMatrix<T> right) => !(left == right);

        public override string ToString()
        {
            if (kind != Kd.Normal) return kind.ToString();
            var bitSize = BitSize();
            var sb = new StringBuilder(_v.Length * (bitSize + 2));
            var charsSize = (bitSize + 63) >> 6;

            char[] bufArray = null;
            Span<char> buf = bitSize <= 256
                ? stackalloc char[512]
                : (bufArray = ArrayPool<char>.Shared.Rent(bitSize));

            foreach (var row in _v)
            {
#if NET8_0_OR_GREATER
                row.TryFormat(buf[256..], out var cw, "B", null);
                var rt = buf.Slice(256 - bitSize + cw, bitSize);
                rt[..^cw].Fill('0');
                rt.Reverse();
                sb.Append(rt);
#else
                var v = row;
                for (int i = 0; i < charsSize; i++)
                {
                    var chars = Convert.ToString(long.CreateTruncating(v), 2).ToCharArray();
                    Array.Reverse(chars);
                    sb.Append(chars).Append('0', 64 - chars.Length);
                    v >>= 64;
                }
#endif
                sb.Append('\n');
            }

            if (bufArray != null)
                ArrayPool<char>.Shared.Return(bufArray);
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
        /// <summary>
        /// 0,1 で表された行列をパースします。
        /// </summary>
        public static BitMatrix<T> Parse(string[] rows)
        {
            var arr = new T[rows.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = ParseRow(rows[i]);
            }
            return new BitMatrix<T>(arr);

            static T ParseRow(string row)
            {
                var s = row.AsSpan().Trim();
                Span<char> t = stackalloc char[Unsafe.SizeOf<T>() * 8];
                s.CopyTo(t);
                t[s.Length..].Fill('0');
                t.Reverse();
                return BinaryParser.ParseNumber<T>(t);
            }
        }

        [SourceExpander.NotEmbeddingSource]
        class DebugView
        {
            readonly BitMatrix<T> m;
            public DebugView(BitMatrix<T> matrix)
            {
                m = matrix;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            internal Item[] Items => m._v.Select(v => new Item(v)).ToArray();

            internal readonly record struct Item(T Value)
            {
                public override string ToString()
                {
                    var v = Value;
                    var bitSize = BitSize();
#if NET8_0_OR_GREATER
                    char[] bufArray = null;
                    Span<char> buf = bitSize <= 256
                        ? stackalloc char[512]
                        : (bufArray = ArrayPool<char>.Shared.Rent(bitSize));
                    Value.TryFormat(buf[256..], out var cw, "B", null);
                    var rt = buf.Slice(256 - bitSize + cw, bitSize);
                    rt[..^cw].Fill('0');
                    rt.Reverse();
                    var rs = new string(rt);
                    if (bufArray != null)
                        ArrayPool<char>.Shared.Return(bufArray);
                    return rs;
#else
                    var charsSize = (bitSize + 63) >> 6;
                    var sb = new StringBuilder(bitSize + 2);
                    for (int i = 0; i < charsSize; i++)
                    {
                        var chars = Convert.ToString(long.CreateTruncating(v), 2).ToCharArray();
                        Array.Reverse(chars);
                        sb.Append(chars).Append('0', 64 - chars.Length);
                        v >>= 64;
                    }
                    while (sb.Length > bitSize)
                        sb.Remove(sb.Length - 1, 1);
                    return sb.ToString();
#endif
                }
            }
        }
    }
}
