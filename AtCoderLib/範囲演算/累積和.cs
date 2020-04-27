using System;
using static AtCoderProject.Global;


class Sums
{
    private long[] impl;
    public int Length { get; }
    public Sums(int[] arr)
    {
        this.Length = arr.Length;
        impl = new long[arr.Length + 1];
        for (var i = 0; i < arr.Length; i++)
            impl[i + 1] = impl[i] + arr[i];
    }
    public long this[int toExclusive] => impl[toExclusive];
    public long this[Range range] => impl[range.End.GetOffset(Length)] - impl[range.Start.GetOffset(Length)];
}