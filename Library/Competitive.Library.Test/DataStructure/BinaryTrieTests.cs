namespace Kzrnm.Competitive.Testing.DataStructure;

public class BinaryTrieTests
{
    [Test, MultipleAssertions]
    public async Task Simple()
    {
        var bt = new BinaryTrie();
        bt.Add(1, 2);
        bt.Increment(2);
        bt.Increment(5);

        await bt.KthElement(0).Num.Should().BeEqualTo(1u);
        await bt.KthElement(1).Num.Should().BeEqualTo(1u);
        await bt.KthElement(2).Num.Should().BeEqualTo(2u);
        await bt.KthElement(3).Num.Should().BeEqualTo(5u);

        await bt.MinElement().Num.Should().BeEqualTo(1u);
        await bt.MaxElement().Num.Should().BeEqualTo(5u);

        await bt.Count(0).Should().BeEqualTo(0);
        await bt.Count(1).Should().BeEqualTo(2);
        await bt.Count(2).Should().BeEqualTo(1);
        await bt.Count(3).Should().BeEqualTo(0);
        await bt.Count(4).Should().BeEqualTo(0);
        await bt.Count(5).Should().BeEqualTo(1);
        await bt.Count(6).Should().BeEqualTo(0);
        await bt.Count(ulong.MaxValue).Should().BeEqualTo(0);

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

        await bt.CountLess(1, 4).Should().BeEqualTo(0);
        await bt.CountLess(2, 4).Should().BeEqualTo(1);
        await bt.CountLess(3, 4).Should().BeEqualTo(1);
        await bt.CountLess(5, 4).Should().BeEqualTo(1);
        await bt.CountLess(6, 4).Should().BeEqualTo(3);
        await bt.CountLess(7, 4).Should().BeEqualTo(4);

        bt.Decrement(2);
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

    [Test, MultipleAssertions]
    public async Task Byte()
    {
        var bt = new BinaryTrie<byte>(3);
        var nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
        await nodes[0].Should().BeNull();
        await nodes[1].Should().BeNull();
        await nodes[2].Should().BeNull();
        await nodes[3].Should().BeNull();
        await nodes[4].Should().BeNull();
        await nodes[5].Should().BeNull();
        await nodes[6].Should().BeNull();
        await nodes[7].Should().BeNull();

        bt.Increment(1);
        nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
        await nodes[0].Should().BeNull();
        await nodes[1].Accepts.ToArray().Should().BeEmpty();
        await nodes[1].Exist.Should().BeEqualTo(1);
        await nodes[1].Left.Should().BeNull();
        await nodes[1].Right.Should().BeNull();
        await nodes[2].Should().BeNull();
        await nodes[3].Should().BeNull();
        await nodes[4].Should().BeNull();
        await nodes[5].Should().BeNull();
        await nodes[6].Should().BeNull();
        await nodes[7].Should().BeNull();

        bt.Add(2, 2, idx: 0);
        nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
        await nodes[0].Should().BeNull();
        await nodes[1].Accepts.ToArray().Should().BeEmpty();
        await nodes[1].Exist.Should().BeEqualTo(1);
        await nodes[1].Left.Should().BeNull();
        await nodes[1].Right.Should().BeNull();
        await nodes[2].Accepts.ToArray().Should().BeStrictlyEquivalentTo([0]);
        await nodes[2].Exist.Should().BeEqualTo(2);
        await nodes[2].Left.Should().BeNull();
        await nodes[2].Right.Should().BeNull();
        await nodes[3].Should().BeNull();
        await nodes[4].Should().BeNull();
        await nodes[5].Should().BeNull();
        await nodes[6].Should().BeNull();
        await nodes[7].Should().BeNull();

        bt.Add(0, 3, idx: 1);
        nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
        await nodes[0].Accepts.ToArray().Should().BeStrictlyEquivalentTo([1]);
        await nodes[0].Exist.Should().BeEqualTo(3);
        await nodes[0].Left.Should().BeNull();
        await nodes[0].Right.Should().BeNull();
        await nodes[1].Accepts.ToArray().Should().BeEmpty();
        await nodes[1].Exist.Should().BeEqualTo(1);
        await nodes[1].Left.Should().BeNull();
        await nodes[1].Right.Should().BeNull();
        await nodes[2].Accepts.ToArray().Should().BeStrictlyEquivalentTo([0]);
        await nodes[2].Exist.Should().BeEqualTo(2);
        await nodes[2].Left.Should().BeNull();
        await nodes[2].Right.Should().BeNull();
        await nodes[3].Should().BeNull();
        await nodes[4].Should().BeNull();
        await nodes[5].Should().BeNull();
        await nodes[6].Should().BeNull();
        await nodes[7].Should().BeNull();

        bt.Add(2, -2, idx: 2);
        nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
        await nodes[0].Accepts.ToArray().Should().BeStrictlyEquivalentTo([1]);
        await nodes[0].Exist.Should().BeEqualTo(3);
        await nodes[0].Left.Should().BeNull();
        await nodes[0].Right.Should().BeNull();
        await nodes[1].Accepts.ToArray().Should().BeEmpty();
        await nodes[1].Exist.Should().BeEqualTo(1);
        await nodes[1].Left.Should().BeNull();
        await nodes[1].Right.Should().BeNull();
        await nodes[2].Accepts.ToArray().Should().BeStrictlyEquivalentTo([0, 2]);
        await nodes[2].Exist.Should().BeEqualTo(0);
        await nodes[2].Left.Should().BeNull();
        await nodes[2].Right.Should().BeNull();
        await nodes[3].Should().BeNull();
        await nodes[4].Should().BeNull();
        await nodes[5].Should().BeNull();
        await nodes[6].Should().BeNull();
        await nodes[7].Should().BeNull();

        bt.Decrement(1, clear: true);
        nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
        await nodes[0].Accepts.ToArray().Should().BeStrictlyEquivalentTo([1]);
        await nodes[0].Exist.Should().BeEqualTo(3);
        await nodes[0].Left.Should().BeNull();
        await nodes[0].Right.Should().BeNull();
        await nodes[1].Should().BeNull();
        await nodes[2].Accepts.ToArray().Should().BeStrictlyEquivalentTo([0, 2]);
        await nodes[2].Exist.Should().BeEqualTo(0);
        await nodes[2].Left.Should().BeNull();
        await nodes[2].Right.Should().BeNull();
        await nodes[3].Should().BeNull();
        await nodes[4].Should().BeNull();
        await nodes[5].Should().BeNull();
        await nodes[6].Should().BeNull();
        await nodes[7].Should().BeNull();
    }
}