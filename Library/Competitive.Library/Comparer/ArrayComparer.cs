using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class ArrayComparer<T> : IComparer<T[]>, IComparer<ReadOnlySpan<T>> where T : IComparable<T>
    {
        readonly bool IsReverse;
        public ArrayComparer(bool isReverse = false)
        {
            IsReverse = isReverse;
        }
        public static ArrayComparer<T> Default => new(false);
        public static ArrayComparer<T> Reverse => new(true);
        [凾(256)]
        public int Compare(T[] x, T[] y) => Compare(x.AsSpan(), y);
        [凾(256)]
        public int Compare(ReadOnlySpan<T> x, ReadOnlySpan<T> y) => IsReverse ? y.SequenceCompareTo(x) : x.SequenceCompareTo(y);
    }
}
