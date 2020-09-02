using System;
using System.Collections.Generic;


class Set<TKey, TValue> : Set<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
{
    private class KeyComparer : IComparer<KeyValuePair<TKey, TValue>>
    {
        public readonly IComparer<TKey> comparer;
        public KeyComparer(IComparer<TKey> comparer)
        {
            this.comparer = comparer;
        }
        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            => comparer.Compare(x.Key, y.Key);
    }
    public Set() : base(new KeyComparer(Comparer<TKey>.Default)) { }
    public Set(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection, new KeyComparer(Comparer<TKey>.Default)) { }
    public Set(IComparer<TKey> comparer) : base(new KeyComparer(comparer)) { }
    public Set(IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> comparer) : base(collection, new KeyComparer(comparer)) { }
    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => Add(key, value);
    public bool Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));
    ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw new NotSupportedException();
    ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new NotSupportedException();
    public Node FindNode(TKey key) => FindNode(KeyValuePair.Create(key, default(TValue)));

    public TValue this[TKey key]
    {
        get => FindNode(KeyValuePair.Create(key, default(TValue))).Item.Value;
        set => Add(key, value);
    }
    public bool ContainsKey(TKey key) => FindNode(key) != null;
    public bool Remove(TKey key) => Remove(KeyValuePair.Create(key, default(TValue)));
    public bool TryGetValue(TKey key, out TValue value)
    {
        if (FindNode(key) is { } node)
        {
            value = node.Item.Value;
            return true;
        }
        value = default;
        return false;
    }
}