using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
#if NET7_0_OR_GREATER
using System.Numerics;
#else
using AtCoder.Internal;
#endif

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
    }
}
