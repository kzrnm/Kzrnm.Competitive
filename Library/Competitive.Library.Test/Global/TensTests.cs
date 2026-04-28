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
        await Tens.Longs[0].Should().BeEqualTo(1);
        for (int i = 1; i < Tens.Longs.Length; i++)
        {
            await Tens.Longs[i].Should().BeEqualTo(Tens.Longs[i - 1] * 10);
        }
    }
}