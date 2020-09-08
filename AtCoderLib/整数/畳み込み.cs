using System;
using System.Collections.Generic;
using static AtCoderProject.Global;



class Convolution
{
    public readonly int mod;
    /// <param name="mod">素数でなければならない</param>
    public Convolution(int mod)
    {
        this.mod = mod;
        (sum_e, sum_ie) = BuildSum();
    }
    long[] sum_e;  // sum_e[i] = ies[0] * ... * ies[i - 1] * es[i]
    long[] sum_ie; // sum_ie[i] = es[0] * ... * es[i - 1] * ies[i]

    static Dictionary<int, int> primitiveRootCache = new Dictionary<int, int> {
        { 2, 1 },
        {167772161, 3},
        {469762049, 3},
        {754974721, 11},
        {998244353, 3},
    };
    static int PrimitiveRoot(int p)
    {
        if (primitiveRootCache.TryGetValue(p, out var val))
            return val;
        Span<int> divs = stackalloc int[20];
        divs[0] = 2;
        int cnt = 1;
        int x = (p - 1) / 2;
        while (x % 2 == 0) x /= 2;
        for (int i = 3; (long)i * i <= x; i += 2)
            if (x % i == 0)
            {
                divs[cnt++] = i;
                while (x % i == 0)
                    x /= i;
            }
        if (x > 1)
            divs[cnt++] = x;
        for (int g = 2; ; g++)
        {
            bool ok = true;
            for (int i = 0; i < cnt; i++)
            {
                if (PowMod(g, (p - 1) / divs[i], p) == 1)
                {
                    ok = false;
                    break;
                }
            }
            if (ok)
                return primitiveRootCache[p] = g;
        }
    }
    static long PowMod(long x, int n, int mod)
    {
        if (mod == 1) return 0;
        long r = 1;
        long y = x % mod;
        if (y < 0) y += mod;
        while (n > 0)
        {
            if ((n & 1) != 0) r = r * y % mod;
            y = y * y % mod;
            n >>= 1;
        }
        return r;
    }
    static long Inverse(long a, int mod)
    {
        // ax+by=dの解を求める
        // (u,v,a) => (x,y,d)となっている
        // dは最終的にaとbのgcdとなる
        long b = mod, u = 1, v = 0;
        while (b > 0)
        {
            long t = a / b;
            (a, b) = (b, a - t * b);
            (u, v) = (v, u - t * v);
        }
        u %= mod;
        if (u < 0) u += mod;
        return u;
    }

    (long[] sum_e, long[] sum_ie) BuildSum()
    {
        var g = PrimitiveRoot(mod);
        var sum_e = new long[30];
        var sum_ie = new long[30];
        Span<long> es = stackalloc long[30];  // es[i]^(2^(2+i)) == 1
        Span<long> ies = stackalloc long[30];
        int cnt2 = LSB(mod - 1);
        long e = PowMod(g, (mod - 1) >> cnt2, mod);
        long ie = Inverse(e, mod);
        for (int i = cnt2; i >= 2; i--)
        {
            // e^(2^i) == 1
            es[i - 2] = e;
            ies[i - 2] = ie;
            e = e * e % mod;
            ie = ie * ie % mod;
        }
        long now = 1;
        long inow = 1;
        for (int i = 0; i < cnt2 - 2; i++)
        {
            sum_e[i] = es[i] * now % mod;
            sum_ie[i] = ies[i] * inow % mod;
            now = now * ies[i] % mod;
            inow = inow * es[i] % mod;
        }
        return (sum_e, sum_ie);
    }

    void Butterfly(long[] a)
    {
        int h = MSB(a.Length - 1) + 1;

        for (int ph = 1; ph <= h; ph++)
        {
            int w = 1 << (ph - 1), p = 1 << (h - ph);
            long now = 1;
            for (int s = 0; s < w; s++)
            {
                int offset = s << (h - ph + 1);
                for (int i = 0; i < p; i++)
                {
                    long l = a[i + offset];
                    long r = a[i + offset + p] * now % mod;
                    a[i + offset] = l + r;
                    if (a[i + offset] >= mod) a[i + offset] -= mod;
                    a[i + offset + p] = l - r;
                    if (a[i + offset + p] < 0) a[i + offset + p] += mod;
                }
                now = now * sum_e[LSB(~(uint)s)] % mod;
            }
        }
    }
    void ButterflyInv(long[] a)
    {
        int h = MSB(a.Length - 1) + 1;

        for (int ph = h; ph >= 1; ph--)
        {
            int w = 1 << (ph - 1), p = 1 << (h - ph);
            long inow = 1;
            for (int s = 0; s < w; s++)
            {
                int offset = s << (h - ph + 1);
                for (int i = 0; i < p; i++)
                {
                    long l = a[i + offset];
                    long r = a[i + offset + p];
                    a[i + offset] = l + r;
                    if (a[i + offset] >= mod) a[i + offset] -= mod;
                    a[i + offset + p] = l - r;
                    if (a[i + offset + p] < 0) a[i + offset + p] += mod;
                    a[i + offset + p] = a[i + offset + p] * inow % mod;
                }
                inow = inow * sum_ie[LSB(~(uint)s)] % mod;
            }
        }
    }

