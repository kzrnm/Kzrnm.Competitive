using AtCoder;
using System;
using System.Linq;
using System.Numerics;
using LargeInteger = System.UInt128;

namespace Kzrnm.Competitive.Testing.Number;

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
                .ShouldBe((long)(((LargeInteger)a + (LargeInteger)b) % (LargeInteger)mod));
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
                .ShouldBe((long)((LargeInteger)a * (LargeInteger)b % (LargeInteger)mod));
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
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, (int)max).Select(i => (long)i);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt64(max, mod)));
            foreach (var i in cases)
            {
                if (Gcd(i, mod) != 1) continue;
                var x = new DynamicMontgomeryModInt64<T>(i).Inv().Value;
                ((long)((LargeInteger)x * (LargeInteger)i % (LargeInteger)mod)).ShouldBe(1);
            }
        }
    }

    private struct IncrementID { }
    [Fact]
    public void Increment()
    {
        DynamicMontgomeryModInt64<IncrementID> a;
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


        void RunStatic<T>() where T : struct
        {
            var mod = DynamicMontgomeryModInt64<T>.Mod;
            var nums = Enumerable.Range(-100, 200).SelectMany(i => new[] { i, mod + i });
            foreach (var n in nums)
            {
                var s = n.ToString();
                var expected = (n % mod + mod) % mod;
                DynamicMontgomeryModInt64<T>.TryParse(s, out var num1).ShouldBeTrue();
                var num2 = DynamicMontgomeryModInt64<T>.Parse(s);

                num1.Value.ShouldBe(expected);
                num2.Value.ShouldBe(expected);
            }

            foreach (var n in bigs)
            {
                var s = n.ToString();
                var expected = (int)(n % mod + mod) % mod;
                DynamicMontgomeryModInt64<T>.TryParse(s, out var num1).ShouldBeTrue();
                var num2 = DynamicMontgomeryModInt64<T>.Parse(s);

                num1.Value.ShouldBe(expected);
                num2.Value.ShouldBe(expected);
            }

            foreach (var s in invalids)
            {
                DynamicMontgomeryModInt64<T>.TryParse(s, out _).ShouldBeFalse();
                Should.Throw<FormatException>(() => DynamicMontgomeryModInt64<T>.Parse(s));
            }
        }
    }
    [Fact]
    public void ConstructorStatic()
    {
        new DynamicMontgomeryModInt64<ConstructorID>(3).Value.ShouldBe(3);
        new DynamicMontgomeryModInt64<ConstructorID>(-10).Value.ShouldBe(1);
        (1 + new DynamicMontgomeryModInt64<ConstructorID>(1)).Value.ShouldBe(2);
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
                .ShouldBe(expected);
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
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, (int)max).Select(i => (long)i);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt64(max, mod)));
            foreach (var i in cases)
            {
                var x = (-new DynamicMontgomeryModInt64<T>(i)).Value;
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

        static void RunStatic<T>() where T : struct
        {
            Span<char> chars = stackalloc char[30];
            var mod = DynamicMontgomeryModInt64<T>.Mod;
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, (int)max).Select(i => (long)i);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt64(max, mod)));
            foreach (var i in cases)
            {
                var m = new DynamicMontgomeryModInt64<T>(i);
                var expected = (i % mod).ToString();
                m.ToString().ShouldBe(expected);
                m.TryFormat(chars, out var charsWritten, "", null).ShouldBeTrue();
                new string(chars[..charsWritten]).ShouldBe(expected);
            }
        }
    }
}
