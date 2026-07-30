namespace Kzrnm.Competitive.Testing.GlobalNS;

public class TensTests
{
    [Test, MultipleAssertions]
    public async Task Ints()
    {
        await Tens.Ints[0].Should().BeEqualTo(1);
        for (int i = 1; i < Tens.Ints.Length; i++)
        {
            await Tens.Ints[i].Should().BeEqualTo(Tens.Ints[i - 1] * 10);
        }
    }
    [Test, MultipleAssertions]
    public async Task Longs()
    {
        await Tens.ULongs[0].Should().BeEqualTo(1ul);
        for (int i = 1; i < Tens.ULongs.Length; i++)
        {
            await Tens.ULongs[i].Should().BeEqualTo(Tens.ULongs[i - 1] * 10);
        }
    }
}