using AtCoder.Internal;
using System;
using System.Collections.Generic;

namespace AtCoder
{
    public static class PriorityQueue
    {
        public static PriorityQueueOp<T, DefaultComparerStruct<T>> Create<T>()
            where T : IComparable<T>
            => new PriorityQueueOp<T, DefaultComparerStruct<T>>();
        public static PriorityQueueOp<T, DefaultComparerStruct<T>> Create<T>(int capacity)
            where T : IComparable<T>
            => new PriorityQueueOp<T, DefaultComparerStruct<T>>(capacity);
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(TOp comparer)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(comparer);
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<T>
            => new PriorityQueueOp<T, TOp>(capacity, comparer);
        public static PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>> Create<TKey, TValue>()
            where TKey : IComparable<TKey>
            => new PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>>();
        public static PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>> Create<TKey, TValue>(int capacity)
            where TKey : IComparable<TKey>
            => new PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>>(capacity);
        public static PriorityQueueOp<TKey, TValue, TOp> Create<TKey, TValue, TOp>(TOp comparer)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(comparer);
        public static PriorityQueueOp<TKey, TValue, TOp> Create<TKey, TValue, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<TKey>
            => new PriorityQueueOp<TKey, TValue, TOp>(capacity, comparer);
    }
}
