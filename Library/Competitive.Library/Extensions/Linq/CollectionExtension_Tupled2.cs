using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_Tupled2
    {
        /// <summary>
        /// コレクションの要素2つずつをまとめた配列を返します。
        /// </summary>
        /// <example>
        /// [1, 2, 3] → [(1,2), (2,3)]
        /// </example>
        [凾(256)]
        public static (T, T)[] Tupled2<T>(this T[] a) => Tupled2((ReadOnlySpan<T>)a);
        /// <summary>
        /// コレクションの要素2つずつをまとめた配列を返します。
        /// </summary>
        /// <example>
        /// [1, 2, 3] → [(1,2), (2,3)]
        /// </example>
        [凾(256)]
        public static (T, T)[] Tupled2<T>(this Span<T> a) => Tupled2((ReadOnlySpan<T>)a);
        /// <summary>
        /// コレクションの要素2つずつをまとめた配列を返します。
        /// </summary>
        /// <example>
        /// [1, 2, 3] → [(1,2), (2,3)]
        /// </example>
        [凾(256)]
        public static (T, T)[] Tupled2<T>(this ReadOnlySpan<T> a)
        {
            var r = new (T, T)[a.Length - 1];
            for (int i = 0; i + 1 < a.Length; i++)
                r[i] = (a[i], a[i + 1]);
            return r;
        }
    }
}
