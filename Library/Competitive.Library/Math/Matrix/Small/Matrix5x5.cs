// <auto-generated>
// DO NOT CHANGE THIS FILE.
// </auto-generated>
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct Matrix5x5<T> : IMatrix<Matrix5x5<T>, T>
        , IMultiplyOperators<Matrix5x5<T>, T, Matrix5x5<T>>
        where T : INumberBase<T>
    {
        public int Height => 5;
        public int Width => 5;
        public (T Col0, T Col1, T Col2, T Col3, T Col4) Row0 => (V00, V01, V02, V03, V04);
        public (T Col0, T Col1, T Col2, T Col3, T Col4) Row1 => (V10, V11, V12, V13, V14);
        public (T Col0, T Col1, T Col2, T Col3, T Col4) Row2 => (V20, V21, V22, V23, V24);
        public (T Col0, T Col1, T Col2, T Col3, T Col4) Row3 => (V30, V31, V32, V33, V34);
        public (T Col0, T Col1, T Col2, T Col3, T Col4) Row4 => (V40, V41, V42, V43, V44);
        internal readonly T
            V00, V01, V02, V03, V04,
            V10, V11, V12, V13, V14,
            V20, V21, V22, V23, V24,
            V30, V31, V32, V33, V34,
            V40, V41, V42, V43, V44;
        [凾(256)] public ReadOnlySpan<T> AsSpan() => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Matrix5x5<T>, T>(ref Unsafe.AsRef(this)), 5 * 5);
        public T[][] ToArray() => new[]
        {
            new[]{ V00, V01, V02, V03, V04 },
            new[]{ V10, V11, V12, V13, V14 },
            new[]{ V20, V21, V22, V23, V24 },
            new[]{ V30, V31, V32, V33, V34 },
            new[]{ V40, V41, V42, V43, V44 },
        };
        [凾(256)]
        public Matrix5x5(
            (T Col0, T Col1, T Col2, T Col3, T Col4) row0,
            (T Col0, T Col1, T Col2, T Col3, T Col4) row1,
            (T Col0, T Col1, T Col2, T Col3, T Col4) row2,
            (T Col0, T Col1, T Col2, T Col3, T Col4) row3,
            (T Col0, T Col1, T Col2, T Col3, T Col4) row4
        )
        {
            (V00, V01, V02, V03, V04) = row0;
            (V10, V11, V12, V13, V14) = row1;
            (V20, V21, V22, V23, V24) = row2;
            (V30, V31, V32, V33, V34) = row3;
            (V40, V41, V42, V43, V44) = row4;
        }

        public static Matrix5x5<T> AdditiveIdentity => default;
        public static Matrix5x5<T> MultiplicativeIdentity => new(
            (T.MultiplicativeIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.MultiplicativeIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.AdditiveIdentity, T.MultiplicativeIdentity, T.AdditiveIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.MultiplicativeIdentity, T.AdditiveIdentity),
            (T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity, T.MultiplicativeIdentity));

        [凾(256)] public static Matrix5x5<T> operator +(Matrix5x5<T> x) => x;
        [凾(256)]
        public static Matrix5x5<T> operator -(Matrix5x5<T> x)
            => new(
            (x.V00, x.V01, x.V02, x.V03, x.V04),
            (x.V10, x.V11, x.V12, x.V13, x.V14),
            (x.V20, x.V21, x.V22, x.V23, x.V24),
            (x.V30, x.V31, x.V32, x.V33, x.V34),
            (x.V40, x.V41, x.V42, x.V43, x.V44));


        [凾(256)]
        public static Matrix5x5<T> operator +(Matrix5x5<T> x, Matrix5x5<T> y)
            => new(
            (x.V00 + y.V00, x.V01 + y.V01, x.V02 + y.V02, x.V03 + y.V03, x.V04 + y.V04),
            (x.V10 + y.V10, x.V11 + y.V11, x.V12 + y.V12, x.V13 + y.V13, x.V14 + y.V14),
            (x.V20 + y.V20, x.V21 + y.V21, x.V22 + y.V22, x.V23 + y.V23, x.V24 + y.V24),
            (x.V30 + y.V30, x.V31 + y.V31, x.V32 + y.V32, x.V33 + y.V33, x.V34 + y.V34),
            (x.V40 + y.V40, x.V41 + y.V41, x.V42 + y.V42, x.V43 + y.V43, x.V44 + y.V44));
        [凾(256)]
        public static Matrix5x5<T> operator -(Matrix5x5<T> x, Matrix5x5<T> y)
            => new(
            (x.V00 - y.V00, x.V01 - y.V01, x.V02 - y.V02, x.V03 - y.V03, x.V04 - y.V04),
            (x.V10 - y.V10, x.V11 - y.V11, x.V12 - y.V12, x.V13 - y.V13, x.V14 - y.V14),
            (x.V20 - y.V20, x.V21 - y.V21, x.V22 - y.V22, x.V23 - y.V23, x.V24 - y.V24),
            (x.V30 - y.V30, x.V31 - y.V31, x.V32 - y.V32, x.V33 - y.V33, x.V34 - y.V34),
            (x.V40 - y.V40, x.V41 - y.V41, x.V42 - y.V42, x.V43 - y.V43, x.V44 - y.V44));

        [凾(256)]
        public static Matrix5x5<T> operator *(Matrix5x5<T> x, Matrix5x5<T> y)
            => new(
            (
                x.V00 * y.V00 + x.V01 * y.V10 + x.V02 * y.V20 + x.V03 * y.V30 + x.V04 * y.V40,
                x.V00 * y.V01 + x.V01 * y.V11 + x.V02 * y.V21 + x.V03 * y.V31 + x.V04 * y.V41,
                x.V00 * y.V02 + x.V01 * y.V12 + x.V02 * y.V22 + x.V03 * y.V32 + x.V04 * y.V42,
                x.V00 * y.V03 + x.V01 * y.V13 + x.V02 * y.V23 + x.V03 * y.V33 + x.V04 * y.V43,
                x.V00 * y.V04 + x.V01 * y.V14 + x.V02 * y.V24 + x.V03 * y.V34 + x.V04 * y.V44
            ),
            (
                x.V10 * y.V00 + x.V11 * y.V10 + x.V12 * y.V20 + x.V13 * y.V30 + x.V14 * y.V40,
                x.V10 * y.V01 + x.V11 * y.V11 + x.V12 * y.V21 + x.V13 * y.V31 + x.V14 * y.V41,
                x.V10 * y.V02 + x.V11 * y.V12 + x.V12 * y.V22 + x.V13 * y.V32 + x.V14 * y.V42,
                x.V10 * y.V03 + x.V11 * y.V13 + x.V12 * y.V23 + x.V13 * y.V33 + x.V14 * y.V43,
                x.V10 * y.V04 + x.V11 * y.V14 + x.V12 * y.V24 + x.V13 * y.V34 + x.V14 * y.V44
            ),
            (
                x.V20 * y.V00 + x.V21 * y.V10 + x.V22 * y.V20 + x.V23 * y.V30 + x.V24 * y.V40,
                x.V20 * y.V01 + x.V21 * y.V11 + x.V22 * y.V21 + x.V23 * y.V31 + x.V24 * y.V41,
                x.V20 * y.V02 + x.V21 * y.V12 + x.V22 * y.V22 + x.V23 * y.V32 + x.V24 * y.V42,
                x.V20 * y.V03 + x.V21 * y.V13 + x.V22 * y.V23 + x.V23 * y.V33 + x.V24 * y.V43,
                x.V20 * y.V04 + x.V21 * y.V14 + x.V22 * y.V24 + x.V23 * y.V34 + x.V24 * y.V44
            ),
            (
                x.V30 * y.V00 + x.V31 * y.V10 + x.V32 * y.V20 + x.V33 * y.V30 + x.V34 * y.V40,
                x.V30 * y.V01 + x.V31 * y.V11 + x.V32 * y.V21 + x.V33 * y.V31 + x.V34 * y.V41,
                x.V30 * y.V02 + x.V31 * y.V12 + x.V32 * y.V22 + x.V33 * y.V32 + x.V34 * y.V42,
                x.V30 * y.V03 + x.V31 * y.V13 + x.V32 * y.V23 + x.V33 * y.V33 + x.V34 * y.V43,
                x.V30 * y.V04 + x.V31 * y.V14 + x.V32 * y.V24 + x.V33 * y.V34 + x.V34 * y.V44
            ),
            (
                x.V40 * y.V00 + x.V41 * y.V10 + x.V42 * y.V20 + x.V43 * y.V30 + x.V44 * y.V40,
                x.V40 * y.V01 + x.V41 * y.V11 + x.V42 * y.V21 + x.V43 * y.V31 + x.V44 * y.V41,
                x.V40 * y.V02 + x.V41 * y.V12 + x.V42 * y.V22 + x.V43 * y.V32 + x.V44 * y.V42,
                x.V40 * y.V03 + x.V41 * y.V13 + x.V42 * y.V23 + x.V43 * y.V33 + x.V44 * y.V43,
                x.V40 * y.V04 + x.V41 * y.V14 + x.V42 * y.V24 + x.V43 * y.V34 + x.V44 * y.V44
            )
            );

        [凾(256)]
        public static Matrix5x5<T> operator *(Matrix5x5<T> m, T x)
            => new(
            (x * m.V00, x * m.V01, x * m.V02, x * m.V03, x * m.V04),
            (x * m.V10, x * m.V11, x * m.V12, x * m.V13, x * m.V14),
            (x * m.V20, x * m.V21, x * m.V22, x * m.V23, x * m.V24),
            (x * m.V30, x * m.V31, x * m.V32, x * m.V33, x * m.V34),
            (x * m.V40, x * m.V41, x * m.V42, x * m.V43, x * m.V44));

        /// <summary>
        /// 5次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public static (T Col0, T Col1, T Col2, T Col3, T Col4) operator *(Matrix5x5<T> mat, (T Col0, T Col1, T Col2, T Col3, T Col4) vector) => mat.Multiply(vector);

        /// <summary>
        /// 5次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T Col0, T Col1, T Col2, T Col3, T Col4) Multiply((T Col0, T Col1, T Col2, T Col3, T Col4) vector) => Multiply(vector.Col0, vector.Col1, vector.Col2, vector.Col3, vector.Col4);

        /// <summary>
        /// 5次元ベクトルにかける
        /// </summary>
        [凾(256)]
        public (T Col0, T Col1, T Col2, T Col3, T Col4) Multiply(T v0, T v1, T v2, T v3, T v4)
                => (
                        V00 * v0 + V01 * v1 + V02 * v2 + V03 * v3 + V04 * v4,
                        V10 * v0 + V11 * v1 + V12 * v2 + V13 * v3 + V14 * v4,
                        V20 * v0 + V21 * v1 + V22 * v2 + V23 * v3 + V24 * v4,
                        V30 * v0 + V31 * v1 + V32 * v2 + V33 * v3 + V34 * v4,
                        V40 * v0 + V41 * v1 + V42 * v2 + V43 * v3 + V44 * v4
                   );

        [凾(256)] public static bool operator ==(Matrix5x5<T> left, Matrix5x5<T> right) => left.Equals(right);
        [凾(256)] public static bool operator !=(Matrix5x5<T> left, Matrix5x5<T> right) => !(left == right);
        [凾(256)] public override bool Equals(object obj) => obj is Matrix5x5<T> x && Equals(x);

        [凾(256)]
        public bool Equals(Matrix5x5<T> other) =>
            EqualityComparer<T>.Default.Equals(V00, other.V00) &&
            EqualityComparer<T>.Default.Equals(V01, other.V01) &&
            EqualityComparer<T>.Default.Equals(V02, other.V02) &&
            EqualityComparer<T>.Default.Equals(V03, other.V03) &&
            EqualityComparer<T>.Default.Equals(V04, other.V04) &&
            EqualityComparer<T>.Default.Equals(V10, other.V10) &&
            EqualityComparer<T>.Default.Equals(V11, other.V11) &&
            EqualityComparer<T>.Default.Equals(V12, other.V12) &&
            EqualityComparer<T>.Default.Equals(V13, other.V13) &&
            EqualityComparer<T>.Default.Equals(V14, other.V14) &&
            EqualityComparer<T>.Default.Equals(V20, other.V20) &&
            EqualityComparer<T>.Default.Equals(V21, other.V21) &&
            EqualityComparer<T>.Default.Equals(V22, other.V22) &&
            EqualityComparer<T>.Default.Equals(V23, other.V23) &&
            EqualityComparer<T>.Default.Equals(V24, other.V24) &&
            EqualityComparer<T>.Default.Equals(V30, other.V30) &&
            EqualityComparer<T>.Default.Equals(V31, other.V31) &&
            EqualityComparer<T>.Default.Equals(V32, other.V32) &&
            EqualityComparer<T>.Default.Equals(V33, other.V33) &&
            EqualityComparer<T>.Default.Equals(V34, other.V34) &&
            EqualityComparer<T>.Default.Equals(V40, other.V40) &&
            EqualityComparer<T>.Default.Equals(V41, other.V41) &&
            EqualityComparer<T>.Default.Equals(V42, other.V42) &&
            EqualityComparer<T>.Default.Equals(V43, other.V43) &&
            EqualityComparer<T>.Default.Equals(V44, other.V44);
        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(V00);
            hash.Add(V01);
            hash.Add(V02);
            hash.Add(V03);
            hash.Add(V04);
            hash.Add(V10);
            hash.Add(V11);
            hash.Add(V12);
            hash.Add(V13);
            hash.Add(V14);
            hash.Add(V20);
            hash.Add(V21);
            hash.Add(V22);
            hash.Add(V23);
            hash.Add(V24);
            hash.Add(V30);
            hash.Add(V31);
            hash.Add(V32);
            hash.Add(V33);
            hash.Add(V34);
            hash.Add(V40);
            hash.Add(V41);
            hash.Add(V42);
            hash.Add(V43);
            hash.Add(V44);
            return hash.ToHashCode();
        }
    }
}

