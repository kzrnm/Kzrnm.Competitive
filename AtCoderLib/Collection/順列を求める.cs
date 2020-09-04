using System;
using System.Collections.Generic;
using System.Linq;

static class 順列を求める
{
    public static IEnumerable<T[]> Permutation<T>(IEnumerable<T> orig) where T : IComparable<T>
    {
        var arr = orig.ToArray();
        while (true)
        {
            yield return arr;
            int i;
            for (i = arr.Length - 2; i >= 0; i--)
                if (arr[i].CompareTo(arr[i + 1]) < 0)
                    break;
            if (i < 0) break;
            int j;
            for (j = arr.Length - 1; j >= 0; j--)
                if (arr[i].CompareTo(arr[j]) < 0)
                    break;
            (arr[i], arr[j]) = (arr[j], arr[i]);
            arr.AsSpan(i + 1).Reverse();
        }
    }

}