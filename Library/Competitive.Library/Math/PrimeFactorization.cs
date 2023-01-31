using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://nyaannyaan.github.io/library/prime/fast-factorize.hpp.html
    /// <summary>
    /// O(N^1/4) の高速な素因数分解を行います。
    /// </summary>
    public abstract class PrimeFactorization : PrimeFactorization<Internal.PrimeId.ModId>
    {
    }
    namespace Internal.PrimeId { public struct ModId { } }


    /// <summary>
    /// O(N^1/4) の高速な素因数分解を行います。
    /// </summary>
    public abstract class PrimeFactorization<ModId>
        where ModId : struct
    {
        // 競技プログラミングではほぼ関係ないが並列実行するなら ModId を共用しないほうがよいので差し替えられるようにしておく

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

        /// <summary>
        /// ポラード・ロー法 で <paramref name="n"/> の素因数を返します。
        /// </summary>
        internal static long PollardRhoLarge(long n)
        {
            if ((n & 1) == 0)
                return 2;
            if (IsPrime(n))
                return n;

            Debug.Assert(DynamicMontgomeryModInt64<ModId>.Mod == n);
            //var one = DynamicMontgomeryModInt64<ModId>.One;
            //DynamicMontgomeryModInt64<ModId> F(DynamicMontgomeryModInt64<ModId> x) => x * x + R;
            for (DynamicMontgomeryModInt64<ModId> st = 1; ; ++st)
            {
                var x = st;
                var y = x * x + st;
                uint t = 100000;
                while (--t > 0)
                {
                    var d = (y - x).Value;
                    if (d == 0) break;
                    var p = MathLibEx.Gcd(d, n);
                    if (p != 1) return p;
                    x = x * x + st;
                    y = y * y + st;
                    y = y * y + st;
                }
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

            Debug.Assert(DynamicMontgomeryModInt<ModId>.Mod == n);
            //var one = DynamicMontgomeryModInt<ModId>.One;
            //DynamicMontgomeryModInt<ModId> F(DynamicMontgomeryModInt<ModId> x) => x * x + R;
            for (DynamicMontgomeryModInt<ModId> st = 1; ; ++st)
            {
                var x = st;
                var y = x * x + st;
                uint t = 100000;
                while (--t > 0)
                {
                    var d = (y - x).Value;
                    if (d == 0) break;
                    var p = MathLibEx.Gcd(d, n);
                    if (p != 1) return p;
                    x = x * x + st;
                    y = y * y + st;
                    y = y * y + st;
                }
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
                res[x] = res.GetValueOrDefault(x) + 1;
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
