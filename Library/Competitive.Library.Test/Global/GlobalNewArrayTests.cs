namespace Kzrnm.Competitive.Testing.GlobalNS;

public class GlobalNewArrayTests
{
    [Test, MultipleAssertions]
    public async Task NewArray1()
    {
        var arr = Global.NewArray(2, 1);
        await arr.Length.Should().BeEqualTo(2);
        await arr.Should().BeStrictlyEquivalentTo(Enumerable.Repeat(1, 2));
    }
    [Test, MultipleAssertions]
    public async Task NewArrayFunc1()
    {
        var arr = Global.NewArray(2, () => new object());
        await arr.Length.Should().BeEqualTo(2);
        await arr.Distinct().Count().Should().BeEqualTo(2);
    }
    [Test, MultipleAssertions]
    public async Task NewArray2()
    {
        var arr = Global.NewArray(2, 3, 1);
        await arr.SelectMany(a => a).Count().Should().BeEqualTo(6);
        await arr.SelectMany(a => a).Should().BeStrictlyEquivalentTo(Enumerable.Repeat(1, 6));
    }
    [Test, MultipleAssertions]
    public async Task NewArrayFunc2()
    {
        var arr = Global.NewArray(2, 3, () => new object());
        await arr.SelectMany(a => a).Count().Should().BeEqualTo(6);
        await arr.SelectMany(a => a).Distinct().Count().Should().BeEqualTo(6);
    }
    [Test, MultipleAssertions]
    public async Task NewArray3()
    {
        var arr = Global.NewArray(2, 3, 5, 1);
        await arr.SelectMany(a => a).SelectMany(a => a).Count().Should().BeEqualTo(30);
        await arr.SelectMany(a => a).SelectMany(a => a).Should().BeStrictlyEquivalentTo(Enumerable.Repeat(1, 30));
    }
    [Test, MultipleAssertions]
    public async Task NewArrayFunc3()
    {
        var arr = Global.NewArray(2, 3, 5, () => new object());
        await arr.SelectMany(a => a).SelectMany(a => a).Count().Should().BeEqualTo(30);
        await arr.SelectMany(a => a).SelectMany(a => a).Distinct().Count().Should().BeEqualTo(30);
    }
    [Test, MultipleAssertions]
    public async Task NewArray4()
    {
        var arr = Global.NewArray(2, 3, 5, 7, 1);
        await arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Count().Should().BeEqualTo(210);
        await arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Should().BeStrictlyEquivalentTo(Enumerable.Repeat(1, 210));
    }
    [Test, MultipleAssertions]
    public async Task NewArrayFunc4()
    {
        var arr = Global.NewArray(2, 3, 5, 7, () => new object());
        await arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Count().Should().BeEqualTo(210);
        await arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Distinct().Count().Should().BeEqualTo(210);
    }
}