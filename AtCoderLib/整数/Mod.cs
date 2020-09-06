using AtCoderProject;
using System;
using System.Collections.Generic;
using Unsafe = System.Runtime.CompilerServices.Unsafe;


#region https://qiita.com/drken/items/3b4fdf0a78e7a138cd9a 
#endregion
readonly struct Mod : IEquatable<Mod>
{
    public const long mod = 1000000007;
    const bool useInvCache = true;
    public static readonly Mod invalid;
    public readonly long val;
    public Mod(long val) { this.val = val % mod; if (this.val < 0) this.val += mod; }
    static Mod() { Unsafe.As<Mod, long>(ref invalid) = -1; }
    public override bool Equals(object obj) => (obj is Mod) && this == ((Mod)obj);
    public bool Equals(Mod obj) => this.val == obj.val;
    public override int GetHashCode() => val.GetHashCode();
    public override string ToString() => val.ToString();
    public static implicit operator Mod(long x) => new Mod(x);
    public static implicit operator Mod(ConsoleReader cr) => new Mod(cr.Long);
    public static Mod operator -(Mod x) => new Mod(-x.val);
    public static Mod operator +(Mod x, Mod y) => new Mod(x.val + y.val);
    public static Mod operator -(Mod x, Mod y) => new Mod(x.val - y.val);
    public static Mod operator *(Mod x, Mod y) => new Mod(x.val * y.val);
    public static Mod operator /(Mod x, Mod y) => x * y.Inverse();
    public static bool operator ==(Mod x, Mod y) => x.val == y.val;
    public static bool operator !=(Mod x, Mod y) => x.val != y.val;

    #region Inverse
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    static long EuclideanInverseCore(long a, long mod)
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
        return u;
    }

    static long[] MakeInverseCache()
    {
        var res = new long[1 + (int)Math.Sqrt(mod)];
        res[1] = 1;
        for (int i = 2; i < res.Length; i++)
        {
            res[i] = (-res[mod % i] * (mod / i)) % mod;
        }
        return res;
    }
    static long[] _InvCache;
    static long[] InvCache => _InvCache ??= MakeInverseCache();
    static Mod EuclideanInverseCache(long a)
    {
        var cache = InvCache;
        long b = mod, u = 1, v = 0;
        while (b > 0)
        {
            if (0 <= a && a < cache.Length)
                return cache[a] * u;
            long t = a / b;
            (a, b) = (b, a - t * b);
            (u, v) = (v, u - t * v);
        }
        return u;
    }
    public Mod Inverse() => useInvCache ? EuclideanInverseCache(val) : EuclideanInverseCore(val, mod);
    #endregion Inverse

    public static Mod Pow(Mod x, int y)
    {
        Mod res = 1;
        for (; y > 0; y >>= 1)
        {
            if ((y & 1) == 1) res *= x;
            x *= x;
        }
        return res;
    }
    public static long EuclideanInverse(long a, long mod)
    {
        var u = EuclideanInverseCore(a, mod) % mod;
        if (u < 0) u += mod;
        return u;
    }
    public static Factors CreateFactor(int max) => new Factors(max);
    public class Factors
    {
        readonly Mod[] fac, finv;
        public Factors(int max)
        {
            ++max;
            var inv = new Mod[max];
            fac = new Mod[max]; finv = new Mod[max];
            fac[0] = fac[1] = 1;
            finv[0] = finv[1] = 1;
            inv[1] = 1;
            for (var i = 2; i < max; i++)
            {
                fac[i] = fac[i - 1] * i;
                inv[i] = mod - inv[mod % i].val * (mod / i) % mod;
                finv[i] = finv[i - 1] * inv[i];
            }
        }

        /** <summary>組み合わせ関数(二項係数)</summary> */
        public Mod Combination(int n, int k)
        {
            if (n < k) return 0;
            if (n < 0 || k < 0) return 0;
            return fac[n] * finv[k] * finv[n - k];
        }

        public Mod Factorial(int n) => fac[n];
        public Mod FactorialInvers(int n) => finv[n];
    }
}
static class ModExt
{
    public static Mod Sum(this IEnumerable<Mod> source)
    {
        Mod sum = 0;
        foreach (var v in source) sum += v;
        return sum;
    }
}