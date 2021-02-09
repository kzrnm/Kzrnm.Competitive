using Kzrnm.Competitive.IO;

namespace AtCoder
{
    public class WIntGraphBuilder : WGraphBuilder<int, IntOperator>
    {
        public WIntGraphBuilder(int count, bool isDirected) : base(count, isDirected) { }
        public static WIntGraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WIntGraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Int);
            return gb;
        }
        public static WIntGraphBuilder CreateTree(int count, PropertyConsoleReader cr)
        {
            var gb = new WIntGraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Int);
            return gb;
        }
    }
}
