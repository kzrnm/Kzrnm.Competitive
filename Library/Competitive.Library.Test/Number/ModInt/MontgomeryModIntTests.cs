using AtCoder;

namespace Kzrnm.Competitive.Testing.Number;

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
    [Test, MultipleAssertions]
    public async Task Mod1()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                await (new MontgomeryModInt<Mod1ID>(i) * j).Value.Should().BeEqualTo(0);
            }
        }
        await (new MontgomeryModInt<Mod1ID>(1234) + 5678).Value.Should().BeEqualTo(0);
        await (new MontgomeryModInt<Mod1ID>(1234) - 5678).Value.Should().BeEqualTo(0);
        await (new MontgomeryModInt<Mod1ID>(1234) * 5678).Value.Should().BeEqualTo(0);
        //await (new LazyMontgomeryModInt<Mod1ID>(1234).Pow(5678)).Value.Should().BeEqualTo(0); // faild in Debug.Assert
        await (new MontgomeryModInt<Mod1ID>(0).Inv()).Value.Should().BeEqualTo(0);
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
    [Test, MultipleAssertions]
    public async Task Inv()
    {
        await RunStatic<ModID11>();
        await RunStatic<ModID1000000007>();
        await RunStatic<ModID998244353>();

        static async Task RunStatic<T>() where T : struct, IStaticMod
        {
            var mod = (int)new T().Mod;
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(1, max);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt32(max, mod)));
            foreach (var i in cases)
            {
                if (Gcd(i, mod) != 1) continue;
                int x = new MontgomeryModInt<T>(i).Inv().Value;
                await ((long)x * i % mod).Should().BeEqualTo(1);
            }
        }
    }

    private readonly struct IncrementID : IStaticMod
    {
        public uint Mod => 11;
        public bool IsPrime => true;
    }
    [Test, MultipleAssertions]
    public async Task Increment()
    {
        MontgomeryModInt<IncrementID> a;
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


        async Task RunStatic<T>() where T : struct, IStaticMod
        {
            var mod = (int)new T().Mod;
            var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
            foreach (var n in nums)
            {
                var s = n.ToString();
                var expected = (n % mod + mod) % mod;
                await MontgomeryModInt<T>.TryParse(s, out var num1).Should().BeTrue();
                var num2 = MontgomeryModInt<T>.Parse(s);

                await num1.Value.Should().BeEqualTo(expected);
                await num2.Value.Should().BeEqualTo(expected);
            }

            foreach (var n in bigs)
            {
                var s = n.ToString();
                var expected = (int)(n % mod + mod) % mod;
                await MontgomeryModInt<T>.TryParse(s, out var num1).Should().BeTrue();
                var num2 = MontgomeryModInt<T>.Parse(s);

                await num1.Value.Should().BeEqualTo(expected);
                await num2.Value.Should().BeEqualTo(expected);
            }

            foreach (var s in invalids)
            {
                await MontgomeryModInt<T>.TryParse(s, out _).Should().BeFalse();
                Assert.Throws<FormatException>(() => MontgomeryModInt<T>.Parse(s));
            }
        }
    }
    private readonly struct ConstructorID : IStaticMod
    {
        public uint Mod => 11;
        public bool IsPrime => true;
    }
    [Test, MultipleAssertions]
    public async Task ConstructorStatic()
    {
        await new MontgomeryModInt<ConstructorID>(3).Value.Should().BeEqualTo(3);
        await new MontgomeryModInt<ConstructorID>(-10).Value.Should().BeEqualTo(1);
        await (1 + new MontgomeryModInt<ConstructorID>(1)).Value.Should().BeEqualTo(2);
    }

    private readonly struct MemoryID : IStaticMod
    {
        public uint Mod => 101;
        public bool IsPrime => true;
    }

    [Test, MultipleAssertions]
    public async Task MemoryStatic()
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
            await arr.Select(m => m.Value).ToArray().Should().BeStrictlyEquivalentTo(expected);
        }
    }

    [Test, MultipleAssertions]
    public async Task Minus()
    {
        await RunStatic<ModID11>();
        await RunStatic<ModID1000000007>();
        await RunStatic<ModID998244353>();

        static async Task RunStatic<T>() where T : struct, IStaticMod
        {
            var mod = (int)new T().Mod;
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, max);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt32(max, mod)));
            foreach (var i in cases)
            {
                int x = (-new MontgomeryModInt<T>(i)).Value;
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

        static async Task RunStatic<T>() where T : struct, IStaticMod
        {
            var chars = new char[30];
            var mod = (int)new T().Mod;
            var max = Math.Min(1000, mod);
            var cases = Enumerable.Range(0, max);
            if (max < mod) cases = cases.Concat(Enumerable.Repeat(new Xoshiro256(227), 1000).Select(r => r.NextInt32(max, mod)));
            foreach (var i in cases)
            {
                var m = new MontgomeryModInt<T>(i);
                var expected = (i % mod).ToString();
                await m.ToString().Should().BeEqualTo(expected);
                await m.TryFormat(chars, out var charsWritten, "", null).Should().BeTrue();
                await new string(chars, 0, charsWritten).Should().BeEqualTo(expected);
            }
        }
    }
}