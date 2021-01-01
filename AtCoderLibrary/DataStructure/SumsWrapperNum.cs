using System.Collections.Generic;

namespace AtCoder
{

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
}
