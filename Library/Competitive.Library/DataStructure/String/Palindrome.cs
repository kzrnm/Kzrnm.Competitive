using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 回文
    /// </summary>
    public static class Palindrome
    {
        /// <summary>
        /// <paramref name="s"/> が回文か判定します。
        /// </summary>
        public static bool IsPalindrome(string s) => IsPalindrome((ReadOnlySpan<char>)s);
        /// <summary>
        /// <paramref name="s"/> が回文か判定します。
        /// </summary>
        public static bool IsPalindrome<T>(T[] s) => IsPalindrome((ReadOnlySpan<T>)s);
        /// <summary>
        /// <paramref name="s"/> が回文か判定します。
        /// </summary>
        public static bool IsPalindrome<T>(Span<T> s) => IsPalindrome((ReadOnlySpan<T>)s);
        /// <summary>
        /// <paramref name="s"/> が回文か判定します。
        /// </summary>
        public static bool IsPalindrome<T>(ReadOnlySpan<T> s)
        {
            for (int i = 0; i < s.Length - i - 1; i++)
                if (!EqualityComparer<T>.Default.Equals(s[i], s[s.Length - i - 1]))
                    return false;
            return true;
        }
    }
}
