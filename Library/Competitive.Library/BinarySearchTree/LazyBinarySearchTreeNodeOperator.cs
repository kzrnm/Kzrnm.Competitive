using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    /// <summary>
    /// 反転可能遅延伝搬平衡二分探索木のオペレータ
    /// </summary>
    /// <typeparam name="T">モノイド</typeparam>
    /// <typeparam name="F">作用素</typeparam>
    /// <typeparam name="TOp">モノイドの操作</typeparam>
    /// <typeparam name="Node">ノード</typeparam>
    /// <typeparam name="TImpl">ノードのMerge, Split</typeparam>
    public struct LazyBinarySearchTreeNodeOperator<T, F, TOp, Node, TImpl>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where Node : ILazyBbstNode<T, F, Node>
        where TImpl : struct, ILazyBbstImplOperator<T, Node>
    {
        public static TOp op => default;
        public TImpl im => default;

        /// <summary>
        /// 部分木 <paramref name="t"/> を [0, <paramref name="l"/>) と [<paramref name="l"/>, <paramref name="r"/>), [<paramref name="r"/>, N) に分割します。
        /// </summary>
        [凾(256)]
        public (Node, Node, Node) Split3(Node t, int l, int r)
        {
            var (t1, t2) = im.Split(t, l);
            var (s1, s2) = im.Split(t2, r - l);
            return (t1, s1, s2);
        }

        /// <summary>
        /// 部分木 <paramref name="l"/>, <paramref name="m"/>, <paramref name="r"/> から一つの部分木を作ります。
        /// </summary>
        [凾(256)]
        public Node Merge3(Node l, Node m, Node r)
            => im.Merge(l, im.Merge(m, r));

        [凾(256)]
        public void Toggle(Node t)
        {
            (t.Left, t.Right) = (t.Right, t.Left);
            t.Sum = op.Inverse(t.Sum);
            t.IsReverse = !t.IsReverse;
        }
        [凾(256)]
        public T Prod(ref Node t, int l, int r)
        {
            var (x, y1, y2) = Split3(t, l, r);
            var ret = Sum(y1);
            t = Merge3(x, y1, y2);
            return ret;
        }
        [凾(256)]
        public T Sum(Node t) => t != null ? t.Sum : op.Identity;
        [凾(256)]
        public int Size(Node t) => t?.Size ?? 0;

        [凾(256)]
        public void Reverse(ref Node t, int a, int b)
        {
            var (x, y1, y2) = Split3(t, a, b);
            y1 = im.Copy(y1);
            Toggle(y1);
            t = Merge3(x, y1, y2);
        }

        [凾(256)]
        public void Apply(ref Node t, int l, int r, F f)
        {
            if (l >= r) return;
            var (x, y1, y2) = Split3(t, l, r);
            y1 = im.Copy(y1);
            y1.Lazy = op.Composition(f, y1.Lazy);
            im.Propagate(ref y1);
            t = Merge3(x, y1, y2);
        }

        [凾(256)]
        public void Insert(ref Node t, int k, T v)
        {
            var (x1, x2) = im.Split(t, k);
            t = Merge3(x1, im.Create(v), x2);
        }

        [凾(256)]
        public T Erase(ref Node t, int k)
        {
            var (x, y1, y2) = Split3(t, k, k + 1);
            t = im.Merge(x, y2);
            return y1.Key;
        }

        [凾(256)]
        public T PopFirst(ref Node t)
        {
            var (t1, t2) = im.Split(t, 1);
            t = t2;
            return t1.Key;
        }

        [凾(256)]
        public T PopLast(ref Node t)
        {
            var (t1, t2) = im.Split(t, Size(t) - 1);
            t = t1;
            return t2.Key;
        }

        [凾(256)]
        public void AddFirst(ref Node t, T v)
        {
            t = im.Merge(im.Create(v), t);
        }

        [凾(256)]
        public void AddLast(ref Node t, T v)
        {
            t = im.Merge(t, im.Create(v));
        }
    }
}