using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Algorithm
{
    public class LongestIncreasingSubsequenceTests
    {
        record struct CaseData(int[] Input,
            int[] Strictly, int[] NotStrictly, int[] ReverseStrictly, int[] ReverseNotStrictly,
            int[] StrictlyIndex, int[] NotStrictlyIndex, int[] ReverseStrictlyIndex, int[] ReverseNotStrictlyIndex);
        static CaseData[] Cases = [
            new()
            {
                Input = [],
                Strictly = [],
                StrictlyIndex = [],

                NotStrictly = [],
                NotStrictlyIndex = [],

                ReverseStrictly = [],
                ReverseStrictlyIndex = [],

                ReverseNotStrictly = [],
                ReverseNotStrictlyIndex = [],
            },
            new()
            {
                Input = [-4, 4, 2, 4, 3, 2, 1, 1, 0, -1],
                Strictly = [-4, 2, 3],
                StrictlyIndex = [0, 2, 4],

                NotStrictly = [-4, 1, 1],
                NotStrictlyIndex = [0, 6, 7],

                ReverseStrictly = [4, 3, 2, 1, 0, -1],
                ReverseStrictlyIndex = [1, 4, 5, 6, 8, 9],

                ReverseNotStrictly = [4, 4, 3, 2, 1, 1, 0, -1],
                ReverseNotStrictlyIndex = [1, 3, 4, 5, 6, 7, 8, 9],
            },
            new()
            {
                Input = [9, 1, 3, 1, 2, 3, 4, 4, 5, 6],
                Strictly = [1, 2, 3, 4, 5, 6],
                StrictlyIndex = [1, 4, 5, 6, 8, 9],

                NotStrictly = [1, 1, 2, 3, 4, 4, 5, 6],
                NotStrictlyIndex = [1, 3, 4, 5, 6, 7, 8, 9],

                ReverseStrictly = [9, 3, 2],
                ReverseStrictlyIndex = [0, 2, 4],

                ReverseNotStrictly = [9, 4, 4],
                ReverseNotStrictlyIndex = [0, 6, 7],
            },
            new()
            {
                Input = [1,6,1,3,3,5,4,2,3,5,4,3,7,5,1,4,2],
                Strictly = [1, 2, 3, 4, 5],
                StrictlyIndex = [0, 7, 8, 10, 13],

                NotStrictly = [1, 1, 3, 3, 3, 3, 4],
                NotStrictlyIndex = [0, 2, 3, 4, 8, 11, 15],

                ReverseStrictly = [6, 5, 4, 3, 2],
                ReverseStrictlyIndex = [1, 5, 6, 8, 16],

                ReverseNotStrictly = [6, 5, 5, 5, 4, 2],
                ReverseNotStrictlyIndex = [1, 5, 9, 13, 15, 16],
            },
        ];

        public static IEnumerable<TheoryDataRow<int[], int[], int[]>> Strictly_Data() =>
            Cases.Select(t => new TheoryDataRow<int[], int[], int[]>(t.Input, t.Strictly, t.StrictlyIndex));

        [Theory]
        [MemberData(nameof(Strictly_Data))]
        public void Strictly(int[] input, int[] expectedLis, int[] expectedIndex)
        {
            LongestIncreasingSubsequence.Lis(input).ShouldSatisfyAllConditions([
                t => t.Lis.ShouldBe(expectedLis),
                t => t.Indexes.ShouldBe(expectedIndex),
            ]);
        }

        public static IEnumerable<TheoryDataRow<int[], int[], int[]>> NotStrictly_Data() =>
            Cases.Select(t => new TheoryDataRow<int[], int[], int[]>(t.Input, t.NotStrictly, t.NotStrictlyIndex));

        [Theory]
        [MemberData(nameof(NotStrictly_Data))]
        public void NotStrictly(int[] input, int[] expectedLis, int[] expectedIndex)
        {
            LongestIncreasingSubsequence.Lis(input, false).ShouldSatisfyAllConditions([
                t => t.Lis.ShouldBe(expectedLis),
                t => t.Indexes.ShouldBe(expectedIndex),
            ]);
        }

        public static IEnumerable<TheoryDataRow<int[], int[], int[]>> ReverseStrictly_Data() =>
            Cases.Select(t => new TheoryDataRow<int[], int[], int[]>(t.Input, t.ReverseStrictly, t.ReverseStrictlyIndex));

        [Theory]
        [MemberData(nameof(ReverseStrictly_Data))]
        public void ReverseStrictly(int[] input, int[] expectedLis, int[] expectedIndex)
        {
            LongestIncreasingSubsequence.Lis(input, new ReverseComparer<int>()).ShouldSatisfyAllConditions([
                t => t.Lis.ShouldBe(expectedLis),
                t => t.Indexes.ShouldBe(expectedIndex),
            ]);
        }

        public static IEnumerable<TheoryDataRow<int[], int[], int[]>> ReverseNotStrictly_Data() =>
            Cases.Select(t => new TheoryDataRow<int[], int[], int[]>(t.Input, t.ReverseNotStrictly, t.ReverseNotStrictlyIndex));

        [Theory]
        [MemberData(nameof(ReverseNotStrictly_Data))]
        public void ReverseNotStrictly(int[] input, int[] expectedLis, int[] expectedIndex)
        {
            LongestIncreasingSubsequence.Lis(input, new ReverseComparer<int>(), false).ShouldSatisfyAllConditions([
                t => t.Lis.ShouldBe(expectedLis),
                t => t.Indexes.ShouldBe(expectedIndex),
            ]);
        }
    }
}
