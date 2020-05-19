using System;
using System.Collections.Generic;


class 約数
{


    IEnumerable<long> GetYakusu(long n)
    {
        var list = new List<long>();
        var sqrt = (long)Math.Sqrt(n);
        for (var i = 1; i <= sqrt; i++)
        {
            var d = Math.DivRem(n, i, out long amari);
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
