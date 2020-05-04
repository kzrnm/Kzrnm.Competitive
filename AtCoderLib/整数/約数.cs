using System;
using System.Collections.Generic;


class 約数
{
    /**
     * <summary>
     * 約数
     * </summary>
     * <param name="num">約数を求める数</param>
     * <returns>約数の一覧</returns>
     */
    IEnumerable<int> GetYakusu(int n)
    {
        var list = new List<int>();
        var sqrt = (int)Math.Sqrt(n);
        for (var i = 1; i <= sqrt; i++)
        {
            var d = Math.DivRem(n, i, out int amari);
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
