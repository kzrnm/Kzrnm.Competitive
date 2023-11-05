using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static partial class __MatrixProdStrassen_Montgomery
    {
        /// <summary>
        /// Strassen のアルゴリズムで行列の積を求める。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N^log_2(7))</para>
        /// </remarks>
        public static ArrayMatrix<MontgomeryModInt<T>> Strassen<T>(
            this ArrayMatrix<MontgomeryModInt<T>> mat1,
            ArrayMatrix<MontgomeryModInt<T>> mat2) where T : struct, IStaticMod
        {
            if (mat1.kind != ArrayMatrixKind.Normal || mat2.kind != ArrayMatrixKind.Normal)
                return mat1 * mat2;

            var impl = new StrassenImpl<T>(Math.Max(Math.Max(mat1.Height, mat2.Height), Math.Max(mat1.Width, mat2.Width)));
            var rt = impl.Strassen(ToVectorize(mat1, impl.VectorSize, impl.S8), ToVectorize(mat2, impl.VectorSize, impl.S8));
            return ToMatrix(rt, mat1.Height, mat2.Width, impl.S8);
        }

        [凾(256)]
        static VectorizedModInt<T>[] ToVectorize<T>(ArrayMatrix<MontgomeryModInt<T>> m, int size, int s8) where T : struct, IStaticMod
        {
            var v = m.Value;
            var w = m.Width;
            var rt = new VectorizedModInt<T>[size];
            for (int i = m.Height - 1; i >= 0; i--)
            {
                var src = v.AsSpan(i * w, w);
                ref var d = ref MemoryMarshal.GetReference(MemoryMarshal.Cast<VectorizedModInt<T>, int>(rt.AsSpan(i * s8)));
                for (int j = 0; j < src.Length; j++)
                {
                    Unsafe.Add(ref d, j) = src[j].Value;
                }
            }
            return rt;
        }

        [凾(256)]
        static ArrayMatrix<MontgomeryModInt<T>> ToMatrix<T>(VectorizedModInt<T>[] c, int h, int w, int s8) where T : struct, IStaticMod
        {
            Debug.Assert(h * w <= c.Length * 8);
            var rt = new MontgomeryModInt<T>[h * w];
            for (int i = 0; i < h; i++)
            {
                ref var s = ref MemoryMarshal.GetReference(MemoryMarshal.Cast<VectorizedModInt<T>, int>(c.AsSpan(i * s8)));
                var dst = rt.AsSpan(i * w, w);
                for (int j = 0; j < dst.Length; j++)
                {
                    dst[j] = Unsafe.Add(ref s, j);
                }
            }
            return new(rt, h, w);
        }
    }
}