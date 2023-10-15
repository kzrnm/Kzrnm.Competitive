using AtCoder.Internal;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// <para> H × W の2次元配列に対する冪等半群に対する区間クエリを, 前計算 O(HW log HW), クエリ O(1) で処理する</para>
    /// <para>冪等: 操作を何回行っても集合が等しければ等しくなる性質(最小値・最大値など)</para>
    /// </summary>

    [DebuggerDisplay("H = {" + nameof(Length) + "}, W = {" + nameof(st) + "[0][0]." + nameof(Grid<TValue>.W) + "}")]
    [DebuggerTypeProxy(typeof(SparseTable2D<,>.DebugView))]
    public class SparseTable2D<TValue, TOp> where TOp : struct, ISparseTableOperator<TValue>
    {
        private static TOp op = default;
        private readonly Grid<TValue>[][] st;
        public int Length { get; }

        private static Grid<TValue> ToGrid(TValue[][] array)
        {
            Contract.Assert(array.Length > 0 && array[0].Length > 0, nameof(array) + " must not be empty");
            var g = new Grid<TValue>(array.Length, array[0].Length);
            for (int i = 0; i < array.Length; i++)
                for (int j = 0; j < array[i].Length; j++)
                    g[i, j] = array[i][j];
            return g;
        }
        public SparseTable2D(TValue[][] array) : this(ToGrid(array)) { }
        public SparseTable2D(Grid<TValue> grid)
        {
            Contract.Assert(grid.H > 0 && grid.W > 0, nameof(grid) + " must not be empty");
            int H = grid.H;
            int W = grid.W;
            Length = H;
            st = new Grid<TValue>[BitOperations.Log2((uint)H) + 1][];

            var lw = BitOperations.Log2((uint)W) + 1;
            for (int i = 0; i < st.Length; i++)
                st[i] = new Grid<TValue>[lw];

            st[0][0] = grid.Clone();

            for (int k = 1; k < st.Length; k++)
            {
                var stp = st[k - 1][0];
                var sti = st[k][0] = new Grid<TValue>(H - (1 << k) + 1, W, grid.defaultValue);
                for (int i = 0; i < sti.H; i++)
                    for (int j = 0; j < W; j++)
                        sti[i, j] = op.Operate(stp[i, j], stp[i + (1 << (k - 1)), j]);
            }
            for (int k = 0; k < st.Length; k++)
            {
                for (int l = 1; l < st[k].Length; ++l)
                {
                    var stp = st[k][l - 1];
                    var sti = st[k][l] = new Grid<TValue>(H - (1 << k) + 1, W - (1 << l) + 1, grid.defaultValue);
                    for (int i = 0; i < sti.H; i++)
                        for (int j = 0; j < sti.W; j++)
                            sti[i, j] = op.Operate(stp[i, j], stp[i, j + (1 << (l - 1))]);
                }
            }
        }


        [凾(256)]
        public TValue Prod(int lh, int rh, int lw, int rw)
        {
            Contract.Assert((uint)lh < (uint)st[0][0].H, "l < H");
            Contract.Assert((uint)rh <= (uint)st[0][0].H, "r <= H");
            Contract.Assert((uint)lw < (uint)st[0][0].W, "l < W");
            Contract.Assert((uint)rw <= (uint)st[0][0].W, "r <= W");
            Contract.Assert(lh < rh, "lh < rh");
            Contract.Assert(lw < rw, "lw < rw");
            var bh = BitOperations.Log2((uint)(rh - lh));
            var bw = BitOperations.Log2((uint)(rw - lw));
            var stb = st[bh][bw];
            return op.Operate(
                op.Operate(stb[lh, lw], stb[lh, rw - (1 << bw)]),
                op.Operate(stb[rh - (1 << bh), lw], stb[rh - (1 << bh), rw - (1 << bw)])
            );
        }

        [凾(256)]
        public Slicer Slice(int lh, int length) => new Slicer(this, lh, lh + length);
        public readonly ref struct Slicer
        {
            readonly SparseTable2D<TValue, TOp> impl;
            readonly int lh;
            readonly int rh;
            public int Length { get; }
            [凾(256)]
            public Slicer(SparseTable2D<TValue, TOp> impl, int l, int r)
            {
                this.impl = impl;
                lh = l;
                rh = r;
                Length = impl.st[0][0].W;
            }
            [凾(256)]
            public TValue Slice(int lw, int length)
            {
                var rw = lw + length;
                return impl.Prod(lh, rh, lw, rw);
            }
        }
#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
        private class DebugView
        {
            private readonly SparseTable2D<TValue, TOp> st;
            public DebugView(SparseTable2D<TValue, TOp> st)
            {
                this.st = st;
            }
            public Grid<TValue> Root => st.st[0][0];
        }
    }
}
