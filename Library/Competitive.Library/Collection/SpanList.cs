using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// <see cref="Span{T}"/> を ArrayList として使う。
    /// </summary>
    public ref struct SpanList<T>
    {
        public Span<T> sp;
        public SpanList(Span<T> span)
        {
            sp = span;
            Count = 0;
        }

        public ref T this[int index] => ref sp[index];
        public int Count { get; private set; }

        [凾(256)] public Span<T> AsSpan() => sp[..Count];
        [凾(256)] public void Add(T item) => sp[Count++] = item;
        [凾(256)] public void RemoveLast() => --Count;
        [凾(256)] public void Clear() => Count = 0;
        [凾(256)] public void Reverse() => AsSpan().Reverse();
        [凾(256)] public Span<T>.Enumerator GetEnumerator() => AsSpan().GetEnumerator();
    }
}
