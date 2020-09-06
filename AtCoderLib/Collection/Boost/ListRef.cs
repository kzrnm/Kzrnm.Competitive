using System;



[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
ref struct ListRef<T>
{
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
    Span<T> data;
    Span<T> buffer;
    public ListRef(Span<T> buffer)
    {
        this.buffer = buffer;
        this.data = default;
    }
    public ref T this[int index] => ref data[index];
    public int Count => data.Length;

    public void Add(T item)
    {
        data = buffer.Slice(0, data.Length + 1);
        data[^1] = item;
    }

    public void Clear() => data = default;

    public void CopyTo(Span<T> dest) => data.CopyTo(dest);

    public void RemoveAt(int index)
    {
        for (int i = index + 1; i < data.Length; i++) data[i - 1] = data[i];
        data = data[..^1];
    }
    Span<T>.Enumerator GetEnumerator() => data.GetEnumerator();
    public T[] ToArray() => data.ToArray();
}