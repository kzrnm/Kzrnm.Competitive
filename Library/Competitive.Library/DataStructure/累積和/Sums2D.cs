using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 二次元累積和を求めます。
    /// </summary>
    public class Sums2D<T>
        where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
    {
        private readonly T[][] impl;
        public int Length => impl.Length - 1;
        public Sums2D(T[][] arr, T defaultValue = default)
        {
            impl = Accumulate(arr, defaultValue);
        }
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        public static T[][] Accumulate(T[][] orig, T defaultValue = default)
        {
            var impl = new T[orig.Length + 1][];
            impl[0] = new T[orig[0].Length + 1].Fill(defaultValue);
            for (var i = 0; i < orig.Length; i++)
            {
                impl[i + 1] = new T[orig[i].Length + 1];
                impl[i + 1][0] = defaultValue;
                for (var j = 0; j < orig[i].Length; j++)
                    impl[i + 1][j + 1] =
                            impl[i + 1][j]
                            + impl[i][j + 1]
                            - impl[i][j]
                            + orig[i][j];
            }
            return impl;
        }
        [凾(256)]
        public Slicer Slice(int left, int length) => new Slicer(impl, left, left + length);
        public readonly ref struct Slicer
        {
            readonly T[][] impl;
            readonly int left;
            readonly int rightExclusive;
            public int Length { get; }
            [凾(256)]
            public Slicer(T[][] impl, int left, int rightExclusive)
            {
                this.impl = impl;
                this.left = left;
                this.rightExclusive = rightExclusive;
                Length = impl[0].Length - 1;
            }
            [凾(256)]
            public T Slice(int top, int length)
            {
                var bottomExclusive = top + length;
                return
                    impl[rightExclusive][bottomExclusive]
                    - impl[left][bottomExclusive]
                    - impl[rightExclusive][top]
                    + impl[left][top];
            }
        }
    }
}
