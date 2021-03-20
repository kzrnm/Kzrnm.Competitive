using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
#pragma warning disable IDE1006
    public static class __CollectionExtension_MaxBy
    {
        public static (int index, T max) MaxBy<T>(this T[] arr) where T : IComparable<T>
             => MaxBy((ReadOnlySpan<T>)arr);
        public static (int index, T max) MaxBy<T>(this Span<T> arr) where T : IComparable<T>
             => MaxBy((ReadOnlySpan<T>)arr);
        public static (int index, T max) MaxBy<T>(this ReadOnlySpan<T> arr) where T : IComparable<T>
        {
            T max = arr[0];
            int maxIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (max.CompareTo(arr[i]) < 0)
                {
                    max = arr[i];
                    maxIndex = i;
                }
            }
            return (maxIndex, max);
        }
        public static (int index, T max) MaxBy<T, TMax>(this T[] arr, Func<T, TMax> maxBySelector) where TMax : IComparable<TMax>
            => MaxBy((ReadOnlySpan<T>)arr, maxBySelector);
        public static (int index, T max) MaxBy<T, TMax>(this Span<T> arr, Func<T, TMax> maxBySelector) where TMax : IComparable<TMax>
            => MaxBy((ReadOnlySpan<T>)arr, maxBySelector);
        public static (int index, T max) MaxBy<T, TMax>(this ReadOnlySpan<T> arr, Func<T, TMax> maxBySelector) where TMax : IComparable<TMax>
        {
            var maxItem = maxBySelector(arr[0]);
            var max = arr[0];
            int maxIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                var nx = maxBySelector(arr[i]);
                if (maxItem.CompareTo(nx) < 0)
                {
                    maxItem = nx;
                    max = arr[i];
                    maxIndex = i;
                }
            }
            return (maxIndex, max);
        }
        public static (TSource item, TMax max) MaxBy<TSource, TMax>
            (this IEnumerable<TSource> source, Func<TSource, TMax> maxBySelector)
            where TMax : IComparable<TMax>
        {
            TMax max;
            TSource maxByItem;
            var e = source.GetEnumerator();
            e.MoveNext();
            maxByItem = e.Current;
            max = maxBySelector(maxByItem);
            while (e.MoveNext())
            {
                var item = e.Current;
                var next = maxBySelector(item);
                if (max.CompareTo(next) < 0)
                {
                    max = next;
                    maxByItem = item;
                }
            }
            return (maxByItem, max);
        }
    }
}
