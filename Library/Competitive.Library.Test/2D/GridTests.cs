using Kzrnm.Competitive.IO;
using TUnit.Assertions.Enums;

namespace Kzrnm.Competitive.Testing.TwoDimensional;

public class GridTests
{
    [Test, MultipleAssertions]
    public async Task String()
    {
        var grid = Grid.Create(
        [
            "...#.",
            ".#.#.",
            ".#...",
        ]);

        await grid.H.Should().BeEqualTo(3);
        await grid.W.Should().BeEqualTo(5);
        await grid[0, 0].Should().BeEqualTo('.');
        await grid[0, 1].Should().BeEqualTo('.');
        await grid[0, 2].Should().BeEqualTo('.');
        await grid[0, 3].Should().BeEqualTo('#');
        await grid[0, 4].Should().BeEqualTo('.');
        await grid[1, 0].Should().BeEqualTo('.');
        await grid[1, 1].Should().BeEqualTo('#');
        await grid[1, 2].Should().BeEqualTo('.');
        await grid[1, 3].Should().BeEqualTo('#');
        await grid[1, 4].Should().BeEqualTo('.');
        await grid[2, 0].Should().BeEqualTo('.');
        await grid[2, 1].Should().BeEqualTo('#');
        await grid[2, 2].Should().BeEqualTo('.');
        await grid[2, 3].Should().BeEqualTo('.');
        await grid[2, 4].Should().BeEqualTo('.');
        await grid[-1, 0].Should().BeEqualTo('\0');
        await grid[3, 0].Should().BeEqualTo('\0');
        await grid[-1, -1].Should().BeEqualTo('\0');
        await grid[-1, 5].Should().BeEqualTo('\0');
        await grid[0, 5].Should().BeEqualTo('\0');
        await grid[3, 5].Should().BeEqualTo('\0');
        await grid[0, -1].Should().BeEqualTo('\0');
        await grid[3, -1].Should().BeEqualTo('\0');
        await grid[5, 5].Should().BeEqualTo('\0');
    }
    [Test, MultipleAssertions]
    public async Task StringDefault()
    {
        var grid = Grid.Create([
            "...#.",
            ".#.#.",
            ".#...",
        ], '-');
        await grid.H.Should().BeEqualTo(3);
        await grid.W.Should().BeEqualTo(5);
        await grid[0, 0].Should().BeEqualTo('.');
        await grid[0, 1].Should().BeEqualTo('.');
        await grid[0, 2].Should().BeEqualTo('.');
        await grid[0, 3].Should().BeEqualTo('#');
        await grid[0, 4].Should().BeEqualTo('.');
        await grid[1, 0].Should().BeEqualTo('.');
        await grid[1, 1].Should().BeEqualTo('#');
        await grid[1, 2].Should().BeEqualTo('.');
        await grid[1, 3].Should().BeEqualTo('#');
        await grid[1, 4].Should().BeEqualTo('.');
        await grid[2, 0].Should().BeEqualTo('.');
        await grid[2, 1].Should().BeEqualTo('#');
        await grid[2, 2].Should().BeEqualTo('.');
        await grid[2, 3].Should().BeEqualTo('.');
        await grid[2, 4].Should().BeEqualTo('.');
        await grid[-1, 0].Should().BeEqualTo('-');
        await grid[3, 0].Should().BeEqualTo('-');
        await grid[-1, -1].Should().BeEqualTo('-');
        await grid[-1, 5].Should().BeEqualTo('-');
        await grid[0, 5].Should().BeEqualTo('-');
        await grid[3, 5].Should().BeEqualTo('-');
        await grid[0, -1].Should().BeEqualTo('-');
        await grid[3, -1].Should().BeEqualTo('-');
        await grid[5, 5].Should().BeEqualTo('-');
    }

