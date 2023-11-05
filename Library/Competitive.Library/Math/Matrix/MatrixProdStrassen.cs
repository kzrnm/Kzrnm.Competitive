using AtCoder;
using AtCoder.Internal;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    public readonly partial struct StrassenImpl<T> where T : struct, IStaticMod
    {
        private const int B = 1 << 7;
        private const int B8 = B / 8;

        public readonly int S;
        public readonly int S8;
        public int VectorSize => S8 * S;
        public StrassenImpl(int length)
        {
            S = Math.Max(1 << InternalBit.CeilPow2(length), B);
            S8 = S / 8;
        }
        public VectorizedModInt<T>[] Strassen(VectorizedModInt<T>[] a, VectorizedModInt<T>[] b)
        {
            Contract.Assert(new T().Mod % 2 == 1);
            var c = new VectorizedModInt<T>[VectorSize];

            var len = c.Length * 3 / 2;

            var sb = ArrayPool<VectorizedModInt<T>>.Shared.Rent(len);
            var tb = ArrayPool<VectorizedModInt<T>>.Shared.Rent(len);
            var ub = ArrayPool<VectorizedModInt<T>>.Shared.Rent(len);

            try
            {
                var s = sb.AsSpan(0, len);
                var t = tb.AsSpan(0, len);
                var u = ub.AsSpan(0, len);

                PlaceS(S, 0, 0, s, a);
                PlaceT(S, 0, 0, t, b);
                for (int i = 0; i < S * S8; i++) s[i] = s[i].Itom();
                for (int i = 0; i < S * S8; i++) t[i] = t[i].Itom();

                Strassen(S, s, t, u);

                for (int i = 0; i < S * S8; i++) u[i] = u[i].Mtoi();
                PlaceRev(S, 0, 0, u, c);
                return c;
            }
            finally
            {
                ArrayPool<VectorizedModInt<T>>.Shared.Return(sb);
                ArrayPool<VectorizedModInt<T>>.Shared.Return(tb);
                ArrayPool<VectorizedModInt<T>>.Shared.Return(ub);
            }
        }

        [凾(256 | 512)]
        private void Strassen(int N,
            Span<VectorizedModInt<T>> s,
            Span<VectorizedModInt<T>> t,
            Span<VectorizedModInt<T>> u)
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
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o11 + i] = pu[i]; u[o22 + i] = pu[i]; }

            // P2
            for (int i = 0; i < nx; i++) ps[i] = s[o21 + i] + s[o22 + i];
            for (int i = 0; i < nx; i++) pt[i] = t[o11 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o21 + i] = pu[i]; u[o22 + i] -= pu[i]; }

            // P3
            for (int i = 0; i < nx; i++) ps[i] = s[o11 + i];
            for (int i = 0; i < nx; i++) pt[i] = t[o12 + i] - t[o22 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o12 + i] = pu[i]; u[o22 + i] += pu[i]; }

            // P4
            for (int i = 0; i < nx; i++) ps[i] = s[o22 + i];
            for (int i = 0; i < nx; i++) pt[i] = t[o21 + i] - t[o11 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o11 + i] += pu[i]; u[o21 + i] += pu[i]; }

            // P5
            for (int i = 0; i < nx; i++) ps[i] = s[o11 + i] + s[o12 + i];
            for (int i = 0; i < nx; i++) pt[i] = t[o22 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) { u[o11 + i] -= pu[i]; u[o12 + i] += pu[i]; }

            // P6
            for (int i = 0; i < nx; i++) ps[i] = s[o21 + i] - s[o11 + i];
            for (int i = 0; i < nx; i++) pt[i] = t[o11 + i] + t[o12 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) u[o22 + i] += pu[i];

            // P7
            for (int i = 0; i < nx; i++) ps[i] = s[o12 + i] - s[o22 + i];
            for (int i = 0; i < nx; i++) pt[i] = t[o21 + i] + t[o22 + i];
            Strassen(N / 2, ps, pt, pu);
            for (int i = 0; i < nx; i++) u[o11 + i] += pu[i];
        }

        [凾(256 | 512)]
        public void PlaceRev(int N, int a, int b, Span<VectorizedModInt<T>> dst, Span<VectorizedModInt<T>> src)
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
        public void PlaceS(int N, int a, int b, Span<VectorizedModInt<T>> dst, Span<VectorizedModInt<T>> src)
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
        public void PlaceT(int N, int a, int b, Span<VectorizedModInt<T>> dst, Span<VectorizedModInt<T>> src)
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
