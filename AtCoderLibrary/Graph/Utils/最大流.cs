using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.Graph
{
    public static class 最大流
    {
        public static MFGraph<T, TOp> ToMFGraph<T, TOp>(this WGraphBuilder<T, TOp> gb)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            var children = gb.children;
            var mfg = new MFGraph<T, TOp>(children.Length);
            for (int i = 0; i < children.Length; i++)
                foreach (var e in children[i].AsSpan())
                    mfg.AddEdge(i, e.To, e.Value);
            return mfg;
        }
    }
}