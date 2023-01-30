using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
#if !NET7_0_OR_GREATER
using AtCoder.Operators;
#endif

namespace Kzrnm.Competitive
{
    public static class MathLibEx
    {
        // 高速な Gcd の実装
        // https://nyaannyaan.github.io/library/trial/fast-gcd.hpp

#if NET7_0_OR_GREATER
        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static T Gcd<T>(T a, T b)
            where T : IBinaryInteger<T>
        {
            if (T.IsZero(a)) return b;
            if (T.IsZero(b)) return a;
            a = T.Abs(a);
            b = T.Abs(b);
            var n = int.CreateChecked(T.TrailingZeroCount(a));
            var m = int.CreateChecked(T.TrailingZeroCount(b));

            a >>= n;
            b >>= m;
            while (a != b)
            {
                int m2 = int.CreateChecked(T.TrailingZeroCount(a - b));
                if (a < b) (a, b) = (b, a);
                a = (a - b) >> m2;
            }
            return a << Math.Min(n, m);
        }
        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static T Gcd<T>(params T[] nums)
            where T : IBinaryInteger<T>
        {
            var gcd = nums[0];
            for (var i = 1; i < nums.Length; i++)
                gcd = Gcd(nums[i], gcd);
            return gcd;
        }
#else
        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static int Gcd(int a, int b) => (int)Gcd((long)a, b);
        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static long Gcd(long a, long b)
        {
            if (a == 0) return b;
            if (b == 0) return a;
            a = Math.Abs(a);
            b = Math.Abs(b);
            int n = BitOperations.TrailingZeroCount(a);
            int m = BitOperations.TrailingZeroCount(b);
            a >>= n;
            b >>= m;
            while (a != b)
            {
                int m2 = BitOperations.TrailingZeroCount(a - b);
                if (a < b) (a, b) = (b, a);
                a = (a - b) >> m2;
            }
            return a << Math.Min(n, m);
        }
        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static int Gcd(params int[] nums)
        {
            var gcd = nums[0];
            for (var i = 1; i < nums.Length; i++)
                gcd = Gcd(nums[i], gcd);
            return gcd;
        }
        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static long Gcd(params long[] nums)
        {
            var gcd = nums[0];
            for (var i = 1; i < nums.Length; i++)
                gcd = Gcd(nums[i], gcd);
            return gcd;
        }
#endif
        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static BigInteger Gcd(BigInteger a, BigInteger b) => BigInteger.GreatestCommonDivisor(a, b);

