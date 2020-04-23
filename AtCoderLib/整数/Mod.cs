using System;


// https://qiita.com/drken/items/3b4fdf0a78e7a138cd9a
readonly struct Mod : IEquatable<Mod>
{
    public const long mod = 1000000007;
    public static readonly Mod invalid = new Mod(-1, false);
    public readonly long val;
    private Mod(long val, bool isValid) : this(val) { if (!isValid) this.val = val; }
    public Mod(long val) { this.val = val % mod; if (this.val < 0) this.val += mod; }
    public override bool Equals(object obj) => (obj is Mod) ? this == ((Mod)obj) : false;
    public bool Equals(Mod obj) => this == obj;
    public override int GetHashCode() => val.GetHashCode();
    public override string ToString() => val.ToString();
    public static implicit operator Mod(long x) => new Mod(x);
    public static Mod operator +(Mod x, Mod y) => (x.val + y.val) % mod;
    public static Mod operator -(Mod x, Mod y) => x.val >= y.val ? (x.val - y.val) % mod : (x.val - y.val) % mod + mod;
    public static Mod operator *(Mod x, Mod y) => (x.val * y.val) % mod;
    public static Mod operator /(Mod x, Mod y) => x * y.Inverse();
    public static bool operator ==(Mod x, Mod y) => x.val == y.val;
    public static bool operator !=(Mod x, Mod y) => x.val != y.val;
    public Mod Inverse()
    {
        long a = val, b = mod, u = 1, v = 0;
        while (b > 0)
        {
            long t = a / b;
            var b2 = a - t * b;
            a = b; b = b2;
            var v2 = u - t * v;
            u = v; v = v2;
        }
        u %= mod;
        if (u < 0) u += mod;
        return u;
    }
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

    public static Factor CreateFactor(int max) => new Factor(max);
    public class Factor
    {
        private readonly Mod[] fac, finv;
        public Factor(int max)
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

        // 二項係数計算
        public Mod Combine(int n, int k)
        {
            if (n < k) return 0;
            if (n < 0 || k < 0) return 0;
            return fac[n] * finv[k] * finv[n - k];
        }

        public Mod Factorial(int n) => fac[n];
        public Mod FactorialInvers(int n) => finv[n];
    }
}
