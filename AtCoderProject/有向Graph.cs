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
        public Node[] nodes;
        public GraphBuilder(int count)
        {
            this.nodes = new Node[count];
            for (int i = 0; i < count; i++)
                this.nodes[i] = new Node(i);
        }
        public GraphBuilder(int count, IEnumerable<int[]> arrays) : this(count)
        {
            foreach (var item in arrays)
                this.Add(item[0], item[1]);
        }
        public void Add(int from, int to)
        {
            nodes[from].children.Add(nodes[to]);
            nodes[to].roots.Add(nodes[from]);
        }
    }

    public struct Node : IEquatable<Node>
    {
        public Node(int i)
        {
            this.index = i;
            this.roots = new HashSet<Node>();
            this.children = new HashSet<Node>();
        }
        public int index;
        public HashSet<Node> roots;
        public HashSet<Node> children;

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
