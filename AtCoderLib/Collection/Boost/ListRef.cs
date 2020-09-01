using System;



[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
ref struct ListRef<T>
{
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
    private Span<T> data;
    private Span<T> orig;
    public ListRef(Span<T> orig)
    {
        this.orig = orig;
        this.data = default;
    }
    public ref T this[int index] => ref data[index];
    public int Count => data.Length;

    public void Add(T item)
    {
        data = orig.Slice(0, data.Length + 1);
        data[^1] = item;
    }

    public void Clear() => data.Clear();

    public void CopyTo(Span<T> dest) => data.CopyTo(dest);

    public void RemoveAt(int index)
    {
        for (int i = index + 1; i < data.Length; i++) data[i - 1] = data[i];
        data = data[..^1];
    }
    Span<T>.Enumerator GetEnumerator() => data.GetEnumerator();
    public T[] ToArray() => data.ToArray();
}