using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 

namespace AtCoderProject.Hide.有向
{
    class GraphBuilder
    {
        private List<int>[] roots;
        private List<int>[] children;
        public GraphBuilder(int count)
        {
            this.roots = new List<int>[count];
            this.children = new List<int>[count];
            for (int i = 0; i < count; i++)
            {
                this.roots[i] = new List<int>();
                this.children[i] = new List<int>();
            }
        }
        public GraphBuilder(int count, ConsoleReader cr, int edgeCount) : this(count)
        {
            for (int i = 0; i < edgeCount; i++)
                this.Add(cr.Int - 1, cr.Int - 1);
        }
        public void Add(int from, int to)
        {
            children[from].Add(to);
            roots[to].Add(from);
        }
        public Node[] ToArray() =>
            Enumerable
            .Zip(roots, children, (r, c) => Tuple.Create(r, c))
            .Select((t, i) => new Node(i, t.Item1.ToArray(), t.Item2.ToArray()))
            .ToArray();
    }
    public class Node : IEquatable<Node>
    {
        public Node(int i, int[] roots, int[] children)
        {
            this.index = i;
            this.roots = roots;
            this.children = children;
        }
        public int index;
        public int[] roots;
        public int[] children;

        public override string ToString() => $"children: {string.Join(",", children)}";
        public override bool Equals(object obj)
        {
            if (obj is Node)
                return this.Equals((Node)obj);
            else
                return false;
        }
        public bool Equals(Node other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }

    class GraphSearch
    {
        enum Status
        {
            None,
            Active,
            Done
        }
        Node[] graph;
        Status[] statuses;
        public GraphSearch(Node[] graph)
        {
            this.graph = graph;
            this.statuses = new Status[graph.Length];
        }
        public int[] GetCycle()
        {
            for (int i = 0; i < graph.Length; i++)
            {
                if (statuses[i] == Status.None)
                {
                    var res = GetCycle(i);
                    if (res != null)
                    {
                        res.Reverse();
                        return res.ToArray();
                    }
                }
            }
            return null;
        }
        List<int> GetCycle(int v)
        {
            statuses[v] = Status.Active;

            foreach (var child in graph[v].children)
            {
                switch (statuses[child])
                {
                    case Status.None:
                        var list = GetCycle(child);
                        if (list != null)
                        {
                            if (list.Count < 2 || list[0] != list[list.Count - 1])
                                list.Add(v);
                            return list;
                        }
                        break;
                    case Status.Active:
                        return new List<int> { child, v };
                }
            }

            statuses[v] = Status.Done;
            return null;
        }
    }
}

namespace AtCoderProject.Hide.有向Length
{
    class GraphBuilder
    {
        private List<Next>[] roots;
        private List<Next>[] children;
        public GraphBuilder(int count)
        {
            this.roots = new List<Next>[count];
            this.children = new List<Next>[count];
            for (int i = 0; i < count; i++)
            {
                this.roots[i] = new List<Next>();
                this.children[i] = new List<Next>();
            }
        }
        public GraphBuilder(int count, ConsoleReader cr, int edgeCount) : this(count)
        {
            for (int i = 0; i < edgeCount; i++)
                this.Add(cr.Int - 1, cr.Int - 1, cr.Int);
        }
        public void Add(int from, int to, int length)
        {
            children[from].Add(new Next { to = to, length = length });
            roots[to].Add(new Next { to = from, length = length });
        }
        public Node[] ToArray() =>
            Enumerable
            .Zip(roots, children, (r, c) => Tuple.Create(r, c))
            .Select((t, i) => new Node(i, t.Item1.ToArray(), t.Item2.ToArray()))
            .ToArray();


        public static long[,] WarshallFloyd(Node[] graph)
        {
            var res = new long[graph.Length, graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    res[i, j] = long.MaxValue / 2;
                }
                res[i, i] = 0;
                foreach (var next in graph[i].children)
                    res[i, next.to] = next.length;
            }
            for (int k = 0; k < graph.Length; k++)
                for (int i = 0; i < graph.Length; i++)
                    for (int j = 0; j < graph.Length; j++)
                        if (res[i, j] > res[i, k] + res[k, j])
                            res[i, j] = res[i, k] + res[k, j];
            return res;
        }
        public static long[] Dijkstra(Node[] graph, int start)
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
                foreach (var next in graph[minIndex].children)
                {
                    var nextLength = min + next.length;
                    if (res[next.to] > nextLength)
                        res[next.to] = nextLength;
                }
            }
            return res;
        }
    }
    public struct Next
    {
        public int to;
        public int length;
        public override string ToString() => $"to: {to} length:{length}";
    }
    public class Node : IEquatable<Node>
    {
        public Node(int i, Next[] roots, Next[] children)
        {
            this.index = i;
            this.roots = roots;
            this.children = children;
        }
        public int index;
        public Next[] roots;
        public Next[] children;

        public override bool Equals(object obj)
        {
            if (obj is Node)
                return this.Equals((Node)obj);
            else
                return false;
        }
        public bool Equals(Node other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }
}
