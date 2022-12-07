using AtCoder.Operators;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 累積和を求めます。
    /// </summary>
    public class Sums<T, TOp>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
    {
        private static readonly TOp op = default;
        private readonly T[] impl;
        public int Length => impl.Length - 1;
        public Sums(T[] arr, T defaultValue = default)
        {
            impl = new T[arr.Length + 1];
            impl[0] = defaultValue;
            for (var i = 0; i < arr.Length; i++)
                impl[i + 1] = op.Add(impl[i], arr[i]);
        }
        public Sums(IList<T> collection)
        {
            impl = new T[collection.Count + 1];
            for (var i = 0; i < collection.Count; i++)
                impl[i + 1] = op.Add(impl[i], collection[i]);
        }
        [凾(256)] public T Slice(int from, int length) => op.Subtract(impl[from + length], impl[from]);
        public T this[int toExclusive] { [凾(256)] get => impl[toExclusive]; }
        public T this[int from, int toExclusive] { [凾(256)] get => op.Subtract(impl[toExclusive], impl[from]); }
    }
}
