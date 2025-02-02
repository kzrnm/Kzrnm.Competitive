using AtCoder.Internal;
using System;

namespace Kzrnm.Competitive.Testing.Number;

public class ModLogTests
{
    [Theory]
    [InlineData(2, 1, 5, 0)]
    [InlineData(4, 7, 10, -1)]
    [InlineData(8, 6, 10, 4)]
    [InlineData(5, 2, 11, -1)]
    [InlineData(5, 9, 11, 4)]
    [InlineData(0, 2, 4, -1)]
    [InlineData(0, 0, 1, 0)]
    [InlineData(0, 0, 2, 1)]
    [InlineData(0, 1, 2, 0)]
    [InlineData(0, 0, 4, 1)]
    [InlineData(0, 1, 4, 0)]
    [InlineData(2, 0, 4, 2)]
    [InlineData(2, 1, 4, 0)]
    [InlineData(2, 2, 4, 1)]
    [InlineData(2, 3, 4, -1)]
    public void Solve(long a, long b, long p, long expected)
    {
        ModLog.Solve(a, b, p).ShouldBe(expected, $"a={a}, b={b}, p={p}, expected={expected}");
    }

    [Fact]
    public void Random()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            var p = rnd.Next(1, 1000000);
            var a = rnd.Next(p);
            long b = 1;
            for (int i = 0; i < 10; i++)
            {
                ModLog.Solve(a, b, p).ShouldBe(i, $"a={a}, b={b}, p={p}, expected={i}");
                b = b * a % p;
            }
        }
    }

    [Fact]
    public void Mod1()
    {
        ModLog.Solve(0, 0, 1).ShouldBe(0);
        ModLog.Solve(0, 0, 1, false).ShouldBe(1);
    }

    [Fact]
    public void Small()
    {
        for (int p = 2; p < 10; p++)
            for (int a = 0; a < p; a++)
                for (int b = 0; b < p; b++)
                {
                    var expected = Native(a, b, p, true);
                    ModLog.Solve(a, b, p).ShouldBe(expected, $"a={a}, b={b}, p={p}, expected={expected}");

                    expected = Native(a, b, p, false);
                    ModLog.Solve(a, b, p, false).ShouldBe(expected, $"a={a}, b={b}, p={p}, expected={expected}");
                }
    }

    static long Native(long a, long b, int p, bool includeZero)
    {
        for (int i = includeZero ? 0 : 1; i < p; i++)
            if (ModCalc.PowMod(a, i, p) == b)
                return i;
        return -1;
    }
}
