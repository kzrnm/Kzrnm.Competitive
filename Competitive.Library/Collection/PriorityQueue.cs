using AtCoder.Internal;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class PriorityQueue
    {

        [凾(256)]
        public static PriorityQueueOp<T, DefaultComparerStruct<T>> Create<T>()
            where T : IComparable<T>
            => new PriorityQueueOp<T, DefaultComparerStruct<T>>();

        [凾(256)]
        public static PriorityQueueOp<T, DefaultComparerStruct<T>> Create<T>(int capacity)
            where T : IComparable<T>
            => new PriorityQueueOp<T, DefaultComparerStruct<T>>(capacity);

        [凾(256)]
        public static PriorityQueueOp<T, TOp> Create<T, TOp>()
            where TOp : struct, IComparer<T>
            => new PriorityQueueOp<T, TOp>(default(TOp));

        [凾(256)]
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(TOp comparer)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(comparer);

        [凾(256)]
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(int capacity)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(capacity, default(TOp));

        [凾(256)]
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(capacity, comparer);

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>> CreateDictionary<TKey, TValue>()
            where TKey : IComparable<TKey>
            => new PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>>();

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>> CreateDictionary<TKey, TValue>(int capacity)
            where TKey : IComparable<TKey>
            => new PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>>(capacity);

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>()
            where TOp : struct, IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(default(TOp));

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(TOp comparer)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(comparer);

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(int capacity)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(capacity, default(TOp));

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(capacity, comparer);


        [凾(256)]
        public static bool TryDequeue<TKey, T, TKOp>(this PriorityQueueOp<TKey, (T, T), TKOp> pq, out TKey key, out T Item1, out T Item2) where TKOp : IComparer<TKey>
        {
            var result = pq.TryDequeue(out key, out var tuple);
            (Item1, Item2) = tuple;
            return result;
        }
    }
}
