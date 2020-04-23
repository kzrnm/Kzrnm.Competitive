using System;
using System.Collections.Generic;
using System.Linq;

static class 順列を求める
{
    static T[][] Permutation<T>(IList<T> items) { if (items.Count == 0) throw new IndexOutOfRangeException(); if (items.Count == 1) return new T[][] { items.ToArray() }; var arr = items.ToArray(); var size = 1; for (int i = 2; i <= items.Count; i++) size *= i; var ret = new T[size][]; for (int i = 0; i < items.Count; i++) { var tmp = arr[i]; arr[i] = arr[0]; arr[0] = tmp; foreach (var item in Permutation(new ArraySegment<T>(arr, 1, arr.Length - 1))) { ret[--size] = new T[items.Count]; ret[size][0] = arr[0]; item.CopyTo(ret[size], 1); }; } return ret; }
}