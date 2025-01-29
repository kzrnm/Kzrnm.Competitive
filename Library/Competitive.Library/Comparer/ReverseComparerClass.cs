using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class ReverseComparerClass<T> : IComparer<T>
    {
        public ReverseComparerClass(IComparer<T> orig)
        {
            this.orig = orig;
        }
        public static ReverseComparerClass<T> Default { get; } = new ReverseComparerClass<T>(Comparer<T>.Default);
        readonly IComparer<T> orig;
        [凾(256)]
        public int Compare(T x, T y) => orig.Compare(y, x);
        public override bool Equals(object obj) => obj is ReverseComparerClass<T>;
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
