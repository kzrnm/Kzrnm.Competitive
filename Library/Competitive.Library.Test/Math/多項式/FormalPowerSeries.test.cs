using AtCoder;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class FormalPowerSeriesTest
    {
        [Fact]
        public void Add()
        {
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 }, new int[] { 1, 7, 3, 2 });
            RunTest<Mod998244353>(new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 }, new int[] { 1, 7, 3, 2 });
            RunTest<Mod998244353>(new int[] { 0, 5, 0, 2 }, new int[0], new int[] { 0, 5, 0, 2 });
            RunTest<Mod998244353>(new int[0], new int[] { 0, 5, 0, 2 }, new int[] { 0, 5, 0, 2 });

            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 }, new int[] { 1, 7, 3, 2 });
            RunTest<Mod1000000007>(new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 }, new int[] { 1, 7, 3, 2 });
            RunTest<Mod1000000007>(new int[] { 0, 5, 0, 2 }, new int[0], new int[] { 0, 5, 0, 2 });
            RunTest<Mod1000000007>(new int[0], new int[] { 0, 5, 0, 2 }, new int[] { 0, 5, 0, 2 });

            static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var rhs = new FormalPowerSeries<T>(rhsArray);
                var expected = new FormalPowerSeries<T>(expectedArray);

                (lhs + rhs).Coefficients.Should().Equal(expected.Coefficients);
                (lhs + rhs.Coefficients).Coefficients.Should().Equal(expected.Coefficients);
            }
        }
        [Fact]
        public void Subtract()
        {
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 }, new int[] { 1, -3, 3, -2 });
            RunTest<Mod998244353>(new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 }, new int[] { -1, 3, -3, 2 });
            RunTest<Mod998244353>(new int[] { 0, 5, 0, 2 }, new int[0], new int[] { 0, 5, 0, 2 });
            RunTest<Mod998244353>(new int[0], new int[] { 0, 5, 0, 2 }, new int[] { 0, -5, 0, -2 });

            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 }, new int[] { 1, -3, 3, -2 });
            RunTest<Mod1000000007>(new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 }, new int[] { -1, 3, -3, 2 });
            RunTest<Mod1000000007>(new int[] { 0, 5, 0, 2 }, new int[0], new int[] { 0, 5, 0, 2 });
            RunTest<Mod1000000007>(new int[0], new int[] { 0, 5, 0, 2 }, new int[] { 0, -5, 0, -2 });

            static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
            {
                var lhs = new FormalPowerSeries<T>(lhsArray.Select(n => new StaticModInt<T>(n)).ToArray());
                var rhs = new FormalPowerSeries<T>(rhsArray.Select(n => new StaticModInt<T>(n)).ToArray());
                var expected = new FormalPowerSeries<T>(expectedArray.Select(n => new StaticModInt<T>(n)).ToArray());

                (lhs - rhs).Coefficients.Should().Equal(expected.Coefficients);
                (lhs - rhs.Coefficients).Coefficients.Should().Equal(expected.Coefficients);
            }
        }
        [Fact]
        public void Minus()
        {
            RunTest<Mod998244353>(new int[] { 0, 1, 2, 3 }, new int[] { 0, -1, -2, -3 });
            RunTest<Mod998244353>(new int[] { 0, -1, -2, -3 }, new int[] { 0, 1, 2, 3 });
            RunTest<Mod998244353>(new int[0], new int[0]);

            RunTest<Mod1000000007>(new int[] { 0, 1, 2, 3 }, new int[] { 0, -1, -2, -3 });
            RunTest<Mod1000000007>(new int[] { 0, -1, -2, -3 }, new int[] { 0, 1, 2, 3 });
            RunTest<Mod1000000007>(new int[0], new int[0]);

            static void RunTest<T>(int[] valueArray, int[] expectedArray) where T : struct, IStaticMod
            {
                var value = new FormalPowerSeries<T>(valueArray.Select(n => new StaticModInt<T>(n)).ToArray());
                var expected = new FormalPowerSeries<T>(expectedArray.Select(n => new StaticModInt<T>(n)).ToArray());

                (-value).Coefficients.Should().Equal(expected.Coefficients);
            }
        }
        [Fact]
        public void Multiply()
        {
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 }, new int[] { 0, 5, 10, 17, 4, 6 });
            RunTest<Mod998244353>(new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 }, new int[] { 0, 5, 10, 17, 4, 6 });
            RunTest<Mod998244353>(new int[] { 0, 5, 0, 2 }, new int[0], new int[0]);
            RunTest<Mod998244353>(new int[0], new int[] { 0, 5, 0, 2 }, new int[0]);

            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 }, new int[] { 0, 5, 10, 17, 4, 6 });
            RunTest<Mod1000000007>(new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 }, new int[] { 0, 5, 10, 17, 4, 6 });
            RunTest<Mod1000000007>(new int[] { 0, 5, 0, 2 }, new int[0], new int[0]);
            RunTest<Mod1000000007>(new int[0], new int[] { 0, 5, 0, 2 }, new int[0]);

            static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var rhs = new FormalPowerSeries<T>(rhsArray);
                var expected = new FormalPowerSeries<T>(expectedArray);

                (lhs * rhs).Coefficients.Should().Equal(expected.Coefficients);
            }
        }

        [Fact]
        public void Divide()
        {
            RunTest<Mod998244353>(new int[] { 0, 5, 10, 17, 4, 6 }, new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 });
            RunTest<Mod998244353>(new int[] { 0, 5, 10, 17, 4, 6 }, new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 });
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 10, 17, 4, 6 }, new int[0]);
            RunTest<Mod998244353>(new int[0], new int[] { 0, 5, 10, 17, 4, 6 }, new int[0]);

            RunTest<Mod1000000007>(new int[] { 0, 5, 10, 17, 4, 6 }, new int[] { 0, 5, 0, 2 }, new int[] { 1, 2, 3 });
            RunTest<Mod1000000007>(new int[] { 0, 5, 10, 17, 4, 6 }, new int[] { 1, 2, 3 }, new int[] { 0, 5, 0, 2 });
            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, new int[] { 0, 5, 10, 17, 4, 6 }, new int[0]);
            RunTest<Mod1000000007>(new int[0], new int[] { 0, 5, 10, 17, 4, 6 }, new int[0]);


            static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] expectedArray) where T : struct, IStaticMod
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var rhs = new FormalPowerSeries<T>(rhsArray);
                var expected = new FormalPowerSeries<T>(expectedArray);

                (lhs / rhs).Coefficients.Should().Equal(expected.Coefficients);
            }
        }

        [Fact]
        public void DivRem()
        {
            var rnd = new Random(227);
            const int N = 160;
            var lhs = new int[N];
            var rhs = new int[N + 1];
            var rem = new int[N];
            foreach (ref var v in lhs.AsSpan()) v = rnd.Next(1, 1000000);
            foreach (ref var v in rhs.AsSpan()) v = rnd.Next(1, 1000000);
            foreach (ref var v in rem.AsSpan()) v = rnd.Next(1, 1000000);

            RunTest<Mod998244353>(lhs, rhs, rem);
            RunTest<Mod1000000007>(lhs, rhs, rem);

            static void RunTest<T>(int[] lhsArray, int[] rhsArray, int[] remArray) where T : struct, IStaticMod
            {
                var lhs = new FormalPowerSeries<T>(lhsArray);
                var rhs = new FormalPowerSeries<T>(rhsArray);
                var rem = new FormalPowerSeries<T>(remArray);

                var p = lhs * rhs + rem;
                var (q, r) = p.DivRem(rhs);

                q.Coefficients.Should().Equal((p / rhs).Coefficients);
                r.Coefficients.Should().Equal((p % rhs).Coefficients);
                (q * rhs + r).Coefficients.Should().Equal(p.Coefficients);
            }
        }

        [Fact]
        public void RightShift()
        {
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, 0, new int[] { 1, 2, 3 });
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, 1, new int[] { 2, 3 });
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, 2, new int[] { 3 });
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, 3, new int[0]);

            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, 0, new int[] { 1, 2, 3 });
            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, 1, new int[] { 2, 3 });
            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, 2, new int[] { 3 });
            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, 3, new int[0]);

            static void RunTest<T>(int[] valueArray, int shift, int[] expectedArray) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(valueArray);
                var expected = new FormalPowerSeries<T>(expectedArray);

                (fps >> shift).Coefficients.Should().Equal(expected.Coefficients);
            }
        }

        [Fact]
        public void LeftShift()
        {
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, 0, new int[] { 1, 2, 3 });
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, 1, new int[] { 0, 1, 2, 3 });
            RunTest<Mod998244353>(new int[] { 1, 2, 3 }, 4, new int[] { 0, 0, 0, 0, 1, 2, 3 });

            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, 0, new int[] { 1, 2, 3 });
            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, 1, new int[] { 0, 1, 2, 3 });
            RunTest<Mod1000000007>(new int[] { 1, 2, 3 }, 4, new int[] { 0, 0, 0, 0, 1, 2, 3 });

            static void RunTest<T>(int[] valueArray, int shift, int[] expectedArray) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(valueArray);
                var expected = new FormalPowerSeries<T>(expectedArray);

                (fps << shift).Coefficients.Should().Equal(expected.Coefficients);
            }
        }

        [Fact]
        public void Derivative()
        {
            RunTest<Mod998244353>(new int[] { 3, 5, 10, 17, 4, 6 }, new int[] { 5, 20, 51, 16, 30 });
            RunTest<Mod998244353>(new int[1] { 3 }, new int[0]);
            RunTest<Mod998244353>(new int[0], new int[0]);

            RunTest<Mod1000000007>(new int[] { 3, 5, 10, 17, 4, 6 }, new int[] { 5, 20, 51, 16, 30 });
            RunTest<Mod1000000007>(new int[1] { 3 }, new int[0]);
            RunTest<Mod1000000007>(new int[0], new int[0]);

            static void RunTest<T>(int[] valueArray, int[] expectedArray) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(valueArray);
                fps.Derivative().Coefficients.Should()
                    .Equal(expectedArray.Select(t => new StaticModInt<T>(t)).ToArray());
            }
        }

        [Fact]
        public void Integrate()
        {
            RunTest<Mod998244353>(new int[] { 5, 20, 51, 16, 30 }, new (int Numerator, int Denominator)[] { (0, 1), (5, 1), (10, 1), (17, 1), (4, 1), (6, 1) });
            RunTest<Mod998244353>(new int[] { 1, 1, 1, 1 }, new (int Numerator, int Denominator)[] { (0, 1), (1, 1), (1, 2), (1, 3), (1, 4) });
            RunTest<Mod998244353>(new int[1] { 3 }, new (int Numerator, int Denominator)[] { (0, 1), (3, 1) });
            RunTest<Mod998244353>(new int[0], new (int Numerator, int Denominator)[0]);

            RunTest<Mod1000000007>(new int[] { 5, 20, 51, 16, 30 }, new (int Numerator, int Denominator)[] { (0, 1), (5, 1), (10, 1), (17, 1), (4, 1), (6, 1) });
            RunTest<Mod1000000007>(new int[] { 1, 1, 1, 1 }, new (int Numerator, int Denominator)[] { (0, 1), (1, 1), (1, 2), (1, 3), (1, 4) });
            RunTest<Mod1000000007>(new int[1] { 3 }, new (int Numerator, int Denominator)[] { (0, 1), (3, 1) });
            RunTest<Mod1000000007>(new int[0], new (int Numerator, int Denominator)[0]);

            static void RunTest<T>(int[] valueArray, (int Numerator, int Denominator)[] expectedArray) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(valueArray);
                fps.Integrate().Coefficients.Should()
                    .Equal(expectedArray.Select(t => new StaticModInt<T>(t.Numerator) / t.Denominator).ToArray());
            }
        }

        [Fact]
        public void Eval()
        {
            RunTest<Mod998244353>(new int[] { 5, 20, 51, 16, 30 }, 7, 80162);
            RunTest<Mod998244353>(new int[] { 5, 20, 51, 16, 30 }, 8, 134501);
            RunTest<Mod998244353>(new int[] { 5, 20, 51, 16, 30 }, 9, 212810);
            RunTest<Mod998244353>(new int[0], 9, 0);

            RunTest<Mod1000000007>(new int[] { 5, 20, 51, 16, 30 }, 7, 80162);
            RunTest<Mod1000000007>(new int[] { 5, 20, 51, 16, 30 }, 8, 134501);
            RunTest<Mod1000000007>(new int[] { 5, 20, 51, 16, 30 }, 9, 212810);
            RunTest<Mod1000000007>(new int[0], 9, 0);

            static void RunTest<T>(int[] fpsArray, StaticModInt<T> x, StaticModInt<T> expected) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(fpsArray);
                fps.Eval(x).Should().Be(expected);
            }
        }

        [Fact]
        public void Inv()
        {
            RunTest<Mod998244353>(
                new int[] { 5, 4, 3, 2, 1 },
                new int[] { 598946612, 718735934, 862483121, 635682004, 163871793 });

            RunTest<Mod1000000007>(
                new int[] { 5, 4, 3, 2, 1 },
                new int[] { 400000003, 880000006, 856000006, 427200003, 712640005 });

            static void RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(fpsArray);
                fps.Inv().Coefficients.Should().Equal(new FormalPowerSeries<T>(expected).Coefficients);
            }
        }

        [Fact]
        public void Exp()
        {
            RunTest<Mod998244353>(new int[] { 0, 1, 2, 3, 4 }, new int[] { 1, 1, 499122179, 166374064, 291154613 });
            RunTest<Mod998244353>(
                new int[] { 0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549 },
                new int[] { 1, 907649120, 316060452, 57037696, 378993419, 302467176, 349948335, 115795520, 647455105, 497971134 });
            RunTest<Mod998244353>(new int[1] { 0 }, new int[1] { 1 });
            RunTest<Mod998244353>(new int[0], new int[1] { 1 });

            RunTest<Mod1000000007>(new int[] { 0, 1, 2, 3, 4 }, new int[] { 1, 1, 500000006, 166666673, 41666677 });
            RunTest<Mod1000000007>(
                new int[] { 0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549 },
                new int[] { 1, 907649120, 925644116, 38331988, 156875359, 697776255, 802320078, 499725651, 949053640, 121509191 });
            RunTest<Mod1000000007>(new int[1] { 0 }, new int[1] { 1 });
            RunTest<Mod1000000007>(new int[0], new int[1] { 1 });

            static void RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(fpsArray);
                fps.Exp().Coefficients.Should().Equal(new FormalPowerSeries<T>(expected).Coefficients);
            }
        }

        [Fact]
        public void Log()
        {
            RunTest<Mod998244353>(new int[] { 1, 1, 499122179, 166374064, 291154613 }, new int[] { 0, 1, 2, 3, 4 });
            RunTest<Mod998244353>(
                new int[] { 1, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549 },
                new int[] { 0, 907649120, 265241806, 491547518, 331811826, 54791043, 895176577, 142597055, 60021098, 768274455 });
            RunTest<Mod998244353>(Enumerable.Repeat(0, 50000).Prepend(1).ToArray(), new int[] { 0 });

            RunTest<Mod1000000007>(new int[] { 1, 1, 500000006, 166666673, 41666677 }, new int[] { 0, 1, 2, 3, 4 });
            RunTest<Mod1000000007>(
                new int[] { 1, 907649120, 925644116, 38331988, 156875359, 697776255, 802320078, 499725651, 949053640, 121509191 },
                new int[] { 0, 907649120, 290651129, 813718295, 770591820, 913049957, 587190944, 411145555, 899491439, 722412549 });

            static void RunTest<T>(int[] fpsArray, int[] expected) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(fpsArray);
                fps.Log().Coefficients.Should().Equal(new FormalPowerSeries<T>(expected).Coefficients);
            }
        }

        [Fact]
        public void Pow()
        {
            RunTest<Mod998244353>(
                new int[] { 2, 3, 4, 5, 6 }, 2,
                new int[] { 4, 12, 25, 44, 70 });
            RunTest<Mod998244353>(
                new int[] { 2, 3, 4, 5, 6 }, 3,
                new int[] { 8, 36, 102, 231, 456 });
            RunTest<Mod998244353>(
                new int[] { 0, 0, 2, 3, 4, 5, 6 }, 2,
                new int[] { 0, 0, 0, 0, 4, 12, 25 });
            RunTest<Mod998244353>(new int[0], 2, new int[0]);

            RunTest<Mod1000000007>(
                new int[] { 2, 3, 4, 5, 6 }, 2,
                new int[] { 4, 12, 25, 44, 70 });
            RunTest<Mod1000000007>(
                new int[] { 2, 3, 4, 5, 6 }, 3,
                new int[] { 8, 36, 102, 231, 456 });
            RunTest<Mod1000000007>(
                new int[] { 0, 0, 2, 3, 4, 5, 6 }, 2,
                new int[] { 0, 0, 0, 0, 4, 12, 25 });
            RunTest<Mod1000000007>(new int[0], 2, new int[0]);

            new FormalPowerSeries<Mod998244353>(new int[] { 2, 3, 4, 5, 6 })
                .Pow(3, 13).Coefficients.Should().Equal(8, 36, 102, 231, 456, 735, 1024, 1257, 1344, 1169, 882, 540, 216);
            new FormalPowerSeries<Mod998244353>(new int[] { 0, 0, 2, 3, 4, 5, 6 })
                .Pow(2, 13).Coefficients.Should().Equal(0, 0, 0, 0, 4, 12, 25, 44, 70, 76, 73, 60, 36);

            new FormalPowerSeries<Mod1000000007>(new int[] { 2, 3, 4, 5, 6 })
                .Pow(3, 13).Coefficients.Should().Equal(8, 36, 102, 231, 456, 735, 1024, 1257, 1344, 1169, 882, 540, 216);
            new FormalPowerSeries<Mod1000000007>(new int[] { 0, 0, 2, 3, 4, 5, 6 })
                .Pow(2, 13).Coefficients.Should().Equal(0, 0, 0, 0, 4, 12, 25, 44, 70, 76, 73, 60, 36);

            static void RunTest<T>(int[] fpsArray, int n, int[] expected) where T : struct, IStaticMod
            {
                var fps = new FormalPowerSeries<T>(fpsArray);
                fps.Pow(n).Coefficients.Should().Equal(new FormalPowerSeries<T>(expected).Coefficients);
            }
        }
    }
}
