using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Extension
{
    public class MyLinqExtensionTests
    {
        [Fact]
        public void MaxBy()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.MaxBy().Should().Be((17, 3401434));
        }

        [Fact]
        public void MaxByArrayFunc()
        {
            new (int, long)[] {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MaxBy(tup => tup.Item2).Should().Be((17, (0, 3401434)));
        }

        [Fact]
        public void MaxByFunc()
        {
            new List<(int, long)> {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MaxBy(tup => tup.Item2).Should().Be(((0, 3401434), 3401434));
        }


        [Fact]
        public void MinBy()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.MinBy().Should().Be((3, 4));
        }

        [Fact]
        public void MinByArrayFunc()
        {
            new (int, long)[] {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MinBy(tup => tup.Item2).Should().Be((3, (0, 4)));
        }

        [Fact]
        public void MinByFunc()
        {
            new List<(int, long)> {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MinBy(tup => tup.Item2).Should().Be(((0, 4), 4));
        }

        [Fact]
        public void GroupCount()
        {
            new[] {
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
            }.GroupCount().Should().BeEquivalentTo(new Dictionary<StringComparison, int>
            {
                { StringComparison.Ordinal,1 },
                { StringComparison.OrdinalIgnoreCase,4 },
                { StringComparison.CurrentCulture,3 },
                { StringComparison.InvariantCulture,2 },
                { StringComparison.InvariantCultureIgnoreCase,1 },
            });
        }

        [Fact]
        public void GroupCountFunc()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.GroupCount(i => i % 7).Should().BeEquivalentTo(new Dictionary<long, int>
            {
                { 0, 4 },
                { 1, 7 },
                { 2, 1 },
                { 3, 3 },
                { 4, 2 },
                { 5, 2 },
                { 6, 2 },
            });
        }


        [Fact]
        public void Flatten()
        {
            new[] { "abc", "def", "012", "345", "678" }.Flatten()
                .Should().Equal(
                new char[] { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8' });

            new[] {
                new[] { 1, 2, 3 },
                new[] { -1, -2, -3 },
                new[] { 4, 5, 6 },
                new[] { -6, -5, -4 },
                new[] { 7, 8, 9 },
            }.Flatten()
                .Should().Equal(
                new[] { 1, 2, 3, -1, -2, -3, 4, 5, 6, -6, -5, -4, 7, 8, 9 });
        }

        [Fact]
        public void MaxMin()
        {
            new int[] {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MaxMin()
            .Should().Be((81, -75));
            new List<int> {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MaxMin()
            .Should().Be((81, -75));
            ((Span<int>)new int[] {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }).MaxMin()
            .Should().Be((81, -75));
            ((ReadOnlySpan<int>)new int[] {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }).MaxMin()
            .Should().Be((81, -75));

            Array.Empty<int>().MaxMin().Should().Be((0, 0));
            Array.Empty<int>().AsSpan().MaxMin().Should().Be((0, 0));
        }

        [Fact]
        public void SpanSelect()
        {
            var span = Enumerable.Range(0, 10).ToArray().AsSpan();
            static int Func(int n) => 2 * n;
            span.Select(Func).ToArray().Should().Equal(Enumerable.Range(0, 10).Select(Func));
            ((ReadOnlySpan<int>)span).Select(Func).ToArray().Should().Equal(Enumerable.Range(0, 10).Select(Func));

            static int FuncIndex(int n, int i) => i * n;
            span.Select(FuncIndex).ToArray().Should().Equal(Enumerable.Range(0, 10).Select(FuncIndex));
            ((ReadOnlySpan<int>)span).Select(FuncIndex).ToArray().Should().Equal(Enumerable.Range(0, 10).Select(FuncIndex));
        }

        public static TheoryData Buffered_Data = new TheoryData<IEnumerable<int>, int, int[][]>
        {
            {
                Enumerable.Range(0, 12),
                4,
                new int[][]
                {
                    new int[]{ 0, 1, 2, 3, },
                    new int[]{ 4, 5, 6, 7, },
                    new int[]{ 8, 9, 10, 11, },
                }
            },
            {
                Enumerable.Range(0, 12),
                5,
                new int[][]
                {
                    new int[]{ 0, 1, 2, 3, 4, },
                    new int[]{ 5, 6, 7, 8, 9, },
                    new int[]{ 10, 11, },
                }
            },
            {
                Enumerable.Empty<int>(),
                5,
                Array.Empty<int[]>()
            },
        };
        [Theory]
        [MemberData(nameof(Buffered_Data))]
        public void Buffered(IEnumerable<int> input, int bufferSize, int[][] expected)
        {
            var result = input.Buffered(bufferSize).ToArray();
            result.Should().HaveSameCount(expected);
            for (int i = 0; i < expected.Length; i++)
                result[i].Should().Equal(expected[i]);
        }
    }
}