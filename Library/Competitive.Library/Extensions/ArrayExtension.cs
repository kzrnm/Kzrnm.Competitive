using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __ArrayExtension
    {
        /// <summary>
        /// <paramref name="arr"/> を <paramref name="value"/> で Fill します。
        /// </summary>
        /// <returns><paramref name="arr"/>自身</returns>
        [凾(256)]
        public static T[] Fill<T>(this T[] arr, T value)
        {
            arr.AsSpan().Fill(value);
            return arr;
        }
        /// <summary>
        /// <paramref name="arr"/> をソートします。
        /// </summary>
        /// <returns><paramref name="arr"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T>(this T[] arr) { Array.Sort(arr); return arr; }
        /// <summary>
        /// <paramref name="arr"/> を辞書順でソートします。
        /// </summary>
        /// <returns><paramref name="arr"/>自身</returns>
        [凾(256)]
        public static string[] Sort(this string[] arr) => Sort(arr, StringComparer.Ordinal);
        /// <summary>
        /// <paramref name="arr"/> を <paramref name="selector"/> で取り出した値でソートします。
        /// </summary>
        /// <returns><paramref name="arr"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T, U>(this T[] arr, Func<T, U> selector) where U : IComparable<U>
        {
            Array.Sort(arr.Select(selector).ToArray(), arr);
            return arr;
        }
        /// <summary>
        /// <paramref name="arr"/> を <paramref name="comparison"/> でソートします。
        /// </summary>
        /// <returns><paramref name="arr"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T>(this T[] arr, Comparison<T> comparison) { Array.Sort(arr, comparison); return arr; }
        /// <summary>
        /// <paramref name="arr"/> を <paramref name="comparer"/> でソートします。
        /// </summary>
        /// <returns><paramref name="arr"/>自身</returns>
        [凾(256)]
        public static T[] Sort<T>(this T[] arr, IComparer<T> comparer) { Array.Sort(arr, comparer); return arr; }

        /// <summary>
        /// <paramref name="arr"/> を逆順にします。
        /// </summary>
        /// <returns><paramref name="arr"/>自身</returns>
        [凾(256)]
        public static T[] Reverse<T>(this T[] arr) { Array.Reverse(arr); return arr; }

        /// <summary>
        /// <paramref name="arr"/>[<paramref name="index"/>] を返します。<paramref name="index"/> が負ならば <paramref name="arr"/>[<paramref name="arr"/>.Length + <paramref name="index"/>] を返します。
        /// </summary>
        [凾(256)]
        public static ref T Get<T>(this T[] arr, int index)
        {
            if (index < 0) index += arr.Length;
            return ref arr[index];
        }

        /// <summary>
        /// <paramref name="arr"/>[<paramref name="index"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref readonly T GetOrDummy<T>(this ReadOnlySpan<T> arr, int index, T dummy = default)
        {
            if ((uint)index < (uint)arr.Length)
                return ref arr[index];
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        /// <summary>
        /// <paramref name="arr"/>[<paramref name="index"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this Span<T> arr, int index, T dummy = default)
        {
            if ((uint)index < (uint)arr.Length)
                return ref arr[index];
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        /// <summary>
        /// <paramref name="arr"/>[<paramref name="index"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this T[] arr, int index, T dummy = default)
            => ref GetOrDummy(arr.AsSpan(), index, dummy);

        /// <summary>
        /// <paramref name="arr"/>[<paramref name="index1"/>][<paramref name="index2"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this T[][] arr,
            int index1,
            int index2,
            T dummy = default)
        {
            if ((uint)index1 < (uint)arr.Length)
                return ref GetOrDummy(arr[index1], index2, dummy);
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        /// <summary>
        /// <paramref name="arr"/>[<paramref name="index1"/>][<paramref name="index2"/>][<paramref name="index3"/>] を返します。配列の範囲外ならば <paramref name="dummy"/> を返します。
        /// </summary>
        [凾(256)]
        public static ref T GetOrDummy<T>(this T[][][] arr,
            int index1,
            int index2,
            int index3,
            T dummy = default)
        {
            if ((uint)index1 < (uint)arr.Length)
                return ref GetOrDummy(arr[index1], index2, index3, dummy);
            Dummy<T>.dummy = dummy;
            return ref Dummy<T>.dummy;
        }
        private static class Dummy<T> { public static T dummy; }


        // 見つかったら検索を終えるので LowerBound よりも少し速いかも
        /// <summary>
        /// <para>ソート済みの <paramref name="arr"/> から <paramref name="value"/> のインデックスを返します。</para>
        /// <para><paramref name="arr"/> に <paramref name="value"/> が複数存在するならばそのいずれかのインデックスを返します。</para>
        /// <para><paramref name="arr"/> に <paramref name="value"/> が含まれなければ <paramref name="value"/> を超える最小のインデックスを返します。</para>
        /// </summary>
        /// <remarks>制約: <paramref name="arr"/> はソート済みであること</remarks>
        [凾(256)]
        public static int FindByBinarySearch<T, TCv>(this T[] arr, TCv value) where TCv : IComparable<T> => FindByBinarySearch((ReadOnlySpan<T>)arr, value);
        /// <summary>
        /// <para>ソート済みの <paramref name="arr"/> から <paramref name="value"/> のインデックスを返します。</para>
        /// <para><paramref name="arr"/> に <paramref name="value"/> が複数存在するならばそのいずれかのインデックスを返します。</para>
        /// <para><paramref name="arr"/> に <paramref name="value"/> が含まれなければ <paramref name="value"/> を超える最小のインデックスを返します。</para>
        /// </summary>
        /// <remarks>制約: <paramref name="arr"/> はソート済みであること</remarks>
        [凾(256)]
        public static int FindByBinarySearch<T, TCv>(this Span<T> arr, TCv value) where TCv : IComparable<T> => FindByBinarySearch((ReadOnlySpan<T>)arr, value);
        /// <summary>
        /// <para>ソート済みの <paramref name="arr"/> から <paramref name="value"/> のインデックスを返します。</para>
        /// <para><paramref name="arr"/> に <paramref name="value"/> が複数存在するならばそのいずれかのインデックスを返します。</para>
        /// <para><paramref name="arr"/> に <paramref name="value"/> が含まれなければ <paramref name="value"/> を超える最小のインデックスを返します。</para>
        /// </summary>
        /// <remarks>制約: <paramref name="arr"/> はソート済みであること</remarks>
        [凾(256)]
        public static int FindByBinarySearch<T, TCv>(this ReadOnlySpan<T> arr, TCv value) where TCv : IComparable<T>
        {
            int ix = arr.BinarySearch(value);
            if (ix < 0)
                ix = ~ix;
            return ix;
        }
    }
}
