using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder
{
    public static class MyLinqExtension
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
        public static (int index, T min) MinBy<T>(this T[] arr) where T : IComparable<T>
             => MinBy((ReadOnlySpan<T>)arr);
        public static (int index, T min) MinBy<T>(this Span<T> arr) where T : IComparable<T>
             => MinBy((ReadOnlySpan<T>)arr);
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
        public static (int index, T min) MinBy<T, TMin>(this T[] arr, Func<T, TMin> minBySelector) where TMin : IComparable<TMin>
            => MinBy((ReadOnlySpan<T>)arr, minBySelector);
        public static (int index, T min) MinBy<T, TMin>(this Span<T> arr, Func<T, TMin> minBySelector) where TMin : IComparable<TMin>
            => MinBy((ReadOnlySpan<T>)arr, minBySelector);
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
        public static (TSource item, TMin min) MinBy<TSource, TMin>
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

        /// <summary>
        /// インデックスをつける
        /// </summary>
        public static IEnumerable<(int index, TSource val)> Indexed<TSource>(this IEnumerable<TSource> source)
            => source.Select((v, i) => (i, v));

        /// <summary>
        /// 条件に合致するインデックスを返す
        /// </summary>
        public static IEnumerable<int> WhereBy<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => source.Select((v, i) => (i, v)).Where(t => predicate(t.v)).Select(t => t.i);
        public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
        public static Dictionary<TKey, int> GroupCount<TKey>(this IEnumerable<TKey> source) => source.GroupCount(i => i);

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> source) => source.SelectMany(a => a);
    }
}
