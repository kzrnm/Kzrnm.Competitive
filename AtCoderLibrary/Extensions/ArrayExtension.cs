using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
    public static class ArrayExtension
    {
        [MethodImpl(AggressiveInlining)]
        public static T[] Fill<T>(this T[] arr, T value)
        {
            arr.AsSpan().Fill(value);
            return arr;
        }
        [MethodImpl(AggressiveInlining)]
        public static T[] Sort<T>(this T[] arr) { Array.Sort(arr); return arr; }
        [MethodImpl(AggressiveInlining)]
        public static string[] Sort(this string[] arr) => Sort(arr, StringComparer.Ordinal);
        [MethodImpl(AggressiveInlining)]
        public static T[] Sort<T, U>(this T[] arr, Expression<Func<T, U>> selector) where U : IComparable<U> => Sort(arr, ExComparer<T>.CreateExp(selector));
        [MethodImpl(AggressiveInlining)]
        public static T[] Sort<T>(this T[] arr, Comparison<T> comparison) { Array.Sort(arr, comparison); return arr; }
        [MethodImpl(AggressiveInlining)]
        public static T[] Sort<T>(this T[] arr, IComparer<T> comparer) { Array.Sort(arr, comparer); return arr; }
        [MethodImpl(AggressiveInlining)]
        public static T[] Reverse<T>(this T[] arr) { Array.Reverse(arr); return arr; }
    }
}
