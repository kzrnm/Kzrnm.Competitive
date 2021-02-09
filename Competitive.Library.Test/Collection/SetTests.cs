using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Collection
{
    public class SetTests
    {
        [Fact]
        public void Set()
        {
            var set = new Set<int>(new[]
            {
                6,7,8,1,2,3,4,5,1,2,3,
            });
            set.Add(9);
            set.Add(5);
            set.Should().Equal(1, 2, 3, 4, 5, 6, 7, 8, 9);
            set.Should().HaveCount(9);
            set.Remove(5);
            set.Should().HaveCount(8);
            set.Should().Equal(1, 2, 3, 4, 6, 7, 8, 9);
            set.FindByIndex(8).Should().BeNull();
            set.FindByIndex(7).Item.Should().Be(9);
            set.FindNode(5).Should().BeNull();

            set.FindNodeLowerBound(4).Item.Should().Be(4);
            set.FindNodeUpperBound(4).Item.Should().Be(6);
            set.FindNodeLowerBound(5).Item.Should().Be(6);
            set.FindNodeUpperBound(5).Item.Should().Be(6);

            set.LowerBoundItem(4).Should().Be(4);
            set.UpperBoundItem(4).Should().Be(6);
            set.LowerBoundItem(5).Should().Be(6);
            set.UpperBoundItem(5).Should().Be(6);

            set.LowerBoundIndex(4).Should().Be(3);
            set.UpperBoundIndex(4).Should().Be(4);
            set.LowerBoundIndex(5).Should().Be(4);
            set.UpperBoundIndex(5).Should().Be(4);

            set.FindNodeLowerBound(10).Should().BeNull();
            set.FindNodeUpperBound(10).Should().BeNull();

            set.Remove(set.FindNodeLowerBound(5));
            set.Should().Equal(1, 2, 3, 4, 7, 8, 9);

            set.Remove(set.FindNodeLowerBound(0));
            set.Should().Equal(2, 3, 4, 7, 8, 9);

            set.Remove(set.FindNodeLowerBound(9));
            set.Should().Equal(2, 3, 4, 7, 8);
        }
        [Fact]
        public void MultiSet()
        {
            var set = new Set<int>(new[]
            {
                6,7,8,1,2,3,4,5,1,2,3,
            }, true);
            set.Add(9);
            set.Add(5);
            set.Should().Equal(1, 1, 2, 2, 3, 3, 4, 5, 5, 6, 7, 8, 9);
            set.Should().HaveCount(13);
            set.Remove(5);
            set.Should().HaveCount(12);
            set.Should().Equal(1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9);
            set.FindByIndex(12).Should().BeNull();
            set.FindByIndex(11).Item.Should().Be(9);
            set.FindNode(5).Should().NotBeNull();

            set.FindNodeLowerBound(3).Item.Should().Be(3);
            set.FindNodeUpperBound(3).Item.Should().Be(4);

            set.LowerBoundItem(3).Should().Be(3);
            set.UpperBoundItem(3).Should().Be(4);

            set.LowerBoundIndex(3).Should().Be(4);
            set.UpperBoundIndex(3).Should().Be(6);

            set.FindNodeLowerBound(10).Should().BeNull();
            set.FindNodeUpperBound(10).Should().BeNull();
        }
        [Fact]
        public void ReverseComparer()
        {
            var set = new Set<int, ReverseComparerStruct<int>>(new[]
            {
                6,7,8,1,2,3,4,5,1,2,3,
            });
            set.Add(9);
            set.Add(5);
            set.Should().Equal(9, 8, 7, 6, 5, 4, 3, 2, 1);
            set.Should().HaveCount(9);
            set.Remove(5);
            set.Should().HaveCount(8);
            set.Should().Equal(9, 8, 7, 6, 4, 3, 2, 1);
            set.FindByIndex(8).Should().BeNull();
            set.FindByIndex(7).Item.Should().Be(1);
            set.FindNode(5).Should().BeNull();

            set.FindNodeLowerBound(6).Item.Should().Be(6);
            set.FindNodeUpperBound(6).Item.Should().Be(4);
            set.FindNodeLowerBound(5).Item.Should().Be(4);
            set.FindNodeUpperBound(5).Item.Should().Be(4);

            set.LowerBoundItem(6).Should().Be(6);
            set.UpperBoundItem(6).Should().Be(4);
            set.LowerBoundItem(5).Should().Be(4);
            set.UpperBoundItem(5).Should().Be(4);

            set.LowerBoundIndex(6).Should().Be(3);
            set.UpperBoundIndex(6).Should().Be(4);
            set.LowerBoundIndex(5).Should().Be(4);
            set.UpperBoundIndex(5).Should().Be(4);

            set.FindNodeLowerBound(0).Should().BeNull();
            set.FindNodeUpperBound(0).Should().BeNull();
        }
    }
}
