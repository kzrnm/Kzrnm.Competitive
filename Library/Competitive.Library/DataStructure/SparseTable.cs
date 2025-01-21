using AtCoder.Internal;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// <para>冪等半群に対する区間クエリを, 前計算 O(nlogn), クエリ O(1) で処理する</para>
    /// <para>冪等: 操作を何回行っても集合が等しければ等しくなる性質(最小値・最大値など)</para>
    /// </summary>
    [DebuggerDisplay(nameof(Length) + " = {" + nameof(Length) + "}")]
    [DebuggerTypeProxy(typeof(SparseTable<,>.DebugView))]
    public class SparseTable<TValue, TOp> where TOp : struct, ISparseTableOperator<TValue>
    {
        private static TOp op = default;
        protected readonly TValue[][] st;
        public int Length { get; }
        public SparseTable(TValue[] array)
        {
            Contract.Assert(array.Length > 0, nameof(array) + " must not be empty");
            Length = array.Length;
            st = new TValue[BitOperations.Log2((uint)Length) + 1][];
            st[0] = (TValue[])array.Clone();
            for (int k = 1; k < st.Length; k++)
            {
                var stp = st[k - 1];
                var sti = st[k] = new TValue[Length - (1 << k) + 1];
                var d = 1 << (k - 1);
                for (int i = 0; i < sti.Length; i++)
                    sti[i] = op.Operate(stp[i], stp[i + d]);
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
            var b = BitOperations.Log2((uint)(r - l));
            var stb = st[b];
            return op.Operate(stb[l], stb[r - (1 << b)]);
        }

        [SourceExpander.NotEmbeddingSource]
        [DebuggerDisplay("{" + nameof(Value) + "}", Name = "{" + nameof(Key) + ",nq}")]
        private readonly record struct DebugItem(
            [property: DebuggerBrowsable(0)] int L,
            [property: DebuggerBrowsable(0)] int R,
            TValue Value)
        {

            [DebuggerBrowsable(0)]
            public string Key => R - L == 1 ? $"[{L}]" : $"[{L}-{R})";
        }
        [SourceExpander.NotEmbeddingSource]
        private class DebugView
        {
            private readonly SparseTable<TValue, TOp> st;
            public DebugView(SparseTable<TValue, TOp> st)
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
