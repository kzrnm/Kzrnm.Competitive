using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public class ReverseComparerClass<T> : IComparer<T> where T : IComparable<T>
    {
        public static ReverseComparerClass<T> Default { get; } = new ReverseComparerClass<T>();
        public int Compare(T x, T y) => y.CompareTo(x);
        public override bool Equals(object obj) => obj is ReverseComparerClass<T>;
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
