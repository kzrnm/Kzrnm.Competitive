using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static partial class __MatrixProdStrassen_StaticMod
    {
        readonly struct R { }
        /// <summary>
        /// Strassen のアルゴリズムで行列の積を求める。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N^log_2(7))</para>
        /// </remarks>
        public static ArrayMatrix<StaticModInt<T>> Strassen<T>(this ArrayMatrix<StaticModInt<T>> mat1, ArrayMatrix<StaticModInt<T>> mat2) where T : struct, IStaticMod
        {
            if (mat1.kind != ArrayMatrixKind.Normal || mat2.kind != ArrayMatrixKind.Normal)
                return mat1 * mat2;

            VectorizedModInt<R>.SetMod(new T().Mod);
            var impl = new StrassenImpl<R>(Math.Max(Math.Max(mat1.Height, mat2.Height), Math.Max(mat1.Width, mat2.Width)));
            var rt = impl.Strassen(ToVectorize(mat1, impl.VectorSize, impl.S8), ToVectorize(mat2, impl.VectorSize, impl.S8));
            return ToMatrix<T>(rt, mat1.Height, mat2.Width, impl.S8);
        }

        [凾(256)]
        static VectorizedModInt<R>[] ToVectorize<T>(ArrayMatrix<StaticModInt<T>> m, int size, int s8) where T : struct, IStaticMod
        {
            var v = m.Value;
            var w = m.Width;
            var rt = new VectorizedModInt<R>[size];
            for (int i = m.Height - 1; i >= 0; i--)
            {
                MemoryMarshal.Cast<StaticModInt<T>, uint>(v.AsSpan(i * w, w)).CopyTo(
                MemoryMarshal.Cast<VectorizedModInt<R>, uint>(rt.AsSpan(i * s8)));
            }
            return rt;
        }

        [凾(256)]
        static ArrayMatrix<StaticModInt<T>> ToMatrix<T>(VectorizedModInt<R>[] c, int h, int w, int s8) where T : struct, IStaticMod
        {
            Debug.Assert(h * w <= c.Length * 8);
            var rt = new StaticModInt<T>[h * w];
            for (int i = 0; i < h; i++)
            {
                var src = MemoryMarshal.Cast<VectorizedModInt<R>, uint>(c.AsSpan(s8 * i));
                var dst = MemoryMarshal.Cast<StaticModInt<T>, uint>(rt.AsSpan(i * w, w));
                src[..dst.Length].CopyTo(dst);
            }
            return new(rt, h, w);
        }
    }
}
