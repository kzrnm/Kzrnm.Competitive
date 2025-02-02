using System;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class Sums2DTests
{
    [Fact]
    public void Normal()
    {
        var arr = new int[10][];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new int[20];
            for (int j = 0; j < arr[i].Length; j++)
            {
                arr[i][j] = i * j;
            }
        }
        var sums = Sums2D.Create(arr);
        sums[0..10][0..20].ShouldBe(8550);
        sums[1..10][1..20].ShouldBe(8550);
        sums[3..10][1..20].ShouldBe(7980);
        sums[4..8][7..9].ShouldBe(330);
    }

    [Fact]
    public void Random()
    {
        var rnd = new Random();
        var arr = new long[10][];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new long[20];
            for (int j = 0; j < arr[i].Length; j++)
            {
                arr[i][j] = rnd.Next(-100000, 100000);
            }
        }
        var sums = Sums2D.Create(arr);
        for (int l = 0; l <= 10; l++)
            for (int r = l; r <= 10; r++)
                for (int u = 0; u <= 20; u++)
                    for (int d = u; d <= 20; d++)
                        sums[l..r][u..d].ShouldBe(SumDirect(arr, l, r, u, d));
    }

    static long SumDirect(long[][] arr, int left, int rightEx, int upper, int bottomEx)
    {
        long sum = 0;
        for (int i = left; i < rightEx; i++)
            for (int j = upper; j < bottomEx; j++)
                sum += arr[i][j];
        return sum;
    }
}
