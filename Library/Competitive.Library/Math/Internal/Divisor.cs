using System.Collections.Generic;
using System.Numerics;

namespace Kzrnm.Competitive.Internal
{
    internal static class Divisors
    {
        /// <summary>
        /// <paramref name="n"/> の約数を返します。
        /// </summary>
        public static T[] Divisor<T>(KeyValuePair<T, int>[] pairs)
            where T : IBinaryInteger<T>
        {
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
