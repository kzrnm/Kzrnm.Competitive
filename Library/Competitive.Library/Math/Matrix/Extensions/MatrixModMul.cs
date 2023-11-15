using AtCoder;
using AtCoder.Internal;
using System;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    public static class __MatrixModMul
    {
        [凾(256)]
        public static ArrayMatrix<T> Multiply<T>(this ArrayMatrix<T> lhs, ArrayMatrix<T> rhs) where T : IModInt<T>
        {
            if (lhs.kind != Kd.Normal || rhs.kind != Kd.Normal)
                return lhs * rhs;
            var rh = lhs.Height;
            var rw = rhs.Width;
            var mid = lhs.Width;
            Contract.Assert(lhs.Width == rhs.Height);
            var res = new T[rh * rw];
            if (res.Length == 0) return new();

            ref var rp = ref res[0];
            ref var tp = ref lhs.Value[0];
            ref var op = ref rhs.Value[0];

            for (int i = 0; i < rh; i++)
                for (int j = 0; j < rw; j++)
                {
                    ulong v = 0;
                    for (var k = 0; k < mid; k++)
                    {
                        v += (uint)(Unsafe.Add(ref tp, i * mid + k) * Unsafe.Add(ref op, k * rw + j)).Value;
                    }
                    Unsafe.Add(ref rp, i * rw + j) = T.CreateTruncating(v);
                }
            return new(res, rh, rw);
        }

        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log <paramref name="y"/>)</para>
        /// </remarks>
        [凾(256)]
        public static ArrayMatrix<T> Pow<T>(this ArrayMatrix<T> x, long y) where T : IModInt<T>
        {
            var res = ((y & 1) != 0) ? x : ArrayMatrix<T>.Identity;
            for (y >>= 1; y > 0; y >>= 1)
            {
                x = x.Multiply(x);
                if ((y & 1) != 0)
                    res = res.Multiply(x);
            }
            return res;
        }
    }
}
