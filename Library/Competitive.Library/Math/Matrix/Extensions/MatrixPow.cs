using AtCoder.Internal;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __MatrixPow
    {
        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log <paramref name="y"/>)</para>
        /// </remarks>
        [凾(256)]
        public static ArrayMatrix<T> Pow<T>(this ArrayMatrix<T> x, long y) where T : INumberBase<T>
        {
            Contract.Assert(x.Height == x.Width);
            var m = MathLibGeneric.Pow(x, y);
            if (m == ArrayMatrix<T>.Identity)
                return ArrayMatrix<T>.NormalIdentity(x.Height);
            return m;
        }
    }
}
