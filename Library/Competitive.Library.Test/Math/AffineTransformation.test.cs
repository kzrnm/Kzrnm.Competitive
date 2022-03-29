using AtCoder;
using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: SAMEAS Library/run.test.py
    public class AffineTransformationTests
    {
        public static TheoryData<
            AffineTransformation<double, DoubleOperator>,
            AffineTransformation<double, DoubleOperator>,
            AffineTransformation<double, DoubleOperator>> Multiply_Data = new()
            {
                {
                    new AffineTransformation<double, DoubleOperator>(2.0, 3.0),
                    new AffineTransformation<double, DoubleOperator>(-5, 7.0),
                    new AffineTransformation<double, DoubleOperator>(-10, -8)
                },
                {
                    new AffineTransformation<double, DoubleOperator>(0, 3.0),
                    new AffineTransformation<double, DoubleOperator>(-3, 7.0),
                    new AffineTransformation<double, DoubleOperator>(0, -2)
                },
            };

        [Theory]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(
            AffineTransformation<double, DoubleOperator> a,
            AffineTransformation<double, DoubleOperator> b,
            AffineTransformation<double, DoubleOperator> expected)
        {
            var op = new AffineTransformation<double, DoubleOperator>.Operator();
            (a * b).Should().Be(expected);
            op.Multiply(a, b).Should().Be(expected);
            op.Multiply(op.MultiplyIdentity, a).Should().Be(a);
            op.Multiply(op.MultiplyIdentity, b).Should().Be(b);
            op.Multiply(a, op.MultiplyIdentity).Should().Be(a);
            op.Multiply(b, op.MultiplyIdentity).Should().Be(b);
        }

        public static TheoryData<AffineTransformation<double, DoubleOperator>, double, double> Apply_Data = new()
        {
            { new AffineTransformation<double, DoubleOperator>(2.0, 3.0), 1, 5 },
            { new AffineTransformation<double, DoubleOperator>(-5, 7.0), 1, 2 },
            { new AffineTransformation<double, DoubleOperator>(-10, -8), 1, -18 },
            { new AffineTransformation<double, DoubleOperator>(0, 3.0), 1, 3 },
            { new AffineTransformation<double, DoubleOperator>(-3, 7.0), 1, 4 },
            { new AffineTransformation<double, DoubleOperator>(0, -2), 1, -2 },

            { new AffineTransformation<double, DoubleOperator>(2.0, 3.0), -1.5, 0 },
            { new AffineTransformation<double, DoubleOperator>(-5, 7.0), -1.5, 14.5 },
            { new AffineTransformation<double, DoubleOperator>(-10, -8), -1.5, 7 },
            { new AffineTransformation<double, DoubleOperator>(0, 3.0), -1.5, 3 },
            { new AffineTransformation<double, DoubleOperator>(-3, 7.0), -1.5, 11.5 },
            { new AffineTransformation<double, DoubleOperator>(0, -2), -1.5, -2 },
        };
        [Theory]
        [MemberData(nameof(Apply_Data))]
        public void Apply(
            AffineTransformation<double, DoubleOperator> a, double x, double expected)
        {
            a.Apply(x).Should().Be(expected);
        }
    }
}
