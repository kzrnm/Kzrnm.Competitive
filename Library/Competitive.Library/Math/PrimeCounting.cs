using AtCoder.Internal;
using System;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://nyaannyaan.github.io/library/multiplicative-function/prime-counting-faster.hpp
    /// <summary>
    /// 素数の個数
    /// </summary>
    public static class PrimeCounting
    {
        /// <summary>
        /// <paramref name="n"/> 以下の素数の個数を返します。
        /// </summary>
        [凾(256)]
        public static long Count(long n) => (long)Count((ulong)n);

        /// <summary>
        /// <paramref name="n"/> 以下の素数の個数を返します。
        /// </summary>
        [凾(256)]
        public static ulong Count(ulong n)
        {
            var n2 = (uint)Math.Sqrt(n);
            var ndN2 = n / n2;

            var hl = new ulong[ndN2];
            for (int i = 1; i < hl.Length; i++)
                hl[i] = (n / (uint)i) - 1;

            var hs = (uint[])(object)Enumerable.Range(-1, (int)n2 + 1).ToArray();

            for (uint x = 2, pi = 0; x < hs.Length; ++x)
            {
                if (hs[x] == hs[x - 1]) continue;
                ulong x2 = (ulong)x * x;
                var imax = Math.Min(ndN2, (n / x2) + 1);
                ulong ix = x;
                for (ulong i = 1; i < imax; ++i)
                {
                    hl[i] -= (ix < ndN2 ? hl[(uint)ix] : hs[(uint)(n / ix)]) - pi;
                    ix += x;
                }
                for (uint n3 = n2; n3 >= x2; n3--)
                {
                    hs[n3] -= hs[n3 / x] - pi;
                }
                ++pi;
            }
            return hl[1];
        }
    }
}
