namespace AtCoder.DataStructure
{
    /// <summary>
    /// 累積和を求めます。
    /// </summary>
    public class Sums
    {
        readonly long[] impl;
        public int Length => impl.Length - 1;
        public Sums(int[] arr)
        {
            impl = new long[arr.Length + 1];
            for (var i = 0; i < arr.Length; i++)
                impl[i + 1] = impl[i] + arr[i];
        }
        public long Slice(int from, int length) => impl[from + length] - impl[from];
        public long this[int toExclusive] => impl[toExclusive];
        public long this[int from, int toExclusive] => impl[toExclusive] - impl[from];
    }
}
