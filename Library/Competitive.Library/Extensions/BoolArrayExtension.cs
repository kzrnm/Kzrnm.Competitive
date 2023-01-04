using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __BoolArrayExtension
    {
        /// <summary>
        /// 長さ <paramref name="length"/> の bool 配列を作成し、<paramref name="nums"/> に含まれるインデックスを true に設定します。
        /// </summary>
        [凾(256)]
        public static bool[] ToBoolArray(this int[] nums, int length)
            => ToBoolArray((ReadOnlySpan<int>)nums, length);
        /// <summary>
        /// 長さ <paramref name="length"/> の bool 配列を作成し、<paramref name="nums"/> に含まれるインデックスを true に設定します。
        /// </summary>
        [凾(256)]
        public static bool[] ToBoolArray(this Span<int> nums, int length)
            => ToBoolArray((ReadOnlySpan<int>)nums, length);
        /// <summary>
        /// 長さ <paramref name="length"/> の bool 配列を作成し、<paramref name="nums"/> に含まれるインデックスを true に設定します。
        /// </summary>
        [凾(256)]
        public static bool[] ToBoolArray(this ReadOnlySpan<int> nums, int length)
        {
            var a = new bool[length];
            foreach (var n in nums)
                if ((uint)n < (uint)a.Length)
                    a[n] = true;
            return a;
        }
        /// <summary>
        /// 長さ <paramref name="length"/> の bool 配列を作成し、<paramref name="nums"/> に含まれるインデックスを true に設定します。
        /// </summary>
        [凾(256)]
        public static bool[] ToBoolArray(this IEnumerable<int> nums, int length)
        {
            var a = new bool[length];
            foreach (var n in nums)
                if ((uint)n < (uint)a.Length)
                    a[n] = true;
            return a;
        }
    }
}
