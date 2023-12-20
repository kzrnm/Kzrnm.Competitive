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
            var ord = Enumerable.Repeat(-1, graphArr.Length).ToArray();
            var ids = new int[graphArr.Length];

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
                while (stack.Count > 0)
                {
                    int ci;
                    bool childOk;
                    (v, ci, childOk) = stack.Pop();

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
            => SccGroupsAndIds(graph).groups;

        /// <summary>
        /// 強連結成分ごとに ID を割り振り、各頂点の所属する強連結成分の ID が記録された配列と「頂点のリスト」のリストを取得します。
        /// </summary>
        /// <remarks>
        /// <para>- 全ての頂点がちょうど1つずつ、どれかのリストに含まれます。</para>
        /// <para>- 内側のリストと強連結成分が一対一に対応します。リスト内での頂点の順序は未定義です。</para>
        /// <para>- リストはトポロジカルソートされています。異なる強連結成分の頂点 u, v について、u から v に到達できる時、u の属するリストは v の属するリストよりも前です。</para>
        /// <para>計算量: 追加された辺の本数を m として O(n+m)</para>
        /// </remarks>
        public static (int[][] groups, int[] ids) SccGroupsAndIds<TEdge>(this IGraph<TEdge> graph)
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
    }
}
