namespace AtCoder
{

    /// <summary>
    /// <see cref="long"/> の累積和を求めます。
    /// </summary>
    public class LongSums : Sums<long, LongOperator> { public LongSums(long[] arr) : base(arr) { } }

    /// <summary>
    /// <see cref="int"/> の累積和を求めます。
    /// </summary>
    public class IntSums : Sums<int, IntOperator> { public IntSums(int[] arr) : base(arr) { } }

    /// <summary>
    /// <see cref="StaticModInt{T}"/> の累積和を求めます。
    /// </summary>
    public class StaticModIntSums<T> : Sums<StaticModInt<T>, StaticModIntOperator<T>> where T : struct, IStaticMod { public StaticModIntSums(StaticModInt<T>[] arr) : base(arr) { } }

    /// <summary>
    /// 累積和を求めます。
    /// </summary>
    public class Sums<TValue, TOp>
        where TValue : struct
        where TOp : struct, IArithmeticOperator<TValue>
    {
        static readonly TOp op;
        private readonly TValue[] impl;
        public int Length => impl.Length - 1;
        public Sums(TValue[] arr)
        {
            impl = new TValue[arr.Length + 1];
            for (var i = 0; i < arr.Length; i++)
                impl[i + 1] = op.Add(impl[i], arr[i]);
        }
        public TValue Slice(int from, int length) => op.Subtract(impl[from + length], impl[from]);
        public TValue this[int toExclusive] => impl[toExclusive];
        public TValue this[int from, int toExclusive] => op.Subtract(impl[toExclusive], impl[from]);
    }
}
