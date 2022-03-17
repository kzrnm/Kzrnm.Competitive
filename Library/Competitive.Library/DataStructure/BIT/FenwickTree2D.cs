using AtCoder.Internal;
using AtCoder.Operators;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 長さ H*W の2次元配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>要素の 1 点変更</description>
    /// </item>
    /// <item>
    /// <description>矩形の要素の総和</description>
    /// </item>
    /// </list>
    /// <para>を O(log^2 ⁡N) で求めることが出来るデータ構造です。</para>
    /// </summary>
    /// <typeparam name="T">配列要素の型</typeparam>
    /// <typeparam name="TOp">配列要素の操作を表す型</typeparam>
    public class FenwickTree2D<T, TOp>
        where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
    {
        private static readonly TOp op = default;

        [凾(256)]
        public void Add(int h, int w, T v)
        {
            for (var hh = h + 1; hh < tree.Length; hh += (int)InternalBit.ExtractLowestSetBit(hh))
                for (var ww = w + 1; ww < tree[hh].Length; ww += (int)InternalBit.ExtractLowestSetBit(ww))
                    tree[hh][ww] = op.Add(tree[hh][ww], v);
        }

        [凾(256)]
        public T Sum(int hExclusive, int wExclusive)
        {
            T res = default;
            for (var h = hExclusive; h > 0; h &= h - 1)
                for (var w = wExclusive; w > 0; w &= w - 1)
                    res = op.Add(res, tree[h][w]);
            return res;
        }

        readonly T[][] tree;
        /// <summary>
        /// 要素の縦方向の長さを返します。
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// 要素の横方向の長さを返します。
        /// </summary>
        public int Width { get; }
        [凾(256)]
        public Slicer Slice(int from, int length) => new Slicer(this, from, from + length);

        public FenwickTree2D(int H, int W)
        {
            Length = H;
            Width = W;
            tree = new T[H + 1][];
            for (int i = 0; i < tree.Length; i++) tree[i] = new T[W + 1];
        }

        public ref struct Slicer
        {
            readonly FenwickTree2D<T, TOp> fw;
            readonly int hFrom;
            readonly int hToExclusive;
            public int Length { get; }
            public Slicer(FenwickTree2D<T, TOp> fw, int hFrom, int hToExclusive)
            {
                this.fw = fw;
                this.hFrom = hFrom;
                this.hToExclusive = hToExclusive;
                this.Length = fw.Width - 1;
            }
            [凾(256)]
            public T Slice(int wFrom, int length)
            {
                var wToExclusive = wFrom + length;
                return
                    op.Add(
                        op.Subtract(
                            op.Subtract(
                                fw.Sum(hToExclusive, wToExclusive)
                                , fw.Sum(hToExclusive, wFrom))
                            , fw.Sum(hFrom, wToExclusive))
                        , fw.Sum(hFrom, wFrom));
            }
        }
    }
}