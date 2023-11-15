using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static partial class __MatrixProdStrassen_ModInt
    {
        /// <summary>
        /// Strassen のアルゴリズムで行列の積を求める。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N^log_2(7))</para>
        /// </remarks>
        public static ArrayMatrix<MontgomeryModInt<T>> Strassen<T>(this ArrayMatrix<MontgomeryModInt<T>> m1, ArrayMatrix<MontgomeryModInt<T>> m2) where T : struct, IStaticMod
        {
            if (m1.kind != ArrayMatrixKind.Normal || m2.kind != ArrayMatrixKind.Normal)
                return m1 * m2;

            var rh = m1.Height;
            var rw = m2.Width;
            var impl = new StrassenImpl<T>(Math.Max(Math.Max(rh, m2.Height), Math.Max(m1.Width, rw)));
            var rt = impl.Strassen(impl.ToVectorize(m1.AsSpan(), rh, m1.Width), impl.ToVectorize(m2.AsSpan(), m2.Height, rw));
            return new(impl.ToMatrix(rt, rh, rw), rh, rw);
        }

        /// <summary>
        /// <paramref name="x"/> の <paramref name="y"/> 乗
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log <paramref name="y"/>)</para>
        /// </remarks>
        [凾(256)]

        public static ArrayMatrix<MontgomeryModInt<T>> Pow<T>(this ArrayMatrix<MontgomeryModInt<T>> x, long y) where T : struct, IStaticMod
        {
            var res = ((y & 1) != 0) ? x : ArrayMatrix<MontgomeryModInt<T>>.Identity;
            for (y >>= 1; y > 0; y >>= 1)
            {
                x = x.Strassen(x);
                if ((y & 1) != 0)
                    res = res.Strassen(x);
            }
            return res;
        }
    }
}
