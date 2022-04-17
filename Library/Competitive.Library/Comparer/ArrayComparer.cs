using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public class ArrayComparer<T> : IComparer<T[]> where T : IComparable<T>
    {
        private readonly bool IsReverse;
        public ArrayComparer(bool isReverse = false)
        {
            IsReverse = isReverse;
        }
        public static readonly ArrayComparer<T> Default = new ArrayComparer<T>(false);
        public static readonly ArrayComparer<T> Reverse = new ArrayComparer<T>(true);
        public int Compare(T[] x, T[] y)
        {
            if (IsReverse)
                (x, y) = (y, x);
            for (int i = 0; i < x.Length && i < y.Length; i++)
            {
                var cmp = x[i].CompareTo(y[i]);
                if (cmp != 0)
                    return cmp;
            }
            return x.Length.CompareTo(y.Length);
        }
    }
}
