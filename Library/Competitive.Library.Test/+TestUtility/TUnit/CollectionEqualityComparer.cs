using System.Diagnostics.CodeAnalysis;

namespace Kzrnm.Competitive.Testing;

public class CollectionEqualityComparer<T> :
    IEqualityComparer<IEnumerable<T>>
    , IEqualityComparer<IEnumerable<IEnumerable<T>>>
    , IEqualityComparer<IEnumerable<IEnumerable<IEnumerable<T>>>>
{
    public static CollectionEqualityComparer<T> Default => new();
    public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        => x.SequenceEqual(y, EqualityComparer<T>.Default);

    public bool Equals(IEnumerable<IEnumerable<T>> x, IEnumerable<IEnumerable<T>> y)
        => x.SequenceEqual(y, this);

    public bool Equals(IEnumerable<IEnumerable<IEnumerable<T>>> x, IEnumerable<IEnumerable<IEnumerable<T>>> y)
        => x.SequenceEqual(y, this);

    public int GetHashCode([DisallowNull] IEnumerable<T> obj)
    {
        var hc = new HashCode();
        foreach (var item in obj)
            hc.Add(EqualityComparer<T>.Default.GetHashCode(item));
        return hc.ToHashCode();
    }

    public int GetHashCode([DisallowNull] IEnumerable<IEnumerable<T>> obj)
    {
        var hc = new HashCode();
        foreach (var item in obj)
            hc.Add(GetHashCode(item));
        return hc.ToHashCode();
    }

    public int GetHashCode([DisallowNull] IEnumerable<IEnumerable<IEnumerable<T>>> obj)
    {
        var hc = new HashCode();
        foreach (var item in obj)
            hc.Add(GetHashCode(item));
        return hc.ToHashCode();
    }
}