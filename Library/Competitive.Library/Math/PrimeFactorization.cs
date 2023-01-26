using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://nyaannyaan.github.io/library/prime/fast-factorize.hpp.html
    /// <summary>
    /// O(N^1/4) の高速な素因数分解を行います。
    /// </summary>
    public abstract class PrimeFactorization : PrimeFactorization<PrimeId.ModId>
    {
    }
    namespace PrimeId { public struct ModId { } }


    /// <summary>
    /// O(N^1/4) の高速な素因数分解を行います。
    /// </summary>
    public abstract class PrimeFactorization<ModId>
        where ModId : struct
    {
        /// <summary>
        /// <paramref name="n"/> が素数であるかどうかを判定します。
        /// </summary>
        [凾(256)]
        public static bool IsPrime(long n)
        {
            if (n <= 7)
            {
                if (n <= 1) return false;
                return n == 2 || (n & 1) != 0;
            }
            if ((n & 1) == 0) return false;

#if NET7_0_OR_GREATER
            if (n < (1L << 30))
            {
                int ni = (int)n;
                if (DynamicMontgomeryModInt<ModId>.Mod != ni)
                    DynamicMontgomeryModInt<ModId>.Mod = ni;
                return MillerRabin((int)n,
                    n <= 61
                    ? new DynamicMontgomeryModInt<ModId>[] { 2, 7 }
                    : new DynamicMontgomeryModInt<ModId>[] { 2, 7, 61 });
            }
            else
            {
                if (DynamicMontgomeryModInt64<ModId>.Mod != n)
                    DynamicMontgomeryModInt64<ModId>.Mod = n;
                return MillerRabin(n,
                    n <= 1795265022
                    ? new DynamicMontgomeryModInt64<ModId>[] { 2, 325, 9375, 28178, 450775, 9780504 }
                    : new DynamicMontgomeryModInt64<ModId>[] { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 });
            }
#else
            if (n < (1L << 30))
            {
                int ni = (int)n;
                if (DynamicMontgomeryModInt<ModId>.Mod != ni)
                    DynamicMontgomeryModInt<ModId>.Mod = ni;
                return MillerRabinSmall((int)n);
            }
            else
            {
                if (DynamicMontgomeryModInt64<ModId>.Mod != n)
                    DynamicMontgomeryModInt64<ModId>.Mod = n;
                return MillerRabinLarge(n);
            }
#endif
        }
#if NET7_0_OR_GREATER
        /// <summary>
        /// ミラーラビン法で素数であるか判定します。
        /// </summary>
        internal static bool MillerRabin<TInt, TMod>(TInt n, TMod[] rnds)
            where TInt : IBinaryInteger<TInt>
            where TMod : INumberBase<TMod>
        {
            var nm1 = n;
            --nm1;
            var d = nm1;
            var e = TMod.One;
            var rev = TMod.CreateChecked(d);

            d >>= int.CreateChecked(TInt.TrailingZeroCount(d));
            var two = TInt.One + TInt.One;

            foreach (var a in rnds)
            {
                var t = d;
                var y = a.Pow(long.CreateChecked(t));
                while (t != nm1 && y != e && y != rev)
                {
                    y *= y;
                    t *= two;
                }
                if (y != rev && TInt.IsZero(t % two))
                    return false;
            }
            return true;
        }
#else
        /// <summary>
        /// ミラーラビン法で素数であるか判定します。
        /// </summary>
        internal static bool MillerRabinSmall(int n)
        {
            var rnds =
                n <= 61
                ? new DynamicMontgomeryModInt<ModId>[] { 2, 7 }
                : new DynamicMontgomeryModInt<ModId>[] { 2, 7, 61 };

            var d = n - 1;
            var e = DynamicMontgomeryModInt<ModId>.One;
            var rev = d;

            d >>= BitOperations.TrailingZeroCount(d);

            foreach (var a in rnds)
            {
                var t = d;
                var y = a.Pow(t);
                while (t != n - 1 && y != e && y != rev)
                {
                    y *= y;
                    t *= 2;
                }
                if (y != rev && t % 2 == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// ミラーラビン法で素数であるか判定します。
        /// </summary>
        internal static bool MillerRabinLarge(long n)
        {
            var rnds =
                n <= 1795265022
                ? new DynamicMontgomeryModInt64<ModId>[] { 2, 325, 9375, 28178, 450775, 9780504 }
                : new DynamicMontgomeryModInt64<ModId>[] { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 };
            var d = n - 1;
            var e = DynamicMontgomeryModInt64<ModId>.One;
            var rev = d;

            d >>= BitOperations.TrailingZeroCount(d);

            foreach (var a in rnds)
            {
                var t = d;
                var y = a.Pow(t);
                while (t != n - 1 && y != e && y != rev)
                {
                    y *= y;
                    t *= 2;
                }
                if (y != rev && t % 2 == 0)
                    return false;
            }
            return true;
        }
#endif

        [凾(256)]
        static long NextInt64(Random rnd)
        {
            long a = rnd.Next(1 << 22);
            long b = rnd.Next(1 << 21);
            long c = rnd.Next(1 << 20);

            return (a << 41) | (b << 20) | c;
        }

        /// <summary>
        /// ポラード・ロー法 で <paramref name="n"/> の素因数を返します。
        /// </summary>
        internal static long PollardRhoLarge(long n)
        {
            if ((n & 1) == 0)
                return 2;
            if (IsPrime(n))
                return n;

            var rnd =
#if NET7_0_OR_GREATER
                Random.Shared;
#else
                new Random();
#endif
            var one = DynamicMontgomeryModInt64<ModId>.One;
            var R = one;
            DynamicMontgomeryModInt64<ModId> F(DynamicMontgomeryModInt64<ModId> x) => x * x + R;
            while (true)
            {
                var x = one;
                var y = one;
                var ys = one;
                var q = one;
#if NET7_0_OR_GREATER
                R = rnd.NextInt64(2, n);
                y = rnd.NextInt64(2, n);
#else
                R = NextInt64(rnd) / (n - 2) + 2;
                y = NextInt64(rnd) / (n - 2) + 2;
#endif
                long g = 1;
                const int m = 128;
                for (int r = 1; g == 1; r <<= 1)
                {
                    x = y;
                    for (int i = 0; i < r; ++i) y = F(y);
                    for (int k = 0; g == 1 && k < r; k += m)
                    {
                        ys = y;
                        for (int i = 0; i < m && i < r - k; ++i)
                            q *= x - (y = F(y));
                        g = MathLibEx.Gcd(q.Value, n);
                    }
                }
                if (g == n)
                    do
                        g = MathLibEx.Gcd((x - (ys = F(ys))).Value, n);
                    while (g == 1);
                if (g != n) return g;
            }
        }

        /// <summary>
        /// ポラード・ロー法 で <paramref name="n"/> の素因数を返します。
        /// </summary>
        internal static int PollardRhoSmall(int n)
        {
            if ((n & 1) == 0)
                return 2;
            if (IsPrime(n))
                return n;

            var rnd =
#if NET7_0_OR_GREATER
                Random.Shared;
#else
                new Random();
#endif
            var one = DynamicMontgomeryModInt<ModId>.One;
            var R = one;
            DynamicMontgomeryModInt<ModId> F(DynamicMontgomeryModInt<ModId> x) => x * x + R;
            while (true)
            {
                var x = one;
                var y = one;
                var ys = one;
                var q = one;
#if NET7_0_OR_GREATER
                R = rnd.NextInt64(2, n);
                y = rnd.NextInt64(2, n);
#else
                R = NextInt64(rnd) / (n - 2) + 2;
                y = NextInt64(rnd) / (n - 2) + 2;
#endif
                int g = 1;
                const int m = 128;
                for (int r = 1; g == 1; r <<= 1)
                {
                    x = y;
                    for (int i = 0; i < r; ++i) y = F(y);
                    for (int k = 0; g == 1 && k < r; k += m)
                    {
                        ys = y;
                        for (int i = 0; i < m && i < r - k; ++i)
                            q *= x - (y = F(y));
                        g = MathLibEx.Gcd(q.Value, n);
                    }
                }
                if (g == n)
                    do
                        g = MathLibEx.Gcd((x - (ys = F(ys))).Value, n);
                    while (g == 1);
                if (g != n) return g;
            }
        }

        public static IEnumerable<long> EnumerateFactors(long n)
        {
            if (n <= 1) yield break;
            long p;

            if (n < (1L << 30))
            {
                int ni = (int)n;
                if (DynamicMontgomeryModInt<ModId>.Mod != ni)
                    DynamicMontgomeryModInt<ModId>.Mod = ni;
                p = PollardRhoSmall((int)n);
            }
            else
            {
                if (DynamicMontgomeryModInt64<ModId>.Mod != n)
                    DynamicMontgomeryModInt64<ModId>.Mod = n;
                p = PollardRhoLarge(n);
            }
            if (p == n)
            {
                yield return p;
                yield break;
            }
            foreach (var v in EnumerateFactors(p))
                yield return v;
            foreach (var v in EnumerateFactors(n / p))
                yield return v;
        }

#if NET7_0_OR_GREATER
        /// <summary>
        /// <paramref name="n"/> を素因数分解します。
        /// </summary>
        public static Dictionary<T, int> PrimeFactoring<T>(T n)
            where T : INumberBase<T>
        {
            var res = new Dictionary<T, int>();
            foreach (var x in EnumerateFactors(long.CreateChecked(n)))
            {
                var y = T.CreateChecked(x);
                ++res.GetValueRefOrAddDefault(y);
            }
            return res;
        }

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static T[] Divisor<T>(T n)
            where T : IBinaryInteger<T>
        {
            if (T.IsZero(n)) return Array.Empty<T>();
            if (n <= T.One) return new T[] { T.One };

            var pairs = PrimeFactoring(n).ToArray();
            var list = new List<T>();
            var st = new Stack<(int pi, T x, int pc)>();
            st.Push((0, T.One, ~pairs[0].Value));
            while (st.TryPop(out var tup))
            {
                var (pi, x, pc) = tup;
                if (pc < 0)
                {
                    st.Push((pi, x, ~pc));
                    if (++pi < pairs.Length)
                        st.Push((pi, x, ~pairs[pi].Value));
                    else
                        list.Add(x);
                }
                else if (pc > 0)
                {
                    st.Push((pi, x * pairs[pi].Key, ~--pc));
                }
            }

            list.Sort();
            return list.ToArray();
        }
#else
        /// <summary>
        /// <paramref name="n"/> を素因数分解します。
        /// </summary>
        public static Dictionary<int, int> PrimeFactoring(int n)
        {
            var res = new Dictionary<int, int>();
            foreach (var x in EnumerateFactors(n))
            {
                var y = (int)x;
                res[y] = res.GetValueOrDefault(y) + 1;
            }
            return res;
        }

        /// <summary>
        /// <paramref name="n"/> を素因数分解します。
        /// </summary>
        public static Dictionary<long, int> PrimeFactoring(long n)
        {
            var res = new Dictionary<long, int>();
            foreach (var x in EnumerateFactors(n))
            {
                res[x] = res.GetValueOrDefault(x) + 1;
            }
            return res;
        }

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static long[] Divisor(long n)
        {
            if (n <= 0) return Array.Empty<long>();
            if (n <= 1) return new long[] { 1 };

            var pairs = PrimeFactoring(n).ToArray();
            var list = new List<long>();
            var st = new Stack<(int pi, long x, int pc)>();
            st.Push((0, 1, ~pairs[0].Value));
            while (st.TryPop(out var tup))
            {
                var (pi, x, pc) = tup;
                if (pc < 0)
                {
                    st.Push((pi, x, ~pc));
                    if (++pi < pairs.Length)
                        st.Push((pi, x, ~pairs[pi].Value));
                    else
                        list.Add(x);
                }
                else if (pc > 0)
                {
                    st.Push((pi, x * pairs[pi].Key, ~--pc));
                }
            }

            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static int[] Divisor(int n)
        {
            if (n <= 0) return Array.Empty<int>();
            if (n <= 1) return new int[] { 1 };

            var pairs = PrimeFactoring(n).ToArray();
            var list = new List<int>();
            var st = new Stack<(int pi, int x, int pc)>();
            st.Push((0, 1, ~pairs[0].Value));
            while (st.TryPop(out var tup))
            {
                var (pi, x, pc) = tup;
                if (pc < 0)
                {
                    st.Push((pi, x, ~pc));
                    if (++pi < pairs.Length)
                        st.Push((pi, x, ~pairs[pi].Value));
                    else
                        list.Add(x);
                }
                else if (pc > 0)
                {
                    st.Push((pi, x * pairs[pi].Key, ~--pc));
                }
            }

            list.Sort();
            return list.ToArray();
        }
#endif

    }
}
