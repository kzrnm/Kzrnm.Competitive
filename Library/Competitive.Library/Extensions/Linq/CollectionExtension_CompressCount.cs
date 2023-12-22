using System;
using System.Buffers;
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
            var ls = new List<(T, int C)>();
            if (!e.MoveNext()) return Array.Empty<(T, int)>();
            var cur = e.Current;
            ls.Add((cur, 1));
            ref var t = ref ls.AsSpan()[^1].C;
            while (e.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(cur, e.Current))
                    ++t;
                else
                {
                    ls.Add((cur = e.Current, 1));
                    t = ref ls.AsSpan()[^1].C;
                }
            }
            return ls.ToArray();
        }
    }
}
