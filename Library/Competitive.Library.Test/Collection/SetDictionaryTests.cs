using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class SetDictionaryTests
    {
        [Fact]
        public void SetDictionary()
        {
            var set = new SetDictionary<int, int>(new Dictionary<int, int>
            {
                {5,0},
                {6,1},
                {7,2},
                {8,3},
                {9,4},
                {1,5},
                {2,6},
                {3,7},
                {4,8},
            });

            set.Add(5, 9);
            set.Add(1, 10);
            set.Add(2, 11);
            set.Add(3, 12);
            set.ToArray().ShouldBe(new Dictionary<int, int>
            {
                {1,5},
                {2,6},
                {3,7},
                {4,8},
                {5,0},
                {6,1},
                {7,2},
                {8,3},
                {9,4},
            });
            set.Remove(5);
            set.ToArray().ShouldBe(new Dictionary<int, int>
            {
                {1,5},
                {2,6},
                {3,7},
                {4,8},
                {6,1},
                {7,2},
                {8,3},
                {9,4},
            });
            set.FindNodeLowerBound(4).Pair.ShouldBe(KeyValuePair.Create(4, 8));
            set.FindNodeUpperBound(4).Pair.ShouldBe(KeyValuePair.Create(6, 1));
            set.FindNodeLowerBound(5).Pair.ShouldBe(KeyValuePair.Create(6, 1));
            set.FindNodeUpperBound(5).Pair.ShouldBe(KeyValuePair.Create(6, 1));

            set.FindNodeLowerBound(10).ShouldBeNull();
            set.FindNodeUpperBound(10).ShouldBeNull();

            int k, v;
            KeyValuePair<int, int> pair;
            set.TryGetLowerBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(3); v.ShouldBe(7);
            set.TryGetUpperBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(4); v.ShouldBe(8);
            set.TryGetReverseLowerBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(3); v.ShouldBe(7);
            set.TryGetReverseUpperBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(2); v.ShouldBe(6);
            set.TryGetLowerBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(3, 7));
            set.TryGetUpperBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(4, 8));
            set.TryGetReverseLowerBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(3, 7));
            set.TryGetReverseUpperBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(2, 6));

            set.TryGetLowerBound(9, out _).ShouldBeTrue();
            set.TryGetLowerBound(10, out _).ShouldBeFalse();
            set.TryGetUpperBound(8, out _).ShouldBeTrue();
            set.TryGetUpperBound(9, out _).ShouldBeFalse();

            set.RemoveNode(set.FindNodeLowerBound(5));
            set.ToArray().ShouldBe(new Dictionary<int, int>
            {
                {1,5},
                {2,6},
                {3,7},
                {4,8},
                {7,2},
                {8,3},
                {9,4},
            });

            set.RemoveNode(set.FindNodeLowerBound(0));
            set.ToArray().ShouldBe(new Dictionary<int, int>
            {
                {2,6},
                {3,7},
                {4,8},
                {7,2},
                {8,3},
                {9,4},
            });

            set.RemoveNode(set.FindNodeLowerBound(9));
            set.ToArray().ShouldBe(new Dictionary<int, int>
            {
                {2,6},
                {3,7},
                {4,8},
                {7,2},
                {8,3},
            });
        }
        [Fact]
        public void MultiSetDictionary()
        {
            var set = new SetDictionary<int, int>(new Dictionary<int, int>
            {
                {5,0},
                {6,1},
                {7,2},
                {8,3},
                {9,4},
                {1,5},
                {2,6},
                {3,7},
                {4,8},
            }, true);

            set.Add(5, 9);
            set.Add(1, 10);
            set.Add(2, 11);
            set.Add(3, 12);
            set.ToArray().ShouldBe(new KeyValuePair<int, int>[]
            {
                KeyValuePair.Create(1,5),
                KeyValuePair.Create(1,10),
                KeyValuePair.Create(2,6),
                KeyValuePair.Create(2,11),
                KeyValuePair.Create(3,7),
                KeyValuePair.Create(3,12),
                KeyValuePair.Create(4,8),
                KeyValuePair.Create(5,0),
                KeyValuePair.Create(5,9),
                KeyValuePair.Create(6,1),
                KeyValuePair.Create(7,2),
                KeyValuePair.Create(8,3),
                KeyValuePair.Create(9,4),
            });
            set.Remove(5);
            set.ToArray().ShouldBe(new KeyValuePair<int, int>[]
            {
                KeyValuePair.Create(1,5),
                KeyValuePair.Create(1,10),
                KeyValuePair.Create(2,6),
                KeyValuePair.Create(2,11),
                KeyValuePair.Create(3,7),
                KeyValuePair.Create(3,12),
                KeyValuePair.Create(4,8),
                KeyValuePair.Create(5,9),
                KeyValuePair.Create(6,1),
                KeyValuePair.Create(7,2),
                KeyValuePair.Create(8,3),
                KeyValuePair.Create(9,4),
            });
            set.FindNodeLowerBound(4).Pair.ShouldBe(KeyValuePair.Create(4, 8));
            set.FindNodeUpperBound(4).Pair.ShouldBe(KeyValuePair.Create(5, 9));
            set.FindNodeLowerBound(5).Pair.ShouldBe(KeyValuePair.Create(5, 9));
            set.FindNodeUpperBound(5).Pair.ShouldBe(KeyValuePair.Create(6, 1));

            set.FindNodeLowerBound(10).ShouldBeNull();
            set.FindNodeUpperBound(10).ShouldBeNull();

            int k, v;
            KeyValuePair<int, int> pair;
            set.TryGetLowerBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(3); v.ShouldBe(7);
            set.TryGetUpperBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(4); v.ShouldBe(8);
            set.TryGetReverseLowerBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(3); v.ShouldBe(12);
            set.TryGetReverseUpperBound(3, out k, out v).ShouldBeTrue(); k.ShouldBe(2); v.ShouldBe(11);
            set.TryGetLowerBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(3, 7));
            set.TryGetUpperBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(4, 8));
            set.TryGetReverseLowerBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(3, 12));
            set.TryGetReverseUpperBound(3, out pair).ShouldBeTrue(); pair.ShouldBe(KeyValuePair.Create(2, 11));

            set.TryGetLowerBound(9, out _).ShouldBeTrue();
            set.TryGetLowerBound(10, out _).ShouldBeFalse();
            set.TryGetUpperBound(8, out _).ShouldBeTrue();
            set.TryGetUpperBound(9, out _).ShouldBeFalse();
        }
        [Fact]
        public void ReverseComparer()
        {
            var set = new SetDictionary<int, int, ReverseComparerStruct<int>>(new Dictionary<int, int>
            {
                {5,0},
                {6,1},
                {7,2},
                {8,3},
                {9,4},
                {1,5},
                {2,6},
                {3,7},
                {4,8},
            });

            set.Add(5, 9);
            set.Add(1, 10);
            set.Add(2, 11);
            set.Add(3, 12);
            set.ToArray().ShouldBe(new Dictionary<int, int>
            {
                {9,4},
                {8,3},
                {7,2},
                {6,1},
                {5,0},
                {4,8},
                {3,7},
                {2,6},
                {1,5},
            });
            set.Remove(5);
            set.ToArray().ShouldBe(new Dictionary<int, int>
            {
                {9,4},
                {8,3},
                {7,2},
                {6,1},
                {4,8},
                {3,7},
                {2,6},
                {1,5},
            });
            set.FindNodeLowerBound(6).Pair.ShouldBe(KeyValuePair.Create(6, 1));
            set.FindNodeUpperBound(6).Pair.ShouldBe(KeyValuePair.Create(4, 8));
            set.FindNodeLowerBound(5).Pair.ShouldBe(KeyValuePair.Create(4, 8));
            set.FindNodeUpperBound(5).Pair.ShouldBe(KeyValuePair.Create(4, 8));

            set.FindNodeLowerBound(0).ShouldBeNull();
            set.FindNodeUpperBound(0).ShouldBeNull();
        }
    }
}
