using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace System
{
#pragma warning disable IDE1006
    using static MethodImplOptions;
    public static class __CollectionExtension
    {
#pragma warning disable CS0649
        private class ArrayVal<T> { public T[] arr; }
#pragma warning restore CS0649
        [MethodImpl(AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0) => Unsafe.As<ArrayVal<T>>(list).arr.AsSpan(start, list.Count);
        [MethodImpl(AggressiveInlining)]
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var v);
            return v;
        }

        /// <summary>
        /// 連続する要素をひとまとめにした配列を返す。
        /// </summary>
        public static (T Value, int Count)[] CompressCount<T>(this IEnumerable<T> collection)
        {
            var e = collection.GetEnumerator();
            var list = new SimpleList<(T Value, int Count)>();
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
            var res = new T[span.Length * span[0].Length];
            for (int i = 0; i < span.Length; i++)
                for (int j = 0; j < span[i].Length; j++)
                    res[i * span[i].Length + j] = span[i][j];
            return res;
        }
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static T[] Flatten<T>(this IList<IList<T>> collection)
        {
            var res = new T[collection.Count * collection[0].Count];
            for (int i = 0; i < collection.Count; i++)
                for (int j = 0; j < collection[i].Count; j++)
                    res[i * collection[i].Count + j] = collection[i][j];
            return res;
        }
        /// <summary>
        /// 2次元のコレクションを1次元に平滑化
        /// </summary>
        public static char[] Flatten(this string[] strs)
        {
            var res = new char[strs.Length * strs[0].Length];
            for (int i = 0; i < strs.Length; i++)
                for (int j = 0; j < strs[i].Length; j++)
                    res[i * strs[i].Length + j] = strs[i][j];
            return res;
        }

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
            foreach (var v in collection.Slice(1))
            {
                if (v.CompareTo(max) > 0) max = v;
                else if (v.CompareTo(min) < 0) min = v;
            }
            return (max, min);
        }
    }
}
