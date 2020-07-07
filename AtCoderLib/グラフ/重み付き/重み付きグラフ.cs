using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Linq;

class WGraphBuilder
{
    private List<Next>[] roots;
    private List<Next>[] children;
    public WGraphBuilder(int count, bool isOriented)
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
    public WGraphBuilder(int count, ConsoleReader cr, int edgeCount, bool isOriented) : this(count, isOriented)
    {
        for (var i = 0; i < edgeCount; i++)
            this.Add(cr.Int0, cr.Int0, cr.Int);
    }
    public void Add(int from, int to, int value)
    {
        children[from].Add(new Next { to = to, value = value });
        roots[to].Add(new Next { to = from, value = value });
    }
    public WNode[] ToArray() =>
        Enumerable
        .Zip(roots, children, (root, child) => (root, child))
        .Select((t, i) => new WNode(i, t.root.ToArray(), t.child.ToArray()))
        .ToArray();
    public WTreeNode[] ToTree(int root)
    {
        if (this.roots[0] != this.children[0]) throw new Exception("木には無向グラフをしたほうが良い");
        var res = new WTreeNode[this.children.Length];
        res[root] = new WTreeNode(root, -1, 0, 0, this.children[root].ToArray());

        var queue = new Queue<int>();
        foreach (var child in res[root].children)
        {
            res[child.to] = new WTreeNode(child.to, root, 1, child.value, Array.Empty<Next>());
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
                res[child.to] = new WTreeNode(child.to, from, res[from].depth + 1, res[from].depthLength + child.value, Array.Empty<Next>());
                queue.Enqueue(child.to);
            }
        }

        return res;
    }
    public WGraphBuilder Clone()
    {
        var count = this.roots.Length;
        var isOriented = this.roots[0] != this.children[0];
        var cl = new WGraphBuilder(count, isOriented);
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
struct Next
{
    public int to;
    public int value;
    public override string ToString() => $"to: {to} value:{value}";
}
class WTreeNode
{
    public WTreeNode(int i, int root, int depth, long depthLength, Next[] children)
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
        if (obj is WTreeNode)
            return this.Equals((WTreeNode)obj);
        else
            return false;
    }
    public bool Equals(WTreeNode other) => this.index == other.index;
    public override int GetHashCode() => this.index;
}
class WNode
{
    public WNode(int i, Next[] roots, Next[] children)
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
        if (obj is WNode)
            return this.Equals((WNode)obj);
        else
            return false;
    }
    public bool Equals(WNode other) => this.index == other.index;
    public override int GetHashCode() => this.index;
    public override string ToString() => $"children: ({string.Join("),(", children)})";
}
