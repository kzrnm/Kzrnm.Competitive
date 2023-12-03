using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class RollingHashEditableTests
    {
        Random rnd = new Random(227);
        [Fact]
        public void Static()
        {
            var str = rnd.NextIntArray(100, 1, 4);
            var rh = RollingHash.Create(str);
            var rhe = RollingHashEditable.Create(str);
            for (int l1 = 0; l1 < str.Length; l1++)
                for (int r1 = l1 + 1; r1 <= str.Length; r1++)
                    rhe[l1..r1].Should().Be(rh[l1..r1]);
        }

        [Fact]
        public void Large()
        {
            const int length = 20;
            Span<int> str = rnd.NextIntArray(length, 1, 3);
            var rh = RollingHashEditable.Create(str);

            for (int q = 0; q < 4; q++)
            {
                for (int p = 0; p < 20; p++)
                {
                    var ix = rnd.Next(length);
                    var v = rnd.Next(3);
                    rh.Set(ix, v);
                    str[ix] = v;
                }
                var notMatchCount = 0;
                var notMatchHashNotMatchCount = 0;

                for (int l1 = 0; l1 < str.Length; l1++)
                    for (int r1 = l1 + 1; r1 <= str.Length; r1++)
                        for (int l2 = 0; l2 < str.Length; l2++)
                            for (int r2 = l2 + 1; r2 <= str.Length; r2++)
                            {
                                if (str[l1..r1].SequenceEqual(str[l2..r2]))
                                {
                                    rh[l1..r1].Should().Be(rh[l2..r2]);
                                }
                                else
                                {
                                    notMatchCount++;
                                    if (rh[l1..r1] != rh[l2..r2])
                                        notMatchHashNotMatchCount++;
                                }
                            }
                ((double)notMatchHashNotMatchCount / notMatchCount).Should().BeGreaterThan(0.9999);
            }
        }
    }
}