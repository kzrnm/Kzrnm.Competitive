using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    using static SetNodeBase;
    /// <summary>
    /// 閉区間をSetで保持する
    /// </summary>
    public class SetIntervalClosedLong : SetIntervalClosed<long, LongOperator>
    {
        public SetIntervalClosedLong() : base() { }
        public SetIntervalClosedLong(IEnumerable<(long From, long ToInclusive)> collection) : base(collection) { }
    }
    /// <summary>
    /// 閉区間をSetで保持する
    /// </summary>
    public class SetIntervalClosedInt : SetIntervalClosed<int, IntOperator>
    {
        public SetIntervalClosedInt() : base() { }
        public SetIntervalClosedInt(IEnumerable<(int From, int ToInclusive)> collection) : base(collection) { }
    }

    /// <summary>
    /// 閉区間をSetで保持する
    /// </summary>
    [DebuggerTypeProxy(typeof(SetIntervalClosed<,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SetIntervalClosed<T, TOp>
        : SetBase<(T From, T ToInclusive), T, SetIntervalClosed<T, TOp>.Node, SetIntervalClosed<T, TOp>.NodeOperator>
        where TOp : struct, IComparer<T>, IUnaryNumOperator<T>
    {
        public SetIntervalClosed() : this(default(TOp)) { }
        public SetIntervalClosed(IEnumerable<(T From, T ToInclusive)> collection) : this(collection, default(TOp)) { }
        public SetIntervalClosed(TOp comparer) : base(false, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public SetIntervalClosed(IEnumerable<(T From, T ToInclusive)> collection, TOp comparer)
            : base(false, new NodeOperator(comparer), collection) { }
        protected override ((T From, T ToInclusive)[] array, int arrayCount) InitArray(IEnumerable<(T From, T ToInclusive)> collection)
        {
            var list = new SimpleList<(T From, T ToInclusive)>(
                collection.Where(t => comparer.Compare(t.From, t.ToInclusive) <= 0));
            if (list.Count == 0) return (Array.Empty<(T From, T ToInclusive)>(), 0);

            list.Sort();
            var resList = new SimpleList<(T From, T ToInclusive)>(list.Count)
            {
                list[0]
            };
            for (int i = 1; i < list.Count; i++)
            {
                var pt = resList[^1].ToInclusive;
                var (f, t) = list[i];
                if (comparer.Compare(pt, f) >= 0)
                {
                    if (comparer.Compare(pt, t) < 0)
                        resList[^1].ToInclusive = t;
                }
                else
                    resList.Add((f, t));
            }

            return (resList.ToArray(), resList.Count);
        }

        protected readonly TOp comparer;

        protected new bool Add((T From, T ToInclusive) item) => Add(item.From, item.ToInclusive);
        public bool Add(T from, T toInclusive)
        {
            Node left = null, right = null;
            if (FindNodeLowerBound(from) is { } n)
            {
                if (comparer.Compare(from, n.From) >= 0)
                {
                    if (comparer.Compare(toInclusive, n.ToInclusive) <= 0)
                        return false;
                    left = n;
                    right = FindNodeLowerBound(toInclusive);
                    if (right != null && comparer.Compare(toInclusive, right.From) < 0)
                        right = null;
                }
                else if (comparer.Compare(toInclusive, n.ToInclusive) <= 0 && comparer.Compare(toInclusive, n.From) >= 0)
                {
                    right = n;
                }
            }
            if (left != null && right != null)
            {
                var pt = right.ToInclusive;
                Remove(left.ToInclusive, pt);
                left.ToInclusive = pt;
            }
            else
            {
                Remove(comparer.Increment(from), comparer.Decrement(toInclusive));
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
                order = comparer.Compare(from, current.From);
                if (current.Is4Node)
                {
                    current.Split4Node();
                    if (IsNonNullRed(parent))
                    {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = (Node)(order < 0 ? current.Left : current.Right);
            }
            Node node = new Node(from, toInclusive, NodeColor.Red);
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
                    int order = foundMatch ? -1 : comparer.Compare(from, current.From);
                    if (!foundMatch && order <= 0 && comparer.Compare(toInclusive, current.ToInclusive) >= 0)
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
                    int order = comparer.Compare(from, match.From);
                    if (order <= 0)
                    {
                        if (comparer.Compare(toInclusive, match.From) >= 0) // 右側
                        {
                            resultMatch = true;
                            match.From = comparer.Increment(toInclusive);
                        }
                    }
                    else
                    {
                        if (comparer.Compare(toInclusive, match.ToInclusive) >= 0) // 左側
                        {
                            resultMatch = true;
                            match.ToInclusive = comparer.Decrement(from);

                            match = FindNodeLowerBound(toInclusive);
                            if (match != null && comparer.Compare(toInclusive, match.From) >= 0)
                                match.From = comparer.Increment(toInclusive);
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
            return node != null && comparer.Compare(toInclusive, node.ToInclusive) <= 0;
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toInclusive"/>]の範囲を列挙する。はみ出た範囲は切り捨てる。
        /// </summary>
        public IEnumerable<(T From, T ToInclusive)> RangeTruncate(T from, T toInclusive)
        {
            if (comparer.Compare(from, Max.ToInclusive) > 0) yield break;
            foreach (var tup in EnumerateItem(FindNodeLowerBound(from)))
            {
                var (f, t) = tup;
                if (comparer.Compare(f, from) < 0) f = from;
                if (comparer.Compare(f, toInclusive) > 0) yield break;
                if (comparer.Compare(t, toInclusive) > 0) t = toInclusive;
                yield return (f, t);
            }
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toExclusive"/>)の範囲を列挙する。はみ出た範囲も含める。
        /// </summary>
        public IEnumerable<(T From, T ToInclusive)> RangeAll(T from, T toInclusive)
        {
            if (comparer.Compare(from, Max.ToInclusive) > 0) yield break;
            foreach (var (f, t) in EnumerateItem(FindNodeLowerBound(from)))
            {
                if (comparer.Compare(f, toInclusive) > 0) yield break;
                yield return (f, t);
            }
        }

        public class Node : SetNodeBase
        {
            public T From;
            public T ToInclusive;
            public (T From, T ToInclusive) Pair => (From, ToInclusive);
            internal Node(T from, T toInclusive, NodeColor color) : base(color)
            {
                this.From = from;
                this.ToInclusive = toInclusive;
            }
            public override string ToString() => $"Range = [{From}, {ToInclusive}], Size = {Size}";
        }
        public struct NodeOperator : INodeOperator<(T From, T ToInclusive), T, Node>
        {
            private readonly TOp comparer;
            public IComparer<T> Comparer => comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [MethodImpl(AggressiveInlining)]
            public Node Create((T From, T ToInclusive) item, NodeColor color) => new Node(item.From, item.ToInclusive, color);
            [MethodImpl(AggressiveInlining)]
            public (T From, T ToInclusive) GetValue(Node node) => node.Pair;
            [MethodImpl(AggressiveInlining)]
            public void SetValue(ref Node node, (T From, T ToInclusive) value)
            {
                node.From = value.From;
                node.ToInclusive = value.ToInclusive;
            }
            [MethodImpl(AggressiveInlining)]
            public T GetCompareKey((T From, T ToInclusive) item) => item.From;
            [MethodImpl(AggressiveInlining)]
            public int Compare(T x, T y) => comparer.Compare(x, y);
            [MethodImpl(AggressiveInlining)]
            public int Compare((T From, T ToInclusive) node1, (T From, T ToInclusive) node2) => comparer.Compare(node1.From, node2.From);
            [MethodImpl(AggressiveInlining)]
            public int Compare(Node node1, Node node2) => comparer.Compare(node1.From, node2.From);
            [MethodImpl(AggressiveInlining)]
            public int Compare(T value, Node node)
            {
                int forder = comparer.Compare(node.From, value);
                if (forder > 0) return -1;
                int torder = comparer.Compare(node.ToInclusive, value);
                if (torder >= 0)
                    return 0;
                return 1;
            }
        }
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
