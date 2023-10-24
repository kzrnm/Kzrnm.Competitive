using AtCoder;
using AtCoder.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 形式的冪級数の対数
    /// </summary>
    public static class __FormalPowerSeries__Log
    {
        /// <summary>
        /// log(f(x)) となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <param name="f">形式的冪級数</param>
        /// <param name="deg">先頭の <paramref name="deg"/> 項を取得します。負数のときは元の FPS と同じ長さで取得します。</param>
        /// <example>https://judge.yosupo.jp/problem/log_of_formal_power_series</example>
        [凾(256)]
        public static FormalPowerSeries<T> Log<T>(this FormalPowerSeries<T> f, int deg = -1) where T : struct, IStaticMod
            => f.ToImpl().Log(deg).ToFps();

        /// <summary>
        /// log(f(x)) となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <param name="t">形式的冪級数</param>
        /// <param name="deg">先頭の <paramref name="deg"/> 項を取得します。負数のときは元の FPS と同じ長さで取得します。</param>
        /// <example>https://judge.yosupo.jp/problem/log_of_formal_power_series</example>
        [凾(256)]
        internal static FormalPowerSeries<T>.Impl Log<T>(this FormalPowerSeries<T>.Impl t, int deg = -1) where T : struct, IStaticMod
        {
            var a = t.a;
            Contract.Assert(a[0].Value == 1);
            if (deg < 0) deg = t.Length;
            var inv = new FormalPowerSeries<T>.Impl(t.AsSpan().ToArray()).Inv(deg);
            return t.Derivative().Multiply(inv).Pre(deg - 1).Integrate();
        }
    }
}