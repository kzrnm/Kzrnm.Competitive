using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(PriorityQueueDijkstra<,>.DebugView))]
    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    public class PriorityQueueDijkstra<TKey, TKOp> : IPriorityQueueOp<KeyValuePair<TKey, int>>
        where TKOp : IComparer<TKey>
    {
        private TKey[] keys;
        private int[] values;
        private int[] indexes;
        private readonly TKOp _comparer;
        public PriorityQueueDijkstra(int capacity) : this(capacity, default) { }
        public PriorityQueueDijkstra(int capacity, TKOp comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            keys = new TKey[capacity];
            values = new int[capacity];
            indexes = new int[capacity].Fill(-1);
            _comparer = comparer;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count { get; private set; } = 0;

        public KeyValuePair<TKey, int> Peek => KeyValuePair.Create(keys[0], values[0]);
        [凾(256)]
        void IPriorityQueueOp<KeyValuePair<TKey, int>>.Enqueue(KeyValuePair<TKey, int> pair) => Enqueue(pair.Key, pair.Value);
        [凾(256)]
        public void Enqueue(TKey key, int value)
        {
            var ix = indexes[value];
            if (ix < 0)
            {
                ix = indexes[value] = Count++;
                values[ix] = value;
            }
            keys[ix] = key;
            UpdateUp(ix);
        }
        [凾(256)]
        public bool TryDequeue(out TKey key, out int value)
        {
            if (Count == 0)
            {
                key = default;
                value = 0;
                return false;
            }
            (key, value) = Dequeue();
            return true;
        }
        [凾(256)]
        public bool TryDequeue(out KeyValuePair<TKey, int> result)
        {
            if (Count == 0)
            {
                result = new KeyValuePair<TKey, int>();
                return false;
            }
            result = Dequeue();
            return true;
        }
        [凾(256)]
        public KeyValuePair<TKey, int> Dequeue()
        {
            indexes[values[0]] = -1;
            var res = KeyValuePair.Create(keys[0], values[0]);
            keys[0] = keys[--Count];
            values[0] = values[Count];
            indexes[values[0]] = 0;
            UpdateDown(0);
            return res;
        }
        [凾(256)]
        private void UpdateUp(int i)
        {
            var tar = keys[i];
            var tarVal = values[i];
            while (i > 0)
            {
                var p = (i - 1) >> 1;
                if (_comparer.Compare(tar, keys[p]) >= 0)
                    break;
                keys[i] = keys[p];
                values[i] = values[p];
                indexes[values[i]] = i;
                i = p;
            }
            keys[i] = tar;
            values[i] = tarVal;
            indexes[values[i]] = i;
        }
        [凾(256)]
        private void UpdateDown(int i)
        {
            var tar = keys[i];
            var tarVal = values[i];
            var n = Count;
            var child = 2 * i + 1;
            while (child < n)
            {
                if (child != n - 1 && _comparer.Compare(keys[child], keys[child + 1]) > 0) child++;
                if (_comparer.Compare(tar, keys[child]) <= 0)
                    break;
                keys[i] = keys[child];
                values[i] = values[child];
                indexes[values[i]] = i;
                i = child;
                child = 2 * i + 1;
            }
            keys[i] = tar;
            values[i] = tarVal;
            indexes[values[i]] = i;
        }
        public void Clear() => Count = 0;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ReadOnlySpan<TKey> UnorderdKeys() => keys.AsSpan(0, Count);
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ReadOnlySpan<int> UnorderdValues() => values.AsSpan(0, Count);
        private class DebugView
        {
            private readonly PriorityQueueDijkstra<TKey, TKOp> pq;
            public DebugView(PriorityQueueDijkstra<TKey, TKOp> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<TKey, int>[] Items
            {
                get
                {
                    var count = pq.Count;
                    var keys = pq.keys.AsSpan(0, count).ToArray();
                    var values = pq.values.AsSpan(0, count).ToArray();
                    Array.Sort(keys, values, pq._comparer);
                    var arr = new KeyValuePair<TKey, int>[count];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = KeyValuePair.Create(keys[i], values[i]);
                    return arr;
                }
            }
        }
    }
}
