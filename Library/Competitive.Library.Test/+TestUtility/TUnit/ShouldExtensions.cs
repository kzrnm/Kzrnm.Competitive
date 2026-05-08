using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TUnit.Assertions.Conditions;
using TUnit.Assertions.Core;
using TUnit.Assertions.Enums;
using TUnit.Assertions.Should.Core;

namespace Kzrnm.Competitive.Testing;
#nullable enable

public static class ShouldExtensions
{
    [SuppressMessage("Style", "IDE0059:値の不必要な代入", Justification = "だるい")]
    public static ShouldAssertion<TCollection> BeStrictlyEquivalentTo<TCollection, T>(
        this IShouldSource<TCollection> source,
        IEnumerable<T> expected,
        [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
        where TCollection : IEnumerable<T>
    {
        var innerContext = source.Context;
        innerContext.ExpressionBuilder.Append(".BeStrictlyEquivalentTo(");
        var __added = false;
        if (expectedExpression is not null)
        {
            if (__added) innerContext.ExpressionBuilder.Append(", ");
            innerContext.ExpressionBuilder.Append(expectedExpression);
            __added = true;
        }
        innerContext.ExpressionBuilder.Append(')');
        var inner = new IsEquivalentToAssertion<TCollection, T>(innerContext, expected, EqualityComparer<T>.Default, CollectionOrdering.Matching);
        var __tunit_should_because = source.ConsumeBecauseMessage();
        if (__tunit_should_because is not null)
        {
            inner.Because(__tunit_should_because);
        }
        return new ShouldAssertion<TCollection>(innerContext, inner);
    }

    [SuppressMessage("Style", "IDE0059:値の不必要な代入", Justification = "だるい")]
    public static ShouldAssertion<TCollection> BeStrictlyEquivalentTo<TCollection, T>(
        this IShouldSource<TCollection> source,
        IEnumerable<T> expected,
        IEqualityComparer<T> comparer,
        [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null,
        [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
        where TCollection : IEnumerable<T>
    {
        var innerContext = source.Context;
        innerContext.ExpressionBuilder.Append(".BeStrictlyEquivalentTo(");
        var __added = false;
        if (expectedExpression is not null)
        {
            if (__added) innerContext.ExpressionBuilder.Append(", ");
            innerContext.ExpressionBuilder.Append(expectedExpression);
            __added = true;
        }
        if (comparerExpression is not null)
        {
            if (__added) innerContext.ExpressionBuilder.Append(", ");
            innerContext.ExpressionBuilder.Append(comparerExpression);
            __added = true;
        }
        innerContext.ExpressionBuilder.Append(')');
        var inner = new IsEquivalentToAssertion<TCollection, T>(innerContext, expected, comparer, CollectionOrdering.Matching);
        var __tunit_should_because = source.ConsumeBecauseMessage();
        if (__tunit_should_because is not null)
        {
            inner.Because(__tunit_should_because);
        }
        return new ShouldAssertion<TCollection>(innerContext, inner);
    }


    extension(Assert)
    {
        public static MultipleAsyncScope MultipleAsync()
            => new MultipleAsyncScope();
    }

    public sealed class MultipleAsyncScope : IAsyncDisposable
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