using AtCoderProject;
using System;
using System.Collections.Generic;




[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
ref struct PriorityQueueRef<T>
{
    private readonly Span<T> orig;
    private Span<T> data;
    private readonly IComparer<T> comparer;

    public PriorityQueueRef(Span<T> orig) : this(orig, Comparer<T>.Default) { }
    public PriorityQueueRef(Span<T> orig, IComparer<T> comparer)
    {
        this.orig = orig;
        this.data = orig[0..0];
        this.comparer = comparer;
    }
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
    public int Count => data.Length;

    public T Peek => data[0];

    public void Add(T value)
    {
        data = orig.Slice(0, data.Length + 1);
        data[^1] = value;
        UpdateUp(data.Length - 1);
    }
    public T Dequeue()
    {
        var res = data[0];
        data[0] = data[^1];
        data = data[..^1];
        UpdateDown(0);
        return res;
    }
    private void UpdateUp(int i)
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
    private void UpdateDown(int i)
    {
        var n = data.Length;
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

    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)] private T[] Items => data.ToArray().Sort(comparer);
}
