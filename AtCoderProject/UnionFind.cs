using System;
using System.Collections.Generic;
using System.Linq;
using static Global;

namespace AtCoderProject.Hide
{
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
        public int Count(int x) => -data[Root(x)];
        public void UnionSet(int x, int y)
        {
            var xRoot = Root(x);
            var yRoot = Root(y);
            if (xRoot == yRoot) return;
            else if (data[yRoot] < data[xRoot])//y側の方が多い場合は逆にする
            {
                var tmp = yRoot;
                yRoot = xRoot;
                xRoot = tmp;
            }
            data[xRoot] += data[yRoot];
            data[yRoot] = xRoot;
        }
        public bool IsSameSet(int x, int y) => Root(x) == Root(y);
    }
}
