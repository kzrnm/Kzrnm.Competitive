using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 

namespace AtCoderProject.Hide.無向
{
    class GraphBuilder
    {
        public List<int>[] lists;
        public GraphBuilder(int count)
        {
            this.lists = new List<int>[count];
            for (int i = 0; i < count; i++)
                this.lists[i] = new List<int>();
        }
        public GraphBuilder(int count, IEnumerable<int[]> arrays) : this(count)
        {
            foreach (var item in arrays)
                this.Add(item[0], item[1]);
        }
        public void Add(int from, int to)
        {
            this.lists[from].Add(to);
            this.lists[to].Add(from); // 有向の場合は消す
        }
        public int[][] ToArray()
        {
            var N = this.lists.Length;
            var res = new int[N][];
            for (int i = 0; i < N; i++)
                res[i] = this.lists[i].ToArray();
            return res;
        }
    }

    struct Next
    {
        public int to;
        public int length;
    }
    class LengthGraphBuilder
    {
        public List<Next>[] lists;
        public LengthGraphBuilder(int count)
        {
            this.lists = new List<Next>[count];
            for (int i = 0; i < count; i++)
                this.lists[i] = new List<Next>();
        }
        public LengthGraphBuilder(int count, IEnumerable<int[]> arrays):this(count)
        {
            foreach (var item in arrays)
                this.Add(item[0], item[1], item[2]);
        }
        public void Add(int from, int to, int length)
        {
            this.lists[from].Add(new Next { to = to, length = length });
            this.lists[to].Add(new Next { to = from, length = length }); // 有向の場合は消す
        }
        public Next[][] ToArray()
        {
            var N = this.lists.Length;
            var res = new Next[N][];
            for (int i = 0; i < N; i++)
                res[i] = this.lists[i].ToArray();
            return res;
        }
    }

    class GraphSample
    {
        int[][] graph;
        int[] root;
        void SolveRoot(int val)
        {
            if (root[val] == val)
                SolveRoot(val, graph[val]);
        }
        void SolveRoot(int val, int[] target)
        {
            foreach (var i in target)
            {
                if (root[i] != val)
                {
                    root[i] = val;
                    SolveRoot(val, graph[i]);
                }
            }
        }
    }
}
