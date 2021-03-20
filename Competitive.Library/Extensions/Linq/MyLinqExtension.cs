using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive
{
    public static class MyLinqExtension
    {
        /// <summary>
        /// インデックスをつける
        /// </summary>
        public static IEnumerable<(TSource val, int index)> Indexed<TSource>(this IEnumerable<TSource> source)
            => source.Select((v, i) => (v, i));

        /// <summary>
        /// 条件に合致するインデックスを返す
        /// </summary>
        public static IEnumerable<int> WhereBy<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => source.Select((v, i) => (i, v)).Where(t => predicate(t.v)).Select(t => t.i);
        public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
        public static Dictionary<TKey, int> GroupCount<TKey>(this IEnumerable<TKey> source) => source.GroupCount(i => i);
    }
}
