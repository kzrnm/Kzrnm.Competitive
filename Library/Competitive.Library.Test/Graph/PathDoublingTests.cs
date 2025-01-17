using System;

namespace Kzrnm.Competitive.Testing.Graph
{
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
                    db.Move(i, k).ShouldBe((i + k) % arr.Length);
            Span<ulong> ks = stackalloc ulong[1];
            for (int t = 0; t < 64; t++)
            {
                rnd.NextBytes(ks.Cast<ulong, byte>());
                var k = ks[0];
                for (int i = 0; i < N; i++)
                {
                    db.Move(i, k).ShouldBe((int)(((uint)i + k) % (uint)arr.Length));
                    if ((long)k >= 0)
                        db.Move(i, (long)k).ShouldBe((int)(((uint)i + k) % (uint)arr.Length));
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
            db.Move(0, 1).ShouldBe(-1);
            db.Move(1, 1).ShouldBe(0);
            db.Move(2, 1).ShouldBe(2);
            db.Move(3, 1).ShouldBe(1);
            db.Move(4, 1).ShouldBe(3);
            db.Move(5, 1).ShouldBe(2);
            db.Move(6, 1).ShouldBe(1);

            db.Move(0, 2).ShouldBe(-1);
            db.Move(1, 2).ShouldBe(-1);
            db.Move(2, 2).ShouldBe(2);
            db.Move(3, 2).ShouldBe(0);
            db.Move(4, 2).ShouldBe(1);
            db.Move(5, 2).ShouldBe(2);
            db.Move(6, 2).ShouldBe(0);

            db.Move(0, 3).ShouldBe(-1);
            db.Move(1, 3).ShouldBe(-1);
            db.Move(2, 3).ShouldBe(2);
            db.Move(3, 3).ShouldBe(-1);
            db.Move(4, 3).ShouldBe(0);
            db.Move(5, 3).ShouldBe(2);
            db.Move(6, 3).ShouldBe(-1);

            db.Move(0, 4).ShouldBe(-1);
            db.Move(1, 4).ShouldBe(-1);
            db.Move(2, 4).ShouldBe(2);
            db.Move(3, 4).ShouldBe(-1);
            db.Move(4, 4).ShouldBe(-1);
            db.Move(5, 4).ShouldBe(2);
            db.Move(6, 4).ShouldBe(-1);

            db.Move(0, 5).ShouldBe(-1);
            db.Move(1, 5).ShouldBe(-1);
            db.Move(2, 5).ShouldBe(2);
            db.Move(3, 5).ShouldBe(-1);
            db.Move(4, 5).ShouldBe(-1);
            db.Move(5, 5).ShouldBe(2);
            db.Move(6, 5).ShouldBe(-1);

            db.Move(0, 1L << 20).ShouldBe(-1);
            db.Move(1, 1L << 20).ShouldBe(-1);
            db.Move(2, 1L << 20).ShouldBe(2);
            db.Move(3, 1L << 20).ShouldBe(-1);
            db.Move(4, 1L << 20).ShouldBe(-1);
            db.Move(5, 1L << 20).ShouldBe(2);
            db.Move(6, 1L << 20).ShouldBe(-1);
        }
    }
}
