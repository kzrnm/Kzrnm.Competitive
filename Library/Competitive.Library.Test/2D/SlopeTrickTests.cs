using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.TwoDimensional;

internal static class SlopeTrickRange
{
    public static T[] RangeValue<T>(this SlopeTrick<T> st, int from, int toExclusive)
        where T : INumber<T>, IMinMaxValue<T>
        => Enumerable.Range(from, toExclusive - from).Select(i => st.Eval(T.CreateChecked(i))).ToArray();
}
public class SlopeTrickTests
{
    [Fact]
    public void Simple()
    {
        var st = new SlopeTrick();

        st.Eval(0).ShouldBe(0);
        st.Min.ShouldBe(0);
        st.MinRange().ShouldBe((-long.MaxValue / 2, long.MaxValue / 2));

        st.AddAll(2);
        st.Eval(long.MinValue).ShouldBe(2);
        st.Eval(long.MaxValue).ShouldBe(2);
        st.Min.ShouldBe(2);
        st.MinRange().ShouldBe((-long.MaxValue / 2, long.MaxValue / 2));
        st.RangeValue(-2, 6).ShouldBe([2, 2, 2, 2, 2, 2, 2, 2]);

        st.AddAbs(2);
        st.Min.ShouldBe(2);
        st.MinRange().ShouldBe((2, 2));
        st.RangeValue(-2, 6).ShouldBe([6, 5, 4, 3, 2, 3, 4, 5]);

        st.AddRightUpper(4);
        st.Min.ShouldBe(2);
        st.MinRange().ShouldBe((2, 2));
        st.RangeValue(-2, 6).ShouldBe([6, 5, 4, 3, 2, 3, 4, 6]);

        st.AddLeftUpper(-1);
        st.Min.ShouldBe(2);
        st.MinRange().ShouldBe((2, 2));
        st.RangeValue(-2, 6).ShouldBe([7, 5, 4, 3, 2, 3, 4, 6]);

        st.AddLeftUpper(-1);
        st.Min.ShouldBe(2);
        st.MinRange().ShouldBe((2, 2));
        st.RangeValue(-2, 6).ShouldBe([8, 5, 4, 3, 2, 3, 4, 6]);

        st.AddRightUpper(-1);
        st.Min.ShouldBe(5);
        st.MinRange().ShouldBe((-1, 2));
        st.RangeValue(-2, 6).ShouldBe([8, 5, 5, 5, 5, 7, 9, 12]);

        st.Shift(1);
        st.Min.ShouldBe(5);
        st.MinRange().ShouldBe((0, 3));
        st.RangeValue(-2, 7).ShouldBe([11, 8, 5, 5, 5, 5, 7, 9, 12]);

        st.Shift(0, -2);
        st.Min.ShouldBe(5);
        st.MinRange().ShouldBe((0, 1));
        st.RangeValue(-2, 6).ShouldBe([11, 8, 5, 5, 7, 9, 12, 15]);

        {
            var st2 = new SlopeTrick();
            st2.Merge(st);
            st2.Min.ShouldBe(5);
            st2.MinRange().ShouldBe((0, 1));
            st2.RangeValue(-2, 6).ShouldBe([11, 8, 5, 5, 7, 9, 12, 15]);
            st2.ClearLeft();
            st2.Min.ShouldBe(5);
            st2.MinRange().ShouldBe((-long.MaxValue / 2, 1));
            st2.RangeValue(-2, 6).ShouldBe([5, 5, 5, 5, 7, 9, 12, 15]);
        }
        {
            var st2 = new SlopeTrick();
            st2.Merge(st);
            st2.Min.ShouldBe(5);
            st2.MinRange().ShouldBe((0, 1));
            st2.RangeValue(-2, 6).ShouldBe([11, 8, 5, 5, 7, 9, 12, 15]);
            st2.ClearRight();
            st2.Min.ShouldBe(5);
            st2.MinRange().ShouldBe((0, long.MaxValue / 2));
            st2.RangeValue(-2, 6).ShouldBe([11, 8, 5, 5, 5, 5, 5, 5]);
        }
    }
}