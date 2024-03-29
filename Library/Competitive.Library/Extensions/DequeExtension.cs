using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __DequeExtension
    {
        /// <summary>
        /// <paramref name="collection"/> から <see cref="Deque{T}"/> を作成します。
        /// </summary>
        [凾(256)]
        public static Deque<T> ToDeque<T>(this IEnumerable<T> collection)
        {
            Deque<T> deq;
            if (collection is ICollection<T> ls)
            {
                deq = new Deque<T>(ls.Count);
                ls.CopyTo(deq.data, 0);
                deq.tail = ls.Count;
            }
            else
            {
                deq = new Deque<T>();
                foreach (var item in collection)
                    deq.AddLast(item);
            }
            return deq;
        }
        /// <summary>
        /// <paramref name="collection"/> から <see cref="Deque{T}"/> を作成します。
        /// </summary>
        [凾(256)]
        public static Deque<T> ToDeque<T>(this T[] collection)
            => ToDeque((ReadOnlySpan<T>)collection);
        /// <summary>
        /// <paramref name="collection"/> から <see cref="Deque{T}"/> を作成します。
        /// </summary>
        [凾(256)]
        public static Deque<T> ToDeque<T>(this Span<T> collection)
            => ToDeque((ReadOnlySpan<T>)collection);
        /// <summary>
        /// <paramref name="collection"/> から <see cref="Deque{T}"/> を作成します。
        /// </summary>
        [凾(256)]
        public static Deque<T> ToDeque<T>(this ReadOnlySpan<T> collection)
        {
            var deq = new Deque<T>(collection.Length);
            collection.CopyTo(deq.data);
            deq.tail = collection.Length;
            return deq;
        }
        static void ThrowArgumentOutOfRangeException(string paramName) => throw new ArgumentOutOfRangeException(paramName);

        /// <summary>
        /// <see cref="Deque{T}"/> の先頭 <paramref name="count"/> 個を削除します。
        /// </summary>
        [凾(256)]
        public static void RemoveFirst<T>(this Deque<T> deque, int count)
        {
            if (deque.Count < count) ThrowArgumentOutOfRangeException(nameof(count));
            deque.head = (deque.head + count) & deque.mask;
        }

        /// <summary>
        /// <see cref="Deque{T}"/> の先頭 <paramref name="count"/> 個を削除します。
        /// </summary>
        [凾(256)]
        public static void RemoveLast<T>(this Deque<T> deque, int count)
        {
            if (deque.Count < count) ThrowArgumentOutOfRangeException(nameof(count));
            deque.tail = (deque.tail - count) & deque.mask;
        }
    }
}
