using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
#pragma warning disable CA2231 // Overload operator equals on overriding value type Equals
    public struct ReverseComparerStruct<T> : IComparer<T> where T : IComparable<T>
#pragma warning restore CA2231 // Overload operator equals on overriding value type Equals
    {
        public static ReverseComparerStruct<T> Default { get; } = default;
        [凾(256)]
        public int Compare(T x, T y) => y.CompareTo(x);
        public override bool Equals(object obj) => obj is ReverseComparerStruct<T>;
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
