using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Algorithm
{
    public class IterToolsTest
    {
        public static TheoryData<int[], int[][]> Permutations_Data = new()
        {
            {
                new[] { 1 },
                new[] {
                    new []{ 1 },
                }
            },
            {
                new[] { 1, 2 },
                new[] {
                    new []{ 1, 2 },
                    new []{ 2, 1 },
                }
            },
            {
                new[] { 1, 2, 3 },
                new[] {
                    new []{ 1, 2, 3 },
                    new []{ 1, 3, 2 },
                    new []{ 2, 1, 3 },
                    new []{ 2, 3, 1 },
                    new []{ 3, 1, 2 },
                    new []{ 3, 2, 1 },
                }
                },
            {
                new[] { 4, 1, 2 },
                new[] {
                    new []{ 4, 1, 2 },
                    new []{ 4, 2, 1 },
                    new []{ 1, 4, 2 },
                    new []{ 1, 2, 4 },
                    new []{ 2, 4, 1 },
                    new []{ 2, 1, 4 },
                }
            },
        };
        [Theory]
        [MemberData(nameof(Permutations_Data))]
        public void Permutations(int[] collection, int[][] expected)
        {
            IterTools.Permutations(collection).Should().BeEquivalentTo(expected);
        }

        public static TheoryData<int[], int, int[][]> Combinations_Data = new()
        {
            {
                new[] { 1 }, 0,
                new[] {
                    new int[0],
                }
            },
            {
                new[] { 1 }, 1,
                new[] {
                    new []{ 1 },
                }
            },

            {
                new[] { 1, 2 }, 0,
                new[] {
                    new int[0],
                }
            },
            {
                new[] { 1, 2 }, 1,
                new[] {
                    new []{ 1 },
                    new []{ 2 },
                }
            },
            {
                new[] { 1, 2 }, 2,
                new[] {
                    new []{ 1, 2 },
                }
            },

            {
                new[] { 1, 2, 3, 4 }, 0,
                new[] {
                    new int[0],
                }
            },
            {
                new[] { 1, 2, 3, 4 }, 1,
                new[] {
                    new []{ 1 },
                    new []{ 2 },
                    new []{ 3 },
                    new []{ 4 },
                }
            },
            {
                new[] { 1, 2, 3, 4 }, 2,
                new[] {
                    new []{ 1, 2 },
                    new []{ 1, 3 },
                    new []{ 1, 4 },
                    new []{ 2, 3 },
                    new []{ 2, 4 },
                    new []{ 3, 4 },
                }
            },
            {
                new[] { 1, 2, 3, 4 }, 3,
                new[] {
                    new []{ 1, 2, 3 },
                    new []{ 1, 2, 4 },
                    new []{ 1, 3, 4 },
                    new []{ 2, 3, 4 },
                }
            },
            {
                new[] { 1, 2, 3, 4 }, 4,
                new[] {
                    new []{ 1, 2, 3, 4 },
                }
            },
        };
        [Theory]
        [MemberData(nameof(Combinations_Data))]
        public void Combinations(int[] collection, int k, int[][] expected)
        {
            IterTools.Combinations(collection, k).Should().BeEquivalentTo(expected);
        }


        public static TheoryData<int[], int, int[][]> CombinationsWithReplacement_Data = new()
        {
            {
                new[] { 1 }, 0,
                new[] {
                    new int[0],
                }
            },
            {
                new[] { 1 }, 1,
                new[] {
                    new []{ 1 },
                }
            },
            {
                new[] { 1 }, 3,
                new[] {
                    new []{ 1, 1, 1 },
                }
            },

            {
                new[] { 1, 2 }, 0,
                new[] {
                    new int[0],
                }
            },
            {
                new[] { 1, 2 }, 1,
                new[] {
                    new []{ 1 },
                    new []{ 2 },
                }
            },
            {
                new[] { 1, 2 }, 2,
                new[] {
                    new []{ 1, 1 },
                    new []{ 1, 2 },
                    new []{ 2, 2 },
                }
            },

            {
                new[] { 1, 2, 3 }, 4,
                new[] {
                    new []{ 1, 1, 1, 1 },
                    new []{ 1, 1, 1, 2 },
                    new []{ 1, 1, 1, 3 },
                    new []{ 1, 1, 2, 2 },
                    new []{ 1, 1, 2, 3 },
                    new []{ 1, 1, 3, 3 },
                    new []{ 1, 2, 2, 2 },
                    new []{ 1, 2, 2, 3 },
                    new []{ 1, 2, 3, 3 },
                    new []{ 1, 3, 3, 3 },
                    new []{ 2, 2, 2, 2 },
                    new []{ 2, 2, 2, 3 },
                    new []{ 2, 2, 3, 3 },
                    new []{ 2, 3, 3, 3 },
                    new []{ 3, 3, 3, 3 },
                }
            },

            {
                new[] { 1, 2, 3, 4 }, 0,
                new[] {
                    new int[0],
                }
            },
            {
                new[] { 1, 2, 3, 4 }, 1,
                new[] {
                    new []{ 1 },
                    new []{ 2 },
                    new []{ 3 },
                    new []{ 4 },
                }
            },
            {
                new[] { 1, 2, 3, 4 }, 2,
                new[] {
                    new []{ 1, 1 },
                    new []{ 1, 2 },
                    new []{ 1, 3 },
                    new []{ 1, 4 },
                    new []{ 2, 2 },
                    new []{ 2, 3 },
                    new []{ 2, 4 },
                    new []{ 3, 3 },
                    new []{ 3, 4 },
                    new []{ 4, 4 },
                }
            },
            {
                new[] { 1, 2, 3, 4 }, 3,
                new[] {
                    new []{ 1, 1, 1 },
                    new []{ 1, 1, 2 },
                    new []{ 1, 1, 3 },
                    new []{ 1, 1, 4 },
                    new []{ 1, 2, 2 },
                    new []{ 1, 2, 3 },
                    new []{ 1, 2, 4 },
                    new []{ 1, 3, 3 },
                    new []{ 1, 3, 4 },
                    new []{ 1, 4, 4 },
                    new []{ 2, 2, 2 },
                    new []{ 2, 2, 3 },
                    new []{ 2, 2, 4 },
                    new []{ 2, 3, 3 },
                    new []{ 2, 3, 4 },
                    new []{ 2, 4, 4 },
                    new []{ 3, 3, 3 },
                    new []{ 3, 3, 4 },
                    new []{ 3, 4, 4 },
                    new []{ 4, 4, 4 },
                }
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
