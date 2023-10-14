using AtCoder.Operators;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 累積和
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
            impl = Accumulate(arr.AsSpan(), defaultValue);
        }
        public Sums(IList<T> collection, T defaultValue = default)
        {
            impl = Accumulate(collection, defaultValue);
        }
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        public static T[] Accumulate(ReadOnlySpan<T> orig, T defaultValue = default)
        {
            var impl = new T[orig.Length + 1];
            impl[0] = defaultValue;
            for (var i = 0; i < orig.Length; i++)
                impl[i + 1] = op.Add(impl[i], orig[i]);
            return impl;
        }
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        public static T[] Accumulate(IList<T> orig, T defaultValue = default)
        {
            var impl = new T[orig.Count + 1];
            impl[0] = defaultValue;
            for (var i = 0; i < orig.Count; i++)
                impl[i + 1] = op.Add(impl[i], orig[i]);
            return impl;
        }
        [凾(256)] public T Slice(int from, int length) => op.Subtract(impl[from + length], impl[from]);
        public T this[int toExclusive] { [凾(256)] get => impl[toExclusive]; }
        public T this[int from, int toExclusive] { [凾(256)] get => op.Subtract(impl[toExclusive], impl[from]); }
    }
}
