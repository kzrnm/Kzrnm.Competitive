using System.Numerics;
using TUnit.Assertions.Enums;

namespace Kzrnm.Competitive.Testing.TwoDimensional;

internal static class SlopeTrickRange
{
    public static T[] RangeValue<T>(this SlopeTrick<T> st, int from, int toExclusive)
        where T : INumber<T>, IMinMaxValue<T>
        => Enumerable.Range(from, toExclusive - from).Select(i => st.Eval(T.CreateChecked(i))).ToArray();
}
public class SlopeTrickTests
{
    [Test]
    public async Task Simple()
    {
        var st = new SlopeTrick();
        using (Assert.Multiple())
        {
            await st.Eval(0).Should().BeEqualTo(0);
            await st.Min.Should().BeEqualTo(0);
            await st.MinRange().Should().BeEqualTo((-long.MaxValue / 2, long.MaxValue / 2));
        }

        st.AddAll(2);
        using (Assert.Multiple())
        {
            await st.Eval(long.MinValue).Should().BeEqualTo(2);
            await st.Eval(long.MaxValue).Should().BeEqualTo(2);
            await st.Min.Should().BeEqualTo(2);
            await st.MinRange().Should().BeEqualTo((-long.MaxValue / 2, long.MaxValue / 2));
            await st.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([2L, 2, 2, 2, 2, 2, 2, 2]);
        }

        st.AddAbs(2);
        using (Assert.Multiple())
        {
            await st.Min.Should().BeEqualTo(2);
            await st.MinRange().Should().BeEqualTo((2, 2));
            await st.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([6L, 5, 4, 3, 2, 3, 4, 5]);
        }

        st.AddRightUpper(4);
        using (Assert.Multiple())
        {
            await st.Min.Should().BeEqualTo(2);
            await st.MinRange().Should().BeEqualTo((2, 2));
            await st.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([6L, 5, 4, 3, 2, 3, 4, 6]);
        }

        st.AddLeftUpper(-1);
        using (Assert.Multiple())
        {
            await st.Min.Should().BeEqualTo(2);
            await st.MinRange().Should().BeEqualTo((2, 2));
            await st.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([7L, 5, 4, 3, 2, 3, 4, 6]);
        }

        st.AddLeftUpper(-1);
        using (Assert.Multiple())
        {
            await st.Min.Should().BeEqualTo(2);
            await st.MinRange().Should().BeEqualTo((2, 2));
            await st.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([8L, 5, 4, 3, 2, 3, 4, 6]);
        }

        st.AddRightUpper(-1);
        using (Assert.Multiple())
        {
            await st.Min.Should().BeEqualTo(5);
            await st.MinRange().Should().BeEqualTo((-1, 2));
            await st.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([8L, 5, 5, 5, 5, 7, 9, 12]);
        }

        st.Shift(1);
        using (Assert.Multiple())
        {
            await st.Min.Should().BeEqualTo(5);
            await st.MinRange().Should().BeEqualTo((0, 3));
            await st.RangeValue(-2, 7).Should().BeStrictlyEquivalentTo([11L, 8, 5, 5, 5, 5, 7, 9, 12]);
        }

        st.Shift(0, -2);
        using (Assert.Multiple())
        {
            await st.Min.Should().BeEqualTo(5);
            await st.MinRange().Should().BeEqualTo((0, 1));
            await st.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([11L, 8, 5, 5, 7, 9, 12, 15]);
        }

        using (Assert.Multiple())
        {
            var st2 = new SlopeTrick();
            st2.Merge(st);
            await st2.Min.Should().BeEqualTo(5);
            await st2.MinRange().Should().BeEqualTo((0, 1));
            await st2.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([11L, 8, 5, 5, 7, 9, 12, 15]);
            st2.ClearLeft();
            await st2.Min.Should().BeEqualTo(5);
            await st2.MinRange().Should().BeEqualTo((-long.MaxValue / 2, 1));
            await st2.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([5L, 5, 5, 5, 7, 9, 12, 15]);
        }

        using (Assert.Multiple())
        {
            var st2 = new SlopeTrick();
            st2.Merge(st);
            await st2.Min.Should().BeEqualTo(5);
            await st2.MinRange().Should().BeEqualTo((0, 1));
            await st2.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([11L, 8, 5, 5, 7, 9, 12, 15]);
            st2.ClearRight();
            await st2.Min.Should().BeEqualTo(5);
            await st2.MinRange().Should().BeEqualTo((0, long.MaxValue / 2));
            await st2.RangeValue(-2, 6).Should().BeStrictlyEquivalentTo([11L, 8, 5, 5, 5, 5, 5, 5]);
        }
    }
}