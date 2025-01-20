using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(SetDictionary<,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue>
        : SetBase<KeyValuePair<TKey, TValue>, SetDictionary<TKey, TValue>.C<TKey>, SetDictionary<TKey, TValue>.Node, SetDictionary<TKey, TValue>.Comparer>
        , IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public SetDictionary(bool isMulti = false) : base(isMulti, new()) { }
        public SetDictionary(IDictionary<TKey, TValue> dict, bool isMulti = false) : base(isMulti, new(), dict) { }

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
            get => base.FindNode(new C<TKey>(key)).Value;
            [凾(256)]
            set
            {
                var node = base.FindNode(new C<TKey>(key));
                if (node == null) Add(key, value);
                else node.Value = value;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => DoAdd(KeyValuePair.Create(key, value));
        [凾(256)]
        public bool Add(TKey key, TValue value) => DoAdd(KeyValuePair.Create(key, value));

        [凾(256)] public bool Remove(TKey key) => DoRemove(new C<TKey>(key)) != null;
        [凾(256)] public Node GetAndRemove(TKey key) => DoRemove(new C<TKey>(key));


        [凾(256)]
        public bool ContainsKey(TKey key) => base.FindNode(new C<TKey>(key)) != null;
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            var node = BinarySearch(new C<TKey>(pair.Key), new SetLower()).node; // LowerBound
            if (node == null) return false;
            var e = new Enumerator(this, false, node);
            while (e.MoveNext())
            {
                if (pair.Key.CompareTo(e.Current.Key) != 0) break;
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, e.Current.Value)) return true;
            }
            return false;
        }
        [凾(256)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = base.FindNode(new C<TKey>(key));
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
        public class Node : SetNodeBase<Node>, ISetOperator<KeyValuePair<TKey, TValue>, C<TKey>, Node, Comparer>
        {
            public TKey Key;
            public TValue Value;
            public KeyValuePair<TKey, TValue> Pair => KeyValuePair.Create(Key, Value);
            internal Node(TKey key, TValue value, NodeColor color) : base(color)
            {
                Key = key;
                Value = value;
            }

#if !LIBRARY
            [SourceExpander.NotEmbeddingSource]
#endif
            public override string ToString() => $"Key = {Key}, Value = {Value}, Size = {Size}";
            [凾(256)] public static Node Create(KeyValuePair<TKey, TValue> item, NodeColor color) => new Node(item.Key, item.Value, color);
            [凾(256)] public static KeyValuePair<TKey, TValue> GetValue(Node node) => node.Pair;
            [凾(256)] public static C<TKey> GetCompareKey(Comparer op, KeyValuePair<TKey, TValue> item) => new(item.Key);
        }
        public readonly struct C<Tv> : IComparable<Node> where Tv : IComparable<TKey>
        {
            private readonly Tv v;
            public C(Tv val) { v = val; }
            [凾(256)] public int CompareTo(Node other) => v.CompareTo(other.Key);
        }
        public readonly struct Comparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            [凾(256)] public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => x.Key.CompareTo(y.Key);
        }
        #endregion Operators

        #region Search
        [凾(256)] public new Node FindNode<Tv>(Tv item) where Tv : IComparable<TKey> => base.FindNode(new C<Tv>(item));
        [凾(256)] public bool Contains<Tv>(Tv item) where Tv : IComparable<TKey> => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetLower()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetLower()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetLowerBound<Tv>(Tv item, out KeyValuePair<TKey, TValue> value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetLower()).node is { } n)
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
        public bool TryGetLowerBound<Tv>(Tv item, out TKey key, out TValue value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetLower()).node is { } n)
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
        [凾(256)] public Node FindNodeUpperBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetUpper()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetUpper()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetUpperBound<Tv>(Tv item, out KeyValuePair<TKey, TValue> value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpper()).node is { } n)
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
        public bool TryGetUpperBound<Tv>(Tv item, out TKey key, out TValue value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpper()).node is { } n)
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
        [凾(256)] public Node FindNodeReverseLowerBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetLowerRev()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetLowerRev()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseLowerBound<Tv>(Tv item, out KeyValuePair<TKey, TValue> value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetLowerRev()).node is { } n)
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
        public bool TryGetReverseLowerBound<Tv>(Tv item, out TKey key, out TValue value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetLowerRev()).node is { } n)
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
        [凾(256)] public Node FindNodeReverseUpperBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetUpperRev()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new SetUpperRev()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseUpperBound<Tv>(Tv item, out KeyValuePair<TKey, TValue> value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpperRev()).node is { } n)
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
        public bool TryGetReverseUpperBound<Tv>(Tv item, out TKey key, out TValue value) where Tv : IComparable<TKey>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpperRev()).node is { } n)
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
