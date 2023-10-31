using AtCoder;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class ConvolutionTests
    {
        private readonly struct Mod113 : IStaticMod
        {
            public uint Mod => 113;
            public bool IsPrime => true;
        }
        private readonly struct Mod1000000000 : IStaticMod
        {
            public uint Mod => 1000000000;
            public bool IsPrime => false;
        }
        private readonly struct Mod1000000007 : IStaticMod
        {
            public uint Mod => 1000000007;
            public bool IsPrime => true;
        }

        private readonly struct Mod998244353 : IStaticMod
        {
            public uint Mod => 998244353;
            public bool IsPrime => true;
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
                NumberTheoreticTransform.Convolution(a, b, i).Should().Equal(expected);
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
            NumberTheoreticTransform.Convolution<Mod998244353>(a, b).Should().Equal(ConvNative(a, b, 998244353));
            NumberTheoreticTransform.Convolution<Mod1000000000>(a, b).Should().Equal(ConvNative(a, b, 1000000000));
            NumberTheoreticTransform.Convolution<Mod1000000007>(a, b).Should().Equal(ConvNative(a, b, 1000000007));
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
                NumberTheoreticTransform.Convolution(a, b, 5 + i).Should().Equal(ConvNative(a, b, 5 + i));
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
                NumberTheoreticTransform.Convolution(a, b, 1000000005 + i).Should().Equal(ConvNative(a, b, 1000000005 + i));
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
                        NumberTheoreticTransform.Convolution(a, b, 1000000000 + c).Should().Equal(ConvNative(a, b, 1000000000 + c));
                    }
                }
        }

        [Fact]
        public void Simple113()
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
                    NumberTheoreticTransform.Convolution<Mod113>(a, b).Should().Equal(ConvNative(a, b, 113));
                }
            }
        }


        [Fact]
        public void ULong()
        {
            var arr = new UInt128[10];
            Random.Shared.NextBytes(MemoryMarshal.AsBytes(arr.AsSpan()));
            var brr = System.Runtime.CompilerServices.Unsafe.As<Int128[]>(arr);
            var crr = new Int128[10];
            for (int i = 0; i < crr.Length; i++)
            {
                crr[i] = brr[i];
            }
            var rnd = new Random(42);
            for (int n = 1; n < 65; n++)
            {
                for (int m = 1; m < 65; m++)
                {
                    var a = new ulong[n];
                    var b = new ulong[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = (uint)rnd.Next(int.MaxValue);
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = (uint)rnd.Next(int.MaxValue);
                    }
                    NumberTheoreticTransform.ConvolutionULong(a, b).Should().Equal(ConvNative(a, b));
                }
            }

            static ulong[] ConvNative(ulong[] a, ulong[] b)
            {
                int n = a.Length, m = b.Length;
                var c = new ulong[n + m - 1];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        c[i + j] += (ulong)(new BigInteger(a[i]) * b[j]);
                    }
                }
                return c;
            }
        }
        [Fact]
        public void UInt128()
        {
            var rnd = new Random(42);
            for (int n = 1; n < 65; n++)
            {
                for (int m = 1; m < 65; m++)
                {
                    var a = new UInt128[n];
                    var b = new UInt128[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = rnd.NextUInt();
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = rnd.NextUInt();
                    }
                    NumberTheoreticTransform.ConvolutionUInt128(a, b).Should().Equal(ConvNative(a, b));
                }
            }

            static UInt128[] ConvNative(UInt128[] a, UInt128[] b)
            {
                int n = a.Length, m = b.Length;
                var c = new UInt128[n + m - 1];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                        unchecked
                        {
                            c[i + j] += (a[i] * b[j]);
                        }
                }
                return c;
            }
        }
    }
}
