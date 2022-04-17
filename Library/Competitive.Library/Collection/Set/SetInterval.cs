using AtCoder;
using AtCoder.Operators;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 半開区間をSetで保持する
    /// </summary>
    public class SetIntervalLong : SetInterval<long, LongOperator>
    {
        public SetIntervalLong() : base() { }
        public SetIntervalLong(IEnumerable<(long From, long ToExclusive)> collection) : base(collection) { }
    }
    /// <summary>
    /// 半開区間をSetで保持する
    /// </summary>
    public class SetIntervalInt : SetInterval<int, IntOperator>
    {
        public SetIntervalInt() : base() { }
        public SetIntervalInt(IEnumerable<(int From, int ToExclusive)> collection) : base(collection) { }
    }

    /// <summary>
    /// 半開区間をSetで保持する
    /// </summary>
    [DebuggerTypeProxy(typeof(SetInterval<,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetInterval<T, TOp>
        : SetBase<(T From, T ToExclusive), SetInterval<T, TOp>.C<T>, SetInterval<T, TOp>.Node, SetInterval<T, TOp>.NodeOperator>
        where T : IComparable<T>
        where TOp : struct, IUnaryNumOperator<T>, IMinMaxValue<T>
    {
        public SetInterval() : this(new TOp()) { }
        public SetInterval(IEnumerable<(T From, T ToExclusive)> collection) : this(collection, new TOp()) { }
        public SetInterval(TOp op) : base(false, new NodeOperator())
        {
            this.op = op;
        }
        public SetInterval(IEnumerable<(T From, T ToExclusive)> collection, TOp op)
            : base(false, new NodeOperator(), collection)
        {
            this.op = op;
        }

        protected readonly TOp op;

        protected override ReadOnlySpan<(T From, T ToExclusive)> InitArray(IEnumerable<(T From, T ToExclusive)> collection)
        {
            var list = new List<(T From, T ToExclusive)>(
                collection.Where(t => t.From.CompareTo(t.ToExclusive) < 0));
            if (list.Count == 0) return Array.Empty<(T From, T ToExclusive)>();

            list.Sort();
            var resList = new List<(T From, T ToExclusive)>(list.Count)
            {
                list[0]
            };
            for (int i = 1; i < list.Count; i++)
            {
                var pt = resList[^1].ToExclusive;
                var (f, t) = list[i];
                if (pt.CompareTo(f) >= 0)
                {
                    if (pt.CompareTo(t) < 0)
                        resList[^1] = (resList[^1].From, t);
                }
                else
                    resList.Add((f, t));
            }

            return resList.ToArray();
        }


        protected new bool Add((T From, T ToExclusive) item) => Add(item.From, item.ToExclusive);
        public bool Add(T from, T toExclusive)
        {
            var left = FindNodeLowerBound(op.Decrement(from));
            var right = FindNodeLowerBound(op.Decrement(toExclusive));
            if (left != null)
            {
                if (from.CompareTo(left.From) < 0 || from.CompareTo(left.ToExclusive) > 0)
                    left = null;
                else if (toExclusive.CompareTo(left.ToExclusive) <= 0)
                    return false;
            }
            if (right != null && toExclusive.CompareTo(right.From) < 0)
                right = null;

            if (left != null && right != null)
            {
                var pt = right.ToExclusive;
                Remove(left.ToExclusive, pt);
                left.ToExclusive = pt;
            }
            else
            {
                Remove(op.Increment(from), op.Decrement(toExclusive));
                if (left != null)
                    left.ToExclusive = toExclusive;
                else if (right != null)
                    right.From = from;
                else
                    AddImpl(from, toExclusive);
            }
            return true;
        }
        private void AddImpl(T from, T toExclusive)
        {
            if (root == null)
            {
                root = new Node(from, toExclusive, NodeColor.Black);
                return;
            }

            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;
            int order = 0;
            while (current != null)
            {
                order = from.CompareTo(current.From);
                if (current.Is4Node)
                {
                    current.Split4Node();
                    if (parent.IsNonNullRed())
                    {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = (Node)(order < 0 ? current.Left : current.Right);
            }
            Node node = new Node(from, toExclusive, NodeColor.Red);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.ColorBlack();
            return;
        }

        public bool Remove(T from, T toExclusive)
        {
            if (root == null) return false;
            bool resultMatch = false;
            bool foundMatch = true;
            while (foundMatch)
            {
                foundMatch = false;
                Node current = root;
                Node parent = null;
                Node grandParent = null;
                Node match = null;
                Node parentOfMatch = null;
                while (current != null)
                {
                    if (current.Is2Node)
                        Fix2Node(match, ref parentOfMatch, current, parent, grandParent);
                    int order = foundMatch ? -1 : from.CompareTo(current.From);
                    if (!foundMatch && order <= 0 && toExclusive.CompareTo(current.ToExclusive) >= 0)
                    {
                        resultMatch = foundMatch = true;
                        match = current;
                        parentOfMatch = parent;
                        order = 0;
                    }
                    grandParent = parent;
                    parent = current;
                    current = (Node)(order < 0 ? current.Left : current.Right);
                }
                if (match != null)
                {
                    ReplaceNode(match, parentOfMatch, parent, grandParent);
                }
            }
            {
                var match = FindNodeLowerBound(from);
                if (match != null)
                {
                    int order = from.CompareTo(match.From);
                    if (order <= 0)
                    {
                        if (toExclusive.CompareTo(match.From) > 0) // 右側
                        {
                            resultMatch = true;
                            match.From = toExclusive;
                        }
                    }
                    else
                    {
                        if (toExclusive.CompareTo(match.ToExclusive) >= 0) // 左側
                        {
                            resultMatch = true;
                            match.ToExclusive = from;

                            match = FindNodeLowerBound(toExclusive);
                            if (match != null && toExclusive.CompareTo(match.From) > 0)
                                match.From = toExclusive;
                        }
                        else // 分割
                        {
                            resultMatch = true;
                            var prevTo = match.ToExclusive;
                            match.ToExclusive = from;
                            root?.ColorBlack();
                            AddImpl(toExclusive, prevTo);
                        }
                    }
                }
            }
            root?.ColorBlack();
            return resultMatch;
        }
        protected new bool Contains((T From, T ToExclusive) item) => Contains(item.From, item.ToExclusive);
        public bool Contains(T from, T toExclusive)
        {
            var node = FindNode(from);
            return node != null && toExclusive.CompareTo(node.ToExclusive) <= 0;
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toExclusive"/>)の範囲を列挙する。はみ出た範囲は切り捨てる。
        /// </summary>
        public IEnumerable<(T From, T ToExclusive)> RangeTruncate(T from, T toExclusive)
        {
            if (from.CompareTo(Max.ToExclusive) >= 0) yield break;
            foreach (var tup in EnumerateItem(FindNodeLowerBound(from)))
            {
                var (f, t) = tup;
                if (f.CompareTo(from) < 0) f = from;
                if (f.CompareTo(toExclusive) >= 0) yield break;
                if (t.CompareTo(toExclusive) > 0) t = toExclusive;
                yield return (f, t);
            }
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toExclusive"/>)の範囲を列挙する。はみ出た範囲も含める。
        /// </summary>
        public IEnumerable<(T From, T ToExclusive)> RangeAll(T from, T toExclusive)
        {
            if (from.CompareTo(Max.ToExclusive) >= 0) yield break;
            foreach (var (f, t) in EnumerateItem(FindNodeLowerBound(from)))
            {
                if (f.CompareTo(toExclusive) >= 0) yield break;
                yield return (f, t);
            }
        }

        /// <summary>
        /// <paramref name="other"/> との和集合に更新します。
        /// </summary>
        public void UnionWith(IEnumerable<(T From, T ToExclusive)> other)
        {
            foreach (var (f, t) in other)
                Add(f, t);
        }

        /// <summary>
        /// <paramref name="other"/> との差集合に更新します。
        /// </summary>
        public void ExceptWith(IEnumerable<(T From, T ToExclusive)> other)
        {
            foreach (var (f, t) in other)
                Remove(f, t);
        }

        /// <summary>
        /// <paramref name="other"/> との積集合に更新します。
        /// </summary>
        public void IntersectWith(IEnumerable<(T From, T ToExclusive)> other)
        {
            bool isCalled = false;
            (T From, T ToExclusive) last = default;
            foreach (var tup in other)
            {
                if (isCalled)
                {
                    Remove(last.ToExclusive, tup.From);
                }
                else
                {
                    isCalled = true;
                    Remove(op.MinValue, tup.From);
                }
                last = tup;
            }
            if (isCalled)
                Remove(last.ToExclusive, op.MaxValue);
        }

        public class Node : SetNodeBase<Node>
        {
            public T From;
            public T ToExclusive;
            public (T From, T ToExclusive) Pair => (From, ToExclusive);
            internal Node(T from, T toExclusive, NodeColor color) : base(color)
            {
                From = from;
                ToExclusive = toExclusive;
            }
            public override string ToString() => $"Range = [{From}, {ToExclusive}), Size = {Size}";
        }
        public readonly struct C<Tv> : IComparable<Node> where Tv : IComparable<T>
        {
            private readonly Tv v;
            public C(Tv val) { v = val; }
            [凾(256)]
            public int CompareTo(Node other)
            {
                int forder = v.CompareTo(other.From);
                if (forder < 0) return -1;
                int torder = v.CompareTo(other.ToExclusive);
                if (torder < 0)
                    return 0;
                return 1;
            }
        }
        public struct NodeOperator : ISetOperator<(T From, T ToExclusive), C<T>, Node>
        {
            [凾(256)]
            public Node Create((T From, T ToExclusive) item, NodeColor color) => new Node(item.From, item.ToExclusive, color);
            [凾(256)]
            public (T From, T ToExclusive) GetValue(Node node) => node.Pair;
            [凾(256)]
            public int Compare((T From, T ToExclusive) x, (T From, T ToExclusive) y) => x.From.CompareTo(y.From);
            [凾(256)]
            public C<T> GetCompareKey((T From, T ToExclusive) item) => new C<T>(item.From);
        }

        #region Search
        [凾(256)] public new Node FindNode<Tv>(Tv item) where Tv : IComparable<T> => base.FindNode(new C<Tv>(item));
        [凾(256)] public bool Contains<Tv>(Tv item) where Tv : IComparable<T> => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new L()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new L()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) LowerBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new L()).node.Pair;
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new U()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new U()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) UpperBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new U()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new LR()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new LR()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) ReverseLowerBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new LR()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new UR()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new UR()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) ReverseUpperBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new UR()).node.Pair;
        #endregion Search

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
            private readonly IEnumerable<(T From, T ToExclusive)> collection;
            public DebugView(IEnumerable<(T From, T ToExclusive)> collection)
            {
                this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items => collection.Select(t => new DebugItem(t.From, t.ToExclusive)).ToArray();
        }
    }
}
