using AtCoder;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq
{
    public static class StaticModIntExtension
    {
        public static StaticModInt<T> Sum<T>(this IEnumerable<StaticModInt<T>> source) where T : struct, IStaticMod
        {
            ulong sum = 0;
            foreach (var v in source) sum += (ulong)v.Value;
            return new StaticModInt<T>(sum);
        }
    }
}
