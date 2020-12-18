namespace AtCoder
{
    /// <summary>
    /// 二次元累積和を求めます。
    /// </summary>
    public class Sums2D
    {
        readonly long[][] impl;
        public int Length => impl.Length - 1;
        public Sums2D(int[][] arr)
        {
            impl = new long[arr.Length + 1][];
            impl[0] = new long[arr[0].Length + 1];
            for (var i = 0; i < arr.Length; i++)
            {
                impl[i + 1] = new long[arr[i].Length + 1];
                for (var j = 0; j < arr[i].Length; j++)
                    impl[i + 1][j + 1] = impl[i + 1][j] + impl[i][j + 1] - impl[i][j] + arr[i][j];
            }
        }
        public Slicer Slice(int left, int length) => new Slicer(impl, left, left + length);
        public readonly ref struct Slicer
        {
            readonly long[][] impl;
            readonly int left;
            readonly int rightExclusive;
            public int Length { get; }
            public Slicer(long[][] impl, int left, int rightExclusive)
            {
                this.impl = impl;
                this.left = left;
                this.rightExclusive = rightExclusive;
                this.Length = impl[0].Length - 1;
            }
            public long Slice(int top, int length)
            {
                var bottomExclusive = top + length;
                return impl[rightExclusive][bottomExclusive]
                         - impl[left][bottomExclusive]
                         - impl[rightExclusive][top]
                         + impl[left][top];
            }
        }
    }
}
