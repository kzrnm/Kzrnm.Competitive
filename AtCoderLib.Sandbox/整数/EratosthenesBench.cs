using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class EratosthenesBench
{
    [Params(10000000)]
    public int n;

    [Benchmark(Baseline = true)]
    public (int[] primes, int[] searches) Eratosthenes10()
    {
        var searches = new int[n + 1];
        Array.Copy(new int[11] { 0, 1, 2, 3, 2, 5, 2, 7, 2, 3, 2 }, searches, Math.Min(11, searches.Length));

        var primes = new List<int>(n) { 2, 3, 5, 7 };
        if (n < 11) return (primes.TakeWhile(p => p <= n).ToArray(), searches);

        int current;
        for (var i = 10; i < searches.Length; i += 10)
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
        current = 11;
        while (current * current <= n)
        {
            primes.Add(current);
            var c2 = current << 1;
            for (var i = current; i < searches.Length; i += c2)
                if (searches[i] == 0)
                    searches[i] = current;

            while (searches[current] > 0) current += 2;
        }
        for (var i = current; i < searches.Length; i += 2)
            if (searches[i] == 0)
                primes.Add(i);

        return (primes.ToArray(), searches);
    }

    [Benchmark]
    public (int[] primes, int[] searches) EratosthenesEach()
    {
        var searches = new int[n + 1];
        Array.Copy(new int[11] { 0, 1, 2, 3, 2, 5, 2, 7, 2, 3, 2 }, searches, Math.Min(11, searches.Length));

        var primes = new List<int>(n) { 2, 3, 5, 7 };
        if (n < 11) return (primes.TakeWhile(p => p <= n).ToArray(), searches);

        for (int i = 11; i < searches.Length; i += 2)
        {
            searches[i - 1] = 2;
            if (i % 3 == 0)
                searches[i] = 3;
            else if (i % 5 == 0)
                searches[i] = 5;
            else if (i % 7 == 0)
                searches[i] = 7;
        }
        if (n % 2 == 0) searches[n] = 2;

        var current = 11;
        while (current * current <= n)
        {
            primes.Add(current);
            var c2 = current << 1;
            for (var i = current; i < searches.Length; i += c2)
                if (searches[i] == 0)
                    searches[i] = current;

            while (searches[current] > 0) current += 2;
        }
        for (var i = current; i < searches.Length; i += 2)
            if (searches[i] == 0)
                primes.Add(i);

        return (primes.ToArray(), searches);
    }
}
