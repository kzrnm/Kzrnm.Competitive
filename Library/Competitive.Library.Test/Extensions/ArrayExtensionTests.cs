using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Extensions;

public class ArrayExtensionTests
{
    [Test]
    public async Task Fill()
    {
        await new string[100].Fill("🦈").Should().BeEquivalentOrderTo(Enumerable.Repeat("🦈", 100));
    }

    [Test]
    public async Task Sort()
    {
        var arr = Enumerable.Repeat(new Random(), 2000).Select(r => r.Next()).ToArray();
        await MemoryMarshal.Cast<int, long>(arr).ToArray().Sort().Should().BeInOrder();
    }

    public static IEnumerable<(string[], string[])> SortString_Data =>
    [
        (
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
        ),
    ];
    [Test]
    [MethodDataSource(nameof(SortString_Data))]
    public async Task SortString(string[] input, string[] expected)
    {
        await input.Sort().Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(string[], string[])> SortSelect_Data =>
    [
        (
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
        ),
    ];
    [Test]
    [MethodDataSource(nameof(SortSelect_Data))]
    public async Task SortSelect(string[] input, string[] expected)
    {
        await input.Sort(s => s.Length).Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(string[], string[])> SortComparison_Data =>
    [
        (
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
        ),
    ];
    [Test]
    [MethodDataSource(nameof(SortComparison_Data))]
    public async Task SortComparison(string[] input, string[] expected)
    {
        await input.Sort((s1, s2) => s1.Length.CompareTo(s2.Length)).Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(string[], StringComparison, string[])> SortComparer_Data =>
[
        (
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
        ),
    ];
    [Test]
    [MethodDataSource(nameof(SortComparer_Data))]
    public async Task SortComparer(string[] input, StringComparison comparisonType, string[] expected)
    {
        await input.Sort(StringComparer.FromComparison(comparisonType)).Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(string[], string[])> Reverse_Data =>
    [
        (
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
        ),
    ];
    [Test]
    [MethodDataSource(nameof(Reverse_Data))]
    public async Task Reverse(string[] input, string[] expected)
    {
        await input.Reverse().Should().BeEquivalentOrderTo(expected);
    }

    [Test]
    public async Task Get()
    {
        var arr = new long[] {
            43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
        };
        await arr.Get(1).Should().BeEqualTo(24);
        arr.Get(1) = 25;
        await arr[1].Should().BeEqualTo(25);

        await arr.Get(-1).Should().BeEqualTo(3123);
        arr.Get(-1) = -2;
        await arr[^1].Should().BeEqualTo(-2);
    }


    [Test, MultipleAssertions]
    public async Task GetOrDummy()
    {
        var arr = new long[] {
            43,24,8373,
        };
        await arr.GetOrDummy(0).Should().BeEqualTo(43);
        await arr.GetOrDummy(1).Should().BeEqualTo(24);
        await arr.GetOrDummy(2).Should().BeEqualTo(8373);
        await arr.GetOrDummy(-1, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(3, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.Should().BeEquivalentOrderTo([43L, 24, 8373]);

        arr.GetOrDummy(2) = 33;
        arr.GetOrDummy(3) = 55;
        arr.GetOrDummy(-1) = 66;

        await arr.GetOrDummy(0).Should().BeEqualTo(43);
        await arr.GetOrDummy(1).Should().BeEqualTo(24);
        await arr.GetOrDummy(2).Should().BeEqualTo(33);
        await arr.GetOrDummy(-1, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(3, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.Should().BeEquivalentOrderTo([43L, 24, 33]);
    }

    [Test]
    [Arguments(0, 43)]
    [Arguments(1, 24)]
    [Arguments(2, 8373)]
    [Arguments(-1, 0)]
    [Arguments(-2, 0)]
    public async Task GetOrDummySpan(int index, long expected)
    {
        Span<long> span = [43, 24, 8373,];
        await span.GetOrDummy(index).Should().BeEqualTo(expected);
    }

    [Test]
    [Arguments(0, int.MinValue, 43)]
    [Arguments(1, int.MinValue, 24)]
    [Arguments(2, int.MinValue, 8373)]
    [Arguments(-1, int.MaxValue, int.MaxValue)]
    [Arguments(-2, int.MinValue, int.MinValue)]
    public async Task GetOrDummySpanWithDummyValue(int index, long dummy, long expected)
    {
        Span<long> span = [43, 24, 8373,];
        await span.GetOrDummy(index, dummy).Should().BeEqualTo(expected);
    }

    [Test]
    [Arguments(0, 43)]
    [Arguments(1, 24)]
    [Arguments(2, 33)]
    [Arguments(-1, 0)]
    [Arguments(-2, 0)]
    public async Task GetOrDummySpanUpdated(int index, long expected)
    {
        Span<long> span = [43, 24, 8373,];
        span.GetOrDummy(2) = 33;
        span.GetOrDummy(3) = 55;
        span.GetOrDummy(-1) = 66;

        await span.GetOrDummy(index).Should().BeEqualTo(expected);
    }

    [Test]
    [Arguments(0, 43)]
    [Arguments(1, 24)]
    [Arguments(2, 8373)]
    [Arguments(-1, 0)]
    [Arguments(-2, 0)]
    public async Task GetOrDummyReadOnlySpan(int index, long expected)
    {
        ReadOnlySpan<long> span = [43, 24, 8373,];
        await span.GetOrDummy(index).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    public async Task GetOrDummy2()
    {
        var arr = new long[][] {
            [43, 24, 8373],
            [-13, -4, 54],
        };
        await arr.GetOrDummy(0, 0).Should().BeEqualTo(43);
        await arr.GetOrDummy(0, 1).Should().BeEqualTo(24);
        await arr.GetOrDummy(0, 2).Should().BeEqualTo(8373);
        await arr.GetOrDummy(-1, 0, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(2, 0, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.GetOrDummy(1, -1, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(1, 3, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.GetOrDummy(-1, -1, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(2, 3, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr[0].Should().BeEquivalentOrderTo([43L, 24, 8373]);
        await arr[1].Should().BeEquivalentOrderTo([-13L, -4, 54]);

        arr.GetOrDummy(0, 2) = 33;
        arr.GetOrDummy(0, 3) = 55;
        arr.GetOrDummy(0, -1) = 66;

        await arr.GetOrDummy(0, 0).Should().BeEqualTo(43);
        await arr.GetOrDummy(0, 1).Should().BeEqualTo(24);
        await arr.GetOrDummy(0, 2).Should().BeEqualTo(33);

        await arr.GetOrDummy(1, 0).Should().BeEqualTo(-13);
        await arr.GetOrDummy(1, 1).Should().BeEqualTo(-4);
        await arr.GetOrDummy(1, 2).Should().BeEqualTo(54);

        arr.GetOrDummy(1, 2) = -3;
        arr.GetOrDummy(1, 3) = 55;
        arr.GetOrDummy(1, -1) = 66;

        await arr.GetOrDummy(1, 0).Should().BeEqualTo(-13);
        await arr.GetOrDummy(1, 1).Should().BeEqualTo(-4);
        await arr.GetOrDummy(1, 2).Should().BeEqualTo(-3);

        await arr[0].Should().BeEquivalentOrderTo([43L, 24, 33]);
        await arr[1].Should().BeEquivalentOrderTo([-13L, -4, -3]);
    }


    [Test, MultipleAssertions]
    public async Task GetOrDummy3()
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
        await arr.GetOrDummy(0, 0, 0).Should().BeEqualTo(1);
        await arr.GetOrDummy(0, 0, 1).Should().BeEqualTo(2);
        await arr.GetOrDummy(0, 1, 0).Should().BeEqualTo(3);
        await arr.GetOrDummy(0, 1, 1).Should().BeEqualTo(4);
        await arr.GetOrDummy(1, 0, 0).Should().BeEqualTo(5);
        await arr.GetOrDummy(1, 0, 1).Should().BeEqualTo(6);
        await arr.GetOrDummy(1, 1, 0).Should().BeEqualTo(7);
        await arr.GetOrDummy(1, 1, 1).Should().BeEqualTo(8);
        await arr.GetOrDummy(-1, 0, 0, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(2, 0, 0, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.GetOrDummy(1, -1, 0, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(1, 2, 0, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.GetOrDummy(-1, -1, 0, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(2, 2, -1, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.GetOrDummy(0, 0, 2, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(0, 0, -1, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.GetOrDummy(1, 1, 2, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(1, 1, -1, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr.GetOrDummy(1, 1, 2, int.MaxValue).Should().BeEqualTo(int.MaxValue);
        await arr.GetOrDummy(1, 1, -1, int.MinValue).Should().BeEqualTo(int.MinValue);
        await arr[0][0].Should().BeEquivalentOrderTo([1L, 2]);
        await arr[0][1].Should().BeEquivalentOrderTo([3L, 4]);
        await arr[1][0].Should().BeEquivalentOrderTo([5L, 6]);
        await arr[1][1].Should().BeEquivalentOrderTo([7L, 8]);

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

        await arr.GetOrDummy(0, 0, 0).Should().BeEqualTo(11);
        await arr.GetOrDummy(0, 0, 1).Should().BeEqualTo(12);
        await arr.GetOrDummy(0, 1, 0).Should().BeEqualTo(13);
        await arr.GetOrDummy(0, 1, 1).Should().BeEqualTo(14);
        await arr.GetOrDummy(1, 0, 0).Should().BeEqualTo(15);
        await arr.GetOrDummy(1, 0, 1).Should().BeEqualTo(16);
        await arr.GetOrDummy(1, 1, 0).Should().BeEqualTo(17);
        await arr.GetOrDummy(1, 1, 1).Should().BeEqualTo(18);
        await arr[0][0].Should().BeEquivalentOrderTo([11L, 12]);
        await arr[0][1].Should().BeEquivalentOrderTo([13L, 14]);
        await arr[1][0].Should().BeEquivalentOrderTo([15L, 16]);
        await arr[1][1].Should().BeEquivalentOrderTo([17L, 18]);
    }


    [Test, MultipleAssertions]
    public async Task FindByBinarySearch()
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
        await arr.FindByBinarySearch(double.NegativeInfinity).Should().BeEqualTo(0);
        await ((Span<double>)arr).FindByBinarySearch(double.NegativeInfinity).Should().BeEqualTo(0);
        await ((ReadOnlySpan<double>)arr).FindByBinarySearch(double.NegativeInfinity).Should().BeEqualTo(0);

        await arr.FindByBinarySearch(-1e200).Should().BeEqualTo(1);
        await ((Span<double>)arr).FindByBinarySearch(-1e200).Should().BeEqualTo(1);
        await ((ReadOnlySpan<double>)arr).FindByBinarySearch(-1e200).Should().BeEqualTo(1);

        await arr.FindByBinarySearch(-1e109).Should().BeEqualTo(1);
        await ((Span<double>)arr).FindByBinarySearch(-1e109).Should().BeEqualTo(1);
        await ((ReadOnlySpan<double>)arr).FindByBinarySearch(-1e109).Should().BeEqualTo(1);

        await arr.FindByBinarySearch(-10.0).Should().BeEqualTo(3);
        await ((Span<double>)arr).FindByBinarySearch(-10.0).Should().BeEqualTo(3);
        await ((ReadOnlySpan<double>)arr).FindByBinarySearch(-10.0).Should().BeEqualTo(3);

        await arr.FindByBinarySearch(0.0).Should().BeBetween(4, 8);
        await ((Span<double>)arr).FindByBinarySearch(0.0).Should().BeBetween(4, 8);
        await ((ReadOnlySpan<double>)arr).FindByBinarySearch(0.0).Should().BeBetween(4, 8);

        await arr.FindByBinarySearch(1.0).Should().BeEqualTo(9);
        await ((Span<double>)arr).FindByBinarySearch(1.0).Should().BeEqualTo(9);
        await ((ReadOnlySpan<double>)arr).FindByBinarySearch(1.0).Should().BeEqualTo(9);

        await arr.FindByBinarySearch(40.0).Should().BeEqualTo(12);
        await ((Span<double>)arr).FindByBinarySearch(40.0).Should().BeEqualTo(12);
        await ((ReadOnlySpan<double>)arr).FindByBinarySearch(40.0).Should().BeEqualTo(12);
    }
}