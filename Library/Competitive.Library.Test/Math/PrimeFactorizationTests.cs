namespace Kzrnm.Competitive.Testing.MathNS;

[NotInParallel]
public class PrimeFactorizationTests
{
    [Test, MultipleAssertions]
    public async Task IsPrime()
    {
        var p = new PrimeNumber(1 << 20);
        for (int i = 0; i < 2000; i++)
        {
            foreach (var start in new[] {
            0,
            (1L << 30) - 500,
            1795265022 - 500,
            4759123141 - 500,
        })
            {
                var x = start + i;
                await PrimeFactorization.IsPrime(x).Should().BeEqualTo(p.IsPrime(x));
            }
        }
        await PrimeFactorization.IsPrime(200560490131).Should().BeTrue();
        await PrimeFactorization.IsPrime(92709568269121).Should().BeTrue();
        await PrimeFactorization.IsPrime(9007199254740997).Should().BeTrue();
        await PrimeFactorization.IsPrime(1162193L * 1347377).Should().BeFalse();
        await PrimeFactorization.IsPrime(89652331L * 96325939).Should().BeFalse();
    }

    public static IEnumerable<(int, int[])> DivisorInt_Data =>
    [
        (
            1,
            [1]
        ),
        (
            1 << 16,
            Enumerable.Range(0, 17).Select(i => 1 << i).ToArray()
        ),
        (
            49,
            [1, 7, 49,]
        ),
        (
            2 * 3 * 5,
            [1, 2, 3, 5, 6, 10, 15, 30,]
        ),
        (
            720,
            [
                1, 2, 3, 4, 5, 6, 8, 9, 10,
                12, 15, 16, 18, 20, 24, 30,
                36, 40, 45, 48, 60, 72, 80,
                90, 120, 144, 180, 240, 360, 720,
            ]
        ),
        (
            2147483647,
            [1, 2147483647,]
        ),
    ];

    [Test]
    [MethodDataSource(nameof(DivisorInt_Data))]
    public async Task DivisorInt(int num, int[] expected)
    {
        await PrimeFactorization.Divisor(num).Should().BeEquivalentOrderTo(expected);
    }

    [Test, MultipleAssertions]
    public async Task DivisorIntLarge()
    {
        var divisor6480 = PrimeFactorization.Divisor(6480);
        await divisor6480.Length.Should().BeEqualTo(50);
        await divisor6480[..26].Should().BeEquivalentOrderTo([1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 27, 30, 36, 40, 45, 48, 54, 60, 72, 80, 81]);
        await divisor6480[^4..].Should().BeEquivalentOrderTo([1620, 2160, 3240, 6480]);

        await PrimeFactorization.Divisor(2095133040).Length.Should().BeEqualTo(1600); //高度合成数
    }
    [Test, MultipleAssertions]
    public async Task DivisorLong()
    {
        await PrimeFactorization.Divisor(1L).Should().BeEquivalentOrderTo([1L]);
        await PrimeFactorization.Divisor(128100283921).Should().BeEquivalentOrderTo([
            1,
            71,
            5041,
            357911,
            25411681,
            1804229351,
            128100283921]);
        await PrimeFactorization.Divisor(132147483703).Should().BeEquivalentOrderTo([1, 132147483703]);
        await PrimeFactorization.Divisor(963761198400).Length.Should().BeEqualTo(6720); //高度合成数
        await PrimeFactorization.Divisor(897612484786617600).Length.Should().BeEqualTo(103680); //高度合成数

        await PrimeFactorization.Divisor(9007199254740997).Should().BeEquivalentOrderTo([1, 9007199254740997]);
        await PrimeFactorization.Divisor(89652331L * 96325939).Should().BeEquivalentOrderTo([
            1,
            89652331,
            96325939,
            89652331L * 96325939,
        ]);
    }

