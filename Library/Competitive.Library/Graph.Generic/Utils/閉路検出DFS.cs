using System.Collections.Generic;
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
            var g = graph.AsArray();
            var statuses = new Status[g.Length];
            List<TEdge> DFS(Stack<(int v, int childIdx, int prev)> stack)
            {
                while (stack.TryPop(out var tuple))
                {
                    var (v, ci, prev) = tuple;
                    var children = g[v].Children;
                    if (ci == 0)
                        statuses[v] = Status.Active;

                    if (ci < children.Length)
                    {
                        var e = children[ci];
                        var child = e.To;

                        switch (statuses[child])
                        {
                            case Status.None:
                                stack.Push((v, ci + 1, prev));
                                stack.Push((child, 0, v));
                                break;
                            case Status.Active:
                                if (!g[v].IsDirected && child == prev)
                                    goto default;
                                return new List<TEdge> { e };
                            default:
                                stack.Push((v, ci + 1, prev));
                                break;
                        }
                    }
                    else
                        statuses[v] = Status.Done;
                }

                return null;
            }
            for (var i = 0; i < g.Length; i++)
            {
                var stack = new Stack<(int v, int childIdx, int prev)>();
                if (statuses[i] == Status.None)
                {
                    stack.Push((i, 0, -1));
                    if (DFS(stack) is { } list)
                        while (stack.TryPop(out var tuple))
                        {
                            var (v, ci, _) = tuple;
                            list.Add(g[v].Children[ci - 1]);
                            if (list[0].To == v)
                            {
                                list.Reverse();
                                return (v, list.ToArray());
                            }
                        }
                    stack.Clear();
                }
            }
            return (-1, null);
        }
    }
}