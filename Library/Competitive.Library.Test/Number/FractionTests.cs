namespace Kzrnm.Competitive.Testing.Number;

public class FractionTests
{
    static IEnumerable<Fraction> RandomFractions(Random rnd)
        => Enumerable.Repeat(rnd, 1000).Select(rnd => new Fraction(rnd.Next(), rnd.Next()));
    public static IEnumerable<(long, long, long, long)> Construct_Data =>
    [
        (16, 4, 4, 1),
        (2, 845106, 1, 422553),
        (230895518700, 230811434700, 9995477, 9991837),
        (1, 2, 1, 2),
        (-1,  2, -1, 2),
        ( 1, -2, -1, 2),
        (-1, -2,  1, 2),
        ( 2,  2,  1, 1),
        (-2,  2, -1, 1),
        ( 2, -2, -1, 1),
        (-2, -2,  1, 1),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Construct_Data))]
    [Property("Category", "Normal")]
    public async Task ConstructTest(long 分子in, long 分母in, long 分子out, long 分母out)
    {
        var f = new Fraction(分子in, 分母in);
        await f.Numerator.Should().BeEqualTo(分子out);
        await f.Denominator.Should().BeEqualTo(分母out);
    }

    public static IEnumerable<(long, long, string)> ToString_Data =>
    [
        (16, 4, "4/1"),
        (2, 845106, "1/422553"),
        (230895518700, 230811434700, "9995477/9991837"),
        (1, 2, "1/2"),
        (-1, 2, "-1/2"),
        (1, -2, "-1/2"),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(ToString_Data))]
    [Property("Category", "Normal")]
    public async Task ParseAndToStringTest(long numerator, long denominator, string text)
    {
        var num = new Fraction(numerator, denominator);
        await num.ToString().Should().BeEqualTo(text);
        await Fraction.Parse(text).Should().BeEqualTo(num);
        await Fraction.Parse($"{numerator}/{denominator}").Should().BeEqualTo(num);
    }
    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task LongImplicitTest()
    {
        foreach (var num in Enumerable.Repeat(new Random(150), 1000).Select(rnd => rnd.Next()))
        {
            Fraction f = num;
            await f.Should().BeEqualTo(new Fraction(num, 1));
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task EqualsTest()
    {
        foreach (var f in RandomFractions(new Random(3153)))
        {
            var f2 = f;
            await (f == f2).Should().BeTrue();
            await (f != f2).Should().BeFalse();
            await (f >= f2).Should().BeTrue();
            await (f <= f2).Should().BeTrue();
            await (f > f2).Should().BeFalse();
            await (f < f2).Should().BeFalse();
            await f.Equals(f2).Should().BeTrue();
            await f.Equals((object)f2).Should().BeTrue();
            await f.CompareTo(f2).Should().BeEqualTo(0);
        }
    }

    public static IEnumerable<(Fraction, Fraction)> GreaterThan_Data =>
    [
        (new Fraction(3, 1), new Fraction(2, 1)),
        (new Fraction(2, 3), new Fraction(1, 2)),
        (new Fraction(5, 6), new Fraction(4, 9)),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(GreaterThan_Data))]
    [Property("Category", "Normal")]
    public async Task GreaterThanTest(Fraction left, Fraction right)
    {
        await (left == right).Should().BeFalse();
        await (left != right).Should().BeTrue();
        await (left >= right).Should().BeTrue();
        await (left <= right).Should().BeFalse();
        await (left > right).Should().BeTrue();
        await (left < right).Should().BeFalse();
        await left.Equals(right).Should().BeFalse();
        await left.Equals((object)right).Should().BeFalse();
        await left.CompareTo(right).Should().BeGreaterThan(0);
        await right.CompareTo(left).Should().BeLessThan(0);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    public async Task SingleMinusTest()
    {
        foreach (var f in RandomFractions(new Random(13)))
        {
            await (-f).Should().BeEqualTo(new Fraction(-f.Numerator, f.Denominator));
        }
    }

    public static IEnumerable<(Fraction, Fraction, Fraction)> Add_Data =>
    [
        (new Fraction(3, 1), new Fraction(2, 1), new Fraction(5, 1)),
        (new Fraction(1, 2), new Fraction(1, 3), new Fraction(5, 6)),
        (new Fraction(1, 6), new Fraction(1, 3), new Fraction(1, 2)),
        (new Fraction(-1,6), new Fraction(1, 3), new Fraction(1, 6)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Add_Data))]
    public async Task AddTest(Fraction num1, Fraction num2, Fraction expected)
    {
        await (num1 + num2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Fraction, Fraction, Fraction)> Subtract_Data =>
    [
        (new Fraction(3, 1), new Fraction(2, 1), new Fraction(1, 1)),
        (new Fraction(1, 2), new Fraction(1, 3), new Fraction(1, 6)),
        (new Fraction(1, 6), new Fraction(1, 3), new Fraction(-1, 6)),
        (new Fraction(-1, 6), new Fraction(1, 3), new Fraction(-1, 2)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Subtract_Data))]
    public async Task SubtractTest(Fraction num1, Fraction num2, Fraction expected)
    {
        await (num1 - num2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Fraction, Fraction, Fraction)> Multiply_Data =>
    [
        (new Fraction(3, 1), new Fraction(5, 1), new Fraction(15, 1)),
        (new Fraction(1, 2), new Fraction(1, 7), new Fraction(1, 14)),
        (new Fraction(-1, 6), new Fraction(2, 3), new Fraction(-1, 9)),
        (new Fraction(-1, 16), new Fraction(-4, 3), new Fraction(1, 12)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task MultiplyTest(Fraction num1, Fraction num2, Fraction expected)
    {
        await (num1 * num2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Fraction, Fraction, Fraction)> Divide_Data =>
    [
        (new Fraction(3, 1), new Fraction(2, 1), new Fraction(3, 2)),
        (new Fraction(1, 2), new Fraction(1, 7), new Fraction(7, 2)),
        (new Fraction(-1, 6), new Fraction(2, 3), new Fraction(-1, 4)),
        (new Fraction(-1, 12), new Fraction(-4, 3), new Fraction(1, 16)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Divide_Data))]
    public async Task DivideTest(Fraction num1, Fraction num2, Fraction expected)
    {
        await (num1 / num2).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    public async Task InverseTest()
    {
        foreach (var f in RandomFractions(new Random(48463)))
        {
            await f.Inverse().Should().BeEqualTo(new Fraction(f.Denominator, f.Numerator));
        }
    }

}