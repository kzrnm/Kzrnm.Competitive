using AtCoder.Operators;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 累積和を求めます。
    /// </summary>
    public class Sums<TValue, TOp>
        where TOp : struct, IAdditionOperator<TValue>, ISubtractOperator<TValue>
    {
        private static readonly TOp op = default;
        private readonly TValue[] impl;
        public int Length => impl.Length - 1;
        public Sums(TValue[] arr, TValue defaultValue = default)
        {
            impl = new TValue[arr.Length + 1];
            impl[0] = defaultValue;
            for (var i = 0; i < arr.Length; i++)
                impl[i + 1] = op.Add(impl[i], arr[i]);
        }
        public Sums(IList<TValue> collection)
        {
            impl = new TValue[collection.Count + 1];
            for (var i = 0; i < collection.Count; i++)
                impl[i + 1] = op.Add(impl[i], collection[i]);
        }
        [凾(256)] public TValue Slice(int from, int length) => op.Subtract(impl[from + length], impl[from]);
        public TValue this[int toExclusive] { [凾(256)] get => impl[toExclusive]; }
        public TValue this[int from, int toExclusive] { [凾(256)] get => op.Subtract(impl[toExclusive], impl[from]); }
    }
}
