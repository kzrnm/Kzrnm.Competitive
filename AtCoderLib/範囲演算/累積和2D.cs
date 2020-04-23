using System;
using static AtCoderProject.Global;

class Sums2D
{
    private long[][] impl;
    public int Length1 { get; }
    public int Length2 { get; }
    public Sums2D(int[][] arr)
    {
        this.Length1 = arr.Length;
        this.Length2 = arr[0].Length;
        impl = NewArray(Length1 + 1, Length2 + 1, 0L);
        for (var i = 0; i < Length1; i++)
            for (var j = 0; j < Length2; j++)
                impl[i + 1][j + 1] = impl[i + 1][j] + impl[i][j + 1] - impl[i][j] + arr[i][j];
    }
    public long this[int left, int rightExclusive, int top, int bottomExclusive]
    {
        get
        {
            return impl[rightExclusive][bottomExclusive]
              - impl[left][bottomExclusive]
              - impl[rightExclusive][top]
              + impl[left][top];
        }
    }
}
