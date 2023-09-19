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

#if LIBRARY
#pragma warning disable xUnit1004 // Test methods should not be skipped
        [Fact(Skip = "重いので")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
#else
        [Fact]
#endif
        public void Large998244353_2_24()
        {
            var len = NumberTheoreticTransform<Mod998244353>.NttLength();
            var a = Enumerable.Repeat(1, len).ToArray();
            var ret = ConvolutionLarge.Convolution<Mod998244353>(a, a);
            for (int i = 0; i < len; i++)
            {
                var expected = (uint)(i + 1);
                ret[i].Should().Be(expected);
                ret[ret.Length - i - 1].Should().Be(expected);
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
