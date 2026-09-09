using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing.Algorithm;

public class GoldenSectionSearchTests
{
    [Test]
    [Arguments(0, 3 * Math.PI / 2)]
    [Arguments(0, 1.1 * Math.PI / 2)]
    [Arguments(0.9 * Math.PI / 2, 1.1 * Math.PI / 2)]
    [Arguments(0.9 * Math.PI / 2, 3 * Math.PI / 2)]
    public async Task Double(double f, double t)
    {
        await GoldenSectionSearch.ExtremumDouble<SinOp>(f, t).Should().BeCloseTo(Math.PI / 2, 1e-8);
    }
    readonly struct SinOp : IGoldenSectionSearchFunction<double, double>
    {
        public double Value(double v) => -Math.Sin(v);
    }


    [Test]
    [Arguments(0, 4712388)]
    [Arguments(0, 3524578)]
    [Arguments(1187810, 4712388)]
    [Arguments(-1512387, 1612388)]
    public async Task Long(long f, long t)
    {
        await GoldenSectionSearch.Extremum<long, double, LongSinOp>(f, t).Should().BeEqualTo(1570796);
    }

    [Test]
    public async Task Array()
    {
        await GoldenSectionSearch.Extremum(Enumerable.Range(0, 4712388).Select(v => Math.Sin(v / 1e6)).ToArray(), true).Should().BeEqualTo(1570796);
        await GoldenSectionSearch.Extremum(Enumerable.Range(0, 4712388).Select(v => -Math.Sin(v / 1e6)).ToArray(), false).Should().BeEqualTo(1570796);
    }
    readonly struct LongSinOp : IGoldenSectionSearchFunction<long, double>
    {
        public double Value(long v) => -Math.Sin(v / 1e6);
    }
}