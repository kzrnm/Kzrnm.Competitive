using System.Reflection;
using TUnit.Core.Converters;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class TrieTests
{
    static SortedDictionary<TKey, Trie<TKey, TValue>> TrieDic<TKey, TValue>(Trie<TKey, TValue> trie)
        => (SortedDictionary<TKey, Trie<TKey, TValue>>)typeof(Trie<TKey, TValue>)
            .GetField("children", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(trie);


    [Test]
    public async Task GetChildTest()
    {
        var trie = new Trie<int, int>();
        await trie.GetChild([1, 2, 3]).Should().BeNull();
    }

    [Test, MultipleAssertions]
    public async Task AddTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2, 3], -1);
        await trie.GetChild([1, 2, 3]).Value.Should().BeEqualTo(-1);
        await trie.HasValue.Should().BeFalse();

        SortedDictionary<int, Trie<int, int>> dic;

        dic = TrieDic(trie);
        await dic.Should().ContainKey(1);
        await dic.Count.Should().BeEqualTo(1);

        trie = dic[1];
        await trie.HasValue.Should().BeFalse();
        dic = TrieDic(trie);
        await dic.Should().ContainKey(2);
        await dic.Count.Should().BeEqualTo(1);

        trie = dic[2];
        await trie.HasValue.Should().BeFalse();
        dic = TrieDic(trie);
        await dic.Should().ContainKey(3);
        await dic.Count.Should().BeEqualTo(1);

        trie = dic[3];
        await trie.HasValue.Should().BeTrue();
        await trie.Value.Should().BeEqualTo(-1);
        dic = TrieDic(trie);
        await dic.Should().BeEmpty();
    }

    [Test, MultipleAssertions]
    public async Task GetTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2, 3], -1);

        await trie.TryGet([1, 2], out _).Should().BeFalse();
        await trie.TryGet([1, 2, 2], out _).Should().BeFalse();
        await trie.TryGet([1, 2, 3, 4], out _).Should().BeFalse();

        await new Action(() => _ = trie[[1, 2]]).Should().ThrowExactly<KeyNotFoundException>();
        await new Action(() => _ = trie[[1, 2, 2]]).Should().ThrowExactly<KeyNotFoundException>();
        await new Action(() => _ = trie[[1, 2, 3, 4]]).Should().ThrowExactly<KeyNotFoundException>();

        await trie.TryGet([1, 2, 3], out var val).Should().BeTrue();
        await val.Should().BeEqualTo(-1);
        await trie[[1, 2, 3]].Should().BeEqualTo(-1);
    }

    [Test, MultipleAssertions]
    public async Task IndexTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2, 3], 0);
        trie.Add([1, 3, 2], 1);
        trie.Add([2, 1, 3], 2);
        trie.Add([2, 3, 1], 3);
        trie.Add([3, 1, 3], 4);
        trie.Add([3, 2, 1], 5);

        for (int i = 0; i < 6; i++)
            await trie[i].Should().BeEqualTo(i);

        await new Action(() => _ = trie[-1]).Should().ThrowExactly<IndexOutOfRangeException>();
        await new Action(() => _ = trie[6]).Should().ThrowExactly<IndexOutOfRangeException>();
    }

    [Test, MultipleAssertions]
    public async Task RemoveTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2], 10);
        trie.Add([1, 2, 3], -1);
        await trie.GetChild([1, 2]).Value.Should().BeEqualTo(10);
        await trie.GetChild([1, 2, 3]).Value.Should().BeEqualTo(-1);
        await trie.HasValue.Should().BeFalse();

        Trie<int, int> tt;
        SortedDictionary<int, Trie<int, int>> dic;

        tt = trie;
        dic = TrieDic(tt);
        await dic.Should().ContainKey(1);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[1];
        await tt.HasValue.Should().BeFalse();
        dic = TrieDic(tt);
        await dic.Should().ContainKey(2);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[2];
        await tt.HasValue.Should().BeTrue();
        await tt.Value.Should().BeEqualTo(10);
        dic = TrieDic(tt);
        await dic.Should().ContainKey(3);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[3];
        await tt.HasValue.Should().BeTrue();
        await tt.Value.Should().BeEqualTo(-1);
        dic = TrieDic(tt);
        await dic.Should().BeEmpty();

        // remove last
        trie.Remove([1, 2, 3]);

        tt = trie;
        dic = TrieDic(tt);
        await dic.Should().ContainKey(1);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[1];
        await tt.HasValue.Should().BeFalse();
        dic = TrieDic(tt);
        await dic.Should().ContainKey(2);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[2];
        await tt.HasValue.Should().BeTrue();
        await tt.Value.Should().BeEqualTo(10);
        dic = TrieDic(tt);
        await dic.Should().BeEmpty();


        // remove mid
        trie.Add([1, 2, 3], -2);
        trie.Remove([1, 2]);

        tt = trie;
        dic = TrieDic(tt);
        await dic.Should().ContainKey(1);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[1];
        await tt.HasValue.Should().BeFalse();
        dic = TrieDic(tt);
        await dic.Should().ContainKey(2);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[2];
        await tt.HasValue.Should().BeFalse();
        dic = TrieDic(tt);
        await dic.Should().ContainKey(3);
        await dic.Count.Should().BeEqualTo(1);

        tt = dic[3];
        await tt.HasValue.Should().BeTrue();
        await tt.Value.Should().BeEqualTo(-2);
        dic = TrieDic(tt);
        await dic.Should().BeEmpty();


        // remove last one
        trie.Remove([1, 2, 3]);

        tt = trie;
        dic = TrieDic(tt);
        await dic.Should().BeEmpty();
    }


    [Test, MultipleAssertions]
    public async Task CountTest()
    {
        var trie = new Trie<int, int>();
        await trie.Count.Should().BeEqualTo(0);
        trie.Add([1, 2, 3], -1);
        trie.Add([1, 2], -1);
        trie.Add([2, 2, 4], -1);
        await trie.Count.Should().BeEqualTo(3);
        await trie.GetChild([1, 2, 3]).Count.Should().BeEqualTo(1);
        await trie.GetChild([1, 2]).Count.Should().BeEqualTo(2);

        trie.Remove([2, 2, 4]);
        await trie.Count.Should().BeEqualTo(2);
        await trie.GetChild([1, 2, 3]).Count.Should().BeEqualTo(1);
        await trie.GetChild([1, 2]).Count.Should().BeEqualTo(2);

        trie.Remove([1, 2]);
        await trie.Count.Should().BeEqualTo(1);
        await trie.GetChild([1, 2, 3]).Count.Should().BeEqualTo(1);
        await trie.GetChild([1, 2]).Count.Should().BeEqualTo(1);
    }


    [Test, MultipleAssertions]
    public async Task AllEnumerateTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1], -2);
        trie.Add([1, 2], 10);
        trie.Add([1, 2, 3], -1);
        trie.Add([1, -3], 35);
        trie.Add([1, -2], 8);
        trie.Add([1, -2, 5], 6);

        var all = trie.All().GetEnumerator();
        (int[] key, int val)[] expected = [
            ([1], -2),
            ([1, -3], 35),
            ([1, -2], 8),
            ([1, -2, 5], 6),
            ([1, 2], 10),
            ([1, 2, 3], -1),
        ];
        foreach (var (exKey, exVal) in expected)
        {
            await all.MoveNext().Should().BeTrue();
            var (key, val) = all.Current;
            await key.Should().BeStrictlyEquivalentTo(exKey);
            await val.Should().BeEqualTo(exVal);
        }
        await all.MoveNext().Should().BeFalse();
    }

    [Test, MultipleAssertions]
    public async Task MatchGreedyTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1], -2);
        trie.Add([1, 2], 10);
        trie.Add([1, 2, 3], -1);
        trie.Add([1, -3], 35);
        trie.Add([1, -2], 8);
        trie.Add([1, -2, 5], 6);

        var greedy = trie.MatchGreedy([1, -2, 5, 20]).GetEnumerator();
        var list = new List<(int[] key, int val)>(3);
        foreach (var (k, v) in greedy)
        {
            list.Add((k.ToArray(), v));
        }

        (int[] key, int val)[] expected = [
            ([1], -2),
            ([1, -2], 8),
            ([1, -2, 5], 6),
        ];
        await list.Should()
            .BeStrictlyEquivalentTo(expected,
            comparer: EqualityComparer<(int[] key, int val)>.Create((t1, t2) => t1.key.SequenceEqual(t2.key) && t1.val == t2.val));
    }
}