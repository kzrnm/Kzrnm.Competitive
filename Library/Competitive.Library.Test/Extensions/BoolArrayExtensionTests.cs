using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions;

public class BoolArrayExtensionTests
{
    public static TheoryData<int[], int, bool[]> Arrays => new()
    {
        {
            [2],
            2,
            [false, false]
        },
        {
            [1, 2],
            2,
            [false, true]
        },
        {
            [0, 1],
            2,
            [true, true]
        },
    };
    [Theory]
    [MemberData(nameof(Arrays))]
    public void ToBoolArray(int[] array, int length, bool[] expected)
    {
        array.ToBoolArray(length).ShouldBe(expected);
        array.AsSpan().ToBoolArray(length).ShouldBe(expected);
        array.AsEnumerable().ToBoolArray(length).ShouldBe(expected);
    }
}