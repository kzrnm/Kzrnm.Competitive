using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __MemoryMarshalExtension
    {
        /// <summary>
        /// <see cref="List{T}"/> の内部配列を <see cref="Span{T}"/> として取り出します。null でも空 Span を返します。
        /// </summary>
        [凾(256)]
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0)
            => CollectionsMarshal.AsSpan(list)[start..];

        [凾(256)]
        public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
            => ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out _);
        [凾(256)]
        public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, out bool exists)
            => ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out exists);

        [凾(256)]
        public static Span<TTo> Cast<TFrom, TTo>(this Span<TFrom> span)
            where TFrom : struct
            where TTo : struct
            => MemoryMarshal.Cast<TFrom, TTo>(span);
        [凾(256)]
        public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(this ReadOnlySpan<TFrom> span)
            where TFrom : struct
            where TTo : struct
            => MemoryMarshal.Cast<TFrom, TTo>(span);
        [凾(256)]
        public static ref T GetReference<T>(this Span<T> span) => ref MemoryMarshal.GetReference(span);
        [凾(256)]
        public static ref T GetReference<T>(this ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference(span);
    }
}