    /** <summary>畳み込み</summary><returns>c[i]=Sum(<paramref name="arr"/>[j]*<paramref name="brr"/>[i-j]) mod <paramref name="mod"/> 0&lt;j&lt;=iとなる配列</returns>*/
    public long[] 畳み込み(long[] arr, long[] brr)
    {
        if (arr.Length == 0 || brr.Length == 0) return Array.Empty<long>();
        if (Math.Min(arr.Length, brr.Length) <= 60) return Small(arr, brr, mod);

        // arr.Length + brr.Length - 1<=2^y=zとなる最小のz
        var z = 1 << (MSB(arr.Length + brr.Length - 1 - 1) + 1);

        var arrz = new long[z];
        var brrz = new long[z];
        for (int i = 0; i < arr.Length; i++) arrz[i] = arr[i] % mod;
        for (int i = 0; i < brr.Length; i++) brrz[i] = brr[i] % mod;
        Butterfly(arrz);
        Butterfly(brrz);

        for (int i = 0; i < arrz.Length; i++)
            arrz[i] = arrz[i] * brrz[i] % mod;

        ButterflyInv(arrz);

        var res = new long[arr.Length + brr.Length - 1];
        var iz = Inverse(z, mod);
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = arrz[i] * iz % mod;
        }
        return res;

        static long[] Small(long[] arr, long[] brr, long mod)
        {
            if (arr.Length < brr.Length)
                (arr, brr) = (brr, arr);

            var res = new long[arr.Length + brr.Length - 1];
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < brr.Length; j++)
                {
                    res[i + j] += arr[i] * brr[j] % mod;
                    if (res[i + j] >= mod) res[i + j] -= mod;
                }
            return res;
        }
    }

    /** <summary>畳み込み</summary><returns>c[i]=Sum(<paramref name="arr"/>[j]*<paramref name="brr"/>[i-j]) 0&lt;j&lt;=iとなる配列</returns>*/
    public static long[] 畳み込みLong(long[] arr, long[] brr)
    {
        if (arr.Length == 0 || brr.Length == 0) return Array.Empty<long>();

        const ulong MOD1 = 754974721;  // 2^24
        const ulong MOD2 = 167772161;  // 2^25
        const ulong MOD3 = 469762049;  // 2^26
        const ulong M2M3 = MOD2 * MOD3;
        const ulong M1M3 = MOD1 * MOD3;
        const ulong M1M2 = MOD1 * MOD2;
        const ulong M1M2M3 = unchecked(MOD1 * MOD2 * MOD3);


        const ulong i1 = 190329765; // (MOD2*MOD3).inv mod MOD1
        const ulong i2 = 58587104;  // (MOD1*MOD3).inv mod MOD2
        const ulong i3 = 187290749; // (MOD1*MOD2).inv mod MOD3
        var c1 = new Convolution((int)MOD1).畳み込み(arr, brr);
        var c2 = new Convolution((int)MOD2).畳み込み(arr, brr);
        var c3 = new Convolution((int)MOD3).畳み込み(arr, brr);
        ReadOnlySpan<ulong> offset = stackalloc ulong[5] {
            0,
            0,
            M1M2M3,
            2 * M1M2M3,
            3 * M1M2M3
        };

        var res = new long[arr.Length + brr.Length - 1];
        for (int i = 0; i < res.Length; i++)
        {
            ulong x = 0;
            x += ((ulong)c1[i] * i1) % MOD1 * M2M3;
            x += ((ulong)c2[i] * i2) % MOD2 * M1M3;
            x += ((ulong)c3[i] * i3) % MOD3 * M1M2;
            // B = 2^63, -B <= x, r(real value) < B
            // (x, x - M, x - 2M, or x - 3M) = r (mod 2B)
            // r = c1[i] (mod MOD1)
            // focus on MOD1
            // r = x, x - M', x - 2M', x - 3M' (M' = M % 2^64) (mod 2B)
            // r = x,
            //     x - M' + (0 or 2B),
            //     x - 2M' + (0, 2B or 4B),
            //     x - 3M' + (0, 2B, 4B or 6B) (without mod!)
            // (r - x) = 0, (0)
            //           - M' + (0 or 2B), (1)
            //           -2M' + (0 or 2B or 4B), (2)
            //           -3M' + (0 or 2B or 4B or 6B) (3) (mod MOD1)
            // we checked that
            //   ((1) mod MOD1) mod 5 = 2
            //   ((2) mod MOD1) mod 5 = 3
            //   ((3) mod MOD1) mod 5 = 4

            var mm = ((long)x) % (long)MOD1;
            if (mm < 0) mm += (long)MOD1;
            long diff = c1[i] - mm;
            if (diff < 0) diff += (long)MOD1;
            x -= offset[(int)(diff % 5)];
            res[i] = (long)x;
        }
        return res;
    }
}