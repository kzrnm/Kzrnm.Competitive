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
        public GraphBuilder(int count, IEnumerable<int[]> arrays) : this(count)
        {
            foreach (var item in arrays)
                this.Add(item[0], item[1]);
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
        public GraphBuilder(int count, IEnumerable<int[]> arrays) : this(count)
        {
            foreach (var item in arrays)
                this.Add(item[0], item[1], item[2]);
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
    }
    public struct Next
    {
        public int to;
        public int length;
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
