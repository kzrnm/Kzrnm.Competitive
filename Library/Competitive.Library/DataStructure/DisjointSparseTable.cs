using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// <para>結合則が成り立つ半群への区間クエリを, 前計算 O(nlogn), クエリ O(1) で処理する</para>
    /// </summary>

    [DebuggerDisplay(nameof(Length) + " = {" + nameof(Length) + "}")]
    [DebuggerTypeProxy(typeof(DisjointSparseTable<,>.DebugView))]
    public class DisjointSparseTable<TValue, TOp> where TOp : struct, ISparseTableOperator<TValue>
    {
        private static TOp op = default;
        protected readonly TValue[][] st;
        public int Length { get; }
        public DisjointSparseTable(TValue[] array)
        {
            Contract.Assert(array.Length > 0, nameof(array) + " must not be empty");
            Length = array.Length;
            st = new TValue[BitOperations.Log2((uint)Length) + 1][];
            st[0] = (TValue[])array.Clone();
            for (int i = 1; i < st.Length; i++)
            {
                var stp = st[i - 1];
                var sti = st[i] = new TValue[Length - (1 << i) + 1];
                for (int j = 0; j < sti.Length; j++)
                    sti[j] = op.Operate(stp[j], stp[j + (1 << (i - 1))]);
            }
            for (int h = 1; h < st.Length; h++)
            {
                int s = (1 << h);
                st[h] = new TValue[array.Length];
                for (int i = 0; i < array.Length; i += (s << 1))
                {
                    int t = Math.Min(i + s, array.Length);
                    st[h][t - 1] = array[t - 1];
                    for (int k = t - 2; k >= i; --k)
                        st[h][k] = op.Operate(array[k], st[h][k + 1]);
                    if (array.Length <= t)
                        break;
                    st[h][t] = array[t];
                    for (int k = t + 1; k < array.Length && k < t + s; ++k)
                        st[h][k] = op.Operate(st[h][k - 1], array[k]);
                }
            }
        }

        [凾(256)]
        public TValue Slice(int l, int length) => Prod(l, l + length);

        [凾(256)]
        public TValue Prod(int l, int r)
        {
            Contract.Assert((uint)l < (uint)Length, "l < Length");
            Contract.Assert((uint)r <= (uint)Length, "r <= Length");
            Contract.Assert(l < r, "l < r");
            if (l >= --r) return st[0][l];
            var h = BitOperations.Log2((uint)(l ^ r));
            return op.Operate(st[h][l], st[h][r]);
        }

#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        [DebuggerDisplay("{" + nameof(Value) + "}", Name = "{" + nameof(Key) + ",nq}")]
        private readonly struct DebugItem
        {
            public DebugItem(int l, int r, TValue value)
            {
                L = l;
                R = r;
                Value = value;
            }
            [DebuggerBrowsable(0)]
            public int L { get; }
            [DebuggerBrowsable(0)]
            public int R { get; }
            [DebuggerBrowsable(0)]
            public string Key => R - L == 1 ? $"[{L}]" : $"[{L}-{R})";
            public TValue Value { get; }
        }
#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        private class DebugView
        {
            private readonly DisjointSparseTable<TValue, TOp> st;
            public DebugView(DisjointSparseTable<TValue, TOp> st)
            {
                this.st = st;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var items = new List<DebugItem>(st.st.Length * st.Length);
                    for (int b = 0; b < st.st.Length; b++)
                    {
                        var len = 1 << b;
                        var stb = st.st[b];
                        for (int i = 0; i < stb.Length; i++)
                            items.Add(new DebugItem(i, i + len, stb[i]));
                    }
                    return items.ToArray();
                }
            }
        }
    }
}
