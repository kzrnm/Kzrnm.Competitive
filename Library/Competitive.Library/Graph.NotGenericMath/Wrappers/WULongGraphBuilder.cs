using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE ulong重み付きグラフのBuilder
    public class WULongGraphBuilder : WGraphBuilder<ulong, ULongOperator>
    {
        public WULongGraphBuilder(int count, bool isDirected) : base(count, isDirected) { }
        public static WULongGraphBuilder Create(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WULongGraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.ULong());
            return gb;
        }
        public static WGraphBuilder<ulong, int, ULongOperator> CreateWithEdgeIndex(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WGraphBuilder<ulong, int, ULongOperator>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.ULong(), i);
            return gb;
        }
        public static WULongGraphBuilder CreateTree(int count, ConsoleReader cr)
        {
            var gb = new WULongGraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.ULong());
            return gb;
        }
    }
}
