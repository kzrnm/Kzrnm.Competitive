using System;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
    public static class UpdateExtension
    {
        [MethodImpl(AggressiveInlining)]

        public static bool UpdateMax<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) < 0) { r = val; return true; }
            return false;
        }
        [MethodImpl(AggressiveInlining)]
        public static bool UpdateMin<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) > 0) { r = val; return true; }
            return false;
        }
    }
}
