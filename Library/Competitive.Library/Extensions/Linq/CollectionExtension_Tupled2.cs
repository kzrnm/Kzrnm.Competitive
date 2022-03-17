using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_Tupled2
    {
        /// <summary>
        /// コレクションの要素2つずつをまとめた配列を返します。
        /// </summary>
        [凾(256)]
        public static (T, T)[] Tupled2<T>(this T[] collection) => Tupled2((ReadOnlySpan<T>)collection);
        /// <summary>
        /// コレクションの要素2つずつをまとめた配列を返します。
        /// </summary>
        [凾(256)]
        public static (T, T)[] Tupled2<T>(this Span<T> collection) => Tupled2((ReadOnlySpan<T>)collection);
        /// <summary>
        /// コレクションの要素2つずつをまとめた配列を返します。
        /// </summary>
        [凾(256)]
        public static (T, T)[] Tupled2<T>(this ReadOnlySpan<T> collection)
        {
            var result = new (T, T)[collection.Length - 1];
            for (int i = 0; i + 1 < collection.Length; i++)
                result[i] = (collection[i], collection[i + 1]);
            return result;
        }
    }
}
