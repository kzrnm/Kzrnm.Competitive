using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace Kzrnm.Competitive.Extension
{
    public class ArrayExtensionTests
    {
        [Fact]
        public void Fill()
        {
            new string[100].Fill("🦈").Should().Equal(Enumerable.Repeat("🦈", 100));
        }

        [Fact]
        public void Sort()
        {
            var arr = Enumerable.Repeat(new Random(), 2000).Select(r => r.Next()).ToArray();
            MemoryMarshal.Cast<int, long>(arr).ToArray().Sort().Should().BeInAscendingOrder(Comparer<long>.Default);
        }

        [Fact]
        public void SortString()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort().Should().Equal(new string[] {
                "AB",
                "BCD443",
                "a",
                "aBc",
                "aaa31",
                "dsjkf50000",
                "zzz14144",
            });
        }

        [Fact]
        public void SortExpression()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort(s => s.Length).Should().Equal(new string[] {
                "a",
                "AB",
                "aBc",
                "aaa31",
                "BCD443",
                "zzz14144",
                "dsjkf50000",
            });
        }

        [Fact]
        public void SortComparison()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort((s1, s2) => s1.Length.CompareTo(s2.Length)).Should().Equal(new string[] {
                "a",
                "AB",
                "aBc",
                "aaa31",
                "BCD443",
                "zzz14144",
                "dsjkf50000",
            });
        }

        [Fact]
        public void SortComparer()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort(StringComparer.OrdinalIgnoreCase).Should().Equal(new string[] {
                "a",
                "aaa31",
                "AB",
                "aBc",
                "BCD443",
                "dsjkf50000",
                "zzz14144",
            });
        }

        [Fact]
        public void Reverse()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Reverse().Should().Equal(new string[] {
                "aaa31",
                "BCD443",
                "dsjkf50000",
                "a",
                "AB",
                "aBc",
                "zzz14144",
            });
        }

        [Fact]
        public void Get()
        {
            var arr = new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            };
            arr.Get(1).Should().Be(24);
            arr.Get(1) = 25;
            arr[1].Should().Be(25);

            arr.Get(-1).Should().Be(3123);
            arr.Get(-1) = -2;
            arr[^1].Should().Be(-2);
        }


        [Fact]
        public void GetOrDummy()
        {
            var arr = new long[] {
                43,24,8373,
            };
            arr.GetOrDummy(0).Should().Be(43);
            arr.GetOrDummy(1).Should().Be(24);
            arr.GetOrDummy(2).Should().Be(8373);
            arr.Should().Equal(43, 24, 8373);

            arr.GetOrDummy(2) = 33;
            arr.GetOrDummy(3) = 55;
            arr.GetOrDummy(-1) = 66;

            arr.GetOrDummy(0).Should().Be(43);
            arr.GetOrDummy(1).Should().Be(24);
            arr.GetOrDummy(2).Should().Be(33);
            arr.Should().Equal(43, 24, 33);
        }

        [Fact]
        public void GetOrDummy2()
        {
            var arr = new long[][] {
                new long[]{ 43, 24, 8373 },
                new long[]{ -13, -4, 54},
            };
            arr.GetOrDummy(0, 0).Should().Be(43);
            arr.GetOrDummy(0, 1).Should().Be(24);
            arr.GetOrDummy(0, 2).Should().Be(8373);
            arr[0].Should().Equal(43, 24, 8373);
            arr[1].Should().Equal(-13, -4, 54);

            arr.GetOrDummy(0, 2) = 33;
            arr.GetOrDummy(0, 3) = 55;
            arr.GetOrDummy(0, -1) = 66;

            arr.GetOrDummy(0, 0).Should().Be(43);
            arr.GetOrDummy(0, 1).Should().Be(24);
            arr.GetOrDummy(0, 2).Should().Be(33);

            arr.GetOrDummy(1, 0).Should().Be(-13);
            arr.GetOrDummy(1, 1).Should().Be(-4);
            arr.GetOrDummy(1, 2).Should().Be(54);

            arr.GetOrDummy(1, 2) = -3;
            arr.GetOrDummy(1, 3) = 55;
            arr.GetOrDummy(1, -1) = 66;

            arr.GetOrDummy(1, 0).Should().Be(-13);
            arr.GetOrDummy(1, 1).Should().Be(-4);
            arr.GetOrDummy(1, 2).Should().Be(-3);

            arr[0].Should().Equal(43, 24, 33);
            arr[1].Should().Equal(-13, -4, -3);
        }


        [Fact]
        public void GetOrDummy3()
        {
            var arr = new long[][][] {
                new long[][]
                {
                    new long[] { 1, 2, },
                    new long[] { 3, 4, },
                },
                new long[][]
                {
                    new long[] { 5, 6, },
                    new long[] { 7, 8, },
                },
            };
            arr.GetOrDummy(0, 0, 0).Should().Be(1);
            arr.GetOrDummy(0, 0, 1).Should().Be(2);
            arr.GetOrDummy(0, 1, 0).Should().Be(3);
            arr.GetOrDummy(0, 1, 1).Should().Be(4);
            arr.GetOrDummy(1, 0, 0).Should().Be(5);
            arr.GetOrDummy(1, 0, 1).Should().Be(6);
            arr.GetOrDummy(1, 1, 0).Should().Be(7);
            arr.GetOrDummy(1, 1, 1).Should().Be(8);
            arr[0][0].Should().Equal(1, 2);
            arr[0][1].Should().Equal(3, 4);
            arr[1][0].Should().Equal(5, 6);
            arr[1][1].Should().Equal(7, 8);

            arr.GetOrDummy(0, 0, 0) += 10;
            arr.GetOrDummy(0, 0, 1) += 10;
            arr.GetOrDummy(0, 1, 0) += 10;
            arr.GetOrDummy(0, 1, 1) += 10;
            arr.GetOrDummy(1, 0, 0) += 10;
            arr.GetOrDummy(1, 0, 1) += 10;
            arr.GetOrDummy(1, 1, 0) += 10;
            arr.GetOrDummy(1, 1, 1) += 10;

            arr.GetOrDummy(0, 0, 2) += 100;
            arr.GetOrDummy(0, 2, 0) += 100;
            arr.GetOrDummy(0, 2, 2) += 100;
            arr.GetOrDummy(2, 0, 0) += 100;
            arr.GetOrDummy(2, 0, 2) += 100;
            arr.GetOrDummy(2, 2, 0) += 100;
            arr.GetOrDummy(2, 2, 2) += 100;

            arr.GetOrDummy(0, 0, 0).Should().Be(11);
            arr.GetOrDummy(0, 0, 1).Should().Be(12);
            arr.GetOrDummy(0, 1, 0).Should().Be(13);
            arr.GetOrDummy(0, 1, 1).Should().Be(14);
            arr.GetOrDummy(1, 0, 0).Should().Be(15);
            arr.GetOrDummy(1, 0, 1).Should().Be(16);
            arr.GetOrDummy(1, 1, 0).Should().Be(17);
            arr.GetOrDummy(1, 1, 1).Should().Be(18);
            arr[0][0].Should().Equal(11, 12);
            arr[0][1].Should().Equal(13, 14);
            arr[1][0].Should().Equal(15, 16);
            arr[1][1].Should().Equal(17, 18);
        }
    }
}