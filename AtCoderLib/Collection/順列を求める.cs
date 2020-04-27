using System;

static class 順列を求める
{
    static T[][] Permutation<T>(T[] items) { static T[][] PermutationImpl(ReadOnlySpan<T> items) { if (items.Length == 0) throw new IndexOutOfRangeException(); if (items.Length == 1) return new T[][] { items.ToArray() }; var arr = items.ToArray(); var size = 1; for (int i = 2; i <= items.Length; i++) size *= i; var ret = new T[size][]; for (int i = 0; i < items.Length; i++) { (arr[0], arr[i]) = (arr[i], arr[0]); foreach (var item in PermutationImpl(new ReadOnlySpan<T>(arr).Slice(1))) { ret[--size] = new T[items.Length]; ret[size][0] = arr[0]; item.CopyTo(ret[size], 1); }; } return ret; } return PermutationImpl(items); }
}