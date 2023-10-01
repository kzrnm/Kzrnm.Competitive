using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class CollectionExtensionTests
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
        public void GroupIndex()
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

            grouped.Should().HaveCount(5);

            grouped[0].Key.Should().Be(StringComparison.OrdinalIgnoreCase);
            grouped[0].Should().Equal(new int[] { 0, 1, 7, 9 });

            grouped[1].Key.Should().Be(StringComparison.InvariantCulture);
            grouped[1].Should().Equal(new int[] { 2, 10 });

            grouped[2].Key.Should().Be(StringComparison.InvariantCultureIgnoreCase);
            grouped[2].Should().Equal(new int[] { 3 });

            grouped[3].Key.Should().Be(StringComparison.CurrentCulture);
            grouped[3].Should().Equal(new int[] { 4, 5, 6 });

            grouped[4].Key.Should().Be(StringComparison.Ordinal);
            grouped[4].Should().Equal(new int[] { 8 });
        }

        [Fact]
        public void GroupIndexFunc()
        {
            var grouped = new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.GroupIndex(i => i % 7).ToArray();

            grouped.Should().HaveCount(7);

            grouped[0].Key.Should().Be(1);
            grouped[0].Should().Equal(new int[] { 0, 2, 6, 9, 17, 18, 20 });

            grouped[1].Key.Should().Be(3);
            grouped[1].Should().Equal(new int[] { 1, 11, 14 });

            grouped[2].Key.Should().Be(4);
            grouped[2].Should().Equal(new int[] { 3, 10 });

            grouped[3].Key.Should().Be(0);
            grouped[3].Should().Equal(new int[] { 4, 5, 7, 16 });

            grouped[4].Key.Should().Be(6);
            grouped[4].Should().Equal(new int[] { 8, 19 });

            grouped[5].Key.Should().Be(5);
            grouped[5].Should().Equal(new int[] { 12, 15 });

            grouped[6].Key.Should().Be(2);
            grouped[6].Should().Equal(new int[] { 13 });
        }


        [Fact]
        public void Flatten()
        {
            new[] { "abc", "def", "012", "345", "678" }.Flatten()
                .Should().Equal(
                new char[] { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8' });

            var arr = new[] {
                new[] { 1, 2, 3 },
                new[] { -1, -2, -3 },
                new[] { 4, 5, 6 },
                new[] { -6, -5, -4 },
                new[] { 7, 8, 9 },
            };
            var expected = new[] { 1, 2, 3, -1, -2, -3, 4, 5, 6, -6, -5, -4, 7, 8, 9 };

            arr.Flatten().Should().Equal(expected);
            arr.ToList().Flatten().Should().Equal(expected);
        }

        [Fact]
        public void MinMax()
        {
            new int[] {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MinMax()
            .Should().Be((-75, 81));
            new List<int> {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MinMax()
            .Should().Be((-75, 81));
            ((Span<int>)new int[] {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }).MinMax()
            .Should().Be((-75, 81));
            ((ReadOnlySpan<int>)new int[] {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }).MinMax()
            .Should().Be((-75, 81));

            Array.Empty<int>().MinMax().Should().Be((0, 0));
            Array.Empty<int>().AsSpan().MinMax().Should().Be((0, 0));
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

        public static TheoryData Chunk_Data => new TheoryData<IEnumerable<int>, int, int[][]>
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
        [MemberData(nameof(Chunk_Data))]
        public void Chunk(IEnumerable<int> input, int bufferSize, int[][] expected)
        {
            var result = input.Chunk(bufferSize).ToArray();
            result.Should().HaveSameCount(expected);
            for (int i = 0; i < expected.Length; i++)
                result[i].Should().Equal(expected[i]);
        }

        public static TheoryData Tupled2_Data => new TheoryData<int[], (int, int)[]>
        {
            {
                new int[]{ 1,2,3,4,5,6 },
                new (int,int)[]{ (1,2),(2,3),(3,4),(4,5),(5,6) }
            },
            {
                new int[]{ 1 },
                Array.Empty<(int, int)>()
            },
        };

        [Theory]
        [MemberData(nameof(Tupled2_Data))]
        public void Tupled2(int[] array, (int, int)[] expected)
        {
            array.Tupled2().Should().Equal(expected);
            new Span<int>(array).Tupled2().Should().Equal(expected);
            new ReadOnlySpan<int>(array).Tupled2().Should().Equal(expected);
        }

        public static TheoryData CompressCount_Data => new TheoryData<IEnumerable<byte>, (byte Value, int Count)[]>
        {
            {
                Enumerable.Range(0, 6).Select(i => (byte)i),
                new (byte, int)[]
                {
                    (0, 1),
                    (1, 1),
                    (2, 1),
                    (3, 1),
                    (4, 1),
                    (5, 1),
                }
            },
            {
                new byte[] { 1, 1, 2, 2, 3, 3, },
                new (byte, int)[]
                {
                    (1, 2),
                    (2, 2),
                    (3, 2),
                }
            },
            {
                new byte[] { 1, 1, 2, 2, 3,},
                new (byte, int)[]
                {
                    (1, 2),
                    (2, 2),
                    (3, 1),
                }
            },
            {
                new byte[] { 1, 2, 2, 3, 3, },
                new (byte, int)[]
                {
                    (1, 1),
                    (2, 2),
                    (3, 2),
                }
            },
            {
                new byte[] { 1, 2, 2, 3, 3, 1, 1, 2, },
                new (byte, int)[]
                {
                    (1, 1),
                    (2, 2),
                    (3, 2),
                    (1, 2),
                    (2, 1),
                }
            },
        };
        [Theory]
        [MemberData(nameof(CompressCount_Data))]
        public void CompressCount(IEnumerable<byte> input, (byte Value, int Count)[] expected)
        {
            input.CompressCount().Should().Equal(expected);
        }
    }
}