using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_MaxMin
    {
        /// <summary>
        /// 最大値と最小値を取得する。空ならデフォルト
        /// </summary>
        public static (T Max, T Min) MaxMin<T>(this IEnumerable<T> collection) where T : IComparable<T>
        {
            using var e = collection.GetEnumerator();
            if (!e.MoveNext()) return default((T, T));
            var max = e.Current;
            var min = max;
            while (e.MoveNext())
            {
                var v = e.Current;
                if (v.CompareTo(max) > 0) max = v;
                else if (v.CompareTo(min) < 0) min = v;
            }
            return (max, min);
        }
        /// <summary>
        /// 最大値と最小値を取得する。空ならデフォルト
        /// </summary>
        public static (T Max, T Min) MaxMin<T>(this Span<T> collection) where T : IComparable<T>
            => MaxMin((ReadOnlySpan<T>)collection);
        /// <summary>
        /// 最大値と最小値を取得する。空ならデフォルト
        /// </summary>
        public static (T Max, T Min) MaxMin<T>(this ReadOnlySpan<T> collection) where T : IComparable<T>
        {
            if (collection.IsEmpty) return default((T, T));
            var max = collection[0];
            var min = max;
            foreach (var v in collection[1..])
            {
                if (v.CompareTo(max) > 0) max = v;
                else if (v.CompareTo(min) < 0) min = v;
            }
            return (max, min);
        }
    }
}
