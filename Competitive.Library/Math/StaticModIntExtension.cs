using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class StaticModIntExtension
    {
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
    }
}
