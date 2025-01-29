using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 累積和を求めます。
    /// </summary>
    public static class Sums
    {
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        [凾(256)]
        public static T[] Accumulate<T>(ReadOnlySpan<T> orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>
        {
            var impl = new T[orig.Length + 1];
            impl[0] = defaultValue;
            for (var i = 0; i < orig.Length; i++)
                impl[i + 1] = impl[i] + orig[i];
            return impl;
        }
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        [凾(256)]
        public static T[] Accumulate<T>(Span<T> orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>
            => Accumulate((ReadOnlySpan<T>)orig, defaultValue);
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        [凾(256)]
        public static T[] Accumulate<T>(T[] orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>
            => Accumulate((ReadOnlySpan<T>)orig, defaultValue);
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        [凾(256)]
        public static T[] Accumulate<T>(IList<T> orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>
        {
            var impl = new T[orig.Count + 1];
            impl[0] = defaultValue;
            for (var i = 0; i < orig.Count; i++)
                impl[i + 1] = impl[i] + orig[i];
            return impl;
        }

        /// <summary>
        /// <paramref name="orig"/> の累積和を範囲演算で取得できるデータ構造を返します。
        /// </summary>
        [凾(256)]
        public static Sums<T> Create<T>(ReadOnlySpan<T> orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
            => new(Accumulate(orig, defaultValue));
        /// <summary>
        /// <paramref name="orig"/> の累積和を範囲演算で取得できるデータ構造を返します。
        /// </summary>
        [凾(256)]
        public static Sums<T> Create<T>(Span<T> orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
            => new(Accumulate(orig, defaultValue));
        /// <summary>
        /// <paramref name="orig"/> の累積和を範囲演算で取得できるデータ構造を返します。
        /// </summary>
        [凾(256)]
        public static Sums<T> Create<T>(T[] orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
            => new(Accumulate(orig, defaultValue));
        /// <summary>
        /// <paramref name="orig"/> の累積和を範囲演算で取得できるデータ構造を返します。
        /// </summary>
        [凾(256)]
        public static Sums<T> Create<T>(IList<T> orig, T defaultValue = default) where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
            => new(Accumulate(orig, defaultValue));
    }
    /// <summary>
    /// 範囲演算で累積和を取得します。
    /// </summary>
    public class Sums<T>
        where T : ISubtractionOperators<T, T, T>
    {
        readonly T[] impl;
        public int Length => impl.Length - 1;
        internal Sums(T[] accu)
        {
            impl = accu;
        }
        [凾(256)] public T Slice(int from, int length) => impl[from + length] - impl[from];
        public T this[int toExclusive] { [凾(256)] get => impl[toExclusive]; }
        public T this[int from, int toExclusive] { [凾(256)] get => impl[toExclusive] - impl[from]; }
    }
}
