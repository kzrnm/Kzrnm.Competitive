using AtCoder.Internal;
using System;
using System.Linq;
using System.Numerics;
using System.Text;
using BitArray = System.Collections.BitArray;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    /// <summary>
    /// OR 演算の行列。+: or *: and
    /// </summary>
    public readonly struct BitOrMatrix
        : Internal.IMatrixOperator<BitOrMatrix>
    {
        public bool this[int row, int col] => Value[row][col];
        public readonly BitArray[] Value;

        public static readonly BitOrMatrix Zero = new BitOrMatrix(Kd.Zero);
        public static readonly BitOrMatrix Identity = new BitOrMatrix(Kd.Identity);
        public static BitOrMatrix AdditiveIdentity => Zero;
        public static BitOrMatrix MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal BitOrMatrix(Kd kind)
        {
            this.kind = kind;
            Value = null;
        }

        public BitOrMatrix(BitArray[] value)
        {
            Value = value;
            kind = Kd.Normal;
        }
        public BitOrMatrix(bool[][] value) : this(value.Select(a => new BitArray(a)).ToArray()) { }

        private static BitOrMatrix ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;
        private static BitArray[] NormalZeroMatrix(int row, int col)
        {
            var arr = new BitArray[row];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(col);
            return arr;
        }
        private static BitArray[] CloneArray(BitArray[] arr)
        {
            var res = new BitArray[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                res[i] = new BitArray(arr[i]);
            return res;
        }

        private BitOrMatrix AddIdentity()
        {
            var arr = CloneArray(Value);
            for (int i = 0; i < arr.Length; i++)
                arr[i][i] = true;
            return new BitOrMatrix(arr);
        }
        private BitOrMatrix Add(BitOrMatrix other)
        {
            Contract.Assert(Value.Length == other.Value.Length);
            Contract.Assert(Value[0].Length == other.Value[0].Length);
            var otherArr = other.Value;
            var val = Value;
            var arr = new BitArray[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(val[i]).Or(otherArr[i]);

            return new BitOrMatrix(arr);
        }
        public static BitOrMatrix operator +(BitOrMatrix x, BitOrMatrix y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => new BitOrMatrix(CloneArray(y.Value)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new BitOrMatrix(CloneArray(x.Value)),
                    Kd.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }

        [凾(256)] public static BitOrMatrix operator +(BitOrMatrix x) => x;

        private BitOrMatrix Multiply(BitOrMatrix other)
        {
            var val = Value;
            var otherArr = other.Value;
            Contract.Assert(val[0].Length == otherArr.Length);
            var res = new BitArray[val.Length];
            for (int i = 0; i < res.Length; i++)
            {
                var row = res[i] = new BitArray(otherArr[0].Length);
                for (int j = 0; j < val[i].Length; j++)
                    if (val[i][j])
                        row.Or(otherArr[j]);
            }
            return new BitOrMatrix(res);
        }
        public static BitOrMatrix operator *(BitOrMatrix x, BitOrMatrix y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new BitOrMatrix(NormalZeroMatrix(y.Value.Length, y.Value[0].Length)),
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
                    Kd.Zero => new BitOrMatrix(NormalZeroMatrix(x.Value.Length, x.Value[0].Length)),
                    Kd.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static BitArray operator *(BitOrMatrix mat, bool[] vector) => mat.Multiply(new BitArray(vector));

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public static BitArray operator *(BitOrMatrix mat, BitArray vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        public BitArray Multiply(BitArray vector)
        {
            var val = Value;
            var res = new bool[val.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = new BitArray(val[i]).And(vector).Lsb() < vector.Length;

            return new BitArray(res);
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is BitOrMatrix x && Equals(x);
        [凾(256)]
        public bool Equals(BitOrMatrix other) =>
            kind == other.kind && (kind != Kd.Normal || EqualsMat(Value, other.Value));
        private static bool EqualsMat(BitArray[] a, BitArray[] b)
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
            Kd.Normal => HashCode.Combine(kind, Value.Length, Value[0][0], Value[0][^1], Value[^1][0], Value[^1][^1]),
            _ => HashCode.Combine(kind),
        };
        [凾(256)]
        public static bool operator ==(BitOrMatrix left, BitOrMatrix right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(BitOrMatrix left, BitOrMatrix right) => !(left == right);

        public override string ToString()
        {
            if (kind != Kd.Normal) return kind.ToString();
            var sb = new StringBuilder(Value.Length * (Value[0].Length + 2));
            foreach (var row in Value)
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
        public static BitOrMatrix Parse(string[] rows)
        {
            var arr = new BitArray[rows.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = BinaryParser.ParseBitArray(rows[i]);
                if (i > 0 && arr[i].Length != arr[i - 1].Length)
                    throw new FormatException("Row length are diffrent.");
            }
            return new BitOrMatrix(arr);
        }

        static BitOrMatrix ISubtractionOperators<BitOrMatrix, BitOrMatrix, BitOrMatrix>.operator -(BitOrMatrix left, BitOrMatrix right) => throw new NotSupportedException();
        static BitOrMatrix IUnaryNegationOperators<BitOrMatrix, BitOrMatrix>.operator -(BitOrMatrix value) => throw new NotSupportedException();
    }
}
