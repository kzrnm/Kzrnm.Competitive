using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using BitArray = System.Collections.BitArray;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __BitArrayExtension
    {
#pragma warning disable CS0649
        class Dummy
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
        /// <see cref="BitArray"/> を2進数表記の数値文字列にして返します。
        /// </summary>
        [凾(256)]
        public static bool TryBitFormat(this BitArray b, Span<char> dst, out int written)
        {
            written = 0;
            if (dst.Length < b.Length) return false;
            if (b.Length == 0) return true;

            var a = ToUInt32Array(b);

#if NET8_0_OR_GREATER
            {
                Span<char> d = stackalloc char[32];
                a[^1].TryFormat(d, out var ww, "B32", CultureInfo.InvariantCulture);
                d.Reverse();
                if ((b.Length & 31) is not 0 and var w2)
                    d = d[..w2];
                d.CopyTo(dst[(32 * (a.Length - 1))..]);
            }
            for (int i = a.Length - 2; i >= 0; i--)
            {
                var d = dst.Slice(i << 5, 32);
                a[i].TryFormat(d, out var ww, "B32", CultureInfo.InvariantCulture);
                Debug.Assert(ww == 32);
                d.Reverse();
            }
#else
            {
                var w = Convert.ToString(a[^1], 2);
                var d = dst[(32 * (a.Length - 1))..];
                Debug.Assert(w.Length <= d.Length);
                w.CopyTo(d);
                d[..w.Length].Reverse();
                d[w.Length..((b.Length & 31) switch
                {
                    0 => 32,
                    var v => v,
                })].Fill('0');
            }
            for (int i = a.Length - 2; i >= 0; i--)
            {
                var d = dst.Slice(i << 5, 32);
                var w = Convert.ToString(a[i], 2);
                Debug.Assert(w.Length <= 32);
                w.CopyTo(d);
                d[..w.Length].Reverse();
                d[w.Length..].Fill('0');
            }
#endif
            written = b.Length;
            return true;
        }

        /// <summary>
        /// <see cref="BitArray"/> を2進数表記の数値文字列にして返します。
        /// </summary>
        [凾(256)]
        public static string ToBitString(this BitArray b)
        {
            var rt = new char[b.Length];
            TryBitFormat(b, rt, out _);
            return new(rt);
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
