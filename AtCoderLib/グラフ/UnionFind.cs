using System;
using System.Collections.Generic;
using System.Linq;
using static AtCoderProject.Global;

class UnionFind
{
    int[] data;
    public UnionFind(int size)
    {
        this.data = NewArray(size, -1);
    }

    public int Root(int x)
    {
        if (data[x] < 0) return x;
        return data[x] = Root(data[x]);
    }
    public int Size(int x) => -data[Root(x)];
    public bool Merge(int x, int y)
    {
        var xRoot = Root(x);
        var yRoot = Root(y);
        if (xRoot == yRoot) return false;
        else if (data[yRoot] < data[xRoot])/*y側の方が多い場合は逆にする*/
            (xRoot, yRoot) = (yRoot, xRoot);
        data[xRoot] += data[yRoot];
        data[yRoot] = xRoot;
        return true;
    }
    public bool IsSameRoot(int x, int y) => Root(x) == Root(y);
    public IEnumerable<int> EnumerateRoots()
        => Enumerable.Range(0, data.Length).Where(i => data[i] < 0);
}
