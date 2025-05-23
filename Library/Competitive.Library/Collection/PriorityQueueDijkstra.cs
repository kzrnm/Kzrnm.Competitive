using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(PriorityQueueDijkstra<>.DebugView))]
    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    public class PriorityQueueDijkstra<T> : IPriorityQueueOp<KeyValuePair<T, int>>
        where T : IComparable<T>
    {
        private T[] keys;
        private int[] values;
        private int[] indexes;
        public PriorityQueueDijkstra(int capacity)
        {
            keys = new T[capacity];
            values = new int[capacity];
            indexes = new int[capacity].Fill(-1);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count { get; private set; } = 0;

        public KeyValuePair<T, int> Peek => KeyValuePair.Create(keys[0], values[0]);
        [凾(256)]
        void IPriorityQueueOp<KeyValuePair<T, int>>.Enqueue(KeyValuePair<T, int> pair) => Enqueue(pair.Key, pair.Value);
        [凾(256)]
        public void Enqueue(T key, int value)
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
        public bool TryDequeue(out T key, out int value)
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
        public bool TryDequeue(out KeyValuePair<T, int> result)
        {
            if (Count == 0)
            {
                result = new KeyValuePair<T, int>();
                return false;
            }
            result = Dequeue();
            return true;
        }
        [凾(256)]
        public KeyValuePair<T, int> Dequeue()
        {
            indexes[values[0]] = -1;
            var res = KeyValuePair.Create(keys[0], values[0]);
            if (--Count > 0)
            {
                keys[0] = keys[Count];
                values[0] = values[Count];
                indexes[values[0]] = 0;
                UpdateDown(0);
            }
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
                if (tar.CompareTo(keys[p]) >= 0)
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
                if (child != n - 1 && keys[child].CompareTo(keys[child + 1]) > 0) child++;
                if (tar.CompareTo(keys[child]) <= 0)
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
        public ReadOnlySpan<T> UnorderdKeys() => keys.AsSpan(0, Count);
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ReadOnlySpan<int> UnorderdValues() => values.AsSpan(0, Count);
        [SourceExpander.NotEmbeddingSource]
        private class DebugView
        {
            private readonly PriorityQueueDijkstra<T> pq;
            public DebugView(PriorityQueueDijkstra<T> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<T, int>[] Items
            {
                get
                {
                    var count = pq.Count;
                    var keys = pq.keys.AsSpan(0, count).ToArray();
                    var values = pq.values.AsSpan(0, count).ToArray();
                    Array.Sort(keys, values);
                    var arr = new KeyValuePair<T, int>[count];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = KeyValuePair.Create(keys[i], values[i]);
                    return arr;
                }
            }
        }
    }
}
