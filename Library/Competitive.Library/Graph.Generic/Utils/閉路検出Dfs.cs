using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 閉路検出
    public static class 閉路検出Dfs
    {
        enum Status { None, Active, Done }
        /// <summary>
        /// 閉路があれば返す。なければ(-1, null)
        /// </summary>
        [凾(256)]
        public static (int from, TEdge[] edges) GetCycleDfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
             where TNode : IGraphNode<TEdge>
             where TEdge : IGraphEdge
        {
            var g = graph.AsArray();
            var statuses = new Status[g.Length];
            var prevs = new int[g.Length];
            prevs.AsSpan().Fill(-1);
            List<TEdge> Dfs(Stack<(int v, int childIdx)> stack)
            {
                while (stack.TryPop(out var tuple))
                {
                    var (v, ci) = tuple;
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
                                stack.Push((v, ci + 1));
                                stack.Push((child, 0));
                                prevs[child] = v;
                                break;
                            case Status.Active:
                                if (!g[v].IsDirected && child == prevs[v]) // 戻る辺はスキップ
                                    goto default;
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
            var stack = new Stack<(int v, int childIdx)>();
            for (var i = 0; i < g.Length; i++)
            {
                if (statuses[i] == Status.None)
                {
                    stack.Clear();
                    stack.Push((i, 0));
                    if (Dfs(stack) is { } list)
                        while (stack.TryPop(out var tuple))
                        {
                            var (v, ci) = tuple;
                            list.Add(g[v].Children[ci - 1]);
                            if (list[0].To == v)
                            {
                                list.Reverse();
                                return (v, list.ToArray());
                            }
                        }
                }
            }
            return (-1, null);
        }
    }
}