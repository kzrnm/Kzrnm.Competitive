using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
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
    }
}
