using Kzrnm.Competitive.IO;
using System;
using System.Diagnostics;
using System.Numerics;
using BitArray = System.Collections.BitArray;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


#if NET8_0_OR_GREATER
using System.Globalization;
using System.Runtime.InteropServices;
#endif

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 2進数表記を取り扱います。
    /// </summary>
    public static class BinaryParser
    {
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        public static T ParseNumber<T>(Asciis s) where T : IBinaryInteger<T>
            => ParseNumber<T>(s.d);
#if NET8_0_OR_GREATER
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        public static T ParseNumber<T>(ReadOnlySpan<byte> s) where T : IBinaryInteger<T>
            => T.Parse(s, NumberStyles.BinaryNumber, null);
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        public static T ParseNumber<T>(ReadOnlySpan<char> s) where T : IBinaryInteger<T>
            => T.Parse(s, NumberStyles.BinaryNumber, null);

        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        internal static T ParseNumber<T, U>(ReadOnlySpan<U> s) where T : IBinaryInteger<T> where U : struct, INumber<U>
        {
            if (typeof(U) == typeof(char))
                return ParseNumber<T>(MemoryMarshal.Cast<U, char>(s));
            if (typeof(U) == typeof(byte))
                return ParseNumber<T>(MemoryMarshal.Cast<U, byte>(s));
            Throw();
            return default;
            void Throw() => throw new InvalidOperationException();
        }

#elif NET7_0_OR_GREATER
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        public static T ParseNumber<T>(ReadOnlySpan<byte> s) where T : IBinaryInteger<T>
            => ParseNumber<T, byte>(s);
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        public static T ParseNumber<T>(ReadOnlySpan<char> s) where T : IBinaryInteger<T>
            => ParseNumber<T, char>(s);
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        internal static T ParseNumber<T, U>(ReadOnlySpan<U> s) where T : IBinaryInteger<T> where U : INumber<U>
        {
            s = Trim(s).TrimStart(Cnv<U, char>('0'));
            var v = T.Zero;
            while (s.Length > 0)
            {
                var r = s.IndexOf(Cnv<U, char>('1'));
                if (r < 0)
                {
                    v <<= s.Length;
                    break;
                }
                v = (v << (r + 1)) | T.One;
                s = s[(r + 1)..];
            }
            return v;
        }
#endif
        /// <summary>
        /// 2進数表記の数値を <see cref="uint"/> に変換します。
        /// </summary>
        [凾(256)]
        public static uint ParseUInt32(ReadOnlySpan<byte> s)
            => ParseNumber<uint>(s);

        /// <summary>
        /// 2進数表記の数値を <see cref="uint"/> に変換します。
        /// </summary>
        [凾(256)]
        public static uint ParseUInt32(ReadOnlySpan<char> s)
            => ParseNumber<uint>(s);

        /// <summary>
        /// 2進数表記の数値を <see cref="ulong"/> に変換します。
        /// </summary>
        [凾(256)]
        public static ulong ParseUInt64(ReadOnlySpan<byte> s)
            => ParseNumber<ulong>(s);

        /// <summary>
        /// 2進数表記の数値を <see cref="ulong"/> に変換します。
        /// </summary>
        [凾(256)]
        public static ulong ParseUInt64(ReadOnlySpan<char> s)
            => ParseNumber<ulong>(s);

        /// <summary>
        /// 2進数表記の数値を <see cref="BitArray"/> に変換します。
        /// </summary>
        [凾(256)]
        public static BitArray ParseBitArray(Asciis s)
            => ParseBitArray(s.d);
        /// <summary>
        /// 2進数表記の数値を <see cref="BitArray"/> に変換します。
        /// </summary>
        [凾(256)]
        public static BitArray ParseBitArray(ReadOnlySpan<byte> s)
            => ParseBitArray<byte>(s);
        /// <summary>
        /// 2進数表記の数値を <see cref="BitArray"/> に変換します。
        /// </summary>
        [凾(256)]
        public static BitArray ParseBitArray(ReadOnlySpan<char> s)
            => ParseBitArray<char>(s);
        [凾(256)]
        static BitArray ParseBitArray<T>(ReadOnlySpan<T> s) where T : unmanaged, INumber<T>
        {
            s = Trim(s);
            var length = s.Length;
            var a = new uint[(s.Length + 31) / 32];

            for (int i = 0; i < a.Length; i++)
            {
                ReadOnlySpan<T> t;
                if (s.Length < 32)
                    break;
                else
                {
                    t = s[..32];
                    s = s[32..];
                }
                uint x = ParseNumber<uint, T>(t);
                a[i] = BitOperationsEx.BitReverse(x);
            }

            Debug.Assert(s.Length < 32);
            Debug.Assert(s.Length == 0 || a[^1] == 0);
            if (s.Length > 0)
            {
                Span<T> t = stackalloc T[32];
                s.CopyTo(t);
                t[s.Length..].Fill(Cnv<T, char>('0'));
                uint x = ParseNumber<uint, T>(t);
                a[^1] = BitOperationsEx.BitReverse(x);
            }

            return new BitArray((int[])(object)a) { Length = length };
        }

        [凾(256)]
        internal static ReadOnlySpan<T> Trim<T>(this ReadOnlySpan<T> s)
            where T : INumber<T>
        {
            while (s.Length > 0 && char.IsWhiteSpace(Cnv<char, T>(s[0])))
                s = s[1..];
            while (s.Length > 0 && char.IsWhiteSpace(Cnv<char, T>(s[^1])))
                s = s[..^1];
            return s;
        }
        [凾(256)]
        static T Cnv<T, U>(U c)
            where T : INumber<T>
            where U : INumber<U> => T.CreateChecked(c);
    }
}
