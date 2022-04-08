using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public struct DefaultComparerStruct<T> : IComparer<T> where T : IComparable<T>
    {
        public static DefaultComparerStruct<T> Default { get; } = default;
        [凾(256)]
        public int Compare(T x, T y) => x.CompareTo(y);
        public override bool Equals(object obj) => obj is ReverseComparerStruct<T>;
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
