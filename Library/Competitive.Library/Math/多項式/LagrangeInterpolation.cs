using AtCoder;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// ラグランジュ補間
    /// </summary>
    public static class LagrangeInterpolation
    {
        // https://ei1333.github.io/library/math/combinatorics/lagrange-polynomial-2.cpp
        /// <summary>
        /// <para>ラグランジュ補間</para>
        /// <para>(x0, y0), ..., (xN, yN)を満たす N 次多項式を返します。</para>
        /// <para>制約: xは全て異なる</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N^2)</para>
        /// </remarks>
        [凾(512)]
        public static FormalPowerSeries<T> Coefficient<T>((MontgomeryModInt<T> x, MontgomeryModInt<T> y)[] plots) where T : struct, IStaticMod
        {
            int k = plots.Length - 1;

            var f = new MontgomeryModInt<T>[k + 1];
            var dp = new MontgomeryModInt<T>[k + 2];
            dp[0] = MontgomeryModInt<T>.One;
            for (int j = 0; j <= k; j++)
            {
                var x = plots[j].x;
                for (int l = k + 1; l > 0; l--)
                    dp[l] = dp[l] * -x + dp[l - 1];
                dp[0] *= -x;
            }

            for (int i = 0; i <= k; i++)
            {
                var (xi, yi) = plots[i];
                var d = MontgomeryModInt<T>.One;
                for (int j = 0; j <= k; j++)
                    if (i != j)
                        d *= xi - plots[j].x;

                var mul = yi / d;
                if (xi == 0)
                {
                    for (int j = 0; j <= k; j++)
                        f[j] += dp[j + 1] * mul;
                }
                else
                {
                    var inv = MontgomeryModInt<T>.One / -xi;
                    MontgomeryModInt<T> pre = default;
                    for (int j = 0; j <= k; j++)
                    {
                        var cur = (dp[j] - pre) * inv;
                        f[j] += cur * mul;
                        pre = cur;
                    }
                }
            }
            return new FormalPowerSeries<T>(f);
        }

        // https://ei1333.github.io/library/math/combinatorics/lagrange-polynomial.cpp
        /// <summary>
        /// <para>ラグランジュ補間</para>
        /// <para>(0, y0), ..., (N, yN)を満たす N 次多項式に <paramref name="x"/> を代入した値を返します。</para>
        /// <para>制約: <paramref name="combination"/> が初期化済みならば、<paramref name="y"/>.Length までは使えること</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(512)]
        public static MontgomeryModInt<T> Eval<T>(MontgomeryModInt<T>[] y, long x, ModIntFactor<MontgomeryModInt<T>> combination = null) where T : struct, IStaticMod
        {
            combination ??= new ModIntFactor<MontgomeryModInt<T>>(y.Length);
            if (x < y.Length) return y[(int)x];
            var ret = default(MontgomeryModInt<T>);
            var dp = new MontgomeryModInt<T>[y.Length];
            var pd = new MontgomeryModInt<T>[y.Length];
            pd[^1] = dp[0] = MontgomeryModInt<T>.One;

            for (int i = 0; i + 1 < dp.Length; i++) dp[i + 1] = dp[i] * (x - i);
            for (int i = pd.Length - 1; i > 0; i--) pd[i - 1] = pd[i] * (x - i);
            for (int i = 0; i < dp.Length; i++)
            {
                var t = y[i] * dp[i] * pd[i] * combination.FactorialInvers(i) * combination.FactorialInvers(y.Length - i - 1);
                if (((y.Length ^ i) & 1) == 0) ret -= t;
                else ret += t;
            }
            return ret;
        }
    }
}
