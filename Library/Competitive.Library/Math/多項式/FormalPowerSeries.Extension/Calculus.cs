using AtCoder;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 形式的冪級数の微積分
    /// </summary>
    public static class __FormalPowerSeries__Calculus
    {
        /// <summary>
        /// 微分した多項式を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> Derivative<T>(this FormalPowerSeries<T> f) where T : struct, IStaticMod
            => f.ToImpl().Derivative().ToFps();

        /// <summary>
        /// 微分した多項式を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        internal static FormalPowerSeries<T>.Impl Derivative<T>(this FormalPowerSeries<T>.Impl t) where T : struct, IStaticMod
        {
            var a = t.a;
            if (t.Length > 0)
            {
                for (int i = 1; i < a.Length; i++)
                    a[i - 1] = a[i] * i;
                --t.Length;
            }
            return t;
        }

        /// <summary>
        /// 積分した多項式を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> Integrate<T>(this FormalPowerSeries<T> f) where T : struct, IStaticMod
            => f.ToImpl().Integrate().ToFps();

        /// <summary>
        /// 積分した多項式を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        internal static FormalPowerSeries<T>.Impl Integrate<T>(this FormalPowerSeries<T>.Impl t) where T : struct, IStaticMod
        {
            var a = t.a;
            var r = new MontgomeryModInt<T>[t.Length + 1];
            for (int i = 1; i < r.Length; i++)
                r[i] = a[i - 1] / i;
            return t.Set(r);
        }
    }
}
