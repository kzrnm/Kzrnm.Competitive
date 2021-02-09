using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
#pragma warning disable CA2231 // Overload operator equals on overriding value type Equals
    public struct DefaultComparerStruct<T> : IComparer<T> where T : IComparable<T>
#pragma warning restore CA2231 // Overload operator equals on overriding value type Equals
    {
        public static DefaultComparerStruct<T> Default { get; } = default;
        public int Compare(T x, T y) => x.CompareTo(y);
        public override bool Equals(object obj) => obj is ReverseComparerStruct<T>;
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
