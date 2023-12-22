using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __ArrayExtension
    {
        /// <summary>
        /// <paramref name="a"/> を <paramref name="value"/> で Fill します。
        /// </summary>
        /// <returns><paramref name="a"/>自身</returns>
        [凾(256)]
        public static T[] Fill<T>(this T[] a, T value)
        {
            a.AsSpan().Fill(value);
            return a;
        }
        /// <summary>
        /// <paramref name="a"/> をソートします。
        /// </summary>
        /// <returns><paramref name="a"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T>(this T[] a) { Array.Sort(a); return a; }
        /// <summary>
        /// <paramref name="a"/> を辞書順でソートします。
        /// </summary>
        /// <returns><paramref name="a"/>自身</returns>
        [凾(256)]
        public static string[] Sort(this string[] a) => Sort(a, StringComparer.Ordinal);
        /// <summary>
        /// <paramref name="a"/> を <paramref name="selector"/> で取り出した値でソートします。
        /// </summary>
        /// <returns><paramref name="a"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T, U>(this T[] a, Func<T, U> selector) where U : IComparable<U>
        {
            Array.Sort(a.Select(selector).ToArray(), a);
            return a;
        }
        /// <summary>
        /// <paramref name="a"/> を <paramref name="comparison"/> でソートします。
        /// </summary>
        /// <returns><paramref name="a"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T>(this T[] a, Comparison<T> comparison) { Array.Sort(a, comparison); return a; }
        /// <summary>
        /// <paramref name="a"/> を <paramref name="comparer"/> でソートします。
        /// </summary>
        /// <returns><paramref name="a"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T>(this T[] a, IComparer<T> comparer) { Array.Sort(a, comparer); return a; }

        /// <summary>
        /// <paramref name="a"/> を逆順にします。
        /// </summary>
        /// <returns><paramref name="a"/>自身</returns>
        [凾(256)]
        public static T[] Reverse<T>(this T[] a) { Array.Reverse(a); return a; }

        /// <summary>
        /// <paramref name="a"/>[<paramref name="index"/>] を返します。<paramref name="index"/> が負ならば <paramref name="a"/>[<paramref name="a"/>.Length + <paramref name="index"/>] を返します。
        /// </summary>
        [凾(256)]
        public static ref T Get<T>(this T[] a, int index)
        {
            if (index < 0) index += a.Length;
            return ref a[index];
        }

        /// <summary>
        /// <paramref name="a"/>[<paramref name="index"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref readonly T GetOrDummy<T>(this ReadOnlySpan<T> a, int index, T dummy = default)
        {
            if ((uint)index < (uint)a.Length)
                return ref a[index];
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        /// <summary>
        /// <paramref name="a"/>[<paramref name="index"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this Span<T> a, int index, T dummy = default)
        {
            if ((uint)index < (uint)a.Length)
                return ref a[index];
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        /// <summary>
        /// <paramref name="a"/>[<paramref name="index"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this T[] a, int index, T dummy = default)
            => ref GetOrDummy(a.AsSpan(), index, dummy);

        /// <summary>
        /// <paramref name="a"/>[<paramref name="ix1"/>][<paramref name="ix2"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this T[][] a,
            int ix1,
            int ix2,
            T dummy = default)
        {
            if ((uint)ix1 < (uint)a.Length)
                return ref GetOrDummy(a[ix1], ix2, dummy);
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        /// <summary>
        /// <paramref name="a"/>[<paramref name="ix1"/>][<paramref name="ix2"/>][<paramref name="ix3"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this T[][][] a,
            int ix1,
            int ix2,
            int ix3,
            T dummy = default)
        {
            if ((uint)ix1 < (uint)a.Length)
                return ref GetOrDummy(a[ix1], ix2, ix3, dummy);
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        private static class Dummy<T> { public static T dummy; }


        // 見つかったら検索を終えるので LowerBound よりも少し速いかも
        /// <summary>
        /// <para>ソート済みの <paramref name="a"/> から <paramref name="value"/> のインデックスを返します。</para>
        /// <para><paramref name="a"/> に <paramref name="value"/> が複数存在するならばそのいずれかのインデックスを返します。</para>
        /// <para><paramref name="a"/> に <paramref name="value"/> が含まれなければ <paramref name="value"/> を超える最小のインデックスを返します。</para>
        /// </summary>
        /// <remarks>制約: <paramref name="a"/> はソート済みであること</remarks>
        [凾(256)]
        public static int FindByBinarySearch<T, TCv>(this T[] a, TCv value) where TCv : IComparable<T> => FindByBinarySearch((ReadOnlySpan<T>)a, value);
        /// <summary>
        /// <para>ソート済みの <paramref name="a"/> から <paramref name="value"/> のインデックスを返します。</para>
        /// <para><paramref name="a"/> に <paramref name="value"/> が複数存在するならばそのいずれかのインデックスを返します。</para>
        /// <para><paramref name="a"/> に <paramref name="value"/> が含まれなければ <paramref name="value"/> を超える最小のインデックスを返します。</para>
        /// </summary>
        /// <remarks>制約: <paramref name="a"/> はソート済みであること</remarks>
        [凾(256)]
        public static int FindByBinarySearch<T, TCv>(this Span<T> a, TCv value) where TCv : IComparable<T> => FindByBinarySearch((ReadOnlySpan<T>)a, value);
        /// <summary>
        /// <para>ソート済みの <paramref name="a"/> から <paramref name="value"/> のインデックスを返します。</para>
        /// <para><paramref name="a"/> に <paramref name="value"/> が複数存在するならばそのいずれかのインデックスを返します。</para>
        /// <para><paramref name="a"/> に <paramref name="value"/> が含まれなければ <paramref name="value"/> を超える最小のインデックスを返します。</para>
        /// </summary>
        /// <remarks>制約: <paramref name="a"/> はソート済みであること</remarks>
        [凾(256)]
        public static int FindByBinarySearch<T, TCv>(this ReadOnlySpan<T> a, TCv value) where TCv : IComparable<T>
        {
            int ix = a.BinarySearch(value);
            if (ix < 0)
                ix = ~ix;
            return ix;
        }
    }
}
