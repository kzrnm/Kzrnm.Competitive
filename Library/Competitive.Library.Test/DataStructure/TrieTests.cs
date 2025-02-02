using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class TrieTests
{
    static SortedDictionary<TKey, Trie<TKey, TValue>> TrieDic<TKey, TValue>(Trie<TKey, TValue> trie)
        => (SortedDictionary<TKey, Trie<TKey, TValue>>)typeof(Trie<TKey, TValue>)
            .GetField("children", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(trie);


    [Fact]
    public void GetChildTest()
    {
        var trie = new Trie<int, int>();
        trie.GetChild([1, 2, 3]).ShouldBeNull();
    }

    [Fact]
    public void AddTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2, 3], -1);
        trie.GetChild([1, 2, 3]).Value.ShouldBe(-1);
        trie.HasValue.ShouldBeFalse();

        SortedDictionary<int, Trie<int, int>> dic;

        dic = TrieDic(trie);
        dic.ShouldContainKey(1);
        dic.Count.ShouldBe(1);

        trie = dic[1];
        trie.HasValue.ShouldBeFalse();
        dic = TrieDic(trie);
        dic.ShouldContainKey(2);
        dic.Count.ShouldBe(1);

        trie = dic[2];
        trie.HasValue.ShouldBeFalse();
        dic = TrieDic(trie);
        dic.ShouldContainKey(3);
        dic.Count.ShouldBe(1);

        trie = dic[3];
        trie.HasValue.ShouldBeTrue();
        trie.Value.ShouldBe(-1);
        dic = TrieDic(trie);
        dic.ShouldBeEmpty();
    }

    [Fact]
    public void GetTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2, 3], -1);

        trie.TryGet([1, 2], out _).ShouldBeFalse();
        trie.TryGet([1, 2, 2], out _).ShouldBeFalse();
        trie.TryGet([1, 2, 3, 4], out _).ShouldBeFalse();

        Should.Throw<KeyNotFoundException>(() => trie[[1, 2]]);
        Should.Throw<KeyNotFoundException>(() => trie[[1, 2, 2]]);
        Should.Throw<KeyNotFoundException>(() => trie[[1, 2, 3, 4]]);

        trie.TryGet([1, 2, 3], out var val).ShouldBeTrue();
        val.ShouldBe(-1);
        trie[[1, 2, 3]].ShouldBe(-1);
    }


    [Fact]
    public void IndexTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2, 3], 0);
        trie.Add([1, 3, 2], 1);
        trie.Add([2, 1, 3], 2);
        trie.Add([2, 3, 1], 3);
        trie.Add([3, 1, 3], 4);
        trie.Add([3, 2, 1], 5);

        for (int i = 0; i < 6; i++)
            trie[i].ShouldBe(i);

        Should.Throw<IndexOutOfRangeException>(() => trie[-1]);
        Should.Throw<IndexOutOfRangeException>(() => trie[6]);
    }

    [Fact]
    public void RemoveTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1, 2], 10);
        trie.Add([1, 2, 3], -1);
        trie.GetChild([1, 2]).Value.ShouldBe(10);
        trie.GetChild([1, 2, 3]).Value.ShouldBe(-1);
        trie.HasValue.ShouldBeFalse();

        Trie<int, int> tt;
        SortedDictionary<int, Trie<int, int>> dic;

        tt = trie;
        dic = TrieDic(tt);
        dic.ShouldContainKey(1);
        dic.Count.ShouldBe(1);

        tt = dic[1];
        tt.HasValue.ShouldBeFalse();
        dic = TrieDic(tt);
        dic.ShouldContainKey(2);
        dic.Count.ShouldBe(1);

        tt = dic[2];
        tt.HasValue.ShouldBeTrue();
        tt.Value.ShouldBe(10);
        dic = TrieDic(tt);
        dic.ShouldContainKey(3);
        dic.Count.ShouldBe(1);

        tt = dic[3];
        tt.HasValue.ShouldBeTrue();
        tt.Value.ShouldBe(-1);
        dic = TrieDic(tt);
        dic.ShouldBeEmpty();

        // remove last
        trie.Remove([1, 2, 3]);

        tt = trie;
        dic = TrieDic(tt);
        dic.ShouldContainKey(1);
        dic.Count.ShouldBe(1);

        tt = dic[1];
        tt.HasValue.ShouldBeFalse();
        dic = TrieDic(tt);
        dic.ShouldContainKey(2);
        dic.Count.ShouldBe(1);

        tt = dic[2];
        tt.HasValue.ShouldBeTrue();
        tt.Value.ShouldBe(10);
        dic = TrieDic(tt);
        dic.ShouldBeEmpty();


        // remove mid
        trie.Add([1, 2, 3], -2);
        trie.Remove([1, 2]);

        tt = trie;
        dic = TrieDic(tt);
        dic.ShouldContainKey(1);
        dic.Count.ShouldBe(1);

        tt = dic[1];
        tt.HasValue.ShouldBeFalse();
        dic = TrieDic(tt);
        dic.ShouldContainKey(2);
        dic.Count.ShouldBe(1);

        tt = dic[2];
        tt.HasValue.ShouldBeFalse();
        dic = TrieDic(tt);
        dic.ShouldContainKey(3);
        dic.Count.ShouldBe(1);

        tt = dic[3];
        tt.HasValue.ShouldBeTrue();
        tt.Value.ShouldBe(-2);
        dic = TrieDic(tt);
        dic.ShouldBeEmpty();


        // remove last one
        trie.Remove([1, 2, 3]);

        tt = trie;
        dic = TrieDic(tt);
        dic.ShouldBeEmpty();
    }


    [Fact]
    public void CountTest()
    {
        var trie = new Trie<int, int>();
        trie.Count.ShouldBe(0);
        trie.Add([1, 2, 3], -1);
        trie.Add([1, 2], -1);
        trie.Add([2, 2, 4], -1);
        trie.Count.ShouldBe(3);
        trie.GetChild([1, 2, 3]).Count.ShouldBe(1);
        trie.GetChild([1, 2]).Count.ShouldBe(2);

        trie.Remove([2, 2, 4]);
        trie.Count.ShouldBe(2);
        trie.GetChild([1, 2, 3]).Count.ShouldBe(1);
        trie.GetChild([1, 2]).Count.ShouldBe(2);

        trie.Remove([1, 2]);
        trie.Count.ShouldBe(1);
        trie.GetChild([1, 2, 3]).Count.ShouldBe(1);
        trie.GetChild([1, 2]).Count.ShouldBe(1);
    }


    [Fact]
    public void AllEnumerateTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1], -2);
        trie.Add([1, 2], 10);
        trie.Add([1, 2, 3], -1);
        trie.Add([1, -3], 35);
        trie.Add([1, -2], 8);
        trie.Add([1, -2, 5], 6);

        var all = trie.All().GetEnumerator();
        var expected = new (int[] key, int val)[]
        {
            (new int[] { 1 }, -2),
            (new int[] { 1, -3 }, 35),
            (new int[] { 1, -2 }, 8),
            (new int[] { 1, -2, 5 }, 6),
            (new int[] { 1, 2 }, 10),
            (new int[] { 1, 2, 3 }, -1),
    };
        foreach (var (exKey, exVal) in expected)
        {
            all.MoveNext().ShouldBeTrue();
            var (key, val) = all.Current;
            key.ShouldBe(exKey);
            val.ShouldBe(exVal);
        }
        all.MoveNext().ShouldBeFalse();
    }

    [Fact]
    public void MatchGreedyTest()
    {
        var trie = new Trie<int, int>();
        trie.Add([1], -2);
        trie.Add([1, 2], 10);
        trie.Add([1, 2, 3], -1);
        trie.Add([1, -3], 35);
        trie.Add([1, -2], 8);
        trie.Add([1, -2, 5], 6);

        var greedy = trie.MatchGreedy([1, -2, 5, 20]).GetEnumerator();
        var expected = new (int[] key, int val)[]
        {
            (new int[] { 1 }, -2),
            (new int[] { 1, -2 }, 8),
            (new int[] { 1, -2, 5 }, 6),
        };
        foreach (var (exKey, exVal) in expected)
        {
            greedy.MoveNext().ShouldBeTrue();
            var (key, val) = greedy.Current;
            key.ToArray().ShouldBe(exKey);
            val.ShouldBe(exVal);
        }
        greedy.MoveNext().ShouldBeFalse();
    }
}
