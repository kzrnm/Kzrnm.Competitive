using AtCoder;
using AtCoder.Internal;
using AtCoder.Operators;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __FenwickTreeExtension
    {
        /// <summary>
        /// <paramref name="fw"/>.Sum(0, i) の値が <paramref name="v"/> 以上となる最小のインデックス i を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="fw"/> の値が非負。</para>
        /// <para>計算量: O(log <c>n</c>)</para>
        /// </remarks>
        [凾(256)]
        public static int LowerBound<TValue, TOp>(this FenwickTree<TValue, TOp> fw, TValue v)
            where TOp : struct, IAdditionOperator<TValue>, ISubtractOperator<TValue>, ICompareOperator<TValue>
        {
            var op = new TOp();
            if (op.LessThanOrEqual(v, default)) return 0;
            int x = 0;
            var data = fw.data;
            for (int k = 1 << BitOperations.Log2((uint)data.Length + 1); k > 0; k >>= 1)
            {
                var nx = x + k;
                if (nx < data.Length && op.LessThan(fw.data[nx], v))
                {
                    x = nx;
                    v = op.Subtract(v, fw.data[nx]);
                }
            }
            return x + 1;
        }

        /// <summary>
        /// <paramref name="fw"/>.Sum(0, i) の値が <paramref name="v"/> より大きくなる最小のインデックス i を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="fw"/> の値が非負。</para>
        /// <para>計算量: O(log <c>n</c>)</para>
        /// </remarks>
        [凾(256)]
        public static int UpperBound<TValue, TOp>(this FenwickTree<TValue, TOp> fw, TValue v)
            where TOp : struct, IAdditionOperator<TValue>, ISubtractOperator<TValue>, ICompareOperator<TValue>
        {
            var op = new TOp();
            if (op.LessThan(v, default)) return 0;
            int x = 0;
            var data = fw.data;
            for (int k = 1 << BitOperations.Log2((uint)data.Length + 1); k > 0; k >>= 1)
            {
                var nx = x + k;
                if (nx < data.Length && op.LessThanOrEqual(fw.data[nx], v))
                {
                    x = nx;
                    v = op.Subtract(v, fw.data[nx]);
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
        public static TValue Get<TValue, TOp>(this FenwickTree<TValue, TOp> fw, int i)
            where TOp : struct, IAdditionOperator<TValue>, ISubtractOperator<TValue>
            => fw.Sum(i, i + 1);

        /// <summary>
        /// <c>(<paramref name="fw"/>[i..(i+1)], <paramref name="fw"/>[..(i+1)])</c> の配列にして返す。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(<c>n</c>)</para>
        /// </remarks>
        [凾(256)]
        public static (TValue Item, TValue Sum)[] ToArray<TValue, TOp>(this FenwickTree<TValue, TOp> fw)
            where TOp : struct, IAdditionOperator<TValue>, ISubtractOperator<TValue>
        {
            var op = new TOp();
            var data = fw.data;
            var items = new (TValue Item, TValue Sum)[data.Length - 1];
            items[0] = (data[1], data[1]);
            for (int i = 2; i < data.Length; i++)
            {
                int length = (int)InternalBit.ExtractLowestSetBit(i);
                var pr = i - length - 1;
                var sum = op.Add(data[i], 0 <= pr ? items[pr].Sum : default);
                var val = op.Subtract(sum, items[i - 2].Sum);
                items[i - 1] = (val, sum);
            }
            return items;
        }
    }
}
