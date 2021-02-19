using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Extension
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

        [Fact]
        public void Flatten()
        {
            new[] { "abc", "def", "012", "345", "678" }.Flatten()
                .Should().Equal(
                new char[] { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8' });

            new[] {
                new[] { 1, 2, 3 },
                new[] { -1, -2, -3 },
                new[] { 4, 5, 6 },
                new[] { -6, -5, -4 },
                new[] { 7, 8, 9 },
            }.Flatten()
                .Should().Equal(
                new[] { 1, 2, 3, -1, -2, -3, 4, 5, 6, -6, -5, -4, 7, 8, 9 });
        }
    }
}