using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(SetDictionary<,,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue, TOp>
        : SetBase<KeyValuePair<TKey, TValue>, SetDictionary<TKey, TValue, TOp>.C, SetDictionary<TKey, TValue, TOp>.Node, SetDictionary<TKey, TValue, TOp>.Comparer>
        , IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TOp : struct, IComparer<TKey>
    {
        public SetDictionary(bool isMulti = false) : this(new TOp(), isMulti) { }
        public SetDictionary(IDictionary<TKey, TValue> dict, bool isMulti = false) : this(dict, new TOp(), isMulti) { }
        public SetDictionary(TOp comparer, bool isMulti = false) : base(isMulti, new(comparer))
        {
            this.comparer = comparer;
        }
        public SetDictionary(IDictionary<TKey, TValue> dict, TOp comparer, bool isMulti = false)
            : base(isMulti, new(comparer), dict)
        {
            this.comparer = comparer;
        }

        protected readonly TOp comparer;

        #region IDictionary
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                var res = new TKey[Count];
                var e = new Enumerator(this, false, null);
                for (int i = 0; i < res.Length; i++)
                {
                    e.MoveNext();
                    res[i] = e.Current.Key;
                }
                return res;
            }
        }
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                var res = new TValue[Count];
                var e = new Enumerator(this, false, null);
                for (int i = 0; i < res.Length; i++)
                {
                    e.MoveNext();
                    res[i] = e.Current.Value;
                }
                return res;
            }
        }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IDictionary<TKey, TValue>)this).Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IDictionary<TKey, TValue>)this).Values;
        public TValue this[TKey key]
        {
            [凾(256)]
            get => FindNode(new C(comparer, key)).Value;
            [凾(256)]
            set
            {
                var node = FindNode(new C(comparer, key));
                if (node == null) Add(key, value);
                else node.Value = value;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => DoAdd(KeyValuePair.Create(key, value));
        [凾(256)]
        public bool Add(TKey key, TValue value) => DoAdd(KeyValuePair.Create(key, value));

        [凾(256)] public bool Remove(TKey key) => DoRemove(new C(comparer, key)) != null;
        [凾(256)] public Node GetAndRemove(TKey key) => DoRemove(new C(comparer, key));


        [凾(256)]
        public bool ContainsKey(TKey key) => FindNode(new C(comparer, key)) != null;
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            var node = BinarySearch(new C(comparer, pair.Key), new SetLower()).node; // LowerBound
            if (node == null) return false;
            var e = new Enumerator(this, false, node);
            while (e.MoveNext())
            {
                if (comparer.Compare(pair.Key, e.Current.Key) != 0) break;
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, e.Current.Value)) return true;
            }
            return false;
        }
        [凾(256)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(new C(comparer, key));
            if (node == null)
            {
                value = default;
                return false;
            }
            value = node.Value;
            return true;
        }
        #endregion IDictionary

        #region Operators
        public class Node : SetNodeBase<Node>, ISetOperator<KeyValuePair<TKey, TValue>, C, Node, Comparer>
        {
            public TKey Key;
            public TValue Value;
            public KeyValuePair<TKey, TValue> Pair => KeyValuePair.Create(Key, Value);
            internal Node(TKey key, TValue value, NodeColor color) : base(color)
            {
                Key = key;
                Value = value;
            }
            [凾(256)]
            public static Node Create(KeyValuePair<TKey, TValue> item, NodeColor color) => new Node(item.Key, item.Value, color);
            [凾(256)]
            public static KeyValuePair<TKey, TValue> GetValue(Node node) => node.Pair;
            [凾(256)]
            public static C GetCompareKey(Comparer op, KeyValuePair<TKey, TValue> item) => new(op.inner, item.Key);
        }
        public readonly struct C : IComparable<Node>
        {
            private readonly TOp op;
            private readonly TKey v;
            public C(TOp op, TKey val) { this.op = op; v = val; }
            [凾(256)] public int CompareTo(Node other) => op.Compare(v, other.Key);
        }
        public readonly struct Comparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            public readonly TOp inner;
            public Comparer(TOp k) { inner = k; }
            [凾(256)] public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => inner.Compare(x.Key, y.Key);
        }
        #endregion Operators

        #region Search
        [凾(256)] public Node FindNode(TKey item) => base.FindNode(new C(comparer, item));
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound(TKey item) => BinarySearch(new C(comparer, item), new SetLower()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetLower()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetLowerBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            if (BinarySearch(new C(comparer, item), new SetLower()).node is { } n)
            {
                value = n.Pair;
                return true;
            }
            value = default;
            return false;
        }
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetLowerBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetLower()).node is { } n)
            {
                key = n.Key;
                value = n.Value;
                return true;
            }
            key = default;
            value = default;
            return false;
        }
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeUpperBound(TKey item) => BinarySearch(new C(comparer, item), new SetUpper()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetUpper()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetUpperBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            if (BinarySearch(new C(comparer, item), new SetUpper()).node is { } n)
            {
                value = n.Pair;
                return true;
            }
            value = default;
            return false;
        }
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetUpperBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetUpper()).node is { } n)
            {
                key = n.Key;
                value = n.Value;
                return true;
            }
            key = default;
            value = default;
            return false;
        }

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseLowerBound(TKey item) => BinarySearch(new C(comparer, item), new SetLowerRev()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetLowerRev()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseLowerBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            if (BinarySearch(new C(comparer, item), new SetLowerRev()).node is { } n)
            {
                value = n.Pair;
                return true;
            }
            value = default;
            return false;
        }
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseLowerBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetLowerRev()).node is { } n)
            {
                key = n.Key;
                value = n.Value;
                return true;
            }
            key = default;
            value = default;
            return false;
        }

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseUpperBound(TKey item) => BinarySearch(new C(comparer, item), new SetUpperRev()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetUpperRev()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseUpperBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            if (BinarySearch(new C(comparer, item), new SetUpperRev()).node is { } n)
            {
                value = n.Pair;
                return true;
            }
            value = default;
            return false;
        }
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseUpperBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetUpperRev()).node is { } n)
            {
                key = n.Key;
                value = n.Value;
                return true;
            }
            key = default;
            value = default;
            return false;
        }
        #endregion Search

#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        private class DebugView
        {
            private readonly IEnumerable<KeyValuePair<TKey, TValue>> collection;
            public DebugView(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            {
                this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<TKey, TValue>[] Items => collection.ToArray();
        }
    }
}
