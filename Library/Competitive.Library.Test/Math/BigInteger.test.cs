using FluentAssertions;
using System.Numerics;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class BigIntegerTests
    {
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
            BigIntegerEx.ParseBigInteger(input).Should().Be(BigInteger.Parse(input));
        }
    }
}
