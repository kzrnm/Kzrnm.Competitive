using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue>
        : SetDictionary<TKey, TValue, DefaultComparerStruct<TKey>>
        where TKey : IComparable<TKey>
    {
        public SetDictionary(bool isMulti = false) : base(new DefaultComparerStruct<TKey>(), isMulti) { }
        public SetDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dict, bool isMulti = false) : base(dict, new DefaultComparerStruct<TKey>(), isMulti) { }
    }

    [DebuggerTypeProxy(typeof(SetDictionary<,,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetDictionary<TKey, TValue, TOp>
        : SetBase<KeyValuePair<TKey, TValue>, SetDictionary<TKey, TValue, TOp>.C, SetDictionary<TKey, TValue, TOp>.Comparer, SetNode<TKey, TValue>, SetDictionary<TKey, TValue, TOp>.Op>
        , IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TOp : struct, IComparer<TKey>
    {
        public SetDictionary(bool isMulti = false) : this(new TOp(), isMulti) { }
        public SetDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dict, bool isMulti = false) : this(dict, new TOp(), isMulti) { }
        public SetDictionary(TOp comparer, bool isMulti = false) : base(isMulti, new(comparer))
        {
            this.comparer = comparer;
        }
        public SetDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dict, TOp comparer, bool isMulti = false)
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
                var e = GetNodeEnumerator();
                for (int i = 0; i < res.Length; i++)
                {
                    e.MoveNext();
                    res[i] = e.Current.Node.Key;
                }
                return res;
            }
        }
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                var res = new TValue[Count];
                var e = GetNodeEnumerator();
                for (int i = 0; i < res.Length; i++)
                {
                    e.MoveNext();
                    res[i] = e.Current.Node.Value;
                }
                return res;
            }
        }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IDictionary<TKey, TValue>)this).Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IDictionary<TKey, TValue>)this).Values;
        public TValue this[TKey key]
        {
            [凾(256)]
            get => base.FindNode(new C(comparer, key)) is { NodeRef: >= 0, Node.Value: var v } ? v : NotFound<TValue>();
            [凾(256)]
            set
            {
                var node = base.FindNode(new C(comparer, key));
                if (node is { NodeRef: var n })
                    StructPool<SetNode<TKey, TValue>>.Default.Get(n).Value = value;
                else
                    Add(key, value);
            }
        }
        static T NotFound<T>() => throw new KeyNotFoundException();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => Add(pair);
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => Add(KeyValuePair.Create(key, value));
        [凾(256)]
        public bool Add(TKey key, TValue value) => Add(KeyValuePair.Create(key, value));

        [凾(256)] public bool Remove(TKey key) => Remove(new C(comparer, key));
        [凾(256)] public SetResult<KeyValuePair<TKey, TValue>, int, SetNode<TKey, TValue>>? GetAndRemove(TKey key) => GetAndRemove(new C(comparer, key));


        [凾(256)]
        public bool ContainsKey(TKey key) => FindNode(new C(comparer, key)) != null;
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            if (BinarySearch(new C(comparer, pair.Key), new SetLower()) is not { NodeRef: >= 0 } node)// LowerBound
                return false;
            var e = GetNodeEnumerator(false, node.NodeRef);
            while (e.MoveNext())
            {
                if (comparer.Compare(pair.Key, e.Current.Node.Key) != 0) break;
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, e.Current.Node.Value)) return true;
            }
            return false;
        }
        [凾(256)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (base.FindNode(new C(comparer, key)) is { NodeRef: >= 0, Node.Value: var v })
            {
                value = v;
                return true;
            }
            value = default;
            return false;
        }
        #endregion IDictionary

        #region Operators
        public struct Op : ISetPOp<KeyValuePair<TKey, TValue>, C, Comparer, SetNode<TKey, TValue>, Op>
        {
            [凾(256)]
            public static SetNode<TKey, TValue> CreateNode(KeyValuePair<TKey, TValue> v, bool isBlack)
                => new(v.Key, v.Value, isBlack);
            [凾(256)]
            public static C GetCompareKey(Comparer comparer, KeyValuePair<TKey, TValue> item)
                => new(comparer.inner, item.Key);
        }

        public readonly struct C : IComparable<SetNode<TKey, TValue>>
        {
            private readonly TOp op;
            private readonly TKey v;
            public C(TOp op, TKey val) { this.op = op; v = val; }
            [凾(256)] public int CompareTo(SetNode<TKey, TValue> other) => op.Compare(v, other.Key);

            [SourceExpander.NotEmbeddingSource]
            public readonly override string ToString() => $"{v}";
        }
        public readonly struct Comparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            public readonly TOp inner;
            public Comparer(TOp k) { inner = k; }
            [凾(256)] public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => inner.Compare(x.Key, y.Key);
        }
        #endregion Operators

        #region Search
        [凾(256)] public SetFindResult<KeyValuePair<TKey, TValue>, int, SetNode<TKey, TValue>>? FindNode(TKey item) => FindNode(new C(comparer, item));
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public SetFindResult<KeyValuePair<TKey, TValue>, int, SetNode<TKey, TValue>> FindNodeLowerBound(TKey item) => BinarySearch(new C(comparer, item), new SetLower());
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。なければ Count を返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetLower()).Index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetLowerBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            var b = TryGetLowerBound(item, out var key, out var val);
            value = new(key, val);
            return b;
        }
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetLowerBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetLower()) is { NodeRef: >= 0, Node: var n })
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
        [凾(256)] public SetFindResult<KeyValuePair<TKey, TValue>, int, SetNode<TKey, TValue>> FindNodeUpperBound(TKey item) => BinarySearch(new C(comparer, item), new SetUpper());
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。なければ Count を返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetUpper()).Index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetUpperBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            var b = TryGetUpperBound(item, out var key, out var val);
            value = new(key, val);
            return b;
        }
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetUpperBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetUpper()) is { NodeRef: >= 0, Node: var n })
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
        [凾(256)] public SetFindResult<KeyValuePair<TKey, TValue>, int, SetNode<TKey, TValue>> FindNodeReverseLowerBound(TKey item) => BinarySearch(new C(comparer, item), new SetLowerRev());
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。なければ -1 を返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetLowerRev()).Index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseLowerBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            var b = TryGetReverseLowerBound(item, out var key, out var val);
            value = new(key, val);
            return b;
        }
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseLowerBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetLowerRev()) is { NodeRef: >= 0, Node: var n })
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
        [凾(256)] public SetFindResult<KeyValuePair<TKey, TValue>, int, SetNode<TKey, TValue>> FindNodeReverseUpperBound(TKey item) => BinarySearch(new C(comparer, item), new SetUpperRev());
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。なければ -1 を返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex(TKey item) => BinarySearch(new C(comparer, item), new SetUpperRev()).Index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseUpperBound(TKey item, out KeyValuePair<TKey, TValue> value)
        {
            var b = TryGetReverseUpperBound(item, out var key, out var val);
            value = new(key, val);
            return b;
        }
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseUpperBound(TKey item, out TKey key, out TValue value)
        {
            if (BinarySearch(new C(comparer, item), new SetUpperRev()) is { NodeRef: >= 0, Node: var n })
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

        [SourceExpander.NotEmbeddingSource]
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

    namespace Internal
    {
#pragma warning disable IDE0251 // メンバーを 'readonly' にする
        [StructLayout(LayoutKind.Auto)]
        public struct SetNode<TKey, TValue> : ISetNode<KeyValuePair<TKey, TValue>, int>
        {
            public TKey Key;
            public TValue Value;
            public KeyValuePair<TKey, TValue> Pair => new(Key, Value);
            KeyValuePair<TKey, TValue> ISetNode<KeyValuePair<TKey, TValue>, int>.Value => Pair;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Parent { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Left { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Right { get; set; }
            public bool IsBlack { get; set; }
            public int Size { get; set; }

            [SourceExpander.NotEmbeddingSource]
            readonly object DebugParent => SetNodeConv.Load<SetNode<TKey, TValue>>(Parent);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugLeft => SetNodeConv.Load<SetNode<TKey, TValue>>(Left);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugRight => SetNodeConv.Load<SetNode<TKey, TValue>>(Right);

            internal SetNode(TKey key, TValue value, bool isBlack)
            {
                Parent = Left = Right = -1;
                Size = 1;
                IsBlack = isBlack;
                Key = key;
                Value = value;
            }

            [SourceExpander.NotEmbeddingSource]
            public readonly override string ToString() => $"Key = {Key} Value = {Value} Size = {Size}";
        }
#pragma warning restore IDE0251 // メンバーを 'readonly' にする
    }
}
