using System;
using System.Collections.Generic;

namespace AtCoder.Graph
{
    public static class 閉路検出DFS
    {
        enum Status { None, Active, Done }
        /// <summary>
        /// 閉路があれば返す。なければnull。BFSの方が良さげ
        /// </summary>
        public static TEdge[] GetCycleDFS<TEdge>(this INode<TEdge>[] graph)
            where TEdge : IEdge
        {
            var statuses = new Status[graph.Length];
            List<TEdge> GetCycleDFS(int v)
            {
                statuses[v] = Status.Active;

                foreach (var e in graph[v].Children)
                {
                    var child = e.To;
                    switch (statuses[child])
                    {
                        case Status.None:
                            var list = GetCycleDFS(child);
                            if (list != null)
                            {
                                list.Add(e);
                                return list;
                            }
                            break;
                        case Status.Active:
                            return new List<TEdge> { e };
                    }
                }

                statuses[v] = Status.Done;
                return null;
            }
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
    }
}
