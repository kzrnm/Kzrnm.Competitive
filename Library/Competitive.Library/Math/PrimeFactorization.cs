using System;
using System.Buffers;
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
        /// 素因数分解の結果をキャッシュするかどうかを設定します。default: <see langword="true"/>
        /// </summary>
        public static bool CacheEnabled { set; get; } = true;

        /// <summary>
        /// 257 未満の素数。ReadOnlySpan&lt;byte&gt; に直接代入するとアロケーションが発生しない。
        /// </summary>
        /// <returns></returns>
        static ReadOnlySpan<byte> SmallPrimes() => new byte[]
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73,
            79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163,
            167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251
        };

        /// <summary>
        /// <paramref name="n"/> が素数であるかどうかを判定します。
        /// </summary>
        [凾(256)]
        public static bool IsPrime(long n)
        {
            static bool Naive(int n)
            {
                if ((n & 1) == 0)
                    return n == 2;
                if (n == 1) return false;
                if (n == 3) return true;
                foreach (int p in SmallPrimes()[1..])
                {
                    if (p * p > n)
                        break;
                    if (n % p == 0)
                        return false;
                }
                return true;
            }
            if (n < 257 * 257L) return Naive((int)n);
            if ((n & 1) == 0) return false;

            if (n < (1L << 30))
            {
                int ni = (int)n;
                if (DynamicMontgomeryModInt<ModId>.Mod != ni)
                    DynamicMontgomeryModInt<ModId>.Mod = ni;
                return MillerRabin(ni,
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
        }

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

        private static Dictionary<long, long[]> cachedFactors;

        /// <summary>
        /// <paramref name="n"/> の素因数を順不同で列挙します。
        /// </summary>
        public static long[] EnumerateFactors(long n)
        {
            // 普通の用途なら「とりあえず小さい素数で試し割り」のほうが良さそう
            // 競技プログラミング用途だと大きい素数の積が決め打ちで渡されがちなので確実に割り切れる 257×257 未満のときを特別扱いする
            return Inner(n) switch { long[] a => a, var e => e.ToArray() };


            static IEnumerable<long> Inner(long n)
            {
                if (CacheEnabled)
                {
                    var cache = cachedFactors ??= new Dictionary<long, long[]>();
                    if (cache.TryGetValue(n, out var r))
                        return r;

                    return cache[n] = n < 257 * 257L ? Naive((int)n) : PollardRho(n).ToArray();
                }
                return n < 257 * 257L ? Naive((int)n) : PollardRho(n);
            }
            static long[] Naive(int n)
            {
                Span<long> buf = stackalloc long[16];
                int ix = BitOperations.TrailingZeroCount(n);
                n >>= ix;
                buf[..ix].Fill(2);
                foreach (int p in SmallPrimes()[1..])
                {
                    if (p * p > n)
                        break;
                    while (Math.DivRem(n, p, out var am) is var d && am == 0)
                    {
                        n = d;
                        buf[ix++] = p;
                    }
                }
                if (n > 1)
                    buf[ix++] = n;
                return buf[..ix].ToArray();
            }
            static IEnumerable<long> PollardRho(long n)
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
                }
                else
                {
                    foreach (var v in Inner(p))
                        yield return v;
                    foreach (var v in Inner(n / p))
                        yield return v;
                }
            }
        }

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
    }
}
