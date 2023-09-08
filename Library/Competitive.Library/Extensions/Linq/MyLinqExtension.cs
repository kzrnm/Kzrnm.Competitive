using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE Linq 便利メソッド
    public static class __CollectionExtension_MyLinq
    {
        /// <summary>
        /// インデックスをつける
        /// </summary>
        [凾(256)]
        public static IEnumerable<(TSource Value, int Index)> Indexed<TSource>(this IEnumerable<TSource> source)
              => source.Select((v, i) => (v, i));

        /// <summary>
        /// 条件に合致するインデックスを返す
        /// </summary>
        [凾(256)]
        public static IEnumerable<int> WhereBy<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
              => source.Indexed().Where(t => predicate(t.Value)).Select(t => t.Index);
        /// <summary>
        /// 要素の個数を返します。
        /// </summary>
        [凾(256)]
        public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
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
            => source.Indexed().GroupBy(t => keySelector(t.Value), t => t.Index);
        /// <summary>
        /// 要素をグループ化し、インデックスを保持します。
        /// </summary>
        [凾(256)]
        public static IEnumerable<IGrouping<TKey, int>> GroupIndex<TKey>(this IEnumerable<TKey> source)
            => source.Indexed().GroupBy(t => t.Value, t => t.Index);
    }
}
