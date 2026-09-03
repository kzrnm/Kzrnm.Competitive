namespace Kzrnm.Competitive.Testing.Number;

public class Fraction128Tests
{
    static IEnumerable<Fraction128> RandomFraction128s(Random rnd)
        => Enumerable.Repeat(rnd, 1000).Select(rnd => new Fraction128(rnd.Next(), rnd.Next()));
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
        var f = new Fraction128(分子in, 分母in);
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
        var num = new Fraction128(numerator, denominator);
        await num.ToString().Should().BeEqualTo(text);
        await Fraction128.Parse(text).Should().BeEqualTo(num);
        await Fraction128.Parse($"{numerator}/{denominator}").Should().BeEqualTo(num);
    }
    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task LongImplicitTest()
    {
        foreach (var num in Enumerable.Repeat(new Random(150), 1000).Select(rnd => rnd.Next()))
        {
            Fraction128 f = num;
            await f.Should().BeEqualTo(new Fraction128(num, 1));
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task EqualsTest()
    {
        foreach (var f in RandomFraction128s(new Random(3153)))
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

    public static IEnumerable<(double, double)> CompareTo_Data()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 50; q++)
        {
            var a = Math.Pow(10, rnd.NextDouble() * (q % 2 == 0 ? 1 : 36));
            yield return (a, a);
            for (int r = 0; r < 50; r++)
            {
                var b = Math.Pow(10, rnd.NextDouble() * (q % 3 == 0 ? 1 : 36));
                yield return (a, b);
                yield return (b, a);
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(CompareTo_Data), DeferEnumeration = true)]
    public async Task CompareTo(double a, double b)
    {
        await ((Fraction128)a).CompareTo((Fraction128)b).Should().BeEqualTo(a.CompareTo(b));
    }

    [Test]
    public async Task CompareTo2()
    {
        await (-(Fraction128)9223372036854775808).CompareTo(new Fraction128(-549999999999999998, 3)).Should().BeEqualTo(-1);
        await ((Fraction128)9223372036854775808).CompareTo(new Fraction128(549999999999999998, 3)).Should().BeEqualTo(1);

        await new Fraction128(-549999999999999998, 3).CompareTo(-(Fraction128)9223372036854775808).Should().BeEqualTo(1);
        await new Fraction128(549999999999999998, 3).CompareTo((Fraction128)9223372036854775808).Should().BeEqualTo(-1);
    }

    public static IEnumerable<(Fraction128, Fraction128)> GreaterThan_Data =>
    [
        (new Fraction128(3, 1), new Fraction128(2, 1)),
        (new Fraction128(2, 3), new Fraction128(1, 2)),
        (new Fraction128(5, 6), new Fraction128(4, 9)),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(GreaterThan_Data))]
    [Property("Category", "Normal")]
    public async Task GreaterThanTest(Fraction128 left, Fraction128 right)
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
        foreach (var f in RandomFraction128s(new Random(13)))
        {
            await (-f).Should().BeEqualTo(new Fraction128(-f.Numerator, f.Denominator));
        }
    }

    public static IEnumerable<(Fraction128, Fraction128, Fraction128)> Add_Data =>
    [
        (new Fraction128(3, 1), new Fraction128(2, 1), new Fraction128(5, 1)),
        (new Fraction128(1, 2), new Fraction128(1, 3), new Fraction128(5, 6)),
        (new Fraction128(1, 6), new Fraction128(1, 3), new Fraction128(1, 2)),
        (new Fraction128(-1,6), new Fraction128(1, 3), new Fraction128(1, 6)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Add_Data))]
    public async Task AddTest(Fraction128 num1, Fraction128 num2, Fraction128 expected)
    {
        await (num1 + num2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Fraction128, Fraction128, Fraction128)> Subtract_Data =>
    [
        (new Fraction128(3, 1), new Fraction128(2, 1), new Fraction128(1, 1)),
        (new Fraction128(1, 2), new Fraction128(1, 3), new Fraction128(1, 6)),
        (new Fraction128(1, 6), new Fraction128(1, 3), new Fraction128(-1, 6)),
        (new Fraction128(-1, 6), new Fraction128(1, 3), new Fraction128(-1, 2)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Subtract_Data))]
    public async Task SubtractTest(Fraction128 num1, Fraction128 num2, Fraction128 expected)
    {
        await (num1 - num2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Fraction128, Fraction128, Fraction128)> Multiply_Data =>
    [
        (new Fraction128(3, 1), new Fraction128(5, 1), new Fraction128(15, 1)),
        (new Fraction128(1, 2), new Fraction128(1, 7), new Fraction128(1, 14)),
        (new Fraction128(-1, 6), new Fraction128(2, 3), new Fraction128(-1, 9)),
        (new Fraction128(-1, 16), new Fraction128(-4, 3), new Fraction128(1, 12)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task MultiplyTest(Fraction128 num1, Fraction128 num2, Fraction128 expected)
    {
        await (num1 * num2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Fraction128, Fraction128, Fraction128)> Divide_Data =>
    [
        (new Fraction128(3, 1), new Fraction128(2, 1), new Fraction128(3, 2)),
        (new Fraction128(1, 2), new Fraction128(1, 7), new Fraction128(7, 2)),
        (new Fraction128(-1, 6), new Fraction128(2, 3), new Fraction128(-1, 4)),
        (new Fraction128(-1, 12), new Fraction128(-4, 3), new Fraction128(1, 16)),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Divide_Data))]
    public async Task DivideTest(Fraction128 num1, Fraction128 num2, Fraction128 expected)
    {
        await (num1 / num2).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    public async Task InverseTest()
    {
        foreach (var f in RandomFraction128s(new Random(48463)))
        {
            await f.Inverse().Should().BeEqualTo(new Fraction128(f.Denominator, f.Numerator));
        }
    }

    public static IEnumerable<(Fraction128 num, Fraction128 expected)> RoundOff_Data()
    {
        yield return (new(Int128.Parse("35147736907315327265"), Int128.Parse("618970019642690137449562112")), new(Int128.Parse("1047484186509"), Int128.Parse("18446744073709551616")));
        yield return (new(2, 3), new(2, 3));
    }

    [Test]
    [MethodDataSource(nameof(RoundOff_Data))]
    public async Task RoundOff(Fraction128 num, Fraction128 expected)
    {
        await num.RoundOff().Should().BeEqualTo(expected);
    }

    public static IEnumerable<(double num, Fraction128 expected)> FromDouble_Data()
    {
        yield return (-1.0, new(-1, 1));
        yield return (1.0, new(1, 1));
        yield return (1.41235663e18, new((long)1.41235663e18, 1));
        yield return (234567.432188907435523574, new(8059675599664441, 34359738368));
        yield return (0.0000542342368976897423568974352, new(8003560704676945, Int128.Parse("147573952589676412928")));
        yield return (5.6784231532897532e-12, new(Int128.Parse("3514773690731265"), Int128.Parse("618970019642690137449562112")));
        yield return (1.0002828553543256E+25, (Fraction128)Int128.Parse("10002828553543255926505472"));
        yield return (1.0002828553543256E-25, Fraction128.Parse("2127366360065/21267647932558653966460912964485513216"));
    }

    [Test]
    [MethodDataSource(nameof(FromDouble_Data))]
    public async Task FromDouble(double num, Fraction128 expected)
    {
        await ((Fraction128)num).Should().BeEqualTo(expected);
    }
}