using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class PreriodTests
    {
        public static IEnumerable<(int[], int)> PeriodInt_Data()
        {
            for (int i = 1; i < 10; i++)
            {
                var loop = Enumerable.Range(1, i);
                for (int r = 1; r < 5; r++)
                {
                    yield return (Enumerable.Repeat(loop, r).SelectMany(a => a).ToArray(), i);
                }
            }
            yield return (new[] { 1, 2, 3, 1, 2, }, 5);
        }

        [Theory]
        [TupleMemberData(nameof(PeriodInt_Data))]
        public void PeriodInt(int[] s, int expected)
        {
            StringLibEx.Period(s).ShouldBe(expected);
            var ss = s.AsSpan()[..expected];
            for (int i = 0; i < expected; i++)
            {
                if (s.Length % expected == 0)

                    for (int j = expected; j < s.Length; j += expected)
                    {
                        s.AsSpan(j, expected).StartsWith(ss).ShouldBeTrue();
                    }
            }
        }

        [Fact]
        public void PeriodString()
        {
            StringLibEx.Period("aaa").ShouldBe(1);
            StringLibEx.Period("ababababab").ShouldBe(2);
            StringLibEx.Period("abc").ShouldBe(3);
            StringLibEx.Period("abcabc").ShouldBe(3);
            StringLibEx.Period("ababa").ShouldBe(5);
        }
    }
}
