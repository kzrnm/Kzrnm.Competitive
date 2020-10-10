using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    public static class Extensions
    {
        public static bool UpdateMax<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) < 0) { r = val; return true; }
            return false;
        }
        public static bool UpdateMin<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) > 0) { r = val; return true; }
            return false;
        }
        public static T[] Fill<T>(this T[] arr, T value)
        {
            arr.AsSpan().Fill(value);
            return arr;
        }
        public static T[] Sort<T>(this T[] arr) { Array.Sort(arr); return arr; }
        public static string[] Sort(this string[] arr) => Sort(arr, StringComparer.Ordinal);
        public static T[] Sort<T, U>(this T[] arr, Expression<Func<T, U>> selector) where U : IComparable<U> => Sort(arr, ExComparer<T>.CreateExp(selector));
        public static T[] Sort<T>(this T[] arr, Comparison<T> comparison) { Array.Sort(arr, comparison); return arr; }
        public static T[] Sort<T>(this T[] arr, IComparer<T> comparer) { Array.Sort(arr, comparer); return arr; }
        public static T[] Reverse<T>(this T[] arr) { Array.Reverse(arr); return arr; }
        public static (int index, T max) MaxBy<T>(this T[] arr) where T : IComparable<T>
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
        public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
        public static Dictionary<TKey, int> GroupCount<TKey>(this IEnumerable<TKey> source) => source.GroupCount(i => i);
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0) => Unsafe.As<Tuple<T[]>>(list).Item1.AsSpan(start, list.Count);
        public static ref T Get<T>(this T[] arr, int index)
        {
            if (index < 0)
                return ref arr[arr.Length + index];
            return ref arr[index];
        }
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var v);
            return v;
        }
    }
}
