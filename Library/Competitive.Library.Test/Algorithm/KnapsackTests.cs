using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Algorithm;

public class KnapsackTests
{
    public static IEnumerable<TheoryDataRow<int>> SmallWeight_Data()
        => Enumerable.Range(0, 14).Select(i => new TheoryDataRow<int>(i)).ToArray();

    [Theory]
    [MemberData(nameof(SmallWeight_Data))]
    public void SmallWeight(int W)
    {
        var w = new int[4] { 2, 2, 3, 4 };
        var v = new long[4] { 21, 403, 631, 13 };
        var expected = new long[14]
        {
            0,
            -4611686018427387904,
            403,
            631,
            424,
            1034,
            416,
            1055,
            437,
            1047,
            -4611686018427386849,
            1068,
            -4611686018427386836,
            -4611686018427386836,
        };

        Knapsack.SmallWeight(w.Zip(v).ToArray(), W).ShouldBe(expected[..(W + 1)]);
    }

    [Fact]
    public void SmallValue()
    {
        var w = new long[4] { 2, 2, 3, 4 };
        var v = new int[4] { 0b0001, 0b0100, 0b1000, 0b0101, };
        var expected = new long[]
        {
            0,
            2,
            4611686018427387903,
            4611686018427387903,
            2,
            4,
            6,
            4611686018427387903,
            3,
            5,
            8,
            4611686018427387903,
            5,
            7,
            9,
            4611686018427387903,
            4611686018427387903,
            9,
            11,
        };

        Knapsack.SmallValue(w.Zip(v).ToArray()).ShouldBe(expected);
    }
}