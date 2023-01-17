using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/math/combinatorics/mod-log.hpp
    /// <summary>
    /// 離散対数問題
    /// </summary>
    public static class ModLog
    {
        /// <summary>
        /// <para><paramref name="a"/>^x ≡ <paramref name="b"/> (mod <paramref name="p"/>) をみたす最小の x を返します。存在しない場合は -1 を返します。</para>
        /// </summary>
        [凾(256)]
        public static long Solve(long a, long b, long p, bool includeZero = true)
        {
            a %= p;
            b %= p;

            if (b == 1 && includeZero)
                return 0;
            if (p == 1)
                return includeZero ? 0 : 1;

            if (a == 0)
                return b == 0 ? 1 : -1;

            long g = 1;

            for (var i = p; i > 0; i >>= 1)
                g = g * a % p;
            g = MathLibEx.Gcd(g, p);

            long t = a, c = 1;
            for (; t % g != 0; c++)
            {
                if (t == b)
                    return c;
                t = t * a % p;
            }
            if (b % g != 0) return -1;

            t /= g;
            b /= g;

            long n = p / g, h = 0, gs = 1;

            for (; h * h < n; h++)
                gs = gs * a % n;

            var bs = new Dictionary<long, long>();
            for (long s = 0, e = b; s < h; bs[e] = ++s)
                e = e * a % n;

            for (long s = 0, e = t; s < n;)
            {
                e = e * gs % n;
                s += h;
                if (bs.TryGetValue(e, out var v))
                    return c + s - v;
            }
            return -1;
        }
    }
}
