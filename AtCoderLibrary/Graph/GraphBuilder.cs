using AtCoder.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AtCoder.Graph
{
    public class GraphBuilder
    {
        List<int>[] roots;
        List<int>[] children;
        public GraphBuilder(int count, bool isOriented)
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

        public static GraphBuilder Create(int count, ConsoleReader cr, int edgeCount, bool isOriented)
        {
            var gb = new GraphBuilder(count, isOriented);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0);
            return gb;
        }

        public void Add(int from, int to)
        {
            children[from].Add(to);
            roots[to].Add(from);
        }
        public TreeNode[] ToTree(int root = 0)
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

        public Node[] ToArray()
        {
            Debug.Assert(roots.Length == children.Length);
            var res = new Node[roots.Length];
            for (int i = 0; i < res.Length; i++)
            {
                if (roots[i] == children[i])
                    res[i] = new Node(i, children[i].ToArray());
                else
                    res[i] = new Node(i, roots[i].ToArray(), children[i].ToArray());
            }
            return res;
        }
        public GraphBuilder Clone()
        {
            var count = this.roots.Length;
            var isOriented = this.roots[0] != this.children[0];
            var cl = new GraphBuilder(count, isOriented);
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
    public class TreeNode
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
        public override bool Equals(object obj) => obj is TreeNode node && this.Equals(node);

        public bool Equals(TreeNode other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }
    public class Node
    {
        public Node(int i, int[] children)
        {
            this.index = i;
            this.roots = this.children = children;
        }
        public Node(int i, int[] roots, int[] children)
        {
            this.index = i;
            this.roots = roots;
            this.children = children;
        }
        public readonly int index;
        public readonly int[] roots;
        public readonly int[] children;

        public bool IsDirected => roots != children;
        public override string ToString() => $"children: {string.Join(",", children)}";
        public override bool Equals(object obj) => obj is Node d && this.Equals(d);
        public bool Equals(Node other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }
}