namespace AtCoder
{
    public static class FenwickTreeExtension
    {
        /// <summary>
        /// <paramref name="fw"/>.Sum(0, i) の値が <paramref name="v"/> 以上となる最小のインデックス i を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="fw"/> の値が非負。</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        public static int LowerBound<TValue, TOp>(this FenwickTree<TValue, TOp> fw, TValue v)
            where TValue : struct
            where TOp : struct, IArithmeticOperator<TValue>, ICompareOperator<TValue>
        {
            var op = default(TOp);
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
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        public static int UpperBound<TValue, TOp>(this FenwickTree<TValue, TOp> fw, TValue v)
            where TValue : struct
            where TOp : struct, IArithmeticOperator<TValue>, ICompareOperator<TValue>
        {
            var op = default(TOp);
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
    }
}
