using System;
using System.Collections.Generic;


class 素数
{

    Dictionary<long, int> PrimeFactoring(long num)
    {
        var primes = Eratosthenes((long)Math.Sqrt(num));
        var primeFactors = new Dictionary<long, int>();

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


    static HashSet<long> Eratosthenes(long n)
    {
        var primes = new HashSet<long> { 2, 3, 5, 7 };
        var searches = new long[n + 1];
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
        var sqrtN = (long)Math.Sqrt(n);
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
