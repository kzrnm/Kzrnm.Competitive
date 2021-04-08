using AtCoder;

namespace Kzrnm.Competitive
{
    public static class __MathExtension
    {
        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        public static long Pow(this long x, long y) => MathLibGeneric.Pow<long, LongOperator>(x, y);

    }
}
