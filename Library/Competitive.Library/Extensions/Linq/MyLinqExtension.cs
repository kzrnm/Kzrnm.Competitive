using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_MyLinq
    {
#if !NET9_0_OR_GREATER
        /// <summary>
        /// インデックスをつける
        /// </summary>
        [凾(256)]
        public static IEnumerable<(int Index, TSource Item)> Index<TSource>(this IEnumerable<TSource> source)
              => source.Select((v, i) => (i, v));
#endif
        /// <summary>
        /// 条件に合致するインデックスを返す
        /// </summary>
        [凾(256)]
        public static IEnumerable<int> WhereBy<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
                  => source.Index().Where(t => predicate(t.Item)).Select(t => t.Index);
        /// <summary>
        /// 要素の個数を返します。
        /// </summary>
        [凾(256)]
        public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
#if NET9_0_OR_GREATER
            => source.CountBy(keySelector).ToDictionary();
#else
            => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
#endif
        /// <summary>
        /// 要素の個数を返します。
        /// </summary>
        [凾(256)]
        public static Dictionary<TKey, int> GroupCount<TKey>(this IEnumerable<TKey> source) => source.GroupCount(i => i);

        /// <summary>
        /// 要素をグループ化し、インデックスを保持します。
        /// </summary>
        [凾(256)]
        public static IEnumerable<IGrouping<TKey, int>> GroupIndex<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            => source.Index().GroupBy(t => keySelector(t.Item), t => t.Index);
        /// <summary>
        /// 要素をグループ化し、インデックスを保持します。
        /// </summary>
        [凾(256)]
        public static IEnumerable<IGrouping<TKey, int>> GroupIndex<TKey>(this IEnumerable<TKey> source)
            => source.Index().GroupBy(t => t.Item, t => t.Index);
    }
}
