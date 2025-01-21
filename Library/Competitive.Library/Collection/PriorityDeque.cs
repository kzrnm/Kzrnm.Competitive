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
    public class PriorityDeque<T> : PriorityDeque<T, DefaultComparerStruct<T>> where T : IComparable<T>
    {
        public PriorityDeque() : base() { }
        public PriorityDeque(int capacity) : base(capacity) { }
    }

    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(typeof(PriorityDeque<,>.DebugView))]
    public class PriorityDeque<T, TOp> : IPriorityQueueOp<T> where TOp : IComparer<T>
    {
        T[] data;
        readonly TOp _comparer;

        public PriorityDeque() : this(default(TOp)) { }
        public PriorityDeque(int capacity) : this(capacity, default(TOp)) { }
        public PriorityDeque(TOp comparer) : this(16, comparer) { }
        public PriorityDeque(int capacity, TOp comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            int size = Math.Max(capacity, 16);
            data = new T[size];
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
        public T PeekMin => data[MinHeapTop];
        /// <summary>
        /// 最大の値
        /// </summary>
        public T PeekMax => data[MaxHeapTop];
        T IPriorityQueueOp<T>.Peek => PeekMin;


        [凾(256)]
        internal void Resize()
        {
            Array.Resize(ref data, data.Length << 1);
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
        public void Enqueue(T value)
        {
            if (Count >= data.Length) Resize();
            data[Count++] = value;
            UpdateUp(Count - 1);
        }

        /// <summary>
        /// 空でなければ最も小さな <paramref name="value"/> を取得します。
        /// </summary>
        [凾(256)]
        public bool TryDequeueMin(out T value)
        {
            if (Count == 0)
            {
                value = default(T);
                return false;
            }
            var ix = MinHeapTop;

            value = data[ix];
            data[ix] = data[--Count];

            UpdateDownMin(ix);
            return true;
        }
        /// <summary>
        /// 最も小さな value を取得します。
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        [凾(256)]
        public T DequeueMin()
        {
            if (TryDequeueMin(out var res)) return res;
            throw new InvalidOperationException("PriorityDeque is empty.");
        }
        T IPriorityQueueOp<T>.Dequeue() => DequeueMin();
        bool IPriorityQueueOp<T>.TryDequeue(out T result) => TryDequeueMin(out result);

        /// <summary>
        /// 空でなければ最も大きな <paramref name="value"/> を取得します。
        /// </summary>
        [凾(256)]
        public bool TryDequeueMax(out T value)
        {
            if (Count == 0)
            {
                value = default(T);
                return false;
            }
            var ix = MaxHeapTop;

            value = data[ix];
            data[ix] = data[--Count];

            UpdateDownMax(ix);
            return true;
        }
        /// <summary>
        /// 空でなければ最も大きな value を取得します。
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        [凾(256)]
        public T DequeueMax()
        {
            if (TryDequeueMax(out var res)) return res;
            throw new InvalidOperationException("PriorityDeque is empty.");
        }

        /// <summary>
        /// <paramref name="value"/> を Enqueue(T) してから Dequeue します。
        /// </summary>
        [凾(256)]
        public T EnqueueDequeueMin(T value)
        {
            var ix = MinHeapTop;
            var res = data[ix];
            if (_comparer.Compare(value, data[ix]) <= 0)
            {
                return value;
            }
            data[ix] = value;
            UpdateDownMin(ix);
            return res;
        }

        /// <summary>
        /// <paramref name="value"/> を Enqueue(T) してから Dequeue します。
        /// </summary>
        [凾(256)]
        public T EnqueueDequeueMax(T value)
        {
            var ix = MaxHeapTop;
            var res = data[ix];
            if (_comparer.Compare(value, data[ix]) >= 0)
            {
                return value;
            }
            data[ix] = value;
            UpdateDownMax(ix);
            return res;
        }

        /// <summary>
        /// Dequeue した値に <paramref name="func"/> を適用して Enqueue(T) します。
        /// </summary>
        [凾(256)]
        public void DequeueMinEnqueue(Func<T, T> func)
        {
            var ix = MinHeapTop;
            data[ix] = func(data[ix]);
            UpdateDownMin(ix);
        }
        /// <summary>
        /// Dequeue した値に <paramref name="func"/> を適用して Enqueue(T) します。
        /// </summary>
        [凾(256)]
        public void DequeueMaxEnqueue(Func<T, T> func)
        {
            var ix = MaxHeapTop;
            data[ix] = func(data[ix]);
            UpdateDownMax(ix);
        }

        [凾(256)]
        void UpdateUp(int i)
        {
            var l = i & ~1;
            var r = i | 1;

            if (r < Count)
            {
                if (_comparer.Compare(data[l], data[r]) > 0)
                {
                    (data[l], data[r]) = (data[r], data[l]);
                }
            }
            else r = l;

            var tar = data[l];
            while (l >= 2)
            {
                int par = (l - 2) >> 2 << 1;
                if (_comparer.Compare(tar, data[par]) < 0)
                {
                    data[l] = data[par];
                    l = par;
                }
                else break;
            }
            data[l] = tar;

            tar = data[r];
            while (r >= 2)
            {
                int par = ((r - 2) >> 2 << 1) | 1;
                if (_comparer.Compare(tar, data[par]) > 0)
                {
                    data[r] = data[par];
                    r = par;
                }
                else break;
            }
            data[r] = tar;
        }
        [凾(256)]
        void UpdateDownMin(int i)
        {
            var tar = data[i];
            var n = Count;
            while (true)
            {
                int ch = (i + 1) << 1;
                if (ch >= n)
                {
                    data[i] = tar;
                    UpdateUp(i);
                    return;
                }
                if (ch + 2 < n && _comparer.Compare(data[ch], data[ch + 2]) > 0)
                    ch += 2;

                if (_comparer.Compare(data[ch], tar) >= 0)
                    break;

                data[i] = data[ch];
                i = ch;
            }
            data[i] = tar;
        }

        [凾(256)]
        void UpdateDownMax(int i)
        {
            var tar = data[i];
            var n = Count;
            while (true)
            {
                int lch = (((i + 1) << 1) - 1) | 1;
                if (lch >= n) --lch;
                if (lch >= n)
                {
                    data[i] = tar;
                    UpdateUp(i);
                    return;
                }
                int rch = (((i + 2) << 1) - 1) | 1;
                if (rch >= n) --rch;

                if (rch < n && _comparer.Compare(data[lch], data[rch]) < 0)
                    lch = rch;

                if (_comparer.Compare(data[lch], tar) <= 0)
                    break;

                data[i] = data[lch];
                i = lch;
            }
            data[i] = tar;
        }

        ReadOnlySpan<T> UnorderdValues() => data.AsSpan(0, Count);

        [SourceExpander.NotEmbeddingSource]
        private class DebugView
        {
            private readonly PriorityDeque<T, TOp> pq;
            public DebugView(PriorityDeque<T, TOp> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get
                {
                    var values = pq.UnorderdValues().ToArray();
                    Array.Sort(values, pq._comparer);
                    return values;
                }
            }
        }
    }

}
