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
            var list = new List<int>();

            for (int i = 1, d = Math.DivRem(n, i, out int amari);
                i <= d;
                i++, d = Math.DivRem(n, i, out amari))
            {
                if (amari == 0)
                {
                    yield return i;
                    if (i != d)
                        list.Add(d);
                }
            }

            for (var i = list.Count - 1; i >= 0; i--)
                yield return list[i];
        }

        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static IEnumerable<long> Divisor(long n)
        {
            var list = new List<long>();

            for (long i = 1, d = Math.DivRem(n, i, out long amari);
                i <= d;
                i++, d = Math.DivRem(n, i, out amari))
            {
                if (amari == 0)
                {
                    yield return i;
                    if (i != d)
                        list.Add(d);
                }
            }

            for (var i = list.Count - 1; i >= 0; i--)
                yield return list[i];
        }
    }
}
