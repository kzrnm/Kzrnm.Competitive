using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
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
            var res = new SimpleList<T>(span.Length * Math.Max(span[0].Length, 1));
            for (int i = 0; i < span.Length; i++)
                for (int j = 0; j < span[i].Length; j++)
                    res.Add(span[i][j]);
            return res.ToArray();
        }
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static T[] Flatten<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            var res = new SimpleList<T>();
            foreach (var col in collection)
                foreach (var item in col)
                    res.Add(item);
            return res.ToArray();
        }
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static char[] Flatten(this string[] strs)
        {
            var res = new SimpleList<char>(strs.Length * Math.Max(strs[0].Length, 1));
            for (int i = 0; i < strs.Length; i++)
                for (int j = 0; j < strs[i].Length; j++)
                    res.Add(strs[i][j]);
            return res.ToArray();
        }
    }
}
