// https://ei1333.github.io/luzhiled/snippets/structure/link-cut-tree.html
using AtCoder;
using AtCoder.Internal;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [IsOperator]
    public interface ILinkCutTreeOperator<T, F> : ISLazySegtreeOperator<T, F>
    {
        /// <summary>
        /// 要素の向きを反転する演算
        /// </summary>
        public T Inverse(T v);
    }
    public class LinkCutTree<T, F, TOp> where TOp : struct, ILinkCutTreeOperator<T, F>
    {
        private static TOp op = default;
        public class Node
        {
            public Node Left;
            public Node Right;
            public Node Parent;
            public int Index;

            public T Sum { get; internal set; }
            public T Key { get; internal set; }

            internal F lazy;

            internal bool rev;
            internal int size = 1;

            public bool IsRoot => Parent == null || (Parent.Left != this && Parent.Right != this);

            internal Node(int index) : this(index, op.Identity, op.FIdentity) { }
            internal Node(int index, T key) : this(index, key, op.FIdentity) { }
            internal Node(int index, T key, F lazy)
            {
                Index = index;
                Sum = Key = key;
                this.lazy = lazy;
            }
        }

        /// <summary>
        /// ID が <paramref name="idx"/>, 値に <paramref name="val"/> を入れたノードを新しく生成する。
        /// </summary>
        [凾(256)]
        public Node MakeNode(int idx, T val) => new Node(idx, val);


        [凾(256)]
        static void Propagate(Node t, F x)
        {
            t.lazy = op.Composition(x, t.lazy);
            t.Key = op.Mapping(x, t.Key, 1);
            t.Sum = op.Mapping(x, t.Sum, t.size);
        }
        [凾(256)]
        static void Toggle(Node t)
        {
            (t.Right, t.Left) = (t.Left, t.Right);
            t.Sum = op.Inverse(t.Sum);
            t.rev = !t.rev;
        }
        [凾(256)]
        static void Push(Node t)
        {
            if (t.Left != null) Propagate(t.Left, t.lazy);
            if (t.Right != null) Propagate(t.Right, t.lazy);
            t.lazy = op.FIdentity;

            if (t.rev)
            {
                if (t.Left != null) Toggle(t.Left);
                if (t.Right != null) Toggle(t.Right);
                t.rev = false;
            }
        }

        [凾(256)]
        static void Update(Node t)
        {
            t.size = 1;
            t.Sum = t.Key;
            if (t.Left != null)
            {
                t.size += t.Left.size;
                t.Sum = op.Operate(t.Left.Sum, t.Sum);
            }
            if (t.Right != null)
            {
                t.size += t.Right.size;
                t.Sum = op.Operate(t.Sum, t.Right.Sum);
            }
        }

        [凾(256)]
        static void RotationRight(Node t)
        {
            var x = t.Parent;
            var y = x?.Parent;

            x.Left = t.Right;
            if (t.Right != null) t.Right.Parent = x;
            t.Right = x;
            x.Parent = t;
            Update(x);
            Update(t);
            t.Parent = y;
            if (y != null)
            {
                if (y.Left == x) y.Left = t;
                if (y.Right == x) y.Right = t;
                Update(y);
            }
        }

        [凾(256)]
        static void RotationLeft(Node t)
        {
            var x = t.Parent;
            var y = x?.Parent;
            x.Right = t.Left;
            if (t.Left != null) t.Left.Parent = x;
            t.Left = x;
            x.Parent = t;
            Update(x);
            Update(t);
            t.Parent = y;
            if (y != null)
            {
                if (y.Left == x) y.Left = t;
                if (y.Right == x) y.Right = t;
                Update(y);
            }
        }
        [凾(256)]
        static void Splay(Node t)
        {
            Push(t);
            while (!t.IsRoot)
            {
                var q = t.Parent;
                if (!t.IsRoot)
                {
                    Push(q);
                    Push(t);
                    if (q.Left == t) RotationRight(t);
                    else RotationLeft(t);
                }
                else
                {
                    var r = q.Parent;
                    Push(r);
                    Push(q);
                    Push(t);
                    if (r.Left == q)
                    {
                        if (q.Left == t) { RotationRight(q); RotationRight(t); }
                        else { RotationLeft(t); RotationRight(t); }
                    }
                    else
                    {
                        if (q.Right == t) { RotationLeft(q); RotationLeft(t); }
                        else { RotationRight(t); RotationLeft(t); }
                    }
                }
            }
        }


        /// <summary>
        /// <paramref name="t"/> と根との間を Heavy-edge でつなげる。戻り値は t ではないので注意。
        /// </summary>
        [凾(256)]
        public Node Expose(Node t)
        {
            Node rp = null;
            for (Node cur = t; cur != null; cur = cur.Parent)
            {
                Splay(cur);
                cur.Right = rp;
                Update(cur);
                rp = cur;
            }
            Splay(t);
            return rp;
        }

        /// <summary>
        /// <para><paramref name="child"/> の親を <paramref name="parent"/> にする。</para>
        /// <para>制約: <paramref name="child"/> と <paramref name="parent"/>が非連結。</para>
        /// </summary>
        [凾(256)]
        public void Link(Node child, Node parent)
        {
            Contract.Assert(Lca(child, parent) == null);
            Evert(child);
            Expose(child);
            Expose(parent);
            child.Parent = parent;
            parent.Right = child;
            Update(parent);
        }

        /// <summary>
        /// <paramref name="child"/> の親と <paramref name="child"/> を切り離す。
        /// </summary>
        [凾(256)]
        public void Cut(Node child)
        {
            Expose(child);
            var parent = child.Left;
            child.Left = null;
            parent.Parent = null;
            Update(child);
        }

        /// <summary>
        /// <paramref name="t"/> を木の根にする。
        /// </summary>
        [凾(256)]
        public void Evert(Node t)
        {
            Expose(t);
            Toggle(t);
            Push(t);
        }

        /// <summary>
        /// <paramref name="u"/> と <paramref name="v"/> の最小共通祖先を返す。
        /// </summary>
        [凾(256)]
        public Node Lca(Node u, Node v)
        {
            if (GetRoot(u) != GetRoot(v)) return null;
            Expose(u);
            return Expose(v);
        }

        /// <summary>
        /// <paramref name="x"/> から根までのパスに出現する頂点を返す(O(n))。
        /// </summary>
        [凾(256)]
        public int[] GetPath(Node x)
        {
            var vs = new List<int>();
            Expose(x);
            var stack = new Stack<(Node cur, bool right)>();
            stack.Push((x, true));
            while (stack.Count > 0)
            {
                var (cur, right) = stack.Pop();
                if (cur == null) continue;
                if (right)
                {
                    Push(cur);
                    stack.Push((cur, false));
                    stack.Push((cur.Right, true));
                }
                else
                {
                    vs.Add(cur.Index);
                    stack.Push((cur.Left, true));
                }
            }
            return vs.ToArray();
        }

        /// <summary>
        /// 根からノード <paramref name="t"/> までのパスに作用素 <paramref name="x"/> を適用する。
        /// </summary>
        [凾(256)]
        public void SetPropagate(Node t, F x)
        {
            Expose(t);
            Propagate(t, x);
            Push(t);
        }

        /// <summary>
        /// <paramref name="x"/> から根までのパスに出現する頂点を並べたときの <paramref name="k"/> 番目の頂点を 0-indexed で返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        [凾(256)]
        public Node GetKth(Node x, int k)
        {
            Expose(x);
            while (x != null)
            {
                Push(x);
                if (x.Right != null && x.Right.size > k)
                {
                    x = x.Right;
                }
                else
                {
                    if (x.Right != null) k -= x.Right.size;
                    if (k == 0) return x;
                    k -= 1;
                    x = x.Left;
                }
            }
            return null;
        }

        /// <summary>
        /// <paramref name="x"/> の根を返す。
        /// </summary>
        [凾(256)]
        public Node GetRoot(Node x)
        {
            Expose(x);
            while (x.Left != null)
            {
                Push(x);
                x = x.Left;
            }
            return x;
        }
    }

    public class LinkCutTree : LinkCutTree<object, object, NullOperator>
    {
        /// <summary>
        /// ID が <paramref name="idx"/> のノードを新しく生成する。
        /// </summary>
        [凾(256)]
        public static Node MakeNode(int idx) => new Node(idx, null);
    }
    public readonly struct NullOperator : ILinkCutTreeOperator<object, object>
    {
        public object Identity => null;
        public object FIdentity => null;

        [凾(256)]
        public object Composition(object nf, object cf) => null;
        [凾(256)]
        public object Inverse(object v) => null;
        [凾(256)]
        public object Mapping(object f, object x, int size) => null;
        [凾(256)]
        public object Operate(object x, object y) => null;
    }

}
