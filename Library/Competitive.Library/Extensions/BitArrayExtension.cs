﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __BitArrayExtension
    {
        [凾(256)]
        public static void CopyTo(this BitArray b, int[] array, int index = 0) => b.CopyTo((Array)array, index);
        [凾(256)]
        public static void CopyTo(this BitArray b, uint[] array, int index = 0) => b.CopyTo((Array)array, index);
        [凾(256)]
        public static void CopyTo(this BitArray b, bool[] array, int index = 0) => b.CopyTo((Array)array, index);
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
            var arr = new uint[((b.Length + 63) >> 6) * 2];
            if (arr.Length == 0) return Array.Empty<int>();
            b.CopyTo(arr);
            var brr = MemoryMarshal.Cast<uint, ulong>(arr);
            var l = new List<int>(b.Length);
            for (var i = 0; i < brr.Length; ++i)
                foreach (var bit in brr[i].Bits())
                    l.Add(bit + 64 * i);
            return l.AsSpan().ToArray();
        }
    }
}
