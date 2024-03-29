using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class SetTests
    {
        [Fact]
        public void InitSingleSet()
        {
            for (int i = 0; i < 64; i++)
                new Set<int>(Enumerable.Range(0, i).Reverse().Concat(Enumerable.Range(0, i)))
                    .Should().Equal(Enumerable.Range(0, i));
        }

        [Fact]
        public void InitMultiSet()
        {
            for (int i = 0; i < 64; i++)
                new Set<int>(Enumerable.Range(0, i).Reverse().Concat(Enumerable.Range(0, i)), true)
                    .Should().Equal(Enumerable.Range(0, i).SelectMany(n => new[] { n, n }));
        }

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
            set.FindByIndex(7).Value.Should().Be(9);
            set.FindNode(5).Should().BeNull();

            set.FindNodeLowerBound(4).Value.Should().Be(4);
            set.FindNodeUpperBound(4).Value.Should().Be(6);
            set.FindNodeReverseUpperBound(4).Value.Should().Be(3);
            set.FindNodeReverseLowerBound(4).Value.Should().Be(4);
            set.FindNodeLowerBound(5).Value.Should().Be(6);
            set.FindNodeUpperBound(5).Value.Should().Be(6);
            set.FindNodeReverseUpperBound(5).Value.Should().Be(4);
            set.FindNodeReverseUpperBound(5).Value.Should().Be(4);

            int v;
            set.TryGetLowerBound(4, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetUpperBound(4, out v).Should().BeTrue(); v.Should().Be(6);
            set.TryGetReverseLowerBound(4, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetReverseUpperBound(4, out v).Should().BeTrue(); v.Should().Be(3);
            set.TryGetLowerBound(5, out v).Should().BeTrue(); v.Should().Be(6);
            set.TryGetUpperBound(5, out v).Should().BeTrue(); v.Should().Be(6);
            set.TryGetReverseLowerBound(5, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetReverseUpperBound(5, out v).Should().BeTrue(); v.Should().Be(4);

            set.LowerBoundIndex(4).Should().Be(3);
            set.UpperBoundIndex(4).Should().Be(4);
            set.TryGetReverseLowerBound(4, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetReverseUpperBound(4, out v).Should().BeTrue(); v.Should().Be(3);
            set.LowerBoundIndex(5).Should().Be(4);
            set.UpperBoundIndex(5).Should().Be(4);
            set.TryGetReverseLowerBound(5, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetReverseUpperBound(5, out v).Should().BeTrue(); v.Should().Be(4);

            set.TryGetLowerBound(9, out _).Should().BeTrue();
            set.TryGetLowerBound(10, out _).Should().BeFalse();
            set.TryGetUpperBound(8, out _).Should().BeTrue();
            set.TryGetUpperBound(9, out _).Should().BeFalse();

            set.TryGetReverseLowerBound(1, out _).Should().BeTrue();
            set.TryGetReverseLowerBound(0, out _).Should().BeFalse();
            set.TryGetReverseUpperBound(2, out _).Should().BeTrue();
            set.TryGetReverseUpperBound(1, out _).Should().BeFalse();

            set.FindNodeLowerBound(10).Should().BeNull();
            set.FindNodeUpperBound(10).Should().BeNull();
            set.FindNodeReverseLowerBound(0).Should().BeNull();
            set.FindNodeReverseUpperBound(1).Should().BeNull();

            set.RemoveNode(set.FindNodeLowerBound(5));
            set.Should().Equal(1, 2, 3, 4, 7, 8, 9);

            set.Reversed().Should().Equal(9, 8, 7, 4, 3, 2, 1);
            set.EnumerateItem().Should().Equal(1, 2, 3, 4, 7, 8, 9);
            set.EnumerateItem(set.FindNodeLowerBound(5)).Should().Equal(7, 8, 9);
            set.EnumerateItem(set.FindNodeLowerBound(5), true).Should().Equal(7, 4, 3, 2, 1);

            set.RemoveNode(set.FindNodeLowerBound(0));
            set.Should().Equal(2, 3, 4, 7, 8, 9);

            set.RemoveNode(set.FindNodeLowerBound(9));
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
            set.FindByIndex(11).Value.Should().Be(9);
            set.FindNode(5).Should().NotBeNull();

            set.Reversed().Should().Equal(9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1);
            set.EnumerateItem().Should().Equal(1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9);
            set.EnumerateItem(set.FindNodeLowerBound(6)).Should().Equal(6, 7, 8, 9);
            set.EnumerateItem(set.FindNodeLowerBound(6), true).Should().Equal(6, 5, 4, 3, 3, 2, 2, 1, 1);

            set.FindNodeLowerBound(3).Value.Should().Be(3);
            set.FindNodeUpperBound(3).Value.Should().Be(4);
            set.FindNodeReverseLowerBound(3).Value.Should().Be(3);
            set.FindNodeReverseUpperBound(3).Value.Should().Be(2);

            int v;
            set.TryGetLowerBound(3, out v).Should().BeTrue(); v.Should().Be(3);
            set.TryGetUpperBound(3, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetReverseLowerBound(3, out v).Should().BeTrue(); v.Should().Be(3);
            set.TryGetReverseUpperBound(3, out v).Should().BeTrue(); v.Should().Be(2);

            set.TryGetLowerBound(9, out _).Should().BeTrue();
            set.TryGetLowerBound(10, out _).Should().BeFalse();
            set.TryGetUpperBound(8, out _).Should().BeTrue();
            set.TryGetUpperBound(9, out _).Should().BeFalse();

            set.TryGetReverseLowerBound(1, out _).Should().BeTrue();
            set.TryGetReverseLowerBound(0, out _).Should().BeFalse();
            set.TryGetReverseUpperBound(2, out _).Should().BeTrue();
            set.TryGetReverseUpperBound(1, out _).Should().BeFalse();

            set.LowerBoundIndex(3).Should().Be(4);
            set.UpperBoundIndex(3).Should().Be(6);
            set.ReverseLowerBoundIndex(3).Should().Be(5);
            set.ReverseUpperBoundIndex(3).Should().Be(3);

            set.FindNodeLowerBound(10).Should().BeNull();
            set.FindNodeUpperBound(10).Should().BeNull();
            set.FindNodeReverseLowerBound(0).Should().BeNull();
            set.FindNodeReverseUpperBound(1).Should().BeNull();
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
            set.FindByIndex(7).Value.Should().Be(1);
            set.FindNode(5).Should().BeNull();

            set.FindNodeLowerBound(6).Value.Should().Be(6);
            set.FindNodeUpperBound(6).Value.Should().Be(4);
            set.FindNodeLowerBound(5).Value.Should().Be(4);
            set.FindNodeUpperBound(5).Value.Should().Be(4);

            int v;
            set.TryGetLowerBound(6, out v).Should().BeTrue(); v.Should().Be(6);
            set.TryGetUpperBound(6, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetLowerBound(5, out v).Should().BeTrue(); v.Should().Be(4);
            set.TryGetUpperBound(5, out v).Should().BeTrue(); v.Should().Be(4);

            set.TryGetLowerBound(1, out _).Should().BeTrue();
            set.TryGetLowerBound(0, out _).Should().BeFalse();
            set.TryGetUpperBound(2, out _).Should().BeTrue();
            set.TryGetUpperBound(1, out _).Should().BeFalse();

            set.TryGetReverseLowerBound(9, out _).Should().BeTrue();
            set.TryGetReverseLowerBound(10, out _).Should().BeFalse();
            set.TryGetReverseUpperBound(8, out _).Should().BeTrue();
            set.TryGetReverseUpperBound(9, out _).Should().BeFalse();

            set.LowerBoundIndex(6).Should().Be(3);
            set.UpperBoundIndex(6).Should().Be(4);
            set.LowerBoundIndex(5).Should().Be(4);
            set.UpperBoundIndex(5).Should().Be(4);

            set.FindNodeLowerBound(0).Should().BeNull();
            set.FindNodeUpperBound(0).Should().BeNull();
        }

        [Fact]
        public void FindByIndex()
        {
            for (int count = 0; count < 64; count++)
            {
                IList<int> arr = Enumerable.Range(0, count).ToArray();
                var set = new Set<int>(arr);
                for (int i = 0; i < count; i++)
                {
                    set.FindByIndex(i).Value.Should().Be(i, "Index: {0}", i);
                }
            }
        }

        [Fact]
        public void Enumerate()
        {
            for (int count = 0; count < 64; count++)
            {
                IList<int> arr = Enumerable.Range(0, count).ToArray();
                var set = new Set<int>(arr);
                set.Reversed().Should().Equal(arr.Reverse());
                set.EnumerateItem().Should().Equal(arr);
                set.EnumerateItem(reverse: true).Should().Equal(arr.Reverse());

                for (int i = 0; i < count; i++)
                {
                    set.EnumerateItem(set.FindByIndex(i)).Should().Equal(arr.Skip(i), "Index: {0}", i);
                    set.EnumerateItem(set.FindByIndex(i), true).Should()
                        .Equal(arr.Take(i + 1).Reverse(), "Index: {0} Reverse", i);
                }
            }
        }

        [Fact]
        public void EnumerateMulti()
        {
            var arr = new[] { 1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9 };
            var set = new Set<int>(arr, true);
            set.Reversed().Should().Equal(9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1);
            set.EnumerateItem().Should().Equal(1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9);
            set.EnumerateItem(reverse: true).Should().Equal(9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1);

            for (int i = 0; i < arr.Length; i++)
            {
                set.EnumerateItem(set.FindByIndex(i)).Should().Equal(arr.Skip(i), "Index: {0}", i);
                set.EnumerateItem(set.FindByIndex(i), true).Should()
                    .Equal(arr.Take(i + 1).Reverse(), "Index: {0} Reverse", i);
            }
        }
    }
}
