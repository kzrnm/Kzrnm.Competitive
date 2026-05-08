namespace Kzrnm.Competitive.Testing.Extensions;

public class CollectionExtensionTests
{
    [Test]
    public async Task MaxBy()
    {
        await new long[] {
            43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
        }.MaxBy().Should().BeEqualTo((17, 3401434));
    }

    [Test]
    public async Task MaxByArrayFunc()
    {
        await new (int, long)[] {
            (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
        }.MaxBy(tup => tup.Item2).Should().BeEqualTo((17, (0, 3401434)));
    }

    [Test]
    public async Task MaxByFunc()
    {
        await new List<(int, long)> {
            (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
        }.MaxBy2(tup => tup.Item2).Should().BeEqualTo(((0, 3401434), 3401434));
    }


    [Test]
    public async Task MinBy()
    {
        await new long[] {
            43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
        }.MinBy().Should().BeEqualTo((3, 4));
    }

    [Test]
    public async Task MinByArrayFunc()
    {
        await new (int, long)[] {
            (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
        }.MinBy(tup => tup.Item2).Should().BeEqualTo((3, (0, 4)));
    }

    [Test]
    public async Task MinByFunc()
    {
        await new List<(int, long)> {
            (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
        }.MinBy2(tup => tup.Item2).Should().BeEqualTo(((0, 4), 4));
    }

    [Test, MultipleAssertions]
    public async Task GroupCount()
    {
        var g = new[] {
            StringComparison.OrdinalIgnoreCase,
            StringComparison.OrdinalIgnoreCase,
            StringComparison.InvariantCulture,
            StringComparison.InvariantCultureIgnoreCase,
            StringComparison.CurrentCulture,
            StringComparison.CurrentCulture,
            StringComparison.CurrentCulture,
            StringComparison.OrdinalIgnoreCase,
            StringComparison.Ordinal,
            StringComparison.OrdinalIgnoreCase,
            StringComparison.InvariantCulture,
        }.GroupCount();
        await g.Should().HaveCount(5);
        await g.Should().ContainKeyWithValue(StringComparison.Ordinal, 1);
        await g.Should().ContainKeyWithValue(StringComparison.OrdinalIgnoreCase, 4);
        await g.Should().ContainKeyWithValue(StringComparison.CurrentCulture, 3);
        await g.Should().ContainKeyWithValue(StringComparison.InvariantCulture, 2);
        await g.Should().ContainKeyWithValue(StringComparison.InvariantCultureIgnoreCase, 1);
    }

    [Test, MultipleAssertions]
    public async Task GroupCountFunc()
    {
        var g = new long[] {
            43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
        }.GroupCount(i => i % 7);
        await g.Should().HaveCount(7);
        await g.Should().ContainKeyWithValue(0, 4);
        await g.Should().ContainKeyWithValue(1, 7);
        await g.Should().ContainKeyWithValue(2, 1);
        await g.Should().ContainKeyWithValue(3, 3);
        await g.Should().ContainKeyWithValue(4, 2);
        await g.Should().ContainKeyWithValue(5, 2);
        await g.Should().ContainKeyWithValue(6, 2);
    }


    [Test, MultipleAssertions]
    public async Task GroupIndex()
    {
        var grouped = new[] {
            StringComparison.OrdinalIgnoreCase,
            StringComparison.OrdinalIgnoreCase,
            StringComparison.InvariantCulture,
            StringComparison.InvariantCultureIgnoreCase,
            StringComparison.CurrentCulture,
            StringComparison.CurrentCulture,
            StringComparison.CurrentCulture,
            StringComparison.OrdinalIgnoreCase,
            StringComparison.Ordinal,
            StringComparison.OrdinalIgnoreCase,
            StringComparison.InvariantCulture,
        }.GroupIndex().ToArray();

        await grouped.Should().HaveCount(5);

        await grouped[0].Key.Should().BeEqualTo(StringComparison.OrdinalIgnoreCase);
        await grouped[0].Should().BeStrictlyEquivalentTo([0, 1, 7, 9]);

        await grouped[1].Key.Should().BeEqualTo(StringComparison.InvariantCulture);
        await grouped[1].Should().BeStrictlyEquivalentTo([2, 10]);

        await grouped[2].Key.Should().BeEqualTo(StringComparison.InvariantCultureIgnoreCase);
        await grouped[2].Should().BeStrictlyEquivalentTo([3]);

        await grouped[3].Key.Should().BeEqualTo(StringComparison.CurrentCulture);
        await grouped[3].Should().BeStrictlyEquivalentTo([4, 5, 6]);

        await grouped[4].Key.Should().BeEqualTo(StringComparison.Ordinal);
        await grouped[4].Should().BeStrictlyEquivalentTo([8]);
    }

    [Test, MultipleAssertions]
    public async Task GroupIndexFunc()
    {
        var grouped = new long[] {
            43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
        }.GroupIndex(i => i % 7).ToArray();

        await grouped.Should().HaveCount(7);

        await grouped[0].Key.Should().BeEqualTo(1);
        await grouped[0].Should().BeStrictlyEquivalentTo([0, 2, 6, 9, 17, 18, 20]);

        await grouped[1].Key.Should().BeEqualTo(3);
        await grouped[1].Should().BeStrictlyEquivalentTo([1, 11, 14]);

        await grouped[2].Key.Should().BeEqualTo(4);
        await grouped[2].Should().BeStrictlyEquivalentTo([3, 10]);

        await grouped[3].Key.Should().BeEqualTo(0);
        await grouped[3].Should().BeStrictlyEquivalentTo([4, 5, 7, 16]);

        await grouped[4].Key.Should().BeEqualTo(6);
        await grouped[4].Should().BeStrictlyEquivalentTo([8, 19]);

        await grouped[5].Key.Should().BeEqualTo(5);
        await grouped[5].Should().BeStrictlyEquivalentTo([12, 15]);

        await grouped[6].Key.Should().BeEqualTo(2);
        await grouped[6].Should().BeStrictlyEquivalentTo([13]);
    }


    [Test, MultipleAssertions]
    public async Task Flatten()
    {
        string[] strs = ["abc", "def", "012", "345", "678"];
        await strs.Flatten().Should().BeStrictlyEquivalentTo(['a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8']);

        int[][] arr = [
            [1, 2, 3],
            [-1, -2, -3],
            [4, 5, 6],
            [-6, -5, -4],
            [7, 8, 9],
        ];
        var expected = new[] { 1, 2, 3, -1, -2, -3, 4, 5, 6, -6, -5, -4, 7, 8, 9 };

        await arr.Flatten().Should().BeStrictlyEquivalentTo(expected);
        await arr.ToList().Flatten().Should().BeStrictlyEquivalentTo(expected);
    }

    [Test]
    public async Task FlattenTuple2()
    {
        await new[] { (1, 2), (3, 4), (5, 6) }.Flatten().Should().BeStrictlyEquivalentTo([1, 2, 3, 4, 5, 6]);
    }

    [Test]
    public async Task FlattenTuple3()
    {
        await new[] { (1, 2, 3), (4, 5, 6) }.Flatten().Should().BeStrictlyEquivalentTo([1, 2, 3, 4, 5, 6]);
    }

    [Test, MultipleAssertions]
    public async Task MinMax()
    {
        await new int[] {
            4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
          -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
           27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MinMax().Should().BeEqualTo((-75, 81));
        await new List<int> {
            4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
          -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
           27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MinMax().Should().BeEqualTo((-75, 81));
        await ((Span<int>)[
            4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
          -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
           27, 13,-75,-61, 76, 40,-27, 48, 36, -17]).MinMax().Should().BeEqualTo((-75, 81));
        await ((ReadOnlySpan<int>)[
            4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
          -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
           27, 13,-75,-61, 76, 40,-27, 48, 36, -17]).MinMax().Should().BeEqualTo((-75, 81));

        await Array.Empty<int>().MinMax().Should().BeEqualTo((0, 0));
        await Array.Empty<int>().AsSpan().MinMax().Should().BeEqualTo((0, 0));
    }

    [Test, MultipleAssertions]
    public async Task SpanSelect()
    {
        var arr = Enumerable.Range(0, 10).ToArray();
        static int Func(int n) => 2 * n;
        await arr.AsSpan().Select(Func).ToArray().Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 10).Select(Func));
        await ((ReadOnlySpan<int>)arr).Select(Func).ToArray().Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 10).Select(Func));

        static int FuncIndex(int n, int i) => i * n;
        await arr.AsSpan().Select(FuncIndex).ToArray().Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 10).Select(FuncIndex));
        await ((ReadOnlySpan<int>)arr).Select(FuncIndex).ToArray().Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 10).Select(FuncIndex));
    }

    public static IEnumerable<(int[], int, int[][])> Chunk_Data =>
    [
        (
            Enumerable.Range(0, 12).ToArray(),
            4,
            [
                [0, 1, 2, 3,],
                [4, 5, 6, 7,],
                [8, 9, 10, 11,],
            ]
        ),
        (
            Enumerable.Range(0, 12).ToArray(),
            5,
            [
                [0, 1, 2, 3, 4,],
                [5, 6, 7, 8, 9,],
                [10, 11,],
            ]
        ),
        (
            Enumerable.Empty<int>().ToArray(),
            5,
            []
        ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Chunk_Data))]
    public async Task Chunk(int[] input, int bufferSize, int[][] expected)
    {
        var result = input.Select(t => t).Chunk(bufferSize).ToArray();
        await result.Should().HaveCount(expected.Length);
        for (int i = 0; i < expected.Length; i++)
            await result[i].Should().BeStrictlyEquivalentTo(expected[i]);
    }

    public static IEnumerable<(int[], (int, int)[])> Tupled2_Data =>
    [
        (
            [1,2,3,4,5,6],
            [(1,2),(2,3),(3,4),(4,5),(5,6)]
        ),
        (
            [1],
            []
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Tupled2_Data))]
    public async Task Tupled2(int[] array, (int, int)[] expected)
    {
        await array.Tupled2().Should().BeStrictlyEquivalentTo(expected);
        await new Span<int>(array).Tupled2().Should().BeStrictlyEquivalentTo(expected);
        await new ReadOnlySpan<int>(array).Tupled2().Should().BeStrictlyEquivalentTo(expected);
    }

    public static IEnumerable<(byte[], (byte, int)[])> CompressCount_Data =>
    [
        (
            Enumerable.Range(0, 6).Select(i => (byte)i).ToArray(),
            [
                (0, 1),
                (1, 1),
                (2, 1),
                (3, 1),
                (4, 1),
                (5, 1),
            ]
        ),
        (
            [1, 1, 2, 2, 3, 3,],
            [
                (1, 2),
                (2, 2),
                (3, 2),
            ]
        ),
        (
            [1, 1, 2, 2, 3],
            [
                (1, 2),
                (2, 2),
                (3, 1),
            ]
        ),
        (
            [1, 2, 2, 3, 3,],
            [
                (1, 1),
                (2, 2),
                (3, 2),
            ]
        ),
        (
            [1, 2, 2, 3, 3, 1, 1, 2,],
            [
                (1, 1),
                (2, 2),
                (3, 2),
                (1, 2),
                (2, 1),
            ]
        ),
    ];
    [Test]
    [MethodDataSource(nameof(CompressCount_Data))]
    public async Task CompressCount(byte[] input, (byte, int)[] expected)
    {
        await input.Select(t => t).CompressCount().Should().BeStrictlyEquivalentTo(expected);
    }

    public static IEnumerable<(string, (char, int)[])> CompressCount2_Data =>
    [
        (
            "<<>>",
            [
                ('<', 2),
                ('>', 2),
            ]
        ),
        (
            "<<><>>><><>><><><<>><<<><><<>",
            [
                ('<', 2),
                ('>', 1),
                ('<', 1),
                ('>', 3),
                ('<', 1),
                ('>', 1),
                ('<', 1),
                ('>', 2),
                ('<', 1),
                ('>', 1),
                ('<', 1),
                ('>', 1),
                ('<', 2),
                ('>', 2),
                ('<', 3),
                ('>', 1),
                ('<', 1),
                ('>', 1),
                ('<', 2),
                ('>', 1),
            ]
        ),
    ];
    [Test]
    [MethodDataSource(nameof(CompressCount2_Data))]
    public async Task CompressCount2(string input, (char, int)[] expected)
    {
        await input.CompressCount().Should().BeStrictlyEquivalentTo(expected);
    }
}