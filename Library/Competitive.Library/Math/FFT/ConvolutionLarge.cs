using AtCoder;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
            => ConvolutionImpl<T>(a.Select(v => (MontgomeryModInt<T>)v.Value), b.Select(v => (MontgomeryModInt<T>)v.Value))
                .Select(m => StaticModInt<T>.Raw(m.Value))
                .ToArray();

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
            => ConvolutionImpl<T>(a.Select(v => (MontgomeryModInt<T>)v), b.Select(v => (MontgomeryModInt<T>)v))
            .Select(m => (uint)m.Value)
            .ToArray();

        static MontgomeryModInt<T>[] ConvolutionImpl<T>(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> b, T op = default) where T : struct, IStaticMod
        {
            if (a.Length == 0 || b.Length == 0)
                return Array.Empty<MontgomeryModInt<T>>();

            if (a.Length > b.Length)
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

            var aL = new MontgomeryModInt<T>[(a.Length + half - 1) / half][];
            var bL = new MontgomeryModInt<T>[(b.Length + half - 1) / half][];

            for (int i = 0; i < aL.Length; i++)
            {
                var c = new MontgomeryModInt<T>[nttLength];
                a[(i * half)..Math.Min((i + 1) * half, a.Length)].CopyTo(c);
                NumberTheoreticTransform<T>.Ntt(c);
                aL[i] = c;
            }
            for (int i = 0; i < bL.Length; i++)
            {
                var c = new MontgomeryModInt<T>[nttLength];
                b[(i * half)..Math.Min((i + 1) * half, b.Length)].CopyTo(c);
                NumberTheoreticTransform<T>.Ntt(c);
                bL[i] = c;
            }

            var cL = new MontgomeryModInt<T>[aL.Length + bL.Length - 1][];
            for (int i = 0; i < cL.Length; i++)
            {
                cL[i] = new MontgomeryModInt<T>[nttLength];
            }

            for (int i = 0; i < aL.Length; i++)
            {
                for (int j = 0; j < bL.Length; j++)
                {
                    var cc = cL[i + j];
                    for (int k = 0; k < cc.Length; k++)
                    {
                        cc[k] += aL[i][k] * bL[j][k];
                    }
                }
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
