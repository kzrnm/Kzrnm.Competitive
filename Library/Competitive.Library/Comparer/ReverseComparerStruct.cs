using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly record struct ReverseComparerStruct<T> : IComparer<T> where T : IComparable<T>
    {
        public static ReverseComparerStruct<T> Default => default;
        [凾(256)]
        public int Compare(T x, T y) => y.CompareTo(x);
    }
}
