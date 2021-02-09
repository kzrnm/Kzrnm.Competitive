using AtCoder;
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
        public static T Pow<T, TOp>(T x, long y)
            where TOp : struct, IMultiplicationOperator<T>
        {
            var op = default(TOp);
            T res = op.MultiplyIdentity;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1)
                    res = op.Multiply(res, x);
                x = op.Multiply(x, x);
            }
            return res;
        }
    }
}
