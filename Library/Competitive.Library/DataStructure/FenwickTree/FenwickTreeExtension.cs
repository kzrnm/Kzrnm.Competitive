using AtCoder;
using AtCoder.Internal;
using System;
using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __FenwickTreeExtension
    {
        /// <summary>
        /// <paramref name="fw"/>.Sum(0, i) の値が <paramref name="v"/> 以上となる最小のインデックス i を取得します。<paramref name="fw"/>.Sum(0, fw.Length) でも条件を満たさなければ fw.Length + 1 を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="fw"/> の値が非負。</para>
        /// <para>計算量: O(log <c>n</c>)</para>
        /// </remarks>
        [凾(256)]
        public static int LowerBound<T>(this FenwickTree<T> fw, T v)
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>, IComparisonOperators<T, T, bool>
        {
            if (v <= T.AdditiveIdentity) return 0;
            int x = 0;
            var data = fw.data;
            for (int k = 1 << BitOperations.Log2((uint)data.Length + 1); k > 0; k >>= 1)
            {
                var nx = x + k;
                if (nx < data.Length && data[nx] < v)
                {
                    x = nx;
                    v -= data[nx];
                }
            }
            return x + 1;
        }

        /// <summary>
        /// <paramref name="fw"/>.Sum(0, i) の値が <paramref name="v"/> より大きくなる最小のインデックス i を取得します。<paramref name="fw"/>.Sum(0, fw.Length) でも条件を満たさなければ fw.Length + 1 を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="fw"/> の値が非負。</para>
        /// <para>計算量: O(log <c>n</c>)</para>
        /// </remarks>
        [凾(256)]
        public static int UpperBound<T>(this FenwickTree<T> fw, T v)
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>, IComparisonOperators<T, T, bool>
        {
            if (v < T.AdditiveIdentity) return 0;
            int x = 0;
            var data = fw.data;
            for (int k = 1 << BitOperations.Log2((uint)data.Length + 1); k > 0; k >>= 1)
            {
                var nx = x + k;
                if (nx < data.Length && data[nx] <= v)
                {
                    x = nx;
                    v -= data[nx];
                }
            }
            return x + 1;
        }

        /// <summary>
        /// <c><paramref name="fw"/>[i]=<paramref name="fw"/>[i..(i+1)]</c> の値を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log <c>n</c>)</para>
        /// </remarks>
        public static T Get<T>(this FenwickTree<T> fw, int i)
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => fw.Sum(i, i + 1);

        /// <summary>
        /// <c><paramref name="fw"/>[i]+=<paramref name="s"/>[i]</c> となるよう更新します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        public static void Add<T>(this FenwickTree<T> fw, ReadOnlySpan<T> s)
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            Contract.Assert(s.Length <= fw.Length, reason: $"IndexOutOfRange: input is longer than fenwick tree.");
            var d = fw.data;
            var buf = ArrayPool<T>.Shared.Rent(d.Length);
            s.CopyTo(buf.AsSpan(1));
            buf.AsSpan(s.Length + 1).Clear();
            ref var bp = ref buf[0];
            for (int p = 1; p < d.Length; ++p)
            {
                var t = Unsafe.Add(ref bp, p);
                d[p] += t;
                var q = p + (int)InternalBit.ExtractLowestSetBit(p);
                if (q < d.Length)
                    Unsafe.Add(ref bp, q) += t;
            }
            ArrayPool<T>.Shared.Return(buf);
        }

        /// <summary>
        /// <c>(<paramref name="fw"/>[i..(i+1)], <paramref name="fw"/>[..(i+1)])</c> のタプルを配列にして返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        [凾(256)]
        public static (T Item, T Sum)[] ToArray<T>(this FenwickTree<T> fw)
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var data = fw.data;
            var items = new (T Item, T Sum)[data.Length - 1];
            items[0] = (data[1], data[1]);
            for (int i = 2; i < data.Length; i++)
            {
                int length = (int)InternalBit.ExtractLowestSetBit(i);
                var pr = i - length - 1;
                var sum = data[i] + (0 <= pr ? items[pr].Sum : T.AdditiveIdentity);
                var val = sum - items[i - 2].Sum;
                items[i - 1] = (val, sum);
            }
            return items;
        }
    }
}
