using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __ModIntExtension
    {
#if NET7_0_OR_GREATER
        [凾(256)]
        public static T Sum<T>(this IEnumerable<T> source) where T : IModInt<T>
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return T.CreateTruncating(sum);
        }
        [凾(256)]
        public static T Sum<T>(this ReadOnlySpan<T> source) where T : IModInt<T>
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return T.CreateTruncating(sum);
        }
        [凾(256)]
        public static T Sum<T>(this Span<T> source) where T : IModInt<T>
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return T.CreateTruncating(sum);
        }
#else
        [凾(256)]
        public static StaticModInt<T> Sum<T>(this IEnumerable<StaticModInt<T>> source) where T : struct, IStaticMod
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return new StaticModInt<T>(sum);
        }
        [凾(256)]
        public static StaticModInt<T> Sum<T>(this ReadOnlySpan<StaticModInt<T>> source) where T : struct, IStaticMod
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return new StaticModInt<T>(sum);
        }
        [凾(256)]
        public static StaticModInt<T> Sum<T>(this Span<StaticModInt<T>> source) where T : struct, IStaticMod
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return new StaticModInt<T>(sum);
        }
#endif
    }
}
