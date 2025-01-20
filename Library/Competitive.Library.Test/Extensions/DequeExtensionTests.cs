using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class DequeExtensionTests
    {
        public static TheoryData<int> Lengths => new(Enumerable.Range(0, 18));
        [Theory]
        [MemberData(nameof(Lengths))]
        public void ToDeque(int length)
        {
            var orig = Enumerable.Range(1, length).ToArray();
            Impl(orig.ToDeque());
            Impl(orig.AsSpan().ToDeque());
            Impl(orig.AsEnumerable().ToDeque());

            void Impl(Deque<int> deque)
            {
                deque.Count.ShouldBe(length);
                for (int i = 0; i < orig.Length; i++)
                {
                    deque[i].ShouldBe(orig[i]);
                }
                deque.ShouldBe(orig);
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
                deque.Count.ShouldBe(0);
                deque.ShouldBeEmpty();
                deque.ShouldBe(new int[0]);
            }
        }

        [Fact]
        public void RemoveFirst()
        {
            for (int i = 0; i <= 7; i++)
            {
                var deque = new Deque<int> { 1, 2, 3, 4, 5, 6, 7, };
                deque.RemoveFirst(i);
                deque.ShouldBe(Enumerable.Range(i + 1, 7 - i));
            }
        }

        [Fact]
        public void RemoveLast()
        {
            for (int i = 0; i <= 7; i++)
            {
                var deque = new Deque<int> { 1, 2, 3, 4, 5, 6, 7, };
                deque.RemoveLast(i);
                deque.ShouldBe(Enumerable.Range(1, 7 - i));
            }
        }
    }
}