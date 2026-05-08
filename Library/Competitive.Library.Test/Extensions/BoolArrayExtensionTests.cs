namespace Kzrnm.Competitive.Testing.Extensions;

public class BoolArrayExtensionTests
{
    public static IEnumerable<(int[], int, bool[])> Arrays =>
    [
        (
            [2],
            2,
            [false, false]
        ),
        (
            [1, 2],
            2,
            [false, true]
        ),
        (
            [0, 1],
            2,
            [true, true]
        ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Arrays))]
    public async Task ToBoolArray(int[] array, int length, bool[] expected)
    {
        await array.ToBoolArray(length).Should().BeStrictlyEquivalentTo(expected);
        await array.AsSpan().ToBoolArray(length).Should().BeStrictlyEquivalentTo(expected);
        await array.AsEnumerable().ToBoolArray(length).Should().BeStrictlyEquivalentTo(expected);
    }
}