using AtCoder;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/ntt/ntt.hpp
// https://nyaannyaan.github.io/library/ntt/ntt-avx2.hpp
namespace Kzrnm.Competitive
{
    /// <summary>
    /// 数論変換 number-theoretic transform
    /// </summary>
    public static partial class NumberTheoreticTransform<T> where T : struct, IStaticMod
    {
        static readonly StaticModInt<T> pr = BuildPr();
        static StaticModInt<T> BuildPr()
        {
            var op = new T();
            if (op.Mod == 2) return 1;
            var ds = new ulong[32];
            int idx = 0;
            uint m = op.Mod - 1;
            for (uint i = 2; (ulong)i * i <= m; ++i)
            {
                if (m % i == 0)
                {
                    ds[idx++] = i;
                    while (m % i == 0) m /= i;
                }
            }
            if (m != 1) ds[idx++] = m;

            uint pr = 2;
            while (true)
            {
                for (int i = 0; i < idx; ++i)
                {
                    ulong a = pr, r = 1;
                    uint b = (uint)((op.Mod - 1) / ds[i]);
                    while (b != 0)
                    {
                        if ((b & 1) != 0)
                            r = r * a % op.Mod;
                        a = a * a % op.Mod;
                        b >>= 1;
                    }
                    if (r == 1)
                        goto NEXT;
                }
                return pr;
            NEXT: ++pr;
            }
        }

        static StaticModInt<T>[] dws, dys;
        static readonly LazyMontgomeryModInt<T>[] dw, dy;

        /// <summary>
        /// NTT(数論変換)で使えるかどうかを返します。
        /// </summary>
        [凾(256)]
        internal static bool CanNtt()
        {
            var m = new T();
            return m.IsPrime && BitOperations.TrailingZeroCount(m.Mod - 1) >= 23;
        }
        static NumberTheoreticTransform()
        {
            if (!CanNtt())
                return;

            var level = BitOperations.TrailingZeroCount(new T().Mod - 1);
            dws = new StaticModInt<T>[level];
            dys = new StaticModInt<T>[level];
            SetWy(level);
            dw = new LazyMontgomeryModInt<T>[level];
            dy = new LazyMontgomeryModInt<T>[level];
            for (int i = 0; i < dw.Length; i++) dw[i] = dws[i].Value;
            for (int i = 0; i < dy.Length; i++) dy[i] = dys[i].Value;
        }
        static void SetWy(int k)
        {
            var w = new StaticModInt<T>[k];
            var y = new StaticModInt<T>[k];
            w[k - 1] = pr.Pow((new T().Mod - 1) / (1u << k));
            y[k - 1] = w[k - 1].Inv();
            for (int i = k - 2; i > 0; --i)
            {
                w[i] = w[i + 1] * w[i + 1];
                y[i] = y[i + 1] * y[i + 1];
            }
            dws[0] = dys[0] = w[1] * w[1];
            dws[1] = w[1];
            dys[1] = y[1];
            dws[2] = w[2];
            dys[2] = y[2];
            for (int i = 3; i < y.Length; ++i)
            {
                dws[i] = dws[i - 1] * y[i - 2] * w[i];
                dys[i] = dys[i - 1] * w[i - 2] * y[i];
            }
        }

        static StaticModInt<T>[] MultiplyNative(ReadOnlySpan<StaticModInt<T>> a, ReadOnlySpan<StaticModInt<T>> b)
        {
            if (a.Length == 0 || b.Length == 0)
                return Array.Empty<StaticModInt<T>>();
            int l = a.Length + b.Length - 1;
            var s = new StaticModInt<T>[l];
            for (int i = 0; i < a.Length; ++i)
                for (int j = 0; j < b.Length; ++j)
                    s[i + j] += a[i] * b[j];
            return s;
        }
        [凾(256)]
        public static void Ntt(Span<StaticModInt<T>> a)
        {
            if (Avx2.IsSupported)
                NttSimd(a);
            else
                NttLogical(a);
        }
        [凾(256)]
        public static void INtt(Span<StaticModInt<T>> a)
        {
            if (Avx2.IsSupported)
                INttSimd(a);
            else
                INttLogical(a);
        }

        /// <summary>
        /// c[i+j] = Σ <paramref name="a"/>[i] * <paramref name="b"/>[j] となる畳み込みを行います。
        /// </summary>
        [凾(256)]
        public static StaticModInt<T>[] Multiply(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
            => Multiply(MemoryMarshal.Cast<uint, StaticModInt<T>>(a), MemoryMarshal.Cast<uint, StaticModInt<T>>(b));


        /// <summary>
        /// c[i+j] = Σ <paramref name="a"/>[i] * <paramref name="b"/>[j] となる畳み込みを行います。
        /// </summary>
        [凾(256)]
        public static StaticModInt<T>[] Multiply(ReadOnlySpan<StaticModInt<T>> a, ReadOnlySpan<StaticModInt<T>> b)
            => Avx2.IsSupported ? MultiplySimd(a, b) : MultiplyLogical(a, b);

        [凾(256)]
        public static void NttDoubling(ref StaticModInt<T>[] a)
        {
            int M = a.Length;
            var b = (StaticModInt<T>[])a.Clone();
            INtt(b);
            var r = StaticModInt<T>.Raw(1);
            var zeta = pr.Pow((new T().Mod - 1) / ((uint)M << 1));

            for (int i = 0; i < b.Length; i++)
            {
                b[i] *= r;
                r *= zeta;
            }
            Ntt(b);
            Array.Resize(ref a, 2 * M);
            b.AsSpan().CopyTo(a.AsSpan(M));
        }
    }
}
