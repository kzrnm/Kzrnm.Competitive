using AtCoder;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class MontgomeryModIntTests
    {
        private static long Gcd(long a, long b)
        {
            if (b == 0) return a;
            return Gcd(b, a % b);
        }

        private struct Mod1ID : IStaticMod
        {
            public uint Mod => 1;
            public bool IsPrime => false;
        }
        [Fact]
        public void Mod1()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    (new MontgomeryModInt<Mod1ID>(i) * j).Value.Should().Be(0);
                }
            }
            (new MontgomeryModInt<Mod1ID>(1234) + 5678).Value.Should().Be(0);
            (new MontgomeryModInt<Mod1ID>(1234) - 5678).Value.Should().Be(0);
            (new MontgomeryModInt<Mod1ID>(1234) * 5678).Value.Should().Be(0);
            //(new LazyMontgomeryModInt<Mod1ID>(1234).Pow(5678)).Value.Should().Be(0); // faild in Debug.Assert
            (new MontgomeryModInt<Mod1ID>(0).Inv()).Value.Should().Be(0);
        }

        private struct ModID11 : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        private struct ModID1000000007 : IStaticMod
        {
            public uint Mod => 1000000007;
            public bool IsPrime => false;
        }
        private struct ModID998244353 : IStaticMod
        {
            public uint Mod => 998244353;
            public bool IsPrime => true;
        }
        [Fact]
        public void Inv()
        {
            RunStatic<ModID11>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID998244353>();

            static void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var max = Math.Min(100000, mod);
                for (int i = 1; i < max; i++)
                {
                    if (!new T().IsPrime && Gcd(i, mod) != 1) continue;
                    int x = new MontgomeryModInt<T>(i).Inv().Value;
                    ((long)x * i % mod).Should().Be(1);
                }
            }
        }

        private struct IncrementID : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        [Fact]
        public void Increment()
        {
            MontgomeryModInt<IncrementID> a;
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

        private struct ConstructorID : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        [Fact]
        public void ConstructorStatic()
        {
            new MontgomeryModInt<ConstructorID>(3).Value.Should().Be(3);
            new MontgomeryModInt<ConstructorID>(-10).Value.Should().Be(1);
            (1 + new MontgomeryModInt<ConstructorID>(1)).Value.Should().Be(2);
        }

        private struct MemoryID : IStaticMod
        {
            public uint Mod => 101;
            public bool IsPrime => true;
        }

        [Fact]
        public void MemoryStatic()
        {
            var mt = new Random(227);
            for (int n = 0; n < 100; n++)
            {
                var arr = new MontgomeryModInt<MemoryID>[n];
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

            static void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var max = Math.Min(100000, mod);
                for (int i = 0; i < max; i++)
                {
                    int x = (-new MontgomeryModInt<T>(i)).Value;
                    x.Should().Be((mod - i) % mod);
                }
            }
        }
    }
}
