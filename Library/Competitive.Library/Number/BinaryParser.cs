using System;
using BitArray = System.Collections.BitArray;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
using System.Diagnostics;
using System.Numerics;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 2進数表記を取り扱います。
    /// </summary>
    public static class BinaryParser
    {

#if NET8_0_OR_GREATER
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        public static T ParseNumber<T>(ReadOnlySpan<char> s) where T : IBinaryInteger<T>
            => T.Parse(s, System.Globalization.NumberStyles.BinaryNumber);
#elif NET7_0_OR_GREATER
        /// <summary>
        /// 2進数表記の数値を <typeparamref name="T"/> に変換します。
        /// </summary>
        [凾(256)]
        public static T ParseNumber<T>(ReadOnlySpan<char> s) where T : IBinaryInteger<T>
        {
            s = s.Trim().TrimStart('0');
            var v = T.Zero;
            while (s.Length > 0)
            {
                var r = s.IndexOf('1');
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
        public static uint ParseUInt32(ReadOnlySpan<char> s)
            => ParseNumber<uint>(s);

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
        public static BitArray ParseBitArray(ReadOnlySpan<char> s)
        {
            s = s.Trim();
            var length = s.Length;
            var a = new uint[(s.Length + 31) / 32];

            for (int i = 0; i < a.Length; i++)
            {
                ReadOnlySpan<char> t;
                if (s.Length < 32)
                    break;
                else
                {
                    t = s[..32];
                    s = s[32..];
                }
                uint x = ParseNumber<uint>(t);
                a[i] = BitOperationsEx.BitReverse(x);
            }

            Debug.Assert(s.Length < 32);
            Debug.Assert(s.Length == 0 || a[^1] == 0);
            if (s.Length > 0)
            {
                Span<char> t = stackalloc char[32];
                s.CopyTo(t);
                t[s.Length..].Fill('0');
                uint x = ParseNumber<uint>(t);
                a[^1] = BitOperationsEx.BitReverse(x);
            }

            return new BitArray((int[])(object)a) { Length = length };
        }
    }
}
