using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE Span<T> への Select
    public static class __CollectionExtension_SpanSelect
    {
        [凾(256)]
        public static TResult[] Select<TSource, TResult>(this Span<TSource> source, Func<TSource, TResult> selector)
            => Select((ReadOnlySpan<TSource>)source, selector);
        [凾(256)]
        public static TResult[] Select<TSource, TResult>(this ReadOnlySpan<TSource> source, Func<TSource, TResult> selector)
        {
            var res = new TResult[source.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = selector(source[i]);
            return res;
        }
        [凾(256)]
        public static TResult[] Select<TSource, TResult>(this Span<TSource> source, Func<TSource, int, TResult> selector)
            => Select((ReadOnlySpan<TSource>)source, selector);
        [凾(256)]
        public static TResult[] Select<TSource, TResult>(this ReadOnlySpan<TSource> source, Func<TSource, int, TResult> selector)
        {
            var res = new TResult[source.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = selector(source[i], i);
            return res;
        }
    }
}
