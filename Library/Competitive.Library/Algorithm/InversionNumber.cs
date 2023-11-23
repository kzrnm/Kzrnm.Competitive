using AtCoder;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 転倒数
    /// </summary>
    public static class InversionNumber
    {
        /// <summary>
        /// <para><paramref name="s"/> の転倒数を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>転倒数: i &lt; j かつ s[i] &gt; s[j] を満たす組み合わせの個数</para>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)] public static long Inversion<T>(T[] s) where T : IComparable<T> => Inversion((ReadOnlySpan<T>)s);
        /// <summary>
        /// <para><paramref name="s"/> の転倒数を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>転倒数: i &lt; j かつ s[i] &gt; s[j] を満たす組み合わせの個数</para>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)] public static long Inversion<T>(Span<T> s) where T : IComparable<T> => Inversion((ReadOnlySpan<T>)s);
        /// <summary>
        /// <para><paramref name="s"/> の転倒数を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>転倒数: i &lt; j かつ s[i] &gt; s[j] を満たす組み合わせの個数</para>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)]
        public static long Inversion<T>(ReadOnlySpan<T> s) where T : IComparable<T>
            => Inversion(ZahyoCompress.CompressedArray(s));

        /// <summary>
        /// <para><paramref name="s"/> の転倒数を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約: 0 ≦ s[i] ≦ s.Length </para>
        /// <para>転倒数: i &lt; j かつ s[i] &gt; s[j] を満たす組み合わせの個数</para>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)] public static long Inversion(int[] s) => Inversion((ReadOnlySpan<int>)s);
        /// <summary>
        /// <para><paramref name="s"/> の転倒数を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約: 0 ≦ s[i] ≦ s.Length </para>
        /// <para>転倒数: i &lt; j かつ s[i] &gt; s[j] を満たす組み合わせの個数</para>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)] public static long Inversion(Span<int> s) => Inversion((ReadOnlySpan<int>)s);
        /// <summary>
        /// <para><paramref name="s"/> の転倒数を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約: 0 ≦ s[i] ≦ s.Length </para>
        /// <para>転倒数: i &lt; j かつ s[i] &gt; s[j] を満たす組み合わせの個数</para>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)]
        public static long Inversion(ReadOnlySpan<int> s)
        {
            long r = 0;
            var fw = new FenwickTree<int>(s.Length + 1);
            for (int i = 0; i < s.Length; i++)
            {
                r += fw[(s[i] + 1)..];
                fw.Add(s[i], 1);
            }
            return r;
        }
    }
}
