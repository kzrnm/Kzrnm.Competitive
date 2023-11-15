using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        public static ArrayMatrix<MontgomeryModInt<T>> Strassen<T>(this ArrayMatrix<MontgomeryModInt<T>> mat1, ArrayMatrix<MontgomeryModInt<T>> mat2) where T : struct, IStaticMod
        {
            if (mat1.kind != ArrayMatrixKind.Normal || mat2.kind != ArrayMatrixKind.Normal)
                return mat1 * mat2;

            var impl = new StrassenImpl<T>(Math.Max(Math.Max(mat1.Height, mat2.Height), Math.Max(mat1.Width, mat2.Width)));
            var rt = impl.Strassen(mat1, mat2);
            return impl.ToMatrix(rt, mat1.Height, mat2.Width);
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
                x = x.Multiply(x);
                if ((y & 1) != 0)
                    res = res.Multiply(x);
            }
            return res;
        }
    }
}
