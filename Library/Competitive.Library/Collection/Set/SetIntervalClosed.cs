using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 閉区間をSetで保持する
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(typeof(SetIntervalClosed<>.DebugView))]
    public class SetIntervalClosed<T>
        : SetBase<(T From, T ToInclusive), SetIntervalClosed<T>.C<T>, SetIntervalClosed<T>.Node, SetIntervalClosed<T>.NodeOperator>
        where T : IComparable<T>, IIncrementOperators<T>, IDecrementOperators<T>, IMinMaxValue<T>
    {
        public SetIntervalClosed() : base(false, new NodeOperator()) { }
        public SetIntervalClosed(IEnumerable<(T From, T ToInclusive)> collection) : base(false, new NodeOperator(), collection) { }

#pragma warning disable CS8981
        static class op
#pragma warning restore CS8981
        {
            [凾(256)] public static T Increment(T v) => ++v;
            [凾(256)] public static T Decrement(T v) => --v;
            public static T MaxValue => T.MaxValue;
            public static T MinValue => T.MinValue;
        }

        protected override ReadOnlySpan<(T From, T ToInclusive)> InitArray(IEnumerable<(T From, T ToInclusive)> collection)
        {
            var list = new List<(T From, T ToInclusive)>(
                collection.Where(t => t.From.CompareTo(t.ToInclusive) <= 0));
            if (list.Count == 0) return Array.Empty<(T From, T ToInclusive)>();

            list.Sort();
            var resList = new List<(T From, T ToInclusive)>(list.Count)
            {
                list[0]
            };
            for (int i = 1; i < list.Count; i++)
            {
                var pt = resList[^1].ToInclusive;
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

        protected new bool Add((T From, T ToInclusive) item) => Add(item.From, item.ToInclusive);
        public bool Add(T from, T toInclusive)
        {
            var left = FindNode(from);
            var right = FindNodeLowerBound(toInclusive);
            if (left != null)
            {
                if (toInclusive.CompareTo(left.ToInclusive) <= 0)
                    return false;
            }
            if (right != null && toInclusive.CompareTo(right.From) < 0)
                right = null;

            if (left != null && right != null)
            {
                var pt = right.ToInclusive;
                Remove(left.ToInclusive, pt);
                left.ToInclusive = pt;
            }
            else
            {
                Remove(op.Increment(from), op.Decrement(toInclusive));
                if (left != null)
                    left.ToInclusive = toInclusive;
                else if (right != null)
                    right.From = from;
                else
                    AddImpl(from, toInclusive);
            }
            return true;
        }
        private void AddImpl(T from, T toInclusive)
        {
            if (root == null)
            {
                root = new Node(from, toInclusive, NodeColor.Black);
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
                current = (order < 0 ? current.Left : current.Right);
            }
            var node = new Node(from, toInclusive, NodeColor.Red);
            if (order >= 0) parent.Right = node;
            else parent.Left = node;
            if (parent.IsRed) InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            root.ColorBlack();
            return;
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toInclusive"/>]を削除
        /// </summary>
        public bool Remove(T from, T toInclusive)
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
                    if (!foundMatch && order <= 0 && toInclusive.CompareTo(current.ToInclusive) >= 0)
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
                        if (toInclusive.CompareTo(match.From) >= 0) // 右側
                        {
                            resultMatch = true;
                            match.From = op.Increment(toInclusive);
                        }
                    }
                    else
                    {
                        if (toInclusive.CompareTo(match.ToInclusive) >= 0) // 左側
                        {
                            resultMatch = true;
                            match.ToInclusive = op.Decrement(from);

                            match = FindNodeLowerBound(toInclusive);
                            if (match != null && toInclusive.CompareTo(match.From) >= 0)
                                match.From = op.Increment(toInclusive);
                        }
                        else // 分割
                        {
                            resultMatch = true;
                            var prevTo = match.ToInclusive;
                            match.ToInclusive = from;
                            root?.ColorBlack();
                            AddImpl(toInclusive, prevTo);
                        }
                    }
                }
            }
            root?.ColorBlack();
            return resultMatch;
        }
        protected new bool Contains((T From, T ToInclusive) item) => Contains(item.From, item.ToInclusive);
        public bool Contains(T from, T toInclusive)
        {
            var node = FindNode(from);
            return node != null && toInclusive.CompareTo(node.ToInclusive) <= 0;
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toInclusive"/>]の範囲を列挙する。はみ出た範囲は切り捨てる。
        /// </summary>
        public IEnumerable<(T From, T ToInclusive)> RangeTruncate(T from, T toInclusive)
        {
            if (from.CompareTo(Max.ToInclusive) > 0) yield break;
            foreach (var tup in EnumerateItem(FindNodeLowerBound(from)))
            {
                var (f, t) = tup;
                if (f.CompareTo(from) < 0) f = from;
                if (f.CompareTo(toInclusive) > 0) yield break;
                if (t.CompareTo(toInclusive) > 0) t = toInclusive;
                yield return (f, t);
            }
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toInclusive"/>]の範囲を列挙する。はみ出た範囲も含める。
        /// </summary>
        public IEnumerable<(T From, T ToInclusive)> RangeAll(T from, T toInclusive)
        {
            if (from.CompareTo(Max.ToInclusive) > 0) yield break;
            foreach (var (f, t) in EnumerateItem(FindNodeLowerBound(from)))
            {
                if (f.CompareTo(toInclusive) > 0) yield break;
                yield return (f, t);
            }
        }

        /// <summary>
        /// <paramref name="other"/> との和集合に更新します。
        /// </summary>
        public void UnionWith(IEnumerable<(T From, T ToInclusive)> other)
        {
            foreach (var (f, t) in other)
                Add(f, t);
        }

        /// <summary>
        /// <paramref name="other"/> との差集合に更新します。
        /// </summary>
        public void ExceptWith(IEnumerable<(T From, T ToInclusive)> other)
        {
            foreach (var (f, t) in other)
                Remove(f, t);
        }

        /// <summary>
        /// <paramref name="other"/> との積集合に更新します。
        /// </summary>
        public void IntersectWith(IEnumerable<(T From, T ToInclusive)> other)
        {
            bool isCalled = false;
            (T From, T ToInclusive) last = default;
            foreach (var tup in other)
            {
                if (isCalled)
                {
                    Remove(op.Increment(last.ToInclusive), op.Decrement(tup.From));
                }
                else
                {
                    isCalled = true;
                    if (op.MinValue.CompareTo(tup.From) < 0)
                        Remove(op.MinValue, op.Decrement(tup.From));
                }
                last = tup;
            }
            if (isCalled && last.ToInclusive.CompareTo(op.MaxValue) < 0)
                Remove(op.Increment(last.ToInclusive), op.MaxValue);
        }

        public class Node : SetNodeBase<Node>
        {
            public T From;
            public T ToInclusive;
            public (T From, T ToInclusive) Pair => (From, ToInclusive);
            internal Node(T from, T toInclusive, NodeColor color) : base(color)
            {
                From = from;
                ToInclusive = toInclusive;
            }
            public override string ToString() => $"Range = [{From}, {ToInclusive}], Size = {Size}";
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
                int torder = v.CompareTo(other.ToInclusive);
                if (torder <= 0)
                    return 0;
                return 1;
            }
        }
        public readonly struct NodeOperator : ISetOperator<(T From, T ToInclusive), C<T>, Node>
        {
            [凾(256)]
            public Node Create((T From, T ToInclusive) item, NodeColor color) => new Node(item.From, item.ToInclusive, color);
            [凾(256)]
            public (T From, T ToInclusive) GetValue(Node node) => node.Pair;
            [凾(256)]
            public C<T> GetCompareKey((T From, T ToInclusive) item) => new C<T>(item.From);
            [凾(256)]
            public int Compare((T From, T ToInclusive) x, (T From, T ToInclusive) y) => x.From.CompareTo(y.From);
        }
        #region Search
        [凾(256)] public new Node FindNode<Tv>(Tv item) where Tv : IComparable<T> => base.FindNode(new C<Tv>(item));
        [凾(256)] public bool Contains<Tv>(Tv item) where Tv : IComparable<T> => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLower()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLower()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToInclusive) LowerBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLower()).node.Pair;
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpper()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpper()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToInclusive) UpperBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpper()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLowerRev()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLowerRev()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToInclusive) ReverseLowerBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLowerRev()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpperRev()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpperRev()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToInclusive) ReverseUpperBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpperRev()).node.Pair;
        #endregion Search

        #region Search<T>
        [凾(256)] public Node FindNode(T item) => base.FindNode(new C<T>(item));
        [凾(256)] public bool Contains(T item) => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound(T item) => BinarySearch(new C<T>(item), new SetLower()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex(T item) => BinarySearch(new C<T>(item), new SetLower()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) LowerBoundItem(T item) => BinarySearch(new C<T>(item), new SetLower()).node.Pair;
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeUpperBound(T item) => BinarySearch(new C<T>(item), new SetUpper()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex(T item) => BinarySearch(new C<T>(item), new SetUpper()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) UpperBoundItem(T item) => BinarySearch(new C<T>(item), new SetUpper()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseLowerBound(T item) => BinarySearch(new C<T>(item), new SetLowerRev()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex(T item) => BinarySearch(new C<T>(item), new SetLowerRev()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) ReverseLowerBoundItem(T item) => BinarySearch(new C<T>(item), new SetLowerRev()).node.Pair;

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseUpperBound(T item) => BinarySearch(new C<T>(item), new SetUpperRev()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex(T item) => BinarySearch(new C<T>(item), new SetUpperRev()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素を返します。
        /// </summary>
        [凾(256)] public (T From, T ToExclusive) ReverseUpperBoundItem(T item) => BinarySearch(new C<T>(item), new SetUpperRev()).node.Pair;
        #endregion Search<T>
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
            private readonly IEnumerable<(T From, T ToInclusive)> collection;
            public DebugView(IEnumerable<(T From, T ToInclusive)> collection)
            {
                this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items => collection.Select(t => new DebugItem(t.From, t.ToInclusive)).ToArray();
        }
    }
}
