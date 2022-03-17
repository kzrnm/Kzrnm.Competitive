using Kzrnm.Competitive.SetInternals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class SetDictionary<TKey, TValue> : SetDictionary<TKey, TValue, DefaultComparerStruct<TKey>>
        where TKey : IComparable<TKey>
    {
        public SetDictionary(bool isMulti = false) : base(new DefaultComparerStruct<TKey>(), isMulti) { }
        public SetDictionary(IDictionary<TKey, TValue> dict, bool isMulti = false) : base(dict, new DefaultComparerStruct<TKey>(), isMulti) { }
    }

    [DebuggerTypeProxy(typeof(SetDictionary<,,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue, TOp>
        : SetBase<KeyValuePair<TKey, TValue>, TKey, SetDictionary<TKey, TValue, TOp>.Node, SetDictionary<TKey, TValue, TOp>.NodeOperator>,
        IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TOp : struct, IComparer<TKey>
    {
        public SetDictionary(bool isMulti = false) : this(default(TOp), isMulti) { }
        public SetDictionary(IDictionary<TKey, TValue> dict, bool isMulti = false) : this(dict, default(TOp), isMulti) { }
        public SetDictionary(TOp comparer, bool isMulti = false) : base(isMulti, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public SetDictionary(IDictionary<TKey, TValue> dict, TOp comparer, bool isMulti = false)
            : base(isMulti, new NodeOperator(comparer), dict) { }

        protected readonly TOp comparer;
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
            get => FindNode(key).Value;
            [凾(256)]
            set
            {
                var node = FindNode(key);
                if (node == null) Add(key, value);
                else node.Value = value;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => DoAdd(KeyValuePair.Create(key, value));
        [凾(256)]
        public bool Add(TKey key, TValue value) => DoAdd(KeyValuePair.Create(key, value));


        [凾(256)]
        public bool ContainsKey(TKey key) => FindNode(key) != null;
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            var node = FindNodeLowerBound(pair.Key);
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
            var node = FindNode(key);
            if (node == null)
            {
                value = default;
                return false;
            }
            value = node.Value;
            return true;
        }
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
        public struct NodeOperator : ISetOperator<KeyValuePair<TKey, TValue>, TKey, Node>
        {
            private readonly TOp comparer;
            public IComparer<TKey> Comparer => comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [凾(256)]
            public Node Create(KeyValuePair<TKey, TValue> item, NodeColor color) => new Node(item.Key, item.Value, color);
            [凾(256)]
            public KeyValuePair<TKey, TValue> GetValue(Node node) => node.Pair;
            [凾(256)]
            public void SetValue(ref Node node, KeyValuePair<TKey, TValue> value)
            {
                node.Key = value.Key;
                node.Value = value.Value;
            }
            [凾(256)]
            public TKey GetCompareKey(KeyValuePair<TKey, TValue> item) => item.Key;
            [凾(256)]
            public int Compare(TKey x, TKey y) => comparer.Compare(x, y);
            [凾(256)]
            public int Compare(KeyValuePair<TKey, TValue> node1, KeyValuePair<TKey, TValue> node2) => comparer.Compare(node1.Key, node2.Key);
            [凾(256)]
            public int Compare(Node node1, Node node2) => comparer.Compare(node1.Key, node2.Key);
            [凾(256)]
            public int Compare(TKey value, Node node) => comparer.Compare(value, node.Key);
        }
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
