using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kzrnm.Competitive.Analyzer.Helpers;

namespace Kzrnm.Competitive.Analyzer.Helpers;

public static class CollectionExtensions
{
    public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> collection, Action<TSource> action)
    {
        foreach (var item in collection)
        {
            action(item);
            yield return item;
        }
    }
    public static ParallelQuery<TSource> AsParallel<TSource>(this IEnumerable<TSource> collection, CancellationToken cancellationToken)
        => collection.AsParallel().WithCancellation(cancellationToken);
}
