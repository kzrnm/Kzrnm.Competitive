using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_MinBy
    {
        [凾(256)]
        public static (int index, T min) MinBy<T>(this T[] arr) where T : IComparable<T>
             => MinBy((ReadOnlySpan<T>)arr);
        [凾(256)]
        public static (int index, T min) MinBy<T>(this Span<T> arr) where T : IComparable<T>
             => MinBy((ReadOnlySpan<T>)arr);
        [凾(256)]
        public static (int index, T min) MinBy<T>(this ReadOnlySpan<T> arr) where T : IComparable<T>
        {
            T min = arr[0];
            int minIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (min.CompareTo(arr[i]) > 0)
                {
                    min = arr[i];
                    minIndex = i;
                }
            }
            return (minIndex, min);
        }
        [凾(256)]
        public static (int index, T min) MinBy<T, TMin>(this T[] arr, Func<T, TMin> minBySelector) where TMin : IComparable<TMin>
            => MinBy((ReadOnlySpan<T>)arr, minBySelector);
        [凾(256)]
        public static (int index, T min) MinBy<T, TMin>(this Span<T> arr, Func<T, TMin> minBySelector) where TMin : IComparable<TMin>
            => MinBy((ReadOnlySpan<T>)arr, minBySelector);
        [凾(256)]
        public static (int index, T min) MinBy<T, TMin>(this ReadOnlySpan<T> arr, Func<T, TMin> minBySelector) where TMin : IComparable<TMin>

        {
            var minItem = minBySelector(arr[0]);
            var min = arr[0];
            int minIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                var nx = minBySelector(arr[i]);
                if (minItem.CompareTo(nx) > 0)
                {
                    minItem = nx;
                    min = arr[i];
                    minIndex = i;
                }
            }
            return (minIndex, min);
        }
        [凾(256)]
        public static (TSource item, TMin min) MinBy2<TSource, TMin>
            (this IEnumerable<TSource> source, Func<TSource, TMin> minBySelector)
            where TMin : IComparable<TMin>
        {
            TMin min;
            TSource minByItem;

            var e = source.GetEnumerator();
            e.MoveNext();
            minByItem = e.Current;
            min = minBySelector(minByItem);
            while (e.MoveNext())
            {
                var item = e.Current;
                var next = minBySelector(item);
                if (min.CompareTo(next) > 0)
                {
                    min = next;
                    minByItem = item;
                }
            }
            return (minByItem, min);
        }
    }
}
