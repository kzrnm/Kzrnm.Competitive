using AtCoder;
using AtCoder.Internal;
using System;
using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class XorConvolution
    {
        [凾(256)]
        public static T[] Convolution<T>(T[] a, T[] b) where T : INumberBase<T>
            => Convolution((ReadOnlySpan<T>)a, b);
        [凾(256)]
        public static T[] Convolution<T>(Span<T> a, Span<T> b) where T : INumberBase<T>
            => Convolution((ReadOnlySpan<T>)a, b);
        [凾(256)]
        public static T[] Convolution<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b) where T : INumberBase<T>
        {
            if (a.Length != b.Length) ThrowLengthDiff();
            if (!BitOperations.IsPow2(a.Length)) ThrowNotPow2(nameof(a));
            if (!BitOperations.IsPow2(b.Length)) ThrowNotPow2(nameof(b));

            if (a.Length == 0) return Array.Empty<T>();

            var f = new T[a.Length];
            {
                var gp = ArrayPool<T>.Shared.Rent(b.Length);
                ref var g = ref gp[0];

                a.CopyTo(f);
                b.CopyTo(gp);

                WalshHadamardTransform<T>(f);
                WalshHadamardTransform(gp.AsSpan(0, b.Length));

                for (int i = 0; i < f.Length; i++)
                    f[i] *= Unsafe.Add(ref g, i);
                ArrayPool<T>.Shared.Return(gp);
            }

            WalshHadamardTransform<T>(f);

            var inv = T.One / T.CreateChecked(f.Length);
            for (int i = 0; i < f.Length; i++)
                f[i] *= inv;

            return f;

            static void ThrowLengthDiff() => throw new ArgumentException("All length of inputs must be same.", "b");
            static void ThrowNotPow2(string n) => throw new ArgumentException("A length of input must be power of 2.", n);
        }

        [凾(256)]
        static void WalshHadamardTransform<T>(Span<T> a) where T : INumberBase<T>
        {
            for (int i = 1; i < a.Length; i <<= 1)
                foreach (var j in (~i & a.Length - 1).BitSubset(false))
                {
                    T x = a[j], y = a[j | i];
                    a[j] = x + y;
                    a[j | i] = x - y;
                }
        }
    }
}
