using AtCoder;

namespace Kzrnm.Competitive
{
    public class IntFenwickTree2D : FenwickTree2D<int, IntOperator>
    {
        public IntFenwickTree2D(int H, int W) : base(H, W) { }
    }

    public class LongFenwickTree2D : FenwickTree2D<long, LongOperator>
    {
        public LongFenwickTree2D(int H, int W) : base(H, W) { }
    }
}