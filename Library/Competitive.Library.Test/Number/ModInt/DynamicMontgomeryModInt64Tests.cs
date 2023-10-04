using System;
using System.Linq;

#if NET7_0_OR_GREATER
using LargeInteger = System.UInt128;
#else
using LargeInteger = System.Numerics.BigInteger;
#endif

namespace Kzrnm.Competitive.Testing.Number
{
    public class DynamicMontgomeryModInt64Tests
    {
        public DynamicMontgomeryModInt64Tests()
        {
            DynamicMontgomeryModInt64<ModID11>.Mod = 11;
            DynamicMontgomeryModInt64<ModID1000000007>.Mod = 1000000007;
            DynamicMontgomeryModInt64<ModID998244353>.Mod = 998244353;
            DynamicMontgomeryModInt64<IncrementID>.Mod = 11;
            DynamicMontgomeryModInt64<ConstructorID>.Mod = 11;
            DynamicMontgomeryModInt64<MemoryID>.Mod = 101;
            DynamicMontgomeryModInt64<ModID67280421310721>.Mod = 67280421310721;
        }
        private static long Gcd(long a, long b)
        {
            if (b == 0) return a;
            return Gcd(b, a % b);
        }

        private struct ModID11 { }
        private struct ConstructorID { }
        private struct MemoryID { }
        private struct ModID1000000007 { }
        private struct ModID998244353 { }
        private struct ModID67280421310721 { }


        private struct AddId { }
        [Fact]
        public void Add()
        {
            var rnd = new Random(227);
            for (int q = 0; q < 1000; q++)
            {
                var mod = rnd.NextLong(1L << 59) | 1;
                DynamicMontgomeryModInt64<AddId>.Mod = mod;

                var a = rnd.NextLong();
                var b = rnd.NextLong();

                ((DynamicMontgomeryModInt64<AddId>)a + (DynamicMontgomeryModInt64<AddId>)b).Value
                    .Should().Be((long)(((LargeInteger)a + (LargeInteger)b) % (LargeInteger)mod));
            }
        }
        private struct MultiplyId { }
        [Fact]
        public void Multiply()
        {
            var rnd = new Random(227);
            for (int q = 0; q < 1000; q++)
            {
                var mod = rnd.NextLong(1L << 59) | 1;
                DynamicMontgomeryModInt64<MultiplyId>.Mod = mod;

                var a = rnd.NextLong();
                var b = rnd.NextLong();

                ((DynamicMontgomeryModInt64<MultiplyId>)a * (DynamicMontgomeryModInt64<MultiplyId>)b).Value
                    .Should().Be((long)((LargeInteger)a * (LargeInteger)b % (LargeInteger)mod));
            }
        }

        [Fact]
        public void Inv()
        {
            RunStatic<ModID11>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID998244353>();
            RunStatic<ModID67280421310721>();

            static void RunStatic<T>() where T : struct
            {
                var mod = DynamicMontgomeryModInt64<T>.Mod;
                var max = Math.Min(100000, mod);
                for (int i = 1; i < max; i++)
                {
                    if (Gcd(i, mod) != 1) continue;
                    var x = new DynamicMontgomeryModInt64<T>(i).Inv().Value;
                    ((long)((LargeInteger)x * (LargeInteger)i % (LargeInteger)mod)).Should().Be(1);
                }
            }
        }

        private struct IncrementID { }
        [Fact]
        public void Increment()
        {
            DynamicMontgomeryModInt64<IncrementID> a;
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
            new DynamicMontgomeryModInt64<ConstructorID>(3).Value.Should().Be(3);
            new DynamicMontgomeryModInt64<ConstructorID>(-10).Value.Should().Be(1);
            (1 + new DynamicMontgomeryModInt64<ConstructorID>(1)).Value.Should().Be(2);
        }

        [Fact]
        public void MemoryStatic()
        {
            var mt = new Random(227);
            for (int n = 0; n < 100; n++)
            {
                var arr = new DynamicMontgomeryModInt64<MemoryID>[n];
                var expected = new long[n];
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
            RunStatic<ModID67280421310721>();

            static void RunStatic<T>() where T : struct
            {
                var mod = DynamicMontgomeryModInt64<T>.Mod;
                var max = Math.Min(100000, mod);
                for (int i = 0; i < max; i++)
                {
                    var x = (-new DynamicMontgomeryModInt64<T>(i)).Value;
                    x.Should().Be((mod - i) % mod);
                }
            }
        }


        [Fact]
        public void String()
        {
            RunStatic<ModID11>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID998244353>();

            static void RunStatic<T>() where T : struct
            {
                Span<char> chars = stackalloc char[30];
                var mod = DynamicMontgomeryModInt64<T>.Mod;
                var max = Math.Min(100000, mod);
                for (int i = 0; i < max; i++)
                {
                    var m = new DynamicMontgomeryModInt64<T>(i);
                    var expected = (i % mod).ToString();
                    m.ToString().Should().Be(expected);
                    m.TryFormat(chars, out var charsWritten, "", null).Should().BeTrue();
                    new string(chars[..charsWritten]).Should().Be(expected);
                }
            }
        }
    }
}
