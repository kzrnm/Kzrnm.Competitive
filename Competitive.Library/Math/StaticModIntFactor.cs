using AtCoder;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    public class StaticModIntFactor<T> where T : struct, IStaticMod
    {
        private readonly StaticModInt<T>[] fac, finv;
        public StaticModIntFactor(int max)
        {
            fac = new StaticModInt<T>[++max];
            finv = new StaticModInt<T>[max];
            fac[0] = fac[1] = 1;
            finv[0] = finv[1] = 1;
            for (var i = 2; i < max; i++)
                fac[i] = fac[i - 1] * i;
            finv[^1] = 1 / fac[^1];
            for (int i = finv.Length - 2; i >= 2; i--)
                finv[i] = finv[i + 1] * (i + 1);
        }

        ///<summary>組み合わせ関数(二項係数)</summary>
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Combination(int n, int k)
        {
            if (n < k) return 0;
            if (n < 0 || k < 0) return 0;
            return fac[n] * finv[k] * finv[n - k];
        }

        ///<summary>重複組み合わせ関数</summary>
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Homogeneous(int n, int k) => Combination(n + k - 1, k);

        ///<summary>順列関数</summary>
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Permutation(int n, int k)
        {
            if (n < k) return 0;
            if (n < 0 || k < 0) return 0;
            return fac[n] * finv[n - k];
        }

        /// <summary>
        /// <paramref name="n"/> の階乗
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Factorial(int n) => fac[n];

        /// <summary>
        /// <paramref name="n"/> の階乗の逆数
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> FactorialInvers(int n) => finv[n];
    }
}
