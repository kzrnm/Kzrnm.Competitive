using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_Contribution
    {
        /// <summary>
        /// コレクションを主客転倒します。複数の要素を保持するコレクション <paramref name="source"/> を複数の配列に変換します。
        /// </summary>
        public static (T1[], T2[]) Contribution<T, T1, T2>(this IEnumerable<T> source, Func<T, (T1, T2)> selector)
        {
            T1[] t1;
            T2[] t2;
            if (source.TryGetNonEnumeratedCount(out var cnt))
            {
                t1 = new T1[cnt];
                t2 = new T2[cnt];
                int i = 0;
                foreach (var t in source)
                {
                    (t1[i], t2[i]) = selector(t);
                    i++;
                }
            }
            else
            {
                var l1 = new List<T1>();
                var l2 = new List<T2>();
                foreach (var t in source)
                {
                    var (v1, v2) = selector(t);
                    l1.Add(v1);
                    l2.Add(v2);
                }
                t1 = l1.ToArray();
                t2 = l2.ToArray();
            }
            return (t1, t2);
        }

        /// <inheritdoc cref="Contribution{T, T1, T2}(IEnumerable{T}, Func{T, ValueTuple{T1, T2}})"/>
        public static (T1[], T2[]) Contribution<T1, T2>(this IEnumerable<(T1, T2)> source) => Contribution(source, t => t);

        /// <inheritdoc cref="Contribution{T, T1, T2}(IEnumerable{T}, Func{T, ValueTuple{T1, T2}})"/>
        public static (T1[], T2[], T3[]) Contribution<T, T1, T2, T3>(this IEnumerable<T> source, Func<T, (T1, T2, T3)> selector)
        {
            T1[] t1;
            T2[] t2;
            T3[] t3;
            if (source.TryGetNonEnumeratedCount(out var cnt))
            {
                t1 = new T1[cnt];
                t2 = new T2[cnt];
                t3 = new T3[cnt];
                int i = 0;
                foreach (var t in source)
                {
                    (t1[i], t2[i], t3[i]) = selector(t);
                    i++;
                }
            }
            else
            {
                var l1 = new List<T1>();
                var l2 = new List<T2>();
                var l3 = new List<T3>();
                foreach (var t in source)
                {
                    var (v1, v2, v3) = selector(t);
                    l1.Add(v1);
                    l2.Add(v2);
                    l3.Add(v3);
                }
                t1 = l1.ToArray();
                t2 = l2.ToArray();
                t3 = l3.ToArray();
            }
            return (t1, t2, t3);
        }

        /// <inheritdoc cref="Contribution{T, T1, T2}(IEnumerable{T}, Func{T, ValueTuple{T1, T2}})"/>
        public static (T1[], T2[], T3[]) Contribution<T1, T2, T3>(this IEnumerable<(T1, T2, T3)> source) => Contribution(source, t => t);
    }
}
