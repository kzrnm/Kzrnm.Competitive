using AtCoder;
using FluentAssertions;
using System;
using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class ConvolutionTests
    {
        private struct Mod1000000000 : IStaticMod
        {
            public uint Mod => 1000000000;
            public bool IsPrime => false;
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
        [Trait("Category", "Empty")]
        [MemberData(nameof(EmptyIntTestData))]
        public void EmptyInt(int[] a, int[] b, uint[] expected)
        {
            for (int i = 2; i < 10; i++)
                ConvolutionAnyMod.Convolution(a, b, i).Should().Equal(expected);
        }

        [Fact]
        public void Mid()
        {
            var rnd = new Random(42);
            int n = 123, m = 234;
            var a = new uint[n];
            var b = new uint[m];
            for (int i = 0; i < n; i++)
            {
                a[i] = rnd.NextUInt();
            }
            for (int i = 0; i < m; i++)
            {
                b[i] = rnd.NextUInt();
            }
            ConvolutionAnyMod.Convolution<Mod998244353>(a, b).Should().Equal(ConvNative(a, b, 998244353));
            ConvolutionAnyMod.Convolution<Mod1000000000>(a, b).Should().Equal(ConvNative(a, b, 1000000000));
            ConvolutionAnyMod.Convolution<Mod1000000007>(a, b).Should().Equal(ConvNative(a, b, 1000000007));
        }


        [Fact]
        public void Small()
        {
            var rnd = new Random(42);
            int n = 123, m = 234;
            var a = new uint[n];
            var b = new uint[m];
            for (int i = 0; i < n; i++)
            {
                a[i] = rnd.NextUInt();
            }
            for (int i = 0; i < m; i++)
            {
                b[i] = rnd.NextUInt();
            }
            for (int i = 0; i < 50; i++)
                ConvolutionAnyMod.Convolution(a, b, 5 + i).Should().Equal(ConvNative(a, b, 5 + i));
        }

        [Fact]
        public void Large()
        {
            var rnd = new Random(42);
            int n = 123, m = 234;
            var a = new uint[n];
            var b = new uint[m];
            for (int i = 0; i < n; i++)
            {
                a[i] = rnd.NextUInt();
            }
            for (int i = 0; i < m; i++)
            {
                b[i] = rnd.NextUInt();
            }
            for (int i = 0; i < 50; i++)
                ConvolutionAnyMod.Convolution(a, b, 1000000005 + i).Should().Equal(ConvNative(a, b, 1000000005 + i));
        }

        [Fact]
        public void Simple()
        {
            var rnd = new Random(42);
            for (int c = 0; c < 100; c++)
                for (int n = 1; n < 10; n++)
                {
                    for (int m = 1; m < 10; m++)
                    {
                        var a = new uint[n];
                        var b = new uint[m];

                        for (int i = 0; i < n; i++)
                        {
                            a[i] = rnd.NextUInt();
                        }
                        for (int i = 0; i < m; i++)
                        {
                            b[i] = rnd.NextUInt();
                        }
                        ConvolutionAnyMod.Convolution(a, b, 1000000000 + c).Should().Equal(ConvNative(a, b, 1000000000 + c));
                    }
                }
        }

        private readonly struct Mod113 : IStaticMod
        {
            public uint Mod => 113;
            public bool IsPrime => true;
        }
        [Fact]
        public void Simpl113()
        {
            var rnd = new Random(42);
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new uint[n];
                    var b = new uint[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = rnd.NextUInt();
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = rnd.NextUInt();
                    }
                    ConvolutionAnyMod.Convolution<Mod113>(a, b).Should().Equal(ConvNative(a, b, 113));
                }
            }
        }
    }
}
