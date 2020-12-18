using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
    public static class CollectionExtension
    {
        [MethodImpl(AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0) => Unsafe.As<ArrayVal<T>>(list).arr.AsSpan(start, list.Count);
        [MethodImpl(AggressiveInlining)]
        public static ref T Get<T>(this T[] arr, int index)
        {
            if (index < 0)
                return ref arr[arr.Length + index];
            return ref arr[index];
        }
        [MethodImpl(AggressiveInlining)]
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var v);
            return v;
        }
    }
}
