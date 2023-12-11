using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class RunEnumerateTests
    {
        public static IEnumerable<(string, (int From, int ToExclusive)[][])> RunEnumerate_Data()
        {
            yield return ("", new (int From, int ToExclusive)[1][]
            {
                [],
            });
            yield return ("mississippi", new (int From, int ToExclusive)[][]
            {
                [],
                [(2,4),(5,7),(8,10)],
                [],
                [(1,8)],
                [],
                [],
            });
            yield return ("aaaaaaa", new (int From, int ToExclusive)[][]
            {
                [],
                [(0,7)],
                [],
                [],
            });
        }

        [Theory]
        [TupleMemberData(nameof(RunEnumerate_Data))]
        public void RunEnumerate(string s, (int From, int ToExclusive)[][] expected)
        {
            ShouldEqual(StringLibEx.RunEnumerate(s), expected);
            ShouldEqual(StringLibEx.RunEnumerate(s.Select(t => (int)t).ToArray()), expected);

            static void ShouldEqual((int From, int ToExclusive)[][] got, (int From, int ToExclusive)[][] expected)
            {
                got.Should().HaveSameCount(expected);
                for (int i = 0; i < got.Length; i++)
                {
                    got[i].Should().Equal(expected[i], "i = {0}", i);
                }
            }
        }
    }
}
