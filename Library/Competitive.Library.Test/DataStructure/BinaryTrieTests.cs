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

            bt.KthElement(0).Num.ShouldBe(1u);
            bt.KthElement(1).Num.ShouldBe(1u);
            bt.KthElement(2).Num.ShouldBe(2u);
            bt.KthElement(3).Num.ShouldBe(5u);

            bt.MinElement().Num.ShouldBe(1u);
            bt.MaxElement().Num.ShouldBe(5u);

            bt.Count(0).ShouldBe(0);
            bt.Count(1).ShouldBe(2);
            bt.Count(2).ShouldBe(1);
            bt.Count(3).ShouldBe(0);
            bt.Count(4).ShouldBe(0);
            bt.Count(5).ShouldBe(1);
            bt.Count(6).ShouldBe(0);
            bt.Count(ulong.MaxValue).ShouldBe(0);

            bt.CountLess(1).ShouldBe(0);
            bt.CountLess(2).ShouldBe(2);
            bt.CountLess(3).ShouldBe(3);
            bt.CountLess(5).ShouldBe(3);
            bt.CountLess(6).ShouldBe(4);

            bt.KthElement(0, 4).Num.ShouldBe(1u);
            bt.KthElement(1, 4).Num.ShouldBe(5u);
            bt.KthElement(2, 4).Num.ShouldBe(5u);
            bt.KthElement(3, 4).Num.ShouldBe(6u);

            bt.MinElement(4).Num.ShouldBe(1u);
            bt.MaxElement(4).Num.ShouldBe(6u);

            bt.CountLess(1, 4).ShouldBe(0);
            bt.CountLess(2, 4).ShouldBe(1);
            bt.CountLess(3, 4).ShouldBe(1);
            bt.CountLess(5, 4).ShouldBe(1);
            bt.CountLess(6, 4).ShouldBe(3);
            bt.CountLess(7, 4).ShouldBe(4);

            bt.Decrement(2);
            bt.KthElement(0).Num.ShouldBe(1u);
            bt.KthElement(1).Num.ShouldBe(1u);
            bt.KthElement(2).Num.ShouldBe(5u);

            bt.MinElement().Num.ShouldBe(1u);
            bt.MaxElement().Num.ShouldBe(5u);

            bt.Count(1).ShouldBe(2);
            bt.Count(5).ShouldBe(1);

            bt.CountLess(1).ShouldBe(0);
            bt.CountLess(2).ShouldBe(2);
            bt.CountLess(5).ShouldBe(2);
            bt.CountLess(6).ShouldBe(3);
        }

        [Fact]
        public void Byte()
        {
            var bt = new BinaryTrie<byte>(3);
            var nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].ShouldBeNull();
            nodes[1].ShouldBeNull();
            nodes[2].ShouldBeNull();
            nodes[3].ShouldBeNull();
            nodes[4].ShouldBeNull();
            nodes[5].ShouldBeNull();
            nodes[6].ShouldBeNull();
            nodes[7].ShouldBeNull();

            bt.Increment(1);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].ShouldBeNull();
            nodes[1].Accepts.ToArray().ShouldBeEmpty();
            nodes[1].Exist.ShouldBe(1);
            nodes[1].Left.ShouldBeNull();
            nodes[1].Right.ShouldBeNull();
            nodes[2].ShouldBeNull();
            nodes[3].ShouldBeNull();
            nodes[4].ShouldBeNull();
            nodes[5].ShouldBeNull();
            nodes[6].ShouldBeNull();
            nodes[7].ShouldBeNull();

            bt.Add(2, 2, idx: 0);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].ShouldBeNull();
            nodes[1].Accepts.ToArray().ShouldBeEmpty();
            nodes[1].Exist.ShouldBe(1);
            nodes[1].Left.ShouldBeNull();
            nodes[1].Right.ShouldBeNull();
            nodes[2].Accepts.ToArray().ShouldBe([0]);
            nodes[2].Exist.ShouldBe(2);
            nodes[2].Left.ShouldBeNull();
            nodes[2].Right.ShouldBeNull();
            nodes[3].ShouldBeNull();
            nodes[4].ShouldBeNull();
            nodes[5].ShouldBeNull();
            nodes[6].ShouldBeNull();
            nodes[7].ShouldBeNull();

            bt.Add(0, 3, idx: 1);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Accepts.ToArray().ShouldBe([1]);
            nodes[0].Exist.ShouldBe(3);
            nodes[0].Left.ShouldBeNull();
            nodes[0].Right.ShouldBeNull();
            nodes[1].Accepts.ToArray().ShouldBeEmpty();
            nodes[1].Exist.ShouldBe(1);
            nodes[1].Left.ShouldBeNull();
            nodes[1].Right.ShouldBeNull();
            nodes[2].Accepts.ToArray().ShouldBe([0]);
            nodes[2].Exist.ShouldBe(2);
            nodes[2].Left.ShouldBeNull();
            nodes[2].Right.ShouldBeNull();
            nodes[3].ShouldBeNull();
            nodes[4].ShouldBeNull();
            nodes[5].ShouldBeNull();
            nodes[6].ShouldBeNull();
            nodes[7].ShouldBeNull();

            bt.Add(2, -2, idx: 2);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Accepts.ToArray().ShouldBe([1]);
            nodes[0].Exist.ShouldBe(3);
            nodes[0].Left.ShouldBeNull();
            nodes[0].Right.ShouldBeNull();
            nodes[1].Accepts.ToArray().ShouldBeEmpty();
            nodes[1].Exist.ShouldBe(1);
            nodes[1].Left.ShouldBeNull();
            nodes[1].Right.ShouldBeNull();
            nodes[2].Accepts.ToArray().ShouldBe([0, 2]);
            nodes[2].Exist.ShouldBe(0);
            nodes[2].Left.ShouldBeNull();
            nodes[2].Right.ShouldBeNull();
            nodes[3].ShouldBeNull();
            nodes[4].ShouldBeNull();
            nodes[5].ShouldBeNull();
            nodes[6].ShouldBeNull();
            nodes[7].ShouldBeNull();

            bt.Decrement(1, clear: true);
            nodes = Enumerable.Range(0, 8).Select(i => bt.Find((byte)i)).ToArray();
            nodes[0].Accepts.ToArray().ShouldBe([1]);
            nodes[0].Exist.ShouldBe(3);
            nodes[0].Left.ShouldBeNull();
            nodes[0].Right.ShouldBeNull();
            nodes[1].ShouldBeNull();
            nodes[2].Accepts.ToArray().ShouldBe([0, 2]);
            nodes[2].Exist.ShouldBe(0);
            nodes[2].Left.ShouldBeNull();
            nodes[2].Right.ShouldBeNull();
            nodes[3].ShouldBeNull();
            nodes[4].ShouldBeNull();
            nodes[5].ShouldBeNull();
            nodes[6].ShouldBeNull();
            nodes[7].ShouldBeNull();
        }
    }
}