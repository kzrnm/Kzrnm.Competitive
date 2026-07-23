using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __ModIntExtension
    {
        [凾(256)]
        public static T Sum<T>(this IEnumerable<T> source) where T : IModInt<T>
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return T.Raw((int)(sum % (uint)T.Mod));
        }
        [凾(256)]
        public static T Sum<T>(this ReadOnlySpan<T> source) where T : IModInt<T>
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return T.Raw((int)(sum % (uint)T.Mod));
        }
        [凾(256)]
        public static T Sum<T>(this Span<T> source) where T : IModInt<T>
            => Sum((ReadOnlySpan<T>)source);
    }
}
