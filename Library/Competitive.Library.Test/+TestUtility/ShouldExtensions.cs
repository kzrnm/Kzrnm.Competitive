using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TUnit.Assertions.Core;
using TUnit.Assertions.Enums;
using TUnit.Assertions.Should.Core;

namespace Kzrnm.Competitive.Testing;
#nullable enable

public static class ShouldExtensions
{
    [RequiresUnreferencedCode("Collection equivalency uses structural comparison for complex objects, which requires reflection and is not compatible with AOT")]
    public static ShouldAssertion<TCollection> BeEquivalentOrderTo<TCollection, TItem>(
        this IShouldSource<TCollection> source,
        IEnumerable<TItem> expected,
        [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
        where TCollection : IEnumerable<TItem>
        => source.BeEquivalentTo(expected, CollectionOrdering.Matching, expectedExpression);

    [RequiresUnreferencedCode("Collection equivalency uses structural comparison for complex objects, which requires reflection and is not compatible with AOT")]
    public static ShouldAssertion<TCollection> BeEquivalentOrderTo<TCollection, TItem>(
        this IShouldSource<TCollection> source,
        IEnumerable<TItem> expected,
        IEqualityComparer<TItem> comparer,
        [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
        where TCollection : IEnumerable<TItem>
        => source.BeEquivalentTo(expected, comparer, CollectionOrdering.Matching, expectedExpression);



    extension(Assert)
    {
        public static MultipleAsyncScope MultipleAsync()
            => new MultipleAsyncScope();
    }

    public class MultipleAsyncScope : IAsyncDisposable
    {
        [SuppressMessage("Usage", "TUnitAssertions0004")]
        IDisposable AssertionScope = Assert.Multiple();
        List<Task> list = new();
        public void Add(IAssertion assertion)
        {
            list.Add(assertion.AssertAsync());
        }
        public void Add<T>(ShouldAssertion<T> should)
        {
            var assertion = (IAssertion)typeof(ShouldAssertion<T>).GetField("_inner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .GetValue(should)!;
            list.Add(assertion.AssertAsync());
        }
        public async ValueTask DisposeAsync()
        {
            await Task.WhenAll(list);
            AssertionScope.Dispose();
        }
    }
}