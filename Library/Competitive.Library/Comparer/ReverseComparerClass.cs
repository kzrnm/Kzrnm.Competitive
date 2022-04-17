using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class ReverseComparerClass<T> : IComparer<T> where T : IComparable<T>
    {
        public static ReverseComparerClass<T> Default { get; } = new ReverseComparerClass<T>();
        [凾(256)]
        public int Compare(T x, T y) => y.CompareTo(x);
        public override bool Equals(object obj) => obj is ReverseComparerClass<T>;
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
