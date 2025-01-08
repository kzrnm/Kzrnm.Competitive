using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive
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
        public static (int groupNum, int[] ids) SccIds<TEdge>(this IGraph<TEdge> graph)
            where TEdge : IGraphEdge
        {
            // R. Tarjan のアルゴリズム
            var graphArr = graph.AsArray();
            var g = graph.Edges;
            int nowOrd = 0;
            int groupNum = 0;
            var visited = new Stack<int>(graphArr.Length);
            var low = new int[graphArr.Length];
            var ids = new int[graphArr.Length];
            var ord = new int[graphArr.Length];
            ord.AsSpan().Fill(-1);

            for (int i = 0; i < ord.Length; i++)
            {
                if (ord[i] == -1)
                {
                    Dfs(i);
                }
            }

            for (int i = 0; i < ids.Length; i++)
            {
                // トポロジカル順序にするには逆順にする必要がある。
                ids[i] = groupNum - 1 - ids[i];
            }

            return (groupNum, ids);

            void Dfs(int v)
            {
                var stack = new Stack<(int v, int childIndex, bool childOk)>();
                stack.Push((v, g.Start[v], false));
                while (stack.TryPop(out var z))
                {
                    int ci;
                    bool childOk;
                    (v, ci, childOk) = z;

                    if (!childOk && ci == g.Start[v])
                    {
                        low[v] = nowOrd;
                        ord[v] = nowOrd++;
                        visited.Push(v);
                    }
                Loop:
                    if (ci < g.Start[v + 1])
                    {
                        // 頂点 v から伸びる有向辺を探索する。
                        int to = g.EList[ci].To;
                        if (childOk)
                        {
                            low[v] = Math.Min(low[v], low[to]);
                        }
                        else if (ord[to] == -1)
                        {
                            stack.Push((v, ci, true));
                            stack.Push((to, g.Start[to], false));
                            continue;
                        }
                        else
                        {
                            low[v] = Math.Min(low[v], ord[to]);
                        }
                        ++ci;
                        childOk = false;
                        goto Loop;
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
        public static int[][] Scc<TEdge>(this IGraph<TEdge> graph)
            where TEdge : IGraphEdge
            => SccGroupsAndIds(graph).Groups;

        /// <summary>
        /// 強連結成分ごとに ID を割り振り、各頂点の所属する強連結成分の ID が記録された配列と「頂点のリスト」のリストを取得します。
        /// </summary>
        /// <remarks>
        /// <para>- 全ての頂点がちょうど1つずつ、どれかのリストに含まれます。</para>
        /// <para>- 内側のリストと強連結成分が一対一に対応します。リスト内での頂点の順序は未定義です。</para>
        /// <para>- リストはトポロジカルソートされています。異なる強連結成分の頂点 u, v について、u から v に到達できる時、u の属するリストは v の属するリストよりも前です。</para>
        /// <para>計算量: 追加された辺の本数を m として O(n+m)</para>
        /// </remarks>
        public static (int[][] Groups, int[] Ids) SccGroupsAndIds<TEdge>(this IGraph<TEdge> graph)
            where TEdge : IGraphEdge
        {
            var (groupNum, ids) = SccIds(graph);
            var groups = new int[groupNum][];
            var counts = new int[groupNum];
            var seen = new int[groupNum];

            foreach (var x in ids)
                counts[x]++;

            for (int i = 0; i < groupNum; i++)
                groups[i] = new int[counts[i]];

            for (int i = 0; i < ids.Length; i++)
                groups[ids[i]][seen[ids[i]]++] = i;

            return (groups, ids);
        }

        /// <summary>
        /// 強連結成分ごとに ID を割り振り、各頂点の所属する強連結成分の ID を元に圧縮したグラフを構築します。
        /// </summary>
        /// <remarks>
        /// <para>- 全ての頂点がちょうど1つずつ、どれかのリストに含まれます。</para>
        /// <para>- 内側のリストと強連結成分が一対一に対応します。リスト内での頂点の順序は未定義です。</para>
        /// <para>- リストはトポロジカルソートされています。異なる強連結成分の頂点 u, v について、u から v に到達できる時、u の属するリストは v の属するリストよりも前です。</para>
        /// <para>- 構築されたグラフの各ノードは元のグラフのノードを保持します。</para>
        /// <para>計算量: 追加された辺の本数を m として O(n+m)</para>
        /// </remarks>
        /// <typeparam name="TEdge">辺の型</typeparam>
        /// <param name="graph">元になるグラフ</param>
        /// <param name="isDirected">新しく作るグラフを有向辺にするか。木にするなら <see langword="false"/> </param>
        /// <returns></returns>
        public static DataNodeGraphBuilder<int[]> SccNewGraph<TEdge>(this IGraph<TEdge> graph, bool isDirected = true)
            where TEdge : IGraphEdge
        {
            var (groups, ids) = SccGroupsAndIds(graph);
            DataNodeGraphBuilder<int[]> g = new(groups, isDirected);
            var ss = new SortedSet<(int, int)>();
            foreach (var (f, e) in graph.Edges)
            {
                var u = ids[f];
                var v = ids[e.To];
                if (u != v && ss.Add((u, v)))
                {
                    g.Add(u, v);
                }
            }

            return g;
        }
    }
}
