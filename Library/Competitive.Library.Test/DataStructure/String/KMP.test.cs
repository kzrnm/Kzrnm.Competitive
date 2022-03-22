﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    // verification-helper: SAMEAS Library/run.test.py
    public class KMPTests
    {
        public static TheoryData Match_Data = new TheoryData<string, string, IEnumerable<int>>
        {
            { "ab", new string('q',1998)+"ab", new int[]{ 1998 } },
            { "abc", "abd", Array.Empty<int>() },
        };

        [Theory]
        [MemberData(nameof(Match_Data))]
        public void Matches(string pattern, string target, IEnumerable<int> indexes)
        {
            var kmp = KMP.Create(pattern);
            kmp.Matches(target).ToList().Should().Equal(indexes);
        }
    }
}
