using System;
using System.Collections.Generic;


class 約数
{


    public static IEnumerable<int> GetYakusu(int n)
    {
        var list = new List<int>();

        for (int i = 1, d = Math.DivRem(n, i, out int amari);
            i <= d;
            i++, d = Math.DivRem(n, i, out amari))
        {
            if (amari == 0)
            {
                yield return i;
                if (i != d)
                    list.Add(d);
            }
        }

        for (var i = list.Count - 1; i >= 0; i--)
            yield return list[i];
    }


    public static IEnumerable<long> GetYakusu(long n)
    {
        var list = new List<long>();

        for (long i = 1, d = Math.DivRem(n, i, out long amari);
            i <= d;
            i++, d = Math.DivRem(n, i, out amari))
        {
            if (amari == 0)
            {
                yield return i;
                if (i != d)
                    list.Add(d);
            }
        }

        for (var i = list.Count - 1; i >= 0; i--)
            yield return list[i];
    }


}
