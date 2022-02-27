using AtCoder;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __MathExtension
    {
        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        [凾(256)]
        public static long Pow(this long x, long y) => MathLibGeneric.Pow<long, LongOperator>(x, y);
    }
}
