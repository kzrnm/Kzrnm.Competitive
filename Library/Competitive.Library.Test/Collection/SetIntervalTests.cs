using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Collection
{
    public class SetIntervalTests
    {
        public static TheoryData Add_Data = new TheoryData<(int from, int to)[], (int from, int to)[]>
        {
            {
                Array.Empty<(int from, int to)>(),
                Array.Empty<(int from, int to)>()
            },
            {
                new (int from, int to)[]{
                    (15, 100),
                    (-20, -2),
                    (0, 10),
                },
                new (int from, int to)[]{
                    (-20, -2),
                    (0, 10),
                    (15, 100),
                }
            },
            {
                new (int from, int to)[]{
                    (9, 100),
                    (-20, 2),
                    (0, 10),
                },
                new (int from, int to)[]{
                    (-20, 100),
                }
            },
            {
                new (int from, int to)[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                },
                new (int from, int to)[]{
                    (10, 20),
                    (30, 40),
                    (50, 60),
                }
            },
            {
                new (int from, int to)[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                },
                new (int from, int to)[]{
                    (10, 25),
                    (30, 40),
                    (50, 60),
                }
            },
            {
                new (int from, int to)[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                },
                new (int from, int to)[]{
                    (10, 40),
                    (50, 60),
                }
            },
            {
                new (int from, int to)[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                    (10, 41),
                },
                new (int from, int to)[]{
                    (10, 41),
                    (50, 60),
                }
            },
            {
                new (int from, int to)[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                    (10, 41),
                    (49, 60),
                },
                new (int from, int to)[]{
                    (10, 41),
                    (49, 60),
                }
            },
            {
                new (int from, int to)[]{
                    (50, 60),
                    (10, 20),
                    (30, 40),
                    (15, 25),
                    (25, 35),
                    (10, 41),
                    (49, 60),
                    (9, 61),
                },
                new (int from, int to)[]{
                    (9, 61),
                }
            },
            {
                new (int from, int to)[]{
                    (10, 1000),
                    (20, 0),
                    (100, 900),
                    (100, 900),
                },
                new (int from, int to)[]{
                    (10, 1000),
                }
            },
            {
                new (int from, int to)[]{
                    (10, 100),
                    (-20, 0),
                    (0, 10),
                },
                new (int from, int to)[]{
                    (-20, 100),
                }
            },
            {
                new (int from, int to)[]{
                    (-20, 190),
                    (-1000, 1000),
                    (-30, 100),
                },
                new (int from, int to)[]{
                    (-1000, 1000),
                }
            },
            {
                new (int from, int to)[]{
                    (-30, 100),
                    (-10, 1000),
                    (-20, 190),
                },
                new (int from, int to)[]{
                    (-30, 1000),
                }
            },
            {
                new (int from, int to)[]{
                    (-10, 0),
                    (10, 20),
                    (30, 40),
                    (0, 30),
                },
                new (int from, int to)[]{
                    (-10, 40),
                }
            },
            {
                new (int from, int to)[]{
                    (-10, 0),
                    (10, 20),
                    (30, 40),
                    (-1, 31),
                },
                new (int from, int to)[]{
                    (-10, 40),
                }
            },
            {
                new (int from, int to)[]{
                    (-10, 0),
                    (10, 20),
                    (30, 40),
                    (1, 29),
                },
                new (int from, int to)[]{
                    (-10, 0),
                    (1, 29),
                    (30, 40),
                }
            },
            {
                new (int from, int to)[]{
                    (0, 1),
                    (0, 1),
                    (0, 1),
                },
                new (int from, int to)[]{
                    (0, 1),
                }
            },
            {
                new (int from, int to)[]{
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
                new (int from, int to)[]{
                    (95, 100),
                    (101, 106),
                }
            },
            {
                new (int from, int to)[]{
                    (0, 9),
                    (10, 20),
                    (30, 40),
                    (10, 25),
                },
                new (int from, int to)[]{
                    (0, 9),
                    (10, 25),
                    (30, 40),
                }
            },
            {
                new (int from, int to)[]{
                    (0, 9),
                    (20, 29),
                    (30, 40),
                    (10, 29),
                },
                new (int from, int to)[]{
                    (0, 9),
                    (10, 29),
                    (30, 40),
                }
            }
        };
        [Theory]
        [MemberData(nameof(Add_Data))]
        public void Contructor((int from, int to)[] arg, (int from, int to)[] result)
        {
            new SetIntervalInt(arg).Should().Equal(result);
        }

        [Theory]
        [MemberData(nameof(Add_Data))]
        public void AddTheory((int from, int to)[] arg, (int from, int to)[] result)
        {
            var set = new SetIntervalInt();
            foreach (var (f, t) in arg)
                set.Add(f, t);
            set.Should().Equal(result);
        }
        [Fact]
        public void Add()
        {
            var set = new SetIntervalInt();
            set.Should().Equal(Array.Empty<(int From, int ToExclusive)>());

            set.Add(50, 60);
            set.Should().Equal(new (int, int)[] {
                (50, 60),
            });

            set.Add(10, 20);
            set.Should().Equal(new (int, int)[] {
                (10, 20),
                (50, 60),
            });

            set.Add(30, 40);
            set.Should().Equal(new (int, int)[] {
                (10, 20),
                (30, 40),
                (50, 60),
            });

            set.Add(15, 25);
            set.Should().Equal(new (int, int)[] {
                (10, 25),
                (30, 40),
                (50, 60),
            });

            set.Add(25, 35);
            set.Should().Equal(new (int, int)[] {
                (10, 40),
                (50, 60),
            });

            set.Add(10, 41);
            set.Should().Equal(new (int, int)[] {
                (10, 41),
                (50, 60),
            });

            set.Add(49, 60);
            set.Should().Equal(new (int, int)[] {
                (10, 41),
                (49, 60),
            });

            set.Add(9, 61);
            set.Should().Equal(new (int, int)[] {
                (9, 61),
            });

            set.Add(70, 80);
            set.Should().Equal(new (int, int)[] {
                (9, 61),
                (70,80),
            });

            set.Add(5, 70);
            set.Should().Equal(new (int, int)[] {
                (5,80),
            });
        }

        [Fact]
        public void MinMax()
        {
            var set = new SetIntervalInt();
            set.Should().Equal(Array.Empty<(int From, int ToExclusive)>());
            set.Min.Should().Be(default);
            set.Max.Should().Be(default);
            set.Add(50, 60);
            set.Min.Should().Be((50, 60));
            set.Max.Should().Be((50, 60));
            set.Add(10, 20);
            set.Min.Should().Be((10, 20));
            set.Max.Should().Be((50, 60));
            set.Add(30, 40);
            set.Min.Should().Be((10, 20));
            set.Max.Should().Be((50, 60));
            set.Add(15, 25);
            set.Min.Should().Be((10, 25));
            set.Max.Should().Be((50, 60));
            set.Add(25, 35);
            set.Min.Should().Be((10, 40));
            set.Max.Should().Be((50, 60));
            set.Add(10, 41);
            set.Min.Should().Be((10, 41));
            set.Max.Should().Be((50, 60));
            set.Add(49, 60);
            set.Min.Should().Be((10, 41));
            set.Max.Should().Be((49, 60));
            set.Add(9, 61);
            set.Min.Should().Be((9, 61));
            set.Max.Should().Be((9, 61));
        }


        public static TheoryData Remove_Data = new TheoryData<int, int, bool, (int from, int to)[]>
        {
            { 1,10,false,new (int,int)[]{(10, 20),(25, 30),(35, 40),(50, 60) } },
            { 20,25,false,new (int,int)[]{(10, 20),(25, 30),(35, 40),(50, 60) } },
            { 1,12,true,new (int,int)[]{(12, 20),(25, 30),(35, 40),(50, 60) } },
            { 1,19,true,new (int,int)[]{(19, 20),(25, 30),(35, 40),(50, 60) } },
            { 1,20,true,new (int,int)[]{(25, 30),(35, 40),(50, 60) } },
            { 1,26,true,new (int,int)[]{(26, 30),(35, 40),(50, 60) } },
            { 18,22,true,new (int,int)[]{(10, 18),(25, 30),(35, 40),(50, 60) } },
            { 18,27,true,new (int,int)[]{(10, 18),(27, 30),(35, 40),(50, 60) } },
            { 21,49,true,new (int,int)[]{(10, 20),(50, 60) } },
            { 20,50,true,new (int,int)[]{(10, 20),(50, 60) } },
            { 19,51,true,new (int,int)[]{(10, 19),(51, 60) } },
            { 18,55,true,new (int,int)[]{(10, 18),(55, 60) } },
            { 10,60,true,Array.Empty<(int, int)>()},
            { 1,61,true,Array.Empty<(int, int)>()},
        };
        [Theory]
        [MemberData(nameof(Remove_Data))]
        public void Remove(int from, int to, bool success, (int from, int to)[] result)
        {
            var set = new SetIntervalInt(new[] {
                (10, 20),
                (25, 30),
                (35, 40),
                (50, 60)});
            set.Remove(from, to).Should().Be(success);
            set.Should().Equal(result);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(18, true)]
        [InlineData(19, true)]
        [InlineData(20, false)]
        [InlineData(28, false)]
        [InlineData(29, false)]
        [InlineData(30, true)]
        [InlineData(38, true)]
        [InlineData(39, true)]
        [InlineData(40, false)]
        [InlineData(48, false)]
        [InlineData(49, false)]
        [InlineData(50, true)]
        [InlineData(58, true)]
        [InlineData(59, true)]
        [InlineData(60, false)]
        [InlineData(68, false)]
        [InlineData(69, false)]
        public void Contains(int value, bool isContains)
        {
            var set = new SetIntervalInt(new[] {
                (10, 20),
                (30, 40),
                (50, 60)});
            set.Contains(value).Should().Be(isContains);
            if (isContains)
                set.FindNode(value).Should().NotBeNull();
            else
                set.FindNode(value).Should().BeNull();
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
            var set = new SetIntervalInt(new[] {
                (10, 20),
                (30, 40),
                (50, 60)});
            ((ICollection<(int, int)>)set).Contains((from, to)).Should().Be(isContains);
        }

        public static TheoryData RangeTruncate_Data = new TheoryData<int, int, (int From, int ToExclusive)[]>
        {
            {  0, 10, Array.Empty<(int, int)>() },
            { 20, 30, Array.Empty<(int, int)>() },
            { 60, 70, Array.Empty<(int, int)>() },
            {  0, 11, new[]{ (10, 11) } },
            { 59, 70, new[]{ (59, 60) } },
            { 10, 20, new[]{ (10, 20) } },
            { 10, 30, new[]{ (10, 20) } },
            { 10, 35, new[]{ (10, 20), (30, 35) } },
            { 15, 60, new[]{ (15, 20), (30, 40), (50, 60) } },
        };
        [Theory]
        [MemberData(nameof(RangeTruncate_Data))]
        public void RangeTruncate(int from, int to, (int From, int ToExclusive)[] expected)
        {
            var set = new SetIntervalInt(new[] {
                (10, 20),
                (30, 40),
                (50, 60)});
            set.RangeTruncate(from, to).Should().Equal(expected);
        }

        public static TheoryData RangeAll_Data = new TheoryData<int, int, (int From, int ToExclusive)[]>
        {
            {  0, 10, Array.Empty<(int, int)>() },
            { 20, 30, Array.Empty<(int, int)>() },
            { 60, 70, Array.Empty<(int, int)>() },
            {  0, 11, new[]{ (10, 20) } },
            { 59, 70, new[]{ (50, 60) } },
            { 10, 20, new[]{ (10, 20) } },
            { 10, 30, new[]{ (10, 20) } },
            { 10, 35, new[]{ (10, 20), (30, 40) } },
            { 15, 60, new[]{ (10, 20), (30, 40), (50, 60) } },
        };
        [Theory]
        [MemberData(nameof(RangeAll_Data))]
        public void RangeAll(int from, int to, (int From, int ToExclusive)[] expected)
        {
            var set = new SetIntervalInt(new[] {
                (10, 20),
                (30, 40),
                (50, 60)});
            set.RangeAll(from, to).Should().Equal(expected);
        }

        [Fact]
        public void UnionWith()
        {
            var set = new SetIntervalInt(new[] {
                (10, 20),
                (30, 40),
                (50, 60),
                (100, 115)});
            set.UnionWith(new[] {
                (7, 12),
                (22, 25),
                (40, 75),
            });
            set.Should().Equal(
                (7, 20),
                (22, 25),
                (30, 75),
                (100, 115));
        }

        [Fact]
        public void ExceptWith()
        {
            var set = new SetIntervalInt(new[] {
                (-10,-4),
                (10, 20),
                (30, 40),
                (50, 60),
                (100, 115)});
            set.ExceptWith(new[] {
                (-10,-4),
                (7, 12),
                (22, 25),
                (26, 44),
                (49, 105),
            });
            set.Should().Equal(
                (12, 20),
                (105, 115));
        }

        [Fact]
        public void IntersectWith()
        {
            var set = new SetIntervalInt(new[] {
                (-10,-4),
                (10, 20),
                (30, 40),
                (50, 60),
                (100, 115)});
            set.IntersectWith(new[] {
                (-10,-4),
                (7, 12),
                (22, 25),
                (26, 44),
                (49, 105),
            });
            set.Should().Equal(
                (-10, -4),
                (10, 12),
                (30, 40),
                (50, 60),
                (100, 105));
        }
    }
}
