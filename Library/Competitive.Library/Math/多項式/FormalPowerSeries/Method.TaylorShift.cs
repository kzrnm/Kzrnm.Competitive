using AtCoder;
using System;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 形式的冪級数の平行移動
    /// </summary>
    public static class __FormalPowerSeries__TayloShift
    {

        /// <summary>
        /// <para>元の多項式をf(x) とすると f(x+<paramref name="a"/>) に平行移動した多項式を返します。</para>
        /// <para>制約: <paramref name="combination"/> が初期化済みならば、<paramref name="f"/> の次数までは使えること</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(512)]
        public static FormalPowerSeries<T> TaylorShift<T>(this FormalPowerSeries<T> f, MontgomeryModInt<T> a, ModIntFactor<MontgomeryModInt<T>> combination = null)
            where T : struct, IStaticMod
            => f.ToImpl().TaylorShift(a, combination ?? new(f.Count)).ToFps();


        [凾(256)]
        internal static FpsImpl<T> TaylorShift<T>(this FpsImpl<T> t, MontgomeryModInt<T> a, ModIntFactor<MontgomeryModInt<T>> combination) where T : struct, IStaticMod
        {
            var b = t.AsSpan();
            if (a.Value == 0 || b.Length == 0) return t;
            for (int i = 0; i < b.Length; i++)
                b[i] *= combination.Factorial(i);
            b.Reverse();
            var g = new MontgomeryModInt<T>[b.Length];
            var prev = g[0] = MontgomeryModInt<T>.One;
            for (int i = 1; i < g.Length; i++)
                prev = g[i] = prev * a * combination.Inverse(i);

            int n = b.Length;
            t = t.Multiply(g).Pre(n);
            b = t.AsSpan();
            b.Reverse();
            for (int i = 0; i < b.Length; i++)
                b[i] *= combination.FactorialInverse(i);
            return t;
        }
    }
}