using AtCoder;
using System;
using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.Bit.SubsetDp
{
    public class XorConvolutionTests
    {
        [Fact]
        public void Mod1000000007()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                var len = rnd.Next(1, 10);
                var a = new StaticModInt<Mod1000000007>[1 << len];
                var b = new StaticModInt<Mod1000000007>[1 << len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.Next();
                    b[i] = rnd.Next();
                }
                XorConvolution.Convolution(a, b).Should().Equal(Naive(a, b));
            }
        }

        [Fact]
        public void Mod998244353()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                var len = rnd.Next(1, 10);
                var a = new MontgomeryModInt<Mod998244353>[1 << len];
                var b = new MontgomeryModInt<Mod998244353>[1 << len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.Next();
                    b[i] = rnd.Next();
                }
                XorConvolution.Convolution(a, b).Should().Equal(Naive(a, b));
            }
        }

        [Fact]
        public void Double()
        {
            var rnd = new Random(227);
            for (int q = 100; q >= 0; q--)
            {
                var len = rnd.Next(1, 10);
                var a = new double[1 << len];
                var b = new double[1 << len];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.NextDouble();
                    b[i] = rnd.NextDouble();
                }
                XorConvolution.Convolution(a, b)
                    .Zip(Naive(a, b), (a, b) => Math.Abs(a - b))
                    .Should().AllSatisfy(diff =>
                    {
                        diff.Should().BeLessThan(1e-10);
                    });
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
