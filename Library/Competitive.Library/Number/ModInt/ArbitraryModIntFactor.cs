using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://nyaannyaan.github.io/library/modulo/arbitrary-mod-binomial.hpp.html
    /// <summary>
    /// <para>O(min(N logM/loglogM, M)) で初期構築したあとは、中国剰余定理で二項係数を クエリO(logN logM/loglogM) で求められる。</para>
    /// <para>(M ≦ 1e7 and max(N) ≦ 1e18) or (M &lt; 2^30 and max(N) ≦ 2e7)</para>
    /// </summary>
    public class ArbitraryModIntFactor
    {
        internal uint Mod;
        long[] M;
        DynamicPrimeFactor[] cs;
        public ArbitraryModIntFactor(int mod)
        {
            Mod = (uint)mod;
            var listM = new List<long>();
            var listC = new List<DynamicPrimeFactor>();

            int c = BitOperations.TrailingZeroCount(mod);
            if (c > 0)
            {
                listM.Add(1L << c);
                listC.Add(new DynamicPrimeFactor(2, c));
                mod >>= c;
            }

            for (int i = 3; i * i <= mod; i += 2)
            {
                c = 0;
                int k = 1;
                while (Math.DivRem(mod, i, out var am) is var d && am == 0)
                {
                    mod = d;
                    k *= i;
                    ++c;
                }
                if (c > 0)
                {
                    listM.Add(k);
                    listC.Add(new DynamicPrimeFactor(i, c));
                    Debug.Assert(listC[^1].M == listM[^1]);
                }
            }

            if (mod != 1)
            {
                listM.Add(mod);
                listC.Add(new DynamicPrimeFactor(mod, 1));
            }

            M = listM.ToArray();
            cs = listC.ToArray();
        }

        /// <summary>組み合わせ関数(二項係数)</summary>
        [凾(256)]
        public uint Combination(long n, long k)
        {
            if (Mod == 1) return 0;
            long[] rem = new long[cs.Length];
            for (int i = 0; i < cs.Length; i++)
            {
                rem[i] = cs[i].Combination(n, k);
            }
            return (uint)MathLib.Crt(rem, M).y;
        }

        ///<summary>重複組み合わせ関数</summary>
        [凾(256)]
        public uint Homogeneous(int n, int k) => Combination(n + k - 1, k);
    }

    namespace Internal
    {
        internal class DynamicPrimeFactor
        {
            internal uint p, q, M;
            uint[] fac, finv;
            uint delta;
            Barrett bm, bp;

            /// <summary>
            /// <paramref name="p"/>^<paramref name="q"/> の二項係数を保持
            /// </summary>
            /// <param name="p"></param>
            /// <param name="q"></param>
            public DynamicPrimeFactor(int p, int q)
            {
                this.p = (uint)p;
                this.q = (uint)q;
                var m = 1u;
                while (--q >= 0)
                    m *= (uint)p;
                M = m;
                bm = new Barrett(m);
                bp = new Barrett((uint)p);
                (fac, finv) = Enumerate(M, (uint)p, bm);
                delta = (p == 2 && this.q >= 3) ? 1 : M - 1;
            }
            static (uint[] fac, uint[] finv) Enumerate(uint M, uint p, Barrett bm)
            {
                const int MAX_N = 20000000;
                int size = Math.Min((int)M, MAX_N + 1);
                var fac = new uint[size];
                var finv = new uint[size];
                fac[0] = finv[0] = 1;
                fac[1] = finv[1] = 1;
                for (int i = 2; i < size; i++)
                {
                    if (i % p == 0)
                    {
                        fac[i] = fac[i - 1];
                        fac[i + 1] = bm.Mul(fac[i - 1], (uint)(i + 1));
                        i++;
                    }
                    else
                    {
                        fac[i] = bm.Mul(fac[i - 1], (uint)i);
                    }
                }

                finv[^1] = bm.Pow(fac[^1], M / p * (p - 1) - 1);
                for (int i = size - 2; i > 1; --i)
                {
                    if (i % p == 0)
                    {
                        finv[i] = bm.Mul(finv[i + 1], (uint)(i + 1));
                        finv[i - 1] = finv[i];
                        i--;
                    }
                    else
                    {
                        finv[i] = bm.Mul(finv[i + 1], (uint)(i + 1));
                    }
                }
                return (fac, finv);
            }

            uint Lucas(ulong n, ulong k)
            {
                var r = 1u;
                while (n > 0)
                {
                    uint n0, k0;
                    (n, n0) = bp.DivRem(n);
                    (k, k0) = bp.DivRem(k);
                    if (n0 < k0) return 0;
                    r = bm.Mul(
                        bm.Mul(r, fac[(int)n0]),
                        bm.Mul(finv[(int)(n0 - k0)], finv[(int)k0])
                    );
                }
                return r;
            }

            ///<summary>組み合わせ関数(二項係数)</summary>
            [凾(256)]
            public uint Combination(long n, long k)
            {
                if (n < 0 || k < 0) return 0;
                return Combination((ulong)n, (ulong)k);
            }
            ///<summary>組み合わせ関数(二項係数)</summary>
            [凾(256)]
            public uint Combination(ulong n, ulong k)
            {
                if (n < k)
                    return 0;
                if (q == 1)
                    return Lucas(n, k);
                var r = n - k;
                int e0 = 0, eq = 0, i = 0;
                uint res = 1;
                while (n > 0)
                {
                    res = bm.Mul(res, fac[(int)bm.Reduce(n)]);
                    res = bm.Mul(res, finv[(int)bm.Reduce(k)]);
                    res = bm.Mul(res, finv[(int)bm.Reduce(r)]);
                    n = bp.Div(n);
                    k = bp.Div(k);
                    r = bp.Div(r);
                    int eps = (int)(n - k - r);
                    e0 += eps;
                    if (e0 >= q) return 0;
                    if (++i >= q) eq += eps;
                }
                if ((eq & 1) != 0)
                    res = bm.Mul(res, delta);
                return bm.Mul(res, bm.Pow(p, e0));
            }
        }
    }
}
