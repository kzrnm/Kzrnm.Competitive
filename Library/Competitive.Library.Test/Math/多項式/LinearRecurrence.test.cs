using AtCoder;
using FluentAssertions;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class LinearRecurrenceTest
    {
        private struct DMod : IDynamicModID { }
        private static StaticModInt<T>[] NativeDp<T>(Span<StaticModInt<T>> a, Span<StaticModInt<T>> c) where T : struct, IStaticMod
        {
            var res = new StaticModInt<T>[1000];
            a.CopyTo(res);
            for (int k = a.Length; k < res.Length; k++)
                for (int i = 0; i < c.Length; i++)
                {
                    res[k] += c[i] * res[k - i - 1];
                }
            return res;
        }

        [Fact]
        public void Mod1000000007()
        {
            var rnd = new Random(42);
            for (int n = 2; n < 10; n++)
            {
                var arrInt = rnd.NextIntArray(n, 0, 1000000007);
                var crrInt = rnd.NextIntArray(n, 0, 1000000007);
                var arrUInt = MemoryMarshal.Cast<int, uint>(arrInt);
                var crrUInt = MemoryMarshal.Cast<int, uint>(crrInt);
                var arrModInt = MemoryMarshal.Cast<int, StaticModInt<Mod1000000007>>(arrInt);
                var crrModInt = MemoryMarshal.Cast<int, StaticModInt<Mod1000000007>>(crrInt);
                var expected = NativeDp(arrModInt, crrModInt);
                for (int l = 0; l < 40; l++)
                {
                    LinearRecurrence.Kitamasa<Mod1000000007>(arrInt, crrInt, l).Should().Be(expected[l]);
                    LinearRecurrence.Kitamasa<Mod1000000007>(arrUInt, crrUInt, l).Should().Be(expected[l]);
                    LinearRecurrence.Kitamasa(arrModInt, crrModInt, l).Should().Be(expected[l]);
                }
            }
        }

        [Fact]
        public void Mod998244353()
        {
            var rnd = new Random(42);
            for (int n = 2; n < 10; n++)
            {
                var arrInt = rnd.NextIntArray(n, 0, 998244353);
                var crrInt = rnd.NextIntArray(n, 0, 998244353);
                var arrUInt = MemoryMarshal.Cast<int, uint>(arrInt);
                var crrUInt = MemoryMarshal.Cast<int, uint>(crrInt);
                var arrModInt = MemoryMarshal.Cast<int, StaticModInt<Mod998244353>>(arrInt);
                var crrModInt = MemoryMarshal.Cast<int, StaticModInt<Mod998244353>>(crrInt);
                var expected = NativeDp(arrModInt, crrModInt);
                for (int l = 0; l < 40; l++)
                {
                    LinearRecurrence.Kitamasa<Mod998244353>(arrInt, crrInt, l).Should().Be(expected[l]);
                    LinearRecurrence.Kitamasa<Mod998244353>(arrUInt, crrUInt, l).Should().Be(expected[l]);
                    LinearRecurrence.Kitamasa(arrModInt, crrModInt, l).Should().Be(expected[l]);
                }
            }
        }

        [Fact]
        public void Fibonacci()
        {
            var rnd = new Random(42);
            for (int n = 2; n < 200; n++)
            {
                var a0 = rnd.Next();
                var a1 = rnd.Next();
                var N = (long)rnd.NextUInt() << 24;
                RunTest<Mod998244353>(a0, a1, N);
                RunTest<Mod1000000007>(a0, a1, N);
            }


            void RunTest<T>(int a0, int a1, long n) where T : struct, IStaticMod
            {
                var mat = new Matrix2x2<StaticModInt<T>, StaticModIntOperator<T>>(
                    (0, 1),
                    (1, 1)
                );

                LinearRecurrence.Kitamasa<T>(
                    stackalloc StaticModInt<T>[2] { a0, a1 },
                    stackalloc StaticModInt<T>[2] { 1, 1 }, n)
                    .Should().Be(mat.Pow(n).Multiply(a0, a1).v0);
            }
        }
    }
}
