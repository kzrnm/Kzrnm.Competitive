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
                    .ShouldBe(Enumerable.Range(0, i));
        }

        [Fact]
        public void InitMultiSet()
        {
            for (int i = 0; i < 64; i++)
                new Set<int>(Enumerable.Range(0, i).Reverse().Concat(Enumerable.Range(0, i)), true)
                    .ShouldBe(Enumerable.Range(0, i).SelectMany(n => new[] { n, n }));
        }

        [Fact]
        public void Set()
        {
            var set = new Set<int>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3]);
            set.Add(9);
            set.Add(5);
            set.ShouldBe((int[])[1, 2, 3, 4, 5, 6, 7, 8, 9]);
            set.Count.ShouldBe(9);
            set.Remove(5);
            set.Count.ShouldBe(8);
            set.ShouldBe((int[])[1, 2, 3, 4, 6, 7, 8, 9]);
            set.FindByIndex(8).ShouldBeNull();
            set.FindByIndex(7).Value.ShouldBe(9);
            set.FindNode(5).ShouldBeNull();

            set.FindNodeLowerBound(4).Value.ShouldBe(4);
            set.FindNodeUpperBound(4).Value.ShouldBe(6);
            set.FindNodeReverseUpperBound(4).Value.ShouldBe(3);
            set.FindNodeReverseLowerBound(4).Value.ShouldBe(4);
            set.FindNodeLowerBound(5).Value.ShouldBe(6);
            set.FindNodeUpperBound(5).Value.ShouldBe(6);
            set.FindNodeReverseUpperBound(5).Value.ShouldBe(4);
            set.FindNodeReverseUpperBound(5).Value.ShouldBe(4);

            int v;
            set.TryGetLowerBound(4, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetUpperBound(4, out v).ShouldBeTrue(); v.ShouldBe(6);
            set.TryGetReverseLowerBound(4, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetReverseUpperBound(4, out v).ShouldBeTrue(); v.ShouldBe(3);
            set.TryGetLowerBound(5, out v).ShouldBeTrue(); v.ShouldBe(6);
            set.TryGetUpperBound(5, out v).ShouldBeTrue(); v.ShouldBe(6);
            set.TryGetReverseLowerBound(5, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetReverseUpperBound(5, out v).ShouldBeTrue(); v.ShouldBe(4);

            set.LowerBoundIndex(4).ShouldBe(3);
            set.UpperBoundIndex(4).ShouldBe(4);
            set.TryGetReverseLowerBound(4, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetReverseUpperBound(4, out v).ShouldBeTrue(); v.ShouldBe(3);
            set.LowerBoundIndex(5).ShouldBe(4);
            set.UpperBoundIndex(5).ShouldBe(4);
            set.TryGetReverseLowerBound(5, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetReverseUpperBound(5, out v).ShouldBeTrue(); v.ShouldBe(4);

            set.TryGetLowerBound(9, out _).ShouldBeTrue();
            set.TryGetLowerBound(10, out _).ShouldBeFalse();
            set.TryGetUpperBound(8, out _).ShouldBeTrue();
            set.TryGetUpperBound(9, out _).ShouldBeFalse();

            set.TryGetReverseLowerBound(1, out _).ShouldBeTrue();
            set.TryGetReverseLowerBound(0, out _).ShouldBeFalse();
            set.TryGetReverseUpperBound(2, out _).ShouldBeTrue();
            set.TryGetReverseUpperBound(1, out _).ShouldBeFalse();

            set.FindNodeLowerBound(10).ShouldBeNull();
            set.FindNodeUpperBound(10).ShouldBeNull();
            set.FindNodeReverseLowerBound(0).ShouldBeNull();
            set.FindNodeReverseUpperBound(1).ShouldBeNull();

            set.RemoveNode(set.FindNodeLowerBound(5));
            set.ShouldBe((int[])[1, 2, 3, 4, 7, 8, 9]);

            set.Reversed().ShouldBe([9, 8, 7, 4, 3, 2, 1]);
            set.EnumerateItem().ShouldBe([1, 2, 3, 4, 7, 8, 9]);
            set.EnumerateItem(set.FindNodeLowerBound(5)).ShouldBe([7, 8, 9]);
            set.EnumerateItem(set.FindNodeLowerBound(5), true).ShouldBe([7, 4, 3, 2, 1]);

            set.RemoveNode(set.FindNodeLowerBound(0));
            set.ShouldBe((int[])[2, 3, 4, 7, 8, 9]);

            set.RemoveNode(set.FindNodeLowerBound(9));
            set.ShouldBe((int[])[2, 3, 4, 7, 8]);
        }
        [Fact]
        public void MultiSet()
        {
            var set = new Set<int>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3], true);
            set.Add(9);
            set.Add(5);
            set.ShouldBe((int[])[1, 1, 2, 2, 3, 3, 4, 5, 5, 6, 7, 8, 9]);
            set.Count.ShouldBe(13);
            set.Remove(5);
            set.Count.ShouldBe(12);
            set.ShouldBe((int[])[1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
            set.FindByIndex(12).ShouldBeNull();
            set.FindByIndex(11).Value.ShouldBe(9);
            set.FindNode(5).ShouldNotBeNull();

            set.Reversed().ShouldBe([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);
            set.EnumerateItem().ShouldBe([1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
            set.EnumerateItem(set.FindNodeLowerBound(6)).ShouldBe([6, 7, 8, 9]);
            set.EnumerateItem(set.FindNodeLowerBound(6), true).ShouldBe([6, 5, 4, 3, 3, 2, 2, 1, 1]);

            set.FindNodeLowerBound(3).Value.ShouldBe(3);
            set.FindNodeUpperBound(3).Value.ShouldBe(4);
            set.FindNodeReverseLowerBound(3).Value.ShouldBe(3);
            set.FindNodeReverseUpperBound(3).Value.ShouldBe(2);

            int v;
            set.TryGetLowerBound(3, out v).ShouldBeTrue(); v.ShouldBe(3);
            set.TryGetUpperBound(3, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetReverseLowerBound(3, out v).ShouldBeTrue(); v.ShouldBe(3);
            set.TryGetReverseUpperBound(3, out v).ShouldBeTrue(); v.ShouldBe(2);

            set.TryGetLowerBound(9, out _).ShouldBeTrue();
            set.TryGetLowerBound(10, out _).ShouldBeFalse();
            set.TryGetUpperBound(8, out _).ShouldBeTrue();
            set.TryGetUpperBound(9, out _).ShouldBeFalse();

            set.TryGetReverseLowerBound(1, out _).ShouldBeTrue();
            set.TryGetReverseLowerBound(0, out _).ShouldBeFalse();
            set.TryGetReverseUpperBound(2, out _).ShouldBeTrue();
            set.TryGetReverseUpperBound(1, out _).ShouldBeFalse();

            set.LowerBoundIndex(3).ShouldBe(4);
            set.UpperBoundIndex(3).ShouldBe(6);
            set.ReverseLowerBoundIndex(3).ShouldBe(5);
            set.ReverseUpperBoundIndex(3).ShouldBe(3);

            set.FindNodeLowerBound(10).ShouldBeNull();
            set.FindNodeUpperBound(10).ShouldBeNull();
            set.FindNodeReverseLowerBound(0).ShouldBeNull();
            set.FindNodeReverseUpperBound(1).ShouldBeNull();
        }
        [Fact]
        public void ReverseComparer()
        {
            var set = new Set<int, ReverseComparerStruct<int>>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3]);
            set.Add(9);
            set.Add(5);
            set.ShouldBe((int[])[9, 8, 7, 6, 5, 4, 3, 2, 1]);
            set.Count.ShouldBe(9);
            set.Remove(5);
            set.Count.ShouldBe(8);
            set.ShouldBe((int[])[9, 8, 7, 6, 4, 3, 2, 1]);
            set.FindByIndex(8).ShouldBeNull();
            set.FindByIndex(7).Value.ShouldBe(1);
            set.FindNode(5).ShouldBeNull();

            set.FindNodeLowerBound(6).Value.ShouldBe(6);
            set.FindNodeUpperBound(6).Value.ShouldBe(4);
            set.FindNodeLowerBound(5).Value.ShouldBe(4);
            set.FindNodeUpperBound(5).Value.ShouldBe(4);

            int v;
            set.TryGetLowerBound(6, out v).ShouldBeTrue(); v.ShouldBe(6);
            set.TryGetUpperBound(6, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetLowerBound(5, out v).ShouldBeTrue(); v.ShouldBe(4);
            set.TryGetUpperBound(5, out v).ShouldBeTrue(); v.ShouldBe(4);

            set.TryGetLowerBound(1, out _).ShouldBeTrue();
            set.TryGetLowerBound(0, out _).ShouldBeFalse();
            set.TryGetUpperBound(2, out _).ShouldBeTrue();
            set.TryGetUpperBound(1, out _).ShouldBeFalse();

            set.TryGetReverseLowerBound(9, out _).ShouldBeTrue();
            set.TryGetReverseLowerBound(10, out _).ShouldBeFalse();
            set.TryGetReverseUpperBound(8, out _).ShouldBeTrue();
            set.TryGetReverseUpperBound(9, out _).ShouldBeFalse();

            set.LowerBoundIndex(6).ShouldBe(3);
            set.UpperBoundIndex(6).ShouldBe(4);
            set.LowerBoundIndex(5).ShouldBe(4);
            set.UpperBoundIndex(5).ShouldBe(4);

            set.FindNodeLowerBound(0).ShouldBeNull();
            set.FindNodeUpperBound(0).ShouldBeNull();
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
                    set.FindByIndex(i).Value.ShouldBe(i, $"Index: {i}");
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
                set.Reversed().ShouldBe(arr.Reverse());
                set.EnumerateItem().ShouldBe(arr);
                set.EnumerateItem(reverse: true).ShouldBe(arr.Reverse());

                for (int i = 0; i < count; i++)
                {
                    set.EnumerateItem(set.FindByIndex(i)).ShouldBe(arr.Skip(i), $"Index: {i}");
                    set.EnumerateItem(set.FindByIndex(i), true)
                        .ShouldBe(arr.Take(i + 1).Reverse(), $"Index: {i} Reverse");
                }
            }
        }

        [Fact]
        public void EnumerateMulti()
        {
            var arr = new[] { 1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9 };
            var set = new Set<int>(arr, true);
            set.Reversed().ShouldBe([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);
            set.EnumerateItem().ShouldBe([1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
            set.EnumerateItem(reverse: true).ShouldBe([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);

            for (int i = 0; i < arr.Length; i++)
            {
                set.EnumerateItem(set.FindByIndex(i)).ShouldBe(arr.Skip(i), $"Index: {i}");
                set.EnumerateItem(set.FindByIndex(i), true)
                    .ShouldBe(arr.Take(i + 1).Reverse(), $"Index: {i} Reverse");
            }
        }
    }
}
