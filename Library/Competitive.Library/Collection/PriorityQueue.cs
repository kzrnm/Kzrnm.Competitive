using AtCoder.Internal;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// <see cref="PriorityQueueOp{T, TOp}"/> などを生成します。デフォルトでは小さい方が高優先度です。
    /// </summary>
    public static class PriorityQueue
    {
        /// <summary>
        /// 小さい方を優先とする PriorityQueue を作成します。
        /// </summary>
        [凾(256)]
        public static PriorityQueueOp<T, DefaultComparerStruct<T>> Create<T>(int capacity = 16)
            where T : IComparable<T>
            => new PriorityQueueOp<T, DefaultComparerStruct<T>>(capacity);

        /// <summary>
        /// 大きい方を優先とする PriorityQueue を作成します。
        /// </summary>
        [凾(256)]
        public static PriorityQueueOp<T, ReverseComparer<T>> CreateDesc<T>(int capacity = 16)
            where T : IComparable<T>
            => new PriorityQueueOp<T, ReverseComparer<T>>(capacity);

        [凾(256)]
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(TOp comparer)
            where TOp : IComparer<T>
            => new(comparer);

        [凾(256)]
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(int capacity = 16)
            where TOp : IComparer<T>
            => new(capacity, default(TOp));

        [凾(256)]
        public static PriorityQueueOp<T, TOp> Create<T, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<T>
            => new(capacity, comparer);

        /// <summary>
        /// 小さい方を優先とする PriorityQueue を作成します。
        /// </summary>
        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, DefaultComparerStruct<TKey>> CreateDictionary<TKey, TValue>(int capacity = 16)
            where TKey : IComparable<TKey>
            => new(capacity);

        /// <summary>
        /// 大きい方を優先とする PriorityQueue を作成します。
        /// </summary>
        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, ReverseComparer<TKey>> CreateDictionaryDesc<TKey, TValue>(int capacity = 16)
            where TKey : IComparable<TKey>
            => new(capacity);

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(TOp comparer)
            where TOp : IComparer<TKey>
            => new(comparer);

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(int capacity = 16)
            where TOp : IComparer<TKey>
            => new(capacity, default(TOp));

        [凾(256)]
        public static PriorityQueueOp<TKey, TValue, TOp> CreateDictionary<TKey, TValue, TOp>(int capacity, TOp comparer)
            where TOp : IComparer<TKey>
            => new(capacity, comparer);


        [凾(256)]
        public static bool TryDequeue<TKey, T1, T2, TKOp>(this PriorityQueueOp<TKey, (T1, T2), TKOp> pq, out TKey key, out T1 Item1, out T2 Item2) where TKOp : IComparer<TKey>
        {
            var result = pq.TryDequeue(out key, out var tuple);
            (Item1, Item2) = tuple;
            return result;
        }
    }
}
