using AtCoder;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 累積和のStaticModIntラッパークラス
    /// <summary>
    /// <see cref="StaticModInt{T}"/> の累積和を求めます。
    /// </summary>
    public class StaticModIntSums<T> : Sums<StaticModInt<T>, StaticModIntOperator<T>> where T : struct, IStaticMod
    {
        public StaticModIntSums(StaticModInt<T>[] arr) : base(arr) { }
        public StaticModIntSums(IList<StaticModInt<T>> col) : base(col) { }
    }
    public static class StaticModIntSumsExt
    {
        /// <summary>累積和</summary>
        public static StaticModIntSums<T> CumulativeSum<T>(this StaticModInt<T>[] a) where T : struct, IStaticMod => new StaticModIntSums<T>(a);
        /// <summary>累積和</summary>
        public static StaticModIntSums<T> CumulativeSum<T>(this IList<StaticModInt<T>> a) where T : struct, IStaticMod => new StaticModIntSums<T>(a);
    }
}
