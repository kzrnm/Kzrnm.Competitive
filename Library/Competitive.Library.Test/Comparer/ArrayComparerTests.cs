namespace Kzrnm.Competitive.Testing.Comparer;

public class ArrayComparerTests
{
    [Test, MultipleAssertions]
    public async Task Compare()
    {
        var arr = new[]
        {
            [1,2,3,],
            [1,2,0,],
            [2,2,0,],
            [1,2,0,],
            [2,2,],
            [3,],
            [1,2,],
            [0,],
            Array.Empty<int>(),
        };
        Array.Sort(arr, ArrayComparer<int>.Default);

        var expected = new[]
        {
            Array.Empty<int>(),
            [0,],
            [1,2,],
            [1,2,0,],
            [1,2,0,],
            [1,2,3,],
            [2,2,],
            [2,2,0,],
            [3,],
        };
        await arr.Should().HaveCount(expected.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            await arr[i].Should().BeStrictlyEquivalentTo(expected[i]);
        }
    }

    [Test, MultipleAssertions]
    public async Task Reverse()
    {
        var arr = new[]
        {
            [1,2,3,],
            [1,2,0,],
            [2,2,0,],
            [1,2,0,],
            [2,2,],
            [3,],
            [1,2,],
            [0,],
            Array.Empty<int>(),
        };
        Array.Sort(arr, ArrayComparer<int>.Reverse);

        var expected = new[]
        {
            [3,],
            [2,2,0,],
            [2,2,],
            [1,2,3,],
            [1,2,0,],
            [1,2,0,],
            [1,2,],
            [0,],
            Array.Empty<int>(),
        };
        await arr.Should().HaveCount(expected.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            await arr[i].Should().BeStrictlyEquivalentTo(expected[i]);
        }
    }
}