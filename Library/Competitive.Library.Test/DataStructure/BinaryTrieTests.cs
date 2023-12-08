using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class BinaryTrieTests
    {
        [Fact]
        public void Simple()
        {
            var bt = new BinaryTrie();
            bt.Add(1, 2);
            bt.Increment(2);
            bt.Increment(5);

            bt.KthElement(0).Num.Should().Be(1);
            bt.KthElement(1).Num.Should().Be(1);
            bt.KthElement(2).Num.Should().Be(2);
            bt.KthElement(3).Num.Should().Be(5);

            bt.MinElement().Num.Should().Be(1);
            bt.MaxElement().Num.Should().Be(5);

            bt.Count(1).Should().Be(2);
            bt.Count(2).Should().Be(1);
            bt.Count(5).Should().Be(1);

            bt.CountLess(1).Should().Be(0);
            bt.CountLess(2).Should().Be(2);
            bt.CountLess(3).Should().Be(3);
            bt.CountLess(5).Should().Be(3);
            bt.CountLess(6).Should().Be(4);

            bt.KthElement(0, 4).Num.Should().Be(1);
            bt.KthElement(1, 4).Num.Should().Be(5);
            bt.KthElement(2, 4).Num.Should().Be(5);
            bt.KthElement(3, 4).Num.Should().Be(6);

            bt.MinElement(4).Num.Should().Be(1);
            bt.MaxElement(4).Num.Should().Be(6);

            bt.Count(1, 4).Should().Be(1);
            bt.Count(2, 4).Should().Be(0);
            bt.Count(5, 4).Should().Be(2);
            bt.Count(6, 4).Should().Be(1);

            bt.CountLess(1, 4).Should().Be(0);
            bt.CountLess(2, 4).Should().Be(1);
            bt.CountLess(3, 4).Should().Be(1);
            bt.CountLess(5, 4).Should().Be(1);
            bt.CountLess(6, 4).Should().Be(3);
            bt.CountLess(7, 4).Should().Be(4);

            bt.Decrement(2);
            bt.KthElement(0).Num.Should().Be(1);
            bt.KthElement(1).Num.Should().Be(1);
            bt.KthElement(2).Num.Should().Be(5);

            bt.MinElement().Num.Should().Be(1);
            bt.MaxElement().Num.Should().Be(5);

            bt.Count(1).Should().Be(2);
            bt.Count(5).Should().Be(1);

            bt.CountLess(1).Should().Be(0);
            bt.CountLess(2).Should().Be(2);
            bt.CountLess(5).Should().Be(2);
            bt.CountLess(6).Should().Be(3);
        }

        [Fact]
        public void Byte()
        {
            var bt = new BinaryTrie<byte>(3);
            var nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Should().BeNull();
            nodes[1].Should().BeNull();
            nodes[2].Should().BeNull();
            nodes[3].Should().BeNull();
            nodes[4].Should().BeNull();
            nodes[5].Should().BeNull();
            nodes[6].Should().BeNull();
            nodes[7].Should().BeNull();

            bt.Increment(1);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Should().BeNull();
            nodes[1].Accepts.ToArray().Should().BeEmpty();
            nodes[1].Exist.Should().Be(1);
            nodes[1].Left.Should().BeNull();
            nodes[1].Right.Should().BeNull();
            nodes[2].Should().BeNull();
            nodes[3].Should().BeNull();
            nodes[4].Should().BeNull();
            nodes[5].Should().BeNull();
            nodes[6].Should().BeNull();
            nodes[7].Should().BeNull();

            bt.Add(2, 2, idx: 0);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Should().BeNull();
            nodes[1].Accepts.ToArray().Should().BeEmpty();
            nodes[1].Exist.Should().Be(1);
            nodes[1].Left.Should().BeNull();
            nodes[1].Right.Should().BeNull();
            nodes[2].Accepts.ToArray().Should().Equal(0);
            nodes[2].Exist.Should().Be(2);
            nodes[2].Left.Should().BeNull();
            nodes[2].Right.Should().BeNull();
            nodes[3].Should().BeNull();
            nodes[4].Should().BeNull();
            nodes[5].Should().BeNull();
            nodes[6].Should().BeNull();
            nodes[7].Should().BeNull();

            bt.Add(0, 3, idx: 1);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Accepts.ToArray().Should().Equal(1);
            nodes[0].Exist.Should().Be(3);
            nodes[0].Left.Should().BeNull();
            nodes[0].Right.Should().BeNull();
            nodes[1].Accepts.ToArray().Should().BeEmpty();
            nodes[1].Exist.Should().Be(1);
            nodes[1].Left.Should().BeNull();
            nodes[1].Right.Should().BeNull();
            nodes[2].Accepts.ToArray().Should().Equal(0);
            nodes[2].Exist.Should().Be(2);
            nodes[2].Left.Should().BeNull();
            nodes[2].Right.Should().BeNull();
            nodes[3].Should().BeNull();
            nodes[4].Should().BeNull();
            nodes[5].Should().BeNull();
            nodes[6].Should().BeNull();
            nodes[7].Should().BeNull();

            bt.Add(2, -2, idx: 2);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Accepts.ToArray().Should().Equal(1);
            nodes[0].Exist.Should().Be(3);
            nodes[0].Left.Should().BeNull();
            nodes[0].Right.Should().BeNull();
            nodes[1].Accepts.ToArray().Should().BeEmpty();
            nodes[1].Exist.Should().Be(1);
            nodes[1].Left.Should().BeNull();
            nodes[1].Right.Should().BeNull();
            nodes[2].Accepts.ToArray().Should().Equal(0, 2);
            nodes[2].Exist.Should().Be(0);
            nodes[2].Left.Should().BeNull();
            nodes[2].Right.Should().BeNull();
            nodes[3].Should().BeNull();
            nodes[4].Should().BeNull();
            nodes[5].Should().BeNull();
            nodes[6].Should().BeNull();
            nodes[7].Should().BeNull();

            bt.Decrement(0, clear: true, xorVal: 1);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Accepts.ToArray().Should().Equal(1);
            nodes[0].Exist.Should().Be(3);
            nodes[0].Left.Should().BeNull();
            nodes[0].Right.Should().BeNull();
            nodes[1].Should().BeNull();
            nodes[2].Accepts.ToArray().Should().Equal(0, 2);
            nodes[2].Exist.Should().Be(0);
            nodes[2].Left.Should().BeNull();
            nodes[2].Right.Should().BeNull();
            nodes[3].Should().BeNull();
            nodes[4].Should().BeNull();
            nodes[5].Should().BeNull();
            nodes[6].Should().BeNull();
            nodes[7].Should().BeNull();
        }
    }
}