    [Test, MultipleAssertions]
    public async Task Ascii()
    {
        var grid = Grid.Create(
        [
            new Asciis("...#."u8.ToArray()),
            new Asciis(".#.#."u8.ToArray()),
            new Asciis(".#..."u8.ToArray()),
        ]);
        await grid.H.Should().BeEqualTo(3);
        await grid.W.Should().BeEqualTo(5);
        await grid[0, 0].Should().BeEqualTo((Ascii)'.');
        await grid[0, 1].Should().BeEqualTo((Ascii)'.');
        await grid[0, 2].Should().BeEqualTo((Ascii)'.');
        await grid[0, 3].Should().BeEqualTo((Ascii)'#');
        await grid[0, 4].Should().BeEqualTo((Ascii)'.');
        await grid[1, 0].Should().BeEqualTo((Ascii)'.');
        await grid[1, 1].Should().BeEqualTo((Ascii)'#');
        await grid[1, 2].Should().BeEqualTo((Ascii)'.');
        await grid[1, 3].Should().BeEqualTo((Ascii)'#');
        await grid[1, 4].Should().BeEqualTo((Ascii)'.');
        await grid[2, 0].Should().BeEqualTo((Ascii)'.');
        await grid[2, 1].Should().BeEqualTo((Ascii)'#');
        await grid[2, 2].Should().BeEqualTo((Ascii)'.');
        await grid[2, 3].Should().BeEqualTo((Ascii)'.');
        await grid[2, 4].Should().BeEqualTo((Ascii)'.');
        await grid[-1, 0].Should().BeEqualTo((Ascii)'\0');
        await grid[3, 0].Should().BeEqualTo((Ascii)'\0');
        await grid[-1, -1].Should().BeEqualTo((Ascii)'\0');
        await grid[-1, 5].Should().BeEqualTo((Ascii)'\0');
        await grid[0, 5].Should().BeEqualTo((Ascii)'\0');
        await grid[3, 5].Should().BeEqualTo((Ascii)'\0');
        await grid[0, -1].Should().BeEqualTo((Ascii)'\0');
        await grid[3, -1].Should().BeEqualTo((Ascii)'\0');
        await grid[5, 5].Should().BeEqualTo((Ascii)'\0');
        await grid.ToString().Should().BeEqualTo("""
...#.
.#.#.
.#...
""");
    }
    [Test, MultipleAssertions]
    public async Task AsciiDefault()
    {
        var grid = Grid.Create([
            new Asciis("...#."u8.ToArray()),
            new Asciis(".#.#."u8.ToArray()),
            new Asciis(".#..."u8.ToArray()),
        ], '-');
        await grid.H.Should().BeEqualTo((Ascii)3);
        await grid.W.Should().BeEqualTo((Ascii)5);
        await grid[0, 0].Should().BeEqualTo((Ascii)'.');
        await grid[0, 1].Should().BeEqualTo((Ascii)'.');
        await grid[0, 2].Should().BeEqualTo((Ascii)'.');
        await grid[0, 3].Should().BeEqualTo((Ascii)'#');
        await grid[0, 4].Should().BeEqualTo((Ascii)'.');
        await grid[1, 0].Should().BeEqualTo((Ascii)'.');
        await grid[1, 1].Should().BeEqualTo((Ascii)'#');
        await grid[1, 2].Should().BeEqualTo((Ascii)'.');
        await grid[1, 3].Should().BeEqualTo((Ascii)'#');
        await grid[1, 4].Should().BeEqualTo((Ascii)'.');
        await grid[2, 0].Should().BeEqualTo((Ascii)'.');
        await grid[2, 1].Should().BeEqualTo((Ascii)'#');
        await grid[2, 2].Should().BeEqualTo((Ascii)'.');
        await grid[2, 3].Should().BeEqualTo((Ascii)'.');
        await grid[2, 4].Should().BeEqualTo((Ascii)'.');
        await grid[-1, 0].Should().BeEqualTo((Ascii)'-');
        await grid[3, 0].Should().BeEqualTo((Ascii)'-');
        await grid[-1, -1].Should().BeEqualTo((Ascii)'-');
        await grid[-1, 5].Should().BeEqualTo((Ascii)'-');
        await grid[0, 5].Should().BeEqualTo((Ascii)'-');
        await grid[3, 5].Should().BeEqualTo((Ascii)'-');
        await grid[0, -1].Should().BeEqualTo((Ascii)'-');
        await grid[3, -1].Should().BeEqualTo((Ascii)'-');
        await grid[5, 5].Should().BeEqualTo((Ascii)'-');
    }

