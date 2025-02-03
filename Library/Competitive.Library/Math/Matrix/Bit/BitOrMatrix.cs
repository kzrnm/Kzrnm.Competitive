using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
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
    /// OR 演算の行列。+: or *: and
    /// </summary>
    [DebuggerTypeProxy(typeof(DebugView))]
    public readonly struct BitOrMatrix
        : Internal.IMatrix<BitOrMatrix>
    {
        public readonly BitArray[] _v;
        public int Height => _v.Length;
        public int Width => _v[0].Length;

        public static readonly BitOrMatrix Zero = new BitOrMatrix(Kd.Zero);
        public static readonly BitOrMatrix Identity = new BitOrMatrix(Kd.Identity);
        public static BitOrMatrix AdditiveIdentity => Zero;
        public static BitOrMatrix MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal BitOrMatrix(Kd kind)
        {
            this.kind = kind;
            _v = null;
        }

        public BitOrMatrix(BitArray[] value)
        {
            _v = value;
            kind = Kd.Normal;
        }
        public BitOrMatrix(bool[][] value) : this(value.Select(a => new BitArray(a)).ToArray()) { }
        public bool this[int row, int col] { [凾(256)] get => _v[row][col]; }
        [凾(256)] public BitArray RowUnsafe(int i) => _v[i];

        static BitOrMatrix ThrowNotSupportResponse() => throw new NotSupportedException();

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
        BitOrMatrix AddIdentity()
        {
            var arr = CloneArray(_v);
            for (int i = 0; i < arr.Length; i++)
                arr[i][i] = true;
            return new BitOrMatrix(arr);
        }
        [凾(256)]
        BitOrMatrix Add(BitOrMatrix other)
        {
            Contract.Assert(_v.Length == other._v.Length);
            Contract.Assert(_v[0].Length == other._v[0].Length);
            var otherArr = other._v;
            var val = _v;
            var arr = new BitArray[val.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new BitArray(val[i]).Or(otherArr[i]);

            return new BitOrMatrix(arr);
        }
        [凾(256)]
        public static BitOrMatrix operator +(BitOrMatrix x, BitOrMatrix y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Zero => Zero,
                    Kd.Identity => Identity,
                    _ => new BitOrMatrix(CloneArray(y._v)),
                },
                Kd.Identity => y.kind switch
                {
                    Kd.Zero => Identity,
                    Kd.Identity => ThrowNotSupportResponse(),
                    _ => y.AddIdentity(),
                },
                _ => y.kind switch
                {
                    Kd.Zero => new BitOrMatrix(CloneArray(x._v)),
                    Kd.Identity => x.AddIdentity(),
                    _ => x.Add(y),
                },
            };
        }

        [凾(256)] public static BitOrMatrix operator +(BitOrMatrix x) => x;

        [凾(256)]
        BitOrMatrix Multiply(BitOrMatrix other)
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
                        row.Or(otherArr[j]);
            }
            return new BitOrMatrix(res);
        }
        [凾(256)]
        public static BitOrMatrix operator *(BitOrMatrix x, BitOrMatrix y)
        {
            return x.kind switch
            {
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new BitOrMatrix(NormalZeroMatrix(y._v.Length, y._v[0].Length)),
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
                    Kd.Zero => new BitOrMatrix(NormalZeroMatrix(x._v.Length, x._v[0].Length)),
                    Kd.Identity => x,
                    _ => x.Multiply(y),
                },
            };
        }

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)] public static BitArray operator *(BitOrMatrix mat, bool[] vector) => mat.Multiply(new BitArray(vector));

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)] public static BitArray operator *(BitOrMatrix mat, BitArray vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)]
        public BitArray Multiply(BitArray vector)
        {
            var val = _v;
            var res = new bool[val.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = new BitArray(val[i]).And(vector).Lsb() < vector.Length;

            return new BitArray(res);
        }

        [凾(256)]
        public override bool Equals(object obj) => obj is BitOrMatrix x && Equals(x);
        [凾(256)]
        public bool Equals(BitOrMatrix other) =>
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
        public static bool operator ==(BitOrMatrix left, BitOrMatrix right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(BitOrMatrix left, BitOrMatrix right) => !(left == right);

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
        public static BitOrMatrix Parse(Asciis[] rows)
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
        public static BitOrMatrix Parse(string[] rows)
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

        static BitOrMatrix ISubtractionOperators<BitOrMatrix, BitOrMatrix, BitOrMatrix>.operator -(BitOrMatrix left, BitOrMatrix right) => throw new NotSupportedException();
        static BitOrMatrix IUnaryNegationOperators<BitOrMatrix, BitOrMatrix>.operator -(BitOrMatrix value) => throw new NotSupportedException();

        [SourceExpander.NotEmbeddingSource]
        readonly record struct DebugView(
            [property: DebuggerBrowsable(DebuggerBrowsableState.Never)] BitOrMatrix Matrix)
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public BitArrayDebug Items => new(Matrix._v);
        }
    }
}