    public static IEnumerable<(int, (int, int)[])> PrimeFactoringInt_Data =>
    [
        (
            1,
            []
        ),
        (
            1 << 16,
            [
                (2, 16),
            ]
        ),
        (
            2 * 3 * 5,
            [
                (2, 1),
                (3, 1),
                (5, 1),
            ]
        ),
        (
            99991,
            [
                (99991, 1),
            ]
        ),
        (
            2147483647,
            [
                (2147483647, 1),
            ]
        ),
        (
            2095133040, //高度合成数
            [
                (2, 4),
                (3, 4),
                (5, 1),
                (7, 1),
                (11, 1),
                (13, 1),
                (17, 1),
                (19, 1),
            ]
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(PrimeFactoringInt_Data))]
    public async Task PrimeFactoringInt(int num, (int, int)[] expected)
    {
        var f = PrimeFactorization.PrimeFactoring(num);
        await f.Should().HaveCount(expected.Length);
        foreach (var (k, count) in expected)
        {
            await f.Should().ContainKeyWithValue(k, count);
        }
    }

    public static IEnumerable<(long, (long, int)[])> PrimeFactoringLong_Data =>
    [
        (
            1,
            []
        ),
        (
            903906555552,
            [
                (2, 5),
                (3, 8),
                (7, 1),
                (11, 2),
                (13, 1),
                (17, 1),
                (23, 1),
            ]
        ),
        (
            132147483703,
            [
                (132147483703, 1),
            ]
        ),
        (
            963761198400, //高度合成数
            [
                (2, 6),
                (3, 4),
                (5, 2),
                (7, 1),
                (11, 1),
                (13, 1),
                (17, 1),
                (19, 1),
                (23, 1),
            ]
        ),
        (
            897612484786617600, //高度合成数
            [
                (2, 8),
                (3, 4),
                (5, 2),
                (7, 2),
                (11, 1),
                (13, 1),
                (17, 1),
                (19, 1),
                (23, 1),
                (29, 1),
                (31, 1),
                (37, 1),
            ]
        ),
        (
            89652331L * 96325939,
            [
                (89652331, 1),
                (96325939, 1),
            ]
        ),
        (
            9007199254740997,
            [
                (9007199254740997, 1),
            ]
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(PrimeFactoringLong_Data))]
    public async Task PrimeFactoringLong(long num, (long, int)[] expected)
    {
        var f = PrimeFactorization.PrimeFactoring(num);
        await f.Should().HaveCount(expected.Length);
        foreach (var (k, count) in expected)
        {
            await f.Should().ContainKeyWithValue(k, count);
        }
    }

    public static IEnumerable<long> StressDivisor_Data()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 10; q++)
            yield return rnd.NextLong() % (1L << 45);
    }

    [Test]
    [MethodDataSource(nameof(StressDivisor_Data))]
    public async Task StressDivisor(long n)
    {
        await PrimeFactorization.Divisor(n).Should().BeEquivalentOrderTo(NaiveDivisor(n));
    }

    [Test, MultipleAssertions]
    public async Task SmallDivisor()
    {
        for (int i = 1; i < 257 * 257 + 50; i++)
            await PrimeFactorization.Divisor(i)
                     .Should().BeEquivalentOrderTo(NaiveDivisor(i).Select(n => checked((int)n)));
    }


    static long[] NaiveDivisor(long n)
    {
        if (n <= 1) return [1];

        var left = new List<long>();
        var right = new List<long>();
        left.Add(1);
        right.Add(n);

        for (long i = 2, d = Math.DivRem(n, i, out var am);
            i <= d;
            i++, d = Math.DivRem(n, i, out am))
        {
            if (am == 0)
            {
                left.Add(i);
                if (i != d)
                    right.Add(d);
            }
        }
        right.Reverse();
        var res = new long[left.Count + right.Count];
        left.CopyTo(res, 0);
        right.CopyTo(res, left.Count);
        return res;
    }
}