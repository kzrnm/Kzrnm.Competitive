using System;
using System.Collections.Generic;

namespace AtCoder.Graph
{
    public static class 閉路検出BFS
    {
        enum Status { None, Active, Done }
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
        /// <summary>
        /// 閉路があれば返す。なければnull
        /// </summary>
        public static TEdge[] GetCycleBFS<TNode, TEdge>(this Graph<TNode, TEdge> graph)
            where TNode : INode<TEdge>
            where TEdge : IEdge
        {
            var statuses = new Status[graph.Length];
            TEdge[] GetCycleBFS(int v)
            {
                TEdge[] res = null;
                statuses[v] = Status.Active;
                var queue = new Queue<BFSData>();
                var bfsd = new BFSData(new[] { v }, new bool[graph.Length]);
                bfsd.used[v] = true;
                queue.Enqueue(bfsd);
                while (queue.Count > 0)
                {
                    bfsd = queue.Dequeue();
                    foreach (var e in graph[bfsd.current[^1]].Children)
                    {
                        var child = e.To;
                        if (bfsd.used[child])
                        {
                            var index = Array.IndexOf(bfsd.current, child);
                            if (res == null || res.Length > bfsd.current.Length + 1 - index)
                            {
                                res = new TEdge[bfsd.current.Length + 1 - index];
                                Array.Copy(bfsd.current, index, res, 0, bfsd.current.Length - index);
                                res[^1] = e;
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
    }
}
