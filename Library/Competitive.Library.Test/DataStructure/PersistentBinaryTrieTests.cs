
namespace Kzrnm.Competitive.Testing.DataStructure;

public class PersistentBinaryTrieTests
{
    [Test, MultipleAssertions]
    public async Task Simple()
    {
        var bt = new PersistentBinaryTrie();
        bt = bt.Add(1, 2);
        bt = bt.Add(2);
        bt = bt.Add(3);
        bt = bt.Remove(3);
        bt = bt.Add(5);
        bt.Add(3).Add(6).Remove(2);

        await bt.KthElement(0).Num.Should().BeEqualTo(1u);
        await bt.KthElement(1).Num.Should().BeEqualTo(1u);
        await bt.KthElement(2).Num.Should().BeEqualTo(2u);
        await bt.KthElement(3).Num.Should().BeEqualTo(5u);

        await bt.MinElement().Num.Should().BeEqualTo(1u);
        await bt.MaxElement().Num.Should().BeEqualTo(5u);

        await bt.Count(1).Should().BeEqualTo(2);
        await bt.Count(2).Should().BeEqualTo(1);
        await bt.Count(5).Should().BeEqualTo(1);

        await bt.CountLess(1).Should().BeEqualTo(0);
        await bt.CountLess(2).Should().BeEqualTo(2);
        await bt.CountLess(3).Should().BeEqualTo(3);
        await bt.CountLess(5).Should().BeEqualTo(3);
        await bt.CountLess(6).Should().BeEqualTo(4);



        await bt.KthElement(0, 4).Num.Should().BeEqualTo(1u);
        await bt.KthElement(1, 4).Num.Should().BeEqualTo(5u);
        await bt.KthElement(2, 4).Num.Should().BeEqualTo(5u);
        await bt.KthElement(3, 4).Num.Should().BeEqualTo(6u);

        await bt.MinElement(4).Num.Should().BeEqualTo(1u);
        await bt.MaxElement(4).Num.Should().BeEqualTo(6u);

        await bt.Count(1, 4).Should().BeEqualTo(1);
        await bt.Count(2, 4).Should().BeEqualTo(0);
        await bt.Count(5, 4).Should().BeEqualTo(2);
        await bt.Count(6, 4).Should().BeEqualTo(1);

        await bt.CountLess(1, 4).Should().BeEqualTo(0);
        await bt.CountLess(2, 4).Should().BeEqualTo(1);
        await bt.CountLess(3, 4).Should().BeEqualTo(1);
        await bt.CountLess(5, 4).Should().BeEqualTo(1);
        await bt.CountLess(6, 4).Should().BeEqualTo(3);
        await bt.CountLess(7, 4).Should().BeEqualTo(4);

        bt = bt.Remove(2);
        await bt.KthElement(0).Num.Should().BeEqualTo(1u);
        await bt.KthElement(1).Num.Should().BeEqualTo(1u);
        await bt.KthElement(2).Num.Should().BeEqualTo(5u);

        await bt.MinElement().Num.Should().BeEqualTo(1u);
        await bt.MaxElement().Num.Should().BeEqualTo(5u);

        await bt.Count(1).Should().BeEqualTo(2);
        await bt.Count(5).Should().BeEqualTo(1);

        await bt.CountLess(1).Should().BeEqualTo(0);
        await bt.CountLess(2).Should().BeEqualTo(2);
        await bt.CountLess(5).Should().BeEqualTo(2);
        await bt.CountLess(6).Should().BeEqualTo(3);
    }
}