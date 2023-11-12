using Kzrnm.Competitive;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class ZetaMoebiusTransform
    {
        static void ThrowNotPow2(string n) => throw new ArgumentException("A length of input must be power of 2.", n);

        /// <summary>
        /// <paramref name="a"/> を b[S] = ∑ S⊂T <paramref name="a"/>[T] となる b に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="a"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SupersetZetaTransform<T>(T[] a) where T : IAdditionOperators<T, T, T>
            => SupersetZetaTransform(a.AsSpan());
        /// <summary>
        /// <paramref name="a"/> を b[S] = ∑ S⊂T <paramref name="a"/>[T] となる b に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="a"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SupersetZetaTransform<T>(Span<T> a) where T : IAdditionOperators<T, T, T>
        {
            if (!BitOperations.IsPow2(a.Length)) ThrowNotPow2(nameof(a));
            for (int i = 1; i < a.Length; i <<= 1)
                foreach (var j in (~i & a.Length - 1).BitSubset(false))
                    a[j] += a[j | i];
        }

        /// <summary>
        /// <paramref name="b"/>[S] = ∑ S⊂T a[T] となっている <paramref name="b"/> を a に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="b"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="b"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SupersetMoebiusTransform<T>(T[] b) where T : ISubtractionOperators<T, T, T>
            => SupersetMoebiusTransform(b.AsSpan());
        /// <summary>
        /// <paramref name="b"/>[S] = ∑ S⊂T a[T] となっている <paramref name="b"/> を a に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="b"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="b"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SupersetMoebiusTransform<T>(Span<T> b) where T : ISubtractionOperators<T, T, T>
        {
            if (!BitOperations.IsPow2(b.Length)) ThrowNotPow2(nameof(b));
            for (int i = 1; i < b.Length; i <<= 1)
                foreach (var j in (~i & b.Length - 1).BitSubset(false))
                    b[j] -= b[j | i];
        }


        /// <summary>
        /// <paramref name="a"/> を b[S] = ∑ T⊂S <paramref name="a"/>[T] となる b に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="a"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SubsetZetaTransform<T>(T[] a) where T : IAdditionOperators<T, T, T>
            => SubsetZetaTransform(a.AsSpan());
        /// <summary>
        /// <paramref name="a"/> を b[S] = ∑ T⊂S <paramref name="a"/>[T] となる b に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="a"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SubsetZetaTransform<T>(Span<T> a) where T : IAdditionOperators<T, T, T>
        {
            if (!BitOperations.IsPow2(a.Length)) ThrowNotPow2(nameof(a));
            for (int i = 1; i < a.Length; i <<= 1)
                foreach (var j in (~i & a.Length - 1).BitSubset(false))
                    a[j | i] += a[j];
        }

        /// <summary>
        /// <paramref name="b"/>[S] = ∑ T⊂S a[T] となっている <paramref name="b"/> を a に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="b"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="b"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SubsetMoebiusTransform<T>(T[] b) where T : ISubtractionOperators<T, T, T>
            => SubsetMoebiusTransform(b.AsSpan());
        /// <summary>
        /// <paramref name="b"/>[S] = ∑ T⊂S a[T] となっている <paramref name="b"/> を a に変換します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- |<paramref name="b"/>| は 2の冪乗</para>
        /// <para>計算量: O(3^n) ただし |<paramref name="b"/>| = 2^n</para>
        /// </remarks>
        [凾(256)]
        public static void SubsetMoebiusTransform<T>(Span<T> b) where T : ISubtractionOperators<T, T, T>
        {
            if (!BitOperations.IsPow2(b.Length)) ThrowNotPow2(nameof(b));
            for (int i = 1; i < b.Length; i <<= 1)
                foreach (var j in (~i & b.Length - 1).BitSubset(false))
                    b[j | i] -= b[j];
        }
    }
}
