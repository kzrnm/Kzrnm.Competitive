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
        /// <typeparam name="Nd">ノード</typeparam>
        /// <typeparam name="N">操作ジェネリック型</typeparam>
        public abstract class BinarySearchTreeBase<T, Nd, N> : IList<T>
            where Nd : class, IBbstNode
            where N : IBbstNodeOp<T, Nd, N>
        {
            protected BinarySearchTreeBase() { }
            protected BinarySearchTreeBase(IEnumerable<T> v) : this(v.ToArray()) { }
            protected BinarySearchTreeBase(T[] v) : this(v.AsSpan()) { }
            protected BinarySearchTreeBase(ReadOnlySpan<T> v) : this(N.Build(v)) { }
            protected BinarySearchTreeBase(Nd root)
            {
                this.root = root;
            }
            /// <summary>
            /// 二分木の根
            /// </summary>
            protected Nd root;
            public T this[int index]
            {
                get => N.GetValue(ref root, index);
                set => N.SetValue(ref root, index, value);
            }
            bool ICollection<T>.IsReadOnly => false;
            /// <summary>
            /// 要素数を返します。
            /// </summary>
            public int Count => root?.Size ?? 0;

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
            public Nd RemoveAt(int index) => N.Erase(ref root, index);
            void IList<T>.RemoveAt(int index) { RemoveAt(index); }


            [凾(256)]
            public void RemoveRange(int index, int count) => N.Erase(ref root, index, count);

            [凾(256)]
            public void Clear()
            {
                root = null;
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
            public override string ToString() => root?.ToString() ?? "empty";

            /// <summary>
            /// 可能なら二分木の状態が正常か確認します
            /// </summary>
            [Conditional("DEBUG")]
            [SourceExpander.NotEmbeddingSource]
            internal void Validate() => N.Validate(root);
        }

        public interface IBbstNode
        {
            int Size { get; }
        }

        /// <summary>
        /// 平衡二分探索木のノード操作
        /// </summary>
        /// <typeparam name="Nd">ノード</typeparam>
        /// <typeparam name="N">自身の型</typeparam>
        [IsOperator]
        public interface IBbstNodeOp<Nd, N>
            where Nd : IBbstNode
            where N : IBbstNodeOp<Nd, N>
        {
            /// <summary>
            /// <paramref name="l"/> と <paramref name="r"/> をマージします。
            /// </summary>
            static abstract Nd Merge(Nd l, Nd r);

            /// <summary>
            /// <paramref name="a"/> と <paramref name="b"/> と <paramref name="c"/> をマージします。
            /// </summary>
            [凾(256)]
            static virtual Nd Merge(Nd a, Nd b, Nd c) => N.Merge(a, N.Merge(b, c));

            /// <summary>
            /// <paramref name="t"/> を <paramref name="t"/>[0..<paramref name="k"/>] と <paramref name="t"/>[<paramref name="k"/>..] に分割します。
            /// </summary>
            static abstract (Nd, Nd) Split(Nd t, int k);

            /// <summary>
            /// <paramref name="t"/>[..<paramref name="l"/>], <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>], <paramref name="t"/>[<paramref name="r"/>..] に分割します。
            /// </summary>
            [凾(256)]
            static virtual (Nd, Nd, Nd) Split(Nd t, int l, int r)
            {
                N.Propagate(ref t);
                var (v01, v2) = N.Split(t, r);
                var (v0, v1) = N.Split(v01, l);
                return (v0, v1, v2);
            }

            /// <summary>
            /// 取得前に何かしら伝播させておきます。
            /// </summary>
            static abstract void Propagate(ref Nd t);

            /// <summary>
            /// 更新後に何かしら設定します。
            /// </summary>
            static abstract Nd Update(Nd t);

            /// <summary>
            /// 普通の二分探索木なら <paramref name="t"/>、永続化している場合は <paramref name="t"/> のコピーを返します。
            /// </summary>
            [凾(256)] static virtual Nd Copy(Nd t) => t;

            /// <summary>
            /// 可能なら二分木の状態が正常か確認します
            /// </summary>
            [SourceExpander.NotEmbeddingSource]
            static virtual void Validate(Nd t) { }
        }

        /// <summary>
        /// 値を持つ平衡二分探索木のノード操作
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="Nd">ノード</typeparam>
        /// <typeparam name="N">自身の型</typeparam>
        [IsOperator]
        public interface IBbstNodeOp<T, Nd, N> : IBbstNodeOp<Nd, N>
            where Nd : class, IBbstNode
            where N : IBbstNodeOp<T, Nd, N>
        {
            /// <summary>
            /// <paramref name="t"/>[<paramref name="k"/>] に <paramref name="x"/> を代入します。
            /// </summary>
            static abstract void SetValue(ref Nd t, int k, T x);

            /// <summary>
            /// <paramref name="t"/>[<paramref name="k"/>] を返します。
            /// </summary>
            static abstract T GetValue(ref Nd t, int k);
            /// <summary>
            /// 単一の値を持つノードを作成します。
            /// </summary>
            static abstract Nd Create(T v);

            /// <summary>
            /// <paramref name="t"/> の総積を返します。
            /// </summary>
            static abstract T Sum(Nd t);

            [凾(256)]
            static virtual Nd Build(ReadOnlySpan<T> vs)
            {
                switch (vs.Length)
                {
                    case 0: return null;
                    case 1: return N.Create(vs[0]);
                }

                var half = vs.Length >> 1;
                return N.Merge(N.Build(vs[..half]), N.Build(vs[half..]));
            }

            /// <summary>
            /// <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
            /// </summary>
            [凾(256)]
            static virtual T Prod(ref Nd t, int l, int r)
            {
                N.Propagate(ref t);
                var (a, b, c) = N.Split(t, l, r);
                var v = N.Sum(b);
                t = N.Merge(a, b, c);
                return v;
            }

            /// <summary>
            /// 先頭に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddFirst(ref Nd t, T item)
            {
                N.AddFirst(ref t, N.Create(item));
            }

            /// <summary>
            /// 先頭に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddFirst(ref Nd t, Nd newNode)
            {
                N.Propagate(ref t);
                t = N.Merge(newNode, t);
            }

            /// <summary>
            /// 末尾に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddLast(ref Nd t, T item)
            {
                N.AddLast(ref t, N.Create(item));
            }

            /// <summary>
            /// 末尾に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddLast(ref Nd t, Nd newNode)
            {
                N.Propagate(ref t);
                t = N.Merge(t, newNode);
            }

            /// <summary>
            /// <paramref name="index"/> に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void Insert(ref Nd t, int index, T item)
                => N.Insert(ref t, index, N.Create(item));

            /// <summary>
            /// <paramref name="index"/> に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void Insert(ref Nd t, int index, Nd newNode)
            {
                N.Propagate(ref t);
                var (l, r) = N.Split(t, index);
                t = N.Merge(l, newNode, r);
            }

            /// <summary>
            /// <paramref name="index"/> のノードを削除します。削除した部分木を返します。
            /// </summary>
            [凾(256)]
            static virtual Nd Erase(ref Nd t, int index) => N.Erase(ref t, index, 1);

            /// <summary>
            /// <paramref name="index"/> から <paramref name="count"/> 個のノードを削除します。削除した部分木を返します。
            /// </summary>
            [凾(256)]
            static virtual Nd Erase(ref Nd t, int index, int count)
            {
                N.Propagate(ref t);
                var (l, m, r) = N.Split(t, index, index + count);
                t = N.Merge(l, r);
                return m;
            }

            /// <summary>
            /// <paramref name="t"/> の Enumerator を返します。
            /// </summary>
            static abstract IEnumerator<T> GetEnumerator(ref Nd t);
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
    }
}