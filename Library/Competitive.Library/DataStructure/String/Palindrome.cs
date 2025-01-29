using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 回文
    /// </summary>
    public static class Palindrome
    {
        /// <inheritdoc cref="IsPalindrome{T}(ReadOnlySpan{T})"/>
        public static bool IsPalindrome(string s) => IsPalindrome<char>(s);
        /// <inheritdoc cref="IsPalindrome{T}(ReadOnlySpan{T})"/>
        public static bool IsPalindrome<T>(T[] s) => IsPalindrome((ReadOnlySpan<T>)s);
        /// <inheritdoc cref="IsPalindrome{T}(ReadOnlySpan{T})"/>
        public static bool IsPalindrome<T>(Span<T> s) => IsPalindrome((ReadOnlySpan<T>)s);
        /// <summary>
        /// <paramref name="s"/> が回文か判定します。
        /// </summary>
        [凾(256)]
        public static bool IsPalindrome<T>(ReadOnlySpan<T> s)
        {
            for (int i = 0; i < s.Length - i - 1; i++)
                if (!EqualityComparer<T>.Default.Equals(s[i], s[s.Length - i - 1]))
                    return false;
            return true;
        }

        /// <inheritdoc cref="Manacher{T}(ReadOnlySpan{T})"/>
        public static int[] Manacher(string s) => Manacher(s.AsSpan());
        /// <inheritdoc cref="Manacher{T}(ReadOnlySpan{T})"/>
        public static int[] Manacher<T>(T[] s) => Manacher((ReadOnlySpan<T>)s);
        /// <inheritdoc cref="Manacher{T}(ReadOnlySpan{T})"/>
        public static int[] Manacher<T>(Span<T> s) => Manacher((ReadOnlySpan<T>)s);
        /// <summary>
        /// <paramref name="s"/>の i 文字目を中心とした回文の半径を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        /// <example>abcbaba → 1131221</example>
        [凾(256)]
        public static int[] Manacher<T>(ReadOnlySpan<T> s)
        {
            var rt = new int[s.Length];
            for (int i = 0, j = 0, k; i < s.Length; i += k, j -= k)
            {
                while (
                    (i - j) is var d && (uint)d < (uint)s.Length
                    && (i + j) is var e && (uint)e < (uint)s.Length
                    && EqualityComparer<T>.Default.Equals(s[d], s[e]))
                    ++j;
                rt[i] = j;

                k = 1;
                while (
                    (i - k) is var d && (uint)d < (uint)s.Length
                    && (i + k) is var e && (uint)e < (uint)s.Length
                    && k + rt[d] < j)
                {
                    rt[e] = rt[d];
                    ++k;
                }
            }
            return rt;
        }

        /// <inheritdoc cref="Manacher2{T}(ReadOnlySpan{T})"/>
        public static int[] Manacher2(Asciis s) => Manacher2(s.AsSpan());
        /// <inheritdoc cref="Manacher2{T}(ReadOnlySpan{T})"/>
        public static int[] Manacher2(string s) => Manacher2(s.AsSpan());
        /// <inheritdoc cref="Manacher2{T}(ReadOnlySpan{T})"/>
        public static int[] Manacher2<T>(T[] s) => Manacher2((ReadOnlySpan<T>)s);
        /// <inheritdoc cref="Manacher2{T}(ReadOnlySpan{T})"/>
        public static int[] Manacher2<T>(Span<T> s) => Manacher2((ReadOnlySpan<T>)s);
        /// <summary>
        /// <paramref name="s"/>の i/2 文字目、または i/2 文字目と i/2 + 1 文字目の間を中心とした回文の直径を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        /// <example>abccbc → 10101410301</example>
        [凾(256)]
        public static int[] Manacher2<T>(ReadOnlySpan<T> s)
        {
            if (s.Length == 0) return Array.Empty<int>();
            var t = new Manacher2Val<T>[s.Length * 2 + 1];
            ref var sp = ref MemoryMarshal.GetReference(s);
            for (int i = 1; i < t.Length; i += 2)
            {
                t[i] = new(Unsafe.Add(ref sp, i >> 1), true);
            }
            var r = Manacher(t);
            ref var rp = ref MemoryMarshal.GetReference<int>(r);
            var p = new int[s.Length * 2 - 1];
            Debug.Assert(p.Length == r.Length - 2);
            for (int i = 0; i < p.Length; i++)
                p[i] = Unsafe.Add(ref rp, i + 1) - 1;
            return p;
        }
        readonly record struct Manacher2Val<T>(T Value, bool HasValue);
    }
}
