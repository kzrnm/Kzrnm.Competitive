using AtCoder;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{

    [IsOperator]
    public interface IReversibleBinarySearchTreeOperator<T, F> : ISLazySegtreeOperator<T, F>
    {
        /// <summary>
        /// <paramref name="v"/> を左右反転します。
        /// </summary>
        T Inverse(T v);
    }
    namespace Internal.Bbst
    {
        public interface IBbstNode<T, TNode> where TNode : IBbstNode<T, TNode>
        {
            TNode Left { set; get; }
            TNode Right { set; get; }
            T Key { set; get; }
            T Sum { set; get; }
            int Size { set; get; }
        }
        public interface ILazyBbstNode<T, F, TNode> : IBbstNode<T, TNode>
            where TNode : ILazyBbstNode<T, F, TNode>
        {
            F Lazy { set; get; }
            bool IsReverse { set; get; }
        }
        /// <summary>
        /// Merge, Split の実装
        /// </summary>
        [IsOperator]
        public interface IBbstImplOperator<TNode>
        {
            /// <summary>
            /// 部分木 <paramref name="l"/> と <paramref name="r"/> から一つの部分木を作ります。
            /// </summary>
            TNode Merge(TNode l, TNode r);
            /// <summary>
            /// 部分木 <paramref name="t"/> を [0, <paramref name="p"/>) と [<paramref name="p"/>, N) に分割します。
            /// </summary>
            (TNode, TNode) Split(TNode t, int p);
        }
        /// <summary>
        /// ノード操作の実装
        /// </summary>
        [IsOperator]
        public interface ICopyOperator<T>
        {
            /// <summary>
            /// 部分木 <paramref name="t"/> を返します。Immutable ならコピー、Mutable なら素通し。
            /// </summary>
            T Copy(T t);
        }
        /// <summary>
        /// ノード操作の実装
        /// </summary>
        [IsOperator]
        public interface IBbstImplOperator<T, TNode> : IBbstImplOperator<TNode>, ICopyOperator<TNode>
        {
            /// <summary>
            /// 要素 <paramref name="v"/> のみを持つ部分木を作成します。
            /// </summary>
            TNode Create(T v);
        }
        /// <summary>
        /// ノード操作の実装
        /// </summary>
        [IsOperator]
        public interface ILazyBbstImplOperator<T, TNode> : IBbstImplOperator<T, TNode>
        {
            /// <summary>
            /// <paramref name="t"/> の子に遅延伝搬させます。
            /// </summary>
            /// <returns><paramref name="t"/> でなければならない</returns>
            [凾(256)]
            TNode Propagate(ref TNode t);
        }
        public struct SingleBbstOp<T> : IReversibleBinarySearchTreeOperator<T, T>, ISegtreeOperator<T>
        {
            public T Identity => default;
            public T FIdentity => default;
            [凾(256)] public T Composition(T nf, T cf) => FIdentity;
            [凾(256)] public T Inverse(T v) => v;
            [凾(256)] public T Mapping(T f, T x, int size) => x;
            [凾(256)] public T Operate(T x, T y) => EqualityComparer<T>.Default.Equals(x,default)?y:x;
        }
    }
}
