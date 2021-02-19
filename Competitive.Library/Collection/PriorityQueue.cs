using AtCoder.Internal;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class PriorityQueue
    {
        public static PriorityQueueOp<T, DefaultComparerStruct<T>> Create<T>()
            where T : IComparable<T>
            => new PriorityQueueOp<T, DefaultComparerStruct<T>>();
        public static PriorityQueueOp<T, DefaultComparerStruct<T>> Create<T>(int capacity)
            where T : IComparable<T>
            => new PriorityQueueOp<T, DefaultComparerStruct<T>>(capacity);
        public static PriorityQueueOp<T, TOp> Create<T, TOp>()
            where TOp : struct, IComparer<T>
            => new PriorityQueueOp<T, TOp>(default(TOp));
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(TOp comparer)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(comparer);
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(int capacity)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(capacity, default(TOp));
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(capacity, comparer);
        public static PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>> CreateDictionary<TKey, TValue>()
            where TKey : IComparable<TKey>
            => new PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>>();
        public static PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>> CreateDictionary<TKey, TValue>(int capacity)
            where TKey : IComparable<TKey>
            => new PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>>(capacity);
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>()
            where TOp : struct, IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(default(TOp));
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(TOp comparer)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(comparer);
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(int capacity)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(capacity, default(TOp));
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(capacity, comparer);
    }
}
