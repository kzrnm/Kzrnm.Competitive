using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoderLib.範囲演算
{
    public class 累積和2DTests
    {
        readonly Sums2D sums;
        public 累積和2DTests()
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
            sums = new Sums2D(arr);
        }

        public static TheoryData Sums_Data = new TheoryData<int, int, int, int, long>
        {
            { 0, 10, 0, 20, 8550 },
            { 1, 10, 1, 20, 8550 },
            { 3, 10, 1, 20, 7980 },
            { 4, 8, 7, 9, 330 },
        };

        [Theory]
        [MemberData(nameof(Sums_Data))]
        public void Sums(int left, int rightEx, int top, int BottomEx, long sum)
        {
            sums[left..rightEx][top..BottomEx].Should().Be(sum);
        }
    }
}
