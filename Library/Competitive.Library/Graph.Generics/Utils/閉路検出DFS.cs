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
            var statuses = new Status[graph.Length];
            List<TEdge> DFS(Stack<(int v, int childIdx)> stack)
            {
                while (stack.TryPop(out var v, out var ci))
                {
                    var children = graph[v].Children;
                    if (ci == 0)
                        statuses[v] = Status.Active;

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
                                return new List<TEdge> { e };
                            default:
                                stack.Push((v, ci + 1));
                                break;
                        }
                    }
                    else
                        statuses[v] = Status.Done;
                }

                return null;
            }
            for (var i = 0; i < graph.Length; i++)
            {
                var stack = new Stack<(int v, int childIdx)>();
                if (statuses[i] == Status.None)
                {
                    stack.Push((i, 0));
                    if (DFS(stack) is { } list)
                        while (stack.TryPop(out var v, out var ci))
                        {
                            list.Add(graph[v].Children[ci - 1]);
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