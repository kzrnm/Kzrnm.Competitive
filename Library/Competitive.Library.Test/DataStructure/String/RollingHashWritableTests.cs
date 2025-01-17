using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class RollingHashWritableTests
    {
        Random rnd = new Random(227);
        [Fact]
        public void Static()
        {
            var str = rnd.NextIntArray(100, 1, 4);
            var rh = RollingHash.Create(str);
            var rhe = RollingHashWritable.Create(str);
            for (int l1 = 0; l1 < str.Length; l1++)
                for (int r1 = l1 + 1; r1 <= str.Length; r1++)
                    rhe[l1..r1].ShouldBe(rh[l1..r1]);
        }

        [Fact]
        public void Large()
        {
            const int length = 20;
            Span<int> str = rnd.NextIntArray(length, 1, 3);
            var rh = RollingHashWritable.Create(str);

            for (int q = 0; q < 4; q++)
            {
                for (int p = 0; p < 20; p++)
                {
                    var ix = rnd.Next(length);
                    var v = rnd.Next(3);
                    rh.Set(ix, v);
                    str[ix] = v;
                }
                var re = RollingHash.Create(str);

                rh.Length.ShouldBe(re.Length);
                for (int l = 0; l < rh.Length; l++)
                    for (int r = l + 1; r <= rh.Length; r++)
                        rh[l..r].ShouldBe(re[l..r]);
            }
        }
    }
}