using AtCoder;
using AtCoder.Internal;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Kd = Internal.ArrayMatrixKind;
    public static partial class __MatrixProdStrassen
    {
        private const int B = 1 << 7;
        private const int B8 = B / 8;
        /// <summary>
        /// Strassen のアルゴリズムで行列の積を求める。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N^log_2(7))</para>
        /// </remarks>
        public static ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> Strassen<T>(
            this ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> mat1,
            ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> mat2) where T : struct, IStaticMod
        {
            if (mat1.kind != Kd.Normal || mat2.kind != Kd.Normal)
                return mat1 * mat2;

            var impl = new Impl<T>(Math.Max(Math.Max(mat1.Height, mat2.Height), Math.Max(mat1.Width, mat2.Width)));
            return impl.Strassen(mat1, mat2);
        }
        public readonly partial struct Impl<T> where T : struct, IStaticMod
        {
            public readonly int S;
            public readonly int S8;
            public Impl(int length)
            {
                S = Math.Max(1 << InternalBit.CeilPow2(length), B);
                S8 = S / 8;
            }
            public ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> Strassen(ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> mat1, ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> mat2)
            {
                Contract.Assert(new T().Mod % 2 == 1);
                Contract.Assert(mat1.Height <= S);
                Contract.Assert(mat2.Height <= S);
                Contract.Assert(mat1.Width <= S);
                Contract.Assert(mat2.Width <= S);
                var a = ToVectorize(mat1);
                var b = ToVectorize(mat2);
                var c = new VectorizedStaticModInt<T>[S8 * S];

                var s = new VectorizedStaticModInt<T>[S8 * S * 3 / 2];
                var t = new VectorizedStaticModInt<T>[S8 * S * 3 / 2];
                var u = new VectorizedStaticModInt<T>[S8 * S * 3 / 2];

                PlaceS(S, 0, 0, s.AsSpan(), a);
                PlaceT(S, 0, 0, t.AsSpan(), b);
                for (int i = 0; i < S * S8; i++) s[i] = s[i].Itom();
                for (int i = 0; i < S * S8; i++) t[i] = t[i].Itom();

                StrassenImpl(S, s, t, u.AsSpan());

                for (int i = 0; i < S * S8; i++) u[i] = u[i].Mtoi();
                PlaceRev(S, 0, 0, u.AsSpan(), c);
                return ToMatrix(c, mat1.Height, mat2.Width);
            }
            [凾(256)]
            private ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> ToMatrix(VectorizedStaticModInt<T>[] c, int h, int w)
            {
                Debug.Assert(h * w <= c.Length * 8);
                var rt = new StaticModInt<T>[h * w];
                for (int i = 0; i < h; i++)
                {
                    var src = MemoryMarshal.Cast<VectorizedStaticModInt<T>, uint>(c.AsSpan(S8 * i));
                    var dst = MemoryMarshal.Cast<StaticModInt<T>, uint>(rt.AsSpan(i * w, w));
                    src[..dst.Length].CopyTo(dst);
                }
                return new ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>>(rt, h, w);
            }
            [凾(256 | 512)]
            private void StrassenImpl(int N,
                Span<VectorizedStaticModInt<T>> s,
                Span<VectorizedStaticModInt<T>> t,
                Span<VectorizedStaticModInt<T>> u)
            {
                if (N <= B)
                {
                    MulSimd(s, t, u);
                    return;
                }

                var ps = s[(N * N / 8)..];
                var pt = t[(N * N / 8)..];
                var pu = u[(N * N / 8)..];
                int nx = N * N / 32;
                int o11 = nx * 0, o12 = nx * 1, o21 = nx * 2, o22 = nx * 3;

                // P1
                for (int i = 0; i < nx; i++) ps[i] = s[o11 + i] + s[o22 + i];
                for (int i = 0; i < nx; i++) pt[i] = t[o11 + i] + t[o22 + i];
                StrassenImpl(N / 2, ps, pt, pu);
                for (int i = 0; i < nx; i++) { u[o11 + i] = pu[i]; u[o22 + i] = pu[i]; }

                // P2
                for (int i = 0; i < nx; i++) ps[i] = s[o21 + i] + s[o22 + i];
                for (int i = 0; i < nx; i++) pt[i] = t[o11 + i];
                StrassenImpl(N / 2, ps, pt, pu);
                for (int i = 0; i < nx; i++) { u[o21 + i] = pu[i]; u[o22 + i] -= pu[i]; }

                // P3
                for (int i = 0; i < nx; i++) ps[i] = s[o11 + i];
                for (int i = 0; i < nx; i++) pt[i] = t[o12 + i] - t[o22 + i];
                StrassenImpl(N / 2, ps, pt, pu);
                for (int i = 0; i < nx; i++) { u[o12 + i] = pu[i]; u[o22 + i] += pu[i]; }

                // P4
                for (int i = 0; i < nx; i++) ps[i] = s[o22 + i];
                for (int i = 0; i < nx; i++) pt[i] = t[o21 + i] - t[o11 + i];
                StrassenImpl(N / 2, ps, pt, pu);
                for (int i = 0; i < nx; i++) { u[o11 + i] += pu[i]; u[o21 + i] += pu[i]; }

                // P5
                for (int i = 0; i < nx; i++) ps[i] = s[o11 + i] + s[o12 + i];
                for (int i = 0; i < nx; i++) pt[i] = t[o22 + i];
                StrassenImpl(N / 2, ps, pt, pu);
                for (int i = 0; i < nx; i++) { u[o11 + i] -= pu[i]; u[o12 + i] += pu[i]; }

                // P6
                for (int i = 0; i < nx; i++) ps[i] = s[o21 + i] - s[o11 + i];
                for (int i = 0; i < nx; i++) pt[i] = t[o11 + i] + t[o12 + i];
                StrassenImpl(N / 2, ps, pt, pu);
                for (int i = 0; i < nx; i++) u[o22 + i] += pu[i];

                // P7
                for (int i = 0; i < nx; i++) ps[i] = s[o12 + i] - s[o22 + i];
                for (int i = 0; i < nx; i++) pt[i] = t[o21 + i] + t[o22 + i];
                StrassenImpl(N / 2, ps, pt, pu);
                for (int i = 0; i < nx; i++) u[o11 + i] += pu[i];
            }

            [凾(256 | 512)]
            private VectorizedStaticModInt<T>[] ToVectorize(ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>> m)
            {
                var v = m.Value;
                var w = m.Width;
                var rt = new VectorizedStaticModInt<T>[S8 * S];
                for (int i = m.Height - 1; i >= 0; i--)
                {
                    MemoryMarshal.Cast<StaticModInt<T>, uint>(v.AsSpan(i * w, w)).CopyTo(
                    MemoryMarshal.Cast<VectorizedStaticModInt<T>, uint>(rt.AsSpan(i * S8)));
                }
                return rt;
            }

            [凾(256 | 512)]
            public void PlaceRev(int N, int a, int b, Span<VectorizedStaticModInt<T>> dst, Span<VectorizedStaticModInt<T>> src)
            {
                if (N > B)
                {
                    int nx = N * N / 32, M = N / 2;
                    PlaceRev(M, a + 0, b + 0, dst, src);
                    PlaceRev(M, a + 0, b + M, dst[nx..], src);
                    PlaceRev(M, a + M, b + 0, dst[(2 * nx)..], src);
                    PlaceRev(M, a + M, b + M, dst[(3 * nx)..], src);
                }
                else
                {
                    for (int i = 0; i < B; i++)
                        dst.Slice(i * B8, B8).CopyTo(src.Slice((a + i) * S8 + b / 8, B8));
                }
            }


            [凾(256 | 512)]
            public void PlaceS(int N, int a, int b, Span<VectorizedStaticModInt<T>> dst, Span<VectorizedStaticModInt<T>> src)
            {
                if (N > B)
                {
                    int nx = N * N / 32, M = N / 2;
                    PlaceS(M, a + 0, b + 0, dst, src);
                    PlaceS(M, a + 0, b + M, dst[nx..], src);
                    PlaceS(M, a + M, b + 0, dst[(2 * nx)..], src);
                    PlaceS(M, a + M, b + M, dst[(3 * nx)..], src);
                }
                else
                {
                    for (int i = 0; i < B; i++)
                        src.Slice((a + i) * S8 + b / 8, B8).CopyTo(dst.Slice(i * B8, B8));
                }
            }

            [凾(256 | 512)]
            public void PlaceT(int N, int a, int b, Span<VectorizedStaticModInt<T>> dst, Span<VectorizedStaticModInt<T>> src)
            {
                if (N > B)
                {
                    int nx = N * N / 32, M = N / 2;
                    PlaceT(M, a + 0, b + 0, dst, src);
                    PlaceT(M, a + 0, b + M, dst[nx..], src);
                    PlaceT(M, a + M, b + 0, dst[(2 * nx)..], src);
                    PlaceT(M, a + M, b + M, dst[(3 * nx)..], src);
                }
                else
                {
                    // t : 逆順に配置。すなわちb_{k,j}をt[j * B + k]に配置する。
                    for (int k = 0; k < B; k++)
                        for (int j = 0; j < B8; j++)
                            dst[j * B + k] = src[(a + k) * S8 + j + b / 8];
                }
            }
        }
    }
}
