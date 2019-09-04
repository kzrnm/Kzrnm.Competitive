using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitArray = System.Collections.BitArray;
using BigInteger = System.Numerics.BigInteger;

#pragma warning disable 

namespace AtCoderProject.Hide
{
    public class 整数
    {
        int Gcd(params int[] nums)
        {
            var gcd = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                gcd = Gcd(nums[i], gcd);
            }
            return gcd;
        }
        int Gcd(int a, int b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
        IEnumerable<int> GetYakusu(int n)
        {
            int amari;
            var sqrt = (int)Math.Sqrt(n);
            for (int i = 1; i <= sqrt; i++)
            {
                var d = Math.DivRem(n, i, out amari);
                if (amari == 0)
                {
                    yield return i;
                    if (i != d)
                        yield return d;
                }
            }
        }

        IEnumerable<int> Factoring(int num)
        {
            int o = 0;
            int sqrt = (int)Math.Sqrt(num);
            int amari;
            foreach (var p in new[] { 2, 3, 5, 7 })
            {
                while (true)
                {
                    var d = Math.DivRem(num, p, out amari);
                    if (amari == 0)
                    {
                        yield return p;
                        num = d;
                    }
                    else if (num == 1) yield break;
                    else break;
                }
            }

            while (num > 1)
            {
                o += 10;
                if (sqrt < o) break;
                foreach (var p in new[] { o + 1, o + 3, o + 7, o + 9 })
                {
                    while (true)
                    {
                        var d = Math.DivRem(num, p, out amari);
                        if (amari == 0)
                        {
                            yield return p;
                            num = d;
                        }
                        else if (num == 1) yield break;
                        else break;
                    }
                }
            }
            yield return num;
        }

        /// <summary>
        /// 組合せ関数
        /// </summary>
        /// <param name="n">全体</param>
        /// <param name="k">取り出す個数</param>
        /// <returns></returns>
        BigInteger Combination(int n, int k)
        {
            if (n / 2 < k) return Combination(n, n - k);

            BigInteger ret = 1;
            for (var i = 0; i < k; i++)
            {
                ret *= n - i;
                ret /= i + 1;
            }
            return ret;
        }
        class Comb
        {
            private int mod;
            private long[] fac;
            private long[] finv;
            private long[] inv;
            public Comb(int maxN, int mod)
            {
                maxN++;
                this.mod = mod;
                this.fac = new long[maxN];
                this.finv = new long[maxN];
                this.inv = new long[maxN];

                fac[0] = fac[1] = 1;
                finv[0] = finv[1] = 1;
                inv[1] = 1;
                for (int i = 2; i < maxN; i++)
                {
                    fac[i] = fac[i - 1] * i % mod;
                    inv[i] = mod - inv[mod % i] * (mod / i) % mod;
                    finv[i] = finv[i - 1] * inv[i] % mod;
                }
            }
            public long Combination(int n, int k)
            {
                if (n < k) return 0;
                if (n < 0 || k < 0) return 0;
                return fac[n] * (finv[k] * finv[n - k] % mod) % mod;
            }
        }
    }
}
