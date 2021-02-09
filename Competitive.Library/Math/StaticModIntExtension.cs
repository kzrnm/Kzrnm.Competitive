using AtCoder;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq
{
    public static class StaticModIntExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StaticModInt<T> Sum<T>(this IEnumerable<StaticModInt<T>> source) where T : struct, IStaticMod
        {
            StaticModInt<T> sum = 0;
            foreach (var v in source) sum += v;
            return sum;
        }
    }
}
