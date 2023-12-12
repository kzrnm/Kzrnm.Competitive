using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class Bbst
    {
        [凾(256)] public static T Merge<T>(T l, T r) where T : IBbstNode<T> => T.Merge(l, r);
        [凾(256)] public static (T, T) Split<T>(T t, int p) where T : IBbstNode<T> => T.Split(t, p);
    }
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
        /// <typeparam name="Node">ノード</typeparam>
        public abstract class BinarySearchTreeBase<T, Node> : IList<T>
            where Node : class, IBbstNode<T, Node>
        {
            protected BinarySearchTreeBase() { }
            protected BinarySearchTreeBase(IEnumerable<T> v) : this(v.ToArray()) { }
            protected BinarySearchTreeBase(T[] v) : this(v.AsSpan()) { }
            protected BinarySearchTreeBase(ReadOnlySpan<T> v) : this(Node.Build(v)) { }
            protected BinarySearchTreeBase(Node root)
            {
                this.root = root;
            }
            /// <summary>
            /// 二分木の根
            /// </summary>
            protected Node root;
            public T this[int index]
            {
                get => Node.GetValue(ref root, index);
                set => Node.SetValue(ref root, index, value);
            }
            bool ICollection<T>.IsReadOnly => false;
            /// <summary>
            /// 要素数を返します。
            /// </summary>
            public int Count => root?.Size ?? 0;

            /// <summary>
            /// [<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
            /// </summary>
            [凾(256)] public T Prod(int l, int r) => Node.Prod(ref root, l, r);
            [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);
            /// <summary>
            /// 総積を返します。
            /// </summary>
            public T AllProd => Node.Sum(root);

            void ICollection<T>.Add(T item) => AddLast(item);

            /// <summary>
            /// 先頭に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public void AddFirst(T item) => Node.AddFirst(ref root, item);

            /// <summary>
            /// 末尾に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public void AddLast(T item) => Node.AddLast(ref root, item);

            /// <summary>
            /// 末尾に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public void AddRange(IEnumerable<T> items)
            {
                root = Node.Merge(root, Node.Build(items.ToArray()));
            }

            /// <summary>
            /// <paramref name="index"/> に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public void Insert(int index, T item)
                => Node.Insert(ref root, index, item);

            /// <summary>
            /// <paramref name="index"/> に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public void InsertRange(int index, IEnumerable<T> items)
                => Node.Insert(ref root, index, Node.Build(items.ToArray()));

            /// <summary>
            /// <paramref name="index"/> のノードを削除して該当のノードを返します。
            /// </summary>
            [凾(256)]
            public Node RemoveAt(int index) => Node.Erase(ref root, index);
            void IList<T>.RemoveAt(int index) { RemoveAt(index); }


            [凾(256)]
            public void RemoveRange(int index, int count) => Node.Erase(ref root, index, count);

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

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => Node.GetEnumerator(ref root);
            IEnumerator IEnumerable.GetEnumerator() => Node.GetEnumerator(ref root);
            bool ICollection<T>.Contains(T item) { throw new NotSupportedException(); }
            int IList<T>.IndexOf(T item) { throw new NotSupportedException(); }
            bool ICollection<T>.Remove(T item) { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 平衡二分探索木のノード
        /// </summary>
        /// <typeparam name="Node">ノード</typeparam>
        public interface IBbstNode<Node> where Node : IBbstNode<Node>
        {
            public int Size { get; }

            /// <summary>
            /// <paramref name="l"/> と <paramref name="r"/> をマージします。
            /// </summary>
            static abstract Node Merge(Node l, Node r);

            /// <summary>
            /// <paramref name="a"/> と <paramref name="b"/> と <paramref name="c"/> をマージします。
            /// </summary>
            [凾(256)]
            static virtual Node Merge(Node a, Node b, Node c) => Node.Merge(Node.Merge(a, b), c);

            /// <summary>
            /// <paramref name="t"/> を <paramref name="t"/>[0..<paramref name="k"/>] と <paramref name="t"/>[<paramref name="k"/>..] に分割します。
            /// </summary>
            static abstract (Node, Node) Split(Node t, int k);

            /// <summary>
            /// <paramref name="t"/>[..<paramref name="l"/>], <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>], <paramref name="t"/>[<paramref name="r"/>..] に分割します。
            /// </summary>
            [凾(256)]
            static virtual (Node, Node, Node) Split(Node t, int l, int r)
            {
                Node.Propagate(ref t);
                var (v01, v2) = Node.Split(t, r);
                var (v0, v1) = Node.Split(v01, l);
                return (v0, v1, v2);
            }

            /// <summary>
            /// 取得前に何かしら伝播させておきます。
            /// </summary>
            static abstract void Propagate(ref Node t);

            /// <summary>
            /// 更新後に何かしら設定します。
            /// </summary>
            static abstract Node Update(Node t);
        }

        /// <summary>
        /// 値を持つ平衡二分探索木のノード
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="Node">ノード</typeparam>
        public interface IBbstNode<T, Node> : IBbstNode<Node> where Node : class, IBbstNode<T, Node>
        {
            /// <summary>
            /// <paramref name="t"/>[<paramref name="k"/>] に <paramref name="x"/> を代入します。
            /// </summary>
            static abstract void SetValue(ref Node t, int k, T x);

            /// <summary>
            /// <paramref name="t"/>[<paramref name="k"/>] を返します。
            /// </summary>
            static abstract T GetValue(ref Node t, int k);
            /// <summary>
            /// 単一の値を持つノードを作成します。
            /// </summary>
            static abstract Node Create(T v);

            /// <summary>
            /// <paramref name="t"/> の総積を返します。
            /// </summary>
            static abstract T Sum(Node t);

            [凾(256)]
            static virtual Node Build(ReadOnlySpan<T> vs)
            {
                switch (vs.Length)
                {
                    case 0: return null;
                    case 1: return Node.Create(vs[0]);
                }

                var half = vs.Length >> 1;
                return Node.Merge(Node.Build(vs[..half]), Node.Build(vs[half..]));
            }

            /// <summary>
            /// <paramref name="t"/>[<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
            /// </summary>
            [凾(256)]
            static virtual T Prod(ref Node t, int l, int r)
            {
                Node.Propagate(ref t);
                var (a, b, c) = Node.Split(t, l, r);
                var v = Node.Sum(b);
                t = Node.Merge(a, b, c);
                return v;
            }

            /// <summary>
            /// 先頭に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddFirst(ref Node t, T item)
            {
                Node.AddFirst(ref t, Node.Create(item));
            }

            /// <summary>
            /// 先頭に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddFirst(ref Node t, Node newNode)
            {
                Node.Propagate(ref t);
                t = Node.Merge(newNode, t);
            }

            /// <summary>
            /// 末尾に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddLast(ref Node t, T item)
            {
                Node.AddLast(ref t, Node.Create(item));
            }

            /// <summary>
            /// 末尾に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void AddLast(ref Node t, Node newNode)
            {
                Node.Propagate(ref t);
                t = Node.Merge(t, newNode);
            }

            /// <summary>
            /// <paramref name="index"/> に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void Insert(ref Node t, int index, T item)
                => Node.Insert(ref t, index, Node.Create(item));

            /// <summary>
            /// <paramref name="index"/> に <paramref name="newNode"/> を追加します。
            /// </summary>
            [凾(256)]
            static virtual void Insert(ref Node t, int index, Node newNode)
            {
                Node.Propagate(ref t);
                var (l, r) = Node.Split(t, index);
                t = Node.Merge(l, newNode, r);
            }

            /// <summary>
            /// <paramref name="index"/> のノードを削除します。削除した部分木を返します。
            /// </summary>
            [凾(256)]
            static virtual Node Erase(ref Node t, int index) => Node.Erase(ref t, index, 1);

            /// <summary>
            /// <paramref name="index"/> から <paramref name="count"/> 個のノードを削除します。削除した部分木を返します。
            /// </summary>
            [凾(256)]
            static virtual Node Erase(ref Node t, int index, int count)
            {
                Node.Propagate(ref t);
                var (l, m, r) = Node.Split(t, index, index + count);
                t = Node.Merge(l, r);
                return m;
            }

            /// <summary>
            /// 普通の二分探索木なら <paramref name="t"/>、永続化している場合は <paramref name="t"/> のコピーを返します。
            /// </summary>
            [凾(256)] static virtual Node Copy(Node t) => t;

            /// <summary>
            /// <paramref name="t"/> の Enumerator を返します。
            /// </summary>
            static abstract IEnumerator<T> GetEnumerator(ref Node t);
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