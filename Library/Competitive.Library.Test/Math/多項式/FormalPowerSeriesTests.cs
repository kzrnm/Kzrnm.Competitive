using AtCoder;

namespace Kzrnm.Competitive.Testing.MathNS;

public class FormalPowerSeriesTest
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

    [Test, MultipleAssertions]
    public async Task Coefficients()
    {
        await RunTest<Mod998244353>([1, 2, 3]);
        await RunTest<Mod998244353>([0, 5, 0, 2]);
        await RunTest<Mod998244353>([]);

        await RunTest<Mod1000000007>([1, 2, 3]);
        await RunTest<Mod1000000007>([0, 5, 0, 2]);
        await RunTest<Mod1000000007>([]);

        static async Task RunTest<T>(int[] array) where T : struct, IStaticMod
        {
            var modArray = array.Select(v => (MontgomeryModInt<T>)v).ToArray();
            var f = new FormalPowerSeries<T>(array);
            await f.Coefficients().ToArray().Should().BeEquivalentOrderTo(modArray);

            for (int len = 0; len < modArray.Length; len++)
            {
                await f.Coefficients(len).ToArray().Should().BeEquivalentOrderTo(modArray[..len]);
            }

            await f.Coefficients(modArray.Length + 2).ToArray().Should().BeEquivalentOrderTo(modArray.Append(default).Append(default));
        }
    }

    [Test, MultipleAssertions]
    public async Task Add()
    {
        await RunTest<Mod998244353>([1, 2, 3], [0, 5, 0, 2], [1, 7, 3, 2]);
        await RunTest<Mod998244353>([0, 5, 0, 2], [1, 2, 3], [1, 7, 3, 2]);
        await RunTest<Mod998244353>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        await RunTest<Mod998244353>([], [0, 5, 0, 2], [0, 5, 0, 2]);

        await RunTest<Mod1000000007>([1, 2, 3], [0, 5, 0, 2], [1, 7, 3, 2]);
        await RunTest<Mod1000000007>([0, 5, 0, 2], [1, 2, 3], [1, 7, 3, 2]);
        await RunTest<Mod1000000007>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        await RunTest<Mod1000000007>([], [0, 5, 0, 2], [0, 5, 0, 2]);

        static async Task RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var orig = lhs._cs.ToArray();

                await (lhs + rhs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await (lhs + rhs._cs)._cs.Should().BeEquivalentOrderTo(expected._cs);

                await lhs._cs.Should().BeEquivalentOrderTo(orig);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                await lhs.AddSelf(rhs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await lhs._cs.Should().BeEquivalentOrderTo(expected._cs);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                await lhs.AddSelf(rhs._cs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await lhs._cs.Should().BeEquivalentOrderTo(expected._cs);
            }
        }
    }
    [Test, MultipleAssertions]
    public async Task Subtract()
    {
        await RunTest<Mod998244353>([1, 2, 3], [0, 5, 0, 2], [1, -3, 3, -2]);
        await RunTest<Mod998244353>([0, 5, 0, 2], [1, 2, 3], [-1, 3, -3, 2]);
        await RunTest<Mod998244353>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        await RunTest<Mod998244353>([], [0, 5, 0, 2], [0, -5, 0, -2]);

        await RunTest<Mod1000000007>([1, 2, 3], [0, 5, 0, 2], [1, -3, 3, -2]);
        await RunTest<Mod1000000007>([0, 5, 0, 2], [1, 2, 3], [-1, 3, -3, 2]);
        await RunTest<Mod1000000007>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        await RunTest<Mod1000000007>([], [0, 5, 0, 2], [0, -5, 0, -2]);

        static async Task RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var orig = lhs._cs.ToArray();

                await (lhs - rhs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await (lhs - rhs._cs)._cs.Should().BeEquivalentOrderTo(expected._cs);

                await lhs._cs.Should().BeEquivalentOrderTo(orig);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                await lhs.SubtractSelf(rhs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await lhs._cs.Should().BeEquivalentOrderTo(expected._cs);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                await lhs.SubtractSelf(rhs._cs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await lhs._cs.Should().BeEquivalentOrderTo(expected._cs);
            }
        }
    }
    [Test, MultipleAssertions]
    public async Task Minus()
    {
        await RunTest<Mod998244353>([0, 1, 2, 3], [0, -1, -2, -3]);
        await RunTest<Mod998244353>([0, -1, -2, -3], [0, 1, 2, 3]);
        await RunTest<Mod998244353>([], []);

        await RunTest<Mod1000000007>([0, 1, 2, 3], [0, -1, -2, -3]);
        await RunTest<Mod1000000007>([0, -1, -2, -3], [0, 1, 2, 3]);
        await RunTest<Mod1000000007>([], []);

        static async Task RunTest<T>(int[] valueArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var value = new FormalPowerSeries<T>(valueArray.Select(n => new MontgomeryModInt<T>(n)).ToArray());
            var expected = new FormalPowerSeries<T>(expectedArray.Select(n => new MontgomeryModInt<T>(n)).ToArray());

            await (-value)._cs.Should().BeEquivalentOrderTo(expected._cs);
        }
    }
    [Test, MultipleAssertions]
    public async Task Multiply()
    {
        await RunTest<Mod998244353>([1, 2, 3], [0, 5, 0, 2], [0, 5, 10, 17, 4, 6]);
        await RunTest<Mod998244353>([0, 5, 0, 2], [1, 2, 3], [0, 5, 10, 17, 4, 6]);
        await RunTest<Mod998244353>([0, 5, 0, 2], [], []);
        await RunTest<Mod998244353>([], [0, 5, 0, 2], []);

        await RunTest<Mod1000000007>([1, 2, 3], [0, 5, 0, 2], [0, 5, 10, 17, 4, 6]);
        await RunTest<Mod1000000007>([0, 5, 0, 2], [1, 2, 3], [0, 5, 10, 17, 4, 6]);
        await RunTest<Mod1000000007>([0, 5, 0, 2], [], []);
        await RunTest<Mod1000000007>([], [0, 5, 0, 2], []);

        static async Task RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var orig = lhs._cs.ToArray();

                await (lhs * rhs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await (lhs * rhs._cs)._cs.Should().BeEquivalentOrderTo(expected._cs);

                await lhs._cs.Should().BeEquivalentOrderTo(orig);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                await lhs.MultiplySelf(rhs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await lhs._cs.Should().BeEquivalentOrderTo(expected._cs);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                await lhs.MultiplySelf(rhs._cs)._cs.Should().BeEquivalentOrderTo(expected._cs);
                await lhs._cs.Should().BeEquivalentOrderTo(expected._cs);
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Divide()
    {
        await RunTest<Mod998244353>([0, 5, 10, 17, 4, 6], [0, 5, 0, 2], [1, 2, 3]);
        await RunTest<Mod998244353>([0, 5, 10, 17, 4, 6], [1, 2, 3], [0, 5, 0, 2]);
        await RunTest<Mod998244353>([1, 2, 3], [0, 5, 10, 17, 4, 6], []);
        await RunTest<Mod998244353>([], [0, 5, 10, 17, 4, 6], []);

        await RunTest<Mod1000000007>([0, 5, 10, 17, 4, 6], [0, 5, 0, 2], [1, 2, 3]);
        await RunTest<Mod1000000007>([0, 5, 10, 17, 4, 6], [1, 2, 3], [0, 5, 0, 2]);
        await RunTest<Mod1000000007>([1, 2, 3], [0, 5, 10, 17, 4, 6], []);
        await RunTest<Mod1000000007>([], [0, 5, 10, 17, 4, 6], []);


        static async Task RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var lhs = new FormalPowerSeries<T>(lhsArray);
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);

            await (lhs / rhs)._cs.Should().BeEquivalentOrderTo(expected._cs);
        }
    }

    [Test, MultipleAssertions]
    public async Task DivRem()
    {
        var rnd = new Random(227);
        const int N = 130;
        var lhs = new int[N];
        var rhs = new int[N + 1];
        var rem = new int[N];
        foreach (ref var v in lhs.AsSpan()) v = rnd.Next(1, 1000000);
        foreach (ref var v in rhs.AsSpan()) v = rnd.Next(1, 1000000);
        foreach (ref var v in rem.AsSpan()) v = rnd.Next(1, 1000000);

        await RunTest<Mod998244353>(lhs, rhs, rem);
        await RunTest<Mod1000000007>(lhs, rhs, rem);

        static async Task RunTest<T>(int[] lhsArray, int[] rhsArray, int[] remArray) where T : struct, IStaticMod
        {
            var lhs = new FormalPowerSeries<T>(lhsArray);
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var rem = new FormalPowerSeries<T>(remArray);

            var p = lhs * rhs + rem;
            var (q, r) = p.DivRem(rhs);

            await q._cs.Should().BeEquivalentOrderTo((p / rhs)._cs);
            await r._cs.Should().BeEquivalentOrderTo((p % rhs)._cs);
            await (q * rhs + r)._cs.Should().BeEquivalentOrderTo(p._cs);
        }
    }

    [Test, MultipleAssertions]
    public async Task RightShift()
    {
        await RunTest<Mod998244353>([1, 2, 3], 0, [1, 2, 3]);
        await RunTest<Mod998244353>([1, 2, 3], 1, [2, 3]);
        await RunTest<Mod998244353>([1, 2, 3], 2, [3]);
        await RunTest<Mod998244353>([1, 2, 3], 3, []);

        await RunTest<Mod1000000007>([1, 2, 3], 0, [1, 2, 3]);
        await RunTest<Mod1000000007>([1, 2, 3], 1, [2, 3]);
        await RunTest<Mod1000000007>([1, 2, 3], 2, [3]);
        await RunTest<Mod1000000007>([1, 2, 3], 3, []);

        static async Task RunTest<T>(int[] valueArray, int shift, int[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            var expected = new FormalPowerSeries<T>(expectedArray);

            await (fps >> shift)._cs.Should().BeEquivalentOrderTo(expected._cs);
        }
    }

    [Test, MultipleAssertions]
    public async Task LeftShift()
    {
        await RunTest<Mod998244353>([1, 2, 3], 0, [1, 2, 3]);
        await RunTest<Mod998244353>([1, 2, 3], 1, [0, 1, 2, 3]);
        await RunTest<Mod998244353>([1, 2, 3], 4, [0, 0, 0, 0, 1, 2, 3]);

        await RunTest<Mod1000000007>([1, 2, 3], 0, [1, 2, 3]);
        await RunTest<Mod1000000007>([1, 2, 3], 1, [0, 1, 2, 3]);
        await RunTest<Mod1000000007>([1, 2, 3], 4, [0, 0, 0, 0, 1, 2, 3]);

        static async Task RunTest<T>(int[] valueArray, int shift, int[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            var expected = new FormalPowerSeries<T>(expectedArray);

            await (fps << shift)._cs.Should().BeEquivalentOrderTo(expected._cs);
        }
    }

    [Test, MultipleAssertions]
    public async Task Derivative()
    {
        await RunTest<Mod998244353>([3, 5, 10, 17, 4, 6], [5, 20, 51, 16, 30]);
        await RunTest<Mod998244353>([3], []);
        await RunTest<Mod998244353>([], []);

        await RunTest<Mod1000000007>([3, 5, 10, 17, 4, 6], [5, 20, 51, 16, 30]);
        await RunTest<Mod1000000007>([3], []);
        await RunTest<Mod1000000007>([], []);

        static async Task RunTest<T>(int[] valueArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            await fps.Derivative()._cs.Should().BeEquivalentOrderTo(expectedArray.Select(t => new MontgomeryModInt<T>(t)).ToArray());
        }
    }

    [Test, MultipleAssertions]
    public async Task Integrate()
    {
        await RunTest<Mod998244353>([5, 20, 51, 16, 30], [(0, 1), (5, 1), (10, 1), (17, 1), (4, 1), (6, 1)]);
        await RunTest<Mod998244353>([1, 1, 1, 1], [(0, 1), (1, 1), (1, 2), (1, 3), (1, 4)]);
        await RunTest<Mod998244353>([3], [(0, 1), (3, 1)]);
        await RunTest<Mod998244353>([], []);

        await RunTest<Mod1000000007>([5, 20, 51, 16, 30], [(0, 1), (5, 1), (10, 1), (17, 1), (4, 1), (6, 1)]);
        await RunTest<Mod1000000007>([1, 1, 1, 1], [(0, 1), (1, 1), (1, 2), (1, 3), (1, 4)]);
        await RunTest<Mod1000000007>([3], [(0, 1), (3, 1)]);
        await RunTest<Mod1000000007>([], []);

        static async Task RunTest<T>(int[] valueArray, (int Numerator, int Denominator)[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            await fps.Integrate()._cs.Should().BeEquivalentOrderTo(expectedArray.Select(t => new MontgomeryModInt<T>(t.Numerator) / t.Denominator).ToArray());
        }
    }

    [Test, MultipleAssertions]
    public async Task Eval()
    {
        await RunTest<Mod998244353>([5, 20, 51, 16, 30], 7, 80162);
        await RunTest<Mod998244353>([5, 20, 51, 16, 30], 8, 134501);
        await RunTest<Mod998244353>([5, 20, 51, 16, 30], 9, 212810);
        await RunTest<Mod998244353>([], 9, 0);

        await RunTest<Mod1000000007>([5, 20, 51, 16, 30], 7, 80162);
        await RunTest<Mod1000000007>([5, 20, 51, 16, 30], 8, 134501);
        await RunTest<Mod1000000007>([5, 20, 51, 16, 30], 9, 212810);
        await RunTest<Mod1000000007>([], 9, 0);

        static async Task RunTest<T>(int[] fpsArray, MontgomeryModInt<T> x, MontgomeryModInt<T> expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            await fps.Eval(x).Should().BeEqualTo(expected);
        }
    }

    [Test, MultipleAssertions]
    public async Task Inv()
    {
        await RunTest<Mod998244353>(
                [5, 4, 3, 2, 1],
                [598946612, 718735934, 862483121, 635682004, 163871793]);

        await RunTest<Mod1000000007>(
                [5, 4, 3, 2, 1],
                [400000003, 880000006, 856000006, 427200003, 712640005]);

        static async Task RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            await fps.Inv()._cs.Should().BeEquivalentOrderTo(new FormalPowerSeries<T>(expected)._cs);
        }
    }

    [Test, MultipleAssertions]
    public async Task Exp()
    {
        await RunTest<Mod998244353>([0, 1, 2, 3, 4], [1, 1, 499122179, 166374064, 291154613]);
        await RunTest<Mod998244353>(
                [0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549],
                [1, 907649120, 316060452, 57037696, 378993419, 302467176, 349948335, 115795520, 647455105, 497971134]);
        await RunTest<Mod998244353>([0], [1]);
        await RunTest<Mod998244353>([], [1]);

        await RunTest<Mod1000000007>([0, 1, 2, 3, 4], [1, 1, 500000006, 166666673, 41666677]);
        await RunTest<Mod1000000007>(
                [0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549],
                [1, 907649120, 925644116, 38331988, 156875359, 697776255, 802320078, 499725651, 949053640, 121509191]);
        await RunTest<Mod1000000007>([0], [1]);
        await RunTest<Mod1000000007>([], [1]);

        static async Task RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            await fps.Exp()._cs.Should().BeEquivalentOrderTo(new FormalPowerSeries<T>(expected)._cs);
        }
    }

    [Test, MultipleAssertions]
    public async Task Log()
    {
        await RunTest<Mod998244353>([1, 1, 499122179, 166374064, 291154613], [0, 1, 2, 3, 4]);
        await RunTest<Mod998244353>(
                [1, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549],
                [0, 907649120, 265241806, 491547518, 331811826, 54791043, 895176577, 142597055, 60021098, 768274455]);
        await RunTest<Mod998244353>(Enumerable.Repeat(0, 50000).Prepend(1).ToArray(), [0]);

        await RunTest<Mod1000000007>([1, 1, 500000006, 166666673, 41666677], [0, 1, 2, 3, 4]);
        await RunTest<Mod1000000007>(
                [1, 907649120, 925644116, 38331988, 156875359, 697776255, 802320078, 499725651, 949053640, 121509191],
                [0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549]);

        static async Task RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            await fps.Log()._cs.Should().BeEquivalentOrderTo(new FormalPowerSeries<T>(expected)._cs);
        }
    }

    [Test, MultipleAssertions]
    public async Task Pow()
    {
        await RunTest<Mod998244353>(
                [2, 3, 4, 5, 6], 2,
                [4, 12, 25, 44, 70]);
        await RunTest<Mod998244353>(
                [2, 3, 4, 5, 6], 3,
                [8, 36, 102, 231, 456]);
        await RunTest<Mod998244353>(
                [0, 0, 2, 3, 4, 5, 6], 2,
                [0, 0, 0, 0, 4, 12, 25]);
        await RunTest<Mod998244353>([], 2, []);

        await RunTest<Mod1000000007>(
                [2, 3, 4, 5, 6], 2,
                [4, 12, 25, 44, 70]);
        await RunTest<Mod1000000007>(
                [2, 3, 4, 5, 6], 3,
                [8, 36, 102, 231, 456]);
        await RunTest<Mod1000000007>(
                [0, 0, 2, 3, 4, 5, 6], 2,
                [0, 0, 0, 0, 4, 12, 25]);
        await RunTest<Mod1000000007>([], 2, []);

        await new FormalPowerSeries<Mod998244353>([2, 3, 4, 5, 6])
             .Pow(3, 13)._cs.Should().BeEquivalentOrderTo([(MontgomeryModInt<Mod998244353>)8, 36, 102, 231, 456, 735, 1024, 1257, 1344, 1169, 882, 540, 216]);
        await new FormalPowerSeries<Mod998244353>([0, 0, 2, 3, 4, 5, 6])
             .Pow(2, 13)._cs.Should().BeEquivalentOrderTo([(MontgomeryModInt<Mod998244353>)0, 0, 0, 0, 4, 12, 25, 44, 70, 76, 73, 60, 36]);

        await new FormalPowerSeries<Mod1000000007>([2, 3, 4, 5, 6])
             .Pow(3, 13)._cs.Should().BeEquivalentOrderTo([(MontgomeryModInt<Mod1000000007>)8, 36, 102, 231, 456, 735, 1024, 1257, 1344, 1169, 882, 540, 216]);
        await new FormalPowerSeries<Mod1000000007>([0, 0, 2, 3, 4, 5, 6])
             .Pow(2, 13)._cs.Should().BeEquivalentOrderTo([(MontgomeryModInt<Mod1000000007>)0, 0, 0, 0, 4, 12, 25, 44, 70, 76, 73, 60, 36]);

        static async Task RunTest<T>(int[] fpsArray, int n, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            await fps.Pow(n)._cs.Should().BeEquivalentOrderTo(new FormalPowerSeries<T>(expected)._cs);
        }
    }


    [Test, MultipleAssertions]
    public async Task TaylorShift()
    {
        await RunTest<Mod998244353>([2, 3, 4, 5, 6]);
        await RunTest<Mod998244353>([2, 3, 4]);
        await RunTest<Mod998244353>([0, 0, 2, 3, 4, 5, 6]);
        await RunTest<Mod998244353>([]);

        await RunTest<Mod1000000007>([2, 3, 4]);
        await RunTest<Mod1000000007>([2, 3, 4, 5, 6]);
        await RunTest<Mod1000000007>([0, 0, 2, 3, 4, 5, 6]);
        await RunTest<Mod1000000007>([]);

        static async Task RunTest<T>(int[] fpsArray) where T : struct, IStaticMod
        {
            var fac = new ModIntFactor<MontgomeryModInt<T>>(fpsArray.Length);
            var f = new FormalPowerSeries<T>(fpsArray);
            var rnd = new Random(227);

            for (int shift = -20; shift < 20; shift++)
            {
                var g = f.TaylorShift(shift, fac);
                for (int i = 0; i < 1000; i++)
                {
                    MontgomeryModInt<T> x = rnd.Next();
                    await f.Eval(x + shift).Should().BeEqualTo(g.Eval(x));
                }
            }
        }
    }
}