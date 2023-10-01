using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE Flatten
    public static class __CollectionExtension_Flatten
    {
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        [凾(256)]
        public static T[] Flatten<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            var res = new List<T>();
            foreach (var col in collection)
                res.AddRange(col);
            return res.ToArray();
        }
    }
}
