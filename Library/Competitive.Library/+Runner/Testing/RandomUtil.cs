using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing
{
    public static class RandomUtil
    {
        /// <summary>
        /// Fisher–Yates shuffle
        /// </summary>
        public static T[] Shuffle<T>(this Random rnd, T[] array)
        {
            Shuffle(rnd, array.AsSpan());
            return array;
        }
        /// <summary>
        /// Fisher–Yates shuffle
        /// </summary>
        public static Span<T> Shuffle<T>(this Random rnd, Span<T> span)
        {
            for (var i = span.Length - 1; i > 0; --i)
            {
                var a = i;
                var b = rnd.Next(i);
                (span[a], span[b]) = (span[b], span[a]);
            }
            return span;
        }

        /// <summary>
        /// 英小文字からなる長さ <paramref name="length"/> の <see cref="string"/> を返します。
        /// </summary>
        public static string NextString(this Random rnd, int length)
        {
            var arr = new char[length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (char)(rnd.Next(26) + 'a');
            return new string(arr);
        }

        /// <summary>
        /// [<paramref name="minValue"/>, <paramref name="maxValue"/>) かつ a &lt; b を満たす値 a, b を返します。
        /// </summary>
        public static (int, int) Choice2(this Random rnd, int minValue, int maxValue)
        {
            Contract.Assert(maxValue - minValue > 1);
            int a, b;
            do
            {
                a = rnd.Next(minValue, maxValue);
                b = rnd.Next(minValue, maxValue);
            } while (a == b);
            if (a > b) (a, b) = (b, a);
            return (a, b);
        }

        /// <summary>
        /// [<paramref name="minValue"/>, <paramref name="maxValue"/>) の範囲から <paramref name="n"/> 個を選んで返します。
        /// </summary>
        public static int[] Choice(this Random rnd, int n, int minValue, int maxValue)
        {
            var r = maxValue - minValue;
            Contract.Assert(n <= r);
            var list = new List<int>(Enumerable.Range(minValue, r));
            rnd.Shuffle(list.AsSpan());
            var span = list.AsSpan()[..n];
            span.Sort();
            return span.ToArray();
        }

        /// <summary>
        /// 長さ <paramref name="length"/> の配列を返します。
        /// </summary>
        public static int[] NextIntArray(this Random rnd, int length)
        {
            var arr = new int[length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = rnd.Next();
            return arr;
        }
        /// <summary>
        /// [<paramref name="minValue"/>, <paramref name="maxValue"/>) の値からなる長さ <paramref name="length"/> の配列を返します。
        /// </summary>
        public static int[] NextIntArray(this Random rnd, int length, int minValue, int maxValue)
        {
            var arr = new int[length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = rnd.Next(minValue, maxValue);
            return arr;
        }

        /// <summary>
        /// [0, <see cref="uint.MaxValue"/>] の値を返します。
        /// </summary>
        public static uint NextUInt(this Random rnd)
        {
            Span<byte> bytes = stackalloc byte[4];
            rnd.NextBytes(bytes);
            return MemoryMarshal.AsRef<uint>(bytes);
        }

        /// <summary>
        /// [0, <see cref="long.MaxValue"/>] の値を返します。
        /// </summary>
        public static long NextLong(this Random rnd)
        {
            Span<byte> bytes = stackalloc byte[8];
            rnd.NextBytes(bytes);
            return MemoryMarshal.AsRef<long>(bytes) & long.MaxValue;
        }

        /// <summary>
        /// [0, <paramref name="maxValue"/>] の値を返します。
        /// </summary>
        public static long NextLong(this Random rnd, long maxValue)
        {
            Span<byte> bytes = stackalloc byte[8];
            rnd.NextBytes(bytes);
            return (MemoryMarshal.AsRef<long>(bytes) & long.MaxValue) % maxValue;
        }
    }
}