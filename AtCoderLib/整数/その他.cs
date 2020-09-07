using System;
using System.Collections.Generic;
using System.Text;


static class その他
{
    ///*<summary>
    /// Sum[(<paramref name="a"/>*<paramref name="i"/>+<paramref name="b"/>)/<paramref name="m"/>] 0&lt;<paramref name="i"/>&lt;<paramref name="n"/>
    /// </summary>
    public static long FloorSum(long n, long m, long a, long b)
    {
        long ans = 0;
        if (a >= m)
        {
            ans += (n - 1) * n * (a / m) / 2;
            a %= m;
        }
        if (b >= m)
        {
            ans += n * (b / m);
            b %= m;
        }

        long y_max = (a * n + b) / m, x_max = (y_max * m - b);
        if (y_max == 0) return ans;
        ans += (n - (x_max + a - 1) / a) * y_max;
        ans += FloorSum(y_max, a, m, (a - x_max % a) % a);
        return ans;
    }
}
