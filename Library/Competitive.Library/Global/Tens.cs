using System;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 10の倍数を管理する
    /// </summary>
    public static class Tens
    {
        public static ReadOnlySpan<int> Ints => new[]
        {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
        };
        public static ReadOnlySpan<long> Longs => new[]
        {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
            10000000000,
            100000000000,
            1000000000000,
            10000000000000,
            100000000000000,
            1000000000000000,
            10000000000000000,
            100000000000000000,
            1000000000000000000,
        };
    }
}