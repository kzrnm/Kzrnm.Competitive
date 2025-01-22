using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 二次元累積和を求めます。
    /// </summary>
    public static class Sums2D
    {
        /// <summary>
        /// <paramref name="orig"/> の累積和を返します。
        /// </summary>
        [凾(256)]
        public static T[][] Accumulate<T>(T[][] orig, T defaultValue = default)
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
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

        /// <summary>
        /// <paramref name="orig"/> の累積和を範囲演算で取得できるデータ構造を返します。
        /// </summary>
        [凾(256)]
        public static Sums2D<T> Create<T>(T[][] orig, T defaultValue = default)
            where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
            => new(Accumulate(orig, defaultValue));
    }
    /// <summary>
    /// 範囲演算で二次元累積和を取得します。
    /// </summary>
    public class Sums2D<T> where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
    {
        private readonly T[][] impl;
        public int Length => impl.Length - 1;
        internal Sums2D(T[][] accu)
        {
            impl = accu;
        }
        [凾(256)]
        public Slicer Slice(int left, int length) => new Slicer(impl, left, left + length);
        public readonly record struct Slicer(T[][] impl, int left, int rightExclusive)
        {
            public int Length => impl[0].Length - 1;
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
