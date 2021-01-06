using System;
using System.Collections.Generic;
using System.Linq;
using AtCoder.Internal;

namespace AtCoder
{
    public static class 強連結成分分解
    {
        /// <summary>
        /// 強連結成分ごとに ID を割り振り、各頂点の所属する強連結成分の ID が記録された配列を取得します。
        /// </summary>
        /// <remarks>
        /// <para>強連結成分の ID はトポロジカルソートされています。異なる強連結成分の頂点 u, v について、u から v に到達できる時、u の ID は v の ID よりも小さくなります。</para>
        /// <para>計算量: 追加された辺の本数を m として O(n+m)</para>
        /// </remarks>
        public static (int groupNum, int[] ids) SCCIDs<TNode, TEdge>(IGraph<TNode, TEdge> graph)
            where TNode : INode<TEdge>
            where TEdge : IEdge
        {
            var graphArr = graph.AsArray();
            var edges = new List<(int from, TEdge edge)>();
            for (int i = 0; i < graphArr.Length; i++)
                foreach (var e in graphArr[i].Children)
                    edges.Add((i, e));
            // R. Tarjan のアルゴリズム
            var g = new CSR<TEdge>(graphArr.Length, edges);
            int nowOrd = 0;
            int groupNum = 0;
            var visited = new Stack<int>(graphArr.Length);
            var low = new int[graphArr.Length];
            var ord = Enumerable.Repeat(-1, graphArr.Length).ToArray();
            var ids = new int[graphArr.Length];

            for (int i = 0; i < ord.Length; i++)
            {
                if (ord[i] == -1)
                {
                    DFS(i);
                }
            }

            for (int i = 0; i < ids.Length; i++)
            {
                // トポロジカル順序にするには逆順にする必要がある。
                ids[i] = groupNum - 1 - ids[i];
            }

            return (groupNum, ids);

            void DFS(int v)
            {
                low[v] = nowOrd;
                ord[v] = nowOrd++;
                visited.Push(v);

                // 頂点 v から伸びる有向辺を探索する。
                for (int i = g.Start[v]; i < g.Start[v + 1]; i++)
                {
                    int to = g.EList[i].To;
                    if (ord[to] == -1)
                    {
                        DFS(to);
                        low[v] = Math.Min(low[v], low[to]);
                    }
                    else
                    {
                        low[v] = Math.Min(low[v], ord[to]);
                    }
                }

                // v がSCCの根である場合、強連結成分に ID を割り振る。
                if (low[v] == ord[v])
                {
                    while (true)
                    {
                        int u = visited.Pop();
                        ord[u] = graphArr.Length;
                        ids[u] = groupNum;

                        if (u == v)
                        {
                            break;
                        }
                    }

                    groupNum++;
                }
            }
        }

        /// <summary>
        /// 強連結成分分解の結果である「頂点のリスト」のリストを取得します。
        /// </summary>
        /// <remarks>
        /// <para>- 全ての頂点がちょうど1つずつ、どれかのリストに含まれます。</para>
        /// <para>- 内側のリストと強連結成分が一対一に対応します。リスト内での頂点の順序は未定義です。</para>
        /// <para>- リストはトポロジカルソートされています。異なる強連結成分の頂点 u, v について、u から v に到達できる時、u の属するリストは v の属するリストよりも前です。</para>
        /// <para>計算量: 追加された辺の本数を m として O(n+m)</para>
        /// </remarks>
        public static List<List<int>> Scc<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : INode<TEdge>
            where TEdge : IEdge
        {
            var (groupNum, ids) = SCCIDs(graph);
            var counts = new int[groupNum];

            foreach (var x in ids)
            {
                counts[x]++;
            }

            var groups = new List<List<int>>(groupNum);

            for (int i = 0; i < groupNum; i++)
            {
                groups.Add(new List<int>(counts[i]));
            }

            for (int i = 0; i < ids.Length; i++)
            {
                groups[ids[i]].Add(i);
            }

            return groups;
        }
    }
}
