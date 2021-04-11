using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kzrnm.Competitive
{
    public static class Kitamasa
    {
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa(uint[] a, uint[] c, long n, int mod)
            => FastKitamasa((ReadOnlySpan<uint>)a, c, n, mod);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa(Span<uint> a, Span<uint> c, long n, int mod)
            => FastKitamasa((ReadOnlySpan<uint>)a, c, n, mod);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, long n, int mod)
        {
            Contract.Assert(a.Length == c.Length, reason: "漸化式の係数 c と数列 a の数が違います");
            var inner = new Inner(a, c, mod);
            var b = new uint[a.Length];
            b[^1] = 1;

            var x = new uint[a.Length];
            x[^2] = 1;
            while (n > 0)
            {
                if ((n & 1) != 0)
                    b = inner.MultiplyMod(b, x);
                n >>= 1;
                x = inner.MultiplyMod(x, x);
            }

            uint result = default;
            for (int i = 0; i < a.Length; i++)
            {
                result += (uint)((ulong)a[i] * b[b.Length - i - 1] % (uint)mod);
                result %= (uint)mod;
            }
            return result;
        }

        private ref struct Inner
        {
            public readonly uint mod;
            public ReadOnlySpan<uint> a;
            public ReadOnlySpan<uint> c;
            public ReadOnlySpan<uint> ic;

            public Inner(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, int mod)
            {
                Contract.Assert(a.Length == c.Length, reason: "漸化式の係数 c と数列 a の数が違います");
                this.a = a;
                this.mod = (uint)mod;
                CreateC(c, mod, out this.c, out this.ic);
            }
            static void CreateC(ReadOnlySpan<uint> origC,
                int mod,
                out ReadOnlySpan<uint> c,
                out ReadOnlySpan<uint> ic)
            {
                var cc = new uint[origC.Length + 1];
                origC.CopyTo(cc.AsSpan(1));
                cc[0] = (uint)(mod - 1);
                c = cc;

                ic = new uint[1] { 1 };
                var t = 1;
                while (t < c.Length)
                {
                    t = Math.Min(t << 1, c.Length);
                    var current = ConvolutionAnyMod.Convolution(c[..t], ic, mod).AsSpan();
                    current[0] += 2;
                    current[0] %= (uint)mod;
                    ic = ConvolutionAnyMod.Convolution(ic, current[..t], mod).AsSpan(0, t);
                }
            }

            public uint[] MultiplyMod(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
            {
                var beta = Convolution(a, b);
                var q = Convolution(beta, ic).AsSpan(0, a.Length - 1);
                var right = Convolution(c, q);
                var result = new uint[a.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = (uint)(((ulong)beta[i + a.Length - 1] + right[i + a.Length - 1]) % mod);
                return result;
            }

            public uint[] Convolution(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
                => ConvolutionAnyMod.Convolution(a, b, (int)mod);
        }
    }
}
