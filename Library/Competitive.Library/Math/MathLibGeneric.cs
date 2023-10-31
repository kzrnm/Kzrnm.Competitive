using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class MathLibGeneric
    {
        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log <paramref name="y"/>)</para>
        /// </remarks>
        [凾(256)]
        public static T Pow<T>(this T x, long y)
              where T : IMultiplyOperators<T, T, T>, IMultiplicativeIdentity<T, T>
        {
            T res = ((y & 1) != 0) ? x : T.MultiplicativeIdentity;
            for (y >>= 1; y > 0; y >>= 1)
            {
                x *= x;
                if ((y & 1) != 0)
                    res *= x;
            }
            return res;
        }
    }
}
