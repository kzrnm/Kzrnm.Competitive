using AtCoder;
using System;
using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.Bit.SubsetDp
{
    public class XorConvolutionTests
    {

        [Fact]
        public void UInt32()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                int n = rnd.Next(1, 10);
                int len = 1 << n;
                var a = new uint[len];
                var b = new uint[len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = (uint)rnd.Next(100);
                    b[i] = (uint)rnd.Next(100);
                }
                XorConvolution.Convolution(a, b).ShouldBe(Naive(a, b));
            }
        }

        [Fact]
        public void Long()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                int n = rnd.Next(1, 10);
                int len = 1 << n;
                var a = new long[len];
                var b = new long[len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.Next(int.MaxValue / len);
                    b[i] = rnd.Next(int.MaxValue / len);
                }
                XorConvolution.Convolution(a, b).ShouldBe(Naive(a, b));
            }
        }

        [Fact]
        public void Mod1000000007()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                int n = rnd.Next(1, 10);
                int len = 1 << n;
                var a = new StaticModInt<Mod1000000007>[len];
                var b = new StaticModInt<Mod1000000007>[len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.Next();
                    b[i] = rnd.Next();
                }
                XorConvolution.Convolution(a, b).ShouldBe(Naive(a, b));
            }
        }

        [Fact]
        public void Mod998244353()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                int n = rnd.Next(1, 10);
                int len = 1 << n;
                var a = new MontgomeryModInt<Mod998244353>[len];
                var b = new MontgomeryModInt<Mod998244353>[len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.Next();
                    b[i] = rnd.Next();
                }
                XorConvolution.Convolution(a, b).ShouldBe(Naive(a, b));
            }
        }

        [Fact]
        public void Double()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                int n = rnd.Next(1, 10);
                int len = 1 << n;
                var a = new double[len];
                var b = new double[len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.NextDouble();
                    b[i] = rnd.NextDouble();
                }
                XorConvolution.Convolution(a, b)
                    .Zip(Naive(a, b), (a, b) => Math.Abs(a - b))
                    .ShouldAllBe(diff => diff < 1e-10);
            }
        }

        static T[] Naive<T>(T[] a, T[] b) where T : INumberBase<T>
        {
            var f = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < b.Length; j++)
                    f[i ^ j] += a[i] * b[j];
            return f;
        }
    }
}
