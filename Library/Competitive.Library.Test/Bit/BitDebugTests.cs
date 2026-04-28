namespace Kzrnm.Competitive.Testing.Bit;

public class BitDebugTests
{
    [Test, MultipleAssertions]
    public async Task IntArray()
    {
        var bd = new BitDebug(Enumerable.Range(0, 10).ToArray());
        await bd.Items.Should().HaveCount(10);
        await bd.Items[0].Should().BeEqualTo(new("0000 [0]", 0));
        await bd.Items[1].Should().BeEqualTo(new("0001 [1]", 1));
        await bd.Items[2].Should().BeEqualTo(new("0010 [2]", 2));
        await bd.Items[3].Should().BeEqualTo(new("0011 [3]", 3));
        await bd.Items[4].Should().BeEqualTo(new("0100 [4]", 4));
        await bd.Items[5].Should().BeEqualTo(new("0101 [5]", 5));
        await bd.Items[6].Should().BeEqualTo(new("0110 [6]", 6));
        await bd.Items[7].Should().BeEqualTo(new("0111 [7]", 7));
        await bd.Items[8].Should().BeEqualTo(new("1000 [8]", 8));
        await bd.Items[9].Should().BeEqualTo(new("1001 [9]", 9));
    }
    [Test, MultipleAssertions]
    public async Task StringArray()
    {
        var bd = new BitDebug(Enumerable.Range(0, 6).Select(n => n.ToString()).ToArray());
        await bd.Items.Should().HaveCount(6);
        await bd.Items[0].Should().BeEqualTo(new("000 [0]", "0"));
        await bd.Items[1].Should().BeEqualTo(new("001 [1]", "1"));
        await bd.Items[2].Should().BeEqualTo(new("010 [2]", "2"));
        await bd.Items[3].Should().BeEqualTo(new("011 [3]", "3"));
        await bd.Items[4].Should().BeEqualTo(new("100 [4]", "4"));
        await bd.Items[5].Should().BeEqualTo(new("101 [5]", "5"));
    }
}