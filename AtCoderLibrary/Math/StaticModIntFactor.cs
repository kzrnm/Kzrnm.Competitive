using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder
{
    public class StaticModIntFactor<T> where T : struct, IStaticMod
    {
        private readonly StaticModInt<T>[] fac, finv;
        public StaticModIntFactor(int max)
        {
            var mod = default(T).Mod;
            ++max;
            var inv = new StaticModInt<T>[max];
            fac = new StaticModInt<T>[max]; finv = new StaticModInt<T>[max];
            fac[0] = fac[1] = 1;
            finv[0] = finv[1] = 1;
            inv[1] = 1;
            for (var i = 2; i < max; i++)
            {
                fac[i] = fac[i - 1] * i;
                inv[i] = -(inv[mod % i] * (mod / i));
                finv[i] = finv[i - 1] * inv[i];
            }
        }

        ///<summary>組み合わせ関数(二項係数)</summary>
        public StaticModInt<T> Combination(int n, int k)
        {
            if (n < k) return 0;
            if (n < 0 || k < 0) return 0;
            return fac[n] * finv[k] * finv[n - k];
        }

        public StaticModInt<T> Factorial(int n) => fac[n];
        public StaticModInt<T> FactorialInvers(int n) => finv[n];
    }
}
