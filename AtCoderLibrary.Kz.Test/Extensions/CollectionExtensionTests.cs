using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AtCoder
{
    public class CollectionExtensionTests
    {
        [Fact]
        public void ListAsSpan()
        {
            new List<long> {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.AsSpan()[18..].ToArray().Should().Equal(new long[] { 120, 42314, 3123 });
        }

        [Fact]
        public void ArrayGet()
        {
            var arr = new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            };
            arr.Get(1).Should().Be(24);
            arr.Get(1) = 25;
            arr[1].Should().Be(25);

            arr.Get(-1).Should().Be(3123);
            arr.Get(-1) = -2;
            arr[^1].Should().Be(-2);
        }

        [Fact]
        public void DicGet()
        {
            var dic = new Dictionary<string, int> {
                {"foo",1 },
                {"bar",-1 },
                {"😀",10 },
                {"でぃ",0 },
            };
            dic.Get("foo").Should().Be(1);
            dic.Get("bar").Should().Be(-1);
            dic.Get("😀").Should().Be(10);
            dic.Get("でぃ").Should().Be(0);
            dic.Get("invalid").Should().Be(0);
        }
    }
}