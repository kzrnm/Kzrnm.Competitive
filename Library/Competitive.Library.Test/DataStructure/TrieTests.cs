using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kzrnm.Competitive.Testing.DataStructure
{
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
            trie.GetChild(stackalloc int[] { 1, 2, 3 }).Should().BeNull();
        }

        [Fact]
        public void AddTest()
        {
            var trie = new Trie<int, int>();
            trie.Add(stackalloc int[] { 1, 2, 3 }, -1);
            trie.GetChild(stackalloc int[] { 1, 2, 3 }).Value.Should().Be(-1);
            trie.HasValue.Should().BeFalse();

            SortedDictionary<int, Trie<int, int>> dic;

            dic = TrieDic(trie);
            dic.Should().ContainKey(1).And.HaveCount(1);

            trie = dic[1];
            trie.HasValue.Should().BeFalse();
            dic = TrieDic(trie);
            dic.Should().ContainKey(2).And.HaveCount(1);

            trie = dic[2];
            trie.HasValue.Should().BeFalse();
            dic = TrieDic(trie);
            dic.Should().ContainKey(3).And.HaveCount(1);

            trie = dic[3];
            trie.HasValue.Should().BeTrue();
            trie.Value.Should().Be(-1);
            dic = TrieDic(trie);
            dic.Should().BeEmpty();
        }

        [Fact]
        public void GetTest()
        {
            var trie = new Trie<int, int>();
            trie.Add(stackalloc int[] { 1, 2, 3 }, -1);

            trie.TryGet(stackalloc int[] { 1, 2 }, out _).Should().BeFalse();
            trie.TryGet(stackalloc int[] { 1, 2, 2 }, out _).Should().BeFalse();
            trie.TryGet(stackalloc int[] { 1, 2, 3, 4 }, out _).Should().BeFalse();

            trie.Invoking(trie => trie[stackalloc int[] { 1, 2 }]).Should().Throw<KeyNotFoundException>();
            trie.Invoking(trie => trie[stackalloc int[] { 1, 2, 2 }]).Should().Throw<KeyNotFoundException>();
            trie.Invoking(trie => trie[stackalloc int[] { 1, 2, 3, 4 }]).Should().Throw<KeyNotFoundException>();

            trie.TryGet(stackalloc int[] { 1, 2, 3 }, out var val).Should().BeTrue();
            val.Should().Be(-1);
            trie[stackalloc int[] { 1, 2, 3 }].Should().Be(-1);
        }


        [Fact]
        public void IndexTest()
        {
            var trie = new Trie<int, int>();
            trie.Add(stackalloc int[] { 1, 2, 3 }, 0);
            trie.Add(stackalloc int[] { 1, 3, 2 }, 1);
            trie.Add(stackalloc int[] { 2, 1, 3 }, 2);
            trie.Add(stackalloc int[] { 2, 3, 1 }, 3);
            trie.Add(stackalloc int[] { 3, 1, 3 }, 4);
            trie.Add(stackalloc int[] { 3, 2, 1 }, 5);

            for (int i = 0; i < 6; i++)
                trie[i].Should().Be(i);

            trie.Invoking(trie => trie[-1]).Should().Throw<IndexOutOfRangeException>();
            trie.Invoking(trie => trie[6]).Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void RemoveTest()
        {
            var trie = new Trie<int, int>();
            trie.Add(stackalloc int[] { 1, 2 }, 10);
            trie.Add(stackalloc int[] { 1, 2, 3 }, -1);
            trie.GetChild(stackalloc int[] { 1, 2 }).Value.Should().Be(10);
            trie.GetChild(stackalloc int[] { 1, 2, 3 }).Value.Should().Be(-1);
            trie.HasValue.Should().BeFalse();

            Trie<int, int> tt;
            SortedDictionary<int, Trie<int, int>> dic;

            tt = trie;
            dic = TrieDic(tt);
            dic.Should().ContainKey(1).And.HaveCount(1);

            tt = dic[1];
            tt.HasValue.Should().BeFalse();
            dic = TrieDic(tt);
            dic.Should().ContainKey(2).And.HaveCount(1);

            tt = dic[2];
            tt.HasValue.Should().BeTrue();
            tt.Value.Should().Be(10);
            dic = TrieDic(tt);
            dic.Should().ContainKey(3).And.HaveCount(1);

            tt = dic[3];
            tt.HasValue.Should().BeTrue();
            tt.Value.Should().Be(-1);
            dic = TrieDic(tt);
            dic.Should().BeEmpty();

            // remove last
            trie.Remove(stackalloc int[] { 1, 2, 3 });

            tt = trie;
            dic = TrieDic(tt);
            dic.Should().ContainKey(1).And.HaveCount(1);

            tt = dic[1];
            tt.HasValue.Should().BeFalse();
            dic = TrieDic(tt);
            dic.Should().ContainKey(2).And.HaveCount(1);

            tt = dic[2];
            tt.HasValue.Should().BeTrue();
            tt.Value.Should().Be(10);
            dic = TrieDic(tt);
            dic.Should().BeEmpty();


            // remove mid
            trie.Add(stackalloc int[] { 1, 2, 3 }, -2);
            trie.Remove(stackalloc int[] { 1, 2 });

            tt = trie;
            dic = TrieDic(tt);
            dic.Should().ContainKey(1).And.HaveCount(1);

            tt = dic[1];
            tt.HasValue.Should().BeFalse();
            dic = TrieDic(tt);
            dic.Should().ContainKey(2).And.HaveCount(1);

            tt = dic[2];
            tt.HasValue.Should().BeFalse();
            dic = TrieDic(tt);
            dic.Should().ContainKey(3).And.HaveCount(1);

            tt = dic[3];
            tt.HasValue.Should().BeTrue();
            tt.Value.Should().Be(-2);
            dic = TrieDic(tt);
            dic.Should().BeEmpty();


            // remove last one
            trie.Remove(stackalloc int[] { 1, 2, 3 });

            tt = trie;
            dic = TrieDic(tt);
            dic.Should().BeEmpty();
        }


        [Fact]
        public void CountTest()
        {
            var trie = new Trie<int, int>();
            trie.Count.Should().Be(0);
            trie.Add(stackalloc int[] { 1, 2, 3 }, -1);
            trie.Add(stackalloc int[] { 1, 2 }, -1);
            trie.Add(stackalloc int[] { 2, 2, 4 }, -1);
            trie.Count.Should().Be(3);
            trie.GetChild(stackalloc int[] { 1, 2, 3 }).Count.Should().Be(1);
            trie.GetChild(stackalloc int[] { 1, 2 }).Count.Should().Be(2);

            trie.Remove(stackalloc int[] { 2, 2, 4 });
            trie.Count.Should().Be(2);
            trie.GetChild(stackalloc int[] { 1, 2, 3 }).Count.Should().Be(1);
            trie.GetChild(stackalloc int[] { 1, 2 }).Count.Should().Be(2);

            trie.Remove(stackalloc int[] { 1, 2 });
            trie.Count.Should().Be(1);
            trie.GetChild(stackalloc int[] { 1, 2, 3 }).Count.Should().Be(1);
            trie.GetChild(stackalloc int[] { 1, 2 }).Count.Should().Be(1);
        }


        [Fact]
        public void AllEnumerateTest()
        {
            var trie = new Trie<int, int>();
            trie.Add(stackalloc int[] { 1 }, -2);
            trie.Add(stackalloc int[] { 1, 2 }, 10);
            trie.Add(stackalloc int[] { 1, 2, 3 }, -1);
            trie.Add(stackalloc int[] { 1, -3 }, 35);
            trie.Add(stackalloc int[] { 1, -2 }, 8);
            trie.Add(stackalloc int[] { 1, -2, 5 }, 6);

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
                all.MoveNext().Should().BeTrue();
                var (key, val) = all.Current;
                key.Should().Equal(exKey);
                val.Should().Be(exVal);
            }
            all.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void MatchGreedyTest()
        {
            var trie = new Trie<int, int>();
            trie.Add(stackalloc int[] { 1 }, -2);
            trie.Add(stackalloc int[] { 1, 2 }, 10);
            trie.Add(stackalloc int[] { 1, 2, 3 }, -1);
            trie.Add(stackalloc int[] { 1, -3 }, 35);
            trie.Add(stackalloc int[] { 1, -2 }, 8);
            trie.Add(stackalloc int[] { 1, -2, 5 }, 6);

            var greedy = trie.MatchGreedy(new int[] { 1, -2, 5, 20 }).GetEnumerator();
            var expected = new (int[] key, int val)[]
            {
                (new int[] { 1 }, -2),
                (new int[] { 1, -2 }, 8),
                (new int[] { 1, -2, 5 }, 6),
            };
            foreach (var (exKey, exVal) in expected)
            {
                greedy.MoveNext().Should().BeTrue();
                var (key, val) = greedy.Current;
                key.ToArray().Should().Equal(exKey);
                val.Should().Be(exVal);
            }
            greedy.MoveNext().Should().BeFalse();
        }
    }
}