    [Test, MultipleAssertions]
    public async Task IntArray()
    {
        var grid = Grid.Create([
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ]);
        await grid.H.Should().BeEqualTo(4);
        await grid.W.Should().BeEqualTo(3);
        await grid[0, 0].Should().BeEqualTo(1);
        await grid[0, 1].Should().BeEqualTo(2);
        await grid[0, 2].Should().BeEqualTo(3);
        await grid[1, 0].Should().BeEqualTo(4);
        await grid[1, 1].Should().BeEqualTo(5);
        await grid[1, 2].Should().BeEqualTo(6);
        await grid[2, 0].Should().BeEqualTo(7);
        await grid[2, 1].Should().BeEqualTo(8);
        await grid[2, 2].Should().BeEqualTo(9);
        await grid[3, 0].Should().BeEqualTo(10);
        await grid[3, 1].Should().BeEqualTo(11);
        await grid[3, 2].Should().BeEqualTo(12);

        await grid[-1, 0].Should().BeEqualTo(0);
        await grid[4, 0].Should().BeEqualTo(0);
        await grid[0, -1].Should().BeEqualTo(0);
        await grid[0, 3].Should().BeEqualTo(0);
        await grid[-1, -1].Should().BeEqualTo(0);
        await grid[4, 3].Should().BeEqualTo(0);
    }
    [Test, MultipleAssertions]
    public async Task IntArrayDefault()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);
        await grid.H.Should().BeEqualTo(4);
        await grid.W.Should().BeEqualTo(3);
        await grid[0, 0].Should().BeEqualTo(1);
        await grid[0, 1].Should().BeEqualTo(2);
        await grid[0, 2].Should().BeEqualTo(3);
        await grid[1, 0].Should().BeEqualTo(4);
        await grid[1, 1].Should().BeEqualTo(5);
        await grid[1, 2].Should().BeEqualTo(6);
        await grid[2, 0].Should().BeEqualTo(7);
        await grid[2, 1].Should().BeEqualTo(8);
        await grid[2, 2].Should().BeEqualTo(9);
        await grid[3, 0].Should().BeEqualTo(10);
        await grid[3, 1].Should().BeEqualTo(11);
        await grid[3, 2].Should().BeEqualTo(12);

        await grid[-1, 0].Should().BeEqualTo(-1);
        await grid[4, 0].Should().BeEqualTo(-1);
        await grid[0, -1].Should().BeEqualTo(-1);
        await grid[0, 3].Should().BeEqualTo(-1);
        await grid[-1, -1].Should().BeEqualTo(-1);
        await grid[4, 3].Should().BeEqualTo(-1);
    }

    [Test, MultipleAssertions]
    public async Task Index()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);
        for (int h = 0; h < 4; h++)
            for (int w = 0; w < 3; w++)
                await grid.Index(h, w).Should().BeEqualTo(3 * h + w);

        for (int h = 0; h < 4; h++)
        {
            await grid.Index(h, -1).Should().BeEqualTo(-1);
            await grid.Index(h, 3).Should().BeEqualTo(-1);
        }
        for (int w = 0; w < 3; w++)
        {
            await grid.Index(-1, w).Should().BeEqualTo(-1);
            await grid.Index(4, w).Should().BeEqualTo(-1);
        }
    }

    [Test, MultipleAssertions]
    public async Task Moves()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);
        await grid.Moves(0, 0).Select(ToTuples).Should().BeEquivalentOrderTo([(0, 1), (1, 0)]);
        await grid.Moves(0, 1).Select(ToTuples).Should().BeEquivalentOrderTo([(0, 0), (0, 2), (1, 1)]);
        await grid.Moves(0, 2).Select(ToTuples).Should().BeEquivalentOrderTo([(0, 1), (1, 2)]);
        await grid.Moves(1, 0).Select(ToTuples).Should().BeEquivalentOrderTo([(0, 0), (1, 1), (2, 0)]);
        await grid.Moves(1, 1).Select(ToTuples).Should().BeEquivalentOrderTo([(1, 0), (0, 1), (1, 2), (2, 1)]);
        await grid.Moves(1, 2).Select(ToTuples).Should().BeEquivalentOrderTo([(1, 1), (0, 2), (2, 2)]);
        await grid.Moves(2, 0).Select(ToTuples).Should().BeEquivalentOrderTo([(1, 0), (2, 1), (3, 0)]);
        await grid.Moves(2, 1).Select(ToTuples).Should().BeEquivalentOrderTo([(2, 0), (1, 1), (2, 2), (3, 1)]);
        await grid.Moves(2, 2).Select(ToTuples).Should().BeEquivalentOrderTo([(2, 1), (1, 2), (3, 2)]);
        await grid.Moves(3, 0).Select(ToTuples).Should().BeEquivalentOrderTo([(2, 0), (3, 1)]);
        await grid.Moves(3, 1).Select(ToTuples).Should().BeEquivalentOrderTo([(3, 0), (2, 1), (3, 2)]);
        await grid.Moves(3, 2).Select(ToTuples).Should().BeEquivalentOrderTo([(3, 1), (2, 2)]);

        static (int, int) ToTuples(Grid<int>.Position p)
        {
            var (h, w) = p;
            return (h, w);
        }

        await grid.Moves(0).Select(ToInt).Should().BeEquivalentOrderTo([1, 3]);
        await grid.Moves(1).Select(ToInt).Should().BeEquivalentOrderTo([0, 2, 4]);
        await grid.Moves(2).Select(ToInt).Should().BeEquivalentOrderTo([1, 5]);
        await grid.Moves(3).Select(ToInt).Should().BeEquivalentOrderTo([0, 4, 6]);
        await grid.Moves(4).Select(ToInt).Should().BeEquivalentOrderTo([3, 1, 5, 7]);
        await grid.Moves(5).Select(ToInt).Should().BeEquivalentOrderTo([4, 2, 8]);
        await grid.Moves(6).Select(ToInt).Should().BeEquivalentOrderTo([3, 7, 9]);
        await grid.Moves(7).Select(ToInt).Should().BeEquivalentOrderTo([6, 4, 8, 10]);
        await grid.Moves(8).Select(ToInt).Should().BeEquivalentOrderTo([7, 5, 11]);
        await grid.Moves(9).Select(ToInt).Should().BeEquivalentOrderTo([6, 10]);
        await grid.Moves(10).Select(ToInt).Should().BeEquivalentOrderTo([9, 7, 11]);
        await grid.Moves(11).Select(ToInt).Should().BeEquivalentOrderTo([10, 8]);

        static int ToInt(Grid<int>.Position p)
        {
            return (int)p;
        }

        Memory<int> q = new int[] { 3, 1, 5, 7 };
        foreach (int ix in grid.Moves(1, 1))
        {
            await ix.Should().BeEqualTo(q.Span[0]);
            q = q[1..];
        }

        Memory<(int, int)> r = new[] { (1, 0), (0, 1), (1, 2), (2, 1) };
        foreach (var (h, w) in grid.Moves(4))
        {
            await (h, w).Should().BeEqualTo(r.Span[0]);
            r = r[1..];
        }
    }

    [Test, MultipleAssertions]
    public async Task Clone()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);
        var clone = grid.Clone();

        await grid.data.Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        await clone.data.Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        await grid[-1].Should().BeEqualTo(-1);
        await clone[-1].Should().BeEqualTo(-1);

        grid[0, 0] = 100;
        grid[1, 0] = 200;
        await grid.data.Should().BeEquivalentOrderTo([100, 2, 3, 200, 5, 6, 7, 8, 9, 10, 11, 12]);
        await clone.data.Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        await grid[-1].Should().BeEqualTo(-1);
        await clone[-1].Should().BeEqualTo(-1);
    }

    [Test, MultipleAssertions]
    public async Task Rotate90()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var rot = grid.Rotate90();
        await rot.data.Should().BeEquivalentOrderTo(Grid.Create([
            [10, 7, 4, 1],
            [11, 8, 5, 2],
            [12, 9, 6, 3],
        ]).data);
        await rot.defaultValue.Should().BeEqualTo(grid.defaultValue);
        await grid.data.Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }


    [Test, MultipleAssertions]
    public async Task Rotate180()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var rot = grid.Rotate180();
        await rot.data.Should().BeEquivalentOrderTo(Grid.Create([
            [12, 11, 10],
            [9, 8, 7],
            [6, 5, 4],
            [3, 2, 1],
        ]).data);
        await rot.defaultValue.Should().BeEqualTo(grid.defaultValue);
        await grid.data.Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }

    [Test, MultipleAssertions]
    public async Task Rotate270()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var rot = grid.Rotate270();
        await rot.data.Should().BeEquivalentOrderTo(Grid.Create([
            [3, 6, 9, 12],
            [2, 5, 8, 11],
            [1, 4, 7, 10],
        ]).data);
        await rot.defaultValue.Should().BeEqualTo(grid.defaultValue);
        await grid.data.Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }

    [Test, MultipleAssertions]
    public async Task Transpose()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var tr = grid.Transpose();
        await tr.data.Should().BeEquivalentOrderTo(Grid.Create([
            [1, 4, 7, 10],
            [2, 5, 8, 11],
            [3, 6, 9, 12],
        ]).data);
        await tr.defaultValue.Should().BeEqualTo(grid.defaultValue);
        await grid.data.Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }

    [Test, MultipleAssertions]
    public async Task Foreach()
    {
        var grid = Grid.Create([
            "123",
            "456",
        ], '-');
        var lst = new List<(char, int, int)>();
        foreach (var tuple in grid)
            lst.Add(tuple);

        await lst.Should().BeEquivalentOrderTo(
        [
            ('1', 0, 0),
            ('2', 0, 1),
            ('3', 0, 2),
            ('4', 1, 0),
            ('5', 1, 1),
            ('6', 1, 2),
        ]);
    }
}