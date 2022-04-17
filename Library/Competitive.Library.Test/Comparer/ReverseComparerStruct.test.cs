using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.Comparer
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class ReverseComparerStructTests
    {
        [Fact]
        public void Compare()
        {
            var arr = new int[]
            {
                1022730976,
                1266617064,
                1850902760,
                2014529243,
                1506739568,
                49770599,
                289393729,
                1569600475,
                1983045250,
                1436977866,
                1102883983,
                910065328,
                1507175202,
                1263693310,
                1790270117,
                216600710,
                705468652,
                98686814,
                1580055797,
                382174879
            };
            Array.Sort(arr, new ReverseComparerStruct<int>());
            arr.Should().Equal(
                2014529243,
                1983045250,
                1850902760,
                1790270117,
                1580055797,
                1569600475,
                1507175202,
                1506739568,
                1436977866,
                1266617064,
                1263693310,
                1102883983,
                1022730976,
                910065328,
                705468652,
                382174879,
                289393729,
                216600710,
                98686814,
                49770599);
        }
    }
}
