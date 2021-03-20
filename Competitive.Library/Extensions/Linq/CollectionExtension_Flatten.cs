using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace System.Linq
{
#pragma warning disable IDE1006
    using static MethodImplOptions;
    public static class __CollectionExtension_Flatten
    {
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static T[] Flatten<T>(this T[][] array) => Flatten((ReadOnlySpan<T[]>)array);
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static T[] Flatten<T>(this Span<T[]> span) => Flatten((ReadOnlySpan<T[]>)span);
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static T[] Flatten<T>(this ReadOnlySpan<T[]> span)
        {
            var res = new T[span.Length * span[0].Length];
            for (int i = 0; i < span.Length; i++)
                for (int j = 0; j < span[i].Length; j++)
                    res[i * span[i].Length + j] = span[i][j];
            return res;
        }
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static T[] Flatten<T>(this IList<IList<T>> collection)
        {
            var res = new T[collection.Count * collection[0].Count];
            for (int i = 0; i < collection.Count; i++)
                for (int j = 0; j < collection[i].Count; j++)
                    res[i * collection[i].Count + j] = collection[i][j];
            return res;
        }
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static char[] Flatten(this string[] strs)
        {
            var res = new char[strs.Length * strs[0].Length];
            for (int i = 0; i < strs.Length; i++)
                for (int j = 0; j < strs[i].Length; j++)
                    res[i * strs[i].Length + j] = strs[i][j];
            return res;
        }
    }
}
