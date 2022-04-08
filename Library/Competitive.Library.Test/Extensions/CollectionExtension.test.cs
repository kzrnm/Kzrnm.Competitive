using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Testing.Extensions
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class CollectionExtensionTests
    {
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