using System;
using System.Collections.Generic;
using static AtCoderProject.Global;

class GraphSearch
{
    enum Status
    {
        None,
        Active,
        Done
    }
    Node[] graph;
    Status[] statuses;
    public GraphSearch(Node[] graph)
    {
        this.graph = graph;
        this.statuses = new Status[graph.Length];
    }
    public int[] GetCycleDFS()
    {
        for (var i = 0; i < graph.Length; i++)
        {
            if (statuses[i] == Status.None)
            {
                var res = GetCycleDFS(i);
                if (res != null)
                {
                    res.Reverse();
                    return res.ToArray();
                }
            }
        }
        return null;
    }
    List<int> GetCycleDFS(int v)
    {
        statuses[v] = Status.Active;

        foreach (var child in graph[v].children)
        {
            switch (statuses[child])
            {
                case Status.None:
                    var list = GetCycleDFS(child);
                    if (list != null)
                    {
                        if (list.Count < 2 || list[0] != list[^1])
                            list.Add(v);
                        return list;
                    }
                    break;
                case Status.Active:
                    return new List<int> { child, v };
            }
        }

        statuses[v] = Status.Done;
        return null;
    }

    class BFSData
    {
        public readonly int[] current;
        public readonly bool[] used;
        public BFSData(int[] current, bool[] used)
        {
            this.current = current;
            this.used = used;
        }
    }
    public int[] GetCycleBFS()
    {
        for (var i = 0; i < graph.Length; i++)
        {
            if (statuses[i] == Status.None)
            {
                var res = GetCycleBFS(i);
                if (res != null)
                    return res;
            }
        }
        return null;
    }
    int[] GetCycleBFS(int v)
    {
        int[] res = null;
        statuses[v] = Status.Active;
        var queue = new Queue<BFSData>();
        var bfsd = new BFSData(new[] { v }, new bool[graph.Length]);
        bfsd.used[v] = true;
        queue.Enqueue(bfsd);
        while (queue.Count > 0)
        {
            bfsd = queue.Dequeue();
            foreach (var child in graph[bfsd.current[^1]].children)
            {
                if (bfsd.used[child])
                {
                    var index = Array.IndexOf(bfsd.current, child);
                    if (res == null || res.Length > bfsd.current.Length + 1 - index)
                    {
                        res = new int[bfsd.current.Length + 1 - index];
                        Array.Copy(bfsd.current, index, res, 0, bfsd.current.Length - index);
                        res[^1] = child;
                    }
                }
                else if (res == null)
                {
                    statuses[child] = Status.Done;
                    var next = new int[bfsd.current.Length + 1];
                    Array.Copy(bfsd.current, next, bfsd.current.Length);
                    next[^1] = child;
                    var nextUsed = (bool[])bfsd.used.Clone();
                    nextUsed[child] = true;
                    queue.Enqueue(new BFSData(next, nextUsed));
                }
            }
        }
        return res;
    }
}
