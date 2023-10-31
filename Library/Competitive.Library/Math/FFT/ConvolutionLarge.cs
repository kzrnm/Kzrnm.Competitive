using AtCoder;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{

    public static class ConvolutionLarge
    {
        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static uint[] Convolution<T>(ReadOnlySpan<int> a, ReadOnlySpan<int> b)
              where T : struct, IStaticMod
              => Convolution<T>(MemoryMarshal.Cast<int, uint>(a), MemoryMarshal.Cast<int, uint>(b));

        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static StaticModInt<T>[] Convolution<T>(StaticModInt<T>[] a, StaticModInt<T>[] b)
             where T : struct, IStaticMod
            => Convolution((ReadOnlySpan<StaticModInt<T>>)a, b);
        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static MontgomeryModInt<T>[] Convolution<T>(MontgomeryModInt<T>[] a, MontgomeryModInt<T>[] b)
             where T : struct, IStaticMod
            => Convolution((ReadOnlySpan<MontgomeryModInt<T>>)a, b);
        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static StaticModInt<T>[] Convolution<T>(Span<StaticModInt<T>> a, Span<StaticModInt<T>> b)
             where T : struct, IStaticMod
            => Convolution((ReadOnlySpan<StaticModInt<T>>)a, b);
        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static MontgomeryModInt<T>[] Convolution<T>(Span<MontgomeryModInt<T>> a, Span<MontgomeryModInt<T>> b)
             where T : struct, IStaticMod
            => Convolution((ReadOnlySpan<MontgomeryModInt<T>>)a, b);
        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static MontgomeryModInt<T>[] Convolution<T>(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> b)
             where T : struct, IStaticMod
            => ConvolutionImpl(a, b);
        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static StaticModInt<T>[] Convolution<T>(ReadOnlySpan<StaticModInt<T>> a, ReadOnlySpan<StaticModInt<T>> b)
             where T : struct, IStaticMod
        {
            var ma = ArrayPool<MontgomeryModInt<T>>.Shared.Rent(a.Length);
            var mb = ArrayPool<MontgomeryModInt<T>>.Shared.Rent(b.Length);
            for (int i = 0; i < a.Length; i++) ma[i] = a[i].Value;
            for (int i = 0; i < b.Length; i++) mb[i] = b[i].Value;
            var r = ConvolutionImpl<T>(ma.AsSpan(0, a.Length), mb.AsSpan(0, b.Length));
            ArrayPool<MontgomeryModInt<T>>.Shared.Return(ma);
            ArrayPool<MontgomeryModInt<T>>.Shared.Return(mb);

            var rt = new StaticModInt<T>[r.Length];
            for (int i = 0; i < r.Length; i++) rt[i] = r[i].Value;

            return rt;
        }


        /// <summary>
        /// Ntt 可能な長さより大きな長さの畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static uint[] Convolution<T>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b) where T : struct, IStaticMod
        {
            var ma = ArrayPool<MontgomeryModInt<T>>.Shared.Rent(a.Length);
            var mb = ArrayPool<MontgomeryModInt<T>>.Shared.Rent(b.Length);
            for (int i = 0; i < a.Length; i++) ma[i] = a[i];
            for (int i = 0; i < b.Length; i++) mb[i] = b[i];
            var r = ConvolutionImpl<T>(ma.AsSpan(0, a.Length), mb.AsSpan(0, b.Length));
            ArrayPool<MontgomeryModInt<T>>.Shared.Return(ma);
            ArrayPool<MontgomeryModInt<T>>.Shared.Return(mb);

            var rt = new uint[r.Length];
            for (int i = 0; i < r.Length; i++) rt[i] = (uint)r[i].Value;
            return rt;
        }

        static MontgomeryModInt<T>[] ConvolutionImpl<T>(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> b) where T : struct, IStaticMod
        {
            if (a.Length == 0 || b.Length == 0)
                return Array.Empty<MontgomeryModInt<T>>();

            if (a.Length < b.Length)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }
            var nttLength = NumberTheoreticTransform<T>.NttLength();
            if (nttLength < 2)
                return null;
            if (a.Length + b.Length - 1 <= nttLength)
                return NumberTheoreticTransform<T>.Multiply(a, b);

            var half = nttLength >> 1;

            var halfSizeA = (a.Length + half - 1) / half;
            var halfSizeB = (b.Length + half - 1) / half;
            var bL = new MontgomeryModInt<T>[halfSizeB][];

            for (int i = 0; i < bL.Length; i++)
            {
                var c = new MontgomeryModInt<T>[nttLength];
                var start = i * half;
                b.Slice(start, Math.Min(half, b.Length - start)).CopyTo(c);
                NumberTheoreticTransform<T>.Ntt(c);
                bL[i] = c;
            }

            var cL = new MontgomeryModInt<T>[halfSizeA + halfSizeB - 1][];
            for (int i = 0; i < cL.Length; i++)
                cL[i] = new MontgomeryModInt<T>[nttLength];

            MontgomeryModInt<T>[] aaPool;
            var aa = (aaPool = ArrayPool<MontgomeryModInt<T>>.Shared.Rent(nttLength)).AsSpan(0, nttLength);
            try
            {
                for (int i = 0; i < halfSizeA; i++)
                {
                    var start = i * half;
                    a.Slice(start, Math.Min(half, a.Length - start)).CopyTo(aa);
                    aa[Math.Min(half, a.Length - start)..].Clear();
                    NumberTheoreticTransform<T>.Ntt(aa);

                    for (int j = 0; j < bL.Length; j++)
                    {
                        var cc = cL[i + j];
                        for (int k = 0; k < cc.Length; k++)
                        {
                            cc[k] += aa[k] * bL[j][k];
                        }
                    }
                }
            }
            finally
            {
                ArrayPool<MontgomeryModInt<T>>.Shared.Return(aaPool);
            }
            foreach (var cc in cL) NumberTheoreticTransform<T>.INtt(cc);

            var rt = new MontgomeryModInt<T>[a.Length + b.Length - 1];
            ref var rPtr = ref MemoryMarshal.GetReference(rt.AsSpan());
            for (int i = 0; i < cL.Length; i++)
            {
                int offset = half * i;
                for (int j = Math.Min(nttLength, rt.Length - offset) - 1; j >= 0; j--)
                {
                    Unsafe.Add(ref rPtr, j + offset) += cL[i][j];
                }
            }
            return rt;
        }
    }
}
