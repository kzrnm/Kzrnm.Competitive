using AtCoder.Algebra;
using static AtCoder.Global;

namespace AtCoder.DataStructure
{
    public static class FenwickTreeExtension
    {
        public static int LowerBound<TValue, TOp>(this FenwickTree<TValue, TOp> fw, TValue w)
            where TOp : IAddOperator<TValue>, ISubtractOperator<TValue>, IComparisonOperator<TValue>
        {
            var op = default(TOp);
            if (op.LessThanOrEqual(w, default)) return 0;
            int x = 0;
            for (int k = 1 << MSB(fw.data.Length - 1); k > 0; k >>= 1)
            {
                var nx = x + k;
                if (nx < fw.data.Length && op.LessThan(fw.data[nx], w))
                {
                    x = nx;
                    w = op.Subtract(w, fw.data[nx]);
                }
            }
            return x;
        }
    }
}
