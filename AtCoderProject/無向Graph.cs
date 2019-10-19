using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 

namespace AtCoderProject.Hide.無向
{
    class GraphBuilder
    {
        public List<int>[] lists;
        public GraphBuilder(int count)
        {
            this.lists = new List<int>[count];
            for (int i = 0; i < count; i++)
                this.lists[i] = new List<int>();
        }
        public GraphBuilder(int count, ConsoleReader cr, int edgeCount) : this(count)
        {
            for (int i = 0; i < edgeCount; i++)
                this.Add(cr.Int - 1, cr.Int - 1);
        }
        public void Add(int from, int to)
        {
            this.lists[from].Add(to);
            this.lists[to].Add(from); // 有向の場合は消す
        }
        public int[][] ToArray()
        {
            var N = this.lists.Length;
            var res = new int[N][];
            for (int i = 0; i < N; i++)
                res[i] = this.lists[i].ToArray();
            return res;
        }
    }

    struct Next
    {
        public int to;
        public int length;
        public override string ToString() => $"to: {to} length:{length}";
    }
    class LengthGraphBuilder
    {
        public List<Next>[] lists;
        public LengthGraphBuilder(int count)
        {
            this.lists = new List<Next>[count];
            for (int i = 0; i < count; i++)
                this.lists[i] = new List<Next>();
        }
        public LengthGraphBuilder(int count, ConsoleReader cr, int edgeCount) : this(count)
        {
            for (int i = 0; i < edgeCount; i++)
                this.Add(cr.Int - 1, cr.Int - 1, cr.Int);
        }
        public void Add(int from, int to, int length)
        {
            this.lists[from].Add(new Next { to = to, length = length });
            this.lists[to].Add(new Next { to = from, length = length }); // 有向の場合は消す
        }
        public Next[][] ToArray()
        {
            var N = this.lists.Length;
            var res = new Next[N][];
            for (int i = 0; i < N; i++)
                res[i] = this.lists[i].ToArray();
            return res;
        }
        public static long[,] WarshallFloyd(Next[][] graph)
        {
            var res = new long[graph.Length, graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    res[i, j] = long.MaxValue / 2;
                }
                res[i, i] = 0;
                foreach (var next in graph[i])
                    res[i, next.to] = next.length;
            }
            for (int k = 0; k < graph.Length; k++)
                for (int i = 0; i < graph.Length; i++)
                    for (int j = 0; j < graph.Length; j++)
                        if (res[i, j] > res[i, k] + res[k, j])
                            res[i, j] = res[i, k] + res[k, j];
            return res;
        }
        public static long[] Dijkstra(Next[][] graph, int start)
        {
            var res = new long[graph.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = int.MaxValue;
            res[start] = 0;
            var remains = new HashSet<int>(Enumerable.Range(0, graph.Length));
            while (remains.Count > 0)
            {
                int minIndex = -1;
                long min = long.MaxValue / 2;
                foreach (var r in remains)
                {
                    if (min > res[r])
                    {
                        minIndex = r;
                        min = res[r];
                    }
                }
                remains.Remove(minIndex);
                foreach (var next in graph[minIndex])
                {
                    var nextLength = min + next.length;
                    if (res[next.to] > nextLength)
                        res[next.to] = nextLength;
                }
            }
            return res;
        }
    }

    class GraphSample
    {
        int[][] graph;
        int[] root;
        void SolveRoot(int val)
        {
            if (root[val] == val)
                SolveRoot(val, graph[val]);
        }
        void SolveRoot(int val, int[] target)
        {
            foreach (var i in target)
            {
                if (root[i] != val)
                {
                    root[i] = val;
                    SolveRoot(val, graph[i]);
                }
            }
        }
    }
}
