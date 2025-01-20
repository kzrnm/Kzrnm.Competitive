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
            }.MaxBy().ShouldBe((17, 3401434));
        }

        [Fact]
        public void MaxByArrayFunc()
        {
            new (int, long)[] {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MaxBy(tup => tup.Item2).ShouldBe((17, (0, 3401434)));
        }

        [Fact]
        public void MaxByFunc()
        {
            new List<(int, long)> {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MaxBy2(tup => tup.Item2).ShouldBe(((0, 3401434), 3401434));
        }


        [Fact]
        public void MinBy()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.MinBy().ShouldBe((3, 4));
        }

        [Fact]
        public void MinByArrayFunc()
        {
            new (int, long)[] {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MinBy(tup => tup.Item2).ShouldBe((3, (0, 4)));
        }

        [Fact]
        public void MinByFunc()
        {
            new List<(int, long)> {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MinBy2(tup => tup.Item2).ShouldBe(((0, 4), 4));
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
            }.GroupCount().ShouldSatisfyAllConditions([
                g => g.Count.ShouldBe(5),
                g => g.ShouldContainKeyAndValue(StringComparison.Ordinal, 1),
                g => g.ShouldContainKeyAndValue(StringComparison.OrdinalIgnoreCase, 4),
                g => g.ShouldContainKeyAndValue(StringComparison.CurrentCulture, 3),
                g => g.ShouldContainKeyAndValue(StringComparison.InvariantCulture, 2),
                g => g.ShouldContainKeyAndValue(StringComparison.InvariantCultureIgnoreCase, 1),
            ]);
        }

        [Fact]
        public void GroupCountFunc()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.GroupCount(i => i % 7).ShouldSatisfyAllConditions([
                g => g.Count.ShouldBe(7),
                g => g.ShouldContainKeyAndValue(0, 4),
                g => g.ShouldContainKeyAndValue(1, 7),
                g => g.ShouldContainKeyAndValue(2, 1),
                g => g.ShouldContainKeyAndValue(3, 3),
                g => g.ShouldContainKeyAndValue(4, 2),
                g => g.ShouldContainKeyAndValue(5, 2),
                g => g.ShouldContainKeyAndValue(6, 2),
            ]);
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

            grouped.Length.ShouldBe(5);

            grouped[0].Key.ShouldBe(StringComparison.OrdinalIgnoreCase);
            grouped[0].ShouldBe([0, 1, 7, 9]);

            grouped[1].Key.ShouldBe(StringComparison.InvariantCulture);
            grouped[1].ShouldBe([2, 10]);

            grouped[2].Key.ShouldBe(StringComparison.InvariantCultureIgnoreCase);
            grouped[2].ShouldBe([3]);

            grouped[3].Key.ShouldBe(StringComparison.CurrentCulture);
            grouped[3].ShouldBe([4, 5, 6]);

            grouped[4].Key.ShouldBe(StringComparison.Ordinal);
            grouped[4].ShouldBe([8]);
        }

        [Fact]
        public void GroupIndexFunc()
        {
            var grouped = new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.GroupIndex(i => i % 7).ToArray();

            grouped.Length.ShouldBe(7);

            grouped[0].Key.ShouldBe(1);
            grouped[0].ShouldBe([0, 2, 6, 9, 17, 18, 20]);

            grouped[1].Key.ShouldBe(3);
            grouped[1].ShouldBe([1, 11, 14]);

            grouped[2].Key.ShouldBe(4);
            grouped[2].ShouldBe([3, 10]);

            grouped[3].Key.ShouldBe(0);
            grouped[3].ShouldBe([4, 5, 7, 16]);

            grouped[4].Key.ShouldBe(6);
            grouped[4].ShouldBe([8, 19]);

            grouped[5].Key.ShouldBe(5);
            grouped[5].ShouldBe([12, 15]);

            grouped[6].Key.ShouldBe(2);
            grouped[6].ShouldBe([13]);
        }


        [Fact]
        public void Flatten()
        {
            string[] strs = ["abc", "def", "012", "345", "678"];
            strs.Flatten()
                .ShouldBe(
                ['a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8']);

            int[][] arr = [
                [1, 2, 3],
                [-1, -2, -3],
                [4, 5, 6],
                [-6, -5, -4],
                [7, 8, 9],
            ];
            var expected = new[] { 1, 2, 3, -1, -2, -3, 4, 5, 6, -6, -5, -4, 7, 8, 9 };

            arr.Flatten().ShouldBe(expected);
            arr.ToList().Flatten().ShouldBe(expected);
        }

        [Fact]
        public void FlattenTuple2()
        {
            new[] { (1, 2), (3, 4), (5, 6) }.Flatten().ShouldBe([1, 2, 3, 4, 5, 6]);
        }

        [Fact]
        public void FlattenTuple3()
        {
            new[] { (1, 2, 3), (4, 5, 6) }.Flatten().ShouldBe([1, 2, 3, 4, 5, 6]);
        }

        [Fact]
        public void MinMax()
        {
            new int[] {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MinMax()
            .ShouldBe((-75, 81));
            new List<int> {
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17 }.MinMax()
            .ShouldBe((-75, 81));
            ((Span<int>)[
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17]).MinMax()
            .ShouldBe((-75, 81));
            ((ReadOnlySpan<int>)[
                4, 69, 13,  0,-21,-68,-26, 52, -7, 24,
              -63,-39, 81, 35,  9, 42, -5,-27, 56, 24,
               27, 13,-75,-61, 76, 40,-27, 48, 36, -17]).MinMax()
            .ShouldBe((-75, 81));

            Array.Empty<int>().MinMax().ShouldBe((0, 0));
            Array.Empty<int>().AsSpan().MinMax().ShouldBe((0, 0));
        }

        [Fact]
        public void SpanSelect()
        {
            var span = Enumerable.Range(0, 10).ToArray().AsSpan();
            static int Func(int n) => 2 * n;
            span.Select(Func).ToArray().ShouldBe(Enumerable.Range(0, 10).Select(Func));
            ((ReadOnlySpan<int>)span).Select(Func).ToArray().ShouldBe(Enumerable.Range(0, 10).Select(Func));

            static int FuncIndex(int n, int i) => i * n;
            span.Select(FuncIndex).ToArray().ShouldBe(Enumerable.Range(0, 10).Select(FuncIndex));
            ((ReadOnlySpan<int>)span).Select(FuncIndex).ToArray().ShouldBe(Enumerable.Range(0, 10).Select(FuncIndex));
        }

        public static TheoryData<int[], int, int[][]> Chunk_Data => new()
        {
            {
                Enumerable.Range(0, 12).ToArray(),
                4,
                new int[][]
                {
                    [0, 1, 2, 3,],
                    [4, 5, 6, 7,],
                    [8, 9, 10, 11,],
                }
            },
            {
                Enumerable.Range(0, 12).ToArray(),
                5,
                new int[][]
                {
                    [0, 1, 2, 3, 4,],
                    [5, 6, 7, 8, 9,],
                    [10, 11,],
                }
            },
            {
                Enumerable.Empty<int>().ToArray(),
                5,
                Array.Empty<int[]>()
            },
        };
        [Theory]
        [MemberData(nameof(Chunk_Data))]
        public void Chunk(int[] input, int bufferSize, int[][] expected)
        {
            var result = input.Select(t => t).Chunk(bufferSize).ToArray();
            result.Length.ShouldBe(expected.Length);
            for (int i = 0; i < expected.Length; i++)
                result[i].ShouldBe(expected[i]);
        }

        public static TheoryData<int[], SerializableTuple<int, int>[]> Tupled2_Data => new()
        {
            {
                [1,2,3,4,5,6],
                [(1,2),(2,3),(3,4),(4,5),(5,6)]
            },
            {
                [1],
                []
            },
        };

        [Theory]
        [MemberData(nameof(Tupled2_Data))]
        public void Tupled2(int[] array, SerializableTuple<int, int>[] expected)
        {
            var expectedT = expected.ToTuple();
            array.Tupled2().ShouldBe(expectedT);
            new Span<int>(array).Tupled2().ShouldBe(expectedT);
            new ReadOnlySpan<int>(array).Tupled2().ShouldBe(expectedT);
        }

        public static TheoryData<byte[], SerializableTuple<byte, int>[]> CompressCount_Data => new()
        {
            {
                Enumerable.Range(0, 6).Select(i => (byte)i).ToArray(),
                new SerializableTuple<byte, int>[]
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
                new SerializableTuple<byte, int>[]
                {
                    (1, 2),
                    (2, 2),
                    (3, 2),
                }
            },
            {
                new byte[] { 1, 1, 2, 2, 3,},
                new SerializableTuple<byte, int>[]
                {
                    (1, 2),
                    (2, 2),
                    (3, 1),
                }
            },
            {
                new byte[] { 1, 2, 2, 3, 3, },
                new SerializableTuple<byte, int>[]
                {
                    (1, 1),
                    (2, 2),
                    (3, 2),
                }
            },
            {
                new byte[] { 1, 2, 2, 3, 3, 1, 1, 2, },
                new SerializableTuple<byte, int>[]
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
        public void CompressCount(byte[] input, SerializableTuple<byte, int>[] expected)
        {
            input.Select(t => t).CompressCount().ShouldBe(expected.ToTuple());
        }

        public static TheoryData<string, SerializableTuple<char, int>[]> CompressCount2_Data => new()
        {
            {
                "<<>>",
                new SerializableTuple<char, int>[]
                {
                    ('<', 2),
                    ('>', 2),
                }
            },
            {
                "<<><>>><><>><><><<>><<<><><<>",
                new SerializableTuple<char, int>[]
                {
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
                }
            },
        };
        [Theory]
        [MemberData(nameof(CompressCount2_Data))]
        public void CompressCount2(string input, SerializableTuple<char, int>[] expected)
        {
            input.CompressCount().ShouldBe(expected.ToTuple());
        }
    }
}