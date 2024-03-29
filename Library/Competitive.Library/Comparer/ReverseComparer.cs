using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class ReverseComparer<T> : IComparer<T>
    {
        public static ReverseComparer<T> Default { get; } = new ReverseComparer<T>(Comparer<T>.Default);
        private readonly IComparer<T> orig;
        public ReverseComparer(IComparer<T> orig)
        {
            this.orig = orig;
        }
        [凾(256)]
        public int Compare(T x, T y) => orig.Compare(y, x);
        public override bool Equals(object obj) => obj is ReverseComparer<T>;
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
