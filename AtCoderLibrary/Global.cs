using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    public static class Global
    {
        public static int Gcd(params int[] nums)
        {
            var gcd = nums[0];
            for (var i = 1; i < nums.Length; i++)
                gcd = Gcd(nums[i], gcd);
            return gcd;
        }
        public static int Gcd(int a, int b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
        public static int Lcm(int a, int b) => a / Gcd(a, b) * b;
        public static int Lcm(params int[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }
        public static long Gcd(params long[] nums)
        {
            var gcd = nums[0];
            for (var i = 1; i < nums.Length; i++)
                gcd = Gcd(nums[i], gcd);
            return gcd;
        }
        public static long Gcd(long a, long b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
        public static long Lcm(long a, long b) => a / Gcd(a, b) * b;

        public static long Lcm(params long[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }

        public static T[] NewArray<T>(int len0, T value) where T : struct => new T[len0].Fill(value);
        public static T[] NewArray<T>(int len0, Func<T> factory)
        {
            var arr = new T[len0];
            for (int i = 0; i < arr.Length; i++) arr[i] = factory();
            return arr;
        }
        public static T[][] NewArray<T>(int len0, int len1, T value) where T : struct
        {
            var arr = new T[len0][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, value);
            return arr;
        }
        public static T[][] NewArray<T>(int len0, int len1, Func<T> factory)
        {
            var arr = new T[len0][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, factory);
            return arr;
        }
        public static T[][][] NewArray<T>(int len0, int len1, int len2, T value) where T : struct
        {
            var arr = new T[len0][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, value);
            return arr;
        }
        public static T[][][] NewArray<T>(int len0, int len1, int len2, Func<T> factory)
        {
            var arr = new T[len0][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, factory);
            return arr;
        }
        public static T[][][][] NewArray<T>(int len0, int len1, int len2, int len3, T value) where T : struct
        {
            var arr = new T[len0][][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, len3, value);
            return arr;
        }
        public static T[][][][] NewArray<T>(int len0, int len1, int len2, int len3, Func<T> factory)
        {
            var arr = new T[len0][][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, len3, factory);
            return arr;
        }


        public static long Pow(long x, int y)
        {
            long res = 1;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1) res *= x;
                x *= x;
            }
            return res;
        }
#pragma warning disable IDE0057
        public static BigInteger ParseBigInteger(ReadOnlySpan<char> s)
        {
            /* 自前実装の方が速い */
            if (s[0] == '-') return -ParseBigInteger(s[1..]);
            BigInteger res;
            if (s.Length % 9 == 0)
                res = 0;
            else
            {
                res = new BigInteger(int.Parse(s.Slice(0, s.Length % 9)));
                s = s.Slice(s.Length % 9);
            }

            while (s.Length > 0)
            {
                var sp = s.Slice(0, 9);
                res *= 1000_000_000;
                res += int.Parse(sp);
                s = s.Slice(9);
            }
            return res;
        }
#pragma warning restore IDE0057
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(int x) => BitOperations.PopCount((uint)x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(long x) => BitOperations.PopCount((ulong)x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(ulong x) => BitOperations.PopCount(x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MSB(int x) => BitOperations.Log2((uint)x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MSB(uint x) => BitOperations.Log2(x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MSB(long x) => BitOperations.Log2((ulong)x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MSB(ulong x) => BitOperations.Log2(x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LSB(int x) => BitOperations.TrailingZeroCount((uint)x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LSB(uint x) => BitOperations.TrailingZeroCount(x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LSB(long x) => BitOperations.TrailingZeroCount((ulong)x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LSB(ulong x) => BitOperations.TrailingZeroCount(x);

        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        public static Dictionary<T, int> Compress<T>(IEnumerable<T> orig) where T : IComparable<T> => Compress(orig, Comparer<T>.Default);

        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        public static Dictionary<T, int> Compress<T>(ReadOnlySpan<T> orig) where T : IComparable<T> => Compress(orig, Comparer<T>.Default);

        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        public static Dictionary<T, int> Compress<T>(T[] orig) where T : IComparable<T> => Compress(orig.AsSpan(), Comparer<T>.Default);

        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        public static Dictionary<T, int> Compress<T>(IEnumerable<T> orig, IComparer<T> comparer)
        {
            var ox = new HashSet<T>(orig).ToArray();
            Array.Sort(ox, comparer);
            var zip = new Dictionary<T, int>();
            for (int i = 0; i < ox.Length; i++)
                zip[ox[i]] = i;
            return zip;
        }
        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        public static Dictionary<T, int> Compress<T>(ReadOnlySpan<T> orig, IComparer<T> comparer)
        {
            var hs = new HashSet<T>(orig.Length);
            foreach (var v in orig)
                hs.Add(v);
            var ox = hs.ToArray();
            Array.Sort(ox, comparer);
            var zip = new Dictionary<T, int>();
            for (int i = 0; i < ox.Length; i++)
                zip[ox[i]] = i;
            return zip;
        }
        public static int[] Compressed<T>(ReadOnlySpan<T> orig) where T : IComparable<T>
        {
            static int[] Compressed(ReadOnlySpan<T> orig, Dictionary<T, int> zip)
            {
                var res = new int[orig.Length];
                for (int i = 0; i < res.Length; i++)
                    res[i] = zip[orig[i]];
                return res;
            }
            return Compressed(orig, Compress(orig, Comparer<T>.Default));
        }
    }
}
