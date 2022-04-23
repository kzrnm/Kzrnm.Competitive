using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class PrimeNumber : ICollection<int>
    {
        readonly int[] primes;
        readonly int[] searches;
        public int Count => primes.Length;
        public bool IsReadOnly => true;

        public PrimeNumber(int max)
        {
            (primes, searches) = Eratosthenes(max);
        }
        [凾(256)]
        public Dictionary<long, int> PrimeFactoring(long num)
        {
            var primeFactors = new Dictionary<long, int>();
            foreach (var p in EnumerateFactor(num))
            {
                primeFactors[p] = primeFactors.Get(p) + 1;
            }
            return primeFactors;
        }
        [凾(256)]
        public Dictionary<int, int> PrimeFactoring(int num)
        {
            if (num < searches.Length) return PrimeFactoringFast(num);
            var primeFactors = new Dictionary<int, int>();
            foreach (var pl in EnumerateFactor(num))
            {
                var p = (int)pl;
                primeFactors[p] = primeFactors.Get(p) + 1;
            }
            return primeFactors;
        }
        [凾(256)]
        IEnumerable<long> EnumerateFactor(long num)
        {
            foreach (var p in primes)
            {
                if (num < 2) break;
                while (DivIfMulti(ref num, p))
                    yield return p;
            }
            if (num > 1) yield return num;
        }
        [凾(256)]
        static bool DivIfMulti(ref long num, long p)
        {
            var q = Math.DivRem(num, p, out var d);
            if (d == 0)
            {
                num = q;
                return true;
            }
            return false;
        }
        [凾(256)]
        Dictionary<int, int> PrimeFactoringFast(int num)
        {
            if (num >= searches.Length) throw new ArgumentOutOfRangeException(nameof(num));
            var primeFactors = new Dictionary<int, int>();
            while (num > 1)
            {
                primeFactors[searches[num]] = primeFactors.Get(searches[num]) + 1;
                num /= searches[num];
            }
            return primeFactors;
        }
        [凾(256)]
        static (int[] primes, int[] searches) Eratosthenes(int n)
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
                {
                    searches[i] = i;
                    primes.Add(i);
                }

            return (primes.ToArray(), searches);
        }
        /// <summary>
        /// <para>素数かどうか判定します。</para>
        /// <para><paramref name="num"/> が <see langword="this"/> の最大の2乗より大きい場合は誤って <see langword="true"/> を返す可能性があります。</para>
        /// </summary>
        [凾(256)]
        public bool IsPrime(long num)
        {
            if (num <= primes[^1])
                return Contains((int)num);
            foreach (var p in this)
                if (num % p == 0)
                    return false;
            return true;
        }
        public void Add(int item) => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();
        public bool Contains(int item) => Array.BinarySearch(primes, item) >= 0;
        public void CopyTo(int[] array, int arrayIndex) => primes.CopyTo(array, arrayIndex);
        public bool Remove(int item) => throw new NotSupportedException();
        public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(primes).GetEnumerator();
        IEnumerator<int> IEnumerable<int>.GetEnumerator() => ((IEnumerable<int>)primes).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => primes.GetEnumerator();
    }

}
