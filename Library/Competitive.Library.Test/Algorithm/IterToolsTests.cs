
namespace Kzrnm.Competitive.Testing.Algorithm;

public class IterToolsTest
{
    public static IEnumerable<(int[], int[][])> Permutations_Data =>
    [
        (
            [1],
            [[1],]
        ),
        (
            [ 1, 2 ],
            [
                [1, 2],
                [2, 1],
            ]
        ),
        (
            [ 1, 2, 3 ],
            [
                [1, 2, 3],
                [1, 3, 2],
                [2, 1, 3],
                [2, 3, 1],
                [3, 1, 2],
                [3, 2, 1],
            ]
        ),
        (
            [ 4, 1, 2 ],
            [
                [4, 1, 2],
                [4, 2, 1],
                [1, 4, 2],
                [1, 2, 4],
                [2, 4, 1],
                [2, 1, 4],
            ]
        ),
    ];
    [Test]
    [MethodDataSource(nameof(Permutations_Data))]
    public async Task Permutations(int[] collection, int[][] expected)
    {
        await IterTools.Permutations(collection).Should().BeStrictlyEquivalentTo(expected, CollectionEqualityComparer<int>.Default);
    }

    public static IEnumerable<(int[], int, int[][])> Combinations_Data =>
    [
        (
            [1], 0,
            [[]]
        ),
        (
            [1], 1,
            [[ 1 ]]
        ),
        (
            [1,2], 0,
            [[]]
        ),
        (
            [1,2], 1,
            [[1], [2],]
        ),
        (
            [1,2], 2,
            [[1,2],]
        ),
        (
            [1, 2, 3, 4], 0,
            [[]]
        ),
        (
            [1, 2, 3, 4], 1,
            [[1], [2], [3], [4]]
        ),
        (
            [1, 2, 3, 4], 2,
            [[1, 2], [1, 3], [1, 4], [2, 3], [2, 4], [3, 4],]
        ),
        (
            [1, 2, 3, 4], 3,
            [[1, 2, 3], [1, 2, 4], [1, 3, 4], [2, 3, 4],]
        ),
        (
            [1, 2, 3, 4], 4,
            [[1, 2, 3, 4]]
        ),
    ];
    [Test]
    [MethodDataSource(nameof(Combinations_Data))]
    public async Task Combinations(int[] collection, int k, int[][] expected)
    {
        await IterTools.Combinations(collection, k).Should().BeStrictlyEquivalentTo(expected, CollectionEqualityComparer<int>.Default);
    }


    public static IEnumerable<(int[], int, int[][])> CombinationsWithReplacement_Data =>
    [
        (
            [1], 0,
            [[]]
        ),
        (
            [1], 1,
            [[1]]
        ),
        (
            [1], 3,
            [[1, 1, 1]]
        ),
        (
            [1,2], 0,
            [[]]
        ),
        (
            [1,2], 1,
            [[1], [2],]
        ),
        (
            [1,2], 2,
            [[1, 1], [1, 2], [2, 2],]
        ),
        (
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
        ),
            (
            [1, 2, 3, 4], 0,
            [[]]
        ),
        (
            [1, 2, 3, 4], 1,
            [
                [1],
                [2],
                [3],
                [4],
            ]
        ),
        (
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
        ),
        (
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
        ),
    ];
    [Test]
    [MethodDataSource(nameof(CombinationsWithReplacement_Data))]
    public async Task CombinationsWithReplacement(int[] collection, int k, int[][] expected)
    {
        await IterTools.CombinationsWithReplacement(collection, k).Should().BeStrictlyEquivalentTo(expected, CollectionEqualityComparer<int>.Default);
    }
}