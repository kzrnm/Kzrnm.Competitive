using AtCoder;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 形式的冪級数の対数
    /// </summary>
    public static class __FormalPowerSeries__Pow
    {
        /// <summary>
        /// (f(x))^<paramref name="k"/> となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <param name="f">形式的冪級数</param>
        /// <param name="k"><paramref name="k"/> 乗します。</param>
        /// <param name="deg">先頭の <paramref name="deg"/> 項を取得します。負数のときは元の FPS と同じ長さで取得します。</param>
        /// <example>https://judge.yosupo.jp/problem/pow_of_formal_power_series</example>
        [凾(256)]
        public static FormalPowerSeries<T> Pow<T>(this FormalPowerSeries<T> f, long k, int deg = -1) where T : struct, IStaticMod
            => f.ToImpl().Pow(k, deg).ToFps();



        [凾(256)]
        internal static FpsImpl<T> Pow<T>(this FpsImpl<T> t, long k, int deg = -1) where T : struct, IStaticMod
        {
            if (deg < 0) deg = t.Length;
            if (k == 0)
                return new FpsImpl<T>(new[] { MontgomeryModInt<T>.One });

            var span = t.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i].Value != 0)
                {
                    var rev = span[i].Inv();
                    var right = span[i].Pow(k);

                    return t.Multiply(rev).RightShift(i).Log(deg).Multiply(k).Exp(deg).Multiply(right)
                        .LeftShift((int)(i * k)).Pre(deg);
                }
                if (Math.Max((i + 1) * k, k) > deg)
                    return t.Set(new MontgomeryModInt<T>[deg]);
            }
            return t.Set(new MontgomeryModInt<T>[deg]);
        }
    }
}