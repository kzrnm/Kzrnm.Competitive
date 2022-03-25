using Kzrnm.Competitive.SetInternals;
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
        : SetBase<KeyValuePair<TKey, TValue>, SetDictionary<TKey, TValue>.C<TKey>, SetDictionary<TKey, TValue>.Node, SetDictionary<TKey, TValue>.NodeOperator>
        , IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public SetDictionary(bool isMulti = false) : base(isMulti, new NodeOperator()) { }
        public SetDictionary(IDictionary<TKey, TValue> dict, bool isMulti = false) : base(isMulti, new NodeOperator(), dict) { }

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

        [凾(256)]
        public bool Remove(TKey key) => DoRemove(new C<TKey>(key));


        [凾(256)]
        public bool ContainsKey(TKey key) => base.FindNode(new C<TKey>(key)) != null;
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            var node = BinarySearch(new C<TKey>(pair.Key), new L()).node; // LowerBound
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
        public class Node : SetNodeBase<Node>
        {
            public TKey Key;
            public TValue Value;
            public KeyValuePair<TKey, TValue> Pair => KeyValuePair.Create(Key, Value);
            internal Node(TKey key, TValue value, NodeColor color) : base(color)
            {
                Key = key;
                Value = value;
            }
            public override string ToString() => $"Key = {Key}, Value = {Value}, Size = {Size}";
        }
        public readonly struct C<Tv> : IComparable<Node> where Tv : IComparable<TKey>
        {
            private readonly Tv v;
            public C(Tv val) { v = val; }
            [凾(256)] public int CompareTo(Node other) => v.CompareTo(other.Key);
        }
        public readonly struct NodeOperator : ISetOperator<KeyValuePair<TKey, TValue>, C<TKey>, Node>
        {
            [凾(256)] public Node Create(KeyValuePair<TKey, TValue> item, NodeColor color) => new Node(item.Key, item.Value, color);
            [凾(256)] public KeyValuePair<TKey, TValue> GetValue(Node node) => node.Pair;
            [凾(256)] public C<TKey> GetCompareKey(KeyValuePair<TKey, TValue> item) => new C<TKey>(item.Key);
            [凾(256)] public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => x.Key.CompareTo(y.Key);
        }
        #endregion Operators

        #region Search
        [凾(256)] public new Node FindNode<Tv>(Tv item) where Tv : IComparable<TKey> => base.FindNode(new C<Tv>(item));
        [凾(256)] public bool Contains<Tv>(Tv item) where Tv : IComparable<TKey> => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new L()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new L()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素を返します。
        /// </summary>
        [凾(256)] public KeyValuePair<TKey, TValue> LowerBoundItem<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new L()).node.Pair;
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeUpperBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new U()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new U()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素を返します。
        /// </summary>
        [凾(256)] public KeyValuePair<TKey, TValue> UpperBoundItem<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new U()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseLowerBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new LR()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new LR()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素を返します。
        /// </summary>
        [凾(256)] public KeyValuePair<TKey, TValue> ReverseLowerBoundItem<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new LR()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseUpperBound<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new UR()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new UR()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素を返します。
        /// </summary>
        [凾(256)] public KeyValuePair<TKey, TValue> ReverseUpperBoundItem<Tv>(Tv item) where Tv : IComparable<TKey> => BinarySearch(new C<Tv>(item), new UR()).node.Pair;
        #endregion Search

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
