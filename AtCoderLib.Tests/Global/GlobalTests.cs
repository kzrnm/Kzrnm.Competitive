using AtCoderProject;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Xunit;

namespace AtCoderLib.Global
{
    using Global = AtCoderProject.Global;
    public class GlobalTests
    {
        public class NewArrayTests
        {
            [Fact]
            public void NewArray1()
            {
                var arr = Global.NewArray(2, 1);
                arr.Should().HaveCount(2);
                arr.Should().Equal(Enumerable.Repeat(1, 2));
            }
            [Fact]
            public void NewArrayFunc1()
            {
                var arr = Global.NewArray(2, () => new object());
                arr.Should().HaveCount(2);
                arr.Distinct().Should().HaveCount(2);
            }
            [Fact]
            public void NewArray2()
            {
                var arr = Global.NewArray(2, 3, 1);
                arr.SelectMany(a => a).Should().HaveCount(6);
                arr.SelectMany(a => a).Should().Equal(Enumerable.Repeat(1, 6));
            }
            [Fact]
            public void NewArrayFunc2()
            {
                var arr = Global.NewArray(2, 3, () => new object());
                arr.SelectMany(a => a).Should().HaveCount(6);
                arr.SelectMany(a => a).Distinct().Should().HaveCount(6);
            }
            [Fact]
            public void NewArray3()
            {
                var arr = Global.NewArray(2, 3, 5, 1);
                arr.SelectMany(a => a).SelectMany(a => a).Should().HaveCount(30);
                arr.SelectMany(a => a).SelectMany(a => a).Should().Equal(Enumerable.Repeat(1, 30));
            }
            [Fact]
            public void NewArrayFunc3()
            {
                var arr = Global.NewArray(2, 3, 5, () => new object());
                arr.SelectMany(a => a).SelectMany(a => a).Should().HaveCount(30);
                arr.SelectMany(a => a).SelectMany(a => a).Distinct().Should().HaveCount(30);
            }
            [Fact]
            public void NewArray4()
            {
                var arr = Global.NewArray(2, 3, 5, 7, 1);
                arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Should().HaveCount(210);
                arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Should().Equal(Enumerable.Repeat(1, 210));
            }
            [Fact]
            public void NewArrayFunc4()
            {
                var arr = Global.NewArray(2, 3, 5, 7, () => new object());
                arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Should().HaveCount(210);
                arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Distinct().Should().HaveCount(210);
            }
        }

        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1, 100, 1)]
        [InlineData(2, 0, 1)]
        [InlineData(2, 10, 1024)]
        [InlineData(2, 62, 4611686018427387904L)]
        [InlineData(4, 5, 1024)]
        [InlineData(17, 4, 83521)]
        [InlineData(17, 0, 1)]
        public void Pow(long x, int y, long expected)
        {
            Global.Pow(x, y).Should().Be(expected);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-0")]
        [InlineData("1234567890")]
        [InlineData("-1234567890")]
        [InlineData("-321903178318273190741289045432543262643262363268947289078189571895414896419761976417696741971241434468712463124873424184343720834082342428104301240873442148217850245031275176770641764013760671076131046076403176403176071643164316743854343425407")]
        [InlineData("43248098909218931428148434365435432523436436367208340823424281043012408734421482107")]
        [InlineData("321903178318273190741289041628089476545235432414344687124631248734241843437208340823424281043012408734421482107")]
        public void ParseBigInteger(string input)
        {
            Global.ParseBigInteger(input).Should().Be(BigInteger.Parse(input));
        }


        [Theory]
        [InlineData(int.MaxValue, 31, 30, 0)]
        [InlineData(int.MinValue, 1, 31, 31)]
        [InlineData(-1, 32, 31, 0)]
        [InlineData(1, 1, 0, 0)]
        [InlineData(0, 0, 0, 32)]
        public void BitOperationInt32(int input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }

        [Theory]
        [InlineData(uint.MaxValue, 32, 31, 0)]
        [InlineData(0, 0, 0, 32)]
        [InlineData(1, 1, 0, 0)]
        public void BitOperationUInt32(uint input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }
        [Theory]
        [InlineData(long.MaxValue, 63, 62, 0)]
        [InlineData(long.MinValue, 1, 63, 63)]
        [InlineData(-1, 64, 63, 0)]
        [InlineData(1, 1, 0, 0)]
        [InlineData(0, 0, 0, 64)]
        public void BitOperationInt64(long input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }

        [Theory]
        [InlineData(ulong.MaxValue, 64, 63, 0)]
        [InlineData(0, 0, 0, 64)]
        [InlineData(1, 1, 0, 0)]
        public void BitOperationUInt64(ulong input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }

        [Fact]
        public void ΔDebugView()
        {
            var arr = Util.MakeLongArray(1000);
            new ΔDebugView<long>(arr).Items.Should().Equal(arr);
        }
    }
}
