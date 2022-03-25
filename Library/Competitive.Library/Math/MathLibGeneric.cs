using AtCoder.Operators;
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
        public static T Pow<T, TOp>(T x, long y)
              where TOp : struct, IMultiplicationOperator<T>
        {
            var op = new TOp();
            T res = ((y & 1) != 0) ? x : op.MultiplyIdentity;
            for (y >>= 1; y > 0; y >>= 1)
            {
                x = op.Multiply(x, x);
                if ((y & 1) != 0)
                    res = op.Multiply(res, x);
            }
            return res;
        }
    }
}
