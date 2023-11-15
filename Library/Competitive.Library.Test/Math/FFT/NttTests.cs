using AtCoder;
using System;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class NttTests
    {
        static MontgomeryModInt<Mod998244353>[] ConvNative(MontgomeryModInt<Mod998244353>[] a, MontgomeryModInt<Mod998244353>[] b)
        {
            int n = a.Length, m = b.Length;
            var c = new MontgomeryModInt<Mod998244353>[n + m - 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    c[i + j] += a[i] * b[j];
                }
            }
            return c;
        }

        [Theory]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(128)]
        [InlineData(1024)]
        [InlineData(2048)]
        public void Ntt(int n)
        {
            var rnd = new Random(42);
            var a = new MontgomeryModInt<Mod998244353>[n];
            for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
            var b = (MontgomeryModInt<Mod998244353>[])a.Clone();
            NumberTheoreticTransform<Mod998244353>.Ntt(a);
            NumberTheoreticTransform<Mod998244353>.NttLogical(b);
            a.Should().Equal(b);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(128)]
        [InlineData(1024)]
        [InlineData(2048)]
        public void INtt(int n)
        {
            var rnd = new Random(42);
            var a = new MontgomeryModInt<Mod998244353>[n];
            for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
            var b = (MontgomeryModInt<Mod998244353>[])a.Clone();
            NumberTheoreticTransform<Mod998244353>.INtt(a);
            NumberTheoreticTransform<Mod998244353>.INttLogical(b);
            a.Should().Equal(b);
        }

        [Theory]
        [InlineData(123, 234)]
        [InlineData(1234, 2345)]
        [InlineData(1235, 2345)]
        public void Multiply(int n, int m)
        {
            var rnd = new Random(42);
            var a = new MontgomeryModInt<Mod998244353>[n];
            var b = new MontgomeryModInt<Mod998244353>[m];
            for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
            for (int i = 0; i < m; i++) b[i] = rnd.NextUInt();
            var expected = ConvNative(a, b);
            NumberTheoreticTransform<Mod998244353>.MultiplyLogical(a, b).Should().Equal(expected);
            NumberTheoreticTransform<Mod998244353>.Multiply(a, b).Should().Equal(expected);
        }
    }
}
