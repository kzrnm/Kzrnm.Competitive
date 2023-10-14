using AtCoder;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 累積和のラッパークラス

    /// <summary>
    /// <see cref="long"/> の累積和を求めます。
    /// </summary>
    public class LongSums : Sums<long, LongOperator>
    {
        public LongSums(long[] arr) : base(arr) { }
        public LongSums(IList<long> col) : base(col) { }
    }

    /// <summary>
    /// <see cref="int"/> の累積和を求めます。
    /// </summary>
    public class IntSums : Sums<int, IntOperator>
    {
        public IntSums(int[] arr) : base(arr) { }
        public IntSums(IList<int> col) : base(col) { }
    }
    public static class SumsWrapperExt
    {
        /// <summary>累積和</summary>
        public static IntSums CumulativeSum(this int[] a) => new IntSums(a);
        /// <summary>累積和</summary>
        public static IntSums CumulativeSum(this IList<int> a) => new IntSums(a);
        /// <summary>累積和</summary>
        public static LongSums CumulativeSum(this long[] a) => new LongSums(a);
        /// <summary>累積和</summary>
        public static LongSums CumulativeSum(this IList<long> a) => new LongSums(a);
    }
}
