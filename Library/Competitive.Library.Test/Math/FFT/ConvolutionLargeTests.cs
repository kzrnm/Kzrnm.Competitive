using AtCoder;
using System;
using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class ConvolutionLargeTests
    {
        private readonly struct Mod998244353 : IStaticMod
        {
            public uint Mod => 998244353;
            public bool IsPrime => true;
        }

        [HeavyFact]
        public void Large998244353_2_23_Alpha()
        {
            var len = NumberTheoreticTransform<Mod998244353>.NttLength();
            var a = Enumerable.Repeat(1, len).ToArray();
            var b = Enumerable.Repeat((0, 1), 1000).SelectMany(t => new[] { t.Item1, t.Item2 }).ToArray();
            var ret = ConvolutionLarge.Convolution<Mod998244353>(a, b);
            ret[0].Should().Be(0);
            for (int i = 1; i < 2000; i += 2)
            {
                var expected = (uint)(i + 1) >> 1;
                ret[i].Should().Be(expected);
                ret[i + 1].Should().Be(expected);
                ret[ret.Length - i - 1].Should().Be(expected);
                ret[ret.Length - i].Should().Be(expected);
            }
            ret.Skip(1999).SkipLast(1998).Should().AllBeEquivalentTo(1000);
        }

        [HeavyFact]
        public void Large998244353_2_24()
        {
            var len = NumberTheoreticTransform<Mod998244353>.NttLength();
            var a = Enumerable.Repeat(1, len).ToArray();
            var b = Enumerable.Repeat((0, 1), len / 2).SelectMany(t => new[] { t.Item1, t.Item2 }).ToArray();
            var ret = ConvolutionLarge.Convolution<Mod998244353>(a, b);
            ret[0].Should().Be(0);
            for (int i = 1; i < len; i += 2)
            {
                var expected = (uint)(i + 1) >> 1;
                ret[i].Should().Be(expected);
                ret[i + 1].Should().Be(expected);
                ret[ret.Length - i - 1].Should().Be(expected);
                ret[ret.Length - i].Should().Be(expected);
            }
        }

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

        public static TheoryData EmptyIntTestData => new TheoryData<int[], int[], uint[]>
        {
            { Array.Empty<int>(), Array.Empty<int>(), Array.Empty<uint>() },
            { Array.Empty<int>(), new int[]{ 1, 2 }, Array.Empty<uint>() },
            { new int[]{ 1, 2 }, Array.Empty<int>(), Array.Empty<uint>() },
            { new int[]{ 1 }, Array.Empty<int>(), Array.Empty<uint>() },
        };

        [Theory]
        [MemberData(nameof(EmptyIntTestData))]
        public void EmptyInt(int[] a, int[] b, uint[] expected)
        {
            ConvolutionLarge.Convolution<Mod998244353>(a, b).Should().Equal(expected);
        }

        [Fact]
        public void Small()
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

                    ConvolutionLarge.Convolution<Mod998244353>(a, b).Should().Equal(ConvNative(a, b, 998244353));
                }
        }
    }
}
