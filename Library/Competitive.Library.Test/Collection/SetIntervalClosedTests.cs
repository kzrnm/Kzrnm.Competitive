using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class SetIntervalClosedTests
    {
        public static TheoryData<SerializableTuple<int, int>[], SerializableTuple<int, int>[]> Add_Data => new()
        {
            {
                Array.Empty<SerializableTuple<int, int>>(),
                Array.Empty<SerializableTuple<int, int>>()
            },
            {
                new SerializableTuple<int, int>[]{
                    (101,101),
                    (15, 100),
                    (-20, -2),
                    (0, 10),
                },
                new SerializableTuple<int, int>[]{
                    (-20, -2),
                    (0, 10),
                    (15, 100),
                    (101,101),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (9, 100),
                    (-20, 2),
                    (0, 10),
                },
                new SerializableTuple<int, int>[]{
                    (-20, 100),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                },
                new SerializableTuple<int, int>[]{
                    (10, 20),
                    (30, 40),
                    (50, 60),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                },
                new SerializableTuple<int, int>[]{
                    (10, 25),
                    (30, 40),
                    (50, 60),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                },
                new SerializableTuple<int, int>[]{
                    (10, 40),
                    (50, 60),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                    (10, 41),
                },
                new SerializableTuple<int, int>[]{
                    (10, 41),
                    (50, 60),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                    (10, 41),
                    (49, 60),
                },
                new SerializableTuple<int, int>[]{
                    (10, 41),
                    (49, 60),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                    (10, 41),
                    (49, 60),
                    (9, 61),
                },
                new SerializableTuple<int, int>[]{
                    (9, 61),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (10, 1000),
                    (20, 0),
                    (100, 900),
                    (100, 900),
                },
                new SerializableTuple<int, int>[]{
                    (10, 1000),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (10, 100),
                    (-20, 0),
                    (0, 10),
                },
                new SerializableTuple<int, int>[]{
                    (-20, 100),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (-20, 190),
                    (-1000, 1000),
                    (-30, 100),
                },
                new SerializableTuple<int, int>[]{
                    (-1000, 1000),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (-30, 100),
                    (-10, 1000),
                    (-20, 190),
                },
                new SerializableTuple<int, int>[]{
                    (-30, 1000),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (-10, 0),
                    (10, 20),
                    (30, 40),
                    (0, 30),
                },
                new SerializableTuple<int, int>[]{
                    (-10, 40),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (-10, 0),
                    (10, 20),
                    (30, 40),
                    (-1, 31),
                },
                new SerializableTuple<int, int>[]{
                    (-10, 40),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (-10, 0),
                    (10, 20),
                    (30, 40),
                    (1, 29),
                },
                new SerializableTuple<int, int>[]{
                    (-10, 0),
                    (1, 29),
                    (30, 40),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (0, 0),
                    (0, 0),
                    (0, 0),
                },
                new SerializableTuple<int, int>[]{
                    (0, 0),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (99, 100),
                    (98, 100),
                    (97, 100),
                    (96, 100),
                    (95, 100),
                    (101, 102),
                    (101, 103),
                    (101, 104),
                    (101, 105),
                    (101, 106),
                },
                new SerializableTuple<int, int>[]{
                    (95, 100),
                    (101, 106),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (0, 9),
                    (10, 20),
                    (30, 40),
                    (10, 25),
                },
                new SerializableTuple<int, int>[]{
                    (0, 9),
                    (10, 25),
                    (30, 40),
                }
            },
            {
                new SerializableTuple<int, int>[]{
                    (0, 9),
                    (20, 29),
                    (30, 40),
                    (10, 29),
                },
                new SerializableTuple<int, int>[]{
                    (0, 9),
                    (10, 29),
                    (30, 40),
                }
            }
        };
        [Theory]
        [MemberData(nameof(Add_Data))]
        public void Contructor(SerializableTuple<int, int>[] arg, SerializableTuple<int, int>[] result)
        {
            new SetIntervalClosedInt(arg.ToTuple()).ShouldBe(result.ToTuple());
        }

        [Theory]
        [MemberData(nameof(Add_Data))]
        public void AddTheory(SerializableTuple<int, int>[] arg, SerializableTuple<int, int>[] result)
        {
            var set = new SetIntervalClosedInt();
            foreach (var (f, t) in arg)
                set.Add(f, t);
            set.ShouldBe(result.ToTuple());
        }
        [Fact]
        public void Add()
        {
            var set = new SetIntervalClosedInt();
            set.ShouldBe([]);

            set.Add(50, 60);
            set.ShouldBe([
                (50, 60),
            ]);

            set.Add(10, 20);
            set.ShouldBe([
                (10, 20),
                (50, 60),
            ]);

            set.Add(30, 40);
            set.ShouldBe([
                (10, 20),
                (30, 40),
                (50, 60),
            ]);

            set.Add(15, 25);
            set.ShouldBe([
                (10, 25),
                (30, 40),
                (50, 60),
            ]);

            set.Add(25, 30);
            set.ShouldBe([
                (10, 40),
                (50, 60),
            ]);

            set.Add(10, 41);
            set.ShouldBe([
                (10, 41),
                (50, 60),
            ]);

            set.Add(49, 60);
            set.ShouldBe([
                (10, 41),
                (49, 60),
            ]);

            set.Add(42, 48);
            set.ShouldBe([
                (10, 41),
                (42, 48),
                (49, 60),
            ]);

            set.Add(9, 61);
            set.ShouldBe([
                (9, 61),
            ]);

            set.Add(70, 80);
            set.ShouldBe([
                (9, 61),
                (70,80),
            ]);

            set.Add(5, 70);
            set.ShouldBe([
                (5,80),
            ]);
        }

        [Fact]
        public void MinMax()
        {
            var set = new SetIntervalClosedInt();
            set.ShouldBe([]);
            set.Min.ShouldBe(default);
            set.Max.ShouldBe(default);
            set.Add(50, 60);
            set.Min.ShouldBe((50, 60));
            set.Max.ShouldBe((50, 60));
            set.Add(10, 20);
            set.Min.ShouldBe((10, 20));
            set.Max.ShouldBe((50, 60));
            set.Add(30, 40);
            set.Min.ShouldBe((10, 20));
            set.Max.ShouldBe((50, 60));
            set.Add(15, 25);
            set.Min.ShouldBe((10, 25));
            set.Max.ShouldBe((50, 60));
            set.Add(25, 35);
            set.Min.ShouldBe((10, 40));
            set.Max.ShouldBe((50, 60));
            set.Add(10, 41);
            set.Min.ShouldBe((10, 41));
            set.Max.ShouldBe((50, 60));
            set.Add(49, 60);
            set.Min.ShouldBe((10, 41));
            set.Max.ShouldBe((49, 60));
            set.Add(9, 61);
            set.Min.ShouldBe((9, 61));
            set.Max.ShouldBe((9, 61));
        }


        public static TheoryData<int, int, bool, SerializableTuple<int, int>[]> Remove_Data => new()
        {
            { 1,9,false,new SerializableTuple<int, int>[]{(10, 20),(25, 30),(35, 40),(50, 60) } },
            { 21,24,false,new SerializableTuple<int, int>[]{(10, 20),(25, 30),(35, 40),(50, 60) } },
            { 1,10,true,new SerializableTuple<int, int>[]{(11, 20),(25, 30),(35, 40),(50, 60) } },
            { 1,20,true,new SerializableTuple<int, int>[]{(25, 30),(35, 40),(50, 60) } },
            { 20,25,true,new SerializableTuple<int, int>[]{(10, 19),(26, 30),(35, 40),(50, 60) } },
            { 20,39,true,new SerializableTuple<int, int>[]{(10, 19),(40, 40),(50, 60) } },
            { 10,60,true,Array.Empty<SerializableTuple<int, int>>()},
            { 1,61,true,Array.Empty<SerializableTuple<int, int>>()},
        };
        [Theory]
        [MemberData(nameof(Remove_Data))]
        public void Remove(int from, int to, bool success, SerializableTuple<int, int>[] result)
        {
            var set = new SetIntervalClosedInt([
                (10, 20),
                (25, 30),
                (35, 40),
                (50, 60)]);
            set.Remove(from, to).ShouldBe(success);
            set.ShouldBe(result.ToTuple());
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(18, true)]
        [InlineData(19, true)]
        [InlineData(20, true)]
        [InlineData(28, false)]
        [InlineData(29, false)]
        [InlineData(30, true)]
        [InlineData(38, true)]
        [InlineData(39, true)]
        [InlineData(40, true)]
        [InlineData(48, false)]
        [InlineData(49, false)]
        [InlineData(50, true)]
        [InlineData(58, true)]
        [InlineData(59, true)]
        [InlineData(60, true)]
        [InlineData(68, false)]
        [InlineData(69, false)]
        public void Contains(int value, bool isContains)
        {
            var set = new SetIntervalClosedInt([
                (10, 20),
                (30, 40),
                (50, 60)]);
            set.Contains(value).ShouldBe(isContains);
            if (isContains)
                set.FindNode(value).ShouldNotBeNull();
            else
                set.FindNode(value).ShouldBeNull();
        }


        [Theory]
        [InlineData(1, 10, false)]
        [InlineData(1, 11, false)]
        [InlineData(1, 20, false)]
        [InlineData(9, 20, false)]
        [InlineData(10, 20, true)]
        [InlineData(10, 19, true)]
        [InlineData(11, 19, true)]
        public void ContainsRange(int from, int to, bool isContains)
        {
            var set = new SetIntervalClosedInt([
                (10, 20),
                (30, 40),
                (50, 60)]);
            ((ICollection<(int, int)>)set).Contains((from, to)).ShouldBe(isContains);
        }

        public static TheoryData<int, int, SerializableTuple<int, int>[]> RangeTruncate_Data => new()
        {
            {  0, 9, [] },
            { 21, 29, [] },
            { 61, 70, [] },
            {  0, 10, [(10, 10)]},
            { 60, 70, [(60, 60)]},
            { 10, 20, [(10, 20)]},
            { 10, 30, [(10, 20), (30, 30)]},
            { 10, 35, [(10, 20), (30, 35)]},
            { 15, 60, [(15, 20), (30, 40), (50, 60)]},
        };
        [Theory]
        [MemberData(nameof(RangeTruncate_Data))]
        public void RangeTruncate(int from, int to, SerializableTuple<int, int>[] expected)
        {
            var set = new SetIntervalClosedInt([
                (10, 20),
                (30, 40),
                (50, 60)]);
            set.RangeTruncate(from, to).ShouldBe(expected.ToTuple());
        }

        public static TheoryData<int, int, SerializableTuple<int, int>[]> RangeAll_Data => new()
        {
            {  0, 9, Array.Empty<SerializableTuple<int, int>>() },
            { 21, 29, Array.Empty<SerializableTuple<int, int>>() },
            { 61, 70, Array.Empty<SerializableTuple<int, int>>() },
            {  0, 10, [(10, 20)]},
            { 60, 70, [(50, 60)]},
            { 10, 20, [(10, 20)]},
            { 10, 30, [(10, 20), (30, 40)]},
            { 10, 35, [(10, 20), (30, 40)]},
            { 15, 60, [(10, 20), (30, 40), (50, 60)]},
        };
        [Theory]
        [MemberData(nameof(RangeAll_Data))]
        public void RangeAll(int from, int to, SerializableTuple<int, int>[] expected)
        {
            var set = new SetIntervalClosedInt([
                (10, 20),
                (30, 40),
                (50, 60)]);
            set.RangeAll(from, to).ShouldBe(expected.ToTuple());
        }

        [Fact]
        public void UnionWith()
        {
            var set = new SetIntervalClosedInt([
                (10, 20),
                (30, 40),
                (50, 60),
                (100, 115)]);
            set.UnionWith([
                (7, 12),
                (22, 25),
                (40, 75),
            ]);
            set.ShouldBe([
                (7, 20),
                (22, 25),
                (30, 75),
                (100, 115),
            ]);
        }

        [Fact]
        public void ExceptWith()
        {
            var set = new SetIntervalClosedInt([
                (-10,-4),
                (10, 20),
                (30, 40),
                (50, 60),
                (100, 115)]);
            set.ExceptWith([
                (-10,-4),
                (7, 12),
                (22, 25),
                (26, 44),
                (49, 105),
            ]);
            set.ShouldBe([
                (13, 20),
                (106, 115),
            ]);
        }

        [Fact]
        public void IntersectWith()
        {
            var set = new SetIntervalClosedInt([
                (-10,-4),
                (10, 20),
                (30, 40),
                (50, 60),
                (100, 115)]);
            set.IntersectWith([
                (-10,-4),
                (7, 12),
                (22, 25),
                (26, 44),
                (49, 105),
            ]);
            set.ShouldBe([
                (-10, -4),
                (10, 12),
                (30, 40),
                (50, 60),
                (100, 105),
            ]);
        }
    }
}
