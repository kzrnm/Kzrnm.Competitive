using AtCoder;
using AtCoder.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __MatrixPowSimdMod
    {
        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log <paramref name="y"/>)</para>
        /// </remarks>
        [凾(256)]
        public static SimdModMatrix<T> Pow<T>(this SimdModMatrix<T> x, long y) where T : struct, IStaticMod
        {
            Contract.Assert(x.Height == x.Width);
            var m = MathLibGeneric.Pow(x, y);
            if (m == SimdModMatrix<T>.Identity)
                return SimdModMatrix<T>.NormalIdentity(x.Height);
            return m;
        }
    }
}
