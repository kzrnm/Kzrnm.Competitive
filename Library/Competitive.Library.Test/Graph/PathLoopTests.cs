using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class PathLoopTests
    {
        [Fact]
        public void Move()
        {
            const int N = 180;
            var arr = new int[N];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (i + 1) % arr.Length;
            var rnd = new Random();
            var pl = Enumerable.Range(0, arr.Length).Select(i => new PathLoop(arr, i)).ToArray();

            for (int i = 0; i < N; i++)
                for (int k = 0; k < 20; k++)
                    pl[i].Move(k).ShouldBe((i + k) % arr.Length);
            Span<ulong> ks = stackalloc ulong[1];
            for (int t = 0; t < 128; t++)
            {
                rnd.NextBytes(ks.Cast<ulong, byte>());
                var k = ks[0];
                for (int i = 0; i < N; i++)
                {
                    pl[i].Move(k).ShouldBe((int)(((uint)i + k) % (uint)arr.Length));
                    pl[i].Move((BigInteger)k).ShouldBe((int)(((uint)i + k) % (uint)arr.Length));
                    if ((long)k >= 0)
                        pl[i].Move((long)k).ShouldBe((int)(((uint)i + k) % (uint)arr.Length));
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
            var pl = Enumerable.Range(0, arr.Length).Select(i => new PathLoop(arr, i)).ToArray();
            pl[0].Move(1).ShouldBe(-1);
            pl[1].Move(1).ShouldBe(0);
            pl[2].Move(1).ShouldBe(2);
            pl[3].Move(1).ShouldBe(1);
            pl[4].Move(1).ShouldBe(3);
            pl[5].Move(1).ShouldBe(2);
            pl[6].Move(1).ShouldBe(1);

            pl[0].Move(2).ShouldBe(-1);
            pl[1].Move(2).ShouldBe(-1);
            pl[2].Move(2).ShouldBe(2);
            pl[3].Move(2).ShouldBe(0);
            pl[4].Move(2).ShouldBe(1);
            pl[5].Move(2).ShouldBe(2);
            pl[6].Move(2).ShouldBe(0);

            pl[0].Move(3).ShouldBe(-1);
            pl[1].Move(3).ShouldBe(-1);
            pl[2].Move(3).ShouldBe(2);
            pl[3].Move(3).ShouldBe(-1);
            pl[4].Move(3).ShouldBe(0);
            pl[5].Move(3).ShouldBe(2);
            pl[6].Move(3).ShouldBe(-1);

            pl[0].Move(4).ShouldBe(-1);
            pl[1].Move(4).ShouldBe(-1);
            pl[2].Move(4).ShouldBe(2);
            pl[3].Move(4).ShouldBe(-1);
            pl[4].Move(4).ShouldBe(-1);
            pl[5].Move(4).ShouldBe(2);
            pl[6].Move(4).ShouldBe(-1);

            pl[0].Move(5).ShouldBe(-1);
            pl[1].Move(5).ShouldBe(-1);
            pl[2].Move(5).ShouldBe(2);
            pl[3].Move(5).ShouldBe(-1);
            pl[4].Move(5).ShouldBe(-1);
            pl[5].Move(5).ShouldBe(2);
            pl[6].Move(5).ShouldBe(-1);

            pl[0].Move(1L << 20).ShouldBe(-1);
            pl[1].Move(1L << 20).ShouldBe(-1);
            pl[2].Move(1L << 20).ShouldBe(2);
            pl[3].Move(1L << 20).ShouldBe(-1);
            pl[4].Move(1L << 20).ShouldBe(-1);
            pl[5].Move(1L << 20).ShouldBe(2);
            pl[6].Move(1L << 20).ShouldBe(-1);
        }
    }
}
