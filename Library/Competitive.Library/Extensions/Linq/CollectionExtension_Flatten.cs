using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_Flatten
    {
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        [凾(256)]
        public static T[] Flatten<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            var ls = new List<T>();
            foreach (var col in collection)
                ls.AddRange(col);
            return ls.ToArray();
        }

        /// <summary>
        /// タプルを配列に平滑化
        /// </summary>
        [凾(256)]
        public static T[] Flatten<T>(this IEnumerable<(T, T)> collection)
        {
            var ls = new List<T>();
            foreach (var (a, b) in collection)
            {
                ls.Add(a);
                ls.Add(b);
            }
            return ls.ToArray();
        }

        /// <summary>
        /// タプルを配列に平滑化
        /// </summary>
        [凾(256)]
        public static T[] Flatten<T>(this IEnumerable<(T, T, T)> collection)
        {
            var ls = new List<T>();
            foreach (var (a, b, c) in collection)
            {
                ls.Add(a);
                ls.Add(b);
                ls.Add(c);
            }
            return ls.ToArray();
        }
    }
}
