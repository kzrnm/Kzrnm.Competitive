using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static partial class __MatrixProdStrassen_GenericModInt
    {
        readonly struct R { }
        /// <summary>
        /// Strassen のアルゴリズムで行列の積を求める。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N^log_2(7))</para>
        /// </remarks>
        public static ArrayMatrix<T> Strassen<T>(this ArrayMatrix<T> mat1, ArrayMatrix<T> mat2) where T : IModInt<T>
        {
            if (mat1.kind != ArrayMatrixKind.Normal || mat2.kind != ArrayMatrixKind.Normal)
                return mat1 * mat2;

            VectorizedModInt<R>.SetMod((uint)T.Mod);
            var impl = new StrassenImpl<R>(Math.Max(Math.Max(mat1.Height, mat2.Height), Math.Max(mat1.Width, mat2.Width)));
            var rt = impl.Strassen(ToVectorize(mat1, impl.VectorSize, impl.S8), ToVectorize(mat2, impl.VectorSize, impl.S8));
            return ToMatrix<T>(rt, mat1.Height, mat2.Width, impl.S8);
        }

        [凾(256)]
        static VectorizedModInt<R>[] ToVectorize<T>(ArrayMatrix<T> m, int size, int s8) where T : IModInt<T>
        {
            var v = m.Value;
            var w = m.Width;
            var rt = new VectorizedModInt<R>[size];
            for (int i = m.Height - 1; i >= 0; i--)
            {
                var src = v.AsSpan(i * w, w);
                var dst = MemoryMarshal.Cast<VectorizedModInt<R>, int>(rt.AsSpan(i * s8));
                for (int j = 0; j < src.Length; j++)
                    dst[j] = src[j].Value;
            }
            return rt;
        }

        [凾(256)]
        static ArrayMatrix<T> ToMatrix<T>(VectorizedModInt<R>[] c, int h, int w, int s8) where T : IModInt<T>
        {
            Debug.Assert(h * w <= c.Length * 8);
            var rt = new T[h * w];
            for (int i = 0; i < h; i++)
            {
                var src = MemoryMarshal.Cast<VectorizedModInt<R>, uint>(c.AsSpan(s8 * i));
                var dst = rt.AsSpan(i * w, w);
                for (int j = 0; j < dst.Length; j++)
                    dst[j] = T.CreateTruncating(src[j]);
            }
            return new(rt, h, w);
        }
    }
}
