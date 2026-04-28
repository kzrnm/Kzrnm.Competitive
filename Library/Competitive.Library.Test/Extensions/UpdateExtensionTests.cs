namespace Kzrnm.Competitive.Testing.Extensions;

public class UpdateExtensionTests
{
    [Test, MultipleAssertions]
    public async Task UpdateMax()
    {
        int a = 0;
        await a.UpdateMax(10).Should().BeTrue();
        await a.Should().BeEqualTo(10);
        await a.UpdateMax(0).Should().BeFalse();
        await a.Should().BeEqualTo(10);

        var d = new DateTime(2000, 1, 1);
        await d.UpdateMax(new DateTime(2001, 1, 1)).Should().BeTrue();
        await d.Should().BeEqualTo(new DateTime(2001, 1, 1));
        await d.UpdateMax(new DateTime(2000, 12, 1)).Should().BeFalse();
        await d.Should().BeEqualTo(new DateTime(2001, 1, 1));
    }

    [Test, MultipleAssertions]
    public async Task UpdateMin()
    {
        int a = 0;
        await a.UpdateMin(-10).Should().BeTrue();
        await a.Should().BeEqualTo(-10);
        await a.UpdateMin(0).Should().BeFalse();
        await a.Should().BeEqualTo(-10);

        DateTime d = new(2000, 1, 1);
        await d.UpdateMin(new DateTime(1999, 1, 1)).Should().BeTrue();
        await d.Should().BeEqualTo(new DateTime(1999, 1, 1));
        await d.UpdateMin(new DateTime(2000, 12, 1)).Should().BeFalse();
        await d.Should().BeEqualTo(new DateTime(1999, 1, 1));
    }


    [Test, MultipleAssertions]
    public async Task UpdateValues()
    {
        var rnd = new Random(227);
        for (int i = 0; i < 1000; i++)
        {
            var bytes = new byte[20];
            rnd.NextBytes(bytes);
            bytes[0] = byte.MaxValue >> 1;
            byte num;
            {
                num = 0;
                await num.UpdateMin(bytes).Should().BeFalse();
                await num.Should().BeEqualTo(default);
            }
            {
                num = 0;
                await num.UpdateMax(bytes).Should().BeTrue();
                await num.Should().BeEqualTo(bytes.Max());
            }
            {
                num = byte.MaxValue;
                await num.UpdateMin(bytes).Should().BeTrue();
                await num.Should().BeEqualTo(bytes.Min());
            }
            {
                num = byte.MaxValue;
                await num.UpdateMax(bytes).Should().BeFalse();
                await num.Should().BeEqualTo(byte.MaxValue);
            }
        }
    }
}