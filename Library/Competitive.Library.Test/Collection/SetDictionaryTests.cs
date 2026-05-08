namespace Kzrnm.Competitive.Testing.Collection;

public class SetDictionaryTests
{
    [Test, MultipleAssertions]
    public async Task SetDictionary()
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
        await set.ToArray().Should().BeStrictlyEquivalentTo(new Dictionary<int, int>
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
        await set.ToArray().Should().BeStrictlyEquivalentTo(new Dictionary<int, int>
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
        await set.FindNodeLowerBound(4).Pair.Should().BeEqualTo(KeyValuePair.Create(4, 8));
        await set.FindNodeUpperBound(4).Pair.Should().BeEqualTo(KeyValuePair.Create(6, 1));
        await set.FindNodeLowerBound(5).Pair.Should().BeEqualTo(KeyValuePair.Create(6, 1));
        await set.FindNodeUpperBound(5).Pair.Should().BeEqualTo(KeyValuePair.Create(6, 1));

        await set.FindNodeLowerBound(10).Should().BeNull();
        await set.FindNodeUpperBound(10).Should().BeNull();

        int k, v;
        KeyValuePair<int, int> pair;
        await set.TryGetLowerBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(3);
        await v.Should().BeEqualTo(7);
        await set.TryGetUpperBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(4);
        await v.Should().BeEqualTo(8);
        await set.TryGetReverseLowerBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(3);
        await v.Should().BeEqualTo(7);
        await set.TryGetReverseUpperBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(2);
        await v.Should().BeEqualTo(6);
        await set.TryGetLowerBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(3, 7));
        await set.TryGetUpperBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(4, 8));
        await set.TryGetReverseLowerBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(3, 7));
        await set.TryGetReverseUpperBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(2, 6));

        await set.TryGetLowerBound(9, out _).Should().BeTrue();
        await set.TryGetLowerBound(10, out _).Should().BeFalse();
        await set.TryGetUpperBound(8, out _).Should().BeTrue();
        await set.TryGetUpperBound(9, out _).Should().BeFalse();

        set.RemoveNode(set.FindNodeLowerBound(5));
        await set.ToArray().Should().BeStrictlyEquivalentTo(new Dictionary<int, int>
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
        await set.ToArray().Should().BeStrictlyEquivalentTo(new Dictionary<int, int>
        {
            {2,6},
            {3,7},
            {4,8},
            {7,2},
            {8,3},
            {9,4},
        });

        set.RemoveNode(set.FindNodeLowerBound(9));
        await set.ToArray().Should().BeStrictlyEquivalentTo(new Dictionary<int, int>
        {
            {2,6},
            {3,7},
            {4,8},
            {7,2},
            {8,3},
        });
    }
    [Test, MultipleAssertions]
    public async Task MultiSetDictionary()
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
        await set.ToArray().Should().BeStrictlyEquivalentTo([
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
        ]);
        set.Remove(5);
        await set.ToArray().Should().BeStrictlyEquivalentTo([
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
        ]);
        await set.FindNodeLowerBound(4).Pair.Should().BeEqualTo(KeyValuePair.Create(4, 8));
        await set.FindNodeUpperBound(4).Pair.Should().BeEqualTo(KeyValuePair.Create(5, 9));
        await set.FindNodeLowerBound(5).Pair.Should().BeEqualTo(KeyValuePair.Create(5, 9));
        await set.FindNodeUpperBound(5).Pair.Should().BeEqualTo(KeyValuePair.Create(6, 1));

        await set.FindNodeLowerBound(10).Should().BeNull();
        await set.FindNodeUpperBound(10).Should().BeNull();

        int k, v;
        KeyValuePair<int, int> pair;
        await set.TryGetLowerBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(3);
        await v.Should().BeEqualTo(7);
        await set.TryGetUpperBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(4);
        await v.Should().BeEqualTo(8);
        await set.TryGetReverseLowerBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(3);
        await v.Should().BeEqualTo(12);
        await set.TryGetReverseUpperBound(3, out k, out v).Should().BeTrue();
        await k.Should().BeEqualTo(2);
        await v.Should().BeEqualTo(11);
        await set.TryGetLowerBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(3, 7));
        await set.TryGetUpperBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(4, 8));
        await set.TryGetReverseLowerBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(3, 12));
        await set.TryGetReverseUpperBound(3, out pair).Should().BeTrue();
        await pair.Should().BeEqualTo(KeyValuePair.Create(2, 11));

        await set.TryGetLowerBound(9, out _).Should().BeTrue();
        await set.TryGetLowerBound(10, out _).Should().BeFalse();
        await set.TryGetUpperBound(8, out _).Should().BeTrue();
        await set.TryGetUpperBound(9, out _).Should().BeFalse();
    }
    [Test, MultipleAssertions]
    public async Task ReverseComparer()
    {
        var set = new SetDictionary<int, int, ReverseComparer<int>>(new Dictionary<int, int>
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
        await set.ToArray().Should().BeStrictlyEquivalentTo(new Dictionary<int, int>
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
        await set.ToArray().Should().BeStrictlyEquivalentTo(new Dictionary<int, int>
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
        await set.FindNodeLowerBound(6).Pair.Should().BeEqualTo(KeyValuePair.Create(6, 1));
        await set.FindNodeUpperBound(6).Pair.Should().BeEqualTo(KeyValuePair.Create(4, 8));
        await set.FindNodeLowerBound(5).Pair.Should().BeEqualTo(KeyValuePair.Create(4, 8));
        await set.FindNodeUpperBound(5).Pair.Should().BeEqualTo(KeyValuePair.Create(4, 8));

        await set.FindNodeLowerBound(0).Should().BeNull();
        await set.FindNodeUpperBound(0).Should().BeNull();
    }
}