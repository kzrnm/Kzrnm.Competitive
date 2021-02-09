using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class MathLibEx
    {
        /// <summary>
        /// 最大公約数
        /// </summary>
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
        public static int Gcd(int a, int b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
        /// <summary>
        /// 最小公倍数
        /// </summary>
        public static int Lcm(int a, int b) => a / Gcd(a, b) * b;
        /// <summary>
        /// 最小公倍数
        /// </summary>
        public static int Lcm(params int[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }
        /// <summary>
        /// 最大公約数
        /// </summary>
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
        public static long Gcd(long a, long b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
        /// <summary>
        /// 最小公倍数
        /// </summary>
        public static long Lcm(long a, long b) => checked(a / Gcd(a, b) * b);
        /// <summary>
        /// 最小公倍数
        /// </summary>
        public static long Lcm(params long[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }

        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        public static long Pow(long x, int y)
        {
            long res = 1;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1) res *= x;
                x *= x;
            }
            return res;
        }

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static IEnumerable<int> Divisor(int n)
        {
            var list = new LinkedList<int>();
            var left = list.AddFirst(1);
            if (n == 1)
                return list;
            var right = list.AddLast(n);

            for (int i = 2, d = Math.DivRem(n, i, out int amari);
                i <= d;
                i++, d = Math.DivRem(n, i, out amari))
            {
                if (amari == 0)
                {
                    left = list.AddAfter(left, i);
                    if (i != d)
                        right = list.AddBefore(right, d);
                }
            }

            return list;
        }

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static IEnumerable<long> Divisor(long n)
        {
            var list = new LinkedList<long>();
            var left = list.AddFirst(1);
            if (n == 1)
                return list;
            var right = list.AddLast(n);

            for (long i = 2, d = Math.DivRem(n, i, out long amari);
                i <= d;
                i++, d = Math.DivRem(n, i, out amari))
            {
                if (amari == 0)
                {
                    left = list.AddAfter(left, i);
                    if (i != d)
                        right = list.AddBefore(right, d);
                }
            }

            return list;
        }

        /// <summary>
        /// 組み合わせ <paramref name="n"/>_C_<paramref name="k"/> を返す。
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
    }
}
