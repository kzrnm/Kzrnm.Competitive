using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 閉路削除
    public static class 閉路削除Dfs
    {
        enum Status { None, Active, Done }
        /// <summary>
        /// 閉路に使わない辺を抽出して返します。
        /// </summary>
        [凾(256)]
        public static (int from, TEdge edges)[] RemoveCycle<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
             where TNode : IGraphNode<TEdge>
             where TEdge : IGraphEdge
        {
            var es = new List<(int from, TEdge edge)>();
            var g = graph.AsArray();
            var statuses = new Status[g.Length];
            var prevs = new int[g.Length];
            prevs.AsSpan().Fill(-1);
            var depths = new int[g.Length];
            var rs = new int[g.Length];
            rs.AsSpan().Fill(-1);
            void Dfs(Stack<(int v, int childIdx)> stack)
            {
                while (stack.TryPop(out var tuple))
                {
                    var (v, ci) = tuple;

                    if (v < 0)
                    {
                        v = ~v;
                        var e = g[v].Children[ci];
                        var nr = rs[e.To];
                        if (rs[v] < 0 || nr >= 0 && depths[rs[v]] > depths[nr])
                            rs[v] = nr;
                        if (nr < 0)
                            es.Add((v, e));
                        stack.Push((v, ci + 1));
                        continue;
                    }

                    var children = g[v].Children;
                    if (ci == 0)
                    {
                        statuses[v] = Status.Active;
                        depths[v] = stack.Count;
                    }

                    if (ci < children.Length)
                    {
                        var e = children[ci];
                        var child = e.To;

                        switch (statuses[child])
                        {
                            case Status.None:
                                stack.Push((~v, ci));
                                stack.Push((child, 0));
                                prevs[child] = v;
                                break;
                            case Status.Active:
                                if (!g[v].IsDirected && child == prevs[v]) { } // 戻る辺はスキップ
                                else if (depths[v] < depths[child]) { } // 既に探索済みのときもスキップ
                                else if (rs[v] < 0 || depths[rs[v]] > depths[child])
                                    rs[v] = child;
                                stack.Push((v, ci + 1));
                                break;
                            default:
                                if (rs[v] < 0) es.Add((v, e));
                                stack.Push((v, ci + 1));
                                break;
                        }
                    }
                    else
                    {
                        if (rs[v] == v) rs[v] = -1;
                        statuses[v] = Status.Done;
                    }
                }
            }
            var stack = new Stack<(int v, int childIdx)>();
            for (var i = 0; i < g.Length; i++)
            {
                if (statuses[i] == Status.None)
                {
                    stack.Clear();
                    stack.Push((i, 0));
                    Dfs(stack);
                }
            }
            return es.ToArray();
        }
    }
}