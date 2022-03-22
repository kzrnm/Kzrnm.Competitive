using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    // verification-helper: SAMEAS Library/run.test.py
    public class ConvexHullTrickTests
    {
        [Fact]
        public void Min()
        {
            var xs = new long[] { 0, 10, 12, 20, 29, 30, 40, 47, 50 };
            var cht = new LongMinConvexHullTrick(xs);
            var native = new MinConvexHullTrickNative();
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(cht.YINF);

            cht.AddLine(2, 3);
            native.AddLine(2, 3);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(-6, 300);
            native.AddLine(-6, 300);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(1, 30);
            native.AddLine(1, 30);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(1, 50);
            native.AddLine(1, 50);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(1, 500);
            native.AddLine(1, 500);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(0, 50);
            native.AddLine(0, 50);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));
        }
        [Fact]
        public void Max()
        {
            var xs = new long[] { 0, 10, 12, 20, 29, 30, 40, 47, 50 };
            var cht = new LongMaxConvexHullTrick(xs);
            var native = new MaxConvexHullTrickNative();
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(cht.YINF);

            cht.AddLine(2, 3);
            native.AddLine(2, 3);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(-6, 300);
            native.AddLine(-6, 300);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(1, 30);
            native.AddLine(1, 30);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(1, 50);
            native.AddLine(1, 50);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(1, 500);
            native.AddLine(1, 500);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));

            cht.AddLine(0, 50);
            native.AddLine(0, 50);
            for (int i = 0; i < xs.Length; i++)
                cht.Query(i).Should().Be(native.Min(xs[i]));
        }

        private class MinConvexHullTrickNative
        {
            static long F(long a, long b, long x) => a * x + b;
            private readonly List<(long a, long b)> list = new();
            public void AddLine(long a, long b) => list.Add((a, b));
            public long Min(long x)
            {
                long min = 1000000000000000000;
                foreach (var (a, b) in list)
                    min = Math.Min(min, F(a, b, x));
                return min;
            }
        }

        private class MaxConvexHullTrickNative
        {
            static long F(long a, long b, long x) => a * x + b;
            private readonly List<(long a, long b)> list = new();
            public void AddLine(long a, long b) => list.Add((a, b));
            public long Min(long x)
            {
                long min = -1000000000000000000;
                foreach (var (a, b) in list)
                    min = Math.Max(min, F(a, b, x));
                return min;
            }
        }
    }
}
