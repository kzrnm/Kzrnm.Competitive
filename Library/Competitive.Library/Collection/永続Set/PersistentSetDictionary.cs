using Kzrnm.Competitive.Internal;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class PersistentSetDictionary<TKey, TValue> : PersistentSetBase<KeyValuePair<TKey, TValue>, TKey, PersistentSetDictionary<TKey, TValue>, PersistentSetDictionary<TKey, TValue>.PSOperator>
    {
        public readonly struct PSOperator : IPersistentSetNodeOperator<KeyValuePair<TKey, TValue>, TKey>, IPersistentSetOperator<KeyValuePair<TKey, TValue>, TKey, PersistentSetDictionary<TKey, TValue>, PSOperator>
        {
            [凾(256)]
            public PersistentSetDictionary<TKey, TValue> Empty(bool isMulti, IComparer<TKey> comparer)
            => new PersistentSetDictionary<TKey, TValue>(PersistentSetNode<KeyValuePair<TKey, TValue>, TKey, PSOperator>.EmptyNode, isMulti, comparer ?? Comparer<TKey>.Default);

            [凾(256)]
            public PersistentSetDictionary<TKey, TValue> Create(PersistentSetNode<KeyValuePair<TKey, TValue>, TKey, PSOperator> root, bool isMulti, IComparer<TKey> comparer)
                => new PersistentSetDictionary<TKey, TValue>(root, isMulti, comparer);

            [凾(256)] public TKey GetCompareKey(KeyValuePair<TKey, TValue> value) => value.Key;
        }
        private PersistentSetDictionary(PersistentSetNode<KeyValuePair<TKey, TValue>, TKey, PSOperator> root, bool isMulti, IComparer<TKey> comparer)
            : base(root, isMulti, comparer) { }

        [凾(256)]
        public PersistentSetDictionary<TKey, TValue> Add(TKey key, TValue value)
            => Add(KeyValuePair.Create(key, value));
    }
}
