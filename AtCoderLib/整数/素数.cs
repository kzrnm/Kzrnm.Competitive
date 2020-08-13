using System;
using System.Collections;
using System.Collections.Generic;


class PrimeNumber : ICollection<int>
{
    HashSet<int> primes;
    public int Count => primes.Count;
    public bool IsReadOnly => true;

    public PrimeNumber(int max)
    {
        primes = Eratosthenes(max);
    }
    public Dictionary<long, int> PrimeFactoring(long num)
    {
        var primeFactors = new Dictionary<long, int>();
        foreach (var p in EnumerateFactor(num))
        {
            primeFactors.TryGetValue(p, out var v);
            primeFactors[p] = v + 1;
        }
        return primeFactors;
    }
    public Dictionary<int, int> PrimeFactoring(int num)
    {
        var primeFactors = new Dictionary<int, int>();
        foreach (var pl in EnumerateFactor(num))
        {
            var p = (int)pl;
            primeFactors.TryGetValue(p, out var v);
            primeFactors[p] = v + 1;
        }
        return primeFactors;
    }
    private IEnumerable<long> EnumerateFactor(long num)
    {
        foreach (var p in primes)
        {
            if (num < 2) break;
            while (num % p == 0)
            {
                yield return p;
                num /= p;
            }
        }
        if (num > 1) yield return num;
    }
    HashSet<int> Eratosthenes(int n)
    {
        var primes = new HashSet<int> { 2, 3, 5, 7 };
        if (n < 11)
        {
            primes.RemoveWhere(p => p > n);
            return primes;
        }
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
        current = 11;
        while (current * current <= n)
        {
            primes.Add(current);
            for (var i = current; i <= n; i += (current << 1))
                searches[i] = current;

            while (searches[current] > 0) current += 2;
        }
        for (var i = current; i <= n; i += 2)
            if (searches[i] == 0)
                primes.Add(i);

        return primes;
    }

    public void Add(int item) => throw new NotSupportedException();
    public void Clear() => throw new NotSupportedException();
    public bool Contains(int item) => primes.Contains(item);
    public void CopyTo(int[] array, int arrayIndex) => primes.CopyTo(array, arrayIndex);
    public bool Remove(int item) => throw new NotSupportedException();
    public HashSet<int>.Enumerator GetEnumerator() => primes.GetEnumerator();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => primes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => primes.GetEnumerator();
}
