using System;
using System.Collections.Generic;

namespace AtCoder
{
    public static class MathLibEx
    {

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
