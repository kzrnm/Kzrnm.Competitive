namespace AtCoder
{
    public static class 最大流
    {
        public static MFGraph<T, TOp> ToMFGraph<T, TOp>(this WGraphBuilder<T, TOp> gb)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            var mfg = new MFGraph<T, TOp>(gb.edgeContainer.Length);
            foreach (var (i, e) in gb.edgeContainer.edges)
                mfg.AddEdge(i, e.To, e.Value);
            return mfg;
        }
    }
}