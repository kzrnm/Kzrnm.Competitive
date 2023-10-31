using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
#pragma warning disable IDE0251 // readonly
    /// <summary>
    /// 機能を削った高速なArrayList
    /// </summary>
    [DebuggerTypeProxy(typeof(PoolList<>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public struct PoolList<T> : IDisposable
    {
        internal T[] data;
        public PoolList(int capacity)
        {
            data = ArrayPool<T>.Shared.Rent(capacity);
            Count = 0;
        }
        public PoolList(IEnumerable<T> collection)
        {
            int count;
            if (collection is ICollection<T> col)
            {
                data = ArrayPool<T>.Shared.Rent(count = col.Count);
                col.CopyTo(data, 0);
                Count = count;
            }
            else
            {
                if (!collection.TryGetNonEnumeratedCount(out count))
                    count = 16;
                Count = 0;
                data = ArrayPool<T>.Shared.Rent(count);
                foreach (var item in collection)
                    Add(item);
            }
        }

        [凾(256)] public Memory<T> AsMemory() => new Memory<T>(data, 0, Count);
        [凾(256)] public Span<T> AsSpan() => new Span<T>(data, 0, Count);

        public ref T this[int index]
        {
            [凾(256)]
            get
            {
                if ((uint)index >= (uint)Count)
                    ThrowIndexOutOfRangeException();
                return ref data[index];
            }
        }
        public int Count { get; private set; }

        /// <summary>
        /// <paramref name="item"/> を末尾に追加します。
        /// </summary>
        [凾(256)]
        public void Add(T item)
        {
            var d = data;
            if (d == null)
            {
                data = ArrayPool<T>.Shared.Rent(16);
            }
            else if ((uint)Count >= (uint)data.Length)
            {
                ArrayPool<T>.Shared.Return(d);
                data = ArrayPool<T>.Shared.Rent(d.Length << 1);
            }
            data[Count++] = item;
        }

        /// <summary>
        /// 末尾の要素を削除します。
        /// </summary>
        [凾(256)]
        public void RemoveLast()
        {
            if (--Count < 0)
                ThrowIndexOutOfRangeException();
        }

        /// <summary>
        /// 末尾の要素を <paramref name="size"/> 個削除します。
        /// </summary>
        [凾(256)]
        public void RemoveLast(int size)
        {
            if ((Count -= size) < 0)
                ThrowIndexOutOfRangeException();
        }

        [凾(256)]
        public PoolList<T> Reverse()
        {
            Array.Reverse(data, 0, Count);
            return this;
        }
        [凾(256)]
        public PoolList<T> Reverse(int index, int count)
        {
            Array.Reverse(data, index, count);
            return this;
        }
        [凾(256)]
        public PoolList<T> Sort()
        {
            Array.Sort(data, 0, Count);
            return this;
        }
        [凾(256)]
        public PoolList<T> Sort<TComparer>(TComparer comparer) where TComparer : IComparer<T>
        {
            AsSpan().Sort(comparer);
            return this;
        }
        [凾(256)]
        public PoolList<T> Sort(int index, int count, IComparer<T> comparer)
        {
            Array.Sort(data, index, count, comparer);
            return this;
        }
        [凾(256)] public void Clear() => Count = 0;
        [凾(256)] public bool Contains(T item) => IndexOf(item) >= 0;
        [凾(256)] public int IndexOf(T item) => Array.IndexOf(data, item, 0, Count);
        [凾(256)] public void CopyTo(T[] array, int arrayIndex) => Array.Copy(data, 0, array, arrayIndex, Count);
        [凾(256)] public T[] ToArray() => AsSpan().ToArray();

        public void Dispose()
        {
            var d = data;
            if (d != null) ArrayPool<T>.Shared.Return(d);
            this = default;
        }

        [凾(256)] public Span<T>.Enumerator GetEnumerator() => AsSpan().GetEnumerator();
        private static void ThrowIndexOutOfRangeException() => throw new IndexOutOfRangeException();

#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly ref struct DebugView
        {
            private readonly PoolList<T> list;
            public DebugView(PoolList<T> l)
            {
                list = l;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items => list.ToArray();
        }
    }
}