        /// <summary>
        /// 最大公約数
        /// </summary>
        [凾(256)]
        public static BigInteger Gcd(params BigInteger[] nums)
        {
            var gcd = nums[0];
            for (var i = 1; i < nums.Length; i++)
                gcd = Gcd(nums[i], gcd);
            return gcd;
        }

#if NET7_0_OR_GREATER
        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static T Lcm<T>(T a, T b)
            where T : IBinaryInteger<T>
            => checked(a / Gcd(a, b) * b);

        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static T Lcm<T>(params T[] nums)
            where T : IBinaryInteger<T>
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }
#else
        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static int Lcm(int a, int b) => a / Gcd(a, b) * b;
        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static long Lcm(long a, long b) => checked(a / Gcd(a, b) * b);
        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static int Lcm(params int[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }
        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static long Lcm(params long[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }
#endif

        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static BigInteger Lcm(BigInteger a, BigInteger b) => a / Gcd(a, b) * b;

        /// <summary>
        /// 最小公倍数
        /// </summary>
        [凾(256)]
        public static BigInteger Lcm(params BigInteger[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }

#if NET7_0_OR_GREATER
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

#if NET7_0_OR_GREATER
        /// <summary>
        /// <paramref name="n"/> を素因数分解します。
        /// </summary>
        public static Dictionary<T, int> PrimeFactoring<T>(T n)
            where T : IBinaryInteger<T>
        {
            var res = new Dictionary<T, int>();
            if (n <= T.One)
                return res;

            var two = T.One + T.One;
            var t = int.CreateChecked(T.TrailingZeroCount(n));
            if (t > 0)
            {
                res[two] = t;
                n >>= t;
            }
            {
                int c;
                var three = two + T.One;
                (n, c) = Divide(n, three);
                if (c > 0)
                    res[three] = c;

                var five = three + two;
                (n, c) = Divide(n, five);
                if (c > 0)
                    res[five] = c;
            }
            for (long i = 7; i * i <= long.CreateChecked(n); i += 2)
            {
                for (int p = 0; p < 4; p++, i += 2)
                {
                    int c;
                    // 10x + (7,9,1,3)
                    var ii = T.CreateSaturating(i);
                    (n, c) = Divide(n, ii);
                    if (c > 0)
                        res[ii] = c;
                }
            }
            if (n > T.One)
                res[n] = 1;
            return res;
        }

        [凾(256)]
        static (T Divided, int Count) Divide<T>(T n, T i)
            where T : IBinaryInteger<T>
        {
            int c = 0;
            while (DivRem(n, i) is var (d, am) && T.IsZero(am))
            {
                n = d;
                ++c;
            }
            return (n, c);
        }

        [凾(256)]
        static (T Div, T Remainder) DivRem<T>(T a, T b)
            where T : IBinaryInteger<T>
        {
            var div = a / b;
            return (div, a - div * b);
        }
#else
        /// <summary>
        /// <paramref name="n"/> を素因数分解します。
        /// </summary>
        public static Dictionary<int, int> PrimeFactoring(int n)
        {
            var res = new Dictionary<int, int>();
            if (n <= 1)
                return res;

            int t = BitOperations.TrailingZeroCount(n);
            if (t > 0)
            {
                res[2] = t;
                n >>= t;
            }
            {
                int c;
                (n, c) = Divide(n, 3);
                if (c > 0)
                    res[3] = c;

                (n, c) = Divide(n, 5);
                if (c > 0)
                    res[5] = c;
            }
            for (long i = 7; i * i <= n; i += 2)
            {
                for (int p = 0; p < 4; p++, i += 2)
                {
                    int c;
                    var ii = (int)i;
                    // 10x + (7,9,1,3)
                    (n, c) = Divide(n, ii);
                    if (c > 0)
                        res[ii] = c;
                }
            }
            if (n > 1)
                res[n] = 1;
            return res;
        }

        /// <summary>
        /// <paramref name="n"/> を素因数分解します。
        /// </summary>
        public static Dictionary<long, int> PrimeFactoring(long n)
        {
            var res = new Dictionary<long, int>();
            if (n <= 1)
                return res;

            int t = BitOperations.TrailingZeroCount(n);
            if (t > 0)
            {
                res[2] = t;
                n >>= t;
            }

            {
                int c;
                (n, c) = Divide(n, 3);
                if (c > 0)
                    res[3] = c;

                (n, c) = Divide(n, 5);
                if (c > 0)
                    res[5] = c;
            }
            for (long i = 7; i * i <= n; i += 2)
            {
                for (int p = 0; p < 4; p++, i += 2)
                {
                    int c;
                    // 10x + (7,9,1,3)
                    (n, c) = Divide(n, i);
                    if (c > 0)
                        res[i] = c;
                }
            }
            if (n > 1)
                res[n] = 1;
            return res;
        }

        [凾(256)]
        static (int Divided, int Count) Divide(int n, int i)
        {
            int c = 0;
            while (Math.DivRem(n, i, out var am) is var d && am == 0)
            {
                n = d;
                ++c;
            }
            return (n, c);
        }
        [凾(256)]
        static (long Divided, int Count) Divide(long n, long i)
        {
            int c = 0;
            while (Math.DivRem(n, i, out var am) is var d && am == 0)
            {
                n = d;
                ++c;
            }
            return (n, c);
        }
#endif

        /// <summary>
        /// 二項係数 <paramref name="n"/>_C_<paramref name="k"/> を返す。
        /// </summary>
        public static long Combination(long n, long k)
        {
            long res = 1;
            for (long i = 1; i <= k; i++)
            {
                res = res * (n - i + 1) / i;
            }
            return res;
        }

#if NET7_0_OR_GREATER
        /// <summary>
        /// C[n][k] = 二項係数 n_C_k となる配列 C を返す。
        /// </summary>
        /// <param name="maxSize">n の最大値</param>
        public static T[][] CombinationTable<T>(int maxSize)
            where T : IAdditionOperators<T, T, T>, IMultiplicativeIdentity<T, T>
        {
            var c = new T[++maxSize][];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = new T[i + 1];
                c[i][0] = c[i][^1] = T.MultiplicativeIdentity;
                for (int j = 1; j + 1 < c[i].Length; ++j)
                {
                    c[i][j] = c[i - 1][j - 1] + c[i - 1][j];
                }
            }
            return c;
        }
#else
        /// <summary>
        /// C[n][k] = 二項係数 n_C_k となる配列 C を返す。
        /// </summary>
        /// <param name="maxSize">n の最大値</param>
        public static T[][] CombinationTable<T, TOp>(int maxSize)
            where TOp : struct, IAdditionOperator<T>, IMultiplicationOperator<T>
        {
            var op = new TOp();
            var c = new T[++maxSize][];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = new T[i + 1];
                c[i][0] = c[i][^1] = op.MultiplyIdentity;
                for (int j = 1; j + 1 < c[i].Length; ++j)
                {
                    c[i][j] = op.Add(c[i - 1][j - 1], c[i - 1][j]);
                }
            }
            return c;
        }
#endif

        /// <summary>
        /// 等差数列の和
        /// </summary>
        /// <param name="from">初項</param>
        /// <param name="to">末項</param>
        /// <param name="count">項数</param>
        public static long ArithmeticSeries(long from, long to, long count) => (count * (from + to)) / 2;
    }
}
