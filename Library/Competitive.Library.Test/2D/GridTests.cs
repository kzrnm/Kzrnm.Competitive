using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.TwoDimensional
{
    public class GridTests
    {
        [Fact]
        public void String()
        {
            var grid = Grid.Create(
            [
                "...#.",
                ".#.#.",
                ".#..."
            ]);
            grid.H.Should().Be(3);
            grid.W.Should().Be(5);
            grid[0, 0].Should().Be('.');
            grid[0, 1].Should().Be('.');
            grid[0, 2].Should().Be('.');
            grid[0, 3].Should().Be('#');
            grid[0, 4].Should().Be('.');
            grid[1, 0].Should().Be('.');
            grid[1, 1].Should().Be('#');
            grid[1, 2].Should().Be('.');
            grid[1, 3].Should().Be('#');
            grid[1, 4].Should().Be('.');
            grid[2, 0].Should().Be('.');
            grid[2, 1].Should().Be('#');
            grid[2, 2].Should().Be('.');
            grid[2, 3].Should().Be('.');
            grid[2, 4].Should().Be('.');
            grid[-1, 0].Should().Be('\0');
            grid[3, 0].Should().Be('\0');
            grid[-1, -1].Should().Be('\0');
            grid[-1, 5].Should().Be('\0');
            grid[0, 5].Should().Be('\0');
            grid[3, 5].Should().Be('\0');
            grid[0, -1].Should().Be('\0');
            grid[3, -1].Should().Be('\0');
            grid[5, 5].Should().Be('\0');
        }
        [Fact]
        public void StringDefault()
        {
            var grid = Grid.Create(new[]
            {
                "...#.",
                ".#.#.",
                ".#..."
            }, '-');
            grid.H.Should().Be(3);
            grid.W.Should().Be(5);
            grid[0, 0].Should().Be('.');
            grid[0, 1].Should().Be('.');
            grid[0, 2].Should().Be('.');
            grid[0, 3].Should().Be('#');
            grid[0, 4].Should().Be('.');
            grid[1, 0].Should().Be('.');
            grid[1, 1].Should().Be('#');
            grid[1, 2].Should().Be('.');
            grid[1, 3].Should().Be('#');
            grid[1, 4].Should().Be('.');
            grid[2, 0].Should().Be('.');
            grid[2, 1].Should().Be('#');
            grid[2, 2].Should().Be('.');
            grid[2, 3].Should().Be('.');
            grid[2, 4].Should().Be('.');
            grid[-1, 0].Should().Be('-');
            grid[3, 0].Should().Be('-');
            grid[-1, -1].Should().Be('-');
            grid[-1, 5].Should().Be('-');
            grid[0, 5].Should().Be('-');
            grid[3, 5].Should().Be('-');
            grid[0, -1].Should().Be('-');
            grid[3, -1].Should().Be('-');
            grid[5, 5].Should().Be('-');
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
            grid.H.Should().Be(4);
            grid.W.Should().Be(3);
            grid[0, 0].Should().Be(1);
            grid[0, 1].Should().Be(2);
            grid[0, 2].Should().Be(3);
            grid[1, 0].Should().Be(4);
            grid[1, 1].Should().Be(5);
            grid[1, 2].Should().Be(6);
            grid[2, 0].Should().Be(7);
            grid[2, 1].Should().Be(8);
            grid[2, 2].Should().Be(9);
            grid[3, 0].Should().Be(10);
            grid[3, 1].Should().Be(11);
            grid[3, 2].Should().Be(12);

            grid[-1, 0].Should().Be(0);
            grid[4, 0].Should().Be(0);
            grid[0, -1].Should().Be(0);
            grid[0, 3].Should().Be(0);
            grid[-1, -1].Should().Be(0);
            grid[4, 3].Should().Be(0);
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
            grid.H.Should().Be(4);
            grid.W.Should().Be(3);
            grid[0, 0].Should().Be(1);
            grid[0, 1].Should().Be(2);
            grid[0, 2].Should().Be(3);
            grid[1, 0].Should().Be(4);
            grid[1, 1].Should().Be(5);
            grid[1, 2].Should().Be(6);
            grid[2, 0].Should().Be(7);
            grid[2, 1].Should().Be(8);
            grid[2, 2].Should().Be(9);
            grid[3, 0].Should().Be(10);
            grid[3, 1].Should().Be(11);
            grid[3, 2].Should().Be(12);

            grid[-1, 0].Should().Be(-1);
            grid[4, 0].Should().Be(-1);
            grid[0, -1].Should().Be(-1);
            grid[0, 3].Should().Be(-1);
            grid[-1, -1].Should().Be(-1);
            grid[4, 3].Should().Be(-1);
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
                    grid.Index(h, w).Should().Be(3 * h + w);

            for (int h = 0; h < 4; h++)
            {
                grid.Index(h, -1).Should().Be(-1);
                grid.Index(h, 3).Should().Be(-1);
            }
            for (int w = 0; w < 3; w++)
            {
                grid.Index(-1, w).Should().Be(-1);
                grid.Index(4, w).Should().Be(-1);
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
            grid.Moves(0, 0).Select(ToTuples).Should().Equal((0, 1), (1, 0));
            grid.Moves(0, 1).Select(ToTuples).Should().Equal((0, 0), (0, 2), (1, 1));
            grid.Moves(0, 2).Select(ToTuples).Should().Equal((0, 1), (1, 2));
            grid.Moves(1, 0).Select(ToTuples).Should().Equal((0, 0), (1, 1), (2, 0));
            grid.Moves(1, 1).Select(ToTuples).Should().Equal((1, 0), (0, 1), (1, 2), (2, 1));
            grid.Moves(1, 2).Select(ToTuples).Should().Equal((1, 1), (0, 2), (2, 2));
            grid.Moves(2, 0).Select(ToTuples).Should().Equal((1, 0), (2, 1), (3, 0));
            grid.Moves(2, 1).Select(ToTuples).Should().Equal((2, 0), (1, 1), (2, 2), (3, 1));
            grid.Moves(2, 2).Select(ToTuples).Should().Equal((2, 1), (1, 2), (3, 2));
            grid.Moves(3, 0).Select(ToTuples).Should().Equal((2, 0), (3, 1));
            grid.Moves(3, 1).Select(ToTuples).Should().Equal((3, 0), (2, 1), (3, 2));
            grid.Moves(3, 2).Select(ToTuples).Should().Equal((3, 1), (2, 2));

            static (int, int) ToTuples(Grid<int>.Position p)
            {
                var (h, w) = p;
                return (h, w);
            }

            grid.Moves(0).Select(ToInt).Should().Equal(1, 3);
            grid.Moves(1).Select(ToInt).Should().Equal(0, 2, 4);
            grid.Moves(2).Select(ToInt).Should().Equal(1, 5);
            grid.Moves(3).Select(ToInt).Should().Equal(0, 4, 6);
            grid.Moves(4).Select(ToInt).Should().Equal(3, 1, 5, 7);
            grid.Moves(5).Select(ToInt).Should().Equal(4, 2, 8);
            grid.Moves(6).Select(ToInt).Should().Equal(3, 7, 9);
            grid.Moves(7).Select(ToInt).Should().Equal(6, 4, 8, 10);
            grid.Moves(8).Select(ToInt).Should().Equal(7, 5, 11);
            grid.Moves(9).Select(ToInt).Should().Equal(6, 10);
            grid.Moves(10).Select(ToInt).Should().Equal(9, 7, 11);
            grid.Moves(11).Select(ToInt).Should().Equal(10, 8);

            static int ToInt(Grid<int>.Position p)
            {
                return (int)p;
            }

            var q = new int[] { 3, 1, 5, 7 }.AsSpan();
            foreach (int ix in grid.Moves(1, 1))
            {
                ix.Should().Be(q[0]);
                q = q[1..];
            }

            var r = new (int, int)[] { (1, 0), (0, 1), (1, 2), (2, 1) }.AsSpan();
            foreach (var (h, w) in grid.Moves(4))
            {
                (h, w).Should().Be(r[0]);
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

            grid.data.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            clone.data.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            grid[-1].Should().Be(-1);
            clone[-1].Should().Be(-1);

            grid[0, 0] = 100;
            grid[1, 0] = 200;
            grid.data.Should().Equal(100, 2, 3, 200, 5, 6, 7, 8, 9, 10, 11, 12);
            clone.data.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            grid[-1].Should().Be(-1);
            clone[-1].Should().Be(-1);
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
            rot.data.Should().Equal(Grid.Create([
                [10, 7, 4, 1],
                [11, 8, 5, 2],
                [12, 9, 6, 3],
            ]).data);
            rot.defaultValue.Should().Be(grid.defaultValue);
            grid.data.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
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
            rot.data.Should().Equal(Grid.Create([
                [12, 11, 10],
                [9, 8, 7],
                [6, 5, 4],
                [3, 2, 1],
            ]).data);
            rot.defaultValue.Should().Be(grid.defaultValue);
            grid.data.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
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
            rot.data.Should().Equal(Grid.Create([
                [3, 6, 9, 12],
                [2, 5, 8, 11],
                [1, 4, 7, 10],
            ]).data);
            rot.defaultValue.Should().Be(grid.defaultValue);
            grid.data.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
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
            tr.data.Should().Equal(Grid.Create([
                [1, 4, 7, 10],
                [2, 5, 8, 11],
                [3, 6, 9, 12],
            ]).data);
            tr.defaultValue.Should().Be(grid.defaultValue);
            grid.data.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        }

        [Fact]
        public void Foreach()
        {
            var grid = Grid.Create(new[]
            {
                "123",
                "456",
            }, '-');
            var lst = new List<(char, int, int)>();
            foreach (var tuple in grid)
                lst.Add(tuple);

            lst.Should().Equal(
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
}
