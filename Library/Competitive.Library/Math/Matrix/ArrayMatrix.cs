using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using Kzrnm.Competitive.Internal;
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
    using Kd = ArrayMatrixKind;
    [DebuggerTypeProxy(typeof(ArrayMatrix<>.DebugView))]
    public readonly struct ArrayMatrix<T> : IMatrix<ArrayMatrix<T>, T>
        , IMultiplyOperators<ArrayMatrix<T>, T, ArrayMatrix<T>>
        where T : INumberBase<T>
    {
        public T this[int row, int col] => _v[Index(row, col)];
        [凾(256)] int Index(int row, int col) => row * _w + col;

        public int Height => _h;
        public int Width => _w;
        private readonly int _h, _w;
        internal readonly T[] _v;
        public ReadOnlySpan<T> AsSpan() => _v;
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
            if (_v.Length == 0) return new();
            var m = new ArrayMatrix<T>(CloneArray(), _h, _w);
            m.AddSelf(other);
            return m;
        }
        [凾(256)]
        internal void AddSelf(ArrayMatrix<T> other)
        {
            Contract.Assert(_h == other._h && _w == other._w);
            ref var op = ref other._v[0];
            var v = _v;
            for (int i = 0; i < v.Length; i++)
                v[i] += Unsafe.Add(ref op, i);
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
            if (_v.Length == 0) return new();
            var m = new ArrayMatrix<T>(CloneArray(), _h, _w);
            m.SubtractSelf(other);
            return m;
        }
        [凾(256)]
        internal void SubtractSelf(ArrayMatrix<T> other)
        {
            Contract.Assert(_h == other._h && _w == other._w);
            ref var op = ref other._v[0];
            var v = _v;
            for (int i = 0; i < v.Length; i++)
                v[i] -= Unsafe.Add(ref op, i);
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
        /// 行列式を求めます。
        /// </summary>
        [凾(256)]
        public T Determinant() => ArrayMatrixLogic<T>.DeterminantImpl(ToArray());


        /// <summary>
        /// <c>(<paramref name="i"/>, <paramref name="j"/>)</c> 余因子を求めます。
        /// </summary>
        [凾(256)]
        public T Cofactor(int i, int j)
        {
            Contract.Assert(_h == _w);
            return ArrayMatrixLogic<T>.Cofactor(_v, _h, i, j);
        }

        /// <summary>
        /// 逆行列を掃き出し法で求めます。求められなければ零行列を返します。
        /// </summary>
        [凾(256)]
        public ArrayMatrix<T> Inv()
        {
            Contract.Assert(_h == _w);
            var res = ArrayMatrixLogic<T>.Inv(_v, _h);
            return res == null
                ? Zero
                : new(res);
        }

        /// <summary>
        /// 転置行列を取得します。
        /// </summary>
        public ArrayMatrix<T> Transpose()
        {
            Contract.Assert(kind == Kd.Normal);
            var rt = ArrayMatrixLogic<T>.Transpose(_v, _h, _w);
            return new(rt, _w, _h);
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
            ArrayMatrixLogic<T>.GaussianEliminationImpl(arr, isReduced);
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
        public T[][] LinearSystem(T[] vector)
            => ArrayMatrixLogic<T>.LinearSystem(_v, _h, _w, vector);

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
}
