using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class DequeExtensionTests
    {
        public static IEnumerable<ValueTuple<int>> Lengths => Enumerable.Range(0, 18).Select(ValueTuple.Create<int>);
        [Theory]
        [TupleMemberData(nameof(Lengths))]
        public void ToDeque(int length)
        {
            var orig = Enumerable.Range(1, length).ToArray();
            Impl(orig.ToDeque());
            Impl(orig.AsSpan().ToDeque());
            Impl(orig.AsEnumerable().ToDeque());

            void Impl(Deque<int> deque)
            {
                deque.Count.Should().Be(length);
                for (int i = 0; i < orig.Length; i++)
                {
                    deque[i].Should().Be(orig[i]);
                }
                deque.Should().BeEquivalentTo(orig);
            }
        }
        [Fact]
        public void ToDequeEmpty()
        {
            var orig = new int[0];
            Impl(orig.ToDeque());
            Impl(orig.AsSpan().ToDeque());
            Impl(orig.AsEnumerable().ToDeque());

            static void Impl(Deque<int> deque)
            {
                deque.Count.Should().Be(0);
                deque.Should().BeEmpty();
                deque.Should().Equal(new int[0]);
            }
        }
    }
}