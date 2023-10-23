using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __Xoshiro256Extension
    {
        /// <summary>
        /// [0, <paramref name="max"/>) の乱数を返します。
        /// </summary>
        [凾(256)]
        public static long NextInt64(this Xoshiro256 r, long max)
            => (long)r.NextUInt64((ulong)max);

        /// <summary>
        /// [<paramref name="min"/>, <paramref name="max"/>) の乱数を返します。
        /// </summary>
        [凾(256)]
        public static long NextInt64(this Xoshiro256 r, long min, long max)
            => (long)r.NextUInt64((ulong)(max - min)) + min;

        /// <summary>
        /// [0, <paramref name="max"/>) の乱数を返します。
        /// </summary>
        [凾(256)]
        public static int NextInt32(this Xoshiro256 r, int max)
            => (int)r.NextUInt64((uint)max);

        /// <summary>
        /// [<paramref name="min"/>, <paramref name="max"/>) の乱数を返します。
        /// </summary>
        [凾(256)]
        public static int NextInt32(this Xoshiro256 r, int min, int max)
            => (int)r.NextUInt64((uint)(max - min)) + min;
    }
}