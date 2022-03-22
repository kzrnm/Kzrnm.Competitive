using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    // verification-helper: SAMEAS Library/run.test.py
    public class PathDoublingTests
    {
        [Fact]
        public void Move()
        {
            const int N = 100;
            var arr = new int[N];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (i + 1) % arr.Length;
            var rnd = new Random();
            var db = new PathDoubling(arr);

            for (int i = 0; i < N; i++)
                for (int k = 0; k < 20; k++)
                    db.Move(i, k).Should().Be((i + k) % arr.Length);
            Span<ulong> ks = stackalloc ulong[1];
            for (int t = 0; t < 64; t++)
            {
                rnd.NextBytes(ks.Cast<ulong, byte>());
                var k = ks[0];
                for (int i = 0; i < N; i++)
                {
                    db.Move(i, k).Should().Be((int)(((uint)i + k) % (uint)arr.Length));
                    if ((long)k >= 0)
                        db.Move(i, (long)k).Should().Be((int)(((uint)i + k) % (uint)arr.Length));
                }
            }
        }

        [Fact]
        public void Tree()
        {
            var arr = new int[7]
            {
                -1,
                0,
                2,
                1,
                3,
                2,
                1,
            };
            var db = new PathDoubling(arr);
            db.Move(0, 1).Should().Be(-1);
            db.Move(1, 1).Should().Be(0);
            db.Move(2, 1).Should().Be(2);
            db.Move(3, 1).Should().Be(1);
            db.Move(4, 1).Should().Be(3);
            db.Move(5, 1).Should().Be(2);
            db.Move(6, 1).Should().Be(1);

            db.Move(0, 2).Should().Be(-1);
            db.Move(1, 2).Should().Be(-1);
            db.Move(2, 2).Should().Be(2);
            db.Move(3, 2).Should().Be(0);
            db.Move(4, 2).Should().Be(1);
            db.Move(5, 2).Should().Be(2);
            db.Move(6, 2).Should().Be(0);

            db.Move(0, 3).Should().Be(-1);
            db.Move(1, 3).Should().Be(-1);
            db.Move(2, 3).Should().Be(2);
            db.Move(3, 3).Should().Be(-1);
            db.Move(4, 3).Should().Be(0);
            db.Move(5, 3).Should().Be(2);
            db.Move(6, 3).Should().Be(-1);

            db.Move(0, 4).Should().Be(-1);
            db.Move(1, 4).Should().Be(-1);
            db.Move(2, 4).Should().Be(2);
            db.Move(3, 4).Should().Be(-1);
            db.Move(4, 4).Should().Be(-1);
            db.Move(5, 4).Should().Be(2);
            db.Move(6, 4).Should().Be(-1);

            db.Move(0, 5).Should().Be(-1);
            db.Move(1, 5).Should().Be(-1);
            db.Move(2, 5).Should().Be(2);
            db.Move(3, 5).Should().Be(-1);
            db.Move(4, 5).Should().Be(-1);
            db.Move(5, 5).Should().Be(2);
            db.Move(6, 5).Should().Be(-1);

            db.Move(0, 1L << 20).Should().Be(-1);
            db.Move(1, 1L << 20).Should().Be(-1);
            db.Move(2, 1L << 20).Should().Be(2);
            db.Move(3, 1L << 20).Should().Be(-1);
            db.Move(4, 1L << 20).Should().Be(-1);
            db.Move(5, 1L << 20).Should().Be(2);
            db.Move(6, 1L << 20).Should().Be(-1);
        }
    }
}
