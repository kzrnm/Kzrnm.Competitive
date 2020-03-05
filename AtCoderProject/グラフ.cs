using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Global;

#pragma warning disable

namespace AtCoderProject.Hide
{
    class GraphBuilder
    {
        private List<int>[] roots;
        private List<int>[] children;
        public GraphBuilder(int count, bool isOriented = true)
        {
            this.roots = new List<int>[count];
            this.children = new List<int>[count];
            for (var i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    this.roots[i] = new List<int>();
                    this.children[i] = new List<int>();
                }
                else
                {
                    this.roots[i] = this.children[i] = new List<int>();
                }
            }
        }
        public GraphBuilder(int count, ConsoleReader cr, int edgeCount, bool isOriented = true) : this(count, isOriented)
        {
            for (var i = 0; i < edgeCount; i++)
                this.Add(cr.Int0, cr.Int0);
        }
        public void Add(int from, int to)
        {
            children[from].Add(to);
            roots[to].Add(from);
        }
        public TreeNode[] ToTree(int root)
        {
            if (this.roots[0] != this.children[0]) throw new Exception("木には無向グラフをしたほうが良い");
            var res = new TreeNode[this.children.Length];
            res[root] = new TreeNode(root, -1, 0, this.children[root].ToArray());

            var queue = new Queue<int>();
            foreach (var child in res[root].children)
            {
                res[child] = new TreeNode(child, root, 1, Array.Empty<int>());
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var from = queue.Dequeue();
                if (res[from].root == -1)
                    res[from].children = this.children[from].ToArray();
                else
                {
                    var children = new List<int>(this.children[from].Count);
                    foreach (var c in this.children[from])
                        if (c != res[from].root)
                            children.Add(c);

                    res[from].children = children.ToArray();
                }

                foreach (var child in res[from].children)
                {
                    res[child] = new TreeNode(child, from, res[from].depth + 1, Array.Empty<int>());
                    queue.Enqueue(child);
                }
            }

            return res;
        }

