using System;
using System.Collections.Generic;
using System.Linq;
using BigInteger = System.Numerics.BigInteger;


namespace AtCoderProject.Hide
{
    public class 整数
    {
        int Gcd(params int[] nums)
        {
            var gcd = nums[0];
            for (var i = 1; i < nums.Length; i++)
                gcd = Gcd(nums[i], gcd);
            return gcd;
        }
        int Gcd(int a, int b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
        int Lcm(int a, int b) => a / Gcd(a, b) * b;

        int Lcm(params int[] nums)
        {
            var lcm = nums[0];
            for (var i = 1; i < nums.Length; i++)
                lcm = Lcm(lcm, nums[i]);
            return lcm;
        }

        /// <summary>
        /// 約数
        /// </summary>
        /// <param name="num">約数を求める数</param>
        /// <returns>約数の一覧</returns>
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

        /// <summary>
        /// 素因数分解
        /// </summary>
        /// <param name="num">素因数分解する数</param>
        /// <returns>素因数の一覧</returns>
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
        /// エラトステネスの篩で素数一覧を返す
        /// </summary>
        /// <param name="n">上限</param>
        /// <returns>素数一覧</returns>
        static HashSet<int> Eratosthenes(int n)
        {
            var primes = new HashSet<int> { 2, 3, 5, 7 };
            var searches = new int[n + 1];
            int current;
            for (var i = 10; i <= n; i += 10)
            {
                current = i + 1;
                if (current > n) break;
                if (current % 3 == 0 || current % 7 == 0)
                    searches[current] = 7;
                current = i + 3;
                if (current > n) break;
                if (current % 3 == 0 || current % 7 == 0)
                    searches[current] = 7;
                current = i + 5;
                if (current > n) break;
                searches[current] = 5;
                current = i + 7;
                if (current > n) break;
                if (current % 3 == 0 || current % 7 == 0)
                    searches[current] = 7;
                current = i + 9;
                if (current > n) break;
                if (current % 3 == 0 || current % 7 == 0)
                    searches[current] = 7;
            }
            var sqrtN = (int)Math.Sqrt(n);
            current = 11;
            while (current <= sqrtN)
            {
                primes.Add(current);
                for (var i = current; i <= n; i += current + current)
                    searches[i] = current;

                while (searches[current] > 0) current += 2;
            }
            for (var i = current; i <= n; i += 2)
                if (searches[i] == 0)
                    primes.Add(i);

            return primes;
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
    }
}
