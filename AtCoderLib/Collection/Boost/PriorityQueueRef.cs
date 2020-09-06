using AtCoderProject;
using System;
using System.Collections.Generic;




[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
ref struct PriorityQueueRef<T>
{
    ListRef<T> data;
    readonly IComparer<T> comparer;

    public PriorityQueueRef(Span<T> buffer) : this(buffer, Comparer<T>.Default) { }
    public PriorityQueueRef(Span<T> buffer, IComparer<T> comparer)
    {
        this.data = new ListRef<T>(buffer);
        this.comparer = comparer;
    }
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
    public int Count => data.Count;

    public T Peek => data[0];

    public void Add(T value)
    {
        data.Add(value);
        UpdateUp(data.Count - 1);
    }
    public T Dequeue()
    {
        var res = data[0];
        data[0] = data[^1];
        data.RemoveAt(data.Count - 1);
        UpdateDown(0);
        return res;
    }
    void UpdateUp(int i)
    {
        if (i > 0)
        {
            var p = (i - 1) >> 1;
            if (comparer.Compare(data[i], data[p]) < 0)
            {
                (data[p], data[i]) = (data[i], data[p]);
                UpdateUp(p);
            }
        }
    }
    void UpdateDown(int i)
    {
        var n = data.Count;
        var child = 2 * i + 1;
        if (child < n)
        {
            if (child != n - 1 && comparer.Compare(data[child], data[child + 1]) > 0) child++;
            if (comparer.Compare(data[i], data[child]) > 0)
            {
                (data[child], data[i]) = (data[i], data[child]);
                UpdateDown(child);
            }
        }
    }
    public void Clear() => data.Clear();
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)] T[] Items => data.ToArray().Sort(comparer);
}
