using AtCoder;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.MathNS;

public class LinearRecurrenceTest
{
    private readonly struct Mod1000000007 : IStaticMod
    {
        public uint Mod => 1000000007;
        public bool IsPrime => true;
    }

    private readonly struct Mod998244353 : IStaticMod
    {
        public uint Mod => 998244353;
        public bool IsPrime => true;
    }

    private struct DMod : IDynamicModIntId { }
    private static MontgomeryModInt<T>[] NativeDp<T>(Span<MontgomeryModInt<T>> a, Span<MontgomeryModInt<T>> c) where T : struct, IStaticMod
    {
        var res = new MontgomeryModInt<T>[1000];
        a.CopyTo(res);
        for (int k = a.Length; k < res.Length; k++)
            for (int i = 0; i < c.Length; i++)
            {
                res[k] += c[i] * res[k - i - 1];
            }
        return res;
    }

    [Test, MultipleAssertions]
    public async Task Kitamasa_Mod1000000007()
    {
        var rnd = new Random(42);
        for (int n = 2; n < 10; n++)
        {
            var arrInt = rnd.NextIntArray(n, 0, 1000000007);
            var crrInt = rnd.NextIntArray(n, 0, 1000000007);
            var arrModInt = arrInt.Select(n => new MontgomeryModInt<Mod1000000007>(n)).ToArray();
            var crrModInt = crrInt.Select(n => new MontgomeryModInt<Mod1000000007>(n)).ToArray();
            var expected = NativeDp(arrModInt, crrModInt);
            for (int l = 0; l < 40; l++)
            {
                await LinearRecurrence.Kitamasa<Mod1000000007>(arrInt, crrInt, l).Should().BeEqualTo(expected[l]);
                var arrUInt = MemoryMarshal.Cast<int, uint>(arrInt);
                var crrUInt = MemoryMarshal.Cast<int, uint>(crrInt);
                await LinearRecurrence.Kitamasa<Mod1000000007>(arrUInt, crrUInt, l).Should().BeEqualTo(expected[l]);
                await LinearRecurrence.Kitamasa(arrModInt, crrModInt, l).Should().BeEqualTo(expected[l]);
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Kitamasa_Mod998244353()
    {
        var rnd = new Random(42);
        for (int n = 2; n < 10; n++)
        {
            var arrInt = rnd.NextIntArray(n, 0, 998244353);
            var crrInt = rnd.NextIntArray(n, 0, 998244353);
            var arrModInt = arrInt.Select(n => new MontgomeryModInt<Mod998244353>(n)).ToArray();
            var crrModInt = crrInt.Select(n => new MontgomeryModInt<Mod998244353>(n)).ToArray();
            var expected = NativeDp(arrModInt, crrModInt);
            for (int l = 0; l < 40; l++)
            {
                await LinearRecurrence.Kitamasa<Mod998244353>(arrInt, crrInt, l).Should().BeEqualTo(expected[l]);
                var arrUInt = MemoryMarshal.Cast<int, uint>(arrInt);
                var crrUInt = MemoryMarshal.Cast<int, uint>(crrInt);
                await LinearRecurrence.Kitamasa<Mod998244353>(arrUInt, crrUInt, l).Should().BeEqualTo(expected[l]);
                await LinearRecurrence.Kitamasa(arrModInt, crrModInt, l).Should().BeEqualTo(expected[l]);
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Kitamasa_Fibonacci()
    {
        var rnd = new Random(42);
        for (int n = 2; n < 200; n++)
        {
            var a0 = rnd.Next();
            var a1 = rnd.Next();
            var N = (long)rnd.NextUInt() << 24;
            await RunTest<Mod998244353>(a0, a1, N);
            await RunTest<Mod1000000007>(a0, a1, N);
        }


        static async Task RunTest<T>(int a0, int a1, long n) where T : struct, IStaticMod
        {
            Matrix2x2<MontgomeryModInt<T>> mat = new(
                (0, 1),
                (1, 1)
            );

            await LinearRecurrence.Kitamasa<T>(
                stackalloc MontgomeryModInt<T>[2] { a0, a1 },
                stackalloc MontgomeryModInt<T>[2] { 1, 1 }, n)
                 .Should().BeEqualTo(mat.Pow(n).Multiply(a0, a1).v0);
        }
    }

    [Test, MultipleAssertions]
    public async Task Recurrence_Tribonacci()
    {
        var rnd = new Random(42);


        for (int n = 0; n < 4; n++)
        {
            long a0 = rnd.Next();
            long a1 = rnd.Next();
            long a2 = rnd.Next();

            var native998244353 = Native<Mod998244353>(a0, a1, a2, 80);
            var native1000000007 = Native<Mod1000000007>(a0, a1, a2, 80);

            for (int len = 0; len < 80; len++)
            {
                {
                    var recurrence = LinearRecurrence.Recurrence<Mod998244353>([a0, a1, a2], [1, 1, 1], len);
                    await recurrence.Should().BeStrictlyEquivalentTo(native998244353[..len]);
                }

                {
                    var recurrence = LinearRecurrence.Recurrence<Mod1000000007>([a0, a1, a2], [1, 1, 1], len);
                    await recurrence.Should().BeStrictlyEquivalentTo(native1000000007[..len]);
                }
            }
        }
        for (int n = 0; n < 4; n++)
        {
            long a0 = rnd.Next();
            long a1 = rnd.Next();
            var a2 = a0 + a1;

            var native998244353 = Native<Mod998244353>(a0, a1, a2, 80);
            var native1000000007 = Native<Mod1000000007>(a0, a1, a2, 80);

            for (int len = 0; len < 80; len++)
            {
                {
                    var recurrence = LinearRecurrence.Recurrence<Mod998244353>([a0, a1], [1, 1, 1], len);
                    await recurrence.Should().BeStrictlyEquivalentTo(native998244353[..len]);
                }

                {
                    var recurrence = LinearRecurrence.Recurrence<Mod1000000007>([a0, a1], [1, 1, 1], len);
                    await recurrence.Should().BeStrictlyEquivalentTo(native1000000007[..len]);
                }
            }
        }
        for (int n = 0; n < 4; n++)
        {
            long a0 = rnd.Next();
            var a1 = a0;
            var a2 = a0 + a1;

            var native998244353 = Native<Mod998244353>(a0, a1, a2, 80);
            var native1000000007 = Native<Mod1000000007>(a0, a1, a2, 80);

            for (int len = 0; len < 80; len++)
            {
                {
                    var recurrence = LinearRecurrence.Recurrence<Mod998244353>([a0], [1, 1, 1], len);
                    await recurrence.Should().BeStrictlyEquivalentTo(native998244353[..len]);

                }
                {
                    var recurrence = LinearRecurrence.Recurrence<Mod1000000007>([a0], [1, 1, 1], len);
                    await recurrence.Should().BeStrictlyEquivalentTo(native1000000007[..len]);
                }

            }
        }

        static MontgomeryModInt<T>[] Native<T>(long a0, long a1, long a2, int len) where T : struct, IStaticMod
        {
            var dp = new MontgomeryModInt<T>[len];
            dp[0] = a0;
            dp[1] = a1;
            dp[2] = a2;

            for (int i = 3; i < dp.Length; i++)
                dp[i] = dp[i - 1] + dp[i - 2] + dp[i - 3];
            return dp;
        }
    }
}