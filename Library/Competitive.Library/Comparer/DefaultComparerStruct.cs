using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly record struct DefaultComparerStruct<T> : IComparer<T> where T : IComparable<T>
    {
        public static DefaultComparerStruct<T> Default => default;
        [凾(256)]
        public int Compare(T x, T y) => x.CompareTo(y);
    }
}
