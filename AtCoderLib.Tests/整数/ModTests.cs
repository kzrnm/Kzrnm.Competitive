using AtCoderProject;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace AtCoderLib.整数
{
    public class ModTests
    {
        const int factorSize = 10000;
        readonly Mod.Factors factors;
        public ModTests()
        {
            factors = Mod.CreateFactor(factorSize);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void InvalidTest()
        {
            Mod.invalid.val.Should().Be(-1);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void UnsafeTest()
        {
            var arr = Util.MakeIntArray(1000);
            arr.Any(num => num < 0).Should().BeTrue();
            foreach (var num in arr)
                Mod.Unsafe(num).val.Should().Be(num);
        }

        public static TheoryData Construct_Data = new TheoryData<long, long>
        {
            { 1, 1 },
            { Mod.mod, 0 },
            { Mod.mod+1, 1 },
            { -1, Mod.mod - 1 },
        };
        [Theory]
        [MemberData(nameof(Construct_Data))]
        [Trait("Category", "Normal")]
        public void ConstructTest(long input, long expected)
        {
            new Mod(input).val.Should().Be(expected);
            new Mod(input).Should().Be((Mod)expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void ConsoleReaderImplicitTest()
        {
            var enc = new UTF8Encoding(false);
            using var ms = new MemoryStream(enc.GetBytes($"13 0 {Mod.mod}"));
            var cr = new ConsoleReader(ms, enc);

            Mod m;
            m = cr;
            m.Should().Be((Mod)13);
            m = cr;
            m.Should().Be((Mod)0);
            m = cr;
            m.Should().Be((Mod)0);
        }

        [Theory]
        [MemberData(nameof(Construct_Data))]
        [Trait("Category", "Normal")]
        public void GetHashCodeTest(long input, long expected)
        {
            new Mod(input).GetHashCode().Should().Be(expected.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(Construct_Data))]
        [Trait("Category", "Normal")]
        public void ToStringTest(long input, long expected)
        {
            new Mod(input).ToString().Should().Be(expected.ToString());
        }


        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinusTest()
        {
            var rnd = new Xorshift(42);
            for (int i = 0; i < 1000; i++)
            {
                var num = rnd.Next(0, (int)Mod.mod);
                (-new Mod(num)).Should().Be(new Mod(Mod.mod - num));
            }
        }


        [Theory]
        [Trait("Category", "Operator")]
        [InlineData(1, 1, 2)]
        [InlineData(Mod.mod - 1, 1, 0)]
        [InlineData(2, -1, 1)]
        [InlineData(5, -1, 4)]
        public void PlusTest(long input1, long input2, long expected)
        {
            (new Mod(input1) + input2).Should().Be(new Mod(expected));
        }

        [Theory]
        [Trait("Category", "Operator")]
        [InlineData(1, 1, 0)]
        [InlineData(1, 2, Mod.mod - 1)]
        [InlineData(2, -6, 8)]
        [InlineData(5, -1, 6)]
        public void MinusTest(long input1, long input2, long expected)
        {
            (new Mod(input1) - input2).Should().Be(new Mod(expected));
        }

        [Theory]
        [Trait("Category", "Operator")]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 4)]
        [InlineData(3, 0, 0)]
        [InlineData(0, -2, 0)]
        [InlineData(Mod.mod - 1, 2, Mod.mod - 2)]
        [InlineData(100000, 10001, 99993)]
        public void MultiTest(long input1, long input2, long expected)
        {
            (new Mod(input1) * input2).Should().Be(new Mod(expected));
        }

        [Theory]
        [Trait("Category", "Operator")]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 500000004)]
        [InlineData(8, 4, 2)]
        [InlineData(6, -3, Mod.mod - 2)]
        public void DivTest(long input1, long input2, long expected)
        {
            (new Mod(input1) / input2).Should().Be(new Mod(expected));
            ((new Mod(input1) / input2) * input2).Should().Be(new Mod(input1));
        }

        public static TheoryData Inverse_Data = new TheoryData<long, long>
        {
            { 1, 1 },
            { 2, 500000004 },
            { 3, 333333336 },
            { 1024, 71289063 },
        };
        [Theory]
        [Trait("Category", "Function")]
        [MemberData(nameof(Inverse_Data))]
        public void InverseTest(long input1, long input2)
        {
            new Mod(input1).Inverse().Should().Be(new Mod(input2));
            new Mod(input2).Inverse().Should().Be(new Mod(input1));
        }
        [Theory]
        [Trait("Category", "Function")]
        [MemberData(nameof(Inverse_Data))]
        public void InverseCacheTest(long input1, long input2)
        {
            var method = typeof(Mod).GetMethod("EuclideanInverseCache", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static);
            ((Mod)method.Invoke(null, new object[] { input1 })).Should().Be(new Mod(input2));
            ((Mod)method.Invoke(null, new object[] { input2 })).Should().Be(new Mod(input1));
        }


        [Theory]
        [Trait("Category", "Function")]
        [InlineData(1, 1, 1)]
        [InlineData(1, 1024, 1)]
        [InlineData(2, 10, 1024)]
        [InlineData(2, 1024, 812734592)]
        public void PowTest(long x, int y, long expected)
        {
            Mod.Pow(x, y).Should().Be(new Mod(expected));
            new Mod(x).Pow(y).Should().Be(new Mod(expected));
        }

        public static TheoryData Sum_Data = new TheoryData<long[], long>
        {
            { new long[]{ 1 }, 1 },
            { new long[]{ 1,2,3,4,5 }, 15 },
            { new long[]{ 1000000000, 6 }, 1000000006 },
            { new long[]{ 1000000000, 6, 1 }, 0 },
            { new long[]{ 1000000000, 6, 2 }, 1 },
        };
        [Theory]
        [Trait("Category", "Function")]
        [MemberData(nameof(Sum_Data))]
        public void SumTest(long[] larr, long expected)
        {
            var arr = larr.Select(l => new Mod(l)).ToArray();
            arr.Sum().Should().Be(new Mod(expected));
        }

        [Theory]
        [Trait("Category", "Function")]
        [InlineData(1, 7, 1)]
        [InlineData(2, 7, 4)]
        [InlineData(3, 7, 5)]
        [InlineData(4, 7, 2)]
        [InlineData(5, 7, 3)]
        [InlineData(6, 7, 6)]
        public void EuclidInverseTest(long num, long mod, long expected)
        {
            Mod.EuclideanInverse(num, mod).Should().Be(expected);
            (Mod.EuclideanInverse(num, mod) * num % mod).Should().Be(1);
        }

        [Fact]
        [Trait("Category", "Function")]
        public void CrtTest()
        {
            Mod.Crt(Array.Empty<long>(), Array.Empty<long>()).Should().Be((0, 1));
            Mod.Crt(new long[] { 2, 3, 2 }, new long[] { 3, 5, 7 }).Should().Be((23, 105));
            Mod.Crt(new long[] { 1, 1, 4, 6 }, new long[] { 3, 5, 7, 11 }).Should().Be((886, 1155));
        }

        [Fact]
        [Trait("Category", "Factors")]
        public void FactorialTest()
        {
            long p = 1;
            factors.Factorial(0).Should().Be(new Mod(p));
            for (int i = 1; i <= 12; i++)
            {
                p *= i;
                factors.Factorial(i).Should().Be(new Mod(p));
            }
            factors.Factorial(13).Should().Be(new Mod(227020758));
        }

        [Fact]
        [Trait("Category", "Factors")]
        public void FactorialInversTest()
        {
            for (int i = 0; i <= factorSize; i++)
            {
                factors.FactorialInvers(i).Should().Be(1 / factors.Factorial(i));
            }
        }

        [Fact]
        [Trait("Category", "Factors")]
        public void CTest()
        {
            const int N = 10;
            Mod[][] pascal = new Mod[N + 1][];
            pascal[0] = new Mod[1] { 1 };
            for (var i = 1; i <= N; i++)
            {
                pascal[i] = new Mod[i + 1];
                pascal[i][0] = pascal[i][^1] = 1;
                for (int j = 1; j < pascal[i].Length - 1; j++)
                    pascal[i][j] = pascal[i - 1][j - 1] + pascal[i - 1][j];
            }

            for (int i = 0; i < pascal.Length; i++)
                for (int j = 0; j < pascal[i].Length; j++)
                    factors.Combination(i, j).Should().Be(pascal[i][j]);
        }
    }
}
