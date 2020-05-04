using AtCoderProject;
using AtCoderProject.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using static AtCoderProject.Global;

class Grid
{
    /* https://atcoder.jp/contests/abc088/tasks/abc088_d */
#pragma warning disable CS0649
    ConsoleReader cr;


    private object Calc()
    {
        var H = cr.Int;
        var W = cr.Int;
        var grid = cr.Repeat(H).Ascii;
        var bfs = NewArray(H, W, 100000);
        bfs[0][0] = 1;
        var queue = new Queue<(int h, int w)>();
        queue.Enqueue((0, 0));
        while (queue.Count > 0)
        {
            var (h, w) = queue.Dequeue();
            if (h > 0 && grid[h - 1][w] == '.')
                if (bfs[h - 1][w].UpdateMin(bfs[h][w] + 1))
                    queue.Enqueue((h - 1, w));
            if (h + 1 < H && grid[h + 1][w] == '.')
                if (bfs[h + 1][w].UpdateMin(bfs[h][w] + 1))
                    queue.Enqueue((h + 1, w));
            if (w > 0 && grid[h][w - 1] == '.')
                if (bfs[h][w - 1].UpdateMin(bfs[h][w] + 1))
                    queue.Enqueue((h, w - 1));
            if (w + 1 < W && grid[h][w + 1] == '.')
                if (bfs[h][w + 1].UpdateMin(bfs[h][w] + 1))
                    queue.Enqueue((h, w + 1));
        }
        return Math.Max(grid.SelectMany(st => st).Count(c => c == '.') - bfs[H - 1][W - 1], -1);
    }
}
