using AtCoder;
using AtCoder.Internal;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    using static SimdMontgomery;
    public readonly partial struct SimdStrassenImpl<T> where T : struct, IStaticMod
    {
        const int B = 1 << 7, B8 = B / 8;

        public readonly int S;
        public readonly int S8;
        public int VectorSize => S8 * S;
        readonly Vector256<uint> R, M1, M2;
        public SimdStrassenImpl(int length)
        {
            S = Math.Max(1 << InternalBit.CeilPow2(length), B);
            S8 = S / 8;
            R = Vector256.Create(MontgomeryModInt<T>.r);
            M1 = Vector256.Create(new T().Mod);
            M2 = Vector256.Create(new T().Mod * 2);
        }
        /// <summary>
        /// Strassen のアルゴリズムで行列の積を求める。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N^log_2(7))</para>
        /// </remarks>
        public Vector256<uint>[] Strassen(Vector256<uint>[] lhs, Vector256<uint>[] rhs)
        {
            var c = new Vector256<uint>[VectorSize];

            var len = c.Length * 3 / 2;

            var sb = ArrayPool<Vector256<uint>>.Shared.Rent(len);
            var tb = ArrayPool<Vector256<uint>>.Shared.Rent(len);
            var ub = ArrayPool<Vector256<uint>>.Shared.Rent(len);

            try
            {
                var s = sb.AsSpan(0, len);
                var t = tb.AsSpan(0, len);
                var u = ub.AsSpan(0, len);

                PlaceS(S, 0, 0, s, lhs);
                PlaceT(S, 0, 0, t, rhs);

                Strassen(S, s, t, u);

                PlaceRev(S, 0, 0, u, c);
                return c;
            }
            finally
            {
                ArrayPool<Vector256<uint>>.Shared.Return(sb);
                ArrayPool<Vector256<uint>>.Shared.Return(tb);
                ArrayPool<Vector256<uint>>.Shared.Return(ub);
            }
        }

        [凾(256 | 512)]
        void Strassen(int N,
            Span<Vector256<uint>> s,
            Span<Vector256<uint>> t,
            Span<Vector256<uint>> u)
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
            for (int i = 0; i < nx; i++) ps[i] = MontgomeryAdd(s[o11 + i], s[o22 + i], M2);
            for (int i = 0; i < nx; i++) pt[i] = MontgomeryAdd(t[o11 + i], t[o22 + i], M2);
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o11 + i] = pu[i]; u[o22 + i] = pu[i]; }

            // P2
            for (int i = 0; i < nx; i++) ps[i] = MontgomeryAdd(s[o21 + i], s[o22 + i], M2);
            for (int i = 0; i < nx; i++) pt[i] = t[o11 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o21 + i] = pu[i]; u[o22 + i] = MontgomerySubtract(u[o22 + i], pu[i], M2); }

            // P3
            for (int i = 0; i < nx; i++) ps[i] = s[o11 + i];
            for (int i = 0; i < nx; i++) pt[i] = MontgomerySubtract(t[o12 + i], t[o22 + i], M2);
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o12 + i] = pu[i]; u[o22 + i] = MontgomeryAdd(u[o22 + i], pu[i], M2); }

            // P4
            for (int i = 0; i < nx; i++) ps[i] = s[o22 + i];
            for (int i = 0; i < nx; i++) pt[i] = MontgomerySubtract(t[o21 + i], t[o11 + i], M2);
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o11 + i] = MontgomeryAdd(u[o11 + i], pu[i], M2); u[o21 + i] = MontgomeryAdd(u[o21 + i], pu[i], M2); }

            // P5
            for (int i = 0; i < nx; i++) ps[i] = MontgomeryAdd(s[o11 + i], s[o12 + i], M2);
            for (int i = 0; i < nx; i++) pt[i] = t[o22 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o11 + i] = MontgomerySubtract(u[o11 + i], pu[i], M2); u[o12 + i] = MontgomeryAdd(u[o12 + i], pu[i], M2); }

            // P6
            for (int i = 0; i < nx; i++) ps[i] = MontgomerySubtract(s[o21 + i], s[o11 + i], M2);
            for (int i = 0; i < nx; i++) pt[i] = MontgomeryAdd(t[o11 + i], t[o12 + i], M2);
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) u[o22 + i] = MontgomeryAdd(u[o22 + i], pu[i], M2);

            // P7
            for (int i = 0; i < nx; i++) ps[i] = MontgomerySubtract(s[o12 + i], s[o22 + i], M2);
            for (int i = 0; i < nx; i++) pt[i] = MontgomeryAdd(t[o21 + i], t[o22 + i], M2);
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) u[o11 + i] = MontgomeryAdd(u[o11 + i], pu[i], M2);
        }

        [凾(256 | 512)]
        public void PlaceRev(int N, int a, int b, Span<Vector256<uint>> dst, Span<Vector256<uint>> src)
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
        public void PlaceS(int N, int a, int b, Span<Vector256<uint>> dst, Span<Vector256<uint>> src)
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
        public void PlaceT(int N, int a, int b, Span<Vector256<uint>> dst, Span<Vector256<uint>> src)
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


        [凾(256)]
        public Vector256<uint>[] ToVectorize(ReadOnlySpan<MontgomeryModInt<T>> v, int h, int w)
        {
            var s8 = S8;
            var rt = new Vector256<uint>[VectorSize];
            for (int i = h - 1; i >= 0; i--)
            {
                var src = v.Slice(i * w, w);
                var dst = MemoryMarshal.Cast<Vector256<uint>, MontgomeryModInt<T>>(rt.AsSpan(i * s8));
                src.CopyTo(dst);
            }
            return rt;
        }

        [凾(256)]
        public MontgomeryModInt<T>[] ToMatrix(Vector256<uint>[] c, int h, int w)
        {
            Debug.Assert(h * w <= c.Length * 8);
            var s8 = S8;
            var rt = new MontgomeryModInt<T>[h * w];
            for (int i = 0; i < h; i++)
            {
                var src = MemoryMarshal.Cast<Vector256<uint>, MontgomeryModInt<T>>(c.AsSpan(s8 * i));
                var dst = rt.AsSpan(i * w, w);
                src[..dst.Length].CopyTo(dst);
            }
            return rt;
        }
    }
}
