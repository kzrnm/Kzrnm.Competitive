﻿using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class 閉路検出DFS
    {
        enum Status { None, Active, Done }
        /// <summary>
        /// 閉路があれば返す。なければ(-1, null)
        /// </summary>
        [凾(256)]
        public static (int from, TEdge[] edges) GetCycleDFS<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
             where TNode : IGraphNode<TEdge>
             where TEdge : IGraphEdge
        {
            var statuses = new Status[graph.Length];
            (int from, List<TEdge> edges) DFS(Stack<(int v, int childIdx)> stack)
            {
                List<TEdge> list = null;
                while (stack.TryPop(out var v, out var ci))
                {
                    var children = graph[v].Children;
                    if (ci == 0)
                        statuses[v] = Status.Active;
                    else if (list != null)
                    {
                        list.Add(children[ci - 1]);
                        if (list[0].To == v)
                            return (v, list);
                        continue;
                    }

                    if (ci < children.Length)
                    {
                        var e = children[ci];
                        var child = e.To;
                        switch (statuses[child])
                        {
                            case Status.None:
                                stack.Push((v, ci + 1));
                                stack.Push((child, 0));
                                break;
                            case Status.Active:
                                list = new List<TEdge> { e };
                                break;
                        }
                    }
                    else
                        statuses[v] = Status.Done;
                }
                return (-1, null);
            }
            for (var i = 0; i < graph.Length; i++)
            {
                var stack = new Stack<(int v, int childIdx)>();
                if (statuses[i] == Status.None)
                {
                    stack.Push((i, 0));
                    var (from, res) = DFS(stack);
                    if (res != null)
                    {
                        res.Reverse();
                        return (from, res.ToArray());
                    }
                    stack.Clear();
                }
            }
            return (-1, null);
        }
    }
}