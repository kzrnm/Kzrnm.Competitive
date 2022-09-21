using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// Python の itertools 的なやつ
    /// </summary>
    public static class IterTools
    {
        /// <summary>
        /// 順列を列挙します。
        /// </summary>
        [凾(256)]
        public static IEnumerable<T[]> Permutations<T>(IEnumerable<T> collection)
        {
            var array = collection.ToArray();
            var indices = Enumerable.Range(0, array.Length).ToArray();
            yield return array;
            while (StlFunction.NextPermutation(indices))
            {
                var result = new T[array.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = array[indices[i]];
                yield return result;
            }
        }

        /// <summary>
        /// <paramref name="collection"/> から <paramref name="k"/> 個取り出す部分列を列挙します。
        /// </summary>
        [凾(256)]
        public static IEnumerable<T[]> Combinations<T>(IEnumerable<T> collection, int k)
        {
            var array = collection.ToArray();
            if (array.Length < k || array.Length <= 0 || k < 0) yield break;
            var indices = Enumerable.Range(0, k).ToArray();
            yield return array[..k];
            while (true)
            {
                int ix;
                for (ix = k - 1; ix >= 0; ix--)
                    if (indices[ix] != ix + array.Length - k)
                        break;
                if (ix < 0)
                    yield break;
                ++indices[ix];
                for (int j = ix + 1; j < indices.Length; j++)
                    indices[j] = indices[j - 1] + 1;

                var result = new T[k];
                for (int i = 0; i < result.Length; i++)
                    result[i] = array[indices[i]];
                yield return result;
            }
        }

        /// <summary>
        /// <paramref name="collection"/> から重複を許して <paramref name="k"/> 個取り出す部分列を列挙します。
        /// </summary>
        [凾(256)]
        public static IEnumerable<T[]> CombinationsWithReplacement<T>(IEnumerable<T> collection, int k)
        {
            var array = collection.ToArray();
            if (array.Length <= 0 || k < 0) yield break;
            var indices = new int[k];
            {
                var result = new T[k];
                result.AsSpan().Fill(array[0]);
                yield return result;
            }
            while (true)
            {
                int ix;
                for (ix = k - 1; ix >= 0; ix--)
                    if (indices[ix] != array.Length - 1)
                        break;
                if (ix < 0)
                    yield break;
                indices.AsSpan(ix).Fill(indices[ix] + 1);
                var result = new T[k];
                for (int i = 0; i < result.Length; i++)
                    result[i] = array[indices[i]];
                yield return result;
            }
        }
    }
}
