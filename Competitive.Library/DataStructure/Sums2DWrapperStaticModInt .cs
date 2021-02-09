using System.Collections.Generic;

namespace AtCoder
{
    /// <summary>
    /// <see cref="StaticModInt{T}"/> の累積和を求めます。
    /// </summary>
    public class StaticModIntSums2D<T> : Sums2D<StaticModInt<T>, StaticModIntOperator<T>> where T : struct, IStaticMod
    {
        public StaticModIntSums2D(StaticModInt<T>[][] arr) : base(arr) { }
    }
}
