using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class Bit
    {
#if NET8_0_OR_GREATER
        /// <summary>
        /// <paramref name="num"/> を長さ <see cref="Unsafe.SizeOf{T}()"/> × 8 の 2 進数文字列にします。
        /// </summary>
        [凾(256)]
        public static string ToBitString<T>(this T num) where T : IBinaryInteger<T>
            => ToBitString(num, Unsafe.SizeOf<T>() * 8);
        /// <summary>
        /// <paramref name="num"/> を長さ <paramref name="size"/> 以上の 2 進数文字列にします。
        /// </summary>
        [凾(256)]
        public static string ToBitString<T>(this T num, int size) where T : IBinaryInteger<T>
        {
            return num.ToString($"B{size}", null);
        }
#else
        /// <summary>
        /// <paramref name="num"/> を長さ <paramref name="size"/> 以上の 2 進数文字列にします。
        /// </summary>
        [凾(256)]
        public static string ToBitString(this int num, int size = sizeof(int) * 8) => Convert.ToString(num, 2).PadLeft(size, '0');
        /// <summary>
        /// <paramref name="num"/> を長さ <paramref name="size"/> 以上の 2 進数文字列にします。
        /// </summary>
        [凾(256)]
        public static string ToBitString(this uint num, int size = sizeof(uint) * 8) => Convert.ToString((int)num, 2).PadLeft(size, '0');
        /// <summary>
        /// <paramref name="num"/> を長さ <paramref name="size"/> 以上の 2 進数文字列にします。
        /// </summary>
        [凾(256)]
        public static string ToBitString(this long num, int size = sizeof(long) * 8) => Convert.ToString(num, 2).PadLeft(size, '0');
        /// <summary>
        /// <paramref name="num"/> を長さ <paramref name="size"/> 以上の 2 進数文字列にします。
        /// </summary>
        [凾(256)]
        public static string ToBitString(this ulong num, int size = sizeof(ulong) * 8) => Convert.ToString(unchecked((long)num), 2).PadLeft(size, '0');
#endif

        /// <summary>
        /// <paramref name="num"/> の <paramref name="index"/> 番目のビットが立っているかを返します。
        /// </summary>
        [凾(256)]
        public static bool On<T>(this T num, int index) where T : IBinaryInteger<T> => T.IsOddInteger(num >> index);

        /// <summary>
        /// <paramref name="num"/> の立っているビットを列挙します。
        /// </summary>
        [凾(256)]
        public static Enumerator Bits(this int num) => new Enumerator((uint)num);
        /// <summary>
        /// <paramref name="num"/> の立っているビットを列挙します。
        /// </summary>
        [凾(256)]
        public static Enumerator Bits(this uint num) => new Enumerator(num);
        /// <summary>
        /// <paramref name="num"/> の立っているビットを列挙します。
        /// </summary>
        [凾(256)]
        public static Enumerator Bits(this long num) => new Enumerator((ulong)num);
        /// <summary>
        /// <paramref name="num"/> の立っているビットを列挙します。
        /// </summary>
        [凾(256)]
        public static Enumerator Bits(this ulong num) => new Enumerator(num);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
        public struct Enumerator : IEnumerable<int>, IEnumerator<int>
        {
            ulong num;
            public Enumerator(ulong num) { this.num = num; Current = -1; }
            public Enumerator GetEnumerator() => this;
            public int Current { get; private set; }
            [凾(256)]
            public bool MoveNext()
            {
                if (num == 0) return false;
                Current = BitOperations.TrailingZeroCount(num);
                num &= num - 1;
                return true;
            }

            object IEnumerator.Current => Current;
            public int[] ToArray()
            {
                var res = new int[BitOperations.PopCount(num)];
                for (int i = 0; i < res.Length; i++)
                {
                    MoveNext();
                    res[i] = Current;
                }
                return res;
            }
            void IEnumerator.Reset() => throw new NotSupportedException();

            void IDisposable.Dispose() { }

            IEnumerator<int> IEnumerable<int>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
        }
    }
}