using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AtCoder.GlobalNS
{
    public class GlobalTests
    {
        [Fact]
        public void Compress()
        {
            Global.Compress("compress").Should().Equal(new Dictionary<char, int>
            {
                { 'c',0 },
                { 'o',3 },
                { 'm',2 },
                { 'p',4 },
                { 'r',5 },
                { 'e',1 },
                { 's',6 },
            });
            Global.Compressed("compress".AsSpan()).Should().Equal(0, 3, 2, 4, 5, 1, 6, 6);
        }
    }
}
