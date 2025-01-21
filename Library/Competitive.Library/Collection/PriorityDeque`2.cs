/*
 * Original
 * https://github.com/yosupo06/library-checker-problems/blob/b2d2c050026820706dea6c9f18b8275a0fb0cada/datastructure/double_ended_priority_queue/sol/correct.cpp
 * 
 * Apache License Version 2.0
 * https://github.com/yosupo06/library-checker-problems
 */
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class PriorityDequeDictionary<TKey, TValue> : PriorityDequeDictionary<TKey, TValue, DefaultComparerStruct<TKey>> where TKey : IComparable<TKey>
    {
        public PriorityDequeDictionary() : base() { }
        public PriorityDequeDictionary(int capacity) : base(capacity) { }
    }

    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(typeof(PriorityDequeDictionary<,,>.DebugView))]
    public class PriorityDequeDictionary<TKey, TValue, TOp> : IPriorityQueueOp<KeyValuePair<TKey, TValue>> where TOp : IComparer<TKey>
    {
        TKey[] keys;
        TValue[] values;
        readonly TOp _comparer;

        public PriorityDequeDictionary() : this(default(TOp)) { }
        public PriorityDequeDictionary(TOp comparer) : this(16, comparer) { }
        public PriorityDequeDictionary(int capacity) : this(capacity, default(TOp)) { }
        public PriorityDequeDictionary(int capacity, TOp comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            int size = Math.Max(capacity, 16);
            keys = new TKey[size];
            values = new TValue[size];
            _comparer = comparer;
        }

        /// <summary>
        /// 最小の値を格納するインデックス
        /// </summary>
        static int MinHeapTop => 0;
        /// <summary>
        /// 最大の値を格納するインデックス
        /// </summary>
        int MaxHeapTop { [凾(256)] get => Math.Min(Count - 1, 1); }

        [DebuggerBrowsable(0)]
        public int Count { get; private set; } = 0;
        /// <summary>
        /// 最小の値
        /// </summary>
        public KeyValuePair<TKey, TValue> PeekMin => KeyValuePair.Create(keys[MinHeapTop], values[MinHeapTop]);
        /// <summary>
        /// 最大の値
        /// </summary>
        public KeyValuePair<TKey, TValue> PeekMax => KeyValuePair.Create(keys[MaxHeapTop], values[MaxHeapTop]);
        KeyValuePair<TKey, TValue> IPriorityQueueOp<KeyValuePair<TKey, TValue>>.Peek => PeekMin;


        [凾(256)]
        internal void Resize()
        {
            Array.Resize(ref keys, keys.Length << 1);
            Array.Resize(ref values, values.Length << 1);
        }

        /// <summary>
        /// 格納した値をすべて削除します。
        /// </summary>
        [凾(256)]
        public void Clear()
        {
            Count = 0;
        }

        /// <summary>
        /// 値を追加します。
        /// </summary>
        [凾(256)]
        public void Enqueue(TKey key, TValue value)
        {
            if (Count >= keys.Length) Resize();
            keys[Count] = key;
            values[Count++] = value;
            UpdateUp(Count - 1);
        }
        void IPriorityQueueOp<KeyValuePair<TKey, TValue>>.Enqueue(KeyValuePair<TKey, TValue> pair) => Enqueue(pair.Key, pair.Value);

        /// <summary>
        /// 空でなければ最も小さな <paramref name="key"/> と対応する <paramref name="value"/> を取得します。
        /// </summary>
        [凾(256)]
        public bool TryDequeueMin(out TKey key, out TValue value)
        {
            if (Count == 0)
            {
                key = default(TKey);
                value = default(TValue);
                return false;
            }
            var ix = MinHeapTop;

            key = keys[ix];
            value = values[ix];

            keys[ix] = keys[--Count];
            values[ix] = values[Count];

            UpdateDownMin(ix);
            return true;
        }
        /// <summary>
        /// 空でなければ最も小さな key と対応する value を取得します。
        /// </summary>
        [凾(256)]
        public bool TryDequeueMin(out KeyValuePair<TKey, TValue> result)
        {
            if (TryDequeueMin(out var k, out var v))
            {
                result = KeyValuePair.Create(k, v);
                return true;
            }
            result = default;
            return false;
        }
        /// <summary>
        /// 最も小さな key と対応する value を取得します。
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        [凾(256)]
        public KeyValuePair<TKey, TValue> DequeueMin()
        {
            if (TryDequeueMin(out var res)) return res;
            throw new InvalidOperationException("PriorityDeque is empty.");
        }
        KeyValuePair<TKey, TValue> IPriorityQueueOp<KeyValuePair<TKey, TValue>>.Dequeue() => DequeueMin();
        bool IPriorityQueueOp<KeyValuePair<TKey, TValue>>.TryDequeue(out KeyValuePair<TKey, TValue> result) => TryDequeueMin(out result);

        /// <summary>
        /// 空でなければ最も大きな <paramref name="key"/> と対応する <paramref name="value"/> を取得します。
        /// </summary>
        [凾(256)]
        public bool TryDequeueMax(out TKey key, out TValue value)
        {
            if (Count == 0)
            {
                key = default(TKey);
                value = default(TValue);
                return false;
            }
            var ix = MaxHeapTop;

            key = keys[ix];
            value = values[ix];

            keys[ix] = keys[--Count];
            values[ix] = values[Count];

            UpdateDownMax(ix);
            return true;
        }
        /// <summary>
        /// 空でなければ最も大きな key と対応する value を取得します。
        /// </summary>
        [凾(256)]
        public bool TryDequeueMax(out KeyValuePair<TKey, TValue> result)
        {
            if (TryDequeueMax(out var k, out var v))
            {
                result = KeyValuePair.Create(k, v);
                return true;
            }
            result = default;
            return false;
        }
        /// <summary>
        /// 空でなければ最も大きな key と対応する value を取得します。
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        [凾(256)]
        public KeyValuePair<TKey, TValue> DequeueMax()
        {
            if (TryDequeueMax(out var res)) return res;
            throw new InvalidOperationException("PriorityDeque is empty.");
        }

        /// <summary>
        /// <paramref name="key"/>, <paramref name="value"/> を Enqueue(T) してから Dequeue します。
        /// </summary>
        [凾(256)]
        public KeyValuePair<TKey, TValue> EnqueueDequeueMin(TKey key, TValue value)
        {
            var ix = MinHeapTop;
            var res = KeyValuePair.Create(keys[ix], values[ix]);
            if (_comparer.Compare(key, keys[ix]) <= 0)
            {
                return KeyValuePair.Create(key, value);
            }
            keys[ix] = key;
            values[ix] = value;
            UpdateDownMin(ix);
            return res;
        }

        /// <summary>
        /// <paramref name="key"/>, <paramref name="value"/> を Enqueue(T) してから Dequeue します。
        /// </summary>
        [凾(256)]
        public KeyValuePair<TKey, TValue> EnqueueDequeueMax(TKey key, TValue value)
        {
            var ix = MaxHeapTop;
            var res = KeyValuePair.Create(keys[ix], values[ix]);
            if (_comparer.Compare(key, keys[ix]) >= 0)
            {
                return KeyValuePair.Create(key, value);
            }
            keys[ix] = key;
            values[ix] = value;
            UpdateDownMax(ix);
            return res;
        }

        /// <summary>
        /// Dequeue した値に <paramref name="func"/> を適用して Enqueue(T) します。
        /// </summary>
        [凾(256)]
        public void DequeueMinEnqueue(Func<TKey, TValue, (TKey key, TValue value)> func)
        {
            var ix = MinHeapTop;
            (keys[ix], values[ix]) = func(keys[ix], values[ix]);
            UpdateDownMin(ix);
        }
        /// <summary>
        /// Dequeue した値に <paramref name="func"/> を適用して Enqueue(T) します。
        /// </summary>
        [凾(256)]
        public void DequeueMaxEnqueue(Func<TKey, TValue, (TKey key, TValue value)> func)
        {
            var ix = MaxHeapTop;
            (keys[ix], values[ix]) = func(keys[ix], values[ix]);
            UpdateDownMax(ix);
        }

        [凾(256)]
        void UpdateUp(int i)
        {
            var l = i & ~1;
            var r = i | 1;

            if (r < Count)
            {
                if (_comparer.Compare(keys[l], keys[r]) > 0)
                {
                    (keys[l], keys[r]) = (keys[r], keys[l]);
                    (values[l], values[r]) = (values[r], values[l]);
                }
            }
            else r = l;

            var tar = keys[l];
            var tarVal = values[l];
            while (l >= 2)
            {
                int par = (l - 2) >> 2 << 1;
                if (_comparer.Compare(tar, keys[par]) < 0)
                {
                    keys[l] = keys[par];
                    values[l] = values[par];
                    l = par;
                }
                else break;
            }
            keys[l] = tar;
            values[l] = tarVal;

            tar = keys[r];
            tarVal = values[r];
            while (r >= 2)
            {
                int par = ((r - 2) >> 2 << 1) | 1;
                if (_comparer.Compare(tar, keys[par]) > 0)
                {
                    keys[r] = keys[par];
                    values[r] = values[par];
                    r = par;
                }
                else break;
            }
            keys[r] = tar;
            values[r] = tarVal;
        }
        [凾(256)]
        void UpdateDownMin(int i)
        {
            var tar = keys[i];
            var tarVal = values[i];
            var n = Count;
            while (true)
            {
                int ch = (i + 1) << 1;
                if (ch >= n)
                {
                    keys[i] = tar;
                    values[i] = tarVal;
                    UpdateUp(i);
                    return;
                }
                if (ch + 2 < n && _comparer.Compare(keys[ch], keys[ch + 2]) > 0)
                    ch += 2;

                if (_comparer.Compare(keys[ch], tar) >= 0)
                    break;

                keys[i] = keys[ch];
                values[i] = values[ch];
                i = ch;
            }
            keys[i] = tar;
            values[i] = tarVal;
        }

        [凾(256)]
        void UpdateDownMax(int i)
        {
            var tar = keys[i];
            var tarVal = values[i];
            var n = Count;
            while (true)
            {
                int lch = (((i + 1) << 1) - 1) | 1;
                if (lch >= n) --lch;
                if (lch >= n)
                {
                    keys[i] = tar;
                    values[i] = tarVal;
                    UpdateUp(i);
                    return;
                }
                int rch = (((i + 2) << 1) - 1) | 1;
                if (rch >= n) --rch;

                if (rch < n && _comparer.Compare(keys[lch], keys[rch]) < 0)
                    lch = rch;

                if (_comparer.Compare(keys[lch], tar) <= 0)
                    break;

                keys[i] = keys[lch];
                values[i] = values[lch];
                i = lch;
            }
            keys[i] = tar;
            values[i] = tarVal;
        }

        ReadOnlySpan<TKey> UnorderdKeys() => keys.AsSpan(0, Count);
        ReadOnlySpan<TValue> UnorderdValues() => values.AsSpan(0, Count);

        [SourceExpander.NotEmbeddingSource]
        private class DebugView
        {
            private readonly PriorityDequeDictionary<TKey, TValue, TOp> pq;
            public DebugView(PriorityDequeDictionary<TKey, TValue, TOp> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<TKey, TValue>[] Items
            {
                get
                {
                    var count = pq.Count;
                    var keys = pq.UnorderdKeys().ToArray();
                    var values = pq.UnorderdValues().ToArray();
                    Array.Sort(keys, values, pq._comparer);
                    var arr = new KeyValuePair<TKey, TValue>[count];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = KeyValuePair.Create(keys[i], values[i]);
                    return arr;
                }
            }
        }
    }
    public static class PriorityDequeDictionaryExtension
    {
        [凾(256)]
        public static bool TryDequeueMax<TKey, T1, T2, TKOp>(this PriorityDequeDictionary<TKey, (T1, T2), TKOp> pq, out TKey key, out T1 Item1, out T2 Item2) where TKOp : IComparer<TKey>
        {
            var result = pq.TryDequeueMax(out key, out var tuple);
            (Item1, Item2) = tuple;
            return result;
        }
        [凾(256)]
        public static bool TryDequeueMin<TKey, T1, T2, TKOp>(this PriorityDequeDictionary<TKey, (T1, T2), TKOp> pq, out TKey key, out T1 Item1, out T2 Item2) where TKOp : IComparer<TKey>
        {
            var result = pq.TryDequeueMin(out key, out var tuple);
            (Item1, Item2) = tuple;
            return result;
        }
    }
}