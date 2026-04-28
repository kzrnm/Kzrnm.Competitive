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
    [Test, MultipleAssertions]
    public async Task Add()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 1000; q++)
        {
            var mod = rnd.NextLong(1L << 59) | 1;
            DynamicMontgomeryModInt64<AddId>.Mod = mod;

            var a = rnd.NextLong();
            var b = rnd.NextLong();

            await ((DynamicMontgomeryModInt64<AddId>)a + (DynamicMontgomeryModInt64<AddId>)b).Value
                .Should().BeEqualTo((long)(((LargeInteger)a + (LargeInteger)b) % (LargeInteger)mod));
        }
    }
    private struct MultiplyId { }
    [Test, MultipleAssertions]
    public async Task Multiply()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 1000; q++)
        {
            var mod = rnd.NextLong(1L << 59) | 1;
            DynamicMontgomeryModInt64<MultiplyId>.Mod = mod;

            var a = rnd.NextLong();
            var b = rnd.NextLong();

            await ((DynamicMontgomeryModInt64<MultiplyId>)a * (DynamicMontgomeryModInt64<MultiplyId>)b).Value
                .Should().BeEqualTo((long)((LargeInteger)a * (LargeInteger)b % (LargeInteger)mod));
        }
    }

    [Test, MultipleAssertions]
    public async Task Inv()
    {
        await RunStatic<ModID11>();
        await RunStatic<ModID1000000007>();
        await RunStatic<ModID998244353>();
        await RunStatic<ModID67280421310721>();

        static async Task RunStatic<T>() where T : struct
        {
            var mod = DynamicMontgomeryModInt64<T>.Mod;
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, (int)max).Select(i => (long)i);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt64(max, mod)));
            foreach (var i in cases)
            {
                if (Gcd(i, mod) != 1) continue;
                var x = new DynamicMontgomeryModInt64<T>(i).Inv().Value;
                await ((long)((LargeInteger)x * (LargeInteger)i % (LargeInteger)mod)).Should().BeEqualTo(1);
            }
        }
    }

    private struct IncrementID { }
    [Test, MultipleAssertions]
    public async Task Increment()
    {
        DynamicMontgomeryModInt64<IncrementID> a;
        a = 8;
        await (++a).Value.Should().BeEqualTo(9);
        await (++a).Value.Should().BeEqualTo(10);
        await (++a).Value.Should().BeEqualTo(0);
        await (++a).Value.Should().BeEqualTo(1);
        a = 3;
        await (--a).Value.Should().BeEqualTo(2);
        await (--a).Value.Should().BeEqualTo(1);
        await (--a).Value.Should().BeEqualTo(0);
        await (--a).Value.Should().BeEqualTo(10);
        a = 8;
        await (a++).Value.Should().BeEqualTo(8);
        await (a++).Value.Should().BeEqualTo(9);
        await (a++).Value.Should().BeEqualTo(10);
        await (a++).Value.Should().BeEqualTo(0);
        await a.Value.Should().BeEqualTo(1);
        a = 3;
        await (a--).Value.Should().BeEqualTo(3);
        await (a--).Value.Should().BeEqualTo(2);
        await (a--).Value.Should().BeEqualTo(1);
        await (a--).Value.Should().BeEqualTo(0);
        await a.Value.Should().BeEqualTo(10);
    }

    [Test, MultipleAssertions]
    public async Task Parse()
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
        await RunStatic<ModID11>();
        await RunStatic<ModID1000000007>();
        await RunStatic<ModID998244353>();


        async Task RunStatic<T>() where T : struct
        {
            var mod = DynamicMontgomeryModInt64<T>.Mod;
            var nums = Enumerable.Range(-100, 200).SelectMany(i => new[] { i, mod + i });
            foreach (var n in nums)
            {
                var s = n.ToString();
                var expected = (n % mod + mod) % mod;
                await DynamicMontgomeryModInt64<T>.TryParse(s, out var num1).Should().BeTrue();
                var num2 = DynamicMontgomeryModInt64<T>.Parse(s);

                await num1.Value.Should().BeEqualTo(expected);
                await num2.Value.Should().BeEqualTo(expected);
            }

            foreach (var n in bigs)
            {
                var s = n.ToString();
                var expected = (int)(n % mod + mod) % mod;
                await DynamicMontgomeryModInt64<T>.TryParse(s, out var num1).Should().BeTrue();
                var num2 = DynamicMontgomeryModInt64<T>.Parse(s);

                await num1.Value.Should().BeEqualTo(expected);
                await num2.Value.Should().BeEqualTo(expected);
            }

            foreach (var s in invalids)
            {
                await DynamicMontgomeryModInt64<T>.TryParse(s, out _).Should().BeFalse();
                Assert.Throws<FormatException>(() => DynamicMontgomeryModInt64<T>.Parse(s));
            }
        }
    }
    [Test, MultipleAssertions]
    public async Task ConstructorStatic()
    {
        await new DynamicMontgomeryModInt64<ConstructorID>(3).Value.Should().BeEqualTo(3);
        await new DynamicMontgomeryModInt64<ConstructorID>(-10).Value.Should().BeEqualTo(1);
        await (1 + new DynamicMontgomeryModInt64<ConstructorID>(1)).Value.Should().BeEqualTo(2);
    }

    [Test, MultipleAssertions]
    public async Task MemoryStatic()
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
            await arr.Select(m => m.Value).ToArray().Should().BeEquivalentOrderTo(expected);
        }
    }

    [Test, MultipleAssertions]
    public async Task Minus()
    {
        await RunStatic<ModID11>();
        await RunStatic<ModID1000000007>();
        await RunStatic<ModID998244353>();
        await RunStatic<ModID67280421310721>();

        static async Task RunStatic<T>() where T : struct
        {
            var mod = DynamicMontgomeryModInt64<T>.Mod;
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, (int)max).Select(i => (long)i);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt64(max, mod)));
            foreach (var i in cases)
            {
                var x = (-new DynamicMontgomeryModInt64<T>(i)).Value;
                await x.Should().BeEqualTo((mod - i) % mod);
            }
        }
    }


    [Test, MultipleAssertions]
    public async Task String()
    {
        await RunStatic<ModID11>();
        await RunStatic<ModID1000000007>();
        await RunStatic<ModID998244353>();

        static async Task RunStatic<T>() where T : struct
        {
            var chars = new char[30];
            var mod = DynamicMontgomeryModInt64<T>.Mod;
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, (int)max).Select(i => (long)i);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt64(max, mod)));
            foreach (var i in cases)
            {
                var m = new DynamicMontgomeryModInt64<T>(i);
                var expected = (i % mod).ToString();
                await m.ToString().Should().BeEqualTo(expected);
                await m.TryFormat(chars, out var charsWritten, "", null).Should().BeTrue();
                await new string(chars, 0, charsWritten).Should().BeEqualTo(expected);
            }
        }
    }
}