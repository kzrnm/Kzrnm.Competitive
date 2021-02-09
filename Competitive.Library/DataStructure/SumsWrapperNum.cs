using System.Collections.Generic;

namespace AtCoder
{

    /// <summary>
    /// <see cref="long"/> の累積和を求めます。
    /// </summary>
    public class LongSums2D : Sums2D<long, LongOperator>
    {
        public LongSums2D(long[][] arr) : base(arr) { }
    }

    /// <summary>
    /// <see cref="int"/> の累積和を求めます。
    /// </summary>
    public class IntSums2D : Sums2D<int, IntOperator>
    {
        public IntSums2D(int[][] arr) : base(arr) { }
    }
}
