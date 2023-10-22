using System;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __UpdateExtension
    {
        [凾(256)]
        public static bool UpdateMax<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) < 0) { r = val; return true; }
            return false;
        }
        [凾(256)]
        public static bool UpdateMax<T>(this ref T r, params T[] vals) where T : struct, IComparable<T> => r.UpdateMax(vals.Max());
        [凾(256)]
        public static bool UpdateMin<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) > 0) { r = val; return true; }
            return false;
        }
        [凾(256)]
        public static bool UpdateMin<T>(this ref T r, params T[] vals) where T : struct, IComparable<T> => r.UpdateMin(vals.Min());
    }
}
