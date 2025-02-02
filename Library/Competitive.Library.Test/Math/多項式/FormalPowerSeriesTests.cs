using AtCoder;
using System;
using System.Linq;

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

    [Fact]
    public void Coefficients()
    {
        RunTest<Mod998244353>([1, 2, 3]);
        RunTest<Mod998244353>([0, 5, 0, 2]);
        RunTest<Mod998244353>([]);

        RunTest<Mod1000000007>([1, 2, 3]);
        RunTest<Mod1000000007>([0, 5, 0, 2]);
        RunTest<Mod1000000007>([]);

        static void RunTest<T>(int[] array) where T : struct, IStaticMod
        {
            var modArray = array.Select(v => (MontgomeryModInt<T>)v).ToArray();
            var f = new FormalPowerSeries<T>(array);
            f.Coefficients().ToArray().ShouldBe(modArray);

            for (int len = 0; len < modArray.Length; len++)
            {
                f.Coefficients(len).ToArray().ShouldBe(modArray[..len]);
            }

            f.Coefficients(modArray.Length + 2).ToArray().ShouldBe(modArray.Append(default).Append(default));
        }
    }

    [Fact]
    public void Add()
    {
        RunTest<Mod998244353>([1, 2, 3], [0, 5, 0, 2], [1, 7, 3, 2]);
        RunTest<Mod998244353>([0, 5, 0, 2], [1, 2, 3], [1, 7, 3, 2]);
        RunTest<Mod998244353>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        RunTest<Mod998244353>([], [0, 5, 0, 2], [0, 5, 0, 2]);

        RunTest<Mod1000000007>([1, 2, 3], [0, 5, 0, 2], [1, 7, 3, 2]);
        RunTest<Mod1000000007>([0, 5, 0, 2], [1, 2, 3], [1, 7, 3, 2]);
        RunTest<Mod1000000007>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        RunTest<Mod1000000007>([], [0, 5, 0, 2], [0, 5, 0, 2]);

        static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var orig = lhs._cs.ToArray();

                (lhs + rhs)._cs.ShouldBe(expected._cs);
                (lhs + rhs._cs)._cs.ShouldBe(expected._cs);

                lhs._cs.ShouldBe(orig);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                lhs.AddSelf(rhs)._cs.ShouldBe(expected._cs);
                lhs._cs.ShouldBe(expected._cs);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                lhs.AddSelf(rhs._cs)._cs.ShouldBe(expected._cs);
                lhs._cs.ShouldBe(expected._cs);
            }
        }
    }
    [Fact]
    public void Subtract()
    {
        RunTest<Mod998244353>([1, 2, 3], [0, 5, 0, 2], [1, -3, 3, -2]);
        RunTest<Mod998244353>([0, 5, 0, 2], [1, 2, 3], [-1, 3, -3, 2]);
        RunTest<Mod998244353>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        RunTest<Mod998244353>([], [0, 5, 0, 2], [0, -5, 0, -2]);

        RunTest<Mod1000000007>([1, 2, 3], [0, 5, 0, 2], [1, -3, 3, -2]);
        RunTest<Mod1000000007>([0, 5, 0, 2], [1, 2, 3], [-1, 3, -3, 2]);
        RunTest<Mod1000000007>([0, 5, 0, 2], [], [0, 5, 0, 2]);
        RunTest<Mod1000000007>([], [0, 5, 0, 2], [0, -5, 0, -2]);

        static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var orig = lhs._cs.ToArray();

                (lhs - rhs)._cs.ShouldBe(expected._cs);
                (lhs - rhs._cs)._cs.ShouldBe(expected._cs);

                lhs._cs.ShouldBe(orig);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                lhs.SubtractSelf(rhs)._cs.ShouldBe(expected._cs);
                lhs._cs.ShouldBe(expected._cs);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                lhs.SubtractSelf(rhs._cs)._cs.ShouldBe(expected._cs);
                lhs._cs.ShouldBe(expected._cs);
            }
        }
    }
    [Fact]
    public void Minus()
    {
        RunTest<Mod998244353>([0, 1, 2, 3], [0, -1, -2, -3]);
        RunTest<Mod998244353>([0, -1, -2, -3], [0, 1, 2, 3]);
        RunTest<Mod998244353>([], []);

        RunTest<Mod1000000007>([0, 1, 2, 3], [0, -1, -2, -3]);
        RunTest<Mod1000000007>([0, -1, -2, -3], [0, 1, 2, 3]);
        RunTest<Mod1000000007>([], []);

        static void RunTest<T>(int[] valueArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var value = new FormalPowerSeries<T>(valueArray.Select(n => new MontgomeryModInt<T>(n)).ToArray());
            var expected = new FormalPowerSeries<T>(expectedArray.Select(n => new MontgomeryModInt<T>(n)).ToArray());

            (-value)._cs.ShouldBe(expected._cs);
        }
    }
    [Fact]
    public void Multiply()
    {
        RunTest<Mod998244353>([1, 2, 3], [0, 5, 0, 2], [0, 5, 10, 17, 4, 6]);
        RunTest<Mod998244353>([0, 5, 0, 2], [1, 2, 3], [0, 5, 10, 17, 4, 6]);
        RunTest<Mod998244353>([0, 5, 0, 2], [], []);
        RunTest<Mod998244353>([], [0, 5, 0, 2], []);

        RunTest<Mod1000000007>([1, 2, 3], [0, 5, 0, 2], [0, 5, 10, 17, 4, 6]);
        RunTest<Mod1000000007>([0, 5, 0, 2], [1, 2, 3], [0, 5, 10, 17, 4, 6]);
        RunTest<Mod1000000007>([0, 5, 0, 2], [], []);
        RunTest<Mod1000000007>([], [0, 5, 0, 2], []);

        static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var orig = lhs._cs.ToArray();

                (lhs * rhs)._cs.ShouldBe(expected._cs);
                (lhs * rhs._cs)._cs.ShouldBe(expected._cs);

                lhs._cs.ShouldBe(orig);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                lhs.MultiplySelf(rhs)._cs.ShouldBe(expected._cs);
                lhs._cs.ShouldBe(expected._cs);
            }
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                lhs.MultiplySelf(rhs._cs)._cs.ShouldBe(expected._cs);
                lhs._cs.ShouldBe(expected._cs);
            }
        }
    }

    [Fact]
    public void Divide()
    {
        RunTest<Mod998244353>([0, 5, 10, 17, 4, 6], [0, 5, 0, 2], [1, 2, 3]);
        RunTest<Mod998244353>([0, 5, 10, 17, 4, 6], [1, 2, 3], [0, 5, 0, 2]);
        RunTest<Mod998244353>([1, 2, 3], [0, 5, 10, 17, 4, 6], []);
        RunTest<Mod998244353>([], [0, 5, 10, 17, 4, 6], []);

        RunTest<Mod1000000007>([0, 5, 10, 17, 4, 6], [0, 5, 0, 2], [1, 2, 3]);
        RunTest<Mod1000000007>([0, 5, 10, 17, 4, 6], [1, 2, 3], [0, 5, 0, 2]);
        RunTest<Mod1000000007>([1, 2, 3], [0, 5, 10, 17, 4, 6], []);
        RunTest<Mod1000000007>([], [0, 5, 10, 17, 4, 6], []);


        static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var lhs = new FormalPowerSeries<T>(lhsArray);
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var expected = new FormalPowerSeries<T>(expectedArray);

            (lhs / rhs)._cs.ShouldBe(expected._cs);
        }
    }

    [Fact]
    public void DivRem()
    {
        var rnd = new Random(227);
        const int N = 130;
        var lhs = new int[N];
        var rhs = new int[N + 1];
        var rem = new int[N];
        foreach (ref var v in lhs.AsSpan()) v = rnd.Next(1, 1000000);
        foreach (ref var v in rhs.AsSpan()) v = rnd.Next(1, 1000000);
        foreach (ref var v in rem.AsSpan()) v = rnd.Next(1, 1000000);

        RunTest<Mod998244353>(lhs, rhs, rem);
        RunTest<Mod1000000007>(lhs, rhs, rem);

        static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] remArray) where T : struct, IStaticMod
        {
            var lhs = new FormalPowerSeries<T>(lhsArray);
            var rhs = new FormalPowerSeries<T>(rhsArray);
            var rem = new FormalPowerSeries<T>(remArray);

            var p = lhs * rhs + rem;
            var (q, r) = p.DivRem(rhs);

            q._cs.ShouldBe((p / rhs)._cs);
            r._cs.ShouldBe((p % rhs)._cs);
            (q * rhs + r)._cs.ShouldBe(p._cs);
        }
    }

    [Fact]
    public void RightShift()
    {
        RunTest<Mod998244353>([1, 2, 3], 0, [1, 2, 3]);
        RunTest<Mod998244353>([1, 2, 3], 1, [2, 3]);
        RunTest<Mod998244353>([1, 2, 3], 2, [3]);
        RunTest<Mod998244353>([1, 2, 3], 3, []);

        RunTest<Mod1000000007>([1, 2, 3], 0, [1, 2, 3]);
        RunTest<Mod1000000007>([1, 2, 3], 1, [2, 3]);
        RunTest<Mod1000000007>([1, 2, 3], 2, [3]);
        RunTest<Mod1000000007>([1, 2, 3], 3, []);

        static void RunTest<T>(int[] valueArray, int shift, int[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            var expected = new FormalPowerSeries<T>(expectedArray);

            (fps >> shift)._cs.ShouldBe(expected._cs);
        }
    }

    [Fact]
    public void LeftShift()
    {
        RunTest<Mod998244353>([1, 2, 3], 0, [1, 2, 3]);
        RunTest<Mod998244353>([1, 2, 3], 1, [0, 1, 2, 3]);
        RunTest<Mod998244353>([1, 2, 3], 4, [0, 0, 0, 0, 1, 2, 3]);

        RunTest<Mod1000000007>([1, 2, 3], 0, [1, 2, 3]);
        RunTest<Mod1000000007>([1, 2, 3], 1, [0, 1, 2, 3]);
        RunTest<Mod1000000007>([1, 2, 3], 4, [0, 0, 0, 0, 1, 2, 3]);

        static void RunTest<T>(int[] valueArray, int shift, int[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            var expected = new FormalPowerSeries<T>(expectedArray);

            (fps << shift)._cs.ShouldBe(expected._cs);
        }
    }

    [Fact]
    public void Derivative()
    {
        RunTest<Mod998244353>([3, 5, 10, 17, 4, 6], [5, 20, 51, 16, 30]);
        RunTest<Mod998244353>([3], []);
        RunTest<Mod998244353>([], []);

        RunTest<Mod1000000007>([3, 5, 10, 17, 4, 6], [5, 20, 51, 16, 30]);
        RunTest<Mod1000000007>([3], []);
        RunTest<Mod1000000007>([], []);

        static void RunTest<T>(int[] valueArray, int[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            fps.Derivative()._cs.ShouldBe(expectedArray.Select(t => new MontgomeryModInt<T>(t)).ToArray());
        }
    }

    [Fact]
    public void Integrate()
    {
        RunTest<Mod998244353>([5, 20, 51, 16, 30], [(0, 1), (5, 1), (10, 1), (17, 1), (4, 1), (6, 1)]);
        RunTest<Mod998244353>([1, 1, 1, 1], [(0, 1), (1, 1), (1, 2), (1, 3), (1, 4)]);
        RunTest<Mod998244353>([3], [(0, 1), (3, 1)]);
        RunTest<Mod998244353>([], []);

        RunTest<Mod1000000007>([5, 20, 51, 16, 30], [(0, 1), (5, 1), (10, 1), (17, 1), (4, 1), (6, 1)]);
        RunTest<Mod1000000007>([1, 1, 1, 1], [(0, 1), (1, 1), (1, 2), (1, 3), (1, 4)]);
        RunTest<Mod1000000007>([3], [(0, 1), (3, 1)]);
        RunTest<Mod1000000007>([], []);

        static void RunTest<T>(int[] valueArray, (int Numerator, int Denominator)[] expectedArray) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(valueArray);
            fps.Integrate()._cs.ShouldBe(expectedArray.Select(t => new MontgomeryModInt<T>(t.Numerator) / t.Denominator).ToArray());
        }
    }

    [Fact]
    public void Eval()
    {
        RunTest<Mod998244353>([5, 20, 51, 16, 30], 7, 80162);
        RunTest<Mod998244353>([5, 20, 51, 16, 30], 8, 134501);
        RunTest<Mod998244353>([5, 20, 51, 16, 30], 9, 212810);
        RunTest<Mod998244353>([], 9, 0);

        RunTest<Mod1000000007>([5, 20, 51, 16, 30], 7, 80162);
        RunTest<Mod1000000007>([5, 20, 51, 16, 30], 8, 134501);
        RunTest<Mod1000000007>([5, 20, 51, 16, 30], 9, 212810);
        RunTest<Mod1000000007>([], 9, 0);

        static void RunTest<T>(int[] fpsArray, MontgomeryModInt<T> x, MontgomeryModInt<T> expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            fps.Eval(x).ShouldBe(expected);
        }
    }

    [Fact]
    public void Inv()
    {
        RunTest<Mod998244353>(
            [5, 4, 3, 2, 1],
            [598946612, 718735934, 862483121, 635682004, 163871793]);

        RunTest<Mod1000000007>(
            [5, 4, 3, 2, 1],
            [400000003, 880000006, 856000006, 427200003, 712640005]);

        static void RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            fps.Inv()._cs.ShouldBe(new FormalPowerSeries<T>(expected)._cs);
        }
    }

    [Fact]
    public void Exp()
    {
        RunTest<Mod998244353>([0, 1, 2, 3, 4], [1, 1, 499122179, 166374064, 291154613]);
        RunTest<Mod998244353>(
            [0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549],
            [1, 907649120, 316060452, 57037696, 378993419, 302467176, 349948335, 115795520, 647455105, 497971134]);
        RunTest<Mod998244353>([0], [1]);
        RunTest<Mod998244353>([], [1]);

        RunTest<Mod1000000007>([0, 1, 2, 3, 4], [1, 1, 500000006, 166666673, 41666677]);
        RunTest<Mod1000000007>(
            [0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549],
            [1, 907649120, 925644116, 38331988, 156875359, 697776255, 802320078, 499725651, 949053640, 121509191]);
        RunTest<Mod1000000007>([0], [1]);
        RunTest<Mod1000000007>([], [1]);

        static void RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            fps.Exp()._cs.ShouldBe(new FormalPowerSeries<T>(expected)._cs);
        }
    }

    [Fact]
    public void Log()
    {
        RunTest<Mod998244353>([1, 1, 499122179, 166374064, 291154613], [0, 1, 2, 3, 4]);
        RunTest<Mod998244353>(
            [1, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549],
            [0, 907649120, 265241806, 491547518, 331811826, 54791043, 895176577, 142597055, 60021098, 768274455]);
        RunTest<Mod998244353>(Enumerable.Repeat(0, 50000).Prepend(1).ToArray(), [0]);

        RunTest<Mod1000000007>([1, 1, 500000006, 166666673, 41666677], [0, 1, 2, 3, 4]);
        RunTest<Mod1000000007>(
            [1, 907649120, 925644116, 38331988, 156875359, 697776255, 802320078, 499725651, 949053640, 121509191],
            [0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549]);

        static void RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            fps.Log()._cs.ShouldBe(new FormalPowerSeries<T>(expected)._cs);
        }
    }

    [Fact]
    public void Pow()
    {
        RunTest<Mod998244353>(
            [2, 3, 4, 5, 6], 2,
            [4, 12, 25, 44, 70]);
        RunTest<Mod998244353>(
            [2, 3, 4, 5, 6], 3,
            [8, 36, 102, 231, 456]);
        RunTest<Mod998244353>(
            [0, 0, 2, 3, 4, 5, 6], 2,
            [0, 0, 0, 0, 4, 12, 25]);
        RunTest<Mod998244353>([], 2, []);

        RunTest<Mod1000000007>(
            [2, 3, 4, 5, 6], 2,
            [4, 12, 25, 44, 70]);
        RunTest<Mod1000000007>(
            [2, 3, 4, 5, 6], 3,
            [8, 36, 102, 231, 456]);
        RunTest<Mod1000000007>(
            [0, 0, 2, 3, 4, 5, 6], 2,
            [0, 0, 0, 0, 4, 12, 25]);
        RunTest<Mod1000000007>([], 2, []);

        new FormalPowerSeries<Mod998244353>([2, 3, 4, 5, 6])
            .Pow(3, 13)._cs.ShouldBe([8, 36, 102, 231, 456, 735, 1024, 1257, 1344, 1169, 882, 540, 216]);
        new FormalPowerSeries<Mod998244353>([0, 0, 2, 3, 4, 5, 6])
            .Pow(2, 13)._cs.ShouldBe([0, 0, 0, 0, 4, 12, 25, 44, 70, 76, 73, 60, 36]);

        new FormalPowerSeries<Mod1000000007>([2, 3, 4, 5, 6])
            .Pow(3, 13)._cs.ShouldBe([8, 36, 102, 231, 456, 735, 1024, 1257, 1344, 1169, 882, 540, 216]);
        new FormalPowerSeries<Mod1000000007>([0, 0, 2, 3, 4, 5, 6])
            .Pow(2, 13)._cs.ShouldBe([0, 0, 0, 0, 4, 12, 25, 44, 70, 76, 73, 60, 36]);

        static void RunTest<T>(int[] fpsArray, int n, int[] expected) where T : struct, IStaticMod
        {
            var fps = new FormalPowerSeries<T>(fpsArray);
            fps.Pow(n)._cs.ShouldBe(new FormalPowerSeries<T>(expected)._cs);
        }
    }


    [Fact]
    public void TaylorShift()
    {
        RunTest<Mod998244353>([2, 3, 4, 5, 6]);
        RunTest<Mod998244353>([2, 3, 4]);
        RunTest<Mod998244353>([0, 0, 2, 3, 4, 5, 6]);
        RunTest<Mod998244353>([]);

        RunTest<Mod1000000007>([2, 3, 4]);
        RunTest<Mod1000000007>([2, 3, 4, 5, 6]);
        RunTest<Mod1000000007>([0, 0, 2, 3, 4, 5, 6]);
        RunTest<Mod1000000007>([]);

        static void RunTest<T>(int[] fpsArray) where T : struct, IStaticMod
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
                    f.Eval(x + shift).ShouldBe(g.Eval(x));
                }
            }
        }
    }
}
