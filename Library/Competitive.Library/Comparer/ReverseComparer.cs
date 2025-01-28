using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct ReverseComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public static ReverseComparer<T> Default => default;
        [凾(256)]
        public int Compare(T x, T y) => y.CompareTo(x);
    }
}
