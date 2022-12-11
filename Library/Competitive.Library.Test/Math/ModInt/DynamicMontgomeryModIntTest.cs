using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class DynamicMontgomeryModIntTests
    {
        public DynamicMontgomeryModIntTests()
        {
            DynamicMontgomeryModInt<Mod1ID>.Mod = 1;
            DynamicMontgomeryModInt<ModID11>.Mod = 11;
            DynamicMontgomeryModInt<ModID1000000007>.Mod = 1000000007;
            DynamicMontgomeryModInt<ModID998244353>.Mod = 998244353;
            DynamicMontgomeryModInt<IncrementID>.Mod = 11;
            DynamicMontgomeryModInt<ConstructorID>.Mod = 11;
            DynamicMontgomeryModInt<MemoryID>.Mod = 101;
        }
        private static long Gcd(long a, long b)
        {
            if (b == 0) return a;
            return Gcd(b, a % b);
        }

        private struct Mod1ID { }
        private struct ModID11 { }
        private struct ConstructorID { }
        private struct MemoryID { }
        private struct ModID1000000007 { }
        private struct ModID998244353 { }
        [Fact]
        public void Mod1()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    (new DynamicMontgomeryModInt<Mod1ID>(i) * j).Value.Should().Be(0);
                }
            }
            (new DynamicMontgomeryModInt<Mod1ID>(1234) + 5678).Value.Should().Be(0);
            (new DynamicMontgomeryModInt<Mod1ID>(1234) - 5678).Value.Should().Be(0);
            (new DynamicMontgomeryModInt<Mod1ID>(1234) * 5678).Value.Should().Be(0);
            (new DynamicMontgomeryModInt<Mod1ID>(0).Inv()).Value.Should().Be(0);
        }

        [Fact]
        public void Inv()
        {
            RunStatic<ModID11>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID998244353>();

            static void RunStatic<T>() where T : struct
            {
                var mod = DynamicMontgomeryModInt<T>.Mod;
                var max = Math.Min(100000, mod);
                for (int i = 1; i < max; i++)
                {
                    if (Gcd(i, mod) != 1) continue;
                    int x = new DynamicMontgomeryModInt<T>(i).Inv().Value;
                    ((long)x * i % mod).Should().Be(1);
                }
            }
        }

        private struct IncrementID { }
        [Fact]
        public void Increment()
        {
            DynamicMontgomeryModInt<IncrementID> a;
            a = 8;
            (++a).Value.Should().Be(9);
            (++a).Value.Should().Be(10);
            (++a).Value.Should().Be(0);
            (++a).Value.Should().Be(1);
            a = 3;
            (--a).Value.Should().Be(2);
            (--a).Value.Should().Be(1);
            (--a).Value.Should().Be(0);
            (--a).Value.Should().Be(10);
            a = 8;
            (a++).Value.Should().Be(8);
            (a++).Value.Should().Be(9);
            (a++).Value.Should().Be(10);
            (a++).Value.Should().Be(0);
            a.Value.Should().Be(1);
            a = 3;
            (a--).Value.Should().Be(3);
            (a--).Value.Should().Be(2);
            (a--).Value.Should().Be(1);
            (a--).Value.Should().Be(0);
            a.Value.Should().Be(10);
        }

        [Fact]
        public void ConstructorStatic()
        {
            new DynamicMontgomeryModInt<ConstructorID>(3).Value.Should().Be(3);
            new DynamicMontgomeryModInt<ConstructorID>(-10).Value.Should().Be(1);
            (1 + new DynamicMontgomeryModInt<ConstructorID>(1)).Value.Should().Be(2);
        }

        [Fact]
        public void MemoryStatic()
        {
            var mt = new Random(227);
            for (int n = 0; n < 100; n++)
            {
                var arr = new DynamicMontgomeryModInt<MemoryID>[n];
                var expected = new int[n];
                for (int i = 0; i < n; i++)
                {
                    var v = mt.Next();
                    arr[i] = v;
                    expected[i] = v % 101;
                }
                arr.Select(m => m.Value).ToArray()
                    .Should().Equal(expected);
            }
        }

        [Fact]
        public void Minus()
        {
            RunStatic<ModID11>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID998244353>();

            static void RunStatic<T>() where T : struct
            {
                var mod = DynamicMontgomeryModInt<T>.Mod;
                var max = Math.Min(100000, mod);
                for (int i = 0; i < max; i++)
                {
                    int x = (-new DynamicMontgomeryModInt<T>(i)).Value;
                    x.Should().Be((mod - i) % mod);
                }
            }
        }
    }
}
