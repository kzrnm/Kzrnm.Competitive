using AtCoder.Operators;
using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class MathLibEx
    {
        // 高速な Gcd の実装
        // https://nyaannyaan.github.io/library/trial/fast-gcd.hpp
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
            if (a == 0 || b == 0) return a | b;
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
        public static BigInteger Gcd(BigInteger a, BigInteger b) => BigInteger.GreatestCommonDivisor(a, b);

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
        public static BigInteger Lcm(BigInteger a, BigInteger b) => a / Gcd(a, b) * b;

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

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static int[] Divisor(int n)
        {
            if (n == 1)
                return new int[] { 1 };

            var left = new List<int>();
            var right = new List<int>();
            left.Add(1);
            right.Add(n);

            for (int i = 2, d = Math.DivRem(n, i, out int amari);
                i <= d;
                i++, d = Math.DivRem(n, i, out amari))
            {
                if (amari == 0)
                {
                    left.Add(i);
                    if (i != d)
                        right.Add(d);
                }
            }
            right.Reverse();
            var res = new int[left.Count + right.Count];
            left.CopyTo(res, 0);
            right.CopyTo(res, left.Count);
            return res;
        }

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static long[] Divisor(long n)
        {
            if (n == 1)
                return new long[] { 1 };

            var left = new List<long>();
            var right = new List<long>();
            left.Add(1);
            right.Add(n);

            for (long i = 2, d = Math.DivRem(n, i, out long amari);
                i <= d;
                i++, d = Math.DivRem(n, i, out amari))
            {
                if (amari == 0)
                {
                    left.Add(i);
                    if (i != d)
                        right.Add(d);
                }
            }
            right.Reverse();
            var res = new long[left.Count + right.Count];
            left.CopyTo(res, 0);
            right.CopyTo(res, left.Count);
            return res;
        }

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

        /// <summary>
        /// 等差数列の和
        /// </summary>
        /// <param name="from">初項</param>
        /// <param name="to">末項</param>
        /// <param name="count">項数</param>
        public static long ArithmeticSeries(long from, long to, long count) => (count * (from + to)) / 2;
    }
}
