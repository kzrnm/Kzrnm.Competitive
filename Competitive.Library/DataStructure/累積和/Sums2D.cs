using AtCoder;
using AtCoder.Operators;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 二次元累積和を求めます。
    /// </summary>
    public class Sums2D<TValue, TOp>
        where TOp : struct, IAdditionOperator<TValue>, ISubtractOperator<TValue>
    {
        private static readonly TOp op = default;
        private readonly TValue[][] impl;
        public int Length => impl.Length - 1;
        public Sums2D(TValue[][] arr, TValue defaultValue = default)
        {
            impl = new TValue[arr.Length + 1][];
            impl[0] = new TValue[arr[0].Length + 1].Fill(defaultValue);
            for (var i = 0; i < arr.Length; i++)
            {
                impl[i + 1] = new TValue[arr[i].Length + 1];
                impl[i + 1][0] = defaultValue;
                for (var j = 0; j < arr[i].Length; j++)
                    impl[i + 1][j + 1]
                        = op.Add(
                            op.Subtract(
                                op.Add(impl[i + 1][j],
                                    impl[i][j + 1]),
                                impl[i][j]), arr[i][j]);
            }
        }
        public Slicer Slice(int left, int length) => new Slicer(impl, left, left + length);
        public readonly ref struct Slicer
        {
            readonly TValue[][] impl;
            readonly int left;
            readonly int rightExclusive;
            public int Length { get; }
            public Slicer(TValue[][] impl, int left, int rightExclusive)
            {
                this.impl = impl;
                this.left = left;
                this.rightExclusive = rightExclusive;
                this.Length = impl[0].Length - 1;
            }
            public TValue Slice(int top, int length)
            {
                var bottomExclusive = top + length;
                return op.Add(
                    op.Subtract(
                        op.Subtract(impl[rightExclusive][bottomExclusive],
                            impl[left][bottomExclusive]),
                        impl[rightExclusive][top]),
                    impl[left][top]);
            }
        }
    }
}
