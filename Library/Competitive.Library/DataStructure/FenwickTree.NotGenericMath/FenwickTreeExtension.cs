using AtCoder;
using AtCoder.Internal;
using AtCoder.Operators;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class FenwickTreeExtension
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
            for (int k = 1 << BitOperationsEx.MSB(fw.data.Length - 1); k > 0; k >>= 1)
            {
                var nx = x + k;
                if (nx < fw.data.Length && op.LessThan(fw.data[nx], v))
                {
                    x = nx;
                    v = op.Subtract(v, fw.data[nx]);
                }
            }
            return x;
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
            if (op.LessThanOrEqual(v, default)) return 0;
            int x = 0;
            for (int k = 1 << BitOperationsEx.MSB(fw.data.Length - 1); k > 0; k >>= 1)
            {
                var nx = x + k;
                if (nx < fw.data.Length && op.LessThanOrEqual(fw.data[nx], v))
                {
                    x = nx;
                    v = op.Subtract(v, fw.data[nx]);
                }
            }
            return x;
        }

        /// <summary>
        /// <c>(<paramref name="fw"/>[i..(i+1)], <paramref name="fw"/>[..(i+1)])</c> の配列にして返す。
        /// </summary>
        /// <returns></returns>
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
