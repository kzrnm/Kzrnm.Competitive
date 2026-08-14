using AtCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    namespace Internal
    {
        /// <summary>
        /// 平衡二分探索木を実装する
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="R">ノード参照</typeparam>
        /// <typeparam name="N">操作ジェネリック型</typeparam>
        public abstract class BinarySearchTreeBase<T, R, N> : IList<T>
            where N : IBbstOp<T, R, N>
        {
            protected BinarySearchTreeBase() : this(N.Null) { }
            protected BinarySearchTreeBase(ReadOnlySpan<T> v) : this(N.Build(v)) { }
            protected BinarySearchTreeBase(R root)
            {
                this.root = root;
            }
            /// <summary>
            /// 二分木の根
            /// </summary>
            protected R root;
            public T this[int index]
            {
                get
                {
                    root = N.GetValue(root, index, out T x);
                    return x;
                }
                set => root = N.SetValue(root, index, value);
            }
            bool ICollection<T>.IsReadOnly => false;
            /// <summary>
            /// 要素数を返します。
            /// </summary>
            public int Count => N.Size(root);

            /// <summary>
            /// [<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
            /// </summary>
            [凾(256)] public T Prod(int l, int r) => N.Prod(ref root, l, r);
            [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);
            /// <summary>
            /// 総積を返します。
            /// </summary>
            public T AllProd => N.Sum(root);

            void ICollection<T>.Add(T item) => AddLast(item);

            /// <summary>
            /// 先頭に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public void AddFirst(T item) => N.AddFirst(ref root, item);

            /// <summary>
            /// 末尾に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public void AddLast(T item) => N.AddLast(ref root, item);

            /// <summary>
            /// 末尾に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public void AddRange(IEnumerable<T> items)
            {
                root = N.Merge(root, N.Build(items.ToArray()));
            }

            /// <summary>
            /// <paramref name="index"/> に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public void Insert(int index, T item)
                => N.Insert(ref root, index, item);

            /// <summary>
            /// <paramref name="index"/> に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public void InsertRange(int index, IEnumerable<T> items)
                => N.Insert(ref root, index, N.Build(items.ToArray()));

            /// <summary>
            /// <paramref name="index"/> のノードを削除して該当のノードを返します。
            /// </summary>
            [凾(256)]
            public R RemoveAt(int index) => N.Erase(ref root, index);
            void IList<T>.RemoveAt(int index) { RemoveAt(index); }


            [凾(256)]
            public void RemoveRange(int index, int count) => N.Erase(ref root, index, count);

            [凾(256)]
            public void Clear()
            {
                root = N.Null;
            }

            [凾(256)]
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var v in this)
                    array[arrayIndex++] = v;
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => N.GetEnumerator(ref root);
            IEnumerator IEnumerable.GetEnumerator() => N.GetEnumerator(ref root);
            bool ICollection<T>.Contains(T item) { throw new NotSupportedException(); }
            int IList<T>.IndexOf(T item) { throw new NotSupportedException(); }
            bool ICollection<T>.Remove(T item) { throw new NotSupportedException(); }

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => Root?.ToString() ?? "empty";

            [SourceExpander.NotEmbeddingSource]
            object Root => N.DebugObject(root);

            /// <summary>
            /// 可能なら二分木の状態が正常か確認します
            /// </summary>
            [Conditional("DEBUG")]
            [SourceExpander.NotEmbeddingSource]
            internal void Validate() => N.Validate(root);
        }

        /// <summary>
        /// 平衡二分探索木のノード操作
        /// </summary>
        /// <typeparam name="R">ノード参照</typeparam>
        /// <typeparam name="N">自身の型</typeparam>
        [IsOperator]
        public interface IBbstOp<R, N>
            where N : IBbstOp<R, N>
        {
            static abstract R Null { get; }
            /// <summary>
            /// <paramref name="t"/> のサイズを返します。
            /// </summary>
            static abstract int Size(R t);

            /// <summary>
            /// <paramref name="l"/> と <paramref name="r"/> をマージします。
            /// </summary>
            static abstract R Merge(R l, R r);

            /// <summary>
            /// <paramref name="a"/> と <paramref name="b"/> と <paramref name="c"/> をマージします。
            /// </summary>
            [凾(256)]
            static virtual R Merge(R a, R b, R c) => N.Merge(a, N.Merge(b, c));

            /// <summary>
            /// <paramref name="t"/> を <paramref name="t"/>[0..<paramref name="k"/>] と <paramref name="t"/>[<paramref name="k"/>..] に分割します。
            /// </summary>
            static abstract (R, R) Split(R t, int k);

            /// <summary>
            /// <paramref name="t"/>[..<paramref name="l"/>], <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>], <paramref name="t"/>[<paramref name="r"/>..] に分割します。
            /// </summary>
            [凾(256)]
            static virtual (R, R, R) Split(R t, int l, int r)
            {
                t = N.Propagate(t);
                var (v01, v2) = N.Split(t, r);
                var (v0, v1) = N.Split(v01, l);
                return (v0, v1, v2);
            }

            /// <summary>
            /// 取得前に何かしら伝播させておきます。伝搬後に <paramref name="t"/> に相当するノードを返します。
            /// </summary>
            static virtual R Propagate(R t) => N.Copy(t);

            /// <summary>
            /// 更新後に何かしら設定します。
            /// </summary>
            static abstract R Update(R t);

            /// <summary>
            /// 普通の二分探索木なら <paramref name="t"/>、永続化している場合は <paramref name="t"/> のコピーを返します。
            /// </summary>
            [凾(256)] static virtual R Copy(R t) => t;

            /// <summary>
            /// 可能なら二分木の状態が正常か確認します
            /// </summary>
            [SourceExpander.NotEmbeddingSource]
            static virtual void Validate(R t) { }

            [SourceExpander.NotEmbeddingSource]
            static virtual object DebugObject(R t) => null;
        }

        public interface IBbstStructNodeOp<T, Nd, N> : IBbstOp<T, int, N>
            where Nd : struct
            where N : IBbstStructNodeOp<T, Nd, N>
        {
            /// <summary>
            /// 単一の値を持つノードを作成します。
            /// </summary>
            static abstract Nd CreateNode(T v);

            [凾(256)]
            static int IBbstOp<T, int, N>.Create(T v)
            {
                StructPool<Nd>.Default.Rent(out var i) = N.CreateNode(v);
                return i;
            }

            [凾(256)] static void IBbstOp<T, int, N>.Free(int t) => StructPool<Nd>.Default.Return(t);
        }

        /// <summary>
        /// 値を持つ平衡二分探索木のノード操作
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="R">ノード参照</typeparam>
        /// <typeparam name="N">自身の型</typeparam>
        [IsOperator]
        public interface IBbstOp<T, R, N> : IBbstOp<R, N>
            where N : IBbstOp<T, R, N>
        {
            /// <summary>
            /// <paramref name="t"/>[<paramref name="k"/>] に <paramref name="x"/> を代入します。<paramref name="t"/> に相当するノードが返されます。
            /// </summary>
            static abstract R SetValue(R t, int k, T x);

            /// <summary>
            /// <paramref name="t"/>[<paramref name="k"/>] を <paramref name="x"/> で返します。<paramref name="t"/> に相当するノードが返されます。
            /// </summary>
            static abstract R GetValue(R t, int k, out T x);
            /// <summary>
            /// 単一の値を持つノードを作成します。
            /// </summary>
            static abstract R Create(T v);

            /// <summary>
            /// <paramref name="t"/> を削除した際に、可能ならメモリを解放します。
            /// </summary>
            [凾(256)]
            static virtual void Free(R t) { }

            /// <summary>
            /// <paramref name="t"/> の総積を返します。
            /// </summary>
            static abstract T Sum(R t);

            [凾(256)]
            static virtual R Build(ReadOnlySpan<T> vs)
            {
                switch (vs.Length)
                {
                    case 0: return N.Null;
                    case 1: return N.Create(vs[0]);
                }

                var half = vs.Length >> 1;
                return N.Merge(N.Build(vs[..half]), N.Build(vs[half..]));
            }

            /// <summary>
            /// <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
            /// </summary>
            [凾(256)]
            static virtual T Prod(ref R t, int l, int r)
            {
                t = N.Propagate(t);
                var (a, b, c) = N.Split(t, l, r);
                var v = N.Sum(b);
                t = N.Merge(a, b, c);
                return v;
            }

            /// <summary>
            /// 先頭に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddFirst(ref R t, T item)
            {
                N.AddFirst(ref t, N.Create(item));
            }

            /// <summary>
            /// 先頭に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddFirst(ref R t, R newNode)
            {
                t = N.Propagate(t);
                t = N.Merge(newNode, t);
            }

            /// <summary>
            /// 末尾に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddLast(ref R t, T item)
            {
                N.AddLast(ref t, N.Create(item));
            }

            /// <summary>
            /// 末尾に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddLast(ref R t, R newNode)
            {
                t = N.Propagate(t);
                t = N.Merge(t, newNode);
            }

            /// <summary>
            /// <paramref name="index"/> に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void Insert(ref R t, int index, T item)
                => N.Insert(ref t, index, N.Create(item));

            /// <summary>
            /// <paramref name="index"/> に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void Insert(ref R t, int index, R newNode)
            {
                t = N.Propagate(t);
                var (l, r) = N.Split(t, index);
                t = N.Merge(l, newNode, r);
            }

            /// <summary>
            /// <paramref name="index"/> のノードを削除します。削除した部分木を返します。
            /// </summary>
            [凾(256)]
            static virtual R Erase(ref R t, int index) => N.Erase(ref t, index, 1);

            /// <summary>
            /// <paramref name="index"/> から <paramref name="count"/> 個のノードを削除します。削除した部分木を返します。
            /// </summary>
            [凾(256)]
            static virtual R Erase(ref R t, int index, int count)
            {
                t = N.Propagate(t);
                var (l, m, r) = N.Split(t, index, index + count);
                t = N.Merge(l, r);
                return m;
            }

            /// <summary>
            /// <paramref name="t"/> の Enumerator を返します。
            /// </summary>
            static abstract IEnumerator<T> GetEnumerator(ref R t);
        }

        [IsOperator]
        public interface IBbstCnv<Nd, R, N, C> : IBbstOp<R, N>
            where Nd : IBbstNode<R>
            where N : IBbstOp<R, N>
            where C : IPoolRefOp<Nd, R>
        {
            static R IBbstOp<R, N>.Null => C.Null;
            [凾(256)] static int IBbstOp<R, N>.Size(R t) => C.IsNull(t) ? 0 : C.Load(t).Size;
            [SourceExpander.NotEmbeddingSource]
            static object IBbstOp<R, N>.DebugObject(R t) => C.IsNull(t) ? null : C.Load(t);
        }

        /// <summary>
        /// 平衡二分探索木のノード
        /// </summary>
        /// <typeparam name="R">ノード参照</typeparam>
        public interface IBbstNode<R>
        {
            int Size { get; set; }
            R Left { get; set; }
            R Right { get; set; }
        }

        /// <summary>
        /// 平衡二分探索木のノード
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="R">ノード参照</typeparam>
        public interface IBbstNode<T, R> : IBbstNode<R>
        {
            T Sum { get; set; }
        }

        public readonly struct SingleBbstOp<T> : IReversibleBinarySearchTreeOperator<T, byte>, ISegtreeOperator<T>
        {
            public T Identity => default;
            public byte FIdentity => default;
            [凾(256)] public byte Composition(byte nf, byte cf) => 0;
            [凾(256)] public T Inverse(T v) => v;
            [凾(256)] public T Mapping(byte f, T x, int size) => x;
            [凾(256)] public T Operate(T x, T y) => EqualityComparer<T>.Default.Equals(x, default) ? y : x;
        }

        [SourceExpander.NotEmbeddingSource]
        public class BbstNodeConv(object r, object n)
        {
            public object NodeReference => r;
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object Node => n;
            public override string ToString() => $"{Node} @{NodeReference}";
            public static BbstNodeConv Load<Nd, R, N, C>(IBbstCnv<Nd, R, N, C> _, R t)
                where Nd : IBbstNode<R>
                where N : IBbstOp<R, N>
                where C : IPoolRefOp<Nd, R> => new(t, C.IsNull(t) ? null : C.Load(t));
        }
    }
}