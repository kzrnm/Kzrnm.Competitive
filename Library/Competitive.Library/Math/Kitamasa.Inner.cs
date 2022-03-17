using AtCoder;
using AtCoder.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static partial class Kitamasa
    {
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
                    if (current[0] >= (uint)mod)
                        current[0] -= (uint)mod;
                    ic = ConvolutionAnyMod.Convolution(ic, current[..t], mod).AsSpan(0, t);
                }
            }

            [凾(256)]
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

            [凾(256)]
            public uint[] Convolution(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
                  => ConvolutionAnyMod.Convolution(a, b, (int)mod);

            [凾(256)]
            public uint Calculate(long n)
            {
                var b = new uint[a.Length];
                b[^1] = 1;

                var x = new uint[a.Length];
                x[^2] = 1;
                while (n > 0)
                {
                    if ((n & 1) != 0)
                        b = MultiplyMod(b, x);
                    n >>= 1;
                    x = MultiplyMod(x, x);
                }

                uint result = default;
                for (int i = 0; i < a.Length; i++)
                {
                    result += (uint)((ulong)a[i] * b[b.Length - i - 1] % (uint)mod);
                    if (result >= (uint)mod)
                        result -= (uint)mod;
                }
                return result;
            }
        }

        private ref struct Inner<TMod> where TMod : struct, IStaticMod
        {
            public static readonly TMod op = default;
            public ReadOnlySpan<uint> a;
            public ReadOnlySpan<uint> c;
            public ReadOnlySpan<uint> ic;

            public Inner(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c)
            {
                Contract.Assert(a.Length == c.Length, reason: "漸化式の係数 c と数列 a の数が違います");
                this.a = a;
                CreateC(c, out this.c, out this.ic);
            }
            static void CreateC(ReadOnlySpan<uint> origC,
                out ReadOnlySpan<uint> c,
                out ReadOnlySpan<uint> ic)
            {
                var cc = new uint[origC.Length + 1];
                origC.CopyTo(cc.AsSpan(1));
                cc[0] = (uint)(op.Mod - 1);
                c = cc;

                ic = new uint[1] { 1 };
                var t = 1;
                while (t < c.Length)
                {
                    t = Math.Min(t << 1, c.Length);
                    var current = ConvolutionAnyMod.Convolution<TMod>(c[..t], ic).AsSpan();
                    current[0] += 2;
                    if (current[0] >= op.Mod)
                        current[0] -= op.Mod;
                    ic = ConvolutionAnyMod.Convolution<TMod>(ic, current[..t]).AsSpan(0, t);
                }
            }

            [凾(256)]
            public uint[] MultiplyMod(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
            {
                var beta = Convolution(a, b);
                var q = Convolution(beta, ic).AsSpan(0, a.Length - 1);
                var right = Convolution(c, q);
                var result = new uint[a.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = (uint)(((ulong)beta[i + a.Length - 1] + right[i + a.Length - 1]) % op.Mod);
                return result;
            }

            [凾(256)]
            public uint[] Convolution(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
                   => ConvolutionAnyMod.Convolution<TMod>(a, b);

            [凾(256)]
            public uint Calculate(long n)
            {
                var b = new uint[a.Length];
                b[^1] = 1;

                var x = new uint[a.Length];
                x[^2] = 1;
                while (n > 0)
                {
                    if ((n & 1) != 0)
                        b = MultiplyMod(b, x);
                    n >>= 1;
                    x = MultiplyMod(x, x);
                }

                uint result = default;
                for (int i = 0; i < a.Length; i++)
                {
                    result += (uint)((ulong)a[i] * b[b.Length - i - 1] % op.Mod);
                    if (result >= op.Mod)
                        result -= op.Mod;
                }
                return result;
            }
        }
    }
}