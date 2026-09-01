using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    /// <summary>
    /// 階乗とその逆数を保持する。O(N) で初期構築したあとは二項係数を O(1) で求められる。
    /// </summary>
    /// <remarks>
    /// <para>すぐオーバーフローするのでデバッグ用を想定</para>
    /// </remarks>
    public class FractionFactor
    {
        readonly Fraction[] fac, finv;

        public FractionFactor()
        {
            const int max = 20 + 1;
            fac = new Fraction[max];
            finv = new Fraction[max];
            fac[0] = fac[1] = Fraction.One;
            finv[0] = finv[1] = Fraction.One;
            for (var i = 2; i < max; i++)
            {
                fac[i] = fac[i - 1] * i;
                finv[i] = fac[i].Inverse();
            }
        }

        ///<summary>組み合わせ関数(二項係数)</summary>
        [凾(256)]
        public Fraction Combination(int n, int k)
        {
            if (n < k) return default;
            if (n < 0 || k < 0) return default;
            return fac[n] * finv[k] * finv[n - k];
        }

        ///<summary>重複組み合わせ関数</summary>
        [凾(256)]
        public Fraction Homogeneous(int n, int k) => Combination(n + k - 1, k);

        ///<summary>順列関数</summary>
        [凾(256)]
        public Fraction Permutation(int n, int k)
        {
            if (n < k) return default;
            if (n < 0 || k < 0) return default;
            return fac[n] * finv[n - k];
        }

        /// <summary>
        /// <paramref name="n"/> の逆数
        /// </summary>
        [凾(256)]
        public Fraction Inverse(int n) => n switch
        {
            > 0 => finv[n] * fac[n - 1],
            < 0 => -Inverse(-n),
            0 => default, // ゼロ除算だが気にしないでおく
        };

        /// <summary>
        /// <paramref name="n"/> の階乗
        /// </summary>
        [凾(256)]
        public Fraction Factorial(int n) => fac[n];

        /// <summary>
        /// <paramref name="n"/> の階乗の逆数
        /// </summary>
        [凾(256)]
        public Fraction FactorialInverse(int n) => finv[n];
    }
}
