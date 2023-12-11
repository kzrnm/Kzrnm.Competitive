
namespace Kzrnm.Competitive.Testing.Algorithm
{
    public class IterToolsTest
    {
        public static TheoryData<int[], int[][]> Permutations_Data => new()
        {
            {
                [1],
                [[1],]
            },
            {
                [ 1, 2 ],
                [
                    [1, 2],
                    [2, 1],
                ]
            },
            {
                [ 1, 2, 3 ],
                [
                    [1, 2, 3],
                    [1, 3, 2],
                    [2, 1, 3],
                    [2, 3, 1],
                    [3, 1, 2],
                    [3, 2, 1],
                ]
            },
            {
                [ 4, 1, 2 ],
                [
                    [4, 1, 2],
                    [4, 2, 1],
                    [1, 4, 2],
                    [1, 2, 4],
                    [2, 4, 1],
                    [2, 1, 4],
                ]
            },
        };
        [Theory]
        [MemberData(nameof(Permutations_Data))]
        public void Permutations(int[] collection, int[][] expected)
        {
            IterTools.Permutations(collection).Should().BeEquivalentTo(expected);
        }

        public static TheoryData<int[], int, int[][]> Combinations_Data => new()
        {
            {
                [1], 0,
                [[]]
            },
            {
                [1], 1,
                [[ 1 ]]
            },

            {
                [1,2], 0,
                [[]]
            },
            {
                [1,2], 1,
                [[1], [2],]
            },
            {
                [1,2], 2,
                [[1,2],]
            },

            {
                [1, 2, 3, 4], 0,
                [[]]
            },
            {
                [1, 2, 3, 4], 1,
                [[1], [2], [3], [4]]
            },
            {
                [1, 2, 3, 4], 2,
                [[1, 2], [1, 3], [1, 4], [2, 3], [2, 4], [3, 4],]
            },
            {
                [1, 2, 3, 4], 3,
                [[1, 2, 3], [1, 2, 4], [1, 3, 4], [2, 3, 4],]
            },
            {
                [1, 2, 3, 4], 4,
                [[1, 2, 3, 4]]
            },
        };
        [Theory]
        [MemberData(nameof(Combinations_Data))]
        public void Combinations(int[] collection, int k, int[][] expected)
        {
            IterTools.Combinations(collection, k).Should().BeEquivalentTo(expected);
        }


        public static TheoryData<int[], int, int[][]> CombinationsWithReplacement_Data => new()
        {
            {
                [1], 0,
                [[]]
            },
            {
                [1], 1,
                [[1]]
            },
            {
                [1], 3,
                [[1, 1, 1]]
            },

            {
                [1,2], 0,
                [[]]
            },
            {
                [1,2], 1,
                [[1], [2],]
            },
            {
                [1,2], 2,
                [[1, 1], [1, 2], [2, 2],]
            },

            {
                [1,2,3], 4,
                [
                    [1, 1, 1, 1],
                    [1, 1, 1, 2],
                    [1, 1, 1, 3],
                    [1, 1, 2, 2],
                    [1, 1, 2, 3],
                    [1, 1, 3, 3],
                    [1, 2, 2, 2],
                    [1, 2, 2, 3],
                    [1, 2, 3, 3],
                    [1, 3, 3, 3],
                    [2, 2, 2, 2],
                    [2, 2, 2, 3],
                    [2, 2, 3, 3],
                    [2, 3, 3, 3],
                    [3, 3, 3, 3],
                ]
            },

            {
                [1, 2, 3, 4], 0,
                [[]]
            },
            {
                [1, 2, 3, 4], 1,
                [
                    [1],
                    [2],
                    [3],
                    [4],
                ]
            },
            {
                [1, 2, 3, 4], 2,
                [
                    [1, 1],
                    [1, 2],
                    [1, 3],
                    [1, 4],
                    [2, 2],
                    [2, 3],
                    [2, 4],
                    [3, 3],
                    [3, 4],
                    [4, 4],
                ]
            },
            {
                [1, 2, 3, 4], 3,
                [
                    [1, 1, 1],
                    [1, 1, 2],
                    [1, 1, 3],
                    [1, 1, 4],
                    [1, 2, 2],
                    [1, 2, 3],
                    [1, 2, 4],
                    [1, 3, 3],
                    [1, 3, 4],
                    [1, 4, 4],
                    [2, 2, 2],
                    [2, 2, 3],
                    [2, 2, 4],
                    [2, 3, 3],
                    [2, 3, 4],
                    [2, 4, 4],
                    [3, 3, 3],
                    [3, 3, 4],
                    [3, 4, 4],
                    [4, 4, 4],
                ]
            },
        };
        [Theory]
        [MemberData(nameof(CombinationsWithReplacement_Data))]
        public void CombinationsWithReplacement(int[] collection, int k, int[][] expected)
        {
            IterTools.CombinationsWithReplacement(collection, k).Should().BeEquivalentTo(expected);
        }
    }
}
