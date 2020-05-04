using System;
using System.Collections.Generic;


class 素数
{
    /**
     * <summary>
     * 素因数分解
     * </summary>
     * <param name="num">素因数分解する数</param>
     * <returns>素因数の一覧</returns>
     */
    Dictionary<int, int> PrimeFactoring(int num)
    {
        var primes = Eratosthenes((int)Math.Sqrt(num));
        var primeFactors = new Dictionary<int, int>();

        foreach (var p in primes)
        {
            if (num < 2) break;
            while (num % p == 0)
            {
                int v;
                primeFactors.TryGetValue(p, out v);
                primeFactors[p] = v + 1;
                num /= p;
            }
        }

        if (num > 1)
            primeFactors[num] = 1;

        return primeFactors;
    }
    /**
     * <summary>
     * エラトステネスの篩で素数一覧を返す
     * </summary>
     * <param name="n">上限</param>
     * <returns>素数一覧</returns>
     */
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
}
