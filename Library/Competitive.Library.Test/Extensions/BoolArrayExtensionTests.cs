using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class BoolArrayExtensionTests
    {
        public static TheoryData Arrays = new TheoryData<int[], int, bool[]>
        {
            {
                new int[] { 2 },
                2,
                new bool[2] { false, false }
            },
            {
                new int[] { 1, 2 },
                2,
                new bool[2] { false, true }
            },
            {
                new int[] { 0, 1 },
                2,
                new bool[2] { true, true }
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
}