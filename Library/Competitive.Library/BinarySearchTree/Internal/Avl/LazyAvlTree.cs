using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal.Bbst
{
    /// <summary>
    /// 遅延伝播反転可能AVL木
    /// </summary>
    public class LazyAvlTree<T> : LazyAvlTree<T, byte, SingleBbstOp<T>>
    {
        public LazyAvlTree() { }
        public LazyAvlTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazyAvlTree(T[] v) : base(v) { }
        public LazyAvlTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyAvlTree(int root) : base(root) { }
    }

    /// <summary>
    /// 遅延伝播反転可能AVL木
    /// </summary>
    public class LazyAvlTree<T, F, TOp> : LazyBinarySearchTreeBase<T, F, int, LazyAvlTreeNode<T, F, TOp>.Op>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public LazyAvlTree() { }
        public LazyAvlTree(IEnumerable<T> v) : base(v.ToArray()) { }
        public LazyAvlTree(T[] v) : base(v) { }
        public LazyAvlTree(ReadOnlySpan<T> v) : base(v) { }
        public LazyAvlTree(int root) : base(root) { }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct LazyAvlTreeNode<T, F, TOp> : IAvlNode<T, int>, ILazyBbstNode<T, F, int>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        public struct Op : ILazyAvlOp<T, F, TOp, LazyAvlTreeNode<T, F, TOp>, int, Op, PoolStructRefOp<LazyAvlTreeNode<T, F, TOp>>>
                , IBbstStructNodeOp<T, LazyAvlTreeNode<T, F, TOp>, Op>
        {
            [凾(256)] public static LazyAvlTreeNode<T, F, TOp> CreateNode(T v) => new(v);
        }

        public int Left { get; set; }
        public int Right { get; set; }
        public T Value { get; set; }
        public T Sum { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public F Lazy { get; set; }
        public bool Reversed { get; set; }
        public LazyAvlTreeNode(T v)
        {
            Left = Right = -1;
            Height = 1;
            Size = 1;
            Sum = Value = v;
            Lazy = new TOp().FIdentity;
        }

        [SourceExpander.NotEmbeddingSource]
        public readonly override string ToString() => $"Size = {Size} Lazy = {Lazy}{(Reversed ? "!" : "")} Value = {Value} Sum = {Sum}";
    }

    public interface ILazyAvlOp<T, F, TOp, Nd, R, N, C> : IAvlOp<T, TOp, Nd, R, N, C>, ILazyBbstOp<T, F, TOp, Nd, R, N, C>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
        where Nd : IAvlNode<T, R>, ILazyBbstNode<T, F, R>
        where N : ILazyAvlOp<T, F, TOp, Nd, R, N, C>
        where C : IPoolRefOp<Nd, R>
    {
        [凾(256)]
        static R IBbstOp<R, N>.Propagate(R t)
        {
            t = N.Copy(t);
            if (C.IsNull(t)) return t;
            var op = new TOp();
            ref Nd d = ref C.Load(t);
            var lazy = !EqualityComparer<F>.Default.Equals(d.Lazy, op.FIdentity);
            var rev = d.Reversed;

            if (lazy || rev)
            {
                d.Left = N.Copy(d.Left);
                d.Right = N.Copy(d.Right);

                if (lazy)
                {
                    d.Value = op.Mapping(d.Lazy, d.Value, 1);
                    if (!C.IsNull(d.Left))
                    {
                        ref Nd tl = ref C.Load(d.Left);
                        tl.Lazy = op.Composition(d.Lazy, tl.Lazy);
                        tl.Sum = op.Mapping(d.Lazy, tl.Sum, tl.Size);
                    }
                    if (!C.IsNull(d.Right))
                    {
                        ref Nd tr = ref C.Load(d.Right);
                        tr.Lazy = op.Composition(d.Lazy, tr.Lazy);
                        tr.Sum = op.Mapping(d.Lazy, tr.Sum, tr.Size);
                    }
                    d.Lazy = op.FIdentity;
                }
                if (rev)
                {
                    N.Reverse(d.Left);
                    N.Reverse(d.Right);
                }
                d.Reversed = false;
                t = N.Update(t);
            }
            return t;
        }
    }
}
