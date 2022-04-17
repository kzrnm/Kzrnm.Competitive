using AtCoder;
using AtCoder.Operators;

namespace Kzrnm.Competitive
{
    public static class 最大流
    {
        public static MfGraph<T, TOp> ToMFGraph<T, TOp>(this WGraphBuilder<T, TOp> gb)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            var mfg = new MfGraph<T, TOp>(gb.edgeContainer.Length);
            foreach (var (i, e) in gb.edgeContainer.edges)
                mfg.AddEdge(i, e.To, e.Value);
            return mfg;
        }
    }
}