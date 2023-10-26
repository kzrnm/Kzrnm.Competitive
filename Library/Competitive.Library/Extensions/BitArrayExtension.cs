using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BitArray = System.Collections.BitArray;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __BitArrayExtension
    {
#pragma warning disable CS0649
        private class Dummy
        {
            public int[] arr;
            public int len;
            public int ver;
        }
#pragma warning restore CS0649
        [凾(256)]
        public static int[] GetArray(this BitArray b) => Unsafe.As<Dummy>(b).arr;

        [凾(256)]
        public static void CopyTo(this BitArray b, int[] array, int index = 0) => b.CopyTo(array, index);
        [凾(256)]
        public static void CopyTo(this BitArray b, uint[] array, int index = 0) => b.CopyTo(array, index);
        [凾(256)]
        public static void CopyTo(this BitArray b, bool[] array, int index = 0) => b.CopyTo(array, index);
        /// <summary>
        /// 内部の配列を <see langword="uint"/> の配列にコピーします。
        /// </summary>
        [凾(256)]
        public static uint[] ToUInt32Array(this BitArray b)
        {
            var arr = new uint[(b.Length + 31) / 32];
            b.CopyTo(arr);
            return arr;
        }

        /// <summary>
        /// フラグの立っているインデックスを列挙します。
        /// </summary>
        [凾(256)]
        public static int[] OnBits(this BitArray b)
        {
            var arr = b.GetArray();
            var brr = MemoryMarshal.Cast<int, ulong>(arr);
            var l = new List<int>(b.Length);
            for (var i = 0; i < brr.Length; ++i)
                foreach (var bit in brr[i].Bits())
                {
                    var bb = bit + 64 * i;
                    if (bb >= b.Length) break;
                    l.Add(bb);
                }
            if ((arr.Length & 1) != 0)
                foreach (var bit in arr[^1].Bits())
                {
                    var bb = bit + 64 * brr.Length;
                    if (bb >= b.Length) break;
                    l.Add(bb);
                }
            return l.AsSpan().ToArray();
        }

        /// <summary>
        /// フラグの立っているインデックスの数を数えます。
        /// </summary>
        [凾(256)]
        public static int PopCount(this BitArray b)
        {
            var arr = b.GetArray().AsSpan();
            var rem = b.Length & 31;
            int cnt = rem == 0 ? 0 : -BitOperations.PopCount((uint)((-1 << rem) & arr[^1]));
            if ((arr.Length & 1) != 0)
            {
                cnt += BitOperations.PopCount((uint)arr[^1]);
                arr = arr[..^1];
            }
            var brr = MemoryMarshal.Cast<int, ulong>(arr);
            foreach (var bb in brr)
                cnt += BitOperations.PopCount(bb);
            return cnt;
        }

        /// <summary>
        /// フラグの立っている最下位インデックス
        /// </summary>
        [凾(256)]
        public static int Lsb(this BitArray b)
        {
            var arr = b.GetArray().AsSpan();
            var brr = MemoryMarshal.Cast<int, ulong>(arr);
            for (int i = 0; i < brr.Length; i++)
                if (brr[i] != 0)
                    return Math.Min(b.Length, BitOperations.TrailingZeroCount(brr[i]) + i * 64);
            if ((arr.Length & 1) != 0)
                return Math.Min(b.Length, BitOperations.TrailingZeroCount((uint)arr[^1]) + brr.Length * 64);
            return b.Length;
        }

        /// <summary>
        /// フラグの立っている最上位インデックス
        /// </summary>
        [凾(256)]
        public static int Msb(this BitArray b)
        {
            var arr = b.GetArray().AsSpan();
            var rem = b.Length & 31;
            var m = (uint)arr[^1] & ((1u << rem) - 1);
            if (rem == 0)
                m = (uint)arr[^1];
            if (m != 0)
                return BitOperations.Log2(m) + 32 * (arr.Length - 1);
            arr = arr[..^1];

            if ((arr.Length & 1) != 0)
            {
                m = (uint)arr[^1];
                if (m != 0)
                    return BitOperations.Log2(m) + 32 * (arr.Length - 1);
            }
            var brr = MemoryMarshal.Cast<int, ulong>(arr);
            for (int i = brr.Length - 1; i >= 0; i--)
                if (brr[i] != 0)
                    return Math.Min(b.Length, BitOperations.Log2(brr[i]) + i * 64);
            return b.Length;
        }
    }
}
