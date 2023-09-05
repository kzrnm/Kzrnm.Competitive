using AtCoder.Internal;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    /// <summary>
    /// Mod2 の行列。+: xor *: and
    /// </summary>
    public readonly struct BitMatrix<T> : Internal.IMatrixOperator<BitMatrix<T>>
        where T : unmanaged, IBinaryInteger<T>
    {
        public bool this[int row, int col] => (uint.CreateTruncating(Value[row] >> col) & 1) != 0;
        public readonly T[] Value;

        public static readonly BitMatrix<T> Zero = new BitMatrix<T>(Kd.Zero);
        public static readonly BitMatrix<T> Identity = new BitMatrix<T>(Kd.Identity);
        public static BitMatrix<T> AdditiveIdentity => Zero;
        public static BitMatrix<T> MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal BitMatrix(Kd kind)
        {
            this.kind = kind;
            Value = null;
        }

        public BitMatrix(T[] value)
        {

            Value = value;
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
        static int BitSize() => int.CreateTruncating(Unsafe.SizeOf<T>() << 3);


        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;

        static T[] CloneArray(T[] arr) => arr.ToArray();

        BitMatrix<T> AddIdentity()
        {
            var arr = CloneArray(Value);
            for (int i = arr.Length - 1; i >= 0; i--)
                arr[i] ^= T.One << (arr.Length - i - 1);
            return new BitMatrix<T>(arr);
        }
        BitMatrix<T> Add(BitMatrix<T> other)
        {
            Contract.Assert(Value.Length == other.Value.Length);
            var otherArr = other.Value;
            var val = Value;
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
                    _ => new BitMatrix<T>(CloneArray(y.Value)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new BitMatrix<T>(CloneArray(x.Value)),
                    Kd.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }

        [凾(256)] public static BitMatrix<T> operator +(BitMatrix<T> x) => x;
        public static BitMatrix<T> operator -(BitMatrix<T> x)
        {
            var val = x.Value;
            var arr = new T[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = ~val[i];
            return new BitMatrix<T>(arr);
        }
        public static BitMatrix<T> operator ~(BitMatrix<T> x) => -x;

        public static BitMatrix<T> operator -(BitMatrix<T> x, BitMatrix<T> y) => x + y;
        public static BitMatrix<T> operator ^(BitMatrix<T> x, BitMatrix<T> y) => x + y;

        private BitMatrix<T> Multiply(BitMatrix<T> other)
        {
            var val = Value;
            var otherArr = other.Value;
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
                    Kd.Normal => new BitMatrix<T>(new T[y.Value.Length]),
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
                    Kd.Zero => new BitMatrix<T>(new T[x.Value.Length]),
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
            var val = Value;
            var res = default(T);
            for (int i = 0; i < val.Length; i++)
                res |= (T.PopCount(val[i] & vector) & T.One) << (val.Length - i - 1);

            return res;
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        public BitMatrix<T> GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = CloneArray(Value);
            GaussianEliminationImpl(arr, isReduced);
            return new BitMatrix<T>(arr);
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>0ではない列のインデックス</returns>
        private static List<int> GaussianEliminationImpl(T[] arr, bool isReduced)
        {
            var idx = new List<int>(arr.Length);
            int r = 0;
            int width = BitSize();
            for (int x = 0; x < width && r < arr.Length; x++)
            {
                var xx = width - x - 1;
                if (!SearchNonZero(arr, r, x))
                    continue;
                var arrR = arr[r];
                for (int y = isReduced ? 0 : r + 1; y < arr.Length; y++)
                {
                    var arrY = arr[y];
                    if (y == r || (uint.CreateTruncating(arrY >> xx) & 1) == 0)
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
        private static bool SearchNonZero(T[] mat, int r, int x)
        {
            x = BitSize() - x - 1;
            for (int y = r; y < mat.Length; y++)
                if ((uint.CreateTruncating(mat[y] >> x) & 1) != 0)
                {
                    (mat[r], mat[y]) = (mat[y], mat[r]);
                    return true;
                }
            return false;
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is BitMatrix<T> x && Equals(x);
        [凾(256)]
        public bool Equals(BitMatrix<T> other) =>
            kind == other.kind && (kind != Kd.Normal || EqualsMat(Value, other.Value));
        private static bool EqualsMat(T[] a, T[] b)
            => a.AsSpan().SequenceEqual(b);

        [凾(256)]
        public override int GetHashCode() => kind switch
        {
            Kd.Normal => HashCode.Combine(kind, Value.Length, Value[0], Value[^1], Value[1], Value[^2]),
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
            var sb = new StringBuilder(Value.Length * (64 + 2));

            var charsSize = (bitSize + 63) / 64;
            var cache = ArrayPool<string>.Shared.Rent(charsSize);
            foreach (var row in Value)
            {
                // TODO: .NET 8 か 9 以降では 2 進数の Parse/Format ができるようになりそう
                // https://github.com/dotnet/runtime/issues/83619

                var inner = new StringBuilder(cache.Length * 64 + bitSize);
                inner.Append('0', bitSize);

                var v = row;
                for (int i = 0; i < charsSize; i++)
                {
                    cache[i] = Convert.ToString(long.CreateTruncating(v), 2);
                    v >>= 64;
                }
                for (int i = charsSize - 1; i >= 0; i--)
                    inner.Append(cache[i]);

                sb.Append(inner, inner.Length - bitSize, bitSize).Append('\n');
            }
            ArrayPool<string>.Shared.Return(cache);
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
        /// <summary>
        /// 0,1 で表された行列をパースします。
        /// </summary>
        public static BitMatrix<T> Parse(string[] rows)
        {
            var arr = new T[rows.Length];
            arr[0] = ParseRow(rows[0]);
            for (int i = 1; i < arr.Length; i++)
            {
                arr[i] = ParseRow(rows[i]);
            }
            return new BitMatrix<T>(arr);

            static T ParseRow(string row)
            {
                var span = row.AsSpan().Trim();
                var res = T.Zero;

                var reminder = span.Length & 63; // % 64
                if (reminder > 0)
                {
                    res = T.CreateTruncating(Convert.ToUInt64(new string(span[..reminder]), 2));
                    span = span[reminder..];
                }

                while (span.Length > 0)
                {
                    res <<= 64;
                    if (span.Length < 64)
                        res |= T.CreateTruncating(Convert.ToUInt64(new string(span), 2));
                    else
                    {
                        res |= T.CreateTruncating(Convert.ToUInt64(new string(span[..64]), 2));
                        span = span[64..];
                    }
                }
                return res;
            }
        }
    }
}
