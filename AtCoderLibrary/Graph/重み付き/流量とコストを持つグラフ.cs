using AtCoder;
using AtCoder.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static AtCoder.Global;


namespace AtCoder.Graph
{
    public class WGraphBuilderD
    {
        List<NextD>[] roots;
        List<NextD>[] children;
        public WGraphBuilderD(int count, bool isOriented)
        {
            this.roots = new List<NextD>[count];
            this.children = new List<NextD>[count];
            for (var i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    this.roots[i] = new List<NextD>();
                    this.children[i] = new List<NextD>();
                }
                else
                {
                    this.roots[i] = this.children[i] = new List<NextD>();
                }
            }
        }
        public WGraphBuilderD(int count, ConsoleReader cr, int edgeCount, bool isOriented) : this(count, isOriented)
        {
            for (var i = 0; i < edgeCount; i++)
                this.Add(cr.Int0, cr.Int0, cr.Int, cr.Int);
        }
        public static WTreeNodeD[] MakeTree(int count, ConsoleReader cr, int root = 0)
            => new WGraphBuilderD(count, cr, count - 1, false).ToTree(root);
        public void Add(int from, int to, int capacity, long cost)
        {
            children[from].Add(new NextD { to = to, capacity = capacity, cost = cost });
            roots[to].Add(new NextD { to = from, capacity = capacity, cost = cost });
        }
        public WNodeD[] ToArray()
        {
            Debug.Assert(roots.Length == children.Length);
            var res = new WNodeD[roots.Length];
            for (int i = 0; i < res.Length; i++)
            {
                if (roots[i] == children[i])
                    res[i] = new WNodeD(i, children[i].ToArray());
                else
                    res[i] = new WNodeD(i, roots[i].ToArray(), children[i].ToArray());
            }
            return res;
        }
        public WTreeNodeD[] ToTree(int root = 0)
        {
            if (this.roots[0] != this.children[0]) throw new Exception("木には無向グラフをしたほうが良い");
            var res = new WTreeNodeD[this.children.Length];
            res[root] = new WTreeNodeD(root, -1, 0, 0, this.children[root].ToArray());

            var queue = new Queue<int>();
            foreach (var child in res[root].children)
            {
                res[child.to] = new WTreeNodeD(child.to, root, 1, child.cost, Array.Empty<NextD>());
                queue.Enqueue(child.to);
            }

            while (queue.Count > 0)
            {
                var from = queue.Dequeue();
                if (res[from].root == -1)
                    res[from].children = this.children[from].ToArray();
                else
                {
                    var children = new List<NextD>(this.children[from].Count);
                    foreach (var c in this.children[from])
                        if (c.to != res[from].root)
                            children.Add(c);

                    res[from].children = children.ToArray();
                }

                foreach (var child in res[from].children)
                {
                    res[child.to] = new WTreeNodeD(child.to, from, res[from].depth + 1, res[from].depthLength + child.cost, Array.Empty<NextD>());
                    queue.Enqueue(child.to);
                }
            }

            return res;
        }
        public WGraphBuilderD Clone()
        {
            var count = this.roots.Length;
            var isOriented = this.roots[0] != this.children[0];
            var cl = new WGraphBuilderD(count, isOriented);
            for (int i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    cl.children[i] = this.children[i].ToList();
                    cl.roots[i] = this.roots[i].ToList();
                }
                else
                    cl.children[i] = cl.roots[i] = this.roots[i].ToList();
            }
            return cl;
        }
    }
    public struct NextD
    {
        public int to;
        public int capacity;
        public long cost;
        public void Deconstruct(out int to, out int capacity, out long cost)
        {
            to = this.to;
            capacity = this.capacity;
            cost = this.cost;
        }
        public override string ToString() => $"to: {to} value:{capacity}/{cost}";
    }
    public class WTreeNodeD
    {
        public WTreeNodeD(int i, int root, int depth, long depthLength, NextD[] children)
        {
            this.index = i;
            this.root = root;
            this.children = children;
            this.depth = depth;
            this.depthLength = depthLength;
        }
        public readonly int index;
        public readonly int root;
        public readonly int depth;
        public readonly long depthLength;
        public NextD[] children;

        public override string ToString() => $"children: {string.Join(",", children)}";
        public override bool Equals(object obj) => obj is WTreeNodeD d && this.Equals(d);
        public bool Equals(WTreeNodeD other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }
    public class WNodeD
    {
        public WNodeD(int i, NextD[] children)
        {
            this.index = i;
            this.roots = this.children = children;
        }
        public WNodeD(int i, NextD[] roots, NextD[] children)
        {
            this.index = i;
            this.roots = roots;
            this.children = children;
        }
        public int index;
        public NextD[] roots;
        public NextD[] children;
        public bool IsDirected => roots != children;

        public override bool Equals(object obj) => obj is WNodeD d && this.Equals(d);

        public bool Equals(WNodeD other) => this.index == other.index;
        public override int GetHashCode() => this.index;
        public override string ToString() => $"children: ({string.Join("),(", children)})";
    }
    class MinCostFlow
    {
        const long INF = long.MaxValue >> 3;
        Node[][] graph;
        public MinCostFlow(WNodeD[] orig)
        {
            var gb = NewArray(orig.Length, () => new List<Node>());
            for (int i = 0; i < orig.Length; i++)
            {
                foreach (var next in orig[i].children)
                {
                    gb[i].Add(new Node(next.to, next.capacity, next.cost, gb[next.to].Count, false));
                    gb[next.to].Add(new Node(i, 0, -next.cost, gb[i].Count - 1, true));
                }
            }
            graph = gb.Select(l => l.ToArray()).ToArray();
        }
        class Node
        {
            public Node(NextD next, int rev, bool isReverse)
                : this(next.to, next.capacity, next.cost, rev, isReverse) { }
            public Node(int to, int capacity, long cost, int rev, bool isReverse)
            {
                this.to = to;
                this.capacity = capacity;
                this.cost = cost;
                this.rev = rev;
                this.isReverse = isReverse;
            }

            public int to;
            public int capacity;
            public long cost;
            public int rev;
            public bool isReverse;
            public override string ToString() => $"to: {to} value:{capacity}/{cost}";
        }
        public long Calc(int from, int to, int flow)
        {
            long res = 0;
            var pq = new PriorityQueue<long, int>(graph.Length);
            var potential = new long[graph.Length];
            var preve = NewArray(graph.Length, -1);
            var prevv = NewArray(graph.Length, -1);
            while (flow > 0)
            {
                // ダイクストラを繰り返す
                var minCosts = NewArray(graph.Length, INF);
                pq.Add(0, from);
                minCosts[from] = 0;
                while (pq.Count > 0)
                {
                    var (cost, v) = pq.Dequeue();
                    if (minCosts[v] < cost) continue;
                    for (int i = 0; i < graph[v].Length; i++)
                    {
                        var next = graph[v][i];
                        var nxCost = minCosts[v] + next.cost + potential[v] - potential[next.to];

                        if (next.capacity > 0 && minCosts[next.to].UpdateMin(nxCost))
                        {
                            prevv[next.to] = v;
                            preve[next.to] = i;
                            pq.Add(minCosts[next.to], next.to);
                        }
                    }
                }
                if (minCosts[to] == INF) return -1;

                for (int v = 0; v < potential.Length; v++) potential[v] += minCosts[v];

                var addflow = flow;
                for (int v = to; v != from; v = prevv[v])
                    addflow.UpdateMin(graph[prevv[v]][preve[v]].capacity);
                flow -= addflow;
                res += addflow * potential[to];
                for (int v = to; v != from; v = prevv[v])
                {
                    var next = graph[prevv[v]][preve[v]];
                    next.capacity -= addflow;
                    graph[v][next.rev].capacity += addflow;
                }
            }
            return res;
        }
    }
}