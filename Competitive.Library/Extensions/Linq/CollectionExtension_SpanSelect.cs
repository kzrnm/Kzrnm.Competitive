using System;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
#pragma warning disable IDE1006
    public static class __CollectionExtension_SpanSelect
    {
        public static TResult[] Select<TSource, TResult>(this Span<TSource> source, Func<TSource, TResult> selector)
            => Select((ReadOnlySpan<TSource>)source, selector);
        public static TResult[] Select<TSource, TResult>(this ReadOnlySpan<TSource> source, Func<TSource, TResult> selector)
        {
            var res = new TResult[source.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = selector(source[i]);
            return res;
        }
        public static TResult[] Select<TSource, TResult>(this Span<TSource> source, Func<TSource, int, TResult> selector)
            => Select((ReadOnlySpan<TSource>)source, selector);
        public static TResult[] Select<TSource, TResult>(this ReadOnlySpan<TSource> source, Func<TSource, int, TResult> selector)
        {
            var res = new TResult[source.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = selector(source[i], i);
            return res;
        }
    }
}
