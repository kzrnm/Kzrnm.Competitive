using AtCoderProject;
using System.Collections.Generic;





[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
class PriorityQueue<TKey, TValue>
{
    private readonly List<KeyValuePair<TKey, TValue>> data;
    private readonly IComparer<TKey> comparer;
    public PriorityQueue() : this(Comparer<TKey>.Default) { }
    public PriorityQueue(int capacity) : this(capacity, Comparer<TKey>.Default) { }
    public PriorityQueue(IComparer<TKey> comparer) { this.data = new List<KeyValuePair<TKey, TValue>>(); this.comparer = comparer; }
    public PriorityQueue(int capacity, IComparer<TKey> comparer) { this.data = new List<KeyValuePair<TKey, TValue>>(capacity); this.comparer = comparer; }

    public int Count => data.Count;
    public KeyValuePair<TKey, TValue> Peek => data[0];

    public void Add(TKey key, TValue value)
    {
        data.Add(new KeyValuePair<TKey, TValue>(key, value));
        UpdateUp(data.Count - 1);
    }
    public KeyValuePair<TKey, TValue> Dequeue()
    {
        var res = data[0];
        data[0] = data[data.Count - 1];
        data.RemoveAt(data.Count - 1);
        UpdateDown(0);
        return res;
    }

    private void UpdateUp(int i)
    {
        if (i > 0)
        {
            var p = (i - 1) >> 1;
            if (comparer.Compare(data[i].Key, data[p].Key) < 0)
            {
                var tmp = data[p];
                data[p] = data[i];
                data[i] = tmp;
                UpdateUp(p);
            }
        }
    }
    private void UpdateDown(int i)
    {
        var n = data.Count;
        var child = 2 * i + 1;
        if (child < n)
        {
            if (child != n - 1 && comparer.Compare(data[child].Key, data[child + 1].Key) > 0)
                child++;
            if (comparer.Compare(data[i].Key, data[child].Key) > 0)
            {
                var tmp = data[child];
                data[child] = data[i];
                data[i] = tmp;
                UpdateDown(child);
            }
        }
    }

#pragma warning disable IDE0051
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)] private KeyValuePair<TKey, TValue>[] Items => data.ToArray().Sort((a, b) => comparer.Compare(a.Key, b.Key));

}
