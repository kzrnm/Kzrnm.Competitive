using AtCoder;

namespace Kzrnm.Competitive
{
    public class StaticModIntFenwickTree2D<T> : FenwickTree2D<StaticModInt<T>, StaticModIntOperator<T>>
        where T : struct, IStaticMod
    {
        public StaticModIntFenwickTree2D(int H, int W) : base(H, W) { }
    }
}