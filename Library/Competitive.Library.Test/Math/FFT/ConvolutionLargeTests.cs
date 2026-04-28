using AtCoder;

namespace Kzrnm.Competitive.Testing.MathNS;

public class ConvolutionLargeTests
{
#pragma warning disable IDE0079
#pragma warning disable xUnit1004 // Test methods should not be skipped
#pragma warning restore IDE0079
    public static bool LibraryTest
#if LIBRARY
        => true;
#else
        => false;
#endif
    private readonly struct Mod998244353 : IStaticMod
    {
        public uint Mod => 998244353;
        public bool IsPrime => true;
    }

    [Test, MultipleAssertions]
    [Skip("重いので飛ばす")]
    public async Task Large998244353_2_23_Alpha()
    {
        var len = NumberTheoreticTransform<Mod998244353>.NttLength();
        var a = Enumerable.Repeat(1, len).ToArray();
        var b = Enumerable.Repeat((0, 1), 1000).SelectMany(t => new[] { t.Item1, t.Item2 }).ToArray();
        var ret = ConvolutionLarge.Convolution<Mod998244353>(a, b);
        await ret[0].Should().BeEqualTo(0u);
        for (int i = 1; i < 2000; i += 2)
        {
            var expected = (uint)(i + 1) >> 1;
            await ret[i].Should().BeEqualTo(expected);
            await ret[i + 1].Should().BeEqualTo(expected);
            await ret[ret.Length - i - 1].Should().BeEqualTo(expected);
            await ret[^i].Should().BeEqualTo(expected);
        }
        await ret.Skip(1999).SkipLast(1998).Should().All(v => v == 1000);
    }

    [Test, MultipleAssertions]
    [Skip("重いので飛ばす")]
    public async Task Large998244353_2_24()
    {
        var len = NumberTheoreticTransform<Mod998244353>.NttLength();
        var a = Enumerable.Repeat(1, len).ToArray();
        var b = Enumerable.Repeat((0, 1), len / 2).SelectMany(t => new[] { t.Item1, t.Item2 }).ToArray();
        var ret = ConvolutionLarge.Convolution<Mod998244353>(a, b);
        await ret[0].Should().BeEqualTo(0u);
        for (int i = 1; i < len; i += 2)
        {
            var expected = (uint)(i + 1) >> 1;
            await ret[i].Should().BeEqualTo(expected);
            await ret[i + 1].Should().BeEqualTo(expected);
            await ret[ret.Length - i - 1].Should().BeEqualTo(expected);
            await ret[^i].Should().BeEqualTo(expected);
        }
    }
#pragma warning disable IDE0079
#pragma warning restore xUnit1004 // Test methods should not be skipped
#pragma warning restore IDE0079

    static uint[] ConvNative(uint[] a, uint[] b, long mod)
    {
        int n = a.Length, m = b.Length;
        var c = new long[n + m - 1];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                c[i + j] += (long)(new BigInteger(a[i]) * b[j] % mod);
                if (c[i + j] >= mod) c[i + j] -= mod;
            }
        }
        return c.Select(n => (uint)n).ToArray();
    }

    public static IEnumerable<(int[], int[], uint[])> EmptyIntTestData =>
    [
        ([], [], []),
        ([], [1, 2], []),
        ([1, 2], [], []),
        ([1], [], []),
    ];

    [Test]
    [MethodDataSource(nameof(EmptyIntTestData))]
    public async Task EmptyInt(int[] a, int[] b, uint[] expected)
    {
        await ConvolutionLarge.Convolution<Mod998244353>(a, b).Should().BeEquivalentOrderTo(expected);
    }

    [Test, MultipleAssertions]
    public async Task Small()
    {
        var rnd = new Random(42);

        for (int n = 1; n < 70; n++)
            for (int m = 1; m < 70; m++)
            {
                var a = new uint[n];
                var b = new uint[m];
                for (int i = 0; i < n; i++)
                {
                    a[i] = rnd.NextUInt() % 998244353;
                }
                for (int i = 0; i < m; i++)
                {
                    b[i] = rnd.NextUInt() % 998244353;
                }

                await ConvolutionLarge.Convolution<Mod998244353>(a, b).Should().BeEquivalentOrderTo(ConvNative(a, b, 998244353));
            }
    }
}