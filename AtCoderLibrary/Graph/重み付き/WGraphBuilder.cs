using AtCoder.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AtCoder.Graph
{
    public class WIntGraphBuilder : WGraphBuilder<int, IntOperator>
    {
        public WIntGraphBuilder(int count, bool isOriented) : base(count, isOriented) { }
        public static WIntGraphBuilder Create(int count, ConsoleReader cr, int edgeCount, bool isOriented)
        {
            var gb = new WIntGraphBuilder(count, isOriented);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Int);
            return gb;
        }
    }
    public class WLongGraphBuilder : WGraphBuilder<long, LongOperator>
    {
        public WLongGraphBuilder(int count, bool isOriented) : base(count, isOriented) { }
        public static WLongGraphBuilder Create(int count, ConsoleReader cr, int edgeCount, bool isOriented)
        {
            var gb = new WLongGraphBuilder(count, isOriented);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Long);
            return gb;
        }
    }
    public class WGraphBuilder<T, TOp>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
    {
        protected static readonly TOp op = default;
        readonly List<Next<T>>[] roots;
        readonly List<Next<T>>[] children;
        public WGraphBuilder(int count, bool isOriented)
        {
            this.roots = new List<Next<T>>[count];
            this.children = new List<Next<T>>[count];
            for (var i = 0; i < count; i++)
            {
                if (isOriented)
                {
                    this.roots[i] = new List<Next<T>>();
                    this.children[i] = new List<Next<T>>();
                }
                else
                {
                    this.roots[i] = this.children[i] = new List<Next<T>>();
                }
            }
        }

        public void Add(int from, int to, T value)
        {
            children[from].Add(new Next<T> { to = to, value = value });
            roots[to].Add(new Next<T> { to = from, value = value });
        }
        public WNode<T>[] ToArray()
        {
            Debug.Assert(roots.Length == children.Length);
            var res = new WNode<T>[roots.Length];
            for (int i = 0; i < res.Length; i++)
            {
                if (roots[i] == children[i])
                    res[i] = new WNode<T>(i, children[i].ToArray());
                else
                    res[i] = new WNode<T>(i, roots[i].ToArray(), children[i].ToArray());
            }
            return res;
        }

        public WTreeNode<T>[] ToTree(int root = 0)
        {
            if (this.roots[0] != this.children[0]) throw new Exception("木には無向グラフをしたほうが良い");
            var res = new WTreeNode<T>[this.children.Length];
            res[root] = new WTreeNode<T>(root, -1, 0, default, this.children[root].ToArray());

            var queue = new Queue<int>();
            foreach (var child in res[root].children)
            {
                res[child.to] = new WTreeNode<T>(child.to, root, 1, child.value, Array.Empty<Next<T>>());
                queue.Enqueue(child.to);
            }

            while (queue.Count > 0)
            {
                var from = queue.Dequeue();
                if (res[from].root == -1)
                    res[from].children = this.children[from].ToArray();
                else
                {
                    var children = new List<Next<T>>(this.children[from].Count);
                    foreach (var c in this.children[from])
                        if (c.to != res[from].root)
                            children.Add(c);

                    res[from].children = children.ToArray();
                }

                foreach (var child in res[from].children)
                {
                    res[child.to] = new WTreeNode<T>(child.to, from, res[from].depth + 1, op.Add(res[from].depthLength, child.value), Array.Empty<Next<T>>());
                    queue.Enqueue(child.to);
                }
            }

            return res;
        }
        public WGraphBuilder<T, TOp> Clone()
        {
            var count = this.roots.Length;
            var isOriented = this.roots[0] != this.children[0];
            var cl = new WGraphBuilder<T, TOp>(count, isOriented);
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

    /// <summary>
    /// (to, value)
    /// </summary>
    public struct Next<T>
    {
        public int to;
        public T value;
        public void Deconstruct(out int to, out T value)
        {
            to = this.to;
            value = this.value;
        }
        public override string ToString() => $"to: {to} value:{value}";
    }
    public class WTreeNode<T>
    {
        public WTreeNode(int i, int root, int depth, T depthLength, Next<T>[] children)
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
        public readonly T depthLength;
        public Next<T>[] children;

        public override string ToString() => $"children: {string.Join(",", children)}";
        public override bool Equals(object obj) => obj is WTreeNode<T> d && this.Equals(d);
        public bool Equals(WTreeNode<T> other) => this.index == other.index;
        public override int GetHashCode() => this.index;
    }
    public class WNode<T>
    {
        public WNode(int i, Next<T>[] children)
        {
            this.index = i;
            this.roots = this.children = children;
        }
        public WNode(int i, Next<T>[] roots, Next<T>[] children)
        {
            this.index = i;
            this.roots = roots;
            this.children = children;
        }
        public int index;
        public Next<T>[] roots;
        public Next<T>[] children;
        public bool IsDirected => roots != children;
        public override bool Equals(object obj) => obj is WNode<T> d && this.Equals(d);
        public bool Equals(WNode<T> other) => this.index == other.index;
        public override int GetHashCode() => this.index;
        public override string ToString() => $"children: ({string.Join("),(", children)})";
    }
}