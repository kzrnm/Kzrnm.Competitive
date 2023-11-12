using Kzrnm.Competitive.Internal;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// AND 畳み込み
    /// </summary>
    public static class AndConvolution
    {
        /// <summary>
        /// AND 畳み込み。c[i&amp;j] = ∑ <paramref name="a"/>[i] <paramref name="b"/>[j] となる c を返します。
        /// </summary>
        [凾(256)]
        public static T[] Convolution<T>(T[] a, T[] b) where T : INumberBase<T>
            => Convolution((ReadOnlySpan<T>)a, b);
        /// <summary>
        /// AND 畳み込み。c[i&amp;j] = ∑ <paramref name="a"/>[i] <paramref name="b"/>[j] となる c を返します。
        /// </summary>
        [凾(256)]
        public static T[] Convolution<T>(Span<T> a, Span<T> b) where T : INumberBase<T>
            => Convolution((ReadOnlySpan<T>)a, b);
        /// <summary>
        /// AND 畳み込み。c[i&amp;j] = ∑ <paramref name="a"/>[i] <paramref name="b"/>[j] となる c を返します。
        /// </summary>
        [凾(256)]
        public static T[] Convolution<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b) where T : INumberBase<T>
            => BitConv.Conv<T, Op<T>>(a, b);

        readonly struct Op<T> : IBitConvOp<T> where T : INumberBase<T>
        {
            [凾(256)]
            public void Transform(Span<T> a)
                => ZetaMoebiusTransform.SupersetZetaTransform(a);

            [凾(256)]
            public void Inverse(Span<T> a)
                => ZetaMoebiusTransform.SupersetMoebiusTransform(a);
        }
    }
}
