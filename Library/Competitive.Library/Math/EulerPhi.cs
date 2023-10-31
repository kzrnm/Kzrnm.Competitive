using System;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
using System.Numerics;

namespace Kzrnm.Competitive
{
    // https://ei1333.github.io/library/math/number-theory/euler-phi.hpp
    // https://ei1333.github.io/library/math/number-theory/euler-phi-table.hpp

    /// <summary>
    /// <para>オイラーの φ 関数</para>
    /// <para>φ(n): n 以下で n と互いに素な数の数</para>
    /// </summary>
    public static class EulerPhi
    {
        /// <summary>
        /// <para><paramref name="n"/> 以下の整数の φ(<paramref name="n"/>) の値</para>
        /// <para>φ(<paramref name="n"/>): <paramref name="n"/> 以下で <paramref name="n"/> と互いに素な数の数</para>
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
        /// <para>φ(<paramref name="n"/>): <paramref name="n"/> 以下で <paramref name="n"/> と互いに素な数の数</para>
        /// <para>計算量: O(√n)</para>
        /// </summary>
        [凾(256)]
        public static T Solve<T>(T n) where T : IBinaryNumber<T>
        {
            T r = n;
            for (T i = T.One + T.One; i * i <= n; i++)
                if (T.IsZero(n % i))
                {
                    r -= r / i;
                    while (DivRem(n, i) is (var dd, var rm) && T.IsZero(rm))
                        n = dd;
                }
            if (n > T.One)
                r -= r / n;
            return r;
        }
        [凾(256)]
        private static (T Quotient, T Remainder) DivRem<T>(T left, T right) where T : IBinaryNumber<T>
        {
            T quotient = left / right;
            return (quotient, left - (quotient * right));
        }
    }
}