using AtCoder;
using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Number
{
    public class MontgomeryModIntTests
    {
        private static long Gcd(long a, long b)
        {
            if (b == 0) return a;
            return Gcd(b, a % b);
        }

        readonly struct Mod1ID : IStaticMod
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
                    (new MontgomeryModInt<Mod1ID>(i) * j).Value.ShouldBe(0);
                }
            }
            (new MontgomeryModInt<Mod1ID>(1234) + 5678).Value.ShouldBe(0);
            (new MontgomeryModInt<Mod1ID>(1234) - 5678).Value.ShouldBe(0);
            (new MontgomeryModInt<Mod1ID>(1234) * 5678).Value.ShouldBe(0);
            //(new LazyMontgomeryModInt<Mod1ID>(1234).Pow(5678)).Value.ShouldBe(0); // faild in Debug.Assert
            (new MontgomeryModInt<Mod1ID>(0).Inv()).Value.ShouldBe(0);
        }

        private readonly struct ModID11 : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        private readonly struct ModID1000000007 : IStaticMod
        {
            public uint Mod => 1000000007;
            public bool IsPrime => false;
        }
        private readonly struct ModID998244353 : IStaticMod
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
                var max = Math.Min(1000, mod);
                var cases = Enumerable.Range(1, max);
                if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt32(max, mod)));
                foreach (var i in cases)
                {
                    if (Gcd(i, mod) != 1) continue;
                    int x = new MontgomeryModInt<T>(i).Inv().Value;
                    ((long)x * i % mod).ShouldBe(1);
                }
            }
        }

        private readonly struct IncrementID : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        [Fact]
        public void Increment()
        {
            MontgomeryModInt<IncrementID> a;
            a = 8;
            (++a).Value.ShouldBe(9);
            (++a).Value.ShouldBe(10);
            (++a).Value.ShouldBe(0);
            (++a).Value.ShouldBe(1);
            a = 3;
            (--a).Value.ShouldBe(2);
            (--a).Value.ShouldBe(1);
            (--a).Value.ShouldBe(0);
            (--a).Value.ShouldBe(10);
            a = 8;
            (a++).Value.ShouldBe(8);
            (a++).Value.ShouldBe(9);
            (a++).Value.ShouldBe(10);
            (a++).Value.ShouldBe(0);
            a.Value.ShouldBe(1);
            a = 3;
            (a--).Value.ShouldBe(3);
            (a--).Value.ShouldBe(2);
            (a--).Value.ShouldBe(1);
            (a--).Value.ShouldBe(0);
            a.Value.ShouldBe(10);
        }

        [Fact]
        public void Parse()
        {
            var bigs = new[]{
                BigInteger.Pow(10, 1000),
                BigInteger.Pow(10, 1000),
            };
            var invalids = new[]
            {
                "2-2",
                "ABC",
                "111.0",
                "111,1",
            };
            RunStatic<ModID11>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID998244353>();


            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    var s = n.ToString();
                    var expected = (n % mod + mod) % mod;
                    MontgomeryModInt<T>.TryParse(s, out var num1).ShouldBeTrue();
                    var num2 = MontgomeryModInt<T>.Parse(s);

                    num1.Value.ShouldBe(expected);
                    num2.Value.ShouldBe(expected);
                }

                foreach (var n in bigs)
                {
                    var s = n.ToString();
                    var expected = (int)(n % mod + mod) % mod;
                    MontgomeryModInt<T>.TryParse(s, out var num1).ShouldBeTrue();
                    var num2 = MontgomeryModInt<T>.Parse(s);

                    num1.Value.ShouldBe(expected);
                    num2.Value.ShouldBe(expected);
                }

                foreach (var s in invalids)
                {
                    MontgomeryModInt<T>.TryParse(s, out _).ShouldBeFalse();
                    Should.Throw<FormatException>(() => MontgomeryModInt<T>.Parse(s));
                }
            }
        }
        private readonly struct ConstructorID : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        [Fact]
        public void ConstructorStatic()
        {
            new MontgomeryModInt<ConstructorID>(3).Value.ShouldBe(3);
            new MontgomeryModInt<ConstructorID>(-10).Value.ShouldBe(1);
            (1 + new MontgomeryModInt<ConstructorID>(1)).Value.ShouldBe(2);
        }

        private readonly struct MemoryID : IStaticMod
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
                    .ShouldBe(expected);
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
                var max = Math.Min(1000, mod);
                var cases = Enumerable.Range(0, max);
                if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt32(max, mod)));
                foreach (var i in cases)
                {
                    int x = (-new MontgomeryModInt<T>(i)).Value;
                    x.ShouldBe((mod - i) % mod);
                }
            }
        }

        [Fact]
        public void String()
        {
            RunStatic<ModID11>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID998244353>();

            static void RunStatic<T>() where T : struct, IStaticMod
            {
                Span<char> chars = stackalloc char[30];
                var mod = (int)new T().Mod;
                var max = Math.Min(1000, mod);
                var cases = Enumerable.Range(0, max);
                if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt32(max, mod)));
                foreach (var i in cases)
                {
                    var m = new MontgomeryModInt<T>(i);
                    var expected = (i % mod).ToString();
                    m.ToString().ShouldBe(expected);
                    m.TryFormat(chars, out var charsWritten, "", null).ShouldBeTrue();
                    new string(chars[..charsWritten]).ShouldBe(expected);
                }
            }
        }
    }
}
