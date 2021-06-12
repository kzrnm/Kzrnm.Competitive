using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AtCoder.Internal;
using System.Runtime.InteropServices;

namespace System
{
    using static MethodImplOptions;
    public static class __MemoryMarshalExtension
    {
#pragma warning disable CS0649
        private class ArrayVal<T> { public T[] arr; }
#pragma warning restore CS0649
        [MethodImpl(AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0) => Unsafe.As<ArrayVal<T>>(list).arr.AsSpan(start, list.Count - start);
        [MethodImpl(AggressiveInlining)]
        public static Span<TTo> Cast<TFrom, TTo>(this Span<TFrom> span)
            where TFrom : struct
            where TTo : struct
            => MemoryMarshal.Cast<TFrom, TTo>(span);
        [MethodImpl(AggressiveInlining)]
        public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(this ReadOnlySpan<TFrom> span)
            where TFrom : struct
            where TTo : struct
            => MemoryMarshal.Cast<TFrom, TTo>(span);
        [MethodImpl(AggressiveInlining)]
        public static ref T GetReference<T>(this Span<T> span) => ref MemoryMarshal.GetReference(span);
        [MethodImpl(AggressiveInlining)]
        public static ref T GetReference<T>(this ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference(span);
    }
}
