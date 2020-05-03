using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Linq;
using static AtCoderProject.Global;

class ShortestPath
{
    long[][] WarshallFloyd(WNode[] graph)
    {
        var res = NewArray(graph.Length, graph.Length, 0L);
        for (var i = 0; i < graph.Length; i++)
        {
            for (var j = 0; j < graph.Length; j++)
            {
                res[i][j] = long.MaxValue / 2;
            }
            res[i][i] = 0;
            foreach (var next in graph[i].children)
                res[i][next.to] = next.value;
        }
        for (var k = 0; k < graph.Length; k++)
            for (var i = 0; i < graph.Length; i++)
                for (var j = 0; j < graph.Length; j++)
                    if (res[i][j] > res[i][k] + res[k][j])
                        res[i][j] = res[i][k] + res[k][j];
        return res;
    }
    long[] Dijkstra(WNode[] graph, int start)
    {
        var res = NewArray(graph.Length, long.MaxValue / 2);
        res[start] = 0;

        var used = new bool[graph.Length];
        int count = 0;
        var remains = new PriorityQueue<long, int>();
        remains.Add(0, start);

        while (remains.Count > 0)
        {
            var first = remains.Dequeue();
            if (used[first.Value]) continue;
            used[first.Value] = true;
            if (++count >= graph.Length) break;
            foreach (var next in graph[first.Value].children)
            {
                var nextLength = first.Key + next.value;
                if (res[next.to] > nextLength)
                    remains.Add(res[next.to] = nextLength, next.to);
            }
        }
        return res;
    }
    long[] BellmanFord(WNode[] graph, int start)
    {
        var res = NewArray(graph.Length, long.MaxValue / 2);
        res[start] = 0;

        for (int i = 1; i <= graph.Length; i++)
            foreach (var node in graph)
                foreach (var next in node.children)
                    if (res[next.to].UpdateMin(res[node.index] + next.value))
                        if (i == graph.Length)
                            throw new InvalidOperationException("負の閉路");
        return res;
    }
}
