using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.TwoDimensional;

public class GridTests
{
    [Fact]
    public void String()
    {
        var grid = Grid.Create(
        [
            "...#.",
            ".#.#.",
            ".#...",
        ]);
        grid.H.ShouldBe(3);
        grid.W.ShouldBe(5);
        grid[0, 0].ShouldBe('.');
        grid[0, 1].ShouldBe('.');
        grid[0, 2].ShouldBe('.');
        grid[0, 3].ShouldBe('#');
        grid[0, 4].ShouldBe('.');
        grid[1, 0].ShouldBe('.');
        grid[1, 1].ShouldBe('#');
        grid[1, 2].ShouldBe('.');
        grid[1, 3].ShouldBe('#');
        grid[1, 4].ShouldBe('.');
        grid[2, 0].ShouldBe('.');
        grid[2, 1].ShouldBe('#');
        grid[2, 2].ShouldBe('.');
        grid[2, 3].ShouldBe('.');
        grid[2, 4].ShouldBe('.');
        grid[-1, 0].ShouldBe('\0');
        grid[3, 0].ShouldBe('\0');
        grid[-1, -1].ShouldBe('\0');
        grid[-1, 5].ShouldBe('\0');
        grid[0, 5].ShouldBe('\0');
        grid[3, 5].ShouldBe('\0');
        grid[0, -1].ShouldBe('\0');
        grid[3, -1].ShouldBe('\0');
        grid[5, 5].ShouldBe('\0');
    }
    [Fact]
    public void StringDefault()
    {
        var grid = Grid.Create([
            "...#.",
            ".#.#.",
            ".#...",
        ], '-');
        grid.H.ShouldBe(3);
        grid.W.ShouldBe(5);
        grid[0, 0].ShouldBe('.');
        grid[0, 1].ShouldBe('.');
        grid[0, 2].ShouldBe('.');
        grid[0, 3].ShouldBe('#');
        grid[0, 4].ShouldBe('.');
        grid[1, 0].ShouldBe('.');
        grid[1, 1].ShouldBe('#');
        grid[1, 2].ShouldBe('.');
        grid[1, 3].ShouldBe('#');
        grid[1, 4].ShouldBe('.');
        grid[2, 0].ShouldBe('.');
        grid[2, 1].ShouldBe('#');
        grid[2, 2].ShouldBe('.');
        grid[2, 3].ShouldBe('.');
        grid[2, 4].ShouldBe('.');
        grid[-1, 0].ShouldBe('-');
        grid[3, 0].ShouldBe('-');
        grid[-1, -1].ShouldBe('-');
        grid[-1, 5].ShouldBe('-');
        grid[0, 5].ShouldBe('-');
        grid[3, 5].ShouldBe('-');
        grid[0, -1].ShouldBe('-');
        grid[3, -1].ShouldBe('-');
        grid[5, 5].ShouldBe('-');
    }

    [Fact]
    public void Ascii()
    {
        var grid = Grid.Create(
        [
            new Asciis("...#."u8.ToArray()),
            new Asciis(".#.#."u8.ToArray()),
            new Asciis(".#..."u8.ToArray()),
        ]);
        grid.H.ShouldBe(3);
        grid.W.ShouldBe(5);
        grid[0, 0].ShouldBe((Ascii)'.');
        grid[0, 1].ShouldBe((Ascii)'.');
        grid[0, 2].ShouldBe((Ascii)'.');
        grid[0, 3].ShouldBe((Ascii)'#');
        grid[0, 4].ShouldBe((Ascii)'.');
        grid[1, 0].ShouldBe((Ascii)'.');
        grid[1, 1].ShouldBe((Ascii)'#');
        grid[1, 2].ShouldBe((Ascii)'.');
        grid[1, 3].ShouldBe((Ascii)'#');
        grid[1, 4].ShouldBe((Ascii)'.');
        grid[2, 0].ShouldBe((Ascii)'.');
        grid[2, 1].ShouldBe((Ascii)'#');
        grid[2, 2].ShouldBe((Ascii)'.');
        grid[2, 3].ShouldBe((Ascii)'.');
        grid[2, 4].ShouldBe((Ascii)'.');
        grid[-1, 0].ShouldBe((Ascii)'\0');
        grid[3, 0].ShouldBe((Ascii)'\0');
        grid[-1, -1].ShouldBe((Ascii)'\0');
        grid[-1, 5].ShouldBe((Ascii)'\0');
        grid[0, 5].ShouldBe((Ascii)'\0');
        grid[3, 5].ShouldBe((Ascii)'\0');
        grid[0, -1].ShouldBe((Ascii)'\0');
        grid[3, -1].ShouldBe((Ascii)'\0');
        grid[5, 5].ShouldBe((Ascii)'\0');
        grid.ToString().ShouldBe("""
...#.
.#.#.
.#...
""");
    }
    [Fact]
    public void AsciiDefault()
    {
        var grid = Grid.Create([
            new Asciis("...#."u8.ToArray()),
            new Asciis(".#.#."u8.ToArray()),
            new Asciis(".#..."u8.ToArray()),
        ], '-');
        grid.H.ShouldBe((Ascii)3);
        grid.W.ShouldBe((Ascii)5);
        grid[0, 0].ShouldBe((Ascii)'.');
        grid[0, 1].ShouldBe((Ascii)'.');
        grid[0, 2].ShouldBe((Ascii)'.');
        grid[0, 3].ShouldBe((Ascii)'#');
        grid[0, 4].ShouldBe((Ascii)'.');
        grid[1, 0].ShouldBe((Ascii)'.');
        grid[1, 1].ShouldBe((Ascii)'#');
        grid[1, 2].ShouldBe((Ascii)'.');
        grid[1, 3].ShouldBe((Ascii)'#');
        grid[1, 4].ShouldBe((Ascii)'.');
        grid[2, 0].ShouldBe((Ascii)'.');
        grid[2, 1].ShouldBe((Ascii)'#');
        grid[2, 2].ShouldBe((Ascii)'.');
        grid[2, 3].ShouldBe((Ascii)'.');
        grid[2, 4].ShouldBe((Ascii)'.');
        grid[-1, 0].ShouldBe((Ascii)'-');
        grid[3, 0].ShouldBe((Ascii)'-');
        grid[-1, -1].ShouldBe((Ascii)'-');
        grid[-1, 5].ShouldBe((Ascii)'-');
        grid[0, 5].ShouldBe((Ascii)'-');
        grid[3, 5].ShouldBe((Ascii)'-');
        grid[0, -1].ShouldBe((Ascii)'-');
        grid[3, -1].ShouldBe((Ascii)'-');
        grid[5, 5].ShouldBe((Ascii)'-');
    }

