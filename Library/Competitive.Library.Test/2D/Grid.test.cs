using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.TwoDimensional
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class GridTests
    {
        [Fact]
        public void String()
        {
            var grid = Grid.Create(new[]
            {
                "...#.",
                ".#.#.",
                ".#..."
            });
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
            var grid = Grid.Create(new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            });
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
            var grid = Grid.Create(new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            }, -1);
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
        public void IntSpan()
        {
            var grid = Grid.Create(new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            }.AsSpan());
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
        public void IntSpanDefault()
        {
            var grid = Grid.Create(new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            }.AsSpan(), -1);
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
        public void IntReadOnlySpan()
        {
            var grid = Grid.Create((ReadOnlySpan<int[]>)new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            });
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
        public void IntReadOnlySpanDefault()
        {
            var grid = Grid.Create((ReadOnlySpan<int[]>)new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            }, -1);
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
            var grid = Grid.Create((ReadOnlySpan<int[]>)new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            }, -1);
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
            var grid = Grid.Create((ReadOnlySpan<int[]>)new[]
            {
                new[]{ 1,2,3 },
                new[]{ 4,5,6 },
                new[]{ 7,8,9 },
                new[]{ 10,11,12 },
            }, -1);
            grid.Moves(0, 0).Should().Equal((0, 1), (1, 0));
            grid.Moves(0, 1).Should().Equal((0, 0), (0, 2), (1, 1));
            grid.Moves(0, 2).Should().Equal((0, 1), (1, 2));
            grid.Moves(1, 0).Should().Equal((0, 0), (1, 1), (2, 0));
            grid.Moves(1, 1).Should().Equal((1, 0), (0, 1), (1, 2), (2, 1));
            grid.Moves(1, 2).Should().Equal((1, 1), (0, 2), (2, 2));
            grid.Moves(2, 0).Should().Equal((1, 0), (2, 1), (3, 0));
            grid.Moves(2, 1).Should().Equal((2, 0), (1, 1), (2, 2), (3, 1));
            grid.Moves(2, 2).Should().Equal((2, 1), (1, 2), (3, 2));
            grid.Moves(3, 0).Should().Equal((2, 0), (3, 1));
            grid.Moves(3, 1).Should().Equal((3, 0), (2, 1), (3, 2));
            grid.Moves(3, 2).Should().Equal((3, 1), (2, 2));
        }
    }
}
