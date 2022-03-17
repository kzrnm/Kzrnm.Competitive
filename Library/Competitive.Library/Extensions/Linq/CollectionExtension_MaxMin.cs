using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_MinMax
    {
        /// <summary>
        /// 最大値と最小値を取得する。空ならデフォルト
        /// </summary>
        [凾(256)]
        public static (T Min, T Max) MinMax<T>(this IEnumerable<T> collection) where T : IComparable<T>
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
            return (min, max);
        }
        /// <summary>
        /// 最大値と最小値を取得する。空ならデフォルト
        /// </summary>
        [凾(256)]
        public static (T Min, T Max) MinMax<T>(this Span<T> collection) where T : IComparable<T>
               => MinMax((ReadOnlySpan<T>)collection);
        /// <summary>
        /// 最大値と最小値を取得する。空ならデフォルト
        /// </summary>
        [凾(256)]
        public static (T Min, T Max) MinMax<T>(this ReadOnlySpan<T> collection) where T : IComparable<T>
        {
            if (collection.IsEmpty) return default((T, T));
            var max = collection[0];
            var min = max;
            foreach (var v in collection[1..])
            {
                if (v.CompareTo(max) > 0) max = v;
                else if (v.CompareTo(min) < 0) min = v;
            }
            return (min, max);
        }
    }
}
