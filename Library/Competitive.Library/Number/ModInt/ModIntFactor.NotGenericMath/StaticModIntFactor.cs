using AtCoder;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 階乗とその逆数を保持する。O(N) で初期構築したあとは二項係数を O(1) で求められる。
    /// </summary>
    public class StaticModIntFactor<T> where T : struct, IStaticMod
    {
        private readonly StaticModInt<T>[] fac, finv;
        private StaticModInt<T>[] pow2, invPow2;
        [凾(256)]
        private void BuildPow2(int size)
        {
            // 2の冪乗とその逆数を計算する
            invPow2 = new StaticModInt<T>[size];
            pow2 = new StaticModInt<T>[size];
            pow2[0] = invPow2[0] = StaticModInt<T>.One;
            var two = StaticModInt<T>.Raw(2);
            var invTwo = two.Inv();
            for (int i = 1; i < invPow2.Length; i++)
            {
                pow2[i] = pow2[i - 1] * two;
                invPow2[i] = invPow2[i - 1] * invTwo;
            }
        }

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
        [凾(256)]
        public StaticModInt<T> Combination(int n, int k)
        {
            if (n < k) return 0;
            if (n < 0 || k < 0) return 0;
            return fac[n] * finv[k] * finv[n - k];
        }

        ///<summary>重複組み合わせ関数</summary>
        [凾(256)]
        public StaticModInt<T> Homogeneous(int n, int k) => Combination(n + k - 1, k);

        ///<summary>順列関数</summary>
        [凾(256)]
        public StaticModInt<T> Permutation(int n, int k)
        {
            if (n < k) return 0;
            if (n < 0 || k < 0) return 0;
            return fac[n] * finv[n - k];
        }

        /// <summary>
        /// <paramref name="n"/> の階乗
        /// </summary>
        [凾(256)]
        public StaticModInt<T> Factorial(int n) => fac[n];

        /// <summary>
        /// <paramref name="n"/> の階乗の逆数
        /// </summary>
        [凾(256)]
        public StaticModInt<T> FactorialInvers(int n) => finv[n];


        /// <summary>
        /// <paramref name="n"/> の二重階乗
        /// </summary>
        [凾(256)]
        public StaticModInt<T> DoubleFactorial(int n)
        {
            if (pow2 == null) BuildPow2(fac.Length);
            var h = (n + 1) / 2;
            return (n & 1) != 0 ? fac[n + 1] * finv[h] * invPow2[h] : fac[h] * pow2[h];
        }
    }
}
