using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(Trie<,>.DebugView))]
    public class Trie<TKey, TValue>
    {
        public Trie() : this(null) { }
        public Trie(IComparer<TKey> comparer) { children = new SortedDictionary<TKey, Trie<TKey, TValue>>(comparer); }

        readonly SortedDictionary<TKey, Trie<TKey, TValue>> children;
        public bool HasValue { private set; get; }
        TValue _Value;
        public TValue Value
        {
            set
            {
                _Value = value;
                HasValue = true;
            }
            get => _Value;
        }
        public int Count { private set; get; }

        /// <summary>
        /// <para><paramref name="index"/>から要素を取得する。</para>
        /// <para>計算量: O(N) 要素がバラけていると O(log N)</para>
        /// </summary>
        public TValue this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count) throw new IndexOutOfRangeException();
                if (HasValue && --index == -1) return _Value;
                foreach (var trie in children.Values)
                {
                    if ((uint)index < (uint)trie.Count)
                        return trie[index];
                    index -= trie.Count;
                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// <para>現在のTrieの<paramref name="key"/>子要素を取得する存在しなければnull。</para>
        /// <para>計算量: O(|<paramref name="key"/>|)</para>
        /// </summary>
        public Trie<TKey, TValue> GetChild(ReadOnlySpan<TKey> key)
        {
            var trie = this;
            foreach (var k in key)
            {
                if (!trie.children.TryGetValue(k, out var child))
                    return null;
                trie = child;
            }
            return trie;
        }

        /// <summary>
        /// <para>現在のTrieの<paramref name="key"/>子要素を追加する。</para>
        /// <para>計算量: O(|<paramref name="key"/>|)</para>
        /// </summary>
        /// <returns>追加された子要素</returns>
        public Trie<TKey, TValue> Add(ReadOnlySpan<TKey> key, TValue value)
        {
            var trie = this;
            if (key.Length == 0)
            {
                if (!this.HasValue)
                {
                    ++this.Count;
                }
                this.Value = value;
                return this;
            }

            var updatedTries = new List<Trie<TKey, TValue>>(key.Length);
            ++trie.Count;
            foreach (var k in key)
            {
                if (!trie.children.TryGetValue(k, out var child))
                {
                    child = trie.children[k] = new Trie<TKey, TValue>(trie.children.Comparer);
                    updatedTries.Clear();
                }
                trie = child;
                updatedTries.Add(trie);
                ++trie.Count;
            }
            if (updatedTries.Count == key.Length && trie.HasValue)
            {
                --this.Count;
                foreach (var trie2 in updatedTries.AsSpan())
                {
                    --trie2.Count;
                }
            }
            trie.Value = value;
            return trie;
        }

        /// <summary>
        /// <para>現在のTrieの<paramref name="key"/>子要素を取得する存在しなければnull。</para>
        /// <para>計算量: O(|<paramref name="key"/>|)</para>
        /// </summary>
        public TValue this[ReadOnlySpan<TKey> key]
        {
            [凾(256)]
            get
            {
                var trie = GetChild(key);
                if (trie?.HasValue != true)
                    ThrowKeyNotFoundException();
                return trie.Value;
            }
            [凾(256)]
            set => Add(key, value);
        }
        private static void ThrowKeyNotFoundException() => throw new KeyNotFoundException();

        public bool Remove(ReadOnlySpan<TKey> key)
        {
            var stack = new Stack<(TKey k, Trie<TKey, TValue> trie)>(key.Length + 1);
            var trie = this;
            stack.Push((default, trie));
            foreach (var k in key)
            {
                if (!trie.children.TryGetValue(k, out trie))
                    return false;
                stack.Push((k, trie));
            }

            var cur = stack.Pop();
            if (!cur.trie.HasValue) return false;
            cur.trie.HasValue = false;
            if (--cur.trie.Count == 0)
            {
                var prevK = cur.k;
                while (stack.TryPop(out cur))
                {
                    --cur.trie.Count;
                    if (cur.trie.Count > 0 || stack.Count == 0)
                    {
                        cur.trie.children.Remove(prevK);
                        break;
                    }
                    prevK = cur.k;
                }
            }
            while (stack.TryPop(out cur))
            {
                --cur.trie.Count;
            }
            return true;
        }
        [凾(256)]
        public bool TryGet(ReadOnlySpan<TKey> key, out TValue value)
        {
            var child = GetChild(key);
            if (child == null || !child.HasValue)
            {
                value = default;
                return false;
            }
            value = child.Value;
            return true;
        }
        IEnumerable<KeyValuePair<TKey[], TValue>> All(List<TKey> list)
        {
            if (this.HasValue)
                yield return KeyValuePair.Create(list.ToArray(), this.Value);

            foreach (var (k, trie) in children)
            {
                list.Add(k);
                foreach (var p in trie.All(list))
                    yield return p;
                list.RemoveAt(list.Count - 1);
            }
        }
        [凾(256)]
        public IEnumerable<KeyValuePair<TKey[], TValue>> All() => All(new List<TKey>());
        public void Clear()
        {
            HasValue = false;
            children.Clear();
        }

        /// <summary>
        /// <paramref name="key"/> の prefix となる Trie の値を返します。
        /// </summary>
        [凾(256)]
        public MatchEnumerator MatchGreedy(ReadOnlySpan<TKey> key)
            => new MatchEnumerator(this, key);
        public ref struct MatchEnumerator
        {
            Trie<TKey, TValue> trie;
            readonly ReadOnlySpan<TKey> span;
            public MatchEnumerator Current => this;
            int len;
            TValue value;
            public MatchEnumerator(Trie<TKey, TValue> trie, ReadOnlySpan<TKey> span)
            {
                this.trie = trie;
                this.span = span;
                len = -1;
                value = default;
            }
            public bool MoveNext()
            {
                bool ok = false;
                while (trie != null && !ok)
                {
                    if (trie.HasValue)
                    {
                        value = trie.Value;
                        ok = true;
                    }
                    if (len + 1 < span.Length)
                        trie = trie.children.Get(span[++len]);
                    else
                    {
                        ++len;
                        trie = null;
                    }
                }
                return ok;
            }
            public MatchEnumerator GetEnumerator() => this;
            public void Deconstruct(out ReadOnlySpan<TKey> key, out TValue value)
            {
                key = span[..len];
                value = this.value;
            }
        }

        [DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
        private struct DebugItem
        {
            public DebugItem(TKey[] key, TValue value)
            {
                this.key = string.Join(", ", key);
                this.value = value;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly string key;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly TValue value;
        }
        private class DebugView
        {
            private readonly Trie<TKey, TValue> trie;
            public DebugView(Trie<TKey, TValue> trie)
            {
                this.trie = trie;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items => trie.All().Select(p => new DebugItem(p.Key, p.Value)).ToArray();
        }
    }

    public class Trie<T> : Trie<T, bool>
    {
        public Trie() : base() { }
        public Trie(IComparer<T> comparer) : base(comparer) { }
        [凾(256)]
        public void Add(ReadOnlySpan<T> key) => Add(key, true);
        [凾(256)]
        public bool Contains(ReadOnlySpan<T> key) => GetChild(key)?.Value == true;
    }
}