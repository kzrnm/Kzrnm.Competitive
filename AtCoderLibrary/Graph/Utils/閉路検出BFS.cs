using System;
using System.Collections.Generic;

namespace AtCoder
{
    public static class 閉路検出BFS
    {
        enum Status { None, Active, Done }
        class BFSData<TEdge> where TEdge : IEdge
        {
            public readonly TEdge[] current;
            public readonly bool[] used;
            public BFSData(TEdge[] current, bool[] used)
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
            where TEdge : IEdge, IReversable<TEdge>
        {
            var statuses = new Status[graph.Length];
            TEdge[] GetCycleBFS(int v)
            {
                TEdge[] res = null;
                statuses[v] = Status.Active;
                var queue = new Queue<BFSData<TEdge>>();
                var bfsd = new BFSData<TEdge>(new[] { default(TEdge).Reversed(v) }, new bool[graph.Length]);
                bfsd.used[v] = true;
                queue.Enqueue(bfsd);
                while (queue.Count > 0)
                {
                    bfsd = queue.Dequeue();
                    foreach (var e in graph[bfsd.current[^1].To].Children)
                    {
                        var child = e.To;
                        if (bfsd.used[child])
                        {
                            var index = Array.IndexOf(bfsd.current, e);
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
                            var next = new TEdge[bfsd.current.Length + 1];
                            Array.Copy(bfsd.current, next, bfsd.current.Length);
                            next[^1] = e;
                            var nextUsed = (bool[])bfsd.used.Clone();
                            nextUsed[child] = true;
                            queue.Enqueue(new BFSData<TEdge>(next, nextUsed));
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
