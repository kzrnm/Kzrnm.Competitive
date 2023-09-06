using AtCoder.Internal;
using AtCoder.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    /// <summary>
    /// Mod2 の行列。+: xor *: and
    /// </summary>
    public readonly struct BitMatrix64
    {
        public bool this[int row, int col] => ((Value[row] >> col) & 1) != 0;
        public readonly ulong[] Value;

        public static readonly BitMatrix64 Zero = new BitMatrix64(Kd.Zero);
        public static readonly BitMatrix64 Identity = new BitMatrix64(Kd.Identity);
        public static BitMatrix64 AdditiveIdentity => Zero;
        public static BitMatrix64 MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal BitMatrix64(Kd kind)
        {
            this.kind = kind;
            Value = null;
        }

        public BitMatrix64(ulong[] value)
        {
            Value = value;
            kind = Kd.Normal;
        }
        public BitMatrix64(bool[][] value) : this(value.Select(BoolArrayToUlong).ToArray()) { }
        static ulong BoolArrayToUlong(bool[] arr)
        {
            var res = 0ul;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i])
                    res |= 1ul << i;
            return res;
        }
        static BitMatrix64 ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;
        static ulong[] CloneArray(ulong[] arr) => arr.ToArray();

        BitMatrix64 AddIdentity()
        {
            var arr = CloneArray(Value);
            for (int i = arr.Length - 1; i >= 0; i--)
                arr[i] ^= 1ul << i;
            return new BitMatrix64(arr);
        }
        private BitMatrix64 Add(BitMatrix64 other)
        {
            Contract.Assert(Value.Length == other.Value.Length);
            var otherArr = other.Value;
            var val = Value;
            var arr = new ulong[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = val[i] ^ otherArr[i];

            return new BitMatrix64(arr);
        }
        public static BitMatrix64 operator +(BitMatrix64 x, BitMatrix64 y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => new BitMatrix64(CloneArray(y.Value)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new BitMatrix64(CloneArray(x.Value)),
                    Kd.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }

        [凾(256)] public static BitMatrix64 operator +(BitMatrix64 x) => x;
        public static BitMatrix64 operator -(BitMatrix64 x)
        {
            var val = x.Value;
            var arr = new ulong[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = ~val[i];
            return new BitMatrix64(arr);
        }
        public static BitMatrix64 operator ~(BitMatrix64 x) => -x;

        public static BitMatrix64 operator -(BitMatrix64 x, BitMatrix64 y) => x + y;
        public static BitMatrix64 operator ^(BitMatrix64 x, BitMatrix64 y) => x + y;

        private BitMatrix64 Multiply(BitMatrix64 other)
        {
            var val = Value;
            var otherArr = other.Value;
            Contract.Assert(otherArr.Length <= 64);
            var res = new ulong[val.Length];
            for (int i = 0; i < res.Length; i++)
            {
                ref var row = ref res[i];
                for (int j = 0; j < otherArr.Length; j++)
                    if (((val[i] >> j) & 1) != 0)
                        row ^= otherArr[^(j + 1)];
            }
            return new BitMatrix64(res);
        }
        public static BitMatrix64 operator *(BitMatrix64 x, BitMatrix64 y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new BitMatrix64(new ulong[y.Value.Length]),
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
                    Kd.Zero => new BitMatrix64(new ulong[x.Value.Length]),
                    Kd.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static ulong operator *(BitMatrix64 mat, bool[] vector) => mat.Multiply(BoolArrayToUlong(vector));

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static ulong operator *(BitMatrix64 mat, ulong vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public ulong Multiply(ulong vector)
        {
            var val = Value;
            var res = 0ul;
            for (int i = 0; i < val.Length; i++)
                res |= (ulong)(BitOperations.PopCount(val[i] & vector) & 1) << i;

            return res;
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        public BitMatrix64 GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = CloneArray(Value);
            GaussianEliminationImpl(arr, isReduced);
            return new BitMatrix64(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>0ではない列のインデックス</returns>
        private static List<int> GaussianEliminationImpl(ulong[] arr, bool isReduced)
        {
            var idx = new List<int>(arr.Length);
            int r = 0;
            const int width = 64;
            for (int x = 0; x < width && r < arr.Length; x++)
            {
                if (!SearchNonZero(arr, r, x))
                    continue;
                var arrR = arr[r];
                for (int y = isReduced ? 0 : r + 1; y < arr.Length; y++)
                {
                    var arrY = arr[y];
                    if (y == r || ((arrY >> x) & 1) == 0)
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
        private static bool SearchNonZero(ulong[] mat, int r, int x)
        {
            for (int y = r; y < mat.Length; y++)
                if (((mat[y] >> x) & 1) != 0)
                {
                    (mat[r], mat[y]) = (mat[y], mat[r]);
                    return true;
                }
            return false;
        }

        /// <summary>
        /// <para>連立一次方程式 <see langword="this"/>・X=<paramref name="vector"/> を満たす ベクトル X を求める。</para>
        /// <para>最上位ビットは計算で使うのでベクトルは 64 より小さくなければならない。</para>
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item><description>最初の配列: 特殊解</description></item>
        /// <item><description>2番目以降の配列: 解空間の基底ベクトル</description></item>
        /// <item><description>ただし解無しのときは空配列を返す</description></item>
        /// </list>
        /// </returns>
        public ulong[] LinearSystem(ulong vector)
        {
            int log2 = 0;
            foreach (var v in Value)
            {
                var s = BitOperations.Log2(v);
                if (log2 < s) log2 = s;
            }
            {
                var s = BitOperations.Log2(vector);
                if (log2 < s) log2 = s;
            }
            return LinearSystem(vector, log2 + 1);
        }
        /// <summary>
        /// <para>連立一次方程式 <see langword="this"/>・X=<paramref name="vector"/> を満たす ベクトル X を求める。</para>
        /// <para>最上位ビットは計算で使うのでベクトルは 64 より小さくなければならない。</para>
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item><description>最初の配列: 特殊解</description></item>
        /// <item><description>2番目以降の配列: 解空間の基底ベクトル</description></item>
        /// <item><description>ただし解無しのときは空配列を返す</description></item>
        /// </list>
        /// </returns>
        public ulong[] LinearSystem(ulong vector, int width)
        {
            Contract.Assert(width < 64);
            var impl = (ulong[])Value.Clone();
            {
                var last = 1ul << width;
                for (int i = 0; i < impl.Length; i++)
                {
                    Contract.Assert((impl[i] >> width) == 0);
                    if (((vector >> i) & 1) != 0)
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
                if (((impl[i] >> width) & 1) != 0)
                    return Array.Empty<ulong>();
            }
            if (idxs.IsEmpty)
            {
                var eres = new ulong[w + 1];
                for (int i = 1; i < eres.Length; i++)
                {
                    eres[i] = 1ul << (i - 1);
                }
                return eres;
            }
            if (idxs[^1] == width)
                return Array.Empty<ulong>();

            var used = new HashSet<int>(Enumerable.Range(0, w));
            var lst = new List<ulong>(w);
            {
                var v = 0ul;
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    used.Remove(f);
                    v |= (impl[y] >> width) << f;
                    for (int x = f + 1; x < w; x++)
                        v ^= (((v & impl[y]) >> x) & 1ul) << f;
                }
                lst.Add(v);
            }

            foreach (var s in used)
            {
                var v = 1ul << s;
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    for (int x = f + 1; x < w; x++)
                        v ^= (((v & impl[y]) >> x) & 1ul) << f;
                }
                lst.Add(v);
            }
            return lst.ToArray();
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is BitMatrix64 x && Equals(x);
        [凾(256)]
        public bool Equals(BitMatrix64 other) =>
            kind == other.kind && (kind != Kd.Normal || EqualsMat(Value, other.Value));
        private static bool EqualsMat(ulong[] a, ulong[] b)
            => a.AsSpan().SequenceEqual(b);

        [凾(256)]
        public override int GetHashCode() => kind switch
        {
            Kd.Normal => HashCode.Combine(kind, Value.Length, Value[0], Value[^1], Value[1], Value[^2]),
            _ => HashCode.Combine(kind),
        };
        [凾(256)]
        public static bool operator ==(BitMatrix64 left, BitMatrix64 right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(BitMatrix64 left, BitMatrix64 right) => !(left == right);
        public override string ToString()
        {
            if (kind != Kd.Normal) return kind.ToString();
            var sb = new StringBuilder(Value.Length * (64 + 2));
            foreach (var row in Value)
            {
                var chars = Convert.ToString((long)row, 2).ToCharArray();
                Array.Reverse(chars);
                sb.Append(chars).Append('0', 64 - chars.Length).Append('\n');
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
        /// <summary>
        /// 0,1 で表された行列をパースします。
        /// </summary>
        public static BitMatrix64 Parse(string[] rows)
        {
            var arr = new ulong[rows.Length];
            arr[0] = ParseRow(rows[0]);
            for (int i = 1; i < arr.Length; i++)
            {
                arr[i] = ParseRow(rows[i]);
            }
            return new BitMatrix64(arr);

            static ulong ParseRow(string row)
            {
                var arr = row.ToCharArray().AsSpan().Trim();
                arr.Reverse();
                return Convert.ToUInt64(new string(arr), 2);
            }
        }

        /// <summary>
        /// <paramref name="y"/> 乗した行列を返す。
        /// </summary>
        public BitMatrix64 Pow(long y) => MathLibGeneric.Pow<BitMatrix64, Operator>(this, y);

        public readonly struct Operator : IArithmeticOperator<BitMatrix64>
        {
            public BitMatrix64 MultiplyIdentity => Identity;

            [凾(256)]
            public BitMatrix64 Add(BitMatrix64 x, BitMatrix64 y) => x + y;
            [凾(256)]
            public BitMatrix64 Subtract(BitMatrix64 x, BitMatrix64 y) => x - y;
            [凾(256)]
            public BitMatrix64 Multiply(BitMatrix64 x, BitMatrix64 y) => x * y;
            [凾(256)]
            public BitMatrix64 Minus(BitMatrix64 x) => -x;

            [凾(256)]
            public BitMatrix64 Increment(BitMatrix64 x) => throw new NotSupportedException();
            [凾(256)]
            public BitMatrix64 Decrement(BitMatrix64 x) => throw new NotSupportedException();
            [凾(256)]
            public BitMatrix64 Divide(BitMatrix64 x, BitMatrix64 y) => throw new NotSupportedException();
            [凾(256)]
            public BitMatrix64 Modulo(BitMatrix64 x, BitMatrix64 y) => throw new NotSupportedException();
        }
    }
}
