using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.Comparer
{
    public class DefaultComparerStructTests
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
            Array.Sort(arr, new DefaultComparerStruct<int>());
            arr.Should().Equal(
                49770599,
                98686814,
                216600710,
                289393729,
                382174879,
                705468652,
                910065328,
                1022730976,
                1102883983,
                1263693310,
                1266617064,
                1436977866,
                1506739568,
                1507175202,
                1569600475,
                1580055797,
                1790270117,
                1850902760,
                1983045250,
                2014529243);
        }
    }
}
