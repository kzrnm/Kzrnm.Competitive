using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder.Graph
{
    public static class 強連結成分分解
    {
        /// <summary>
        /// 強連結成分分解する
        /// </summary>
        /// <returns>各ノードの強連結成分分解されたグループ番号</returns>
        public static int[] Scc<TEdge>(this INode<TEdge>[] graph)
            where TEdge : IEdge
        {
            int[] Dfs1()
            {
                var cix = new int[graph.Length];
                var jun = new int[graph.Length];
                var cur = graph.Length;
                var idx = new Stack<int>(graph.Length);

                for (int i = 0; i < graph.Length; i++)
                {
                    if (cix[i] > graph[i].Children.Length) continue;
                    idx.Push(i);
                    while (idx.Count > 0)
                    {
                        var index = idx.Pop();
                        var children = graph[index].Children;
                        ref var ci = ref cix[index];
                        if (ci < children.Length)
                        {
                            var to = children[ci++].To;
                            idx.Push(index);
                            idx.Push(to);
                        }
                        else if (ci == children.Length)
                        {
                            ci++;
                            jun[--cur] = index;
                        }
                    }
                }
                return jun;
            }

            var jun = Dfs1();

            var res = Global.NewArray(graph.Length, -1);
            var idx = new Stack<int>(graph.Length);
            var g = 0;
            foreach (var i in jun)
            {
                if (res[i] >= 0)
                    continue;
                idx.Push(i);
                while (idx.Count > 0)
                {
                    int index = idx.Pop();
                    if (res[index] >= 0)
                        continue;
                    res[index] = g;
                    foreach (var r in graph[index].Roots)
                        idx.Push(r.To);
                }
                ++g;
            }
            return res;
        }
    }
}
