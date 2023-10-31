using AtCoder;
using System.Numerics;

namespace Kzrnm.Competitive
{
    public static class 最大流
    {
        public static MfGraph<T> ToMFGraph<T>(this WGraphBuilder<T> gb)
            where T : INumber<T>, IMinMaxValue<T>
        {
            var mfg = new MfGraph<T>(gb.edgeContainer.Length);
            foreach (var (i, e) in gb.edgeContainer.edges)
                mfg.AddEdge(i, e.To, e.Value);
            return mfg;
        }
    }
}