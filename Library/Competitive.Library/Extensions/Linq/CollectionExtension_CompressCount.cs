using AtCoder.Internal;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_CompressCount
    {
        /// <summary>
        /// 連続する要素をひとまとめにした配列を返す。
        /// </summary>
        public static (T Value, int Count)[] CompressCount<T>(this IEnumerable<T> collection)
        {
            var e = collection.GetEnumerator();
            using var list = new PoolList<(T Value, int Count)>();
            if (!e.MoveNext()) return Array.Empty<(T, int)>();
            var cur = e.Current;
            list.Add((cur, 1));
            while (e.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(cur, e.Current))
                    list[^1].Count++;
                else
                {
                    cur = e.Current;
                    list.Add((cur, 1));
                }
            }
            return list.ToArray();
        }
    }
}
