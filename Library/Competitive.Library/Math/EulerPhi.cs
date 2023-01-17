using System;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/math/number-theory/euler-phi.hpp
    // https://ei1333.github.io/library/math/number-theory/euler-phi-table.hpp

    /// <summary>
    /// オイラーの φ 関数
    /// </summary>
    public static class EulerPhi
    {
        /// <summary>
        /// <para><paramref name="n"/> 以下の整数の φ(<paramref name="n"/>) の値</para>
        /// <para>計算量: O(n log log n)</para>
        /// </summary>
        [凾(256)]
        public static int[] Table(int n)
        {
            var e = Enumerable.Range(0, n + 1).ToArray();
            for (int i = 2; i < e.Length; i++)
                if (e[i] == i)
                {
                    for (int j = i; j < e.Length; j += i)
                        e[j] = e[j] / i * (i - 1);
                }
            return e;
        }
        /// <summary>
        /// <para>φ(<paramref name="n"/>) の値</para>
        /// <para>計算量: O(√n)</para>
        /// </summary>
        [凾(256)]
        public static long F(long n)
        {
            long r = n;
            for (long i = 2; i * i <= n; i++)
                if (n % i == 0)
                {
                    r -= r / i;
                    while (Math.DivRem(n, i, out var rm) is var dd && rm == 0) n = dd;
                }
            if (n > 1) r -= r / n;
            return r;
        }
    }
}