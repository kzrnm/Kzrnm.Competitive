using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 半開区間をSetで保持する
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(typeof(SetInterval<>.DebugView))]
    public class SetInterval<T> : SetInterval<T, SetInterval<T>.Range, SetInterval<T>.Node, SetInterval<T>.Op>
            where T : IComparable<T>, IMinMaxValue<T>
    {
        public SetInterval() : base() { }
        public SetInterval(IEnumerable<(T, T)> vals) : base(vals) { }
        public readonly record struct Range(T From, T ToExclusive) : ISetIntervalRange<T>, IComparable<Range>, IComparable<T>
        {
            T ISetIntervalRange<T>.To => ToExclusive;

            [凾(256)]
            public int CompareTo(Range other) => From.CompareTo(other.From) switch { 0 => ToExclusive.CompareTo(other.ToExclusive), var c => c };

            [凾(256)]
            public int CompareTo(T other)
            {
                int c = From.CompareTo(other);
                if (c > 0) return c;
                c = ToExclusive.CompareTo(other);
                if (c <= 0) return -1; // exclusive
                return 0;
            }

            public static implicit operator Range((T F, T T) t) => new(t.F, t.T);
            public static implicit operator (T, T)(Range r) => (r.From, r.ToExclusive);

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => $"[{From}, {ToExclusive})";
        }

#pragma warning disable IDE0251 // メンバーを 'readonly' にする
        [StructLayout(LayoutKind.Auto)]
        public struct Node : ISetNode<Range, int>, ISetIntervalRangeNode<T>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Parent { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Left { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Right { get; set; }
            public bool IsBlack { get; set; }
            public int Size { get; set; }
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugParent => SetNodeConv.Load<Node>(Parent);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugLeft => SetNodeConv.Load<Node>(Left);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugRight => SetNodeConv.Load<Node>(Right);

            public T From { get; set; }
            public T To { get; set; }
            Range ISetNode<Range, int>.Value => new(From, To);

            internal Node(Range item, bool isBlack)
            {
                Parent = Left = Right = -1;
                Size = 1;
                IsBlack = isBlack;
                From = item.From;
                To = item.ToExclusive;
            }

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => $"Value = {((ISetNode<Range, int>)this).Value} Size = {Size}";
        }

#pragma warning restore IDE0251 // メンバーを 'readonly' にする

        public struct Op : ISetIntervalOp<T, Range, Node, Op>
        {
            [凾(256)]
            public static Node CreateNode(Range v, bool isBlack) => new(v, isBlack);

            [凾(256)]
            public static Range CreateRange(T from, T to) => new(from, to);
        }

        [SourceExpander.NotEmbeddingSource]
        private class DebugView
        {
            [DebuggerDisplay("[{" + nameof(From) + "}, {" + nameof(ToExclusive) + "})")]
            public class DebugItem
            {
                T From;
                T ToExclusive;
                public DebugItem(T From, T ToExclusive)
                {
                    this.From = From;
                    this.ToExclusive = ToExclusive;
                }
            }
            private readonly IEnumerable<Range> collection;
            public DebugView(IEnumerable<Range> collection)
            {
                this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items => collection.Select(t => new DebugItem(t.From, t.ToExclusive)).ToArray();
        }
    }

    /// <summary>
    /// 閉区間をSetで保持する
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(typeof(SetIntervalClosed<>.DebugView))]
    public class SetIntervalClosed<T> : SetInterval<T, SetIntervalClosed<T>.Range, SetIntervalClosed<T>.Node, SetIntervalClosed<T>.Op>
            where T : IComparable<T>, IMinMaxValue<T>, IIncrementOperators<T>, IDecrementOperators<T>
    {
        public SetIntervalClosed() : base() { }
        public SetIntervalClosed(IEnumerable<(T, T)> vals) : base(vals) { }
        public readonly record struct Range(T From, T ToInclusive) : ISetIntervalRange<T>, IComparable<Range>, IComparable<T>
        {
            T ISetIntervalRange<T>.To => ToInclusive;

            [凾(256)]
            public int CompareTo(Range other) => From.CompareTo(other.From) switch { 0 => ToInclusive.CompareTo(other.ToInclusive), var c => c };

            [凾(256)]
            public int CompareTo(T other)
            {
                int c = From.CompareTo(other);
                if (c > 0) return c;
                c = ToInclusive.CompareTo(other);
                if (c < 0) return c; // inclusive
                return 0;
            }
            public static implicit operator Range((T F, T T) t) => new(t.F, t.T);
            public static implicit operator (T, T)(Range r) => (r.From, r.ToInclusive);
            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => $"[{From}, {ToInclusive}]";
        }

#pragma warning disable IDE0251 // メンバーを 'readonly' にする
        [StructLayout(LayoutKind.Auto)]
        public struct Node : ISetNode<Range, int>, ISetIntervalRangeNode<T>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Parent { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Left { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Right { get; set; }
            public bool IsBlack { get; set; }
            public int Size { get; set; }
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugParent => SetNodeConv.Load<Node>(Parent);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugLeft => SetNodeConv.Load<Node>(Left);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugRight => SetNodeConv.Load<Node>(Right);

            public T From { get; set; }
            public T To { get; set; }
            Range ISetNode<Range, int>.Value => new(From, To);

            internal Node(Range item, bool isBlack)
            {
                Parent = Left = Right = -1;
                Size = 1;
                IsBlack = isBlack;
                From = item.From;
                To = item.ToInclusive;
            }

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => $"Value = {((ISetNode<Range, int>)this).Value} Size = {Size}";
        }

#pragma warning restore IDE0251 // メンバーを 'readonly' にする

        public struct Op : ISetIntervalOp<T, Range, Node, Op>
        {
            [凾(256)]
            public static Node CreateNode(Range v, bool isBlack) => new(v, isBlack);

            [凾(256)]
            public static Range CreateRange(T from, T to) => new(from, to);

            [凾(256)]
            public static SetResult<Range, int, Node> Remove(ref int root, Range item, DefaultComparerStruct<Range> comparer)
                => Rm<Op>(ref root, item);
            [凾(256)]
            static SetResult<Range, int, Node> Rm<O>(ref int root, Range item) where O : ISetIntervalOp<T, Range, Node, Op>
            {
                var f = item.From;
                var t = item.ToInclusive;
                if (!EqualityComparer<T>.Default.Equals(f, T.MinValue)) --f;
                if (!EqualityComparer<T>.Default.Equals(t, T.MaxValue)) ++t;
                return O.Remove(ref root, f, t);
            }
        }

        [SourceExpander.NotEmbeddingSource]
        private class DebugView
        {
            [DebuggerDisplay("[{" + nameof(From) + "}, {" + nameof(ToInclusive) + "}]")]
            public class DebugItem
            {
                T From;
                T ToInclusive;
                public DebugItem(T From, T ToInclusive)
                {
                    this.From = From;
                    this.ToInclusive = ToInclusive;
                }
            }
            private readonly IEnumerable<Range> collection;
            public DebugView(IEnumerable<Range> collection)
            {
                this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items => collection.Select(t => new DebugItem(t.From, t.ToInclusive)).ToArray();
        }
    }

    namespace Internal
    {
        public class SetInterval<T, Tr, Nd, N> : SetBase<Tr, ISetIntervalOp<T, Tr, Nd, N>.C, DefaultComparerStruct<Tr>, Nd, N>
            where T : IComparable<T>, IMinMaxValue<T>
            where Tr : IComparable<Tr>, IComparable<T>, ISetIntervalRange<T>
            where Nd : struct, ISetNode<Tr, int>, ISetIntervalRangeNode<T>
            where N : ISetIntervalOp<T, Tr, Nd, N>
        {
            public SetInterval() : base(false, default) { }
            public SetInterval(IEnumerable<(T, T)> vals) : base(false, default, vals.Select(t => N.CreateRange(t.Item1, t.Item2))) { }

            public bool Add(T from, T to) => Add(N.CreateRange(from, to));
            public bool Remove(T from, T to) => Remove(N.CreateRange(from, to));
            public new bool Contains(Tr item) => Contains(item.From, item.To);
            public bool Contains(T from, T to)
            {
                if (FindNode(from) is not { } b) return false;
                Debug.Assert(b.NodeRef >= 0);
                Debug.Assert(b.Node.Value.CompareTo(from) == 0);
                return from.CompareTo(b.Node.From) >= 0 && to.CompareTo(b.Node.To) <= 0;
            }

            /// <summary>
            /// [<paramref name="from"/>, <paramref name="to"/>)の範囲を列挙する。はみ出た範囲は切り捨てる。
            /// </summary>
            public IEnumerable<Tr> RangeTruncate(T from, T to)
            {
                var n = FindNodeLowerBound(from);
                if (n.NodeRef >= 0)
                {
                    var r = N.CreateRange(from, to);
                    foreach (var nnt in EnumerateNode(n.NodeRef))
                    {
                        var nt = nnt.Node.Value;
                        int rcf = r.CompareTo(nt.From);

                        // ノードが range を過ぎたら終了
                        // |--| range
                        //        ~~~~  node
                        if (rcf < 0)
                            yield break;

                        int rct = r.CompareTo(nt.To);
                        if (rcf == 0)
                        {
                            // ノードが range を過ぎかけ
                            // |----| range
                            //     ~~~~  node
                            // 閉区間ならノードがギリギリ重なる場合があるがノードの途中までのパターンと同じ
                            // |--| range
                            //    ~~~~  node
                            if (rct < 0)
                                yield return N.CreateRange(nt.From, to);

                            // ノードが range に覆われている
                            // |-----------| range
                            //     ~~~~  node
                            else
                                yield return nt;
                        }
                        else
                        {
                            // ノードが range を覆っている
                            //    |-| range
                            // ~~~~~~~~~~  node
                            if (rct < 0)
                                yield return r;


                            // ノードが range より小さい場合(FindNodeLowerBoundのためありえない)
                            //       |--| range
                            // ~~~~  node
                            // if (rct > 0) {}


                            // ノードが range に重なる
                            //   |----| range
                            // ~~~~  node
                            // 閉区間ならノードがギリギリ重なる場合があるがノードの途中までのパターンと同じ
                            //    |--| range
                            // ~~~~  node
                            else
                                yield return N.CreateRange(from, nt.To);
                        }
                    }
                }
            }

            /// <summary>
            /// [<paramref name="from"/>, <paramref name="to"/>)の範囲を列挙する。はみ出た範囲も含める。
            /// </summary>
            public IEnumerable<Tr> RangeAll(T from, T to)
            {
                var n = FindNodeLowerBound(from);
                if (n.NodeRef >= 0)
                {
                    var r = N.CreateRange(from, to);
                    foreach (var nnt in EnumerateNode(n.NodeRef))
                    {
                        var nt = nnt.Node.Value;
                        int rcf = r.CompareTo(nt.From);

                        // ノードが range を過ぎたら終了
                        // |--| range
                        //        ~~~~  node
                        if (rcf < 0)
                            yield break;
                        yield return nt;
                    }
                }
            }

            /// <summary>
            /// <paramref name="other"/> との和集合に更新します。
            /// </summary>
            public void UnionWith(IEnumerable<Tr> other)
            {
                foreach (var nt in other)
                    Add(nt.From, nt.To);
            }

            /// <summary>
            /// <paramref name="other"/> との差集合に更新します。
            /// </summary>
            public void ExceptWith(IEnumerable<Tr> other)
            {
                foreach (var nt in other)
                    Remove(nt.From, nt.To);
            }

            /// <summary>
            /// <paramref name="other"/> との積集合に更新します。
            /// </summary>
            public void IntersectWith(IEnumerable<Tr> other)
            {
                var a = other.ToArray().AsSpan();
                a.Sort();

                var ls = new List<Tr>();

                while (Count > 0)
                {
                    var r = RemoveAt(0).Node;
                    while (a.Length > 0)
                    {
                        int c1 = a[0].CompareTo(r.From);
                        if (c1 < 0) { }
                        else if (c1 == 0)
                        {
                            int c2 = a[0].CompareTo(r.To);
                            if (c2 == 0)
                            {
                                ls.Add(r.Value);
                                break;
                            }
                            Debug.Assert(c2 < 0);
                            ls.Add(N.CreateRange(r.From, a[0].To));
                        }
                        else
                        {
                            int c2 = a[0].CompareTo(r.To);
                            if (c2 > 0) break;
                            if (c2 == 0)
                            {
                                ls.Add(N.CreateRange(a[0].From, r.To));
                                break;
                            }
                            ls.Add(a[0]);
                        }
                        a = a[1..];
                    }
                }

                root = N.ConstructRootFromSortedArray(CollectionsMarshal.AsSpan(ls), -1);
            }

            #region Search
            /// <summary>
            /// <paramref name="item"/> が含まれていれば返します。
            /// </summary>
            [凾(256)]
            public SetFindResult<Tr, int, Nd>? FindNode(T item)
                => FindNode(N.GetCompareKey(item));
            [凾(256)] public bool Contains(T item) => Contains(item, item);
            /// <summary>
            /// <paramref name="item"/> 以上の最初のノードを返します。
            /// </summary>
            [凾(256)] public SetFindResult<Tr, int, Nd> FindNodeLowerBound(T item) => BinarySearch(N.GetCompareKey(item), new SetLower());
            /// <summary>
            /// <paramref name="item"/> 以上の最初のインデックスを返します。なければ Count を返します。
            /// </summary>
            [凾(256)] public int LowerBoundIndex(T item) => BinarySearch(N.GetCompareKey(item), new SetLower()).Index;
            /// <summary>
            /// <paramref name="item"/> を超える最初のノードを返します。
            /// </summary>
            [凾(256)] public SetFindResult<Tr, int, Nd> FindNodeUpperBound(T item) => BinarySearch(N.GetCompareKey(item), new SetUpper());
            /// <summary>
            /// <paramref name="item"/> を超える最初のインデックスを返します。なければ Count を返します。
            /// </summary>
            [凾(256)] public int UpperBoundIndex(T item) => BinarySearch(N.GetCompareKey(item), new SetUpper()).Index;

            /// <summary>
            /// <paramref name="item"/> 以下の最後のノードを返します。
            /// </summary>
            [凾(256)] public SetFindResult<Tr, int, Nd> FindNodeReverseLowerBound(T item) => BinarySearch(N.GetCompareKey(item), new SetLowerRev());
            /// <summary>
            /// <paramref name="item"/> 以下の最後のインデックスを返します。なければ -1 を返します。
            /// </summary>
            [凾(256)] public int ReverseLowerBoundIndex(T item) => BinarySearch(N.GetCompareKey(item), new SetLowerRev()).Index;

            /// <summary>
            /// <paramref name="item"/> 未満の最後のノードを返します。
            /// </summary>
            [凾(256)] public SetFindResult<Tr, int, Nd> FindNodeReverseUpperBound(T item) => BinarySearch(N.GetCompareKey(item), new SetUpperRev());
            /// <summary>
            /// <paramref name="item"/> 未満の最後のインデックスを返します。なければ -1 を返します。
            /// </summary>
            [凾(256)] public int ReverseUpperBoundIndex(T item) => BinarySearch(N.GetCompareKey(item), new SetUpperRev()).Index;
            #endregion Search
        }

        public interface ISetIntervalRange<T>
        {
            T From { get; }
            T To { get; }
        }
        public interface ISetIntervalRangeNode<T> : ISetIntervalRange<T>
        {
            new T From { get; set; }
            new T To { get; set; }
        }

        [IsOperator]
        public interface ISetIntervalOp<T, Tr, Nd, N> : ISetPOp<Tr, ISetIntervalOp<T, Tr, Nd, N>.C, DefaultComparerStruct<Tr>, Nd, N>
            where T : IComparable<T>
            where Tr : IComparable<Tr>, IComparable<T>, ISetIntervalRange<T>
            where Nd : struct, ISetNode<Tr, int>, ISetIntervalRangeNode<T>
            where N : ISetIntervalOp<T, Tr, Nd, N>
        {
            static abstract Tr CreateRange(T from, T to);

            [凾(256)]
            static virtual C GetCompareKey(T v) => new(v);

            [凾(256)]
            static C ISetOp<Tr, C, DefaultComparerStruct<Tr>, Nd, int, N, PoolStructRefOp<Nd>>.GetCompareKey(DefaultComparerStruct<Tr> comparer, Tr item)
                => new(item.From);


            static ReadOnlySpan<Tr> ISetOp<Tr, C, DefaultComparerStruct<Tr>, Nd, int, N, PoolStructRefOp<Nd>>.InitArray(IEnumerable<Tr> col, DefaultComparerStruct<Tr> op, bool multi)
            {
                Debug.Assert(!multi);

                var list = new List<Tr>(col.Where(t => t.From.CompareTo(t.To) <= 0));
                if (list.Count == 0) return [];

                list.Sort();
                var resList = new List<Tr>(list.Count)
                {
                    list[0]
                };
                for (int i = 1; i < list.Count; i++)
                {
                    var pt = resList[^1].To;
                    var ll = list[i];
                    var f = ll.From;
                    var t = ll.To;
                    if (pt.CompareTo(f) >= 0)
                    {
                        if (pt.CompareTo(t) < 0)
                            resList[^1] = N.CreateRange(resList[^1].From, t);
                    }
                    else
                        resList.Add(N.CreateRange(f, t));
                }

                return resList.ToArray();
            }

            /// <summary>
            /// 閉区間として <typeparamref name="T"/> と比較。
            /// </summary>
            public readonly record struct L(T v) : IComparable<Nd>
            {
                [凾(256)]
                public int CompareTo(Nd other)
                {
                    int c = v.CompareTo(other.Value.From);
                    if (c <= 0) return c;
                    c = v.CompareTo(other.Value.To);
                    if (c <= 0) return 0;
                    return c;
                }
            }
            /// <summary>
            /// <typeparamref name="Tr"/> の定義通りの <typeparamref name="T"/> と比較。
            /// </summary>
            public readonly record struct C(T v) : IComparable<Nd>
            {
                [凾(256)] public int CompareTo(Nd other) => -other.Value.CompareTo(v);
            }
            [凾(256)]
            static int ISetOp<Tr, C, DefaultComparerStruct<Tr>, Nd, int, N, PoolStructRefOp<Nd>>.Add(ref int root, Tr item, DefaultComparerStruct<Tr> comparer, bool isMulti)
            {
                T f = item.From;
                T t = item.To;

                var fr = N.BinarySearch(root, new L(f), new SetLower());
                var tr = N.BinarySearch(root, new L(t), new SetLowerRev());

                if (fr.Index <= tr.Index)
                {
                    // 区間内に既存ノードがある

                    // 最初のノードを除いて削除する
                    for (int i = tr.Index; i > fr.Index; i--)
                        N.RemoveAt(ref root, i);

                    if (f.CompareTo(fr.Node.Value.From) > 0)
                        f = fr.Node.Value.From;
                    if (t.CompareTo(tr.Node.Value.To) < 0)
                        t = tr.Node.Value.To;
                    ref Nd d = ref StructPool<Nd>.Default.Get(fr.NodeRef);
                    d.From = f;
                    d.To = t;
                    return fr.NodeRef;
                }

                // 新規ノード
                return N.Add(ref root, N.GetCompareKey(comparer, item), item, comparer, isMulti);
            }

            [凾(256)]
            static SetResult<Tr, int, Nd> ISetOp<Tr, C, DefaultComparerStruct<Tr>, Nd, int, N, PoolStructRefOp<Nd>>.Remove(ref int root, Tr item, DefaultComparerStruct<Tr> comparer)
                => N.Remove(ref root, item.From, item.To);
            [凾(256)]
            static virtual SetResult<Tr, int, Nd> Remove(ref int root, T from, T to)
            {
                var fr = N.BinarySearch(root, new L(from), new SetLower());
                var tr = N.BinarySearch(root, new L(to), new SetLowerRev());

                SetResult<Tr, int, Nd> nullResult = new(-1, default);
                bool ne;

                if (fr.Index <= tr.Index)
                {
                    // 区間内に既存ノードがある

                    // 区間内にノードが1つだけ
                    if (fr.Index == tr.Index)
                    {
                        ref Nd d = ref StructPool<Nd>.Default.Get(fr.NodeRef);
                        switch (from.CompareTo(d.From), to.CompareTo(d.To))
                        {
                            case ( <= 0, >= 0):
                                return N.RemoveAt(ref root, fr.Index);
                            case ( <= 0, _):
                                ne = !EqualityComparer<T>.Default.Equals(d.From, to);
                                d.From = to;
                                return ne ? new(fr.NodeRef, d) : nullResult;
                            case (_, >= 0):
                                ne = !EqualityComparer<T>.Default.Equals(d.To, from);
                                d.To = from;
                                return ne ? new(fr.NodeRef, d) : nullResult;
                        }
                        Debug.Assert(from.CompareTo(d.From) > 0);
                        Debug.Assert(to.CompareTo(d.To) < 0);
                        var add = N.CreateRange(to, d.To);
                        d.To = from;
                        N.Add(ref root, N.GetCompareKey(new(), add), add, new(), false);
                    }

                    var rt = nullResult;
                    if (tr.Node.Value.CompareTo(to) == 0)
                    {
                        ref Nd d = ref StructPool<Nd>.Default.Get(tr.NodeRef);
                        ne = !EqualityComparer<T>.Default.Equals(d.From, to);
                        d.From = to;
                        if (ne)
                            rt = new(tr.NodeRef, d);
                    }
                    else
                        rt = N.RemoveAt(ref root, tr.Index);

                    for (int i = tr.Index - 1; i > fr.Index; i--)
                        rt = N.RemoveAt(ref root, i);

                    if (from.CompareTo(fr.Node.Value.From) > 0)
                    {
                        ref Nd d = ref StructPool<Nd>.Default.Get(fr.NodeRef);
                        ne = !EqualityComparer<T>.Default.Equals(d.To, from);
                        d.To = from;
                        if (ne)
                            rt = new(fr.NodeRef, d);
                    }
                    else
                        rt = N.RemoveAt(ref root, fr.Index);

                    return rt;
                }

                // 既存ノードなし
                return nullResult;
            }
        }
    }
}