﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __MemoryMarshalExtension
    {
#pragma warning disable CS0649
        private class ArrayVal<T> { public T[] arr; }
#pragma warning restore CS0649
        [凾(256)]
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0) => Unsafe.As<ArrayVal<T>>(list).arr.AsSpan(start, list.Count - start);
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
