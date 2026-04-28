namespace Kzrnm.Competitive.Testing.DataStructure;

public class Sums2DTests
{
    [Test, MultipleAssertions]
    public async Task Normal()
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
        await sums[0..10][0..20].Should().BeEqualTo(8550);
        await sums[1..10][1..20].Should().BeEqualTo(8550);
        await sums[3..10][1..20].Should().BeEqualTo(7980);
        await sums[4..8][7..9].Should().BeEqualTo(330);
    }

    [Test, MultipleAssertions]
    public async Task Random()
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
                        await sums[l..r][u..d].Should().BeEqualTo(SumDirect(arr, l, r, u, d));
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