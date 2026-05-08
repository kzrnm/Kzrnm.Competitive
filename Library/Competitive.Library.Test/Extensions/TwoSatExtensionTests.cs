using AtCoder;

namespace Kzrnm.Competitive.Testing.Extensions;

public class TwoSatExtensionTests
{
    [Test, MultipleAssertions]
    public async Task And_all()
    {
        var twoSat = new TwoSat(3);
        twoSat.And(0, true, 1, true);
        twoSat.And(2, true, 1, true);

        await twoSat.Satisfiable().Should().BeTrue();
        await twoSat.Answer().Should().BeStrictlyEquivalentTo([true, true, true]);
    }
    [Test, MultipleAssertions]
    public async Task And_set1()
    {
        var twoSat = new TwoSat(3);
        twoSat.And(0, true, 1, true);
        twoSat.Set(1, false);

        await twoSat.Satisfiable().Should().BeFalse();
    }
    [Test, MultipleAssertions]
    public async Task And_set2()
    {
        var twoSat = new TwoSat(3);
        twoSat.And(0, true, 1, true);
        twoSat.Set(2, false);

        await twoSat.Satisfiable().Should().BeTrue();
        await twoSat.Answer().Should().BeStrictlyEquivalentTo([true, true, false]);
    }
    [Test, MultipleAssertions]
    public async Task Same()
    {
        var twoSat = new TwoSat(3);
        twoSat.Same(0, 1);
        twoSat.And(1, true, 2, false);

        await twoSat.Satisfiable().Should().BeTrue();
        await twoSat.Answer().Should().BeStrictlyEquivalentTo([true, true, false]);
    }
    [Test, MultipleAssertions]
    public async Task NotSame()
    {
        var twoSat = new TwoSat(3);
        twoSat.NotSame(0, 1);
        twoSat.And(1, true, 2, false);

        await twoSat.Satisfiable().Should().BeTrue();
        await twoSat.Answer().Should().BeStrictlyEquivalentTo([false, true, false]);
    }
    [Test, MultipleAssertions]
    public async Task IfThen1()
    {
        var twoSat = new TwoSat(3);
        twoSat.IfThen(0, true, 1, false);
        twoSat.And(0, true, 2, false);

        await twoSat.Satisfiable().Should().BeTrue();
        await twoSat.Answer().Should().BeStrictlyEquivalentTo([true, false, false]);
    }
    [Test, MultipleAssertions]
    public async Task IfThen2()
    {
        var twoSat = new TwoSat(3);
        twoSat.IfThen(0, true, 1, false);
        twoSat.And(1, true, 2, false);

        await twoSat.Satisfiable().Should().BeTrue();
        await twoSat.Answer().Should().BeStrictlyEquivalentTo([false, true, false]);
    }
}