    [Fact]
    public void IntArray()
    {
        var grid = Grid.Create([
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ]);
        grid.H.ShouldBe(4);
        grid.W.ShouldBe(3);
        grid[0, 0].ShouldBe(1);
        grid[0, 1].ShouldBe(2);
        grid[0, 2].ShouldBe(3);
        grid[1, 0].ShouldBe(4);
        grid[1, 1].ShouldBe(5);
        grid[1, 2].ShouldBe(6);
        grid[2, 0].ShouldBe(7);
        grid[2, 1].ShouldBe(8);
        grid[2, 2].ShouldBe(9);
        grid[3, 0].ShouldBe(10);
        grid[3, 1].ShouldBe(11);
        grid[3, 2].ShouldBe(12);

        grid[-1, 0].ShouldBe(0);
        grid[4, 0].ShouldBe(0);
        grid[0, -1].ShouldBe(0);
        grid[0, 3].ShouldBe(0);
        grid[-1, -1].ShouldBe(0);
        grid[4, 3].ShouldBe(0);
    }
    [Fact]
    public void IntArrayDefault()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);
        grid.H.ShouldBe(4);
        grid.W.ShouldBe(3);
        grid[0, 0].ShouldBe(1);
        grid[0, 1].ShouldBe(2);
        grid[0, 2].ShouldBe(3);
        grid[1, 0].ShouldBe(4);
        grid[1, 1].ShouldBe(5);
        grid[1, 2].ShouldBe(6);
        grid[2, 0].ShouldBe(7);
        grid[2, 1].ShouldBe(8);
        grid[2, 2].ShouldBe(9);
        grid[3, 0].ShouldBe(10);
        grid[3, 1].ShouldBe(11);
        grid[3, 2].ShouldBe(12);

        grid[-1, 0].ShouldBe(-1);
        grid[4, 0].ShouldBe(-1);
        grid[0, -1].ShouldBe(-1);
        grid[0, 3].ShouldBe(-1);
        grid[-1, -1].ShouldBe(-1);
        grid[4, 3].ShouldBe(-1);
    }

    [Fact]
    public void Index()
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
                grid.Index(h, w).ShouldBe(3 * h + w);

        for (int h = 0; h < 4; h++)
        {
            grid.Index(h, -1).ShouldBe(-1);
            grid.Index(h, 3).ShouldBe(-1);
        }
        for (int w = 0; w < 3; w++)
        {
            grid.Index(-1, w).ShouldBe(-1);
            grid.Index(4, w).ShouldBe(-1);
        }
    }

    [Fact]
    public void Moves()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);
        grid.Moves(0, 0).Select(ToTuples).ShouldBe([(0, 1), (1, 0)]);
        grid.Moves(0, 1).Select(ToTuples).ShouldBe([(0, 0), (0, 2), (1, 1)]);
        grid.Moves(0, 2).Select(ToTuples).ShouldBe([(0, 1), (1, 2)]);
        grid.Moves(1, 0).Select(ToTuples).ShouldBe([(0, 0), (1, 1), (2, 0)]);
        grid.Moves(1, 1).Select(ToTuples).ShouldBe([(1, 0), (0, 1), (1, 2), (2, 1)]);
        grid.Moves(1, 2).Select(ToTuples).ShouldBe([(1, 1), (0, 2), (2, 2)]);
        grid.Moves(2, 0).Select(ToTuples).ShouldBe([(1, 0), (2, 1), (3, 0)]);
        grid.Moves(2, 1).Select(ToTuples).ShouldBe([(2, 0), (1, 1), (2, 2), (3, 1)]);
        grid.Moves(2, 2).Select(ToTuples).ShouldBe([(2, 1), (1, 2), (3, 2)]);
        grid.Moves(3, 0).Select(ToTuples).ShouldBe([(2, 0), (3, 1)]);
        grid.Moves(3, 1).Select(ToTuples).ShouldBe([(3, 0), (2, 1), (3, 2)]);
        grid.Moves(3, 2).Select(ToTuples).ShouldBe([(3, 1), (2, 2)]);

        static (int, int) ToTuples(Grid<int>.Position p)
        {
            var (h, w) = p;
            return (h, w);
        }

        grid.Moves(0).Select(ToInt).ShouldBe([1, 3]);
        grid.Moves(1).Select(ToInt).ShouldBe([0, 2, 4]);
        grid.Moves(2).Select(ToInt).ShouldBe([1, 5]);
        grid.Moves(3).Select(ToInt).ShouldBe([0, 4, 6]);
        grid.Moves(4).Select(ToInt).ShouldBe([3, 1, 5, 7]);
        grid.Moves(5).Select(ToInt).ShouldBe([4, 2, 8]);
        grid.Moves(6).Select(ToInt).ShouldBe([3, 7, 9]);
        grid.Moves(7).Select(ToInt).ShouldBe([6, 4, 8, 10]);
        grid.Moves(8).Select(ToInt).ShouldBe([7, 5, 11]);
        grid.Moves(9).Select(ToInt).ShouldBe([6, 10]);
        grid.Moves(10).Select(ToInt).ShouldBe([9, 7, 11]);
        grid.Moves(11).Select(ToInt).ShouldBe([10, 8]);

        static int ToInt(Grid<int>.Position p)
        {
            return (int)p;
        }

        Span<int> q = [3, 1, 5, 7];
        foreach (int ix in grid.Moves(1, 1))
        {
            ix.ShouldBe(q[0]);
            q = q[1..];
        }

        Span<(int, int)> r = [(1, 0), (0, 1), (1, 2), (2, 1)];
        foreach (var (h, w) in grid.Moves(4))
        {
            (h, w).ShouldBe(r[0]);
            r = r[1..];
        }
    }

    [Fact]
    public void Clone()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);
        var clone = grid.Clone();

        grid.data.ShouldBe([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        clone.data.ShouldBe([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        grid[-1].ShouldBe(-1);
        clone[-1].ShouldBe(-1);

        grid[0, 0] = 100;
        grid[1, 0] = 200;
        grid.data.ShouldBe([100, 2, 3, 200, 5, 6, 7, 8, 9, 10, 11, 12]);
        clone.data.ShouldBe([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        grid[-1].ShouldBe(-1);
        clone[-1].ShouldBe(-1);
    }

    [Fact]
    public void Rotate90()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var rot = grid.Rotate90();
        rot.data.ShouldBe(Grid.Create([
            [10, 7, 4, 1],
            [11, 8, 5, 2],
            [12, 9, 6, 3],
        ]).data);
        rot.defaultValue.ShouldBe(grid.defaultValue);
        grid.data.ShouldBe([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }


    [Fact]
    public void Rotate180()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var rot = grid.Rotate180();
        rot.data.ShouldBe(Grid.Create([
            [12, 11, 10],
            [9, 8, 7],
            [6, 5, 4],
            [3, 2, 1],
        ]).data);
        rot.defaultValue.ShouldBe(grid.defaultValue);
        grid.data.ShouldBe([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }

    [Fact]
    public void Rotate270()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var rot = grid.Rotate270();
        rot.data.ShouldBe(Grid.Create([
            [3, 6, 9, 12],
            [2, 5, 8, 11],
            [1, 4, 7, 10],
        ]).data);
        rot.defaultValue.ShouldBe(grid.defaultValue);
        grid.data.ShouldBe([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }

    [Fact]
    public void Transpose()
    {
        var grid = Grid.Create(
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [10, 11, 12],
        ], -1);

        var tr = grid.Transpose();
        tr.data.ShouldBe(Grid.Create([
            [1, 4, 7, 10],
            [2, 5, 8, 11],
            [3, 6, 9, 12],
        ]).data);
        tr.defaultValue.ShouldBe(grid.defaultValue);
        grid.data.ShouldBe([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
    }

    [Fact]
    public void Foreach()
    {
        var grid = Grid.Create([
            "123",
            "456",
        ], '-');
        var lst = new List<(char, int, int)>();
        foreach (var tuple in grid)
            lst.Add(tuple);

        lst.ShouldBe(
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