        public Node[] ToArray() =>
            Enumerable
            .Zip(roots, children, (r, c) => Tuple.Create(r, c))
            .Select((t, i) => new Node(i, t.Item1.ToArray(), t.Item2.ToArray()))
            .ToArray();
    }
    class TreeNode
    {
        public TreeNode(int i, int root, int depth, int[] children)
        {
            this.index = i;
            this.root = root;
            this.children = children;
            this.depth = depth;
        }
        public readonly int index;
        public readonly int root;
        public readonly int depth;
        public int[] children;

        public override string ToString() => $"children: {string.Join(",", children)}";
        public override bool Equals(object obj)
        {
            if (obj is TreeNode)
                return this.Equals((TreeNode)obj);
            else
                return false;
        }
        public bool Equals(TreeNode other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }
    class Node
    {
        public Node(int i, int[] roots, int[] children)
        {
            this.index = i;
            this.roots = roots;
            this.children = children;
        }
        public readonly int index;
        public readonly int[] roots;
        public readonly int[] children;

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

    class LowestCommonAncestor // 最小共通祖先
    {
        private TreeNode[] tree;

        // kprv[u][k] 頂点uの2^k個上の祖先頂点v, 0<=k<logN
        private int[][] kprv;
        private int logN;
        public LowestCommonAncestor(TreeNode[] tree)
        {
            if (tree.Length == 0) throw new ArgumentException(nameof(tree));

            this.tree = tree;
            this.logN = (int)(Math.Log(tree.Length, 2) + 1);
            this.kprv = NewArray(tree.Length, logN, 0);
            for (int v = 0; v < tree.Length; v++)
            {
                this.kprv[v][0] = tree[v].root;
            }
            for (int k = 0; k < logN - 1; k++)
            {
                for (int v = 0; v < tree.Length; v++)
                {
                    if (this.kprv[v][k] < 0)
                        this.kprv[v][k + 1] = -1;
                    else
                        this.kprv[v][k + 1] = this.kprv[this.kprv[v][k]][k];
                }
            }
        }

        public int Lca(int u, int v)
        {
            if (Depth(u) > Depth(v))
            {
                var tmp = u;
                u = v;
                v = tmp;
            }
            for (int k = 0; k <= logN; k++)
            {
                if ((((Depth(v) - Depth(u)) >> k) & 1) == 1)
                {
                    v = kprv[v][k];
                }
            }
            if (u == v)
                return u;

            for (int k = logN - 1; k >= 0; k--)
            {
                if (kprv[u][k] != kprv[v][k] && kprv[u][k] != -1 && kprv[v][k] != -1)
                {
                    u = kprv[u][k];
                    v = kprv[v][k];
                }
            }
            return kprv[u][0];
        }

        int Depth(int index) => tree[index].depth;
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
        public int[] GetCycleDFS()
        {
            for (var i = 0; i < graph.Length; i++)
            {
                if (statuses[i] == Status.None)
                {
                    var res = GetCycleDFS(i);
                    if (res != null)
                    {
                        res.Reverse();
                        return res.ToArray();
                    }
                }
            }
            return null;
        }
        List<int> GetCycleDFS(int v)
        {
            statuses[v] = Status.Active;

            foreach (var child in graph[v].children)
            {
                switch (statuses[child])
                {
                    case Status.None:
                        var list = GetCycleDFS(child);
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

        class BFSData
        {
            public readonly int[] current;
            public readonly bool[] used;
            public BFSData(int[] current, bool[] used)
            {
                this.current = current;
                this.used = used;
            }
        }
        public int[] GetCycleBFS()
        {
            for (var i = 0; i < graph.Length; i++)
            {
                if (statuses[i] == Status.None)
                {
                    var res = GetCycleBFS(i);
                    if (res != null)
                        return res;
                }
            }
            return null;
        }
        int[] GetCycleBFS(int v)
        {
            int[] res = null;
            statuses[v] = Status.Active;
            var queue = new Queue<BFSData>();
            var bfsd = new BFSData(new[] { v }, new bool[graph.Length]);
            bfsd.used[v] = true;
            queue.Enqueue(bfsd);
            while (queue.Count > 0)
            {
                bfsd = queue.Dequeue();
                foreach (var child in graph[bfsd.current[bfsd.current.Length - 1]].children)
                {
                    if (bfsd.used[child])
                    {
                        var index = Array.IndexOf(bfsd.current, child);
                        if (res == null || res.Length > bfsd.current.Length + 1 - index)
                        {
                            res = new int[bfsd.current.Length + 1 - index];
                            Array.Copy(bfsd.current, index, res, 0, bfsd.current.Length - index);
                            res[res.Length - 1] = child;
                        }
                    }
                    else if (res == null)
                    {
                        statuses[child] = Status.Done;
                        var next = new int[bfsd.current.Length + 1];
                        Array.Copy(bfsd.current, next, bfsd.current.Length);
                        next[next.Length - 1] = child;
                        var nextUsed = (bool[])bfsd.used.Clone();
                        nextUsed[child] = true;
                        queue.Enqueue(new BFSData(next, nextUsed));
                    }
                }
            }
            return res;
        }
    }
}

namespace AtCoderProject.Hide.Length
{
    class GraphBuilder
    {
        private List<Next>[] roots;
        private List<Next>[] children;
        public GraphBuilder(int count, bool isOriented = true)
        {
            this.roots = new List<Next>[count];
            this.children = new List<Next>[count];
            for (var i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    this.roots[i] = new List<Next>();
                    this.children[i] = new List<Next>();
                }
                else
                {
                    this.roots[i] = this.children[i] = new List<Next>();
                }
            }
        }
        public GraphBuilder(int count, ConsoleReader cr, int edgeCount, bool isOriented = true) : this(count, isOriented)
        {
            for (var i = 0; i < edgeCount; i++)
                this.Add(cr.Int0, cr.Int0, cr.Int);
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
        public TreeNode[] ToTree(int root)
        {
            if (this.roots[0] != this.children[0]) throw new Exception("木には無向グラフをしたほうが良い");
            var res = new TreeNode[this.children.Length];
            res[root] = new TreeNode(root, -1, 0, 0, this.children[root].ToArray());

            var queue = new Queue<int>();
            foreach (var child in res[root].children)
            {
                res[child.to] = new TreeNode(child.to, root, 1, child.length, Array.Empty<Next>());
                queue.Enqueue(child.to);
            }

            while (queue.Count > 0)
            {
                var from = queue.Dequeue();
                if (res[from].root == -1)
                    res[from].children = this.children[from].ToArray();
                else
                {
                    var children = new List<Next>(this.children[from].Count);
                    foreach (var c in this.children[from])
                        if (c.to != res[from].root)
                            children.Add(c);

                    res[from].children = children.ToArray();
                }

                foreach (var child in res[from].children)
                {
                    res[child.to] = new TreeNode(child.to, from, res[from].depth + 1, res[from].depthLength + child.length, Array.Empty<Next>());
                    queue.Enqueue(child.to);
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
    class TreeNode
    {
        public TreeNode(int i, int root, int depth, long depthLength, Next[] children)
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
        public Next[] children;

        public override string ToString() => $"children: {string.Join(",", children)}";
        public override bool Equals(object obj)
        {
            if (obj is TreeNode)
                return this.Equals((TreeNode)obj);
            else
                return false;
        }
        public bool Equals(TreeNode other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }
    public class Node
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
        public override string ToString() => $"children: ({string.Join("),(", children)})";
    }

    class ShortestPath
    {
        public static int[] BFS(Node[] graph, int from)
        {
            var res = NewArray(graph.Length, int.MaxValue);
            var queue = new Queue<int>();
            queue.Enqueue(from);
            res[from] = 0;
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var n in graph[cur].children.Select(node => node.to))
                {
                    var to = res[cur] + 1;
                    if (res[n] > to)
                    {
                        res[n] = to;
                        queue.Enqueue(n);
                    }
                }
            }
            return res;
        }
        public static long[][] WarshallFloyd(Node[] graph)
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
                    res[i][next.to] = next.length;
            }
            for (var k = 0; k < graph.Length; k++)
                for (var i = 0; i < graph.Length; i++)
                    for (var j = 0; j < graph.Length; j++)
                        if (res[i][j] > res[i][k] + res[k][j])
                            res[i][j] = res[i][k] + res[k][j];
            return res;
        }
        public static long[] Dijkstra(Node[] graph, int start)
        {
            var res = new long[graph.Length];
            for (var i = 0; i < res.Length; i++)
                res[i] = long.MaxValue / 2;
            res[start] = 0;

            var used = new bool[graph.Length];
            int count = 0;
            var remains = new PriorityQueue<long, int>();
            for (var i = 0; i < res.Length; i++)
                remains.Add(res[i], i);

            while (remains.Count > 0)
            {
                var first = remains.Dequeue();
                if (used[first.Value]) continue;
                used[first.Value] = true;
                if (++count >= graph.Length) break;
                foreach (var next in graph[first.Value].children)
                {
                    var nextLength = first.Key + next.length;
                    if (res[next.to] > nextLength)
                        remains.Add(res[next.to] = nextLength, next.to);
                }
            }
            return res;
        }
    }
}
