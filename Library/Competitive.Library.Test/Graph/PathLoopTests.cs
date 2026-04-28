using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Graph;

public class PathLoopTests
{
    [Test, MultipleAssertions]
    public async Task Move()
    {
        const int N = 180;
        var arr = new int[N];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = (i + 1) % arr.Length;
        var rnd = new Random();
        var pl = Enumerable.Range(0, arr.Length).Select(i => new PathLoop(arr, i)).ToArray();

        for (int i = 0; i < N; i++)
            for (int k = 0; k < 20; k++)
                await pl[i].Move(k).Should().BeEqualTo((i + k) % arr.Length);
        for (int t = 0; t < 128; t++)
        {
            ulong k = 0;
            rnd.NextBytes(MemoryMarshal.CreateSpan(ref k, 1).Cast<ulong, byte>());
            for (int i = 0; i < N; i++)
            {
                await pl[i].Move(k).Should().BeEqualTo((int)(((uint)i + k) % (uint)arr.Length));
                await pl[i].Move((BigInteger)k).Should().BeEqualTo((int)(((uint)i + k) % (uint)arr.Length));
                if ((long)k >= 0)
                    await pl[i].Move((long)k).Should().BeEqualTo((int)(((uint)i + k) % (uint)arr.Length));
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Tree()
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
        await pl[0].Move(1).Should().BeEqualTo(-1);
        await pl[1].Move(1).Should().BeEqualTo(0);
        await pl[2].Move(1).Should().BeEqualTo(2);
        await pl[3].Move(1).Should().BeEqualTo(1);
        await pl[4].Move(1).Should().BeEqualTo(3);
        await pl[5].Move(1).Should().BeEqualTo(2);
        await pl[6].Move(1).Should().BeEqualTo(1);

        await pl[0].Move(2).Should().BeEqualTo(-1);
        await pl[1].Move(2).Should().BeEqualTo(-1);
        await pl[2].Move(2).Should().BeEqualTo(2);
        await pl[3].Move(2).Should().BeEqualTo(0);
        await pl[4].Move(2).Should().BeEqualTo(1);
        await pl[5].Move(2).Should().BeEqualTo(2);
        await pl[6].Move(2).Should().BeEqualTo(0);

        await pl[0].Move(3).Should().BeEqualTo(-1);
        await pl[1].Move(3).Should().BeEqualTo(-1);
        await pl[2].Move(3).Should().BeEqualTo(2);
        await pl[3].Move(3).Should().BeEqualTo(-1);
        await pl[4].Move(3).Should().BeEqualTo(0);
        await pl[5].Move(3).Should().BeEqualTo(2);
        await pl[6].Move(3).Should().BeEqualTo(-1);

        await pl[0].Move(4).Should().BeEqualTo(-1);
        await pl[1].Move(4).Should().BeEqualTo(-1);
        await pl[2].Move(4).Should().BeEqualTo(2);
        await pl[3].Move(4).Should().BeEqualTo(-1);
        await pl[4].Move(4).Should().BeEqualTo(-1);
        await pl[5].Move(4).Should().BeEqualTo(2);
        await pl[6].Move(4).Should().BeEqualTo(-1);

        await pl[0].Move(5).Should().BeEqualTo(-1);
        await pl[1].Move(5).Should().BeEqualTo(-1);
        await pl[2].Move(5).Should().BeEqualTo(2);
        await pl[3].Move(5).Should().BeEqualTo(-1);
        await pl[4].Move(5).Should().BeEqualTo(-1);
        await pl[5].Move(5).Should().BeEqualTo(2);
        await pl[6].Move(5).Should().BeEqualTo(-1);

        await pl[0].Move(1L << 20).Should().BeEqualTo(-1);
        await pl[1].Move(1L << 20).Should().BeEqualTo(-1);
        await pl[2].Move(1L << 20).Should().BeEqualTo(2);
        await pl[3].Move(1L << 20).Should().BeEqualTo(-1);
        await pl[4].Move(1L << 20).Should().BeEqualTo(-1);
        await pl[5].Move(1L << 20).Should().BeEqualTo(2);
        await pl[6].Move(1L << 20).Should().BeEqualTo(-1);
    }
}