using System;
using System.Collections.Generic;

namespace AtCoder
{
    public class Trie<TKey, TValue>
    {
        readonly Dictionary<TKey, Trie<TKey, TValue>> children;
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
        public Trie() : this(EqualityComparer<TKey>.Default) { }
        public Trie(IEqualityComparer<TKey> comparer) { children = new Dictionary<TKey, Trie<TKey, TValue>>(comparer); }
        public Trie<TKey, TValue> Next(TKey key) => children.Get(key);
        public Trie<TKey, TValue> GetChild(ReadOnlySpan<TKey> key, bool force = false)
        {
            var trie = this;
            foreach (var k in key)
            {
                var child = trie.Next(k);
                if (child == null)
                {
                    if (!force) return null;
                    child = trie.children[k] = new Trie<TKey, TValue>(trie.children.Comparer);
                }
                trie = child;
            }
            return trie;
        }
        public void Add(ReadOnlySpan<TKey> key, TValue value)
            => GetChild(key, true).Value = value;
        public bool Remove(ReadOnlySpan<TKey> key)
        {
            var stack = new Stack<(TKey k, Trie<TKey, TValue> trie)>(key.Length + 1);
            var trie = this;
            stack.Push((default, trie));
            foreach (var k in key)
            {
                trie = trie.Next(k);
                if (trie == null) return false;
                stack.Push((k, trie));
            }

            var cur = stack.Pop();
            if (!cur.trie.HasValue) return false;
            cur.trie.HasValue = false;
            while (stack.Count > 0 && !cur.trie.HasValue && cur.trie.children.Count == 0)
            {
                var prevK = cur.k;
                cur = stack.Pop();
                cur.trie.children.Remove(prevK);
            }
            return true;
        }
        public TValue Get(ReadOnlySpan<TKey> key)
        {
            if (TryGet(key, out var val)) return val;
            throw new KeyNotFoundException();
        }
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
        public IEnumerable<KeyValuePair<TKey[], TValue>> All() => All(new List<TKey>());
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
                        trie = trie.Next(span[++len]);
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
    }

    public class Trie<T> : Trie<T, bool>
    {
        public Trie() : base() { }
        public Trie(IEqualityComparer<T> comparer) : base(comparer) { }
        public void Add(ReadOnlySpan<T> key) => Add(key, true);
        public bool Contains(ReadOnlySpan<T> key) => GetChild(key)?.Value == true;
    }
}