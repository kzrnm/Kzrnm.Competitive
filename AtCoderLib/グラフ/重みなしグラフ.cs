using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Linq;


class GraphBuilder
{
    private List<int>[] roots;
    private List<int>[] children;
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
    public GraphBuilder(int count, ConsoleReader cr, int edgeCount, bool isOriented) : this(count, isOriented)
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
        .Zip(roots, children, (root, child) => (root, child))
        .Select((t, i) => new Node(i, t.root.ToArray(), t.child.ToArray()))
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
