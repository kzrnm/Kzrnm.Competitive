using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System
{
#pragma warning disable IDE1006
    using static MethodImplOptions;
    public static class __CollectionExtension
    {
#pragma warning disable CS0649
        private class ArrayVal<T> { public T[] arr; }
#pragma warning restore CS0649
        [MethodImpl(AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0) => Unsafe.As<ArrayVal<T>>(list).arr.AsSpan(start, list.Count);
        [MethodImpl(AggressiveInlining)]
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var v);
            return v;
        }


        public static T[] Flatten<T>(this T[][] array) => Flatten((ReadOnlySpan<T[]>)array);
        public static T[] Flatten<T>(this Span<T[]> span) => Flatten((ReadOnlySpan<T[]>)span);
        public static T[] Flatten<T>(this ReadOnlySpan<T[]> span)
        {
            var res = new T[span.Length * span[0].Length];
            for (int i = 0; i < span.Length; i++)
                for (int j = 0; j < span[i].Length; j++)
                    res[i * span[i].Length + j] = span[i][j];
            return res;
        }
        public static T[] Flatten<T>(this IList<IList<T>> collection)
        {
            var res = new T[collection.Count * collection[0].Count];
            for (int i = 0; i < collection.Count; i++)
                for (int j = 0; j < collection[i].Count; j++)
                    res[i * collection[i].Count + j] = collection[i][j];
            return res;
        }
        public static char[] Flatten(this string[] strs)
        {
            var res = new char[strs.Length * strs[0].Length];
            for (int i = 0; i < strs.Length; i++)
                for (int j = 0; j < strs[i].Length; j++)
                    res[i * strs[i].Length + j] = strs[i][j];
            return res;
        }
    }
}
