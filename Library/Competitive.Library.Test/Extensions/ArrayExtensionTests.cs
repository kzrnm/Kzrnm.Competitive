using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class ArrayExtensionTests
    {
        [Fact]
        public void Fill()
        {
            new string[100].Fill("🦈").ShouldBe(Enumerable.Repeat("🦈", 100));
        }

        [Fact]
        public void Sort()
        {
            var arr = Enumerable.Repeat(new Random(), 2000).Select(r => r.Next()).ToArray();
            MemoryMarshal.Cast<int, long>(arr).ToArray().Sort().ShouldBeInOrder();
        }

        public static TheoryData<string[], string[]> SortString_Data => new()
        {
            {
                [
                    "zzz14144",
                    "aBc",
                    "AB",
                    "a",
                    "dsjkf50000",
                    "BCD443",
                    "aaa31",
                ],
                [
                    "AB",
                    "BCD443",
                    "a",
                    "aBc",
                    "aaa31",
                    "dsjkf50000",
                    "zzz14144",
                ]
            },
        };
        [Theory]
        [MemberData(nameof(SortString_Data))]
        public void SortString(string[] input, string[] expected)
        {
            input.Sort().ShouldBe(expected);
        }

        public static TheoryData<string[], string[]> SortSelect_Data => new()
        {
            {
                [
                    "zzz14144",
                    "aBc",
                    "AB",
                    "a",
                    "dsjkf50000",
                    "BCD443",
                    "aaa31",
                ],
                [
                    "a",
                    "AB",
                    "aBc",
                    "aaa31",
                    "BCD443",
                    "zzz14144",
                    "dsjkf50000",
                ]
            },
        };
        [Theory]
        [MemberData(nameof(SortSelect_Data))]
        public void SortSelect(string[] input, string[] expected)
        {
            input.Sort(s => s.Length).ShouldBe(expected);
        }

        public static TheoryData<string[], string[]> SortComparison_Data => new()
        {
            {
                [
                    "zzz14144",
                    "aBc",
                    "AB",
                    "a",
                    "dsjkf50000",
                    "BCD443",
                    "aaa31",
                ],
                [
                    "a",
                    "AB",
                    "aBc",
                    "aaa31",
                    "BCD443",
                    "zzz14144",
                    "dsjkf50000",
                ]
            },
        };
        [Theory]
        [MemberData(nameof(SortComparison_Data))]
        public void SortComparison(string[] input, string[] expected)
        {
            input.Sort((s1, s2) => s1.Length.CompareTo(s2.Length)).ShouldBe(expected);
        }

        public static TheoryData<string[], StringComparison, string[]> SortComparer_Data => new()
        {
            {
                [
                    "zzz14144",
                    "aBc",
                    "AB",
                    "a",
                    "dsjkf50000",
                    "BCD443",
                    "aaa31",
                ],
                StringComparison.OrdinalIgnoreCase,
                [
                    "a",
                    "aaa31",
                    "AB",
                    "aBc",
                    "BCD443",
                    "dsjkf50000",
                    "zzz14144",
                ]
            },
        };
        [Theory]
        [MemberData(nameof(SortComparer_Data))]
        public void SortComparer(string[] input, StringComparison comparisonType, string[] expected)
        {
            input.Sort(StringComparer.FromComparison(comparisonType)).ShouldBe(expected);
        }

        public static TheoryData<string[], string[]> Reverse_Data => new()
        {
            {
                [
                    "zzz14144",
                    "aBc",
                    "AB",
                    "a",
                    "dsjkf50000",
                    "BCD443",
                    "aaa31",
                ],
                [
                    "aaa31",
                    "BCD443",
                    "dsjkf50000",
                    "a",
                    "AB",
                    "aBc",
                    "zzz14144",
                ]
            },
        };
        [Theory]
        [MemberData(nameof(Reverse_Data))]
        public void Reverse(string[] input, string[] expected)
        {
            input.Reverse().ShouldBe(expected);
        }

        [Fact]
        public void Get()
        {
            var arr = new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            };
            arr.Get(1).ShouldBe(24);
            arr.Get(1) = 25;
            arr[1].ShouldBe(25);

            arr.Get(-1).ShouldBe(3123);
            arr.Get(-1) = -2;
            arr[^1].ShouldBe(-2);
        }


        [Fact]
        public void GetOrDummy()
        {
            var arr = new long[] {
                43,24,8373,
            };
            arr.GetOrDummy(0).ShouldBe(43);
            arr.GetOrDummy(1).ShouldBe(24);
            arr.GetOrDummy(2).ShouldBe(8373);
            arr.GetOrDummy(-1, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(3, int.MinValue).ShouldBe(int.MinValue);
            arr.ShouldBe([43, 24, 8373]);

            arr.GetOrDummy(2) = 33;
            arr.GetOrDummy(3) = 55;
            arr.GetOrDummy(-1) = 66;

            arr.GetOrDummy(0).ShouldBe(43);
            arr.GetOrDummy(1).ShouldBe(24);
            arr.GetOrDummy(2).ShouldBe(33);
            arr.GetOrDummy(-1, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(3, int.MinValue).ShouldBe(int.MinValue);
            arr.ShouldBe([43, 24, 33]);
        }
        [Fact]
        public void GetOrDummySpan()
        {
            Span<long> arr = [
                43,
                24,
                8373,
            ];
            arr.GetOrDummy(0).ShouldBe(43);
            arr.GetOrDummy(1).ShouldBe(24);
            arr.GetOrDummy(2).ShouldBe(8373);
            arr.GetOrDummy(-1, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(3, int.MinValue).ShouldBe(int.MinValue);
            arr.ToArray().ShouldBe([43, 24, 8373]);

            arr.GetOrDummy(2) = 33;
            arr.GetOrDummy(3) = 55;
            arr.GetOrDummy(-1) = 66;

            arr.GetOrDummy(0).ShouldBe(43);
            arr.GetOrDummy(1).ShouldBe(24);
            arr.GetOrDummy(2).ShouldBe(33);
            arr.GetOrDummy(-1, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(3, int.MinValue).ShouldBe(int.MinValue);
            arr.ToArray().ShouldBe([43, 24, 33]);
        }
        [Fact]
        public void GetOrDummyReadOnlySpan()
        {
            ReadOnlySpan<long> arr = [
                43,
                24,
                8373,
            ];
            arr.GetOrDummy(0).ShouldBe(43);
            arr.GetOrDummy(1).ShouldBe(24);
            arr.GetOrDummy(2).ShouldBe(8373);
            arr.GetOrDummy(-1, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(3, int.MinValue).ShouldBe(int.MinValue);
        }

        [Fact]
        public void GetOrDummy2()
        {
            var arr = new long[][] {
                [43, 24, 8373],
                [-13, -4, 54],
            };
            arr.GetOrDummy(0, 0).ShouldBe(43);
            arr.GetOrDummy(0, 1).ShouldBe(24);
            arr.GetOrDummy(0, 2).ShouldBe(8373);
            arr.GetOrDummy(-1, 0, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(2, 0, int.MinValue).ShouldBe(int.MinValue);
            arr.GetOrDummy(1, -1, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(1, 3, int.MinValue).ShouldBe(int.MinValue);
            arr.GetOrDummy(-1, -1, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(2, 3, int.MinValue).ShouldBe(int.MinValue);
            arr[0].ShouldBe([43, 24, 8373]);
            arr[1].ShouldBe([-13, -4, 54]);

            arr.GetOrDummy(0, 2) = 33;
            arr.GetOrDummy(0, 3) = 55;
            arr.GetOrDummy(0, -1) = 66;

            arr.GetOrDummy(0, 0).ShouldBe(43);
            arr.GetOrDummy(0, 1).ShouldBe(24);
            arr.GetOrDummy(0, 2).ShouldBe(33);

            arr.GetOrDummy(1, 0).ShouldBe(-13);
            arr.GetOrDummy(1, 1).ShouldBe(-4);
            arr.GetOrDummy(1, 2).ShouldBe(54);

            arr.GetOrDummy(1, 2) = -3;
            arr.GetOrDummy(1, 3) = 55;
            arr.GetOrDummy(1, -1) = 66;

            arr.GetOrDummy(1, 0).ShouldBe(-13);
            arr.GetOrDummy(1, 1).ShouldBe(-4);
            arr.GetOrDummy(1, 2).ShouldBe(-3);

            arr[0].ShouldBe([43, 24, 33]);
            arr[1].ShouldBe([-13, -4, -3]);
        }


        [Fact]
        public void GetOrDummy3()
        {
            var arr = new long[][][] {
                [
                    [1, 2,],
                    [3, 4,],
                ],
                [
                    [5, 6,],
                    [7, 8,],
                ],
            };
            arr.GetOrDummy(0, 0, 0).ShouldBe(1);
            arr.GetOrDummy(0, 0, 1).ShouldBe(2);
            arr.GetOrDummy(0, 1, 0).ShouldBe(3);
            arr.GetOrDummy(0, 1, 1).ShouldBe(4);
            arr.GetOrDummy(1, 0, 0).ShouldBe(5);
            arr.GetOrDummy(1, 0, 1).ShouldBe(6);
            arr.GetOrDummy(1, 1, 0).ShouldBe(7);
            arr.GetOrDummy(1, 1, 1).ShouldBe(8);
            arr.GetOrDummy(-1, 0, 0, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(2, 0, 0, int.MinValue).ShouldBe(int.MinValue);
            arr.GetOrDummy(1, -1, 0, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(1, 2, 0, int.MinValue).ShouldBe(int.MinValue);
            arr.GetOrDummy(-1, -1, 0, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(2, 2, -1, int.MinValue).ShouldBe(int.MinValue);
            arr.GetOrDummy(0, 0, 2, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(0, 0, -1, int.MinValue).ShouldBe(int.MinValue);
            arr.GetOrDummy(1, 1, 2, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(1, 1, -1, int.MinValue).ShouldBe(int.MinValue);
            arr.GetOrDummy(1, 1, 2, int.MaxValue).ShouldBe(int.MaxValue);
            arr.GetOrDummy(1, 1, -1, int.MinValue).ShouldBe(int.MinValue);
            arr[0][0].ShouldBe([1, 2]);
            arr[0][1].ShouldBe([3, 4]);
            arr[1][0].ShouldBe([5, 6]);
            arr[1][1].ShouldBe([7, 8]);

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

            arr.GetOrDummy(0, 0, 0).ShouldBe(11);
            arr.GetOrDummy(0, 0, 1).ShouldBe(12);
            arr.GetOrDummy(0, 1, 0).ShouldBe(13);
            arr.GetOrDummy(0, 1, 1).ShouldBe(14);
            arr.GetOrDummy(1, 0, 0).ShouldBe(15);
            arr.GetOrDummy(1, 0, 1).ShouldBe(16);
            arr.GetOrDummy(1, 1, 0).ShouldBe(17);
            arr.GetOrDummy(1, 1, 1).ShouldBe(18);
            arr[0][0].ShouldBe([11, 12]);
            arr[0][1].ShouldBe([13, 14]);
            arr[1][0].ShouldBe([15, 16]);
            arr[1][1].ShouldBe([17, 18]);
        }


        [Fact]
        public void FindByBinarySearch()
        {
            var arr = new double[] {
                double.NegativeInfinity,
                -1e109,
                -1e19,
                -1.5,
                0,
                0,
                0,
                0,
                0,
                26,
                27,
                30,
            };
            arr.FindByBinarySearch(double.NegativeInfinity).ShouldBe(0);
            ((Span<double>)arr).FindByBinarySearch(double.NegativeInfinity).ShouldBe(0);
            ((ReadOnlySpan<double>)arr).FindByBinarySearch(double.NegativeInfinity).ShouldBe(0);

            arr.FindByBinarySearch(-1e200).ShouldBe(1);
            ((Span<double>)arr).FindByBinarySearch(-1e200).ShouldBe(1);
            ((ReadOnlySpan<double>)arr).FindByBinarySearch(-1e200).ShouldBe(1);

            arr.FindByBinarySearch(-1e109).ShouldBe(1);
            ((Span<double>)arr).FindByBinarySearch(-1e109).ShouldBe(1);
            ((ReadOnlySpan<double>)arr).FindByBinarySearch(-1e109).ShouldBe(1);

            arr.FindByBinarySearch(-10.0).ShouldBe(3);
            ((Span<double>)arr).FindByBinarySearch(-10.0).ShouldBe(3);
            ((ReadOnlySpan<double>)arr).FindByBinarySearch(-10.0).ShouldBe(3);

            arr.FindByBinarySearch(0.0).ShouldBeInRange(4, 8);
            ((Span<double>)arr).FindByBinarySearch(0.0).ShouldBeInRange(4, 8);
            ((ReadOnlySpan<double>)arr).FindByBinarySearch(0.0).ShouldBeInRange(4, 8);

            arr.FindByBinarySearch(1.0).ShouldBe(9);
            ((Span<double>)arr).FindByBinarySearch(1.0).ShouldBe(9);
            ((ReadOnlySpan<double>)arr).FindByBinarySearch(1.0).ShouldBe(9);

            arr.FindByBinarySearch(40.0).ShouldBe(12);
            ((Span<double>)arr).FindByBinarySearch(40.0).ShouldBe(12);
            ((ReadOnlySpan<double>)arr).FindByBinarySearch(40.0).ShouldBe(12);
        }
    }
}