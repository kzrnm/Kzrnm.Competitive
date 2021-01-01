using System.Collections.Generic;

namespace AtCoder
{
    /// <summary>
    /// 累積和を求めます。
    /// </summary>
    public class Sums<TValue, TOp>
        where TValue : struct
        where TOp : struct, IArithmeticOperator<TValue>
    {
        private static readonly TOp op = default;
        private readonly TValue[] impl;
        public int Length => impl.Length - 1;
        public Sums(TValue[] arr)
        {
            impl = new TValue[arr.Length + 1];
            for (var i = 0; i < arr.Length; i++)
                impl[i + 1] = op.Add(impl[i], arr[i]);
        }
        public Sums(IList<TValue> collection)
        {
            impl = new TValue[collection.Count + 1];
            for (var i = 0; i < collection.Count; i++)
                impl[i + 1] = op.Add(impl[i], collection[i]);
        }
        public TValue Slice(int from, int length) => op.Subtract(impl[from + length], impl[from]);
        public TValue this[int toExclusive] => impl[toExclusive];
        public TValue this[int from, int toExclusive] => op.Subtract(impl[toExclusive], impl[from]);
    }
}
