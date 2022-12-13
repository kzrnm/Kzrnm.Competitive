using AtCoder;
using FluentAssertions;
using System;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class NttTests
    {
        static StaticModInt<Mod998244353>[] ConvNative(StaticModInt<Mod998244353>[] a, StaticModInt<Mod998244353>[] b)
        {
            int n = a.Length, m = b.Length;
            var c = new StaticModInt<Mod998244353>[n + m - 1];
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
        public void Ntt(int n)
        {
            var rnd = new Random(42);
            var a = new StaticModInt<Mod998244353>[n];
            for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
            var b = (StaticModInt<Mod998244353>[])a.Clone();
            NumberTheoreticTransform<Mod998244353>.Ntt(a);
            NumberTheoreticTransform<Mod998244353>.NttLogical(b);
            a.Should().Equal(b);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(128)]
        public void INtt(int n)
        {
            var rnd = new Random(42);
            var a = new StaticModInt<Mod998244353>[n];
            for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
            var b = (StaticModInt<Mod998244353>[])a.Clone();
            NumberTheoreticTransform<Mod998244353>.INtt(a);
            NumberTheoreticTransform<Mod998244353>.INttLogical(b);
            a.Should().Equal(b);
        }

        [Fact]
        public void Multiply()
        {
            var rnd = new Random(42);
            int n = 123, m = 234;
            var a = new StaticModInt<Mod998244353>[n];
            var b = new StaticModInt<Mod998244353>[m];
            for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
            for (int i = 0; i < m; i++) b[i] = rnd.NextUInt();
            var expected = ConvNative(a, b);
            NumberTheoreticTransform<Mod998244353>.MultiplyLogical(a, b).Should().Equal(expected);
            NumberTheoreticTransform<Mod998244353>.Multiply(a, b).Should().Equal(expected);
        }
    }
}
