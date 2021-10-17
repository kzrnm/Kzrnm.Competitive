using AtCoder;
using AtCoder.Operators;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive
{
    public class WLongGraphBuilder : WGraphBuilder<long, LongOperator>
    {
        public WLongGraphBuilder(int count, bool isDirected) : base(count, isDirected) { }
        public static WLongGraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WLongGraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Long);
            return gb;
        }
        public static WGraphBuilder<long, int, LongOperator> CreateWithEdgeIndex(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WGraphBuilder<long, int, LongOperator>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Long, i);
            return gb;
        }
        public static WLongGraphBuilder CreateTree(int count, PropertyConsoleReader cr)
        {
            var gb = new WLongGraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0, cr.Int0, cr.Long);
            return gb;
        }
    }
}
