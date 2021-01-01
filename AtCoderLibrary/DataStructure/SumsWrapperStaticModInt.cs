using System.Collections.Generic;

namespace AtCoder
{
    /// <summary>
    /// <see cref="StaticModInt{T}"/> の累積和を求めます。
    /// </summary>
    public class StaticModIntSums<T> : Sums<StaticModInt<T>, StaticModIntOperator<T>> where T : struct, IStaticMod
    {
        public StaticModIntSums(StaticModInt<T>[] arr) : base(arr) { }
        public StaticModIntSums(IList<StaticModInt<T>> col) : base(col) { }
    }
}
