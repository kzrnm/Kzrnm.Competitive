using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using static SimdMontgomery;
    using Kd = ArrayMatrixKind;
    [DebuggerTypeProxy(typeof(SimdModMatrix<>.DebugView))]
    public readonly struct SimdModMatrix<T> : IMatrix<SimdModMatrix<T>, MontgomeryModInt<T>>
        , IMultiplyOperators<SimdModMatrix<T>, MontgomeryModInt<T>, SimdModMatrix<T>>
        where T : struct, IStaticMod
    {
        public MontgomeryModInt<T> this[int row, int col] => _v[Index(row, col)];
        [凾(256)] int Index(int row, int col) => row * _w + col;

        public int Height => _h;
        public int Width => _w;
        readonly int _h, _w;
        internal readonly MontgomeryModInt<T>[] _v;
        public ReadOnlySpan<MontgomeryModInt<T>> AsSpan() => _v;
        public MontgomeryModInt<T>[][] ToArray()
        {
            var arr = new MontgomeryModInt<T>[_h][];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = _v.AsSpan(i * _w, _w).ToArray();
            return arr;
        }

        public static readonly SimdModMatrix<T> Zero = new(Kd.Zero);
        public static readonly SimdModMatrix<T> Identity = new(Kd.Identity);
        public static SimdModMatrix<T> AdditiveIdentity => Zero;
        public static SimdModMatrix<T> MultiplicativeIdentity => Identity;

        internal readonly Kd kind;
        internal SimdModMatrix(Kd kind)
        {
            this.kind = kind;
            _v = null;
        }

        public SimdModMatrix(MontgomeryModInt<T>[] value, int height, int width)
        {
            Contract.Assert(value.Length == height * width, "value.Length が不正です。");
            kind = Kd.Normal;
            _h = height;
            _w = width;
            _v = value;
        }
        public SimdModMatrix(ReadOnlySpan<MontgomeryModInt<T>> span, int height, int width) : this(span.ToArray(), height, width) { }
        public SimdModMatrix(MontgomeryModInt<T>[][] m)
        {
            if (m.Length == 0)
            {
                kind = Kd.Normal;
                _v = Array.Empty<MontgomeryModInt<T>>();
            }
            else
            {
                kind = Kd.Normal;
                var height = m.Length;
                var width = m[0].Length;
                kind = Kd.Normal;
                _h = height;
                _w = width;
                var val = new MontgomeryModInt<T>[height * width];
                for (int i = 0; i < m.Length; i++)
                {
                    Contract.Assert(m[i].Length == width);
                    m[i].AsSpan().CopyTo(val.AsSpan(i * width));
                }
                _v = val;
            }
        }
        public SimdModMatrix(MontgomeryModInt<T>[,] m)
        {
            var height = m.GetLength(0);
            var width = m.GetLength(1);
            kind = Kd.Normal;
            _h = height;
            _w = width;
            _v = MemoryMarshal.CreateReadOnlySpan(ref m[0, 0], m.Length).ToArray();
        }

        static SimdModMatrix<T> ThrowNotSupportResponse() => throw new NotSupportedException();

        /// <summary>
        /// 大きさ <paramref name="s"/> の単位行列を返します。
        /// </summary>
        [凾(256)]
        public static SimdModMatrix<T> NormalIdentity(int s)
        {
            var v = new MontgomeryModInt<T>[s * s];
            for (int i = 0; i < v.Length; i += s + 1)
                v[i] = MontgomeryModInt<T>.One;
            return new(v, s, s);
        }

        /// <summary>
        /// 零行列かどうかを返します。
        /// </summary>
        public bool IsZero => kind is Kd.Zero;
        [凾(256)] MontgomeryModInt<T>[] CloneArray() => (MontgomeryModInt<T>[])_v.Clone();

        SimdModMatrix<T> AddIdentity()
        {
            var arr = CloneArray();
            for (int i = Math.Min(_h, _w) - 1; i >= 0; i--)
                arr[Index(i, i)] += MontgomeryModInt<T>.One;
            return new(arr, _h, _w);
        }
        [凾(256)]
        SimdModMatrix<T> Add(SimdModMatrix<T> other)
        {
            Contract.Assert(_h == other._h && _w == other._w);
            var m2 = Vector256.Create(new T().Mod * 2);
            var arr = CloneArray();
            if (_v.Length == 0) return new();
            {
                ref var op = ref other._v[0];
                for (int i = arr.Length >> 3 << 3; i < arr.Length; i++)
                    arr[i] += Unsafe.Add(ref op, i);
            }
            var brr = MemoryMarshal.Cast<MontgomeryModInt<T>, Vector256<uint>>(arr);
            var os = MemoryMarshal.Cast<MontgomeryModInt<T>, Vector256<uint>>(other._v.AsSpan());
            if (os.Length > 0)
            {
                ref var op = ref os[0];
                for (int i = 0; i < brr.Length; i++)
                    brr[i] = MontgomeryAdd(brr[i], Unsafe.Add(ref op, i), m2);
            }
            return new(arr, _h, _w);
        }
        [凾(256)]
        public static SimdModMatrix<T> operator +(SimdModMatrix<T> x, SimdModMatrix<T> y)
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
        SimdModMatrix<T> SubtractIdentity()
        {
            var arr = CloneArray();
            for (int i = Math.Min(_h, _w) - 1; i >= 0; i--)
                arr[Index(i, i)] -= MontgomeryModInt<T>.One;
            return new(arr, _h, _w);
        }
        [凾(256)]
        SimdModMatrix<T> Subtract(SimdModMatrix<T> other)
        {
            Contract.Assert(_h == other._h && _w == other._w);
            var m2 = Vector256.Create(new T().Mod * 2);
            var arr = CloneArray();
            if (_v.Length == 0) return new();
            {
                ref var op = ref other._v[0];
                for (int i = arr.Length >> 3 << 3; i < arr.Length; i++)
                    arr[i] -= Unsafe.Add(ref op, i);
            }
            var brr = MemoryMarshal.Cast<MontgomeryModInt<T>, Vector256<uint>>(arr);
            var os = MemoryMarshal.Cast<MontgomeryModInt<T>, Vector256<uint>>(other._v.AsSpan());
            if (os.Length > 0)
            {
                ref var op = ref os[0];
                for (int i = 0; i < brr.Length; i++)
                    brr[i] = MontgomerySubtract(brr[i], Unsafe.Add(ref op, i), m2);
            }
            return new(arr, _h, _w);
        }

        [凾(256)] public static SimdModMatrix<T> operator +(SimdModMatrix<T> x) => x;
        [凾(256)]
        public static SimdModMatrix<T> operator -(SimdModMatrix<T> x)
        {
            var arr = x.CloneArray();
            for (int i = 0; i < arr.Length; i++)
                arr[i] = -arr[i];
            return new(arr, x._h, x._w);
        }

        [凾(256)]
        public static SimdModMatrix<T> operator -(SimdModMatrix<T> x, SimdModMatrix<T> y)
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
        SimdModMatrix<T> Multiply(SimdModMatrix<T> other)
        {
            var rh = Height;
            var rw = other.Width;
            var impl = new SimdStrassenImpl<T>(Math.Max(Math.Max(rh, other.Height), Math.Max(Width, rw)));
            var rt = impl.Strassen(impl.ToVectorize(AsSpan(), rh, Width), impl.ToVectorize(other.AsSpan(), other.Height, rw));
            return new(impl.ToMatrix(rt, rh, rw), rh, rw);
        }
        [凾(256)]
        public static SimdModMatrix<T> operator *(SimdModMatrix<T> x, SimdModMatrix<T> y)
        {
            return x.kind switch
            {
                Kd.Normal => y.kind switch
                {
                    Kd.Normal => x.Multiply(y),
                    Kd.Zero => new(new MontgomeryModInt<T>[x._v.Length], x._h, x._w),
                    _ => x,
                },
                Kd.Zero => y.kind switch
                {
                    Kd.Normal => new(new MontgomeryModInt<T>[y._v.Length], y._h, y._w),
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
        SimdModMatrix<T> MultiplyScalar(MontgomeryModInt<T> scalar)
        {
            var arr = _v;
            var res = new MontgomeryModInt<T>[arr.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = scalar * arr[i];
            return new(res, _h, _w);
        }
        [凾(256)]
        public static SimdModMatrix<T> operator *(SimdModMatrix<T> m, MontgomeryModInt<T> x)
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
        [凾(256)] public static MontgomeryModInt<T>[] operator *(SimdModMatrix<T> mat, MontgomeryModInt<T>[] vector) => mat.Multiply(vector);

        /// <summary>
        /// ベクトルにかける
        /// </summary>
        [凾(256)]
        public MontgomeryModInt<T>[] Multiply(MontgomeryModInt<T>[] vector)
        {
            Contract.Assert(_w == vector.Length);
            return (this * new SimdModMatrix<T>(vector, vector.Length, 1))._v;
        }
        /// <summary>
        /// 行列式を求めます。
        /// </summary>
        [凾(256)]
        public MontgomeryModInt<T> Determinant() => ArrayMatrixLogic<MontgomeryModInt<T>>.DeterminantImpl(ToArray());


        /// <summary>
        /// <c>(<paramref name="i"/>, <paramref name="j"/>)</c> 余因子を求めます。
        /// </summary>
        [凾(256)]
        public MontgomeryModInt<T> Cofactor(int i, int j)
        {
            Contract.Assert(_h == _w);
            return ArrayMatrixLogic<MontgomeryModInt<T>>.Cofactor(_v, _h, i, j);
        }

        /// <summary>
        /// 逆行列を掃き出し法で求めます。求められなければ零行列を返します。
        /// </summary>
        [凾(256)]
        public SimdModMatrix<T> Inv()
        {
            Contract.Assert(_h == _w);
            var res = ArrayMatrixLogic<MontgomeryModInt<T>>.Inv(_v, _h);
            return res == null
                ? Zero
                : new(res);
        }

        /// <summary>
        /// 転置行列を取得します。
        /// </summary>
        public SimdModMatrix<T> Transpose()
        {
            Contract.Assert(kind == Kd.Normal);
            var rt = ArrayMatrixLogic<MontgomeryModInt<T>>.Transpose(_v, _h, _w);
            return new(rt, _w, _h);
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)
        /// </summary>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        [凾(256)]
        public SimdModMatrix<T> GaussianElimination(bool isReduced = true)
        {
            Contract.Assert(kind == Kd.Normal);
            var arr = ToArray();
            ArrayMatrixLogic<MontgomeryModInt<T>>.GaussianEliminationImpl(arr, isReduced);
            return new(arr);
        }

        /// <summary>
        /// 連立一次方程式 <see langword="this"/>・X=<paramref name="vector"/> を満たす ベクトル X を求めます。
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item><description>最初の配列: 特殊解</description></item>
        /// <item><description>2番目以降の配列: 解空間の基底ベクトル</description></item>
        /// <item><description>ただし解無しのときは空配列を返す</description></item>
        /// </list>
        /// </returns>
        [凾(256)]
        public MontgomeryModInt<T>[][] LinearSystem(MontgomeryModInt<T>[] vector)
            => ArrayMatrixLogic<MontgomeryModInt<T>>.LinearSystem(_v, _h, _w, vector);

        [凾(256)]
        public override bool Equals(object obj) => obj is SimdModMatrix<T> x && Equals(x);
        [凾(256)]
        public bool Equals(SimdModMatrix<T> other)
            => kind == other.kind && (kind != Kd.Normal || _h == other._h && _v.AsSpan().SequenceEqual(other._v));
        [凾(256)]
        public override int GetHashCode() => kind switch
        {
            Kd.Normal => HashCode.Combine(kind, _v.Length, _v[0], _v[1], _v[^2], _v[^1]),
            _ => HashCode.Combine(kind),
        };
        [凾(256)]
        public static bool operator ==(SimdModMatrix<T> left, SimdModMatrix<T> right) => left.Equals(right);
        [凾(256)]
        public static bool operator !=(SimdModMatrix<T> left, SimdModMatrix<T> right) => !(left == right);

        [SourceExpander.NotEmbeddingSource]
        class DebugView
        {
            readonly SimdModMatrix<T> m;
            public Kd Kind => m.kind;
            public DebugView(SimdModMatrix<T> matrix)
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
                public readonly MontgomeryModInt<T>[] Row;
                public Item(MontgomeryModInt<T>[] row)
                {
                    Row = row;
                }

                [SourceExpander.NotEmbeddingSource]
                public override string ToString()
                    => Row.Length < 50
                    ? string.Join(' ', Row)
                    : (string.Join(' ', Row.Take(50)) + ",...");
            }
        }
    }
}
