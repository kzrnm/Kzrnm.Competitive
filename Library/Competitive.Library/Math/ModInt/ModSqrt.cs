using AtCoder;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive
{
    using static MathLib;
    public static class ModSqrt
    {
        /// <summary>
        /// <paramref name="a"/> ≡ x^2 mod <paramref name="p"/> をみたす x を返します。なければ -1 を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="p"/> は素数</para>
        /// <para>計算量: O(log^2 <paramref name="p"/>)</para>
        /// </remarks>
        [凾(256)]
        public static int Solve(long a, int p)
        {
            if (a == 0) return 0;
            if (p <= 2) return (int)a;
            if (PowMod(a, (p - 1) >> 1, p) != 1) return -1;
            int b = 1;
            while (PowMod(b, (p - 1) >> 1, p) == 1) ++b;
            int m = (int)p - 1;
            int e = BitOperations.TrailingZeroCount(m);
            m >>= e;
            long x = PowMod(a, (m - 1) >> 1, p), y = a * (x * x % p) % p;
            x = (x * a) % p;
            long z = PowMod(b, m, p);
            while (y != 1)
            {
                int j = 0;
                long t = y;
                while (t != 1)
                {
                    j += 1;
                    t = (t * t) % p;
                }
                z = PowMod(z, 1L << (e - j - 1), p);
                x = x * z % p;
                z = z * z % p;
                y = y * z % p;
                e = j;
            }
            return (int)x;
        }
        /// <summary>
        /// <paramref name="a"/> ≡ x^2 mod <typeparamref name="T"/> をみたす x を返します。なければ -1 を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="T"/> は素数</para>
        /// <para>計算量: O(log^2 <typeparamref name="T"/>)</para>
        /// </remarks>
        [凾(256)]
        public static int Solve<T>(StaticModInt<T> a) where T : struct, IStaticMod
        {
            var op = new T();
            if (a == 0) return 0;
            if (op.Mod <= 2) return a.Value;
            if (a.Pow((op.Mod - 1) >> 1).Value != 1) return -1;
            int b = 1;
            while (new StaticModInt<T>(b).Pow((op.Mod - 1) >> 1).Value == 1) ++b;
            int m = (int)op.Mod - 1;
            int e = BitOperations.TrailingZeroCount(m);
            m >>= e;
            var x = a.Pow((m - 1) >> 1);
            var y = a * x * x;
            x *= a;
            var z = new StaticModInt<T>(b).Pow(m);
            while (y != 1)
            {
                int j = 0;
                var t = y;
                while (t != 1)
                {
                    j += 1;
                    t *= t;
                }
                z = z.Pow(1L << (e - j - 1));
                x *= z;
                z *= z;
                y *= z;
                e = j;
            }
            return x.Value;
        }
        /// <summary>
        /// <paramref name="a"/> ≡ x^2 mod <typeparamref name="T"/> をみたす x を返します。なければ -1 を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="T"/> は素数</para>
        /// <para>計算量: O(log^2 <typeparamref name="T"/>)</para>
        /// </remarks>
        [凾(256)]
        public static int Solve<T>(DynamicModInt<T> a) where T : struct
        {
            var p = DynamicModInt<T>.Mod;
            if (a == 0) return 0;
            if (p <= 2) return a.Value;
            if (a.Pow((p - 1) >> 1).Value != 1) return -1;
            int b = 1;
            while (new DynamicModInt<T>(b).Pow((p - 1) >> 1).Value == 1) ++b;
            int m = p - 1;
            int e = BitOperations.TrailingZeroCount(m);
            m >>= e;
            var x = a.Pow((m - 1) >> 1);
            var y = a * x * x;
            x *= a;
            var z = new DynamicModInt<T>(b).Pow(m);
            while (y != 1)
            {
                int j = 0;
                var t = y;
                while (t != 1)
                {
                    j += 1;
                    t *= t;
                }
                z = z.Pow(1L << (e - j - 1));
                x *= z;
                z *= z;
                y *= z;
                e = j;
            }
            return x.Value;
        }
    }
}
