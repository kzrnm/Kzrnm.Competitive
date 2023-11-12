using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// XOR 畳み込み
    /// </summary>
    public static class XorConvolution
    {
        /// <summary>
        /// XOR 畳み込み。c[i^j] = ∑ <paramref name="a"/>[i] <paramref name="b"/>[j] となる c を返します。
        /// </summary>
        [凾(256)]
        public static T[] Convolution<T>(T[] a, T[] b) where T : INumberBase<T>
            => Convolution((ReadOnlySpan<T>)a, b);
        /// <summary>
        /// XOR 畳み込み。c[i^j] = ∑ <paramref name="a"/>[i] <paramref name="b"/>[j] となる c を返します。
        /// </summary>
        [凾(256)]
        public static T[] Convolution<T>(Span<T> a, Span<T> b) where T : INumberBase<T>
            => Convolution((ReadOnlySpan<T>)a, b);
        /// <summary>
        /// XOR 畳み込み。c[i^j] = ∑ <paramref name="a"/>[i] <paramref name="b"/>[j] となる c を返します。
        /// </summary>
        [凾(256)]
        public static T[] Convolution<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b) where T : INumberBase<T>
            => BitConv.Conv<T, Op<T>>(a, b);

        readonly struct Op<T> : IBitConvOp<T> where T : INumberBase<T>
        {
            [凾(256)]
            public void Transform(Span<T> a)
            {
                // Walsh-Hadamard transform
                for (int i = 1; i < a.Length; i <<= 1)
                    foreach (var j in (~i & a.Length - 1).BitSubset(false))
                    {
                        T x = a[j], y = a[j | i];
                        a[j] = x + y;
                        a[j | i] = x - y;
                    }
            }

            [凾(256)]
            public void Inverse(Span<T> a)
            {
                Transform(a);
                if (typeof(IModInt<T>).IsAssignableFrom(typeof(T)))
                {
                    var inv = T.One / T.CreateChecked(a.Length);
                    for (int i = 0; i < a.Length; i++)
                        a[i] *= inv;
                }
                else
                {
                    var n = T.CreateChecked(a.Length);
                    for (int i = 0; i < a.Length; i++)
                        a[i] /= n;
                }
            }
        }
    }
}