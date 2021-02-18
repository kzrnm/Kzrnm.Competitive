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
        : SetBase<(T From, T ToExclusive), T, SetInterval<T, TOp>.Node, SetInterval<T, TOp>.NodeOperator>
        where TOp : struct, IComparer<T>, IUnaryNumOperator<T>
    {
        public SetInterval() : this(default(TOp)) { }
        public SetInterval(IEnumerable<(T From, T ToExclusive)> collection) : this(collection, default(TOp)) { }
        public SetInterval(TOp comparer) : base(false, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public SetInterval(IEnumerable<(T From, T ToExclusive)> collection, TOp comparer)
            : base(false, new NodeOperator(comparer), collection) { }
        protected override ((T From, T ToExclusive)[] array, int arrayCount) InitArray(IEnumerable<(T From, T ToExclusive)> collection)
        {
            var list = new SimpleList<(T From, T ToExclusive)>(
                collection.Where(t => comparer.Compare(t.From, t.ToExclusive) < 0));
            if (list.Count == 0) return (Array.Empty<(T From, T ToExclusive)>(), 0);

            list.Sort();
            var resList = new SimpleList<(T From, T ToExclusive)>(list.Count)
            {
                list[0]
            };
            for (int i = 1; i < list.Count; i++)
            {
                var pt = resList[^1].ToExclusive;
                var (f, t) = list[i];
                if (comparer.Compare(pt, f) >= 0)
                {
                    if (comparer.Compare(pt, t) < 0)
                        resList[^1].ToExclusive = t;
                }
                else
                    resList.Add((f, t));
            }

            return (resList.ToArray(), resList.Count);
        }

        protected readonly TOp comparer;

        protected new bool Add((T From, T ToExclusive) item) => Add(item.From, item.ToExclusive);
        public bool Add(T from, T toExclusive)
        {
            Node left = null, right = null;
            if (FindNodeLowerBound(comparer.Decrement(from)) is { } n)
            {
                if (comparer.Compare(from, n.From) >= 0)
                {
                    if (comparer.Compare(toExclusive, n.ToExclusive) <= 0)
                        return false;
                    left = n;
                    right = FindNodeLowerBound(toExclusive);
                    if (right != null && comparer.Compare(toExclusive, right.From) < 0)
                        right = null;
                }
                else if (comparer.Compare(toExclusive, n.ToExclusive) <= 0 && comparer.Compare(toExclusive, n.From) >= 0)
                {
                    right = n;
                }
            }
            if (left != null && right != null)
            {
                var pt = right.ToExclusive;
                Remove(left.ToExclusive, pt);
                left.ToExclusive = pt;
            }
            else
            {
                Remove(comparer.Increment(from), comparer.Decrement(toExclusive));
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
                    int order = foundMatch ? -1 : comparer.Compare(from, current.From);
                    if (!foundMatch && order <= 0 && comparer.Compare(toExclusive, current.ToExclusive) >= 0)
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
                        if (comparer.Compare(toExclusive, match.From) > 0) // 右側
                        {
                            resultMatch = true;
                            match.From = toExclusive;
                        }
                    }
                    else
                    {
                        if (comparer.Compare(toExclusive, match.ToExclusive) >= 0) // 左側
                        {
                            resultMatch = true;
                            match.ToExclusive = from;

                            match = FindNodeLowerBound(toExclusive);
                            if (match != null && comparer.Compare(toExclusive, match.From) > 0)
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
            return node != null && comparer.Compare(toExclusive, node.ToExclusive) <= 0;
        }

        /// <summary>
        /// [<paramref name="from"/>, <paramref name="toExclusive"/>)の範囲を列挙する
        /// </summary>
        public IEnumerable<(T From, T ToExclusive)> Range(T from, T toExclusive)
        {
            if (comparer.Compare(from, Max.ToExclusive) >= 0) yield break;
            foreach (var tup in EnumerateItem(FindNodeLowerBound(from)))
            {
                var (f, t) = tup;
                if (comparer.Compare(f, from) < 0) f = from;
                if (comparer.Compare(f, toExclusive) >= 0) yield break;
                if (comparer.Compare(t, toExclusive) > 0) t = toExclusive;
                yield return (f, t);
            }
        }

        public class Node : SetNodeBase
        {
            public T From;
            public T ToExclusive;
            public (T From, T ToExclusive) Pair => (From, ToExclusive);
            internal Node(T from, T toExclusive, NodeColor color) : base(color)
            {
                this.From = from;
                this.ToExclusive = toExclusive;
            }
            public override string ToString() => $"Range = [{From}, {ToExclusive}), Size = {Size}";
        }
        public struct NodeOperator : INodeOperator<(T From, T ToExclusive), T, Node>
        {
            private readonly TOp comparer;
            public IComparer<T> Comparer => comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [MethodImpl(AggressiveInlining)]
            public Node Create((T From, T ToExclusive) item, NodeColor color) => new Node(item.From, item.ToExclusive, color);
            [MethodImpl(AggressiveInlining)]
            public (T From, T ToExclusive) GetValue(Node node) => node.Pair;
            [MethodImpl(AggressiveInlining)]
            public void SetValue(ref Node node, (T From, T ToExclusive) value)
            {
                node.From = value.From;
                node.ToExclusive = value.ToExclusive;
            }
            [MethodImpl(AggressiveInlining)]
            public T GetCompareKey((T From, T ToExclusive) item) => item.From;
            [MethodImpl(AggressiveInlining)]
            public int Compare(T x, T y) => comparer.Compare(x, y);
            [MethodImpl(AggressiveInlining)]
            public int Compare((T From, T ToExclusive) node1, (T From, T ToExclusive) node2) => comparer.Compare(node1.From, node2.From);
            [MethodImpl(AggressiveInlining)]
            public int Compare(Node node1, Node node2) => comparer.Compare(node1.From, node2.From);
            [MethodImpl(AggressiveInlining)]
            public int Compare(T value, Node node)
            {
                int forder = comparer.Compare(node.From, value);
                if (forder > 0) return -1;
                int torder = comparer.Compare(node.ToExclusive, value);
                if (torder > 0)
                    return 0;
                return 1;
            }
        }
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
