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
            RollingHashEditable.ResizePow(10000);
            const int length = 20;
            var sl = rnd.NextIntArray(length, 1, 3).ToList();
            var rh = RollingHashEditable.Create(sl.AsSpan());

            for (int q = 0; q < 4; q++)
            {
                for (int p = 0; p < 5; p++)
                {
                    var v = rnd.Next(3);
                    var ix = rnd.Next(rh.Length);
                    switch (rnd.Next(4))
                    {
                        case 0:
                            sl[ix] = v;
                            rh.Set(ix, v);
                            break;
                        case 1:
                            rh.Add(v);
                            sl.Add(v);
                            break;
                        case 2:
                            rh.Insert(ix, v);
                            sl.Insert(ix, v);
                            break;
                        case 3:
                            rh.RemoveAt(ix);
                            sl.RemoveAt(ix);
                            break;

                    }
                }
                var re = RollingHash.Create(sl.AsSpan());

                rh.Length.Should().Be(re.Length);
                for (int l = 0; l < rh.Length; l++)
                    for (int r = l + 1; r <= rh.Length; r++)
                        rh[l..r].Should().Be(re[l..r]);
            }
        }
    }